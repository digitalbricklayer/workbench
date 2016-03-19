using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Windows;

namespace Workbench.Core
{
    static class BoardConvert
    {
        static readonly Dictionary<int, Point> lookupTable = new Dictionary<int, Point>
        {
            [1] = new Point(1,1),
            [2] = new Point(1,2),
            [3] = new Point(1, 3),
            [4] = new Point(1, 4),
            [5] = new Point(1, 5),
            [6] = new Point(1, 6),
            [7] = new Point(1, 7),
            [8] = new Point(1, 8),
            [9] = new Point(2, 1),
            [10] = new Point(2, 2),
            [11] = new Point(2, 3),
            [12] = new Point(2, 4),
            [13] = new Point(2, 5),
            [14] = new Point(2, 6),
            [15] = new Point(2, 7),
            [16] = new Point(2, 8),
            [17] = new Point(3, 1),
            [18] = new Point(3, 2),
            [19] = new Point(3, 3),
            [20] = new Point(3, 4),
            [21] = new Point(3, 5),
            [22] = new Point(3, 6),
            [23] = new Point(3, 7),
            [24] = new Point(3, 8),
            [25] = new Point(4, 1),
            [26] = new Point(4, 2),
            [27] = new Point(4, 3),
            [28] = new Point(4, 4),
            [29] = new Point(4, 5),
            [30] = new Point(4, 6),
            [31] = new Point(4, 7),
            [32] = new Point(4, 8),
            [33] = new Point(5, 1),
            [34] = new Point(5, 2),
            [35] = new Point(5, 3),
            [36] = new Point(5, 4),
            [37] = new Point(5, 5),
            [38] = new Point(5, 6),
            [39] = new Point(5, 7),
            [40] = new Point(5, 8),
            [41] = new Point(6, 1),
            [42] = new Point(6, 2),
            [43] = new Point(6, 3),
            [44] = new Point(6, 4),
            [45] = new Point(6, 5),
            [46] = new Point(6, 6),
            [47] = new Point(6, 7),
            [48] = new Point(6, 8),
            [49] = new Point(7, 1),
            [50] = new Point(7, 2),
            [51] = new Point(7, 3),
            [52] = new Point(7, 4),
            [53] = new Point(7, 5),
            [54] = new Point(7, 6),
            [55] = new Point(7, 7),
            [56] = new Point(7, 8),
            [57] = new Point(8, 1),
            [58] = new Point(8, 2),
            [59] = new Point(8, 3),
            [60] = new Point(8, 4),
            [61] = new Point(8, 5),
            [62] = new Point(8, 6),
            [63] = new Point(8, 7),
            [64] = new Point(8, 8),
        };
         
        public static Point ToPoint(int counter)
        {
            Contract.Requires<ArgumentOutOfRangeException>(counter > 0 && counter <= 64);
            return lookupTable[counter];
        }
    }
}
