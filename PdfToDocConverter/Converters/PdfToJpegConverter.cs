using NReco.PdfRenderer;

namespace PdfToDocConverter.Converters;

public class PdfToJpegConverter : IPdfToJpegConverter
{
    public void ConvertPdfToJpeg(string inputPdfPath, string outputImagePath, int resolution = 300)
    {
        var pdfToImg = new PdfToImageConverter();
        var pdfInfo = new PdfInfo();

        int pageCount = pdfInfo.GetPdfInfo(inputPdfPath).Pages;

        for (int i = 1; i <= pageCount; i++) 
        {
            pdfToImg.GenerateImage(inputPdfPath, i, ImageFormat.Jpeg, $"{outputImagePath}{i}.jpg");
        }
    }
}
