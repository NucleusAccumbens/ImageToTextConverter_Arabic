namespace PdfToDocConverter.Interfaces;

public interface IJpegToDocConverter
{
    void ConvertJpegToDoc(string inputImagePath, string outputWordPath);
}
