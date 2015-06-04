using System.Windows;
using System.Windows.Controls;

namespace DynaApp.Views
{
    /// <summary>
    /// Implements an ListBox for displaying variables in the NetworkView UI.
    /// </summary>
    internal class VariableItemsControl : ListBox
    {
        public VariableItemsControl()
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
        internal VariableItem FindAssociatedVariableItem(object variableDataContext)
        {
            return (VariableItem)this.ItemContainerGenerator.ContainerFromItem(variableDataContext);
        }

        /// <summary>
        /// Creates or identifies the element that is used to display the given item. 
        /// </summary>
        protected override DependencyObject GetContainerForItemOverride()
        {
            return new VariableItem();
        }

        /// <summary>
        /// Determines if the specified item is (or is eligible to be) its own container. 
        /// </summary>
        protected override bool IsItemItsOwnContainerOverride(object item)
        {
            return item is VariableItem;
        }
    }
}
