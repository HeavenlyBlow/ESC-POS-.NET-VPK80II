using ESCPOS_NET.Emitters;

namespace ESCPOS_NET.ConsoleTest
{
    public static partial class Tests
    {
        public static byte[][] LineSpacing(ICommandEmitter e) => new byte[][] {
            e.SetLineSpacingInDots(200),
            e.PrintLine("Это дефолтный отступ."),
            e.SetLineSpacingInDots(15),
            e.PrintLine("Это имеет 200 точек интервала выше."),
            e.ResetLineSpacing(),
            e.PrintLine("Это имеет 15 точек интервала выше."),
            e.PrintLine("Это дефолтный отступ.")
        };
    }
}
