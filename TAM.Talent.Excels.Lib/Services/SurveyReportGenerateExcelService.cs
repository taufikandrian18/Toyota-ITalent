using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.Text;
using TAM.Talent.Excels.Lib.Models;

namespace TAM.Talent.Excels.Lib.Services
{
    public class SurveyReportGenerateExcelService
    {

        public XLWorkbook CreateExcelReport(List<SurveyReportExcelModel> surveyReportList)
        {
            //creating new workbook
            var wb = new XLWorkbook();

            //creating new worksheet
            var ws = wb.AddWorksheet("Sheet1");

            //create header
            ws.Cell("A1").Value = "Timestamp";
            ws.Cell("B1").Value = "Type of Questions";
            ws.Cell("C1").Value = "Question";
            ws.Cell("D1").Value = "Answer";
            ws.Cell("E1").Value = "Employee name";
            ws.Cell("F1").Value = "Area";
            ws.Cell("G1").Value = "Province";
            ws.Cell("H1").Value = "City";
            ws.Cell("I1").Value = "Dealer";
            ws.Cell("J1").Value = "Outlet";

            //table data
            int i = 2;
            foreach (var item in surveyReportList)
            {
                ws.Cell("A" + i.ToString()).Value = item.TimeStamp.ToString();
                ws.Cell("B" + i.ToString()).Value = item.TypeOfQuestion;
                ws.Cell("C" + i.ToString()).Value = item.Question;
                ws.Cell("D" + i.ToString()).SetValue<string>(Convert.ToString(item.Answer));
                ws.Cell("E" + i.ToString()).Value = item.EmployeeName;
                ws.Cell("F" + i.ToString()).Value = item.Area;
                ws.Cell("G" + i.ToString()).Value = item.Province;
                ws.Cell("H" + i.ToString()).Value = item.City;
                ws.Cell("I" + i.ToString()).Value = item.Dealer;
                ws.Cell("J" + i.ToString()).Value = item.Outlet;
                i++;
            }

            //adjust column width
            ws.Columns(1, 10).AdjustToContents();

            //define table header range
            var rangeHeader = ws.Range("A1:J1");
            //set table header border style
            rangeHeader.Style.Border.OutsideBorder = XLBorderStyleValues.Medium;
            rangeHeader.Style.Border.InsideBorder = XLBorderStyleValues.Medium;
            //rangeHeader.Style.Fill.BackgroundColor = XLColor.Yellow;

            //define table data range
            var dataRange = ws.Range("A2" + ":J" + (i-1).ToString());
            //set data table border style
            dataRange.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
            dataRange.Style.Border.InsideBorder = XLBorderStyleValues.Thin;
            //set data table alignment
            dataRange.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
            dataRange.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;

            return wb;
        }
    }
}
