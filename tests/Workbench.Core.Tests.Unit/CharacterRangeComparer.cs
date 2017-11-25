using System.Collections;

namespace Workbench.Core.Tests.Unit
{
    /// <summary>
    /// A comparer for characters. The NUnit InRange constraint doesn't work for 
    /// character ranges like 'a'..'z'.
    /// </summary>
    internal class CharacterRangeComparer : IComparer
    {
        public int Compare(object x, object y)
        {
            var charX = (char)x;
            var charY = (char)y;
            return charX.CompareTo(charY);
        }
    }
}
