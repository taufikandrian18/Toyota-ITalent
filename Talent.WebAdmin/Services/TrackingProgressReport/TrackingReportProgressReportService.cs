using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ClosedXML.Excel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Talent.Entities.DbQueryModels;
using Talent.Entities.Entities;
using Talent.WebAdmin.Enums;
using Talent.WebAdmin.Helpers;
using Talent.WebAdmin.Models;
using Talent.WebAdmin.Models.TrackingProgressReport;
using TAM.Talent.Commons.Core.Interfaces;

namespace Talent.WebAdmin.Services.TrackingProgressReport
{
    public class TrackingReportProgressReportService : Controller
    {
        private readonly TalentContext DB;
        private readonly ClaimHelper ClaimMan;
        IFileStorageService FileService;
        private readonly BlobService BlobService;

        public TrackingReportProgressReportService(TalentContext db, ClaimHelper claimHelper, IFileStorageService fileService, BlobService blobService)
        {
            this.DB = db;
            this.ClaimMan = claimHelper;
            this.BlobService = blobService;
            this.FileService = fileService;
        }

        public async Task<ActionResult> GetDataProgressTrackingReportJson(GetAccountParameterProgressTracking model)
        {
            var count = 0;
            try
            {
                if (model.Limit == 0)
                {
                    model.Limit = 100;
                }

                if (model.Page == 0)
                {
                    model.Page = 1;
                }

                model.Page = (model.Page - 1) * model.Limit;

                List<string> dealerList = new List<string>();

                if (!String.IsNullOrWhiteSpace(model.DealerId))
                {
                    string[] dealerSplit = model.DealerId.Split(',');

                    foreach (var sub in dealerSplit)
                    {
                        dealerList.Add(sub);
                    }
                }

                var getDealerId = DB.Dealers.Where(Q => dealerList.Contains(Q.DealerId)).Select(Q => Q.DealerId).ToList();

                if (dealerList.Count() > 0)
                {
                    //var query = DB.GetProgressTrackingData(model.ProgramTypeId,model.TrainingId, dealerList).AsQueryable();

                    var query = DB.GetProgressTrackingModelWithDealer(model.ProgramTypeId, model.TrainingId, dealerList).AsQueryable();

                    //if (model.ProgramTypeId != 0)
                    //{
                    //    query = query.Where(Q => Q.ProgramTypeId == model.ProgramTypeId).AsQueryable();
                    //}

                    //if (model.TrainingId != 0)
                    //{
                    //    query = query.Where(Q => Q.TrainingId == model.TrainingId).AsQueryable();
                    //}

                    if (!String.IsNullOrWhiteSpace(model.AreaId))
                    {
                        query = query.Where(Q => Q.AreaId == model.AreaId).AsQueryable();
                    }

                    if (!String.IsNullOrWhiteSpace(model.AreaSpecialistId))
                    {
                        query = query.Where(Q => Q.AreaSpecialistId == model.AreaSpecialistId).AsQueryable();
                    }

                    if (!String.IsNullOrWhiteSpace(model.EmployeeId))
                    {
                        query = query.Where(Q => Q.EmployeeId == model.EmployeeId).AsQueryable();
                    }

                    if (!String.IsNullOrWhiteSpace(model.LearningType))
                    {
                        if (model.LearningType == "Assesment")
                        {
                            query = query.Where(Q => Q.ModuleId == 0).AsQueryable();
                        }
                        if (model.LearningType == "Module")
                        {
                            query = query.Where(Q => Q.ModuleId != 0).AsQueryable();
                        }
                    }

                    if (!String.IsNullOrWhiteSpace(model.EmployeeName))
                    {
                        query = query.Where(Q => Q.EmployeeName.ToLower().Contains(model.EmployeeName.ToLower())).AsQueryable();
                    }

                    //if (!String.IsNullOrWhiteSpace(model.DealerId))
                    //{
                    //    query = query.Where(Q => Q.DealerId == model.DealerId).AsQueryable();
                    //}

                    //if (!String.IsNullOrWhiteSpace(model.DealerId))
                    //{
                    //    query = query.Where(Q => dealerList.Contains(Q.DealerId)).AsQueryable();
                    //}

                    if (!String.IsNullOrWhiteSpace(model.OutletID))
                    {
                        query = query.Where(Q => Q.OutletId == model.OutletID).AsQueryable();
                    }

                    count = query.Count();

                    var data = await query.Skip(model.Page).Take(model.Limit).ToListAsync();

                    if (data.Count() > 0)
                    {
                        var outputData = (from sl in data
                                          select new TrackingReportProgressReportModel
                                          {
                                              SetupModuleId = sl.SetupModuleId,
                                              EmployeeId = sl.EmployeeId,
                                              EmployeeName = sl.EmployeeName,
                                              OutletName = sl.OutletName,
                                              AreaName = sl.AreaName,
                                              AreaSpecialistName = sl.AreaSpecialistName,
                                              PositionName = sl.PositionName,
                                              DealerName = sl.DealerName,
                                              ProgramTypeName = sl.ProgramTypeName,
                                              LearningType = sl.Flagging,
                                              TrainingId = sl.TrainingId,
                                              CourseName = sl.CourseName,
                                              ModuleName = sl.ModuleName,
                                              AssesmentName = sl.AssesmentName,
                                              Batch = sl.Batch,
                                              FirstAccess = sl.FirstAccess,
                                              LastAccess = sl.LastAccess,
                                              TotalStudyTime = sl.TotalStudyTime,
                                              AverageScore = sl.AverageScore,
                                              HighestScore = sl.HighestScore,
                                              LatestScore = sl.LatestScore,
                                              FinalScore = sl.FinalScore,
                                              CompletionStatus = sl.CompletionStatus,
                                              MinimumScore = sl.MinimumScore,
                                              Attempts = sl.Attempts
                                          }).ToList();

                        return StatusCode(200, BaseResponse.ResponseOk(outputData, count));
                    }
                    else
                    {
                        var msg = new Message
                        {
                            En = "Your Progress Tracking Report Does Not Contains Data",
                            Id = "Laporan Progress Tracking Tidak Memuat Data"
                        };
                        return StatusCode(400, BaseResponse.BadRequest(msg));
                    }
                }
                else
                {
                    //nasional

                    //var query = DB.GetProgressTrackingDataWithoutDealerId(model.ProgramTypeId, model.TrainingId).AsQueryable();

                    var query = DB.GetProgressTrackingModelNational(model.ProgramTypeId, model.TrainingId).AsQueryable();

                    //if (model.ProgramTypeId != 0)
                    //{
                    //    query = query.Where(Q => Q.ProgramTypeId == model.ProgramTypeId).AsQueryable();
                    //}

                    //if (model.TrainingId != 0)
                    //{
                    //    query = query.Where(Q => Q.TrainingId == model.TrainingId).AsQueryable();
                    //}

                    if (!String.IsNullOrWhiteSpace(model.AreaId))
                    {
                        query = query.Where(Q => Q.AreaId == model.AreaId).AsQueryable();
                    }

                    if (!String.IsNullOrWhiteSpace(model.AreaSpecialistId))
                    {
                        query = query.Where(Q => Q.AreaSpecialistId == model.AreaSpecialistId).AsQueryable();
                    }

                    if (!String.IsNullOrWhiteSpace(model.EmployeeId))
                    {
                        query = query.Where(Q => Q.EmployeeId == model.EmployeeId).AsQueryable();
                    }

                    if (!String.IsNullOrWhiteSpace(model.LearningType))
                    {
                        if (model.LearningType == "Assesment")
                        {
                            query = query.Where(Q => Q.ModuleId == 0).AsQueryable();
                        }
                        if (model.LearningType == "Module")
                        {
                            query = query.Where(Q => Q.ModuleId != 0).AsQueryable();
                        }
                    }

                    if (!String.IsNullOrWhiteSpace(model.EmployeeName))
                    {
                        query = query.Where(Q => Q.EmployeeName.ToLower().Contains(model.EmployeeName.ToLower())).AsQueryable();
                    }

                    //if (!String.IsNullOrWhiteSpace(model.DealerId))
                    //{
                    //    query = query.Where(Q => Q.DealerId == model.DealerId).AsQueryable();
                    //}

                    if (!String.IsNullOrWhiteSpace(model.DealerId))
                    {
                        query = query.Where(Q => dealerList.Contains(Q.DealerId)).AsQueryable();
                    }

                    if (!String.IsNullOrWhiteSpace(model.OutletID))
                    {
                        query = query.Where(Q => Q.OutletId == model.OutletID).AsQueryable();
                    }

                    count = query.Count();

                    var data = await query.Skip(model.Page).Take(model.Limit).ToListAsync();

                    if (data.Count() > 0)
                    {
                        var outputData = (from sl in data
                                          select new TrackingReportProgressReportModel
                                          {
                                              SetupModuleId = sl.SetupModuleId,
                                              EmployeeId = sl.EmployeeId,
                                              EmployeeName = sl.EmployeeName,
                                              OutletName = sl.OutletName,
                                              AreaName = sl.AreaName,
                                              AreaSpecialistName = sl.AreaSpecialistName,
                                              PositionName = sl.PositionName,
                                              DealerName = sl.DealerName,
                                              ProgramTypeName = sl.ProgramTypeName,
                                              LearningType = sl.Flagging,
                                              TrainingId = sl.TrainingId,
                                              CourseName = sl.CourseName,
                                              ModuleName = sl.ModuleName,
                                              AssesmentName = sl.AssesmentName,
                                              Batch = sl.Batch,
                                              FirstAccess = sl.FirstAccess,
                                              LastAccess = sl.LastAccess,
                                              TotalStudyTime = sl.TotalStudyTime,
                                              AverageScore = sl.AverageScore,
                                              HighestScore = sl.HighestScore,
                                              LatestScore = sl.LatestScore,
                                              FinalScore = sl.FinalScore,
                                              CompletionStatus = sl.CompletionStatus,
                                              MinimumScore = sl.MinimumScore,
                                              Attempts = sl.Attempts
                                          }).ToList();

                        return StatusCode(200, BaseResponse.ResponseOk(outputData, count));
                    }
                    else
                    {
                        var msg = new Message
                        {
                            En = "Your Progress Tracking Report Does Not Contains Data",
                            Id = "Laporan Progress Tracking Tidak Memuat Data"
                        };
                        return StatusCode(400, BaseResponse.BadRequest(msg));
                    }
                }

            }
            catch (Exception x)
            {
                return StatusCode(500, BaseResponse.Error(null, x));
            }
        }

        public async Task<ActionResult> ExportExcelTrackingReportProgress(GetAccountParameterProgressTracking model, string filetype)
        {
            var count = 0;
            try
            {
                model.Page = (model.Page - 1) * model.Limit;

                List<string> dealerList = new List<string>();

                if (!String.IsNullOrWhiteSpace(model.DealerId))
                {
                    string[] dealerSplit = model.DealerId.Split(',');

                    foreach (var sub in dealerSplit)
                    {
                        dealerList.Add(sub);
                    }
                }

                if(dealerList.Count() > 0)
                {
                    //var query = DB.GetProgressTrackingData(model.ProgramTypeId,model.TrainingId, dealerList).AsQueryable();

                    var query = DB.GetProgressTrackingModelWithDealer(model.ProgramTypeId, model.TrainingId, dealerList).AsQueryable();

                    //if (model.ProgramTypeId != 0)
                    //{
                    //    query = query.Where(Q => Q.ProgramTypeId == model.ProgramTypeId).AsQueryable();
                    //}

                    //if (model.TrainingId != 0)
                    //{
                    //    query = query.Where(Q => Q.TrainingId == model.TrainingId).AsQueryable();
                    //}

                    if (!String.IsNullOrWhiteSpace(model.AreaId))
                    {
                        query = query.Where(Q => Q.AreaId == model.AreaId).AsQueryable();
                    }

                    if (!String.IsNullOrWhiteSpace(model.AreaSpecialistId))
                    {
                        query = query.Where(Q => Q.AreaSpecialistId == model.AreaSpecialistId).AsQueryable();
                    }

                    if (!String.IsNullOrWhiteSpace(model.EmployeeId))
                    {
                        query = query.Where(Q => Q.EmployeeId == model.EmployeeId.Trim()).AsQueryable();
                    }

                    if (!String.IsNullOrWhiteSpace(model.LearningType))
                    {
                        if (model.LearningType == "Assesment")
                        {
                            query = query.Where(Q => Q.ModuleId == 0).AsQueryable();
                        }
                        if (model.LearningType == "Module")
                        {
                            query = query.Where(Q => Q.ModuleId != 0).AsQueryable();
                        }
                    }

                    if (!String.IsNullOrWhiteSpace(model.EmployeeName))
                    {
                        query = query.Where(Q => Q.EmployeeName.ToLower().Contains(model.EmployeeName.ToLower())).AsQueryable();
                    }

                    //if (!String.IsNullOrWhiteSpace(model.DealerId))
                    //{
                    //    query = query.Where(Q => Q.DealerId == model.DealerId).AsQueryable();
                    //}

                    //if (!String.IsNullOrWhiteSpace(model.DealerId))
                    //{
                    //    query = query.Where(Q => dealerList.Contains(Q.DealerId)).AsQueryable();
                    //}

                    if (!String.IsNullOrWhiteSpace(model.OutletID))
                    {
                        query = query.Where(Q => Q.OutletId == model.OutletID).AsQueryable();
                    }

                    count = query.Count();

                    var data = query.ToList();

                    if (data.Count() > 0)
                    {
                        var MS = await CreateExcelProgressTrackingReport(data);

                        return File(MS, FormatDocumentEnum.ExcelFormat, "Summary_Learning_Analysis_Report." + filetype);
                    }
                    else
                    {
                        var msg = new Message
                        {
                            En = "Your Progress Tracking Report Does Not Contains Data",
                            Id = "Laporan Progress Tracking Tidak Memuat Data"
                        };
                        return StatusCode(400, BaseResponse.BadRequest(msg));
                    }
                }
                else
                {
                    //nasional

                    //var query = DB.GetProgressTrackingDataWithoutDealerId(model.ProgramTypeId, model.TrainingId).AsQueryable();

                    var query = DB.GetProgressTrackingModelNational(model.ProgramTypeId, model.TrainingId).AsQueryable();

                    //if (model.ProgramTypeId != 0)
                    //{
                    //    query = query.Where(Q => Q.ProgramTypeId == model.ProgramTypeId).AsQueryable();
                    //}

                    //if (model.TrainingId != 0)
                    //{
                    //    query = query.Where(Q => Q.TrainingId == model.TrainingId).AsQueryable();
                    //}

                    if (!String.IsNullOrWhiteSpace(model.AreaId))
                    {
                        query = query.Where(Q => Q.AreaId == model.AreaId).AsQueryable();
                    }

                    if (!String.IsNullOrWhiteSpace(model.AreaSpecialistId))
                    {
                        query = query.Where(Q => Q.AreaSpecialistId == model.AreaSpecialistId).AsQueryable();
                    }

                    if (!String.IsNullOrWhiteSpace(model.EmployeeId))
                    {
                        query = query.Where(Q => Q.EmployeeId == model.EmployeeId.Trim()).AsQueryable();
                    }

                    if (!String.IsNullOrWhiteSpace(model.LearningType))
                    {
                        if (model.LearningType == "Assesment")
                        {
                            query = query.Where(Q => Q.ModuleId == 0).AsQueryable();
                        }
                        if (model.LearningType == "Module")
                        {
                            query = query.Where(Q => Q.ModuleId != 0).AsQueryable();
                        }
                    }

                    if (!String.IsNullOrWhiteSpace(model.EmployeeName))
                    {
                        query = query.Where(Q => Q.EmployeeName.ToLower().Contains(model.EmployeeName.ToLower())).AsQueryable();
                    }

                    //if (!String.IsNullOrWhiteSpace(model.DealerId))
                    //{
                    //    query = query.Where(Q => Q.DealerId == model.DealerId).AsQueryable();
                    //}

                    if (!String.IsNullOrWhiteSpace(model.DealerId))
                    {
                        query = query.Where(Q => dealerList.Contains(Q.DealerId)).AsQueryable();
                    }

                    if (!String.IsNullOrWhiteSpace(model.OutletID))
                    {
                        query = query.Where(Q => Q.OutletId == model.OutletID).AsQueryable();
                    }

                    count = query.Count();

                    var data = query.ToList();

                    if (data.Count() > 0)
                    {
                        var MS = await CreateExcelProgressTrackingReport(data);

                        return File(MS, FormatDocumentEnum.ExcelFormat, "Summary_Learning_Analysis_Report." + filetype);
                    }
                    else
                    {
                        var msg = new Message
                        {
                            En = "Your Progress Tracking Report Does Not Contains Data",
                            Id = "Laporan Progress Tracking Tidak Memuat Data"
                        };
                        return StatusCode(400, BaseResponse.BadRequest(msg));
                    }
                }
            }
            catch (Exception x)
            {
                return StatusCode(500, BaseResponse.Error(null, x));
            }
        }

        public async Task<MemoryStream> CreateExcelProgressTrackingReport(List<GetProgressTrackingModel> progressTrackingReportList)
        {
            //creating new workbook
            var wb = new XLWorkbook();

            //creating new worksheet
            var ws = wb.AddWorksheet("Sheet1");

            //create header
            ws.Cell("A1").Value = "Employee Id";
            ws.Cell("B1").Value = "Employee Name";
            ws.Cell("C1").Value = "Outlet Name";
            ws.Cell("D1").Value = "Area Name";
            ws.Cell("E1").Value = "Area Specialist Name";
            ws.Cell("F1").Value = "Position Name";
            ws.Cell("G1").Value = "Dealer Name";
            ws.Cell("H1").Value = "Program Type Name";
            ws.Cell("I1").Value = "Learning Type";
            ws.Cell("J1").Value = "Course Name";
            ws.Cell("K1").Value = "Module Name";
            ws.Cell("L1").Value = "Assesment Name";
            ws.Cell("M1").Value = "Batch";
            ws.Cell("N1").Value = "First Access";
            ws.Cell("O1").Value = "Last Study";
            ws.Cell("P1").Value = "Total Study Time";
            ws.Cell("Q1").Value = "Average Score";
            ws.Cell("R1").Value = "Highest Score";
            ws.Cell("S1").Value = "Latest Score";
            ws.Cell("T1").Value = "Final Score";
            ws.Cell("U1").Value = "Completion Status";
            ws.Cell("V1").Value = "Minimum Score";
            ws.Cell("W1").Value = "Attempts";

            //table data
            int i = 2;
            foreach (var item in progressTrackingReportList)
            {
                ws.Cell("A" + i.ToString()).Value = item.EmployeeId.Trim();
                ws.Cell("B" + i.ToString()).Value = item.EmployeeName;
                ws.Cell("C" + i.ToString()).Value = item.OutletName;
                ws.Cell("D" + i.ToString()).Value = item.AreaName;
                ws.Cell("E" + i.ToString()).Value = item.AreaSpecialistName;
                ws.Cell("F" + i.ToString()).Value = item.PositionName;
                ws.Cell("G" + i.ToString()).Value = item.DealerName;
                ws.Cell("H" + i.ToString()).Value = item.ProgramTypeName;
                ws.Cell("I" + i.ToString()).Value = item.Flagging;
                ws.Cell("J" + i.ToString()).Value = item.CourseName;
                ws.Cell("K" + i.ToString()).Value = item.ModuleName;
                ws.Cell("L" + i.ToString()).Value = item.AssesmentName;
                ws.Cell("M" + i.ToString()).Value = item.Batch;
                ws.Cell("N" + i.ToString()).Value = item.FirstAccess;
                ws.Cell("O" + i.ToString()).Value = item.LastAccess;
                ws.Cell("P" + i.ToString()).Value = item.TotalStudyTime;
                ws.Cell("Q" + i.ToString()).Value = item.AverageScore;
                ws.Cell("R" + i.ToString()).Value = item.HighestScore;
                ws.Cell("S" + i.ToString()).Value = item.LatestScore;
                ws.Cell("T" + i.ToString()).Value = item.FinalScore;
                ws.Cell("U" + i.ToString()).Value = item.CompletionStatus;
                ws.Cell("V" + i.ToString()).Value = item.MinimumScore;
                ws.Cell("W" + i.ToString()).Value = item.Attempts;
                i++;
            }

            //adjust column width
            ws.Columns(1, 10).AdjustToContents();

            //define table header range
            var rangeHeader = ws.Range("A1:W1");
            //set table header border style
            rangeHeader.Style.Border.OutsideBorder = XLBorderStyleValues.Medium;
            rangeHeader.Style.Border.InsideBorder = XLBorderStyleValues.Medium;
            //rangeHeader.Style.Fill.BackgroundColor = XLColor.Yellow;

            //define table data range
            var dataRange = ws.Range("A2" + ":W" + (i - 1).ToString());
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

        public async Task<ActionResult> ExportExcelTrackingReportDetailProgressJson(int trainingId, int setupModuleId, string employeeId, string flagging)
        {
            var count = 0;
            try
            {
                var MS = new MemoryStream();

                if (flagging.Trim() == "Assesment")
                {
                    var query = DB.getDetailProgressTrackingAssesment(trainingId, setupModuleId, employeeId).AsQueryable();

                    count = query.Count();

                    var data = await query.OrderBy(Q => Q.AttemptedCount).ThenBy(Q => Q.SkillCheckName).ToListAsync();

                    if(data.Count() > 0)
                    {
                        return StatusCode(200, BaseResponse.ResponseOk(data,count));
                    }
                    else
                    {
                        var msg = new Message
                        {
                            En = "Your Progress Tracking Report Detail Does Not Contains Data",
                            Id = "Laporan Progress Tracking Tidak Memuat Data"
                        };
                        return StatusCode(400, BaseResponse.BadRequest(msg));
                    }
                }

                else if (flagging.Trim() == "Module")
                {
                    var query = DB.getDetailProgressTrackingModule(trainingId, setupModuleId, employeeId).AsQueryable();

                    count = query.Count();

                    var data = await query.ToListAsync();

                    if(data.Count() > 0)
                    {
                        foreach(var item in data)
                        {
                            if(item.QuestionType.Trim() == "File Upload")
                            {
                                var fileName = this.DB.Blobs.Where(Q => Q.BlobId.ToString() == item.Answers.Trim()).FirstOrDefault();
                                if(fileName != null)
                                {
                                    item.Answers = await FileService.GetFileAsync(fileName.BlobId.ToString(), fileName.Mime);
                                }
                            }
                        }
                        return StatusCode(200, BaseResponse.ResponseOk(data,count));
                    }
                    else
                    {
                        var msg = new Message
                        {
                            En = "Your Progress Tracking Report Detail Does Not Contains Data",
                            Id = "Laporan Progress Tracking Tidak Memuat Data"
                        };
                        return StatusCode(400, BaseResponse.BadRequest(msg));
                    }
                }
                else
                {
                    var msg = new Message
                    {
                        En = "Your Progress Tracking Report Need Flagging Data",
                        Id = "Laporan Progress Tracking Membutuhkan Data Flagging"
                    };
                    return StatusCode(400, BaseResponse.BadRequest(msg));
                }
            }
            catch (Exception x)
            {
                return StatusCode(500, BaseResponse.Error(null, x));
            }
        }

        public async Task<ActionResult> ExportExcelTrackingReportDetailProgress(int trainingId,int setupModuleId,string employeeId, string filetype, string flagging)
        {
            try
            {
                var MS = new MemoryStream();

                if(flagging.Trim() == "Assesment")
                {
                    var query = DB.getDetailProgressTrackingAssesment(trainingId, setupModuleId, employeeId).AsQueryable();

                    var data = query.ToList();

                    MS = await CreateExcelProgressTrackingDetailReportAssesment(data);
                }

                if(flagging.Trim() == "Module")
                {
                    var query = DB.getDetailProgressTrackingModule(trainingId,setupModuleId,employeeId).AsQueryable();

                    var data = query.ToList();

                    MS = await CreateExcelProgressTrackingDetailReport(data);
                }

                return File(MS, FormatDocumentEnum.ExcelFormat, "Detail_Learning_Analysis_Report_" + flagging.Trim() + "." + filetype);
            }
            catch (Exception x)
            {
                return StatusCode(500, BaseResponse.Error(null, x));
            }
        }

        public async Task<MemoryStream> CreateExcelProgressTrackingDetailReport(List<GetProgressTrackingDetailModel> progressTrackingReportDetailList)
        {
            //creating new workbook
            var wb = new XLWorkbook();

            //creating new worksheet
            var ws = wb.AddWorksheet("Sheet1");

            //create header
            ws.Cell("A1").Value = "Program Type Name";
            ws.Cell("B1").Value = "Employee Id";
            ws.Cell("C1").Value = "Employee Name";
            ws.Cell("D1").Value = "Outlet Name";
            ws.Cell("E1").Value = "Area Name";
            ws.Cell("F1").Value = "Area Specialist Name";
            ws.Cell("G1").Value = "Position Name";
            ws.Cell("H1").Value = "Dealer Name";
            ws.Cell("I1").Value = "Course Name";
            ws.Cell("J1").Value = "Module Name";
            ws.Cell("K1").Value = "Attempted Count";
            ws.Cell("L1").Value = "Attempted Date";
            ws.Cell("M1").Value = "Score";
            ws.Cell("N1").Value = "Minimum Score";
            ws.Cell("O1").Value = "Total Score";
            ws.Cell("P1").Value = "QuestionType";
            ws.Cell("Q1").Value = "Questions";
            ws.Cell("R1").Value = "Answers";

            //table data
            int i = 2;
            foreach (var item in progressTrackingReportDetailList)
            {
                ws.Cell("A" + i.ToString()).Value = item.ProgramTypeName;
                ws.Cell("B" + i.ToString()).Value = item.EmployeeId.Trim();
                ws.Cell("C" + i.ToString()).Value = item.EmployeeName;
                ws.Cell("D" + i.ToString()).Value = item.OutletName;
                ws.Cell("E" + i.ToString()).Value = item.AreaName;
                ws.Cell("F" + i.ToString()).Value = item.AreaSpecialistName;
                ws.Cell("G" + i.ToString()).Value = item.PositionName;
                ws.Cell("H" + i.ToString()).Value = item.DealerName;
                ws.Cell("I" + i.ToString()).Value = item.CourseName;
                ws.Cell("J" + i.ToString()).Value = item.ModuleName;
                ws.Cell("K" + i.ToString()).Value = item.AttemptedCount;
                ws.Cell("L" + i.ToString()).Value = item.AttemptedDate;
                ws.Cell("M" + i.ToString()).Value = item.Score;
                ws.Cell("N" + i.ToString()).Value = item.MinimumScore;
                ws.Cell("O" + i.ToString()).Value = item.TotalScore;
                ws.Cell("P" + i.ToString()).Value = item.QuestionType;
                ws.Cell("Q" + i.ToString()).Value = item.Questions;
                if(item.QuestionType.Trim() == "File Upload")
                {
                    //var fileName = this.DB.Blobs.Where(Q => Q.BlobId.ToString() == item.Answers.Trim()).Select(Q => Q.Name).FirstOrDefault();
                    //item.Answers = await FileService.GetFileAsync(fileName, fileName.Split('.')[1]);
                    var fileName = this.DB.Blobs.Where(Q => Q.BlobId.ToString() == item.Answers.Trim()).FirstOrDefault();
                    if(fileName != null)
                    {
                        item.Answers = await FileService.GetFileAsync(fileName.BlobId.ToString(), fileName.Mime);
                        ws.Cell("R" + i.ToString()).Value = item.Answers;
                    }
                    else
                    {
                        ws.Cell("R" + i.ToString()).Value = "";
                    }
                }
                else
                {
                    ws.Cell("R" + i.ToString()).Value = item.Answers;
                }
                i++;
            }

            //adjust column width
            ws.Columns(1, 10).AdjustToContents();

            //define table header range
            var rangeHeader = ws.Range("A1:R1");
            //set table header border style
            rangeHeader.Style.Border.OutsideBorder = XLBorderStyleValues.Medium;
            rangeHeader.Style.Border.InsideBorder = XLBorderStyleValues.Medium;
            //rangeHeader.Style.Fill.BackgroundColor = XLColor.Yellow;

            //define table data range
            var dataRange = ws.Range("A2" + ":R" + (i - 1).ToString());
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

        public async Task<MemoryStream> CreateExcelProgressTrackingDetailReportAssesment(List<GetProgressTrackingDetailAssesment> progressTrackingReportDetailList)
        {
            //creating new workbook
            var wb = new XLWorkbook();

            //creating new worksheet
            var ws = wb.AddWorksheet("Sheet1");

            //create header
            ws.Cell("A1").Value = "Program Type Name";
            ws.Cell("B1").Value = "Employee Id";
            ws.Cell("C1").Value = "Employee Name";
            ws.Cell("D1").Value = "Outlet Name";
            ws.Cell("E1").Value = "Area Name";
            ws.Cell("F1").Value = "Area Specialist Name";
            ws.Cell("G1").Value = "Position Name";
            ws.Cell("H1").Value = "Dealer Name";
            ws.Cell("I1").Value = "Course Name";
            ws.Cell("J1").Value = "Assesment Name";
            ws.Cell("K1").Value = "Attempted Count";
            ws.Cell("L1").Value = "Attempted Date";
            ws.Cell("M1").Value = "Score";
            ws.Cell("N1").Value = "Minimum Score";
            ws.Cell("O1").Value = "Total Score";
            ws.Cell("P1").Value = "Skill Check Name";

            //table data
            int i = 2;
            foreach (var item in progressTrackingReportDetailList)
            {
                ws.Cell("A" + i.ToString()).Value = item.ProgramTypeName;
                ws.Cell("B" + i.ToString()).Value = item.EmployeeId.Trim();
                ws.Cell("C" + i.ToString()).Value = item.EmployeeName;
                ws.Cell("D" + i.ToString()).Value = item.OutletName;
                ws.Cell("E" + i.ToString()).Value = item.AreaName;
                ws.Cell("F" + i.ToString()).Value = item.AreaSpecialistName;
                ws.Cell("G" + i.ToString()).Value = item.PositionName;
                ws.Cell("H" + i.ToString()).Value = item.DealerName;
                ws.Cell("I" + i.ToString()).Value = item.CourseName;
                ws.Cell("J" + i.ToString()).Value = item.AssesmentName;
                ws.Cell("K" + i.ToString()).Value = item.AttemptedCount;
                ws.Cell("L" + i.ToString()).Value = item.AttemptedDate;
                ws.Cell("M" + i.ToString()).Value = item.Score;
                ws.Cell("N" + i.ToString()).Value = item.MinimumScore;
                ws.Cell("O" + i.ToString()).Value = item.TotalScore;
                ws.Cell("P" + i.ToString()).Value = item.SkillCheckName;
                i++;
            }

            //adjust column width
            ws.Columns(1, 10).AdjustToContents();

            //define table header range
            var rangeHeader = ws.Range("A1:P1");
            //set table header border style
            rangeHeader.Style.Border.OutsideBorder = XLBorderStyleValues.Medium;
            rangeHeader.Style.Border.InsideBorder = XLBorderStyleValues.Medium;
            //rangeHeader.Style.Fill.BackgroundColor = XLColor.Yellow;

            //define table data range
            var dataRange = ws.Range("A2" + ":P" + (i - 1).ToString());
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
