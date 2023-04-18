using ESCPOS_NET.Emitters;

namespace ESCPOS_NET.ConsoleTest
{
    public static partial class Tests
    {
        public static byte[][] MultiLinePrinting(ICommandEmitter e) => new byte[][] {
            //TODO: sanitize test.
            e.PrintLine("Русский текст печатается"),
            e.FeedDots(250),
            e.PrintLine("Feeding 3 lines."),
            e.FeedLines(3),
            e.PrintLine("Done Feeding."),
            e.PrintLine("Reverse Feeding 6 lines."),
            e.FeedLinesReverse(6),
            e.PrintLine("Done Reverse Feeding."),
            e.FullCut()
        };

        public static byte[] SingleLinePrinting(ICommandEmitter e) => new byte[] {
            10,04,04
            // e.Print("Single Test Line Of Text\r\n"),
        };
    }
}
