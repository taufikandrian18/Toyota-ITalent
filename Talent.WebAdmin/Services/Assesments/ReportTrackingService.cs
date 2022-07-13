using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ClosedXML.Excel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Talent.Entities.DbQueryModels;
using Talent.Entities.Entities;
using Talent.WebAdmin.Enums;
using Talent.WebAdmin.Models;
using TAM.Talent.Commons.Core.Interfaces;
using ReportTrackingCompetencyPoints = Talent.Entities.DbQueryModels.ReportTrackingCompetencyPoints;

namespace Talent.WebAdmin.Services
{
    public class ReportTrackingService : Controller
    {
        private readonly TalentContext DB;
        IFileStorageService FileService;
        private readonly BlobService BlobService;

        public ReportTrackingService(TalentContext db, IFileStorageService fileService, BlobService blobService)
        {
            this.DB = db;
            this.BlobService = blobService;
            this.FileService = fileService;
        }

        public async Task<ActionResult> CompetencyTracking(GetTrackingParameter model)
        {
            if (string.IsNullOrWhiteSpace(model.DealerID))
            {
                model.DealerID = "*";
            }

            if (string.IsNullOrWhiteSpace(model.OutletID))
            {
                model.OutletID = "0";
            }

            if (string.IsNullOrWhiteSpace(model.AreaID))
            {
                model.AreaID = "0";
            }

            if (string.IsNullOrWhiteSpace(model.EmployeeID))
            {
                model.EmployeeID = "0";
            }

            if (string.IsNullOrWhiteSpace(model.EmployeeName))
            {
                model.EmployeeName = "0";
            }

            if (string.IsNullOrWhiteSpace(model.Positions))
            {
                model.Positions = "*";
            }

            if (string.IsNullOrWhiteSpace(model.Status))
            {
                model.Status = "*";
            }

            if (model.Limit == 0)
            {
                model.Limit = 100;
            }

            if (model.Page == 0)
            {
                model.Page = 1;
            }

            model.Page = (model.Page - 1) * model.Limit;

            var query = DB.GetCompetencyTracking(
                                model.TrainingID,
                                model.DealerID,
                                model.OutletID,
                                model.AreaID,
                                model.EmployeeID,
                                model.EmployeeName,
                                model.Positions,
                                model.Status
                             );

            int count = query.Count();

            var data = await query.Skip(model.Page).Take(model.Limit).ToListAsync();

            /*
            var data = DB.GetCompetencyTracking(
                                model.TrainingID,
                                model.DealerID,
                                model.OutletID,
                                model.AreaID,
                                model.EmployeeID,
                                model.EmployeeName
                             ).ToList();
            */

            foreach (var report in data)
            {
                report.PointsJSON = JsonConvert.DeserializeObject<ReportTrackingCompetencyPoints[]>(report.Points);
                report.PointsDuringJSON = JsonConvert.DeserializeObject<ReportTrackingCompetencyPoints[]>(report.PointsDuring);
                report.PointsPostJSON = JsonConvert.DeserializeObject<ReportTrackingCompetencyPoints[]>(report.PointsPost);

                report.Points = "";
                report.PointsDuring = "";
                report.PointsPost = "";
            }

            return StatusCode(200, BaseResponse.ResponseOk(data, count));
        }

        public async Task<ActionResult> ExportExcel(GetTrackingParameter model)
        {
            if (string.IsNullOrWhiteSpace(model.DealerID))
            {
                model.DealerID = "*";
            }

            if (string.IsNullOrWhiteSpace(model.OutletID))
            {
                model.OutletID = "0";
            }

            if (string.IsNullOrWhiteSpace(model.AreaID))
            {
                model.AreaID = "0";
            }

            if (string.IsNullOrWhiteSpace(model.EmployeeID))
            {
                model.EmployeeID = "0";
            }

            if (string.IsNullOrWhiteSpace(model.EmployeeName))
            {
                model.EmployeeName = "0";
            }

            if (string.IsNullOrWhiteSpace(model.Positions))
            {
                model.Positions = "*";
            }

            if (string.IsNullOrWhiteSpace(model.Status))
            {
                model.Status = "*";
            }

            var reports = DB.GetCompetencyTracking(
                                model.TrainingID, 
                                model.DealerID,
                                model.OutletID,
                                model.AreaID,
                                model.EmployeeID,
                                model.EmployeeName,
                                model.Positions,
                                model.Status
                             ).ToList();

            var MS = await CreateExcelReport(reports);
            return File(MS, FormatDocumentEnum.ExcelFormat, "Learning Progress Report.xlsx");
        }

        public async Task<MemoryStream> CreateExcelReport(List<ReportCompetencyTracking> reports)
        {
            //creating new workbook
            var wb = new XLWorkbook();

            //creating new worksheet
            var ws = wb.AddWorksheet("Sheet1");

            var dynamicColumn = "[]";
            if (reports.Count() > 0) {
                dynamicColumn = reports[0].Points;
            }

            ReportTrackingCompetencyPoints[] points = JsonConvert.DeserializeObject<ReportTrackingCompetencyPoints[]>(dynamicColumn);

            var dynamicColumnDuring = "[]";
            if (reports.Count() > 0)
            {
                dynamicColumnDuring = reports[0].PointsDuring;
            }

            ReportTrackingCompetencyPoints[] pointsDuring = JsonConvert.DeserializeObject<ReportTrackingCompetencyPoints[]>(dynamicColumnDuring);

            var dynamicColumnPost = "[]";
            if (reports.Count() > 0)
            {
                dynamicColumnPost = reports[0].PointsPost;
            }

            ReportTrackingCompetencyPoints[] pointsPost = JsonConvert.DeserializeObject<ReportTrackingCompetencyPoints[]>(dynamicColumnPost);

            //create header
            ws.Cell("A1").Value = "Program";
            ws.Range(ws.Cell("A1"), ws.Cell("A2")).Merge();

            ws.Cell("B1").Value = "Course";
            ws.Range(ws.Cell("B1"), ws.Cell("B2")).Merge();

            ws.Cell("C1").Value = "Dealer";
            ws.Range(ws.Cell("C1"), ws.Cell("C2")).Merge();

            ws.Cell("D1").Value = "Outlet";
            ws.Range(ws.Cell("D1"), ws.Cell("D2")).Merge();

            ws.Cell("E1").Value = "Area";
            ws.Range(ws.Cell("E1"), ws.Cell("E2")).Merge();

            ws.Cell("F1").Value = "Area Specialist";
            ws.Range(ws.Cell("F1"), ws.Cell("F2")).Merge();

            ws.Cell("G1").Value = "Position";
            ws.Range(ws.Cell("G1"), ws.Cell("G2")).Merge();

            ws.Cell("H1").Value = "Employee ID";
            ws.Range(ws.Cell("H1"), ws.Cell("H2")).Merge();

            ws.Cell("I1").Value = "Employee Name";
            ws.Range(ws.Cell("I1"), ws.Cell("I2")).Merge();

            int last = 10 + points.Length;
            int tempLast = last;

            if (points.Length > 0)
            {
                ws.Cell("J1").Value = "Pre";
            }

            if (points.Length > 1)
            {
                ws.Range(ws.Cell("J1"), ws.Cell(1, last - 1)).Merge();
            }

            if (pointsDuring.Length > 0)
            {
                ws.Cell(1, last).Value = "During";
                tempLast = last;
                last = last + pointsDuring.Length;
            }

            if (pointsDuring.Length > 1)
            {
                ws.Range(ws.Cell(1, tempLast), ws.Cell(1, last - 1)).Merge();
            }

            if (pointsPost.Length > 0)
            {
                ws.Cell(1, last).Value = "Post";
                tempLast = last;
                last = last + pointsPost.Length;
            }

            if (pointsPost.Length > 1)
            {
                ws.Range(ws.Cell(1, tempLast), ws.Cell(1, last - 1)).Merge();
            }

            String[] alphabet = new String[27]{
                "",
                "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M",
                "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z"
            };

            int firstColumnIndex = 0;
            int secondColumnIndex = 0;
            int thirdColumnIndex = 10;
            String column;

            foreach (var point in points)
            {
                column = alphabet[firstColumnIndex % 27] + alphabet[secondColumnIndex % 27] + alphabet[thirdColumnIndex % 27];
                ws.Cell(column + "2").Value = point.ModuleName;

                if (secondColumnIndex % 26 == 0 && secondColumnIndex != 0)
                {
                    firstColumnIndex++;
                    secondColumnIndex = 1;
                }

                if (thirdColumnIndex % 26 == 0 && thirdColumnIndex != 0)
                {
                    secondColumnIndex++;
                    thirdColumnIndex = 1;
                }
                else
                {
                    thirdColumnIndex++;
                }
            }

            foreach (var point in pointsDuring)
            {
                column = alphabet[firstColumnIndex % 27] + alphabet[secondColumnIndex % 27] + alphabet[thirdColumnIndex % 27];
                ws.Cell(column + "2").Value = point.ModuleName;

                if (secondColumnIndex % 26 == 0 && secondColumnIndex != 0)
                {
                    firstColumnIndex++;
                    secondColumnIndex = 1;
                }

                if (thirdColumnIndex % 26 == 0 && thirdColumnIndex != 0)
                {
                    secondColumnIndex++;
                    thirdColumnIndex = 1;
                }
                else
                {
                    thirdColumnIndex++;
                }
            }

            foreach (var point in pointsPost)
            {
                column = alphabet[firstColumnIndex % 27] + alphabet[secondColumnIndex % 27] + alphabet[thirdColumnIndex % 27];
                ws.Cell(column + "2").Value = point.ModuleName;

                if (secondColumnIndex % 26 == 0 && secondColumnIndex != 0)
                {
                    firstColumnIndex++;
                    secondColumnIndex = 1;
                }

                if (thirdColumnIndex % 26 == 0 && thirdColumnIndex != 0)
                {
                    secondColumnIndex++;
                    thirdColumnIndex = 1;
                }
                else
                {
                    thirdColumnIndex++;
                }
            }

            column = alphabet[firstColumnIndex % 27] + alphabet[secondColumnIndex % 27] + alphabet[thirdColumnIndex % 27];
            ws.Cell(column + "1").Value = "Completeness";
            ws.Range(ws.Cell(column + "1"), ws.Cell(column + "2")).Merge();

            //table data
            int i = 3;
            foreach (var item in reports)
            {
                ws.Cell("A" + i.ToString()).Value = item.Program;
                ws.Cell("B" + i.ToString()).Value = item.Course;
                ws.Cell("C" + i.ToString()).Value = item.Dealer;
                ws.Cell("D" + i.ToString()).Value = item.Outlet;
                ws.Cell("E" + i.ToString()).Value = item.Area;
                ws.Cell("F" + i.ToString()).Value = item.AreaSpecialist;
                ws.Cell("G" + i.ToString()).Value = item.Position;
                ws.Cell("H" + i.ToString()).Value = item.EmployeeID;
                ws.Cell("I" + i.ToString()).Value = item.EmployeeName;

                points = JsonConvert.DeserializeObject<ReportTrackingCompetencyPoints[]>(reports[i - 3].Points);
                pointsDuring = JsonConvert.DeserializeObject<ReportTrackingCompetencyPoints[]>(reports[i - 3].PointsDuring);
                pointsPost = JsonConvert.DeserializeObject<ReportTrackingCompetencyPoints[]>(reports[i - 3].PointsPost);

                firstColumnIndex = 0;
                secondColumnIndex = 0;
                thirdColumnIndex = 10;

                foreach (var point in points)
                {
                    column = alphabet[firstColumnIndex % 27] + alphabet[secondColumnIndex % 27] + alphabet[thirdColumnIndex % 27];
                    ws.Cell(column + i.ToString()).Value = point.Point;

                    if (secondColumnIndex % 26 == 0 && secondColumnIndex != 0)
                    {
                        firstColumnIndex++;
                        secondColumnIndex = 1;
                    }

                    if (thirdColumnIndex % 26 == 0 && thirdColumnIndex != 0)
                    {
                        secondColumnIndex++;
                        thirdColumnIndex = 1;
                    }
                    else
                    {
                        thirdColumnIndex++;
                    }
                }

                foreach (var point in pointsDuring)
                {
                    column = alphabet[firstColumnIndex % 27] + alphabet[secondColumnIndex % 27] + alphabet[thirdColumnIndex % 27];
                    ws.Cell(column + i.ToString()).Value = point.Point;

                    if (secondColumnIndex % 26 == 0 && secondColumnIndex != 0)
                    {
                        firstColumnIndex++;
                        secondColumnIndex = 1;
                    }

                    if (thirdColumnIndex % 26 == 0 && thirdColumnIndex != 0)
                    {
                        secondColumnIndex++;
                        thirdColumnIndex = 1;
                    }
                    else
                    {
                        thirdColumnIndex++;
                    }
                }

                foreach (var point in pointsPost)
                {
                    column = alphabet[firstColumnIndex % 27] + alphabet[secondColumnIndex % 27] + alphabet[thirdColumnIndex % 27];
                    ws.Cell(column + i.ToString()).Value = point.Point;

                    if (secondColumnIndex % 26 == 0 && secondColumnIndex != 0)
                    {
                        firstColumnIndex++;
                        secondColumnIndex = 1;
                    }

                    if (thirdColumnIndex % 26 == 0 && thirdColumnIndex != 0)
                    {
                        secondColumnIndex++;
                        thirdColumnIndex = 1;
                    }
                    else
                    {
                        thirdColumnIndex++;
                    }
                }

                column = alphabet[firstColumnIndex % 27] + alphabet[secondColumnIndex % 27] + alphabet[thirdColumnIndex % 27];
                ws.Cell(column + i.ToString()).Value = reports[i - 3].Video;
                i++;
            }
            if (reports.Count() < 1)
            {
                column = "F";
            }
            //adjust column width
            ws.Columns(1, points.Length + pointsDuring.Length + pointsPost.Length - 4).AdjustToContents();
            
            //define table header range
            var rangeHeader = ws.Range("A1:" + column + "2");
            //set table header border style
            rangeHeader.Style.Border.OutsideBorder = XLBorderStyleValues.Medium;
            rangeHeader.Style.Border.InsideBorder = XLBorderStyleValues.Medium;

            if (reports.Count() > 0)
            {
                //define table data range
                var dataRange = ws.Range("A3" + ":" + column + (i - 1).ToString());
                //set data table border style
                dataRange.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                dataRange.Style.Border.InsideBorder = XLBorderStyleValues.Thin;
                //set data table alignment
                dataRange.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
                dataRange.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
            }

            MemoryStream MS = new MemoryStream();
            wb.SaveAs(MS);
            MS.Position = 0;

            return MS;
        }

        public async Task<ActionResult> ExportExcelKPI(GetKPIParameter model)
        {
            if (string.IsNullOrWhiteSpace(model.DealerID))
            {
                model.DealerID = "0";
            }

            if (string.IsNullOrWhiteSpace(model.AreaID))
            {
                model.AreaID = "0";
            }

            var reports = DB.GetKPITracking(
                                model.TrainingID,
                                model.DealerID,
                                model.AreaID
                             ).ToList();

            var MS = await CreateExcelReportKPI(reports);
            return File(MS, FormatDocumentEnum.ExcelFormat, "KPI.xlsx");
        }

        public async Task<MemoryStream> CreateExcelReportKPI(List<ReportKPITracking> reports)
        {
            //creating new workbook
            var wb = new XLWorkbook();

            //creating new worksheet
            var ws = wb.AddWorksheet("Sheet1");

            var dynamicColumn = "[]";
            if (reports.Count() > 0)
            {
                dynamicColumn = reports[0].ResultTotal;
            }

            ReportKPITrackingPoints[] points = JsonConvert.DeserializeObject<ReportKPITrackingPoints[]>(dynamicColumn);

            //create header
            ws.Range("A1:A3").Merge();
            ws.Range("B1:B3").Merge();
            ws.Range("C1:C3").Merge();

            ws.Cell("A1").Value = "Dealer";
            ws.Cell("B1").Value = "Outlet";
            ws.Cell("C1").Value = "Area";
            ws.Cell("D1").Value = "Pre-SGA: Total Selesperson";

            if (reports.Count() > 0)
            {
                //Total Sales
                int column = 4;
                ws.Range(ws.Cell(1, column), ws.Cell(1, 3 + (points.Count() * 2 - 1))).Merge();

                foreach (var point in points)
                {
                    ws.Cell(2, column).Value = point.ModuleName;

                    if (point.ModuleName == "Target")
                    {
                        ws.Range(ws.Cell(2, column), ws.Cell(3, column)).Merge();
                        column -= 1;
                    }
                    else
                    {
                        ws.Range(ws.Cell(2, column), ws.Cell(3, column+1)).Merge();
                    }
                    column += 2;
                }

                //Cycle 2
                ws.Range(ws.Cell(1, column), ws.Cell(1, column - 1 + 9)).Merge();
                ws.Cell(1, column).Value = "Trainee & JSE Salesperson (include Role Model)";

                ws.Cell(2, column).Value = "Target";
                ws.Range(ws.Cell(2, column), ws.Cell(3, column)).Merge();

                column++;

                ws.Cell(2, column).Value = "Skill Check";
                ws.Range(ws.Cell(2, column), ws.Cell(2, column + 3)).Merge();

                
                ws.Cell(3, column).Value = "Participant Rate";
                ws.Range(ws.Cell(3, column), ws.Cell(3, column + 1)).Merge();
                column += 2;

                
                ws.Cell(3, column).Value = "Passing Rate";
                ws.Range(ws.Cell(3, column), ws.Cell(3, column + 1)).Merge();
                column += 2;

                ws.Cell(2, column).Value = "Video Submission";
                ws.Range(ws.Cell(2, column), ws.Cell(3, column + 1)).Merge();
                column += 2;

                ws.Cell(2, column).Value = "Certified";
                ws.Range(ws.Cell(2, column), ws.Cell(3, column + 1)).Merge();
                column += 2;

                //Cycle 3
                ws.Range(ws.Cell(1, column), ws.Cell(1, column - 1 + 9)).Merge();
                ws.Cell(1, column).Value = "SE & SSE Salesperson";

                ws.Cell(2, column).Value = "Target";
                ws.Range(ws.Cell(2, column), ws.Cell(3, column)).Merge();

                column++;

                ws.Cell(2, column).Value = "Skill Check";
                ws.Range(ws.Cell(2, column), ws.Cell(2, column + 3)).Merge();


                ws.Cell(3, column).Value = "Participant Rate";
                ws.Range(ws.Cell(3, column), ws.Cell(3, column + 1)).Merge();
                column += 2;


                ws.Cell(3, column).Value = "Passing Rate";
                ws.Range(ws.Cell(3, column), ws.Cell(3, column + 1)).Merge();
                column += 2;

                ws.Cell(2, column).Value = "Video Submission";
                ws.Range(ws.Cell(2, column), ws.Cell(3, column + 1)).Merge();
                column += 2;

                ws.Cell(2, column).Value = "Certified";
                ws.Range(ws.Cell(2, column), ws.Cell(3, column + 1)).Merge();
                column += 2;

                //Supervisor
                ws.Cell(1, column).Value = "Supervisor";
                ws.Range(ws.Cell(1, column), ws.Cell(1, column - 1 + (points.Count() * 2 - 1))).Merge();

                foreach (var point in points)
                {
                    ws.Cell(2, column).Value = point.ModuleName;

                    if (point.ModuleName == "Target")
                    {
                        ws.Range(ws.Cell(2, column), ws.Cell(3, column)).Merge();
                        column -= 1;
                    }
                    else
                    {
                        ws.Range(ws.Cell(2, column), ws.Cell(3, column + 1)).Merge();
                    }
                    column += 2;
                }

                //Branch Manager
                ws.Cell(1, column).Value = "Branch Manager";
                ws.Range(ws.Cell(1, column), ws.Cell(1, column - 1 + (points.Count() * 2 - 1))).Merge();

                foreach (var point in points)
                {
                    ws.Cell(2, column).Value = point.ModuleName;

                    if (point.ModuleName == "Target")
                    {
                        ws.Range(ws.Cell(2, column), ws.Cell(3, column)).Merge();
                        column -= 1;
                    }
                    else
                    {
                        ws.Range(ws.Cell(2, column), ws.Cell(3, column + 1)).Merge();
                    }
                    column += 2;
                }

                //Table Data
                int i = 4;
                foreach (var item in reports)
                {
                    ws.Cell("A" + i.ToString()).Value = item.Dealer;
                    ws.Cell("B" + i.ToString()).Value = item.Outlet;
                    ws.Cell("C" + i.ToString()).Value = item.Area;

                    column = 4;

                    points = JsonConvert.DeserializeObject<ReportKPITrackingPoints[]>(reports[i - 4].ResultTotal);

                    foreach (var point in points)
                    {
                        ws.Cell(i, column).Value = point.Point;
                        column++;
                        if (point.ModuleName != "Target")
                        {
                            ws.Cell(i, column).Value = point.Percentage;
                            column++;
                        }
                    }

                    ws.Cell(i, column).Value = item.Cycle2Target;
                    column++;
                    ws.Cell(i, column).Value = item.Cycle2Participant;
                    column++;
                    ws.Cell(i, column).Value = item.Cycle2ParticipantPercentage;
                    column++;
                    ws.Cell(i, column).Value = item.Cycle2Passed;
                    column++;
                    ws.Cell(i, column).Value = item.Cycle2PassedPercentage;
                    column++;
                    ws.Cell(i, column).Value = item.Cycle2Video;
                    column++;
                    ws.Cell(i, column).Value = item.Cycle2VideoPercentage;
                    column++;
                    ws.Cell(i, column).Value = item.Cycle2Certified;
                    column++;
                    ws.Cell(i, column).Value = item.Cycle2CertifiedPercentage;
                    column++;

                    ws.Cell(i, column).Value = item.Cycle3Target;
                    column++;
                    ws.Cell(i, column).Value = item.Cycle3Participant;
                    column++;
                    ws.Cell(i, column).Value = item.Cycle3ParticipantPercentage;
                    column++;
                    ws.Cell(i, column).Value = item.Cycle3Passed;
                    column++;
                    ws.Cell(i, column).Value = item.Cycle3PassedPercentage;
                    column++;
                    ws.Cell(i, column).Value = item.Cycle3Video;
                    column++;
                    ws.Cell(i, column).Value = item.Cycle3VideoPercentage;
                    column++;
                    ws.Cell(i, column).Value = item.Cycle3Certified;
                    column++;
                    ws.Cell(i, column).Value = item.Cycle3CertifiedPercentage;
                    column++;

                    points = JsonConvert.DeserializeObject<ReportKPITrackingPoints[]>(reports[i - 4].ResultSPV);

                    foreach (var point in points)
                    {
                        ws.Cell(i, column).Value = point.Point;
                        column++;
                        if (point.ModuleName != "Target")
                        {
                            ws.Cell(i, column).Value = point.Percentage;
                            column++;
                        }
                    }

                    points = JsonConvert.DeserializeObject<ReportKPITrackingPoints[]>(reports[i - 4].ResultBM);

                    foreach (var point in points)
                    {
                        ws.Cell(i, column).Value = point.Point;
                        column++;
                        if (point.ModuleName != "Target")
                        {
                            ws.Cell(i, column).Value = point.Percentage;
                            column++;
                        }
                    }
                    i++;
                }

                //ws.Columns(1, column - 1).AdjustToContents();

                //define table header range
                var rangeHeader = ws.Range(ws.Cell(1, 1), ws.Cell(3, column - 1));
                //set table header border style
                rangeHeader.Style.Border.OutsideBorder = XLBorderStyleValues.Medium;
                rangeHeader.Style.Border.InsideBorder = XLBorderStyleValues.Medium;
                rangeHeader.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                rangeHeader.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;

                //define table data range
                var dataRange = ws.Range(ws.Cell(4, 1), ws.Cell(reports.Count() + 3, column - 1));
                //set data table border style
                dataRange.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                dataRange.Style.Border.InsideBorder = XLBorderStyleValues.Thin;
                //set data table alignment
                dataRange.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
                dataRange.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
            }

            MemoryStream MS = new MemoryStream();
            wb.SaveAs(MS);
            MS.Position = 0;

            return MS;
        }

        public async Task<ActionResult> ExportExcelKPI2(GetKPIParameter model)
        {
            if (string.IsNullOrWhiteSpace(model.DealerID))
            {
                model.DealerID = "0";
            }

            if (string.IsNullOrWhiteSpace(model.AreaID))
            {
                model.AreaID = "0";
            }

            var reports = DB.GetKPITracking2(
                                model.TrainingID,
                                model.DealerID,
                                model.AreaID
                             ).ToList();

            var MS = await CreateExcelReportKPI2(reports);
            return File(MS, FormatDocumentEnum.ExcelFormat, "KPI.xlsx");
        }

        public async Task<MemoryStream> CreateExcelReportKPI2(List<ReportKPITracking2> reports)
        {
            //creating new workbook
            var wb = new XLWorkbook();

            //creating new worksheet
            var ws = wb.AddWorksheet("Sheet1");

            var dynamicColumn = "[]";
            if (reports.Count() > 0)
            {
                dynamicColumn = reports[0].ResultTotal;
            }

            ReportKPITrackingPositions[] positionColumns = JsonConvert.DeserializeObject<ReportKPITrackingPositions[]>(dynamicColumn);

            //create header
            ws.Range("A1:A4").Merge();
            ws.Range("B1:B4").Merge();
            ws.Range("C1:C4").Merge();

            ws.Cell("A1").Value = "Dealer";
            ws.Cell("B1").Value = "Outlet";
            ws.Cell("C1").Value = "Area";

            int column = 4;
            foreach (var position in positionColumns)
            {
                ws.Cell(1, column).Value = position.Position;
                var tempColumn = column;

                ReportKPITrackingPositionsResults[] pointColumns = JsonConvert.DeserializeObject<ReportKPITrackingPositionsResults[]>(position.Results);

                if (pointColumns.Length > 0)
                {
                    int pre = pointColumns.Where(x => x.TrainingTypeID == 1).Count();
                    int during = pointColumns.Where(x => x.TrainingTypeID == 2).Count();
                    int post = pointColumns.Where(x => x.TrainingTypeID == 3).Count();
                    int last = column + 1;

                    if (pre > 0)
                        ws.Cell(2, last).Value = "PRE";
                    if (pre > 1)
                        ws.Range(ws.Cell(2, last), ws.Cell(2, last + pre - 1)).Merge();

                    last += pre;

                    if (during > 0)
                        ws.Cell(2, last).Value = "During";
                    if (during > 1)
                        ws.Range(ws.Cell(2, last), ws.Cell(2, last + during - 1)).Merge();

                    last += during;

                    if (post > 0)
                        ws.Cell(2, last).Value = "POST";
                    if (post > 1)
                        ws.Range(ws.Cell(2, last), ws.Cell(2, last + post - 1)).Merge();
                }

                int plus = 0;
                foreach (var point in pointColumns)
                {
                    if (point.ModuleName == "Target")
                    {
                        ws.Cell(2, column).Value = "Target";
                        ws.Range(ws.Cell(2, column), ws.Cell(4, column)).Merge();
                        column++;
                        plus++;
                    }
                    else
                    {
                        if (point.ModuleType == "non-task" && point.IsPercent == "N")
                        {
                            ws.Cell(3, column).Value = point.ModuleName;
                            ws.Range(ws.Cell(3, column), ws.Cell(4, column + 1)).Merge();
                            column += 2;
                            plus += 2;
                        }
                        else if (point.ModuleType == "task" && point.IsPercent == "N")
                        {
                            ws.Cell(3, column).Value = point.ModuleName;
                            ws.Range(ws.Cell(3, column), ws.Cell(3, column + 3)).Merge();

                            ws.Cell(4, column).Value = "Participant";
                            ws.Range(ws.Cell(4, column), ws.Cell(4, column + 1)).Merge();

                            ws.Cell(4, column + 2).Value = "Passed";
                            ws.Range(ws.Cell(4, column + 2), ws.Cell(4, column + 3)).Merge();

                            column += 4;
                            plus += 4;
                        }
                        else if (point.ModuleType == "assesment" && point.IsPercent == "N")
                        {
                            ws.Cell(2, column).Value = "Assesment Presentation";
                            ws.Range(ws.Cell(2, column), ws.Cell(2, column + 3)).Merge();

                            ws.Cell(3, column).Value = "Participant";
                            ws.Range(ws.Cell(3, column), ws.Cell(4, column + 1)).Merge();

                            ws.Cell(3, column + 2).Value = "Passed";
                            ws.Range(ws.Cell(3, column + 2), ws.Cell(4, column + 3)).Merge();

                            column += 4;
                            plus += 4;
                        }
                        else if (point.ModuleType == "certificate" && point.IsPercent == "N")
                        {
                            ws.Cell(2, column).Value = point.ModuleName;
                            ws.Range(ws.Cell(2, column), ws.Cell(4, column + 1)).Merge();
                            column += 2;
                            plus += 2;
                        }
                    }
                }

                ws.Range(ws.Cell(1, tempColumn), ws.Cell(1, tempColumn + plus - 1)).Merge();
            }

            int i = 5;
            foreach (var item in reports)
            {
                ws.Cell("A" + i.ToString()).Value = item.Dealer;
                ws.Cell("B" + i.ToString()).Value = item.Outlet;
                ws.Cell("C" + i.ToString()).Value = item.Area;

                column = 4;

                positionColumns = JsonConvert.DeserializeObject<ReportKPITrackingPositions[]>(reports[i - 5].ResultTotal);

                foreach (var position in positionColumns)
                {
                    ws.Cell(i, column).Value = position.Position;
                    ReportKPITrackingPositionsResults[] pointColumns = JsonConvert.DeserializeObject<ReportKPITrackingPositionsResults[]>(position.Results);

                    foreach (var point in pointColumns)
                    {
                        ws.Cell(i, column).Value = point.ModuleValue;
                        column++;
                    }
                }
                
                i++;
            }

            //define table header range
            var rangeHeader = ws.Range(ws.Cell(1, 1), ws.Cell(4, column - 1));
            //set table header border style
            rangeHeader.Style.Border.OutsideBorder = XLBorderStyleValues.Medium;
            rangeHeader.Style.Border.InsideBorder = XLBorderStyleValues.Medium;
            rangeHeader.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            rangeHeader.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;

            //define table data range
            var dataRange = ws.Range(ws.Cell(5, 1), ws.Cell(reports.Count() + 4, column - 1));
            //set data table border style
            dataRange.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
            dataRange.Style.Border.InsideBorder = XLBorderStyleValues.Thin;
            //set data table alignment
            dataRange.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
            dataRange.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;

            MemoryStream MS = new MemoryStream();
            wb.SaveAs(MS);
            MS.Position = 0;

            return MS;
        }
    }
}
