using ESCPOS_NET.Emitters;

namespace ESCPOS_NET.ConsoleTest
{

    public static partial class Tests
    {
        public static byte[][] TextStyles(ICommandEmitter e) => new byte[][] {
            e.SetStyles(PrintStyle.None),
            e.Print("Default: Одну строку стихотворного произведения называют словом\n"),
            e.SetStyles(PrintStyle.FontB),
            e.Print("Font B: Одну строку стихотворного произведения называют словом.\n"),
            e.SetStyles(PrintStyle.Bold),
            e.Print("Bold: Одну строку стихотворного произведения называют словом.\n"),
            e.SetStyles(PrintStyle.Underline),
            e.Print("Underline: Одну строку стихотворного произведения называют словом.\n"),
            e.SetStyles(PrintStyle.DoubleWidth),
            e.Print("DoubleWidth: Одну строку стихотворного произведения называют словом.\n"),
            e.SetStyles(PrintStyle.DoubleHeight),
            e.Print("DoubleHeight: Одну строку стихотворного произведения называют словом.\n"),
            e.SetStyles(PrintStyle.FontB | PrintStyle.DoubleHeight | PrintStyle.DoubleWidth | PrintStyle.Underline | PrintStyle.Bold),
            e.Print("All Styles: Одну строку стихотворного произведения называют словом.\n"),
            e.SetStyles(PrintStyle.None),
            e.ReverseMode(true),
            e.PrintLine("REVERSE MODE: Одну строку стихотворного произведения называют словом."),
            e.SetStyles(PrintStyle.FontB | PrintStyle.DoubleHeight | PrintStyle.DoubleWidth),
            e.PrintLine("REVERSE MODE: Одну строку стихотворного произведения называют словом."),
            e.SetStyles(PrintStyle.None),
                e.ReverseMode(false),
            e.SetStyles(PrintStyle.None),
            e.RightCharacterSpacing(5),
            e.PrintLine("Right space 5: Одну строку стихотворного произведения называют словом."),
            e.RightCharacterSpacing(0),
            e.SetStyles(PrintStyle.None),
            e.UpsideDownMode(true),
            e.PrintLine("Upside Down Mode: Одну строку стихотворного произведения называют словом."),
            e.UpsideDownMode(false)
        };
    }
}
