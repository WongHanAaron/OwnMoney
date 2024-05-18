using Microsoft.Extensions.Logging;
using OwnMoney.Shared.Components.MoneyRepositories;
using OwnMoney.Shared.Components.RepositorySubscription;
using OwnMoney.Shared.Domains.Environment;
using OwnMoney.Shared.Models.Monetary;
using OwnMoney.Shared.Models.Requests;
using OwnMoney.Shared.Models.Responses;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace OwnMoney.Shared.Domains.RepositorySubscription
{
    ///<summary> Responsible for tracking subscriptions and changes that have been made and notifying the subscribers </summary>
    public interface ISubscriptionTracker
    {
        ///<summary> Start tracking a new subscriber </summary>
        Task<SubscriptionResponse> TrackSubscriber(SubscriptionRequest request);

        ///<summary> Stop tracking a subscriber </summary>
        Task UntrackSubscriber(Guid requestId);

        ///<summary> Track these changes for the various subscribers </summary>
        Task TrackChanges(IEnumerable<Entry> upsertEntries, IEnumerable<long> removedEntries);

        ///<summary> Raised when the subscriber needs to be notified of a set of changes </summary>
        EventHandler<SubscriberNotification> SubscriberToBeNotified { get; set; }
    }

    public class SubscriptionTracker : ISubscriptionTracker
    {
        protected readonly ILogger<SubscriptionTracker> _logger;
        protected readonly SubscriptionConfiguration _config;
        protected readonly IDateTimeProvider _dateTimeProvider;
        protected readonly ConcurrentDictionary<Guid, SubscriptionState> _subscribers = new ConcurrentDictionary<Guid, SubscriptionState>();
        protected IEnumerable<IGrouping<GetEntriesQuery, SubscriptionState>> _subscribersByQuery;
        protected readonly Timer _updateTimer;

        public SubscriptionState[] Subscribers
        {
            get
            {
                SubscriptionState[] returned = null;
                lock (this)
                {
                    returned = _subscribers.Values.ToArray();
                }
                return returned;
            }
        }

        public KeyValuePair<GetEntriesQuery, SubscriptionState[]>[] SubscribersByQuery
        {
            get
            {
                KeyValuePair<GetEntriesQuery, SubscriptionState[]>[] returned = null;
                lock (this)
                {
                    returned = _subscribersByQuery.Select(g => new KeyValuePair<GetEntriesQuery, SubscriptionState[]>(g.Key, g.ToArray())).ToArray();
                }
                return returned;
            }
        }

        public SubscriptionTracker(ILogger<SubscriptionTracker> logger,
                                   SubscriptionConfiguration config,
                                   IDateTimeProvider dateTimeProvider)
        {
            _config = config;
            _logger = logger;
            _dateTimeProvider = dateTimeProvider;
            _updateTimer = new Timer(UpdateSubscribersOnTimer, null, 0, _config.UpdateCheckPeriodMs);
        }

        public EventHandler<SubscriberNotification> SubscriberToBeNotified { get; set; }

        public Task TrackChanges(IEnumerable<Entry> upsertEntries, IEnumerable<long> removedEntries)
        {
            var subscribersByQuery = SubscribersByQuery;
            foreach (var subscribers in subscribersByQuery)
            {
                var query = subscribers.Key;

                var matchingEntries = upsertEntries.AsQueryable().Filter(query).Select(e => e.Id);

                if (!matchingEntries.Any() && !removedEntries.Any())
                    continue;

                foreach (var subscriber in subscribers.Value)
                {
                    try
                    {
                        lock (subscriber)
                        {
                            foreach (var matchingEntry in matchingEntries)
                            {
                                subscriber.ChangedEntries.Add(matchingEntry);
                            }

                            foreach (var removedEntry in removedEntries)
                            {
                                subscriber.DeletedEntries.Add(removedEntry);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError("Error in tracking changes for subscriber '{requestId}'. {exception}", subscriber.Request.Id, ex);
                    }
                }
            }
            return Task.CompletedTask;
        }

        public Task<SubscriptionResponse> TrackSubscriber(SubscriptionRequest request)
        {
            _logger.LogInformation("Track new subscriber for '{requestId}'", request.Id);

            var created = Create(request);

            lock (this)
            {
                _subscribers[request.Id] = created;
                UpdateGroupByQuery();
            }

            return Task.FromResult(new SubscriptionResponse() { LifeTimeSeconds = _config.SubscriptionLifeTimeSeconds, RequestId = request.Id });
        }

        public Task UntrackSubscriber(Guid requestId)
        {
            _logger.LogDebug("Untrack subscriber '{requestId}'", requestId);

            lock (this) 
            {
                _subscribers.TryRemove(requestId, out _);
                UpdateGroupByQuery();
            }

            return Task.CompletedTask;
        }

        protected void UpdateGroupByQuery() => _subscribersByQuery = _subscribers.Values.GroupBy(s => s.Request.Query).ToList();
        
        protected SubscriptionState Create(SubscriptionRequest request)
        {
            var returned = new SubscriptionState();
            var currentTime = _dateTimeProvider.Now;
            returned.Request = request;
            returned.LastUpdated = currentTime.AddSeconds(-request.FrequencySeconds);
            returned.ExpiresOn = currentTime.AddSeconds(_config.SubscriptionLifeTimeSeconds);
            return returned;
        }

        private void UpdateSubscribersOnTimer(object timerState) => UpdateSubscribers();

        ///<summary> Check each of the subscribers and if they need to be updated </summary>
        public void UpdateSubscribers()
        {
            try
            {
                var updateTime = _dateTimeProvider.Now;
                var subscribers = Subscribers;
                foreach (var subscriber in subscribers)
                {
                    try
                    {
                        if (!ShouldUpdate(subscriber, updateTime))
                            continue;

                        ChangedContents changes = null;
                        lock (subscriber)
                        {
                            subscriber.LastUpdated = updateTime;
                            changes = subscriber.FlushChanges();
                        }

                        SubscriberToBeNotified?.Invoke(this, new SubscriberNotification()
                        {
                            Changes = changes,
                            RequestId = subscriber.Request.Id
                        });

                        if (ShouldUntrack(subscriber, updateTime))
                            UntrackSubscriber(subscriber.Request.Id);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError("{function} had an error on subscriber {requestId}. {exception}", nameof(UpdateSubscribers), subscriber.Request.Id, ex);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("{function} had an error. {exception}", nameof(UpdateSubscribers), ex);
            }
        }

        protected bool ShouldUpdate(SubscriptionState state, DateTime dateTime)
            => state.LastUpdated.AddSeconds(state.Request.FrequencySeconds) < dateTime;

        protected bool ShouldUntrack(SubscriptionState state, DateTime dateTime)
            => state.ExpiresOn < dateTime;
    }
}
