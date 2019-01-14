using FSBeheer.VM;
using PdfSharp.Drawing;
using PdfSharp.Drawing.Layout;
using PdfSharp.Pdf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Windows;

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
        private CustomerVM _customer;
        private InspectionVM _inspection;
        private DateTime _startDate;
        private DateTime _endDate;

        private XGraphics gfx;

        private readonly XFont fontH1 = new XFont("Calibri", 16, XFontStyle.Underline);
        private readonly XFont font = new XFont("Calibri", 12);
        private readonly XFont fontH2 = new XFont("Calibri", 12, XFontStyle.Bold);
        private readonly XFont fontItalic = new XFont("Calibri", 12, XFontStyle.BoldItalic);

        private double x = 50;
        private double y = 100;

        public PDFGenerator(
            string Filename, 
            string Title, 
            string Description, 
            string Advice, 
            ObservableCollection<QuestionVM> Questions, 
            CustomerVM Customer, InspectionVM SelectedInspection, 
            DateTime? StartDate, 
            DateTime? EndDate)
        {
            document = new PdfDocument();
            document.Info.Title = "Created by Phi";

            _fileName = Filename;
            _title = Title;
            _description = Description;
            _advice = Advice;
            _customer = Customer;
            _inspection = SelectedInspection;
            _startDate = ((DateTime)StartDate).Date;
            _endDate = ((DateTime)EndDate).Date;
            QuestionsList = Questions;        
        }

        private void MakeFrontPage()
        {
            PdfPage page1 = document.AddPage();
            gfx = XGraphics.FromPdfPage(page1);
            double ls = font.GetHeight(gfx);

            // Logo
            string path2 = Directory.GetCurrentDirectory() + "\\Resources\\festispecLogo.jpg";
            if (File.Exists(path2))
            {
                XImage image2 = XImage.FromFile(path2);
                gfx.DrawImage(image2, page1.Width * 0.65, page1.Height * 0.1, 122, 33);
            }

            // Info
            gfx.DrawString("Festispec Rapportage: " + DateTime.Now.ToShortDateString(), 
                fontH1, XBrushes.Black, x, x - 30);
            gfx.DrawString("Rapportage: " + _title,
                fontH1, XBrushes.Black, x, x + 5);
            gfx.DrawString("Klant gegevens",
                fontH2, XBrushes.Black, x, y);
            y += ls;
            gfx.DrawString("Naam: " + _customer.Name, font, XBrushes.Black, x, y);
            y += ls;
            gfx.DrawString("Adres: " + _customer.Address,
                font, XBrushes.Black, x, y);
            y += ls;
            gfx.DrawString("Stad: " + _customer.City,
                font, XBrushes.Black, x, y);
            y += 1.2 * ls;
            gfx.DrawString("Contactpersoon - " + _customer.Contact.Name,
                fontH2, XBrushes.Black, x, y);
            y += ls;
            gfx.DrawString("Telefoonnummer: " + _customer.Contact.PhoneNumber, font, XBrushes.Black, x, y);
            y += ls;
            gfx.DrawString("Email: " + _customer.Contact.Email, font, XBrushes.Black, x, y);
            y += 1.2 * ls;
            gfx.DrawString("Inspectie gegevens",
                fontH2, XBrushes.Black, x, y);
            y += ls;
            gfx.DrawString("Naam: " + _inspection.Name, font, XBrushes.Black, x, y);
            y += ls;
            gfx.DrawString("Event naam: " + _inspection.Event.Name, font, XBrushes.Black, x, y);
            y += 1.2 * ls;
            gfx.DrawString("Datum",
                fontH2, XBrushes.Black, x, y);
            y += ls;
            gfx.DrawString(_startDate.ToShortDateString() + " tot " + _endDate.ToShortDateString(), font, XBrushes.Black, x, y);
            y += 2 * ls;
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
        }

        private void SavePDF(string Filename)
        {
            try
            {
                string filename = Filename;
                document.Save(filename); 
                Process.Start(filename);
            } catch
            {
                MessageBox.Show("You already have an existing document with that file name open! Close it first before opening a new again.");
            }
        }

        public void CreateStandardPDF()
        {
            MakeFrontPage();

            // foreach question, new page 
            PdfPage page2 = document.AddPage();
            XGraphics gfx2 = XGraphics.FromPdfPage(page2);

            SavePDF(_fileName);
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
