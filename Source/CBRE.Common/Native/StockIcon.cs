using System;
using System.Runtime.InteropServices;

namespace CBRE.Common.Native
{
    public class StockIcon
    {
        [DllImport("shell32.dll", SetLastError = false)]
        public static extern int SHGetStockIconInfo(StockIconId iconId, StockIconFlags iconFlags, ref StockIconInfo iconInfo);
    }
}
