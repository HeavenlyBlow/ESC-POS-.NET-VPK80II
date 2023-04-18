using System;
using Custom.CuCustomWndAPI;

namespace CustomWinAPI
{
    class Program
    {
        static void Main(string[] args)
        {
            var customWndAPIWrap = new CuCustomWndAPIWrap(CuCustomWndAPIWrap.CcwLogVerbosity.CCW_LOG_DEEP_DEBUG, null);
            customWndAPIWrap.InitLibrary();
            var comport = customWndAPIWrap.EnumCOMPorts();
            CuCustomWndDevice dev;
            dev.PrintBarcode(strBrcText: "tesx", new PrintBarcodeSettings());
        }
    }
}