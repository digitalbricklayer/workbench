using System;
using System.Windows;
using System.Windows.Controls;
using Workbench.Core.Models;

namespace Workbench.Views
{
    public class LabelDataTemplateSelector : DataTemplateSelector
    {
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            var element = container as FrameworkElement;

            switch (item)
            {
                case SingletonLabelModel _:
                    return element.FindResource("singletonLabel") as DataTemplate;

                case CompoundLabelModel _:
                    return element.FindResource("compoundLabel") as DataTemplate;

                default:
                    throw new NotImplementedException("Unknown label type.");
            }
        }
    }
}
