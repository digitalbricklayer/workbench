using System.Windows;
using System.Windows.Controls;

namespace Workbench.Controls
{
    /// <summary>
    /// Implements a ListBox for displaying graphics in the UI.
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
        /// Find the graphic item UI element that has the specified data context.
        /// Return null if no such graphic item exists.
        /// </summary>
        internal GraphicItem FindAssociatedGraphicItem(object graphicDataContext)
        {
            return (GraphicItem)ItemContainerGenerator.ContainerFromItem(graphicDataContext);
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
