using System;
using System.Collections.Generic;
using System.Windows;
using Caliburn.Micro;
using DynaApp.Services;
using DynaApp.ViewModels;

namespace DynaApp
{
    public class Bootstrapper : BootstrapperBase
    {
        private SimpleContainer container;

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
            container = new SimpleContainer();

            container.Singleton<IWindowManager, WindowManager>();
            container.Singleton<IEventAggregator, EventAggregator>();
            container.Singleton<DataService>();
            container.PerRequest<MainWindowViewModel>();
            container.PerRequest<WorkspaceViewModel>();
            container.PerRequest<ConstraintViewModel>();
            container.PerRequest<DomainViewModel>();
            container.PerRequest<VariableViewModel>();
            container.PerRequest<AggregateVariableViewModel>();
            container.PerRequest<AggregateResizeViewModel>();
            container.PerRequest<ConstraintExpressionViewModel>();
            container.PerRequest<DomainExpressionViewModel>();
            container.PerRequest<ModelErrorsViewModel>();
            container.PerRequest<ModelErrorViewModel>();
            container.PerRequest<ModelViewModel>();
            container.PerRequest<SolutionViewModel>();
            container.PerRequest<VariableDomainExpressionViewModel>();
            container.PerRequest<ValueViewModel>();
        }

        /// <summary>
        /// Override this to provide an IoC specific implementation.
        /// </summary>
        /// <param name="service">The service to locate.</param>
        /// <param name="key">The key to locate.</param>
        /// <returns>
        /// The located service.
        /// </returns>
        protected override object GetInstance(Type service, string key)
        {
            var instance = container.GetInstance(service, key);
            if (instance != null)
                return instance;

            throw new Exception("Could not locate any instances.");
        }

        /// <summary>
        /// Override this to provide an IoC specific implementation
        /// </summary>
        /// <param name="service">The service to locate.</param>
        /// <returns>
        /// The located services.
        /// </returns>
        protected override IEnumerable<object> GetAllInstances(Type service)
        {
            return container.GetAllInstances(service);
        }

        /// <summary>
        /// Override this to provide an IoC specific implementation.
        /// </summary>
        /// <param name="instance">The instance to perform injection on.</param>
        protected override void BuildUp(object instance)
        {
            container.BuildUp(instance);
        }

        /// <summary>
        /// Override this to add custom behavior to execute after the application starts.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The args.</param>
        protected override void OnStartup(object sender, StartupEventArgs e)
        {
            DisplayRootViewFor<MainWindowViewModel>();
        }
    }
}
