using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Threading;
using Caliburn.Micro;
using Castle.Windsor;
using Workbench.ViewModels;
using Workbench.Loggers;

namespace Workbench.Bootstrapper
{
    public class Bootstrapper : BootstrapperBase
    {
        private WindsorContainer container;

        static Bootstrapper()
        {
            LogManager.GetLog = type => new DebugLogger(type);
        }

        /// <summary>
        /// Initialize a new bootstrapper with default values.
        /// </summary>
        public Bootstrapper()
        {
            Initialize();
        }

        /// <summary>
        /// Override to configure the framework and setup your IoC container.
        /// </summary>
        protected override void Configure()
        {
            this.container = ContainerBuilder.Build();
        }

        protected override void OnStartup(object sender, StartupEventArgs e)
        {
            DisplayRootViewFor<IMainWindow>();
        }

        protected override object GetInstance(Type service, string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                return this.container.Resolve(service);
            }
            return this.container.Resolve(key, service);
        }

        protected override IEnumerable<object> GetAllInstances(Type service)
        {
            return this.container.ResolveAll(service).Cast<object>();
        }

        protected override void BuildUp(object instance)
        {
            throw new NotImplementedException("The BuildUp method has not been implemented because Castle Windsor supports property based injection natively.");
         }

        protected override void OnUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            e.Handled = true;
            MessageBox.Show(e.Exception.Message, "An error as occurred", MessageBoxButton.OK);
        }
    }
}
