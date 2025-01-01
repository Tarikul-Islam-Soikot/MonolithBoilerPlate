using PdfSharpCore.Pdf;
using PdfSharpCore.Pdf.IO;
using PdfSharpCore.Pdf.Security;

namespace MonolithBoilerPlate.Service.Helper
{
    public static class PdfHelper
    {
        public static byte[] GeneratePasswordProtectedPdf(this byte[] pdfBytes, string userPassword)
        {
            using MemoryStream inputStream = new(pdfBytes);
            using MemoryStream outputStream = new();
            PdfDocument document = PdfReader.Open(inputStream, PdfDocumentOpenMode.Modify);

            PdfSecuritySettings securitySettings = document.SecuritySettings;
            securitySettings.UserPassword = userPassword;
            securitySettings.OwnerPassword = null;

            #region Optional-Permissions
            //securitySettings.PermitAccessibilityExtractContent = false;
            //securitySettings.PermitAnnotations = false;
            //securitySettings.PermitAssembleDocument = false;
            //securitySettings.PermitExtractContent = false;
            //securitySettings.PermitFormsFill = true;
            //securitySettings.PermitFullQualityPrint = false;
            //securitySettings.PermitModifyDocument = false;
            //securitySettings.PermitPrint = true; 
            #endregion

            document.Save(outputStream, false);
            return outputStream.ToArray();
        }
    }
}
