using System.Drawing.Imaging;
using System.Drawing;
using QRCoder;

namespace MonolithBoilerPlate.Service.Helper
{
    public static class QrCodeGeneratorHelper
    {
        public static string GenerateQrCode(this string text)
        {
            using QRCodeGenerator qrGenerator = new QRCodeGenerator();
            QRCodeData qrCodeData = qrGenerator.CreateQrCode(text, QRCodeGenerator.ECCLevel.Q);

            using QRCode qrCode = new QRCode(qrCodeData);
            using Bitmap qrCodeImage = qrCode.GetGraphic(20);
            using MemoryStream ms = new MemoryStream();
            qrCodeImage.Save(ms, ImageFormat.Png);
            byte[] imageBytes = ms.ToArray();
            return Convert.ToBase64String(imageBytes);
        }
    }
}
