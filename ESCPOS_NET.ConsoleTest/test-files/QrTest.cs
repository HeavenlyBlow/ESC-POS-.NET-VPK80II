using System;
using System.Collections.Generic;
using System.Text;

namespace ESCPOS_NET.ConsoleTest.test_files
{
    public class QrTest
    {
        // private static byte[] Size(QrCodeSize size)
        // {
        //     return new byte[] { 29, 40, 107, 3, 0, 49, 67, 4 };
        // }
        //
        //     private IEnumerable<byte> ModelQr()
        //     {
        //         return new byte[] { 29, 40, 107, 3, 0, 49, 65, 0 };
        //     }
        //
        //     private IEnumerable<byte> ErrorQr()
        //     {
        //         return new byte[] { 29, 40, 107, 3, 0, 49, 69, 0 };
        //     }
        //
        //     private static IEnumerable<byte> StoreQr(string qrData)
        //     {
        //         var length = qrData.Length + 3;
        //         var b = (byte)(length % 256);
        //         var b2 = (byte)(length / 256);
        //
        //         return new byte[] { 29, 40, 107 }
        //             .AddBytes(new[] { b })
        //             .AddBytes(new[] { b2 })
        //             .AddBytes(new byte[] { 49, 80, 49 });
        //     }
        //
        //     private IEnumerable<byte> PrintQr()
        //     {
        //         return new byte[] { 29, 40, 107, 3, 0, 49, 81, 49 };
        //     }
        //
        //     public byte[] Print(string qrData)
        //     {
        //         return Print(qrData, QrCodeSize.Size0);
        //     }
        //
        //     public byte[] Print(string qrData, QrCodeSize qrCodeSize)
        //     {
        //         var list = new List<byte>();
        //         list.AddRange(ModelQr());
        //         list.AddRange(Size(qrCodeSize));
        //         list.AddRange(ErrorQr());
        //         list.AddRange(StoreQr(qrData));
        //         list.AddRange(Encoding.UTF8.GetBytes(qrData));
        //         list.AddRange(PrintQr());
        //         return list.ToArray();
        //     }
        
        //Работает
        private static byte[] Size(QrCodeSize size)
        {
            return new byte[] { 29, 40, 107, 3, 0, 49, 67, 2 };
        }
        
        //Работает
        private IEnumerable<byte> ModelQr()
        {
            return new byte[] { 29, 40, 107, 3, 0, 49, 69, 0 };
        }
        
        
        private IEnumerable<byte> ErrorQr()
        {
            return new byte[] { 29, 40, 107, 3, 0, 49, 69, 2 };
        }

        private static IEnumerable<byte> StoreQr(string qrData)
        {
            var length = qrData.Length + 3;
            var b = (byte)(length % 256);
            var b2 = (byte)(length / 256);

            //TODO узнать что такое пл пх в этом моменте задается данные для кодирования 
            return new byte[] { 29, 40, 107 }
                .AddBytes(new[] { b })
                .AddBytes(new[] { b2 })
                .AddBytes(new byte[] { 49, 80, 49 });
        }

        private IEnumerable<byte> PrintQr()
        {
            return new byte[] { 29, 40, 107, 3, 0, 49, 81, 49 };
        }

        public byte[] Print(string qrData)
        {
            return Print(qrData, QrCodeSize.Size0);
        }

        public byte[] Print(string qrData, QrCodeSize qrCodeSize)
        {
            var list = new List<byte>();
            list.AddRange(ModelQr());
            list.AddRange(Size(qrCodeSize));
            list.AddRange(ErrorQr());
            list.AddRange(StoreQr(qrData));
            // list.AddRange(Encoding.UTF8.GetBytes(qrData));
            list.AddRange(PrintQr());
            return list.ToArray();
        }
    }
    public enum QrCodeSize
    {
        Size0,
        Size1,
        Size2
    }
    }
