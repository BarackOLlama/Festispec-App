using GalaSoft.MvvmLight.Command;
using PdfSharp.Drawing;
using PdfSharp.Pdf;
using System.Diagnostics;
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

        //private void CreateTemp()
        //{
        //    // Create a temporary file
        //    string filename = String.Format("{0}_tempfile.pdf", Guid.NewGuid().ToString("D").ToUpper());
        //    var s_document = new PdfDocument();
        //    s_document.Info.Title = "PDFsharp XGraphic Sample";
        //    s_document.Info.Author = "Stefan Lange";
        //    s_document.Info.Subject = "Created with code snippets that show the use of graphical functions";
        //    s_document.Info.Keywords = "PDFsharp, XGraphics";

        //    using (XGraphics gfx = XGraphics.FromPdfPage(pdfPage))
        //    {
        //        XPen lineRed = new XPen(XColors.Red, 5);

        //        gfx.DrawImage(lineRed, 0, pdfPage.Height / 2, pdfPage.Width, pdfPage.Height / 2);
        //    }



        //    // Save the s_document...
        //    s_document.Save(filename);
        //    // ...and start a viewer
        //    Process.Start(filename);
        //}

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

            // Draw the text
            gfx.DrawString("Hello, World!", font, XBrushes.Black,
              new XRect(0, 0, page1.Width, page1.Height),
              XStringFormats.Center);

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


        public void DrawPage(PdfPage page)
        {
            //XGraphics gfx = XGraphics.FromPdfPage(page);

            //DrawTitle(page, gfx, "Images");

            //DrawImage(gfx, 1);
            //DrawImageScaled(gfx, 2);
            //DrawImageRotated(gfx, 3);
            //DrawImageSheared(gfx, 4);
            //DrawGif(gfx, 5);
            //DrawPng(gfx, 6);
            //DrawTiff(gfx, 7);
            //DrawFormXObject(gfx, 8);
        }
    }
}
