using FSBeheer.VM;
using PdfSharp.Drawing;
using PdfSharp.Drawing.Layout;
using PdfSharp.Pdf;
using System;
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
        private XFont font;

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
                Console.WriteLine(question.Type);
            }
        }

        private void MakeFrontPage()
        {
            PdfPage page1 = document.AddPage();
            XGraphics gfx = XGraphics.FromPdfPage(page1);
            XRect rect;
            XPen pen;
            double x = 50, y = 100;
            XFont fontH1 = new XFont("Calibri", 18, XFontStyle.Bold);
            XFont font = new XFont("Calibri", 12);
            XFont fontItalic = new XFont("Calibri", 12, XFontStyle.BoldItalic);
            double ls = font.GetHeight(gfx);

            DrawTitle(page1, gfx, _title);
            gfx.DrawString(DateTime.Now.ToShortDateString(), font, XBrushes.DarkSalmon,
              new XRect(0, 0, (page1.Width / 2), (page1.Height / 2)),
              XStringFormats.Center);


            // Draw some text
            gfx.DrawString("Create PDF on the fly with PDFsharp",
                fontH1, XBrushes.Black, x, x);
            gfx.DrawString("With PDFsharp you can use the same code to draw graphic, " +
                "text and images on different targets.", font, XBrushes.Black, x, y);
            y += ls;
            gfx.DrawString("The object used for drawing is the XGraphics object.",
                font, XBrushes.Black, x, y);
            y += 2 * ls;

            // Draw some more text
            y += 60 + 2 * ls;
            gfx.DrawString("With XGraphics you can draw on a PDF page as well as " +
                "on any System.Drawing.Graphics object.", font, XBrushes.Black, x, y);
            y += ls * 1.1;
            gfx.DrawString("Use the same code to", font, XBrushes.Black, x, y);
            x += 10;
            y += ls * 1.1;
            gfx.DrawString("• draw on a newly created PDF page", font, XBrushes.Black, x, y);
            y += ls;
            gfx.DrawString("• draw above or beneath of the content of an existing PDF page",
                font, XBrushes.Black, x, y);
            y += ls;
            gfx.DrawString("• draw in a window", font, XBrushes.Black, x, y);
            y += ls;
            gfx.DrawString("• draw on a printer", font, XBrushes.Black, x, y);
            y += ls;
            gfx.DrawString("• draw in a bitmap image", font, XBrushes.Black, x, y);
            x -= 10;
            y += ls * 1.1;
            gfx.DrawString("You can also import an existing PDF page and use it like " +
                "an image, e.g. draw it on another PDF page.", font, XBrushes.Black, x, y);
            y += ls * 1.1 * 2;
            gfx.DrawString("Imported PDF pages are neither drawn nor printed; create a " +
                "PDF file to see or print them!", fontItalic, XBrushes.Firebrick, x, y);
            y += ls * 1.1;
            gfx.DrawString("Below this text is a PDF form that will be visible when " +
                "viewed or printed with a PDF viewer.", fontItalic, XBrushes.Firebrick, x, y);
            y += ls * 1.1;
            XGraphicsState state = gfx.Save();
            XRect rcImage = new XRect(100, y, 100, 100 * Math.Sqrt(2));
            gfx.DrawRectangle(XBrushes.Snow, rcImage);
            gfx.Restore(state);
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
