using FSBeheer.Properties;
using GalaSoft.MvvmLight.Command;
using PdfSharp.Drawing;
using PdfSharp.Drawing.Layout;
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

        private PdfDocument document;

        public TestPDFViewModel()
        {
            _myContext = new CustomFSContext();

            CreatePDFCommand = new RelayCommand(CreatePDF);
            CloseCommand = new RelayCommand<Window>(Close);
        }

        //public class LayoutHelper
        //{
        //    private readonly PdfDocument _document;
        //    private readonly XUnit _topPosition;
        //    private readonly XUnit _bottomMargin;
        //    private XUnit _currentPosition;

        //    public LayoutHelper(PdfDocument document, XUnit topPosition, XUnit bottomMargin)
        //    {
        //        _document = document;
        //        _topPosition = topPosition;
        //        _bottomMargin = bottomMargin;
        //        // Set a value outside the page - a new page will be created on the first request.
        //        _currentPosition = bottomMargin + 10000;
        //    }

        //    public XUnit GetLinePosition(XUnit requestedHeight)
        //    {
        //        return GetLinePosition(requestedHeight, -1f);
        //    }

        //    public XUnit GetLinePosition(XUnit requestedHeight, XUnit requiredHeight)
        //    {
        //        XUnit required = requiredHeight == -1f ? requestedHeight : requiredHeight;
        //        if (_currentPosition + required > _bottomMargin)
        //            CreatePage();
        //        XUnit result = _currentPosition;
        //        _currentPosition += requestedHeight;
        //        return result;
        //    }

        //    public XGraphics Gfx { get; private set; }
        //    public PdfPage Page { get; private set; }

        //    void CreatePage()
        //    {
        //        Page = _document.AddPage();
        //        Page.Size = PageSize.A4;
        //        Gfx = XGraphics.FromPdfPage(Page);
        //        _currentPosition = _topPosition;
        //    }
        //}

        private void Close(Window window)
        {
            window.Close();
        }

        private void CreatePDF()
        {
            // Create a new PDF document
            document = new PdfDocument();
            document.Info.Title = "Created with PDFsharp";

            // Create an empty page
            PdfPage page1 = document.AddPage();
            PdfPage page2 = document.AddPage();
            PdfPage page3 = document.AddPage();


            // Get an XGraphics object for drawing
            XGraphics gfx = XGraphics.FromPdfPage(page1);
            XGraphics gfx2 = XGraphics.FromPdfPage(page2);
            XGraphics gfx3 = XGraphics.FromPdfPage(page3);

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
                gfx.DrawImage(image, page1.Width*0.3, 0, 250, 250);
                //gfx.DrawImage(image, new XRect(page1.Width / 2, page1.Height / 2, page1.Width / 4, page1.Height / 4));
            }

            DrawTitle(page1, gfx, "A Title");

            // page 2
            gfx2.DrawString("Hello, I died!", font, XBrushes.Black,
              new XRect(100, 20, page1.Width, page1.Height),
              XStringFormats.Center);

            string path2 = Directory.GetCurrentDirectory() + "\\Resources\\nature.jpg";
            if (File.Exists(path2))
            {
                XImage image2 = XImage.FromFile(path2);
                gfx2.DrawImage(image2, page1.Width * 0.3, 0, 250, 250);
                //gfx.DrawImage(image, new XRect(page1.Width / 2, page1.Height / 2, page1.Width / 4, page1.Height / 4));
            }

            // page 3
            const string text =
              "Facin exeraessisit la consenim iureet dignibh eu facilluptat vercil dunt autpat. " +
              "Ecte magna faccum dolor sequisc iliquat, quat, quipiss equipit accummy niate magna " +
              "facil iure eraesequis am velit, quat atis dolore dolent luptat nulla adio odipissectet " +
              "lan venis do essequatio conulla facillandrem zzriusci bla ad minim inis nim velit eugait " +
              "aut aut lor at ilit ut nulla ate te eugait alit augiamet ad magnim iurem il eu feuissi.\n" +
              "Guer sequis duis eu feugait luptat lum adiamet, si tate dolore mod eu facidunt adignisl in " +
              "henim dolorem nulla faccum vel inis dolutpatum iusto od min ex euis adio exer sed del " +
              "dolor ing enit veniamcon vullutat praestrud molenis ciduisim doloborem ipit nulla consequisi.\n" +
              "Nos adit pratetu eriurem delestie del ut lumsandreet nis exerilisit wis nos alit venit praestrud " +
              "dolor sum volore facidui blaor erillaortis ad ea augue corem dunt nis  iustinciduis euisi.\n" +
              "Ut ulputate volore min ut nulpute dolobor sequism olorperilit autatie modit wisl illuptat dolore " +
              "min ut in ute doloboreet ip ex et am dunt at.";


            XTextFormatter tf = new XTextFormatter(gfx3);

            XRect rect = new XRect(40, 100, 250, 220);
            gfx3.DrawRectangle(XBrushes.CadetBlue, rect);
            //tf.Alignment = ParagraphAlignment.Left;
            tf.DrawString(text, font, XBrushes.Black, rect, XStringFormats.TopLeft);

            rect = new XRect(310, 100, 250, 220);
            gfx3.DrawRectangle(XBrushes.BurlyWood, rect);
            tf.Alignment = XParagraphAlignment.Right;
            tf.DrawString(text, font, XBrushes.Black, rect, XStringFormats.TopLeft);

            rect = new XRect(40, 400, 250, 220);
            gfx3.DrawRectangle(XBrushes.SeaShell, rect);
            tf.Alignment = XParagraphAlignment.Center;
            tf.DrawString(text, font, XBrushes.Black, rect, XStringFormats.TopLeft);

            rect = new XRect(310, 400, 250, 220);
            gfx3.DrawRectangle(XBrushes.SeaShell, rect);
            tf.Alignment = XParagraphAlignment.Justify;
            tf.DrawString(text, font, XBrushes.Black, rect, XStringFormats.TopLeft);


            // Save the document...
            const string filename = "HelloWorld.pdf";
            document.Save(filename);
            // ...and start a viewer.
            Process.Start(filename);

            // TODO: Image Object needs to be inserted in PDF
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
