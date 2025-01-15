using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace GamesLauncher.Utilities
{
    internal static class XAMLUtils
    {
        /// <summary>
        /// An override that adds a range of items to a collection.
        /// </summary>
        /// <param name="collection"></param>
        /// <param name="values"></param>
        public static void Add(this ColumnDefinitionCollection collection, params ColumnDefinition[] values)
        {
            foreach (var value in values)
            {
                collection.Add(value);
            }
        }

        /// <summary>
        /// An override that adds a range of items to a collection.
        /// </summary>
        /// <param name="collection"></param>
        /// <param name="values"></param>
        public static void Add(this UIElementCollection collection, params UIElement[] values)
        {
            foreach (var value in values)
            {
                collection.Add(value);
            }
        }
    }
}
