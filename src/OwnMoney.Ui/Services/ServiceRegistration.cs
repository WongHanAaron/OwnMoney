using OwnMoney.Ui.Services.Environment;
using OwnMoney.Ui.Views.Dashboard;
using OwnMoney.Ui.Views.Transactions;
using Prism.Ioc;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OwnMoney.Ui.Services
{
    public static class ServiceRegistration
    {
        public static void AddOwnMoneyViews(this IContainerRegistry registry)
        {
            registry.RegisterGlobalNavigationObserver();
            registry.RegisterForNavigation<DashboardPage, DashboardViewModel>();
            registry.RegisterForNavigation<TransactionsPage, TransactionsViewModel>();
        }

        public static void AddOwnMoneyUi(this IServiceCollection collection)
        {
            collection.AddSingleton<IWindowSizeListener, WindowSizeListener>();
        }
    }

    public class ContainerRegistryAdapter : IServiceCollection
    {
        IContainerRegistry _registry;
        public ContainerRegistryAdapter(IContainerRegistry registry)
        {
            _registry = registry;
        }

        public ServiceDescriptor this[int index] { get => null; set { } }

        public int Count => 0;

        public bool IsReadOnly => false;

        public void Add(ServiceDescriptor item)
        {
            switch (item.Lifetime)
            {
                case ServiceLifetime.Singleton:
                    AddByType(item, (inter, impl) => _registry.RegisterSingleton(inter, impl),
                                    (inter, inst) => _registry.RegisterInstance(inter, inst),
                                    (inter, fac) => _registry.RegisterSingleton(inter, fac));
                    return;
                case ServiceLifetime.Transient:
                case ServiceLifetime.Scoped:
                    AddByType(item, (inter, impl) => _registry.RegisterScoped(inter, impl),
                                    (inter, inst) => _registry.RegisterInstance(inter, inst),
                                    (inter, fac) => _registry.RegisterScoped(inter, fac));
                    return;
            }
            throw new NotImplementedException();
        }

        public void AddByType(ServiceDescriptor item, Action<Type, Type> byImplementation, Action<Type, object> byInstance, Action<Type, Func<IContainerProvider, object>> byFactory)
        {
            if (item.ImplementationInstance != null)
            {
                byInstance(item.ServiceType, item.ImplementationInstance);
            }
            else if (item.ImplementationFactory != null)
            {
                byFactory(item.ServiceType, c => item.ImplementationFactory(new ContainerProviderAdapter(c)));
            }
            else if (item.ImplementationType != null)
            {
                byImplementation(item.ServiceType, item.ImplementationType);
            }
        }

        public class ContainerProviderAdapter : IServiceProvider
        {
            protected readonly IContainerProvider _provider;

            public ContainerProviderAdapter(IContainerProvider provider)
            {
                _provider = provider;
            }

            public object? GetService(Type serviceType) => _provider.Resolve(serviceType);
        }

        public void Clear() { }

        public bool Contains(ServiceDescriptor item) => false;

        public void CopyTo(ServiceDescriptor[] array, int arrayIndex) { }

        public IEnumerator<ServiceDescriptor> GetEnumerator()
        {
            return new List<ServiceDescriptor>().GetEnumerator();
        }

        public int IndexOf(ServiceDescriptor item) => -1;

        public void Insert(int index, ServiceDescriptor item) { }

        public bool Remove(ServiceDescriptor item) => false;

        public void RemoveAt(int index) { }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return new List<ServiceDescriptor>().GetEnumerator();
        }
    }
}
