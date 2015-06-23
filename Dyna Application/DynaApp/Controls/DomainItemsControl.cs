using System.Windows;
using System.Windows.Controls;
using DynaApp.Views;

namespace DynaApp.Controls
{
    /// <summary>
    /// Implements a ListBox for displaying domains in the ModelView UI.
    /// </summary>
    internal class DomainItemsControl : ListBox
    {
        public DomainItemsControl()
        {
            //
            // By default, we don't want this UI element to be focusable.
            //
            Focusable = false;
        }

        /// <summary>
        /// Find the VariableItem UI element that has the specified data context.
        /// Return null if no such VariableItem exists.
        /// </summary>
        internal DomainItem FindAssociatedDomainItem(object domainDataContext)
        {
            return (DomainItem)this.ItemContainerGenerator.ContainerFromItem(domainDataContext);
        }

        /// <summary>
        /// Creates or identifies the element that is used to display the given item. 
        /// </summary>
        protected override DependencyObject GetContainerForItemOverride()
        {
            return new DomainItem();
        }

        /// <summary>
        /// Determines if the specified item is (or is eligible to be) its own container. 
        /// </summary>
        protected override bool IsItemItsOwnContainerOverride(object item)
        {
            return item is DomainItem;
        }
    }
}
