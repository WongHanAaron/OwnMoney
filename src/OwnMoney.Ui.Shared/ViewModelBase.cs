using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace OwnMoney.Ui.Shared
{
    public abstract class ViewModelBase : BindableBase, IPageLifecycleAware, IInitialize, INavigationAware, IDestructible
    {
        private readonly BaseServices _baseServices;

        protected ViewModelBase(BaseServices baseServices)
        {
            _baseServices = baseServices;
        }

        protected INavigationService NavigationService => _baseServices.NavigationService;

        protected IRegionManager RegionManager => _baseServices.RegionManager;

        protected IPageDialogService PageDialogs => _baseServices.PageDialogs;

        protected IEventAggregator EventAggregator => _baseServices.EventAggregator;

        protected Dictionary<string, object> _values = new Dictionary<string, object>();

        protected TValue GetValue<TValue>([CallerMemberName] string propertyName = null)
            => _values.TryGetValue(propertyName, out var val) && val is TValue ? (TValue)val : default(TValue);

        protected bool SetValue<TValue>(TValue value, [CallerMemberName] string propertyName = null)
        {
            var currentValue = GetValue<TValue>(propertyName);

            if (EqualityComparer<TValue>.Default.Equals(currentValue, value)) return false;

            _values[propertyName] = value;
            RaisePropertyChanged(propertyName);

            return true;
        }

        public string? Title { get => GetValue<string>(); set => SetValue<string>(value); }

        public bool IsBusy { get => GetValue<bool>(); set => SetValue<bool>(value); }

        protected virtual void OnAppearing()
        {
        }

        protected virtual void OnDisappearing()
        {
        }

        protected virtual void Initialize(INavigationParameters parameters)
        {
        }

        protected virtual void OnNavigatedFrom(INavigationParameters parameters)
        {
        }

        protected virtual void OnNavigatedTo(INavigationParameters parameters)
        {
        }

        protected virtual void Destroy()
        {
        }

        void IPageLifecycleAware.OnAppearing()
        {
            OnAppearing();
        }

        void IPageLifecycleAware.OnDisappearing()
        {
            OnDisappearing();
        }

        void IInitialize.Initialize(INavigationParameters parameters)
        {
            if (parameters.TryGetValue<string>("Title", out var title))
                Title = title;

            Initialize(parameters);
        }

        void INavigatedAware.OnNavigatedFrom(INavigationParameters parameters)
        {
            OnNavigatedFrom(parameters);
        }

        void INavigatedAware.OnNavigatedTo(INavigationParameters parameters)
        {
            OnNavigatedTo(parameters);
        }

        void IDestructible.Destroy()
        {
            Destroy();
        }
    }
}
