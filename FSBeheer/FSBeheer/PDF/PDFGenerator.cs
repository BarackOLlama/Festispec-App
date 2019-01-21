﻿using FSBeheer.PDF;
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
        private List<string> _selectedCharts;
        private CustomerVM _customer;
        private InspectionVM _inspection;
        private DateTime _startDate;
        private DateTime _endDate;

        private XGraphics gfx;
        private XTextFormatter tf;

        private readonly XFont fontH1 = new XFont("Calibri", 16, XFontStyle.Underline);
        private readonly XFont font = new XFont("Calibri", 12);
        private readonly XFont fontH2 = new XFont("Calibri", 12, XFontStyle.Bold);
        private readonly XFont fontItalic = new XFont("Calibri", 12, XFontStyle.BoldItalic);

        private double x = 50;
        private double y = 100;
        private double ls;
        private double x2 = 50;
        private double y2 = 75;

        private XGraphics gfxAll;
        private CustomFSContext _context;
        private ObservableCollection<AnswerVM> answersList;


        public PDFGenerator(
            string Filename,
            string Title,
            string Description,
            string Advice,
            ObservableCollection<QuestionVM> Questions,
            List<string> SelectedCharts,
            CustomerVM Customer, InspectionVM SelectedInspection, 
            DateTime? StartDate, 
            DateTime? EndDate,
            CustomFSContext _context)
        {
            this._context = _context;
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
            _selectedCharts = SelectedCharts;
        }

        private void MakeFrontPage()
        {
            PdfPage page1 = document.AddPage();
            gfx = XGraphics.FromPdfPage(page1);
            ls = font.GetHeight(gfx);
            tf = new XTextFormatter(gfx);

            // Logo
            string path2 = Directory.GetCurrentDirectory() + "\\Resources\\festispecLogo.jpg";
            if (File.Exists(path2))
            {
                XImage image2 = XImage.FromFile(path2);
                gfx.DrawImage(image2, page1.Width * 0.65, page1.Height * 0.1, 122, 33);
            }

            // image test
            // QuestionsList[0] is een multiple choice
            // ChartGenerator chartgen = new ChartGenerator(QuestionsList[1], "Bar", 300, 300);
            // XImage image2 = chartgen.GetImageFromChart();
            // gfx.DrawImage(image2, page1.Width * 0.4, page1.Height * 0.1, image2.PixelWidth, image2.PixelHeight);

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

            if (_description != null)
            {
                gfx.DrawString("Omschrijving: ", font, XBrushes.Black, x, y);
                y += ls * 1.8;

                XRect descRect = new XRect(50, 350, 500, 220);
                gfx.DrawRectangle(XBrushes.White, descRect);
                tf.DrawString(_description, font, XBrushes.Black, descRect, XStringFormats.TopLeft);
            }

            if (_advice != null)
            {
                gfx.DrawString("Advies: ", font, XBrushes.Black, x, 560);
                
                XRect adviceRect = new XRect(50, 580, 500, 220);
                gfx.DrawRectangle(XBrushes.White, adviceRect);
                tf.DrawString(_advice, font, XBrushes.Black, adviceRect, XStringFormats.TopLeft);
            }
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

        // TODO: live chart 
        public void CreateStandardPDF()
        {
            MakeFrontPage();

            
            for (int i = 0; i < QuestionsList.Count; i++)
            {
                switch(QuestionsList[i].Type.Name)
                {
                    case "Open Vraag":
                        DrawInformation("Open vraag: ", QuestionsList[i], i);
                        break;
                    case "Open Tabelvraag":
                        DrawInformation("Open tabelvraag: ", QuestionsList[i], i);
                        break;
                    case "Multiple Choice Tabelvraag":
                        DrawInformation("Meerkeuze tabelvraag: ", QuestionsList[i], i);
                        break;
                    case "Multiple Choice vraag":
                        DrawInformation("Meerkeuze vraag: ", QuestionsList[i], i);
                        break;
                    case "Schaal Vraag":
                        DrawInformation("Schaal vraag: ", QuestionsList[i], i);
                        break;
                }
            }
            SavePDF(_fileName);
        }

        private void DrawInformation(string value, QuestionVM question, int i)
        {
            // question
            gfxAll = XGraphics.FromPdfPage(document.AddPage(new PdfPage()));
            gfxAll.DrawString(value + question.Content,
            font, XBrushes.Black, x2, y2);
            if (question.Options != null)
            {
                y2 += ls + 5;
                string setOptions = question.Options;
                gfxAll.DrawString("Mogelijke antwoorden: " + setOptions,
                font, XBrushes.Black, x2, y2);
            }

            // answers
            y2 += ls + 5;
            gfxAll.DrawString("Antwoorden: ", font, XBrushes.Black, x2, y2);
            y2 += ls + 10;
            answersList = _context.AnswerCrud.GetAllAnswersByQuestionId(question.Id);
            foreach (var answer in answersList)
            {
                gfxAll.DrawString(answer.Content,
                font, XBrushes.Black, x2, y2);
                y2 += ls;
                if (y > 820)
                {
                    gfxAll = XGraphics.FromPdfPage(document.AddPage(new PdfPage()));
                    x2 = 50;
                    y2 = 75;
                }
            }

            y2 += ls + 10;

            if (_selectedCharts[i] == "PieChart")
            {
                gfxAll.DrawString("This is a Pie Chart",
                font, XBrushes.Black, x2, y2);
            }
            else if (_selectedCharts[i] == "BarChart")
            {
                gfxAll.DrawString("This is a Bar Chart",
                font, XBrushes.Black, x2, y2);
            }

            x2 = 50;
            y2 = 75;
        }
    }
}
