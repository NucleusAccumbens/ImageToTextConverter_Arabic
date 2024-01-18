using System;
using System.IO;
using System.Linq;
using System.Windows;
using Microsoft.Win32;
using PdfToDocConverter.Converters;
using PdfToDocConverter.Interfaces;

namespace PdfToDocConverterApp
{
    public partial class MainWindow : Window
    {      
        private IPdfToJpegConverter pdfToJpegConverter;
        private IJpegToDocConverter jpegToDocConverter;
        private string outputFolderPath;
        private string selectedPdfFilePath;

        public MainWindow()
        {
            InitializeComponent();

            pdfToJpegConverter = new PdfToJpegConverter(); 
            jpegToDocConverter= new JpegToDocConverter($"{Directory.GetCurrentDirectory()}\\velvety-outcome-397916-22a7bc6bc1c9.json");
        }

        private void SelectPdfButton_Click(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new OpenFileDialog
            {
                Filter = "PDF файлы (*.pdf)|*.pdf",
                Multiselect = false
            };

            if (openFileDialog.ShowDialog() == true)
            {
                selectedPdfFilePath = openFileDialog.FileName;
            }
        }

        private void SelectOutputFolderButton_Click(object sender, RoutedEventArgs e)
        {
            var folderDialog = new System.Windows.Forms.FolderBrowserDialog();

            if (folderDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                outputFolderPath = folderDialog.SelectedPath;
                OutputFolderTextBox.Text = outputFolderPath;
            }
        }

        private void ConvertButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(selectedPdfFilePath) || string.IsNullOrEmpty(outputFolderPath))
            {
                MessageBox.Show("Select the PDF file and the folder to save.");
                return;
            }

            try
            {
                var directory = Directory.CreateDirectory(@"temp/");
                string jpegTempPath = directory.FullName;
                string outputDocPath = Path.Combine(outputFolderPath, "page_№.doc");

                pdfToJpegConverter.ConvertPdfToJpeg(selectedPdfFilePath, jpegTempPath);

                string[] imgPasses = Directory.GetFiles(jpegTempPath).OrderBy(f => ExtractNumberFromFileName(f)).ToArray();

                for (int i = 0; i < imgPasses.Length; i++)
                {
                     jpegToDocConverter
                        .ConvertJpegToDoc(imgPasses[i], outputFolderPath + $"\\page_{i + 1}.doc");

                    File.Delete(imgPasses[i]);
                }

                MessageBox.Show("The conversion is completed. The document is saved on the way: " + outputDocPath);
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred during conversion: " + ex.Message);
            }
        }

        static int ExtractNumberFromFileName(string fileName)
        {
            // Извлекаем все цифры из имени файла и преобразуем их в число
            string number = new string(fileName.Where(char.IsDigit).ToArray());
            return int.TryParse(number, out int result) ? result : 0;
        }
    }
}
