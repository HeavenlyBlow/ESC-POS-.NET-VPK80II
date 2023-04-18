using System.Linq;
using ESCPOS_NET.Emitters.BaseCommandValues;
using SixLabors.ImageSharp.Processing;

namespace ESCPOS_NET.Emitters
{
    public abstract partial class BaseCommandEmitter : ICommandEmitter
    {
        /* Action Commands */
        public virtual byte[] FullCut() => new byte[] { Cmd.GS, Ops.PaperCut, Functions.PaperCutFullCut };

        public virtual byte[] PartialCut() => new byte[] { Cmd.GS, Ops.PaperCut, Functions.PaperCutPartialCut };

        public virtual byte[] FullCutAfterFeed(int lineCount) => new byte[] { Cmd.GS, Ops.PaperCut, Functions.PaperCutFullCutWithFeed, (byte)lineCount };

        public virtual byte[] PartialCutAfterFeed(int lineCount) => new byte[] { Cmd.GS, Ops.PaperCut, Functions.PaperCutPartialCutWithFeed, (byte)lineCount };
        
        public virtual byte[] EjectPaperAfterCut() => new byte[] {Cmd.GS, Functions.EjectPaper, (byte)0x05};
        public virtual byte[] PresentPaper() => new[] { Cmd.GS, Functions.EjectPaper, (byte)0x03, (byte)0x01 };

        // public virtual byte[][] PrintQR(string str)
        // {
        //     var strLen = str.Length + 3;
        //     byte strPl = (byte)(strLen % 256);
        //     byte strPh = (byte)(strLen / 256);
        //     return new byte[][]
        //     {
        //         new[]
        //         {
        //             (byte)0x1D, (byte)0x28, (byte)0x6b, (byte)0x04, (byte)0x00, (byte)0x31, (byte)0x41, (byte)0x32,
        //             (byte)0x00
        //         },
        //         new[]
        //         {
        //             (byte)0x1D, (byte)0x28, (byte)0x6b, (byte)0x03, (byte)0x00, (byte)0x31, (byte)0x43, (byte)0x03
        //         },
        //         new[]
        //         {
        //             (byte)0x1D, (byte)0x28, (byte)0x6b, (byte)0x03, (byte)0x00, (byte)0x31, (byte)0x45, (byte)0x31
        //         },
        //         new[] { (byte)0x1D, (byte)0x28, (byte)0x6b, strPl, strPh, (byte)0x31, (byte)0x50, (byte)0x30 },
        //         StringEncode(str),
        //         new[]{(byte)0x1D, (byte)0x28, (byte)0x6b, (byte)0x03, (byte)0x00, (byte)0x31, (byte)0x51, (byte)0x30}
        //     };
        // }

        public virtual byte[] ModelQr() => new byte[] { 0x1D, 0x28, 0x6b, 0x04, 0x00, 0x31, 0x41, 0x32, 0x00 };
        public virtual byte[] SizeQr() => new byte[] { 0x1D, 0x28, 0x6b, 0x03, 0x00, 0x31, 0x43, 0x03 };
        public virtual byte[] ErrorQr() => new byte[] { 0x1D, 0x28, 0x6b, 0x03, 0x00, 0x31, 0x45, 0x31 };
        public virtual byte[] StoreQR(byte storePL, byte storePH) => new byte[] {0x1d, 0x28, 0x6b, storePL, storePH, 0x31, 0x50, 0x30};
        public virtual byte[] PrintQr() => new byte[] {0x1D, 0x28, 0x6b, 0x03, 0x00, 0x31, 0x51, 0x30};
        
        private static byte[] StringEncode(string str)
        {
            return str.ToCharArray().Select(x => (byte)x).ToArray();
        }
    }
}
