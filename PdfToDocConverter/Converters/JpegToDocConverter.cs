using System.Text.RegularExpressions;
using Image = Google.Cloud.Vision.V1.Image;

namespace PdfToDocConverter.Converters;

public class JpegToDocConverter : IJpegToDocConverter
{
    private readonly string _googleCredentialsPath;

    public JpegToDocConverter(string googleCredentialsPath)
    {
        _googleCredentialsPath = googleCredentialsPath;
    }

    public void ConvertJpegToDoc(string inputImagePath, string outputWordPath)
    {
        Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", _googleCredentialsPath);

        var client = ImageAnnotatorClient.Create();
        var image = Image.FromFile(inputImagePath);
        var response = client.DetectDocumentText(image);
        string text = RemoveEnglishTextFromFile(response.Text);

        CreateWordDocument(outputWordPath, text);
    }

    private static void CreateWordDocument(string outputWordPath, string recognizedText)
    {
        var doc = DocX.Create(outputWordPath);
        var paragraph = doc.InsertParagraph(recognizedText);

        paragraph.Font("Times New Roman");
        paragraph.FontSize(14);
        paragraph.Alignment = Alignment.right;

        doc.Save();
    }

    private static string RemoveEnglishTextFromFile(string text)
    {
        string pattern = @"[a-zA-Z\[\]]";

        return Regex.Replace(text, pattern, "");
    }
}
