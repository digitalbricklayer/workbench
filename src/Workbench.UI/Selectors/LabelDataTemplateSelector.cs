using System;
using System.Diagnostics;
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

            Debug.Assert(element != null);

            switch (item)
            {
                case SingletonVariableLabelModel _:
                    return element.FindResource("singletonLabel") as DataTemplate;

                case AggregateVariableLabelModel _:
                    return element.FindResource("compoundLabel") as DataTemplate;

                default:
                    throw new NotImplementedException("Label not implemented.");
            }
        }
    }
}
