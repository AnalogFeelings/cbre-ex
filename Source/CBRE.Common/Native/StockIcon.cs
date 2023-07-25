using System;
using System.Runtime.InteropServices;

namespace CBRE.Common.Native
{
    public class StockIcon
    {
        [DllImport("Shell32.dll", SetLastError = false)]
        public static extern Int32 SHGetStockIconInfo(StockIconId iconId, StockIconFlags iconFlags, ref StockIconInfo iconInfo);
    }
}
