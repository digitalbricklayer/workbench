using System;
using System.Diagnostics.Contracts;
using System.Windows;
using System.Windows.Controls;
using Workbench.Core.Models;

namespace Workbench.Selectors
{
    public class LabelDataTemplateSelector : DataTemplateSelector
    {
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            var element = container as FrameworkElement;

            Contract.Assert(element != null);

            switch (item)
            {
                case SingletonLabelModel _:
                    return element.FindResource("singletonLabel") as DataTemplate;

                case AggregateLabelModel _:
                    return element.FindResource("compoundLabel") as DataTemplate;

                default:
                    throw new NotImplementedException("Label not implemented.");
            }
        }
    }
}
