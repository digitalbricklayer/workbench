using System.Windows;
using System.Windows.Controls;

namespace DynaApp.Controls
{
    /// <summary>
    /// Implements a ListBox for displaying constraints in the ModelView UI.
    /// </summary>
    internal class GraphicItemsControl : ListBox
    {
        public GraphicItemsControl()
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
        internal GraphicItem FindAssociatedVariableItem(object variableDataContext)
        {
            return (GraphicItem)this.ItemContainerGenerator.ContainerFromItem(variableDataContext);
        }

        /// <summary>
        /// Creates or identifies the element that is used to display the given item. 
        /// </summary>
        protected override DependencyObject GetContainerForItemOverride()
        {
            return new GraphicItem();
        }

        /// <summary>
        /// Determines if the specified item is (or is eligible to be) its own container. 
        /// </summary>
        protected override bool IsItemItsOwnContainerOverride(object item)
        {
            return item is GraphicItem;
        }
    }
}
