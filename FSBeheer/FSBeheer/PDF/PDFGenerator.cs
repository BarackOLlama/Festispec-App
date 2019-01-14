using FSBeheer.VM;
using PdfSharp.Drawing;
using PdfSharp.Drawing.Layout;
using PdfSharp.Pdf;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;

namespace FSBeheer.ViewModel
{
    public class PDFGenerator
    {
        private PdfDocument document;

        private string _fileName;
        private string _title;
        private string _description;
        private string _advice;

        private ObservableCollection<QuestionVM> QuestionsList;

        public PDFGenerator(string Filename, string Title, string Description, string Advice, ObservableCollection<QuestionVM> Questions)
        {
            // Create a new PDF document
            document = new PdfDocument();
            document.Info.Title = "Created by Phi";

            _fileName = Filename;
            _title = Title;
            _description = Description;
            _advice = Advice;
            QuestionsList = Questions;
            foreach (var question in QuestionsList)
            {
                System.Console.WriteLine(question.Type);
            }
        }

        private void MakeFrontPage()
        {
            PdfPage page1 = document.AddPage();
            XGraphics gfx1 = XGraphics.FromPdfPage(page1);
            DrawTitle(page1, gfx1, _title);
        }

        private void SavePDF()
        {
            const string filename = "Inspectie PDF";
            document.Save(filename);
            Process.Start(filename);
        }

        public void CreateTestPDF()
        {
            MakeFrontPage();

            // sh*t....

            SavePDF();
        }

        public void CreateStandardPDF()
        {
            MakeFrontPage();
            // Create an empty page
            PdfPage page2 = document.AddPage();
            PdfPage page3 = document.AddPage();

            // Get an XGraphics object for drawing
            XGraphics gfx2 = XGraphics.FromPdfPage(page2);
            XGraphics gfx3 = XGraphics.FromPdfPage(page3);

            // Create a font
            XFont font = new XFont("Verdana", 20, XFontStyle.BoldItalic);

            // page 2
            gfx2.DrawString("Hello, I died!", font, XBrushes.Black,
              new XRect(100, 20, page2.Width, page2.Height),
              XStringFormats.Center);

            string path2 = Directory.GetCurrentDirectory() + "\\Resources\\nature.jpg";
            if (File.Exists(path2))
            {
                XImage image2 = XImage.FromFile(path2);
                gfx2.DrawImage(image2, page2.Width * 0.3, 0, 250, 250);
            }

            // page 3
            const string text =
              "Facin exeraessisit la consenim iureet " +
              "min ut in ute doloboreet ip ex et am dunt at.";

            XTextFormatter tf = new XTextFormatter(gfx3);

            XRect rect = new XRect(40, 100, 250, 220);
            gfx3.DrawRectangle(XBrushes.CadetBlue, rect);
            tf.DrawString(text, font, XBrushes.Black, rect, XStringFormats.TopLeft);

            SavePDF();
        }

        public void DrawTitle(PdfPage page, XGraphics gfx, string title)
        {
            XRect rect = new XRect(new XPoint(), gfx.PageSize);
            rect.Inflate(-10, -15);
            XFont font = new XFont("Verdana", 14, XFontStyle.Bold);
            gfx.DrawString(title, font, XBrushes.MidnightBlue, rect, XStringFormats.TopCenter);

            rect.Offset(0, 5);
            font = new XFont("Verdana", 8, XFontStyle.Italic);
            XStringFormat format = new XStringFormat
            {
                Alignment = XStringAlignment.Near,
                LineAlignment = XLineAlignment.Far
            };
            gfx.DrawString("Created with " + PdfSharp.ProductVersionInfo.Producer, font, XBrushes.DarkOrchid, rect, format);

            font = new XFont("Verdana", 8);
            format.Alignment = XStringAlignment.Center;
            gfx.DrawString(document.PageCount.ToString(), font, XBrushes.DarkOrchid, rect, format);

            document.Outlines.Add(title, page, true);
        }
    }
}
