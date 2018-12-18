using FSBeheer.Properties;
using GalaSoft.MvvmLight.Command;
using PdfSharp.Drawing;
using PdfSharp.Pdf;
using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Windows;

namespace FSBeheer.ViewModel
{
    public class TestPDFViewModel
    {
        private CustomFSContext _myContext;

        public RelayCommand CreatePDFCommand { get; set; }

        public RelayCommand<Window> CloseCommand { get; set; }

        public TestPDFViewModel()
        {
            _myContext = new CustomFSContext();

            CreatePDFCommand = new RelayCommand(CreatePDF);
            CloseCommand = new RelayCommand<Window>(Close);
        }

        private void Close(Window window)
        {
            window.Close();
        }

        private void CreatePDF()
        {
            // Create a new PDF document
            PdfDocument document = new PdfDocument();
            document.Info.Title = "Created with PDFsharp";

            // Create an empty page
            PdfPage page1 = document.AddPage();
            PdfPage page2 = document.AddPage();

            // Get an XGraphics object for drawing
            XGraphics gfx = XGraphics.FromPdfPage(page1);
            XGraphics gfx2 = XGraphics.FromPdfPage(page2);

            // Create a font
            XFont font = new XFont("Verdana", 20, XFontStyle.BoldItalic);

            // page 1
            gfx.DrawString("Hello, World!", font, XBrushes.Black,
              new XRect(0, 0, page1.Width, page1.Height),
              XStringFormats.Center);

            string path = Directory.GetCurrentDirectory() + "\\Resources\\testImage.jpg";
            if (File.Exists(path))
            {
                XImage image = XImage.FromFile(path);
                gfx.DrawImage(image, new XRect(page1.Width/2, page1.Height/2, page1.Width / 4, page1.Height / 4));
            }

            // page 2
            gfx2.DrawString("Hello, I died!", font, XBrushes.Black,
              new XRect(100, 20, page1.Width, page1.Height),
              XStringFormats.Center);




            // Save the document...
            const string filename = "HelloWorld.pdf";
            document.Save(filename);
            // ...and start a viewer.
            Process.Start(filename);

            // TODO: Image Object needs to be inserted in PDF
        }
    }
}
