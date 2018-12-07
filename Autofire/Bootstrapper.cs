using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Threading;
using Autofire.Core.Features;
using Autofire.Core.UI.ViewModels;
using Caliburn.Micro;

namespace Autofire
{
    public class Bootstrapper : BootstrapperBase
    {
        private SimpleContainer container;

        public Bootstrapper()
        {
            Initialize();
        }

        protected override void Configure()
        {
            container = new SimpleContainer();

            container.Instance(container);

            container
                .Singleton<IWindowManager, WindowManager>()
                .Singleton<IEventAggregator, EventAggregator>();
            ;

            container
                .PerRequest<ShellViewModel>()
                .PerRequest<DataManagerViewModel>()
                .PerRequest<GameViewModel>();

            ServicesInitialize();
        }

        protected override void OnStartup(object sender, StartupEventArgs e)
        {
            DisplayRootViewFor<ShellViewModel>();
        }

        protected override object GetInstance(Type service, string key)
        {
            return container.GetInstance(service, key);
        }

        protected override IEnumerable<object> GetAllInstances(Type service)
        {
            return container.GetAllInstances(service);
        }

        protected override void BuildUp(object instance)
        {
            container.BuildUp(instance);
        }

        protected override void OnUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            e.Handled = true;
            MessageBox.Show(e.Exception.Message, "An error as occurred", MessageBoxButton.OK);
        }

        protected override void OnExit(object sender, EventArgs e)
        {
            ServicesTermination();
        }

        private void ServicesInitialize()
        {
            var core = new Core.Core(container.GetInstance<IEventAggregator>());

            core.Start();

            container
                .RegisterInstance(typeof(IProfileManager), nameof(IProfileManager), core.Data);

            container
                .RegisterInstance(typeof(IGameManager), nameof(IGameManager), core.Game);
        }

        private void ServicesTermination()
        {
        }
    }
}