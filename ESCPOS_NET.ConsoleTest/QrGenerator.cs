using System;
using System.Drawing;
using QRCoder;
using Color = System.Drawing.Color;
using Image = System.Drawing.Image;
using Size = System.Drawing.Size;

namespace ESCPOS_NET.ConsoleTest
{
    public static class QrGenerator
    {
        public static byte[] Generated(string encode)
        {

            var qrCode = QrCode(encode);
            var mearge = Meagre(AdjustmentRectangle(), qrCode);
            mearge.Save("testimage.png");
            ImageConverter converter = new ImageConverter();
            return (byte[])converter.ConvertTo(mearge, typeof(byte[]));
        }


        private static Bitmap QrCode(string encode)
        {
            QRCodeGenerator qrGenerator = new QRCodeGenerator();
            QRCodeData qrCodeData = qrGenerator.CreateQrCode("fasdfasdfasdf", QRCodeGenerator.ECCLevel.Q);
            QRCode qrCode = new QRCode(qrCodeData);
            Bitmap qrCodeImage = qrCode.GetGraphic(20);
            return new Bitmap(qrCodeImage, new Size(200, 200));
            
        }
        // Bitmap resizeQR = new Bitmap(qrCodeImage, new Size(550, 550));

        private static Bitmap AdjustmentRectangle()
        {
            Bitmap rectangle = new Bitmap(180,200);
            // for (var x = 0; x < rectangle.Width; x++)
            // {
            //     for (var y = 0; y < rectangle.Height; y++)
            //     {
            //         rectangle.SetPixel(x, y, Color.White);
            //     }
            // }
            return rectangle;
        }

        private static Bitmap Meagre(Image image1, Image image2)
        {
            Bitmap bitmap = new Bitmap(image1.Width + image2.Width, Math.Max(image1.Height, image2.Height));
            using (Graphics g = Graphics.FromImage(bitmap))
            {
                g.DrawImage(image1, 0, 0);
                g.DrawImage(image2, image1.Width, 0);
            }
    
            return bitmap;
        }
    }
}