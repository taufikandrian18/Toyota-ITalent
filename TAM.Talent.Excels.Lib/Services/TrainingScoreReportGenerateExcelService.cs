using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.Text;
using TAM.Talent.Excels.Lib.Models;

namespace TAM.Talent.Excels.Lib.Services
{
    public class TrainingScoreReportGenerateExcelService
    {
        public XLWorkbook CreateExcelReport(List<TrainingScoreReportExcellModel> trainingScoreReport)
        {
            //creating new workbook
            var wb = new XLWorkbook();

            //creating new worksheet
            var ws = wb.AddWorksheet("Sheet1");

            //create header
            ws.Cell("A1").Value = "Timestamp";
            ws.Cell("B1").Value = "Total Score (Module)";
            ws.Cell("C1").Value = "Total Score (Question)";
            ws.Cell("D1").Value = "Attempts";
            ws.Cell("E1").Value = "Module";
            ws.Cell("F1").Value = "Competency Code";
            ws.Cell("G1").Value = "Type of Question";
            ws.Cell("H1").Value = "Question";
            ws.Cell("I1").Value = "Answer";
            ws.Cell("J1").Value = "Employee Name";
            ws.Cell("K1").Value = "Area";
            ws.Cell("L1").Value = "Province";
            ws.Cell("M1").Value = "City";
            ws.Cell("N1").Value = "Dealer";
            ws.Cell("O1").Value = "Outlet";

            //table data
            int i = 2;
            foreach (var item in trainingScoreReport)
            {
                ws.Cell("A" + i.ToString()).Value = item.Timestamp;
                ws.Cell("B" + i.ToString()).Value = item.TotalScoreModule;
                ws.Cell("C" + i.ToString()).Value = item.TotalScore;
                ws.Cell("D" + i.ToString()).Value = item.Attempts;
                ws.Cell("E" + i.ToString()).Value = item.ModuleName;
                ws.Cell("F" + i.ToString()).Value = item.CompetencyCode;
                ws.Cell("G" + i.ToString()).Value = item.TypeofQuestion;
                ws.Cell("H" + i.ToString()).Value = item.Question;
                ws.Cell("I" + i.ToString()).SetValue<string>(Convert.ToString(item.Answer));
                ws.Cell("J" + i.ToString()).Value = item.EmployeeName;
                ws.Cell("K" + i.ToString()).Value = item.Area;
                ws.Cell("L" + i.ToString()).Value = item.Province;
                ws.Cell("M" + i.ToString()).Value = item.City;
                ws.Cell("N" + i.ToString()).Value = item.Dealer;
                ws.Cell("O" + i.ToString()).Value = item.Outlet;
                i++;
            }

            //adjust column width
            ws.Columns(1, 14).AdjustToContents();

            //define table header range
            var rangeHeader = ws.Range("A1:N1");
            //set table header border style
            rangeHeader.Style.Border.OutsideBorder = XLBorderStyleValues.Medium;
            rangeHeader.Style.Border.InsideBorder = XLBorderStyleValues.Medium;
            //rangeHeader.Style.Fill.BackgroundColor = XLColor.Yellow;

            if (trainingScoreReport.Count > 0)
            {
                //define table data range
                var dataRange = ws.Range("A2" + ":O" + (i - 1).ToString());
                //set data table border style
                dataRange.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                dataRange.Style.Border.InsideBorder = XLBorderStyleValues.Thin;
                //set data table alignment
                dataRange.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
                dataRange.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
            }


            return wb;
        }
    }
}
