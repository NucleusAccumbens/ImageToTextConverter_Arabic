namespace PdfToDocConverter.Interfaces;

public interface IPdfToJpegConverter
{
    void ConvertPdfToJpeg(string inputPdfPath, string outputImagePath, int resolution = 300);
}
