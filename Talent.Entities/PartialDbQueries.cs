using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using Talent.Entities.DbQueryModels;
using System.Data;
using System.Diagnostics;

namespace Talent.Entities.Entities
{
	public partial class TalentContext
	{
		public virtual DbQuery<TaskTotalScore> TaskTotalScore { get; set; }
		public virtual DbQuery<DashboardTop5TopicData> DashboardTop5TopicData { get; set; }
		public virtual DbQuery<DashboardTop5LearningData> DashboardTop5LearningData { get; set; }
		public virtual DbQuery<DashboardTop5CoachData> DashboardTop5CoachData { get; set; }
		public virtual DbQuery<DashboardTop5RewardTypeData> DashboardTop5RewardTypeData { get; set; }
		public virtual DbQuery<DashboardTop5EventData> DashboardTop5EventData { get; set; }
		public virtual DbQuery<DashboardNPSReportData> DashboardNPSReportData { get; set; }
		public virtual DbQuery<ApprovalTraining> ApprovalTraining { get; set; }

		public virtual DbQuery<UserSideCourseData> UserSideCourseData { get; set; }
		public virtual DbQuery<PrivilegePageMappingData> PrivilegePageMappingData { get; set; }
		public virtual DbQuery<TrainingProcess> TrainingProcess { get; set; }
		public virtual DbQuery<CourseOverviewQueryModel> CourseOverviewQuery { get; set; }
		public virtual DbQuery<SetupModuleOverviewQueryModel> SetupModuleOverviewQuery { get; set; }
		public virtual DbQuery<GetLearningBadges> LearningBadgesQuery { set; get; }
		public virtual DbQuery<GetModuleContent> ModuleContents { get; set; }
		public virtual DbQuery<CourseOverviewQueryWithoutElModel> ModuleContentsWithoutEl { get; set; }
		public virtual DbQuery<CourseOverviewQueryWithoutElModel> ModuleContentsWithoutElForSetupModule { get; set; }
		public virtual DbQuery<StagingDealerEmployeeDistinctIdQueryModel> StagingEmployeeDistinctIds { get; set; }
		public virtual DbQuery<StagingDealerEmployeeDistinctSupervisorIdQueryModel> StagingEmployeeDistinctSupervisorIds { get; set; }
		public virtual DbQuery<GetContinuedLearningModel> ContentContinuedLearningModels { get; set; }
		public virtual DbQuery<GetRecommendedLearningModel> ContentRecommendedLearningModel { get; set; }
		public virtual DbQuery<GetPopularLearningModel> ContentPopularLearningModel { get; set; }
		public virtual DbQuery<GetPopularLearningModel> ContentLatestLearningModel { get; set; }
		public virtual DbQuery<LearningViewModel> LearningSearch { get; set; }
		public virtual DbQuery<EventsModel> EventModel { get; set; }
		public virtual DbQuery<EventSearchModel> EventSearchModel { get; set; }
		public virtual DbQuery<TotalActiveUser> TotalActiveUser { get; set; }
		public virtual DbQuery<AverageAccessTime> AverageAccessTime { get; set; }
		public virtual DbQuery<GetUserSideCourseLikePeopleListModel> UserSideCourseLikePeopleListModel { get; set; }
		public virtual DbQuery<GetUserSidePeopleWhoTookTheCourseListModel> UserSidePeopleWhoTookTheCourseListModel { get; set; }
		public virtual DbQuery<GetAllLearningModel> AllLearningQuery { get; set; }
		public virtual DbQuery<GetAllLearningUpdateModel> AllLearningQueryUpdate { get; set; }
		public virtual DbQuery<GetListDoubleDataModel> AllDoubleDataByTraining { get; set; }
		public virtual DbQuery<GetListDoubleModuleDataModel> AllDoubleDataModuleByTaskAnswerId { get; set; }
		public virtual DbQuery<GetListAssesmentDoubleModel> AllAssesmentDoubleData { get; set; }
		public virtual DbQuery<GetListModuleDoubleModel> AllModuleDoubleData { get; set; }
		public virtual DbQuery<GetProgressTrackingModel> AllProgressTracking { get; set; }
		public virtual DbQuery<GetProgressTrackingDetailModel> AllProgressTrackingDetailModule { get; set; }
		public virtual DbQuery<GetProgressTrackingDetailAssesment> AllProgressTrackingDetailAssesment { get; set; }
		public virtual DbQuery<GetUserSideRankModel> GenerateRank { get; set; }
		public virtual DbQuery<SuperiorEmployee> GetSuperiorEmployees { get; set; }
		public virtual DbQuery<ReportNPSModel> ReportNPS { get; set; }
		public virtual DbQuery<TrainingScoreReportViewQueryModel> GetTrainingScoreReport { get; set; }
		public virtual DbQuery<SurveyReport> GetSurveyReports { get; set; }
		public virtual DbQuery<SurveyReportDetailModel> GetSurveyReportDetail { get; set; }
		public virtual DbQuery<TrainingScoreReportDownloadQueryModel> GetTrainingScoreReportDownload { get; set; }
		public virtual DbQuery<LearningHistoriesQueryModel> GetLearningHistories { get; set; }
		public virtual DbQuery<CoachSearchQueryModel> GetCoachSearch { get; set; }
		public virtual DbQuery<EmployeePositionAggregateQueryModel> GetEmployeePositionAggregate { get; set; }
		public virtual DbQuery<PeopleSearchQueryModel> GetPeopleSearch { get; set; }
		public virtual DbQuery<TaskScoreQueryModel> GetTaskScore { get; set; }
		public virtual DbQuery<ModuleScores> GetModuleScores { get; set; }
		public virtual DbQuery<PersonalMappingModel> GetPersonalMappings { get; set; }
		public virtual DbQuery<DashboardEvaluatedCompetencyMappingModel> GetEvaluatedCompetencyMappings { get; set; }
		public virtual DbQuery<SetupModuleCalculate> GetSetupModuleCalculate { get; set; }
		public virtual DbQuery<PositionCoachSearchQueryModel> GetPositionCoachSearch { get; set; }
		public virtual DbQuery<TopicCoachSearchQueryModel> GetTopicCoachSearch { get; set; }
		public virtual DbQuery<EbadgeCoachSearchQueryModel> GetEbadgeCoachSearch { get; set; }
		public virtual DbQuery<TeamMappingDemandedModel> GetTeamMappingDemanded { get; set; }
		public virtual DbQuery<TeamMappingEvaluatedModel> GetTeamMappingEvaluated { get; set; }
		public virtual DbQuery<WeightedModel> GetWeightedModel { get; set; }
		public virtual DbQuery<ReportCompetencyTracking> ReportCompetencyTracking { get; set; }
		public virtual DbQuery<ReportKPITracking> ReportKPITracking { get; set; }
		public virtual DbQuery<ReportKPITracking2> ReportKPITracking2 { get; set; }
		public virtual DbQuery<GetTrainingIdNonTask> TrainingIdNonTasks { get; set; }
		public virtual DbQuery<GetSkillCheckCountByAssesmentIdModel> SkillCheckCountByAssesmentIds { get; set; }
		public virtual DbQuery<EmployeeByCoach> EmployeeByCoach { get; set; }
		public virtual DbQuery<GetCountTrainingQueryModel> GetCountTrainingQuery { get; set; }
		public virtual DbQuery<GetCountTrainingQueryModel> GetCountTrainingContinueQuery { get; set; }
		public virtual DbQuery<GetDataMyCoachDetail> GetDataMyCoachDetail { get; set; }




		/// <summary>
		/// Get task's total score data.
		/// </summary>
		/// <returns>Return <seealso cref="IQueryable{TaskTotalScore}"/> object.</returns>
		public IQueryable<TaskTotalScore> GetTaskTotalScore()
		{
			// Remove this if not needed.
			var countryName = "Japan";

			// Sample query, change it according to your needs.
			var query = this
				.TaskTotalScore
				.FromSql($@"
SELECT 
	me.reference_number AS ReferenceNumber,
	c.name AS CountryName,
    me.clause AS Clause,
    GROUP_CONCAT(a.name SEPARATOR ', ') AS AgencyNames,
    me.case_title AS CaseTitle,
    me.mla_extradition_status_case_id AS StatusCaseId
FROM mla_extradition me
	JOIN country c ON me.country_id = c.country_id
	LEFT JOIN mla_extradition_agency mea ON me.reference_number = mea.reference_number
    LEFT JOIN agency a ON mea.agency_id = a.agency_id
WHERE c.name = { countryName }
GROUP BY me.reference_number,
	c.name,
    me.clause,
    me.case_title,
    me.mla_extradition_status_case_id ");

			return query;
		}

		/// <summary>
		/// Get Top 5 Topics for dashboard
		/// </summary>
		/// <returns></returns>
		public IQueryable<DashboardTop5TopicData> GetTop5Topic()
		{
			var query = this
				.DashboardTop5TopicData
				.FromSql($@"
                    SELECT TOP 5 
	                    t.TopicName,
	                    COUNT(ei.TopicId) AS Total
                    FROM EmployeeInterests ei 
	                    JOIN Topics t ON ei.TopicId = t.TopicId
                    GROUP BY ei.TopicId, t.TopicName
                    ORDER BY Total DESC");

			return query;
		}

		/// <summary>
		/// Get Top 5 Learning for dashboard
		/// </summary>
		/// <returns></returns>
		public IQueryable<DashboardTop5LearningData> GetTop5Learning()
		{
			var query = this
				.DashboardTop5LearningData
				.FromSql($@"
                SELECT TOP 5
					c.CourseName,
					CAST(AVG(tr.RatingScore * 1.0) AS FLOAT) AS Rating
					FROM TrainingRatings tr 
					JOIN Trainings t ON tr.TrainingId = t.TrainingId
					JOIN Courses c ON t.CourseId = c.CourseId
				WHERE t.isDeleted = 0
				GROUP BY c.CourseId, c.CourseName, t.CreatedAt 
				ORDER BY Rating DESC");

			return query;
		}

		public IQueryable<GetTrainingIdNonTask> GetTrainingIdNonTaskData(int trainingId, string employeeId)
		{
			var query = this
				.TrainingIdNonTasks
				.FromSql($@"
				SELECT dbo.Trainings.TrainingId, COUNT(dbo.EnrollLearningTimes.SetupModuleId) AS JumlahSetupModule
				FROM dbo.EnrollLearningTimes INNER JOIN
				dbo.EnrollLearnings ON dbo.EnrollLearningTimes.EnrollLearningId = dbo.EnrollLearnings.EnrollLearningId INNER JOIN
				dbo.Trainings ON dbo.EnrollLearnings.TrainingId = dbo.Trainings.TrainingId
				WHERE (dbo.Trainings.TrainingId = { trainingId }) AND (dbo.EnrollLearnings.EmployeeId = { employeeId })
				GROUP BY dbo.Trainings.TrainingId, dbo.Enrolllearnings.EmployeeId
			");
			return query;
		}

		public IQueryable<GetSkillCheckCountByAssesmentIdModel> GetSkillCheckByAssesmentData(int trainingId, string employeeId, string assesmentId)
		{
			var query = this
				.SkillCheckCountByAssesmentIds
				.FromSql($@"
					select t.trainingid, count(la.skillcheckGUID), el.employeeid
					from trainings t 
					join enrolllearnings el on el.trainingid = t.trainingid
					join enrolllearningtimes elt on elt.enrolllearningid = el.enrolllearningid
					join setupmodules sm on sm.setupmoduleid = elt.setupmoduleid
					join liveassesmentskillchecks la on la.trainingid = t.trainingid
					join skillchecks sk on sk.GUID = la.skillcheckGUID
					group by t.trainingid, la.skillcheckGUID, el.employeeid
					having t.trainingid = { trainingId } and el.employeeid = { employeeId }
			");
			return query;
		}

		public IQueryable<GetProgressTrackingModel> GetProgressTrackingModelNational(int programTypeId, int trainingId)
        {
			var query = this
				.AllProgressTracking
				.FromSql($@"
                    select
					* from dbo.Flat_Report_Analysis_National
					where ProgramTypeId = { programTypeId } and TrainingId = { trainingId }
                ");

			return query;
		}

		public IQueryable<GetProgressTrackingModel> GetProgressTrackingModelWithDealer(int programTypeId, int trainingId, List<string> dealerId)
        {
			var query = $@"select
			* from dbo.Flat_Report_Analysis_National
			where ProgramTypeId = { programTypeId } and TrainingId = { trainingId } ";
			if (dealerId != null)
			{
				var contains = "'" + string.Join("','", dealerId) + "'";
				query = query + $@"AND n.DealerId in ({contains})";
			}

			var queryTest = (RawSqlString)query;

			var generateQuery = this
				.AllProgressTracking
				.FromSql(queryTest);

			return generateQuery;
		}



		public IQueryable<GetProgressTrackingModel> GetProgressTrackingDataWithoutDealerId(int programTypeId, int trainingId)
		{
			var query = this
				.AllProgressTracking
				.FromSql($@"
                    select
					e.SetupModuleId,
					g.ProgramTypeId,
					o.ProgramTypeName,
					b.EmployeeId,
					b.EmployeeName,
					(SELECT CASE WHEN (k.OutletId is null) 
						then 
							(SELECT CASE WHEN (r.IsTamPeople = 1) 
							then 
								'260' 
							else 
								k.OutletId 
							end) 
						else 
							k.OutletId 
					end) as OutletId,
					(SELECT CASE WHEN (k.OutletId is null) 
						then 
							(SELECT CASE WHEN (r.IsTamPeople = 1) 
							then 
								'TAM Workshop Sunter II' 
							else 
								k.Name 
							end)
						else 
							k.Name 
					end) as OutletName,
					(SELECT CASE WHEN (k.OutletId is null) 
						then 
							(SELECT CASE WHEN (r.IsTamPeople = 1) 
							then 
								(select ot.AreaId from dbo.Outlets ot where ot.OutletId = '260') 
							else 
								p.AreaId 
							end)
						else 
							p.AreaId 
					end) as AreaId,
					(SELECT CASE WHEN (k.OutletId is null) 
						then 
							(SELECT CASE WHEN (r.IsTamPeople = 1) 
							then 
								'TAM'
							else 
								p.Name 
							end)
						else 
							p.Name 
					end) as AreaName,
					s.AreaSpecialistId,
					s.AreaSpecialistName,
					r.PositionId,
					r.PositionName,
					(SELECT CASE WHEN (k.OutletId is null) 
						then 
							(SELECT CASE WHEN (r.IsTamPeople = 1) 
							then 
								'38'
							else 
								n.DealerId 
							end)
						else 
							n.DealerId 
					end) as DealerId,
					(SELECT CASE WHEN (k.OutletId is null) 
						then 
							(SELECT CASE WHEN (r.IsTamPeople = 1) 
							then 
								'TOYOTA ASTRA MOTOR' 
							else 
								n.DealerName 
							end)
						else 
							n.DealerName 
					end) as DealerName,
					(SELECT CASE when(e.ModuleId is not null) then 'Module' else 'Assesment' END ) as Flagging,
					c.TrainingId,
					g.CourseName,
					COALESCE(e.ModuleId,0) as ModuleId,
					coalesce(i.ModuleName,'') as ModuleName,
					e.AssesmentId,
					coalesce(j.Name,'') as AssesmentName,
					CONVERT(varchar,max(c.Batch),0) as Batch,
					CONVERT(varchar,min(f.StartTime),0) as FirstAccess,
					CONVERT(varchar,max(f.EndTime),0) as LastAccess,
					max(convert(varchar(5),DateDiff(s, f.StartTime, f.EndTime)/3600)+':'+convert(varchar(5),DateDiff(s, f.StartTime, f.EndTime)%3600/60)+':'+convert(varchar(5),(DateDiff(s, f.StartTime, f.EndTime)%60))) as TotalStudyTime,
					--CAST(SUM(h.FinalScore) / COUNT(h.FinalScore) AS DECIMAL(16,2)) as AverageScore,
					--CAST(max(h.FinalScore) AS DECIMAL(16,2)) as HighestScore,
					(SELECT CASE when(e.ModuleId is not null) 
						then 
							(SELECT CASE WHEN ((SELECT top 1 taResult.TaskAnswerId FROM dbo.TaskAnswers AS taResult WHERE taResult.SetupModuleId = e.SetupModuleId and taResult.TrainingId = c.TrainingId and taResult.EmployeeId = b.EmployeeId) is not null) 
							then 
								CAST((select CONVERT(DECIMAL(18,2),ROUND(AVG(taScore.Score + 0.0),2)) from (select tad.Attempts as Attempts ,SUM(tad.Score) as Score from dbo.TaskAnswers ta join dbo.TaskAnswerDetails tad on ta.TaskAnswerId = tad.TaskAnswerId where ta.SetupModuleId = e.SetupModuleId and ta.EmployeeId = b.EmployeeId and ta.TrainingId = c.TrainingId group by tad.Attempts) taScore ) as varchar(10)) 
							ELSE 
								'N/A' 
							END)
						else
							(SELECT CASE WHEN ((SELECT top 1 laResult.GUID FROM dbo.LiveAssesmentSkillChecks AS laResult WHERE laResult.EmployeeGUID = b.EmployeeId and laResult.AssesmentGUID = e.AssesmentId and laResult.TrainingId = c.TrainingId) is not null)
							THEN
								CAST((select CONVERT(DECIMAL(18,2),ROUND(AVG(laScore.Score + 0.0),2)) from (select la.Attempts as Attempts ,SUM(la.Score) / (select count(*) from dbo.AssesmentSkillChecks where AssesmentGUID = e.AssesmentId) as Score from dbo.LiveAssesmentSkillChecks la where la.EmployeeGUID = b.EmployeeId and la.AssesmentGUID = e.AssesmentId and la.TrainingId = c.TrainingId group by la.Attempts) laScore ) as varchar(10))
							ELSE
								'N/A'
							END) 
					END) as AverageScore,
					(SELECT CASE when(e.ModuleId is not null) 
						then 
							(SELECT CASE WHEN ((SELECT top 1 taResult.TaskAnswerId FROM dbo.TaskAnswers AS taResult WHERE taResult.SetupModuleId = e.SetupModuleId and taResult.TrainingId = c.TrainingId and taResult.EmployeeId = b.EmployeeId) is not null) 
							then 
								CAST((select CONVERT(DECIMAL(18,2),MAX(taScore.Score)) from (select tad.Attempts as Attempts ,SUM(tad.Score) as Score from dbo.TaskAnswers ta join dbo.TaskAnswerDetails tad on ta.TaskAnswerId = tad.TaskAnswerId where ta.SetupModuleId = e.SetupModuleId and ta.EmployeeId = b.EmployeeId and ta.TrainingId = c.TrainingId group by tad.Attempts) taScore) as varchar(10)) 
							ELSE 
								'N/A' 
							END)
						else
							(SELECT CASE WHEN ((SELECT top 1 laResult.GUID FROM dbo.LiveAssesmentSkillChecks AS laResult WHERE laResult.EmployeeGUID = b.EmployeeId and laResult.AssesmentGUID = e.AssesmentId and laResult.TrainingId = c.TrainingId) is not null)
							then
								CAST((select CONVERT(DECIMAL(18,2),MAX(laScore.Score)) from (select la.Attempts as Attempts ,SUM(la.Score) / (select count(*) from dbo.AssesmentSkillChecks where AssesmentGUID = e.AssesmentId) as Score from dbo.LiveAssesmentSkillChecks la where la.EmployeeGUID = b.EmployeeId and la.AssesmentGUID = e.AssesmentId and la.TrainingId = c.TrainingId group by la.Attempts) laScore ) as varchar(10)) 
							ELSE
								'N/A'
							END)
					END ) as HighestScore,
					(SELECT CASE when(e.ModuleId is not null) 
						then 
							(SELECT CASE WHEN ((SELECT top 1 taResult.TaskAnswerId FROM dbo.TaskAnswers AS taResult WHERE taResult.SetupModuleId = e.SetupModuleId and taResult.TrainingId = c.TrainingId and taResult.EmployeeId = b.EmployeeId) is not null) 
							then 
								CAST((select CONVERT(DECIMAL(18,2),taScore.Score) from (select tad.Attempts as Attempts ,SUM(tad.Score) as Score from dbo.TaskAnswers ta join dbo.TaskAnswerDetails tad on ta.TaskAnswerId = tad.TaskAnswerId where ta.SetupModuleId = e.SetupModuleId and ta.EmployeeId = b.EmployeeId and ta.TrainingId = c.TrainingId group by tad.Attempts) taScore where taScore.Attempts = (SELECT max(wTemp.Attempts) FROM (select tads.Attempts as Attempts ,SUM(tads.Score) as Score from dbo.TaskAnswers tas join dbo.TaskAnswerDetails tads on tas.TaskAnswerId = tads.TaskAnswerId where tas.SetupModuleId = e.SetupModuleId and tas.EmployeeId = b.EmployeeId and tas.TrainingId = c.TrainingId group by tads.Attempts) as wTemp)) as varchar(10)) 
							ELSE 
								'N/A' 
							END)
						else
							(SELECT CASE WHEN ((SELECT top 1 laResult.GUID FROM dbo.LiveAssesmentSkillChecks AS laResult WHERE laResult.EmployeeGUID = b.EmployeeId and laResult.AssesmentGUID = e.AssesmentId and laResult.TrainingId = c.TrainingId) is not null)
							THEN
								CAST((select CONVERT(DECIMAL(18,2),laScore.Score) from (select la.Attempts as Attempts ,SUM(la.Score) / (select count(*) from dbo.AssesmentSkillChecks where AssesmentGUID = e.AssesmentId) as Score from dbo.LiveAssesmentSkillChecks la where la.EmployeeGUID = b.EmployeeId and la.AssesmentGUID = e.AssesmentId and la.TrainingId = c.TrainingId group by la.Attempts) laScore where laScore.Attempts = (SELECT max(wTemp.Attempts) FROM (select lass.Attempts as Attempts ,AVG(lass.Score) as Score from dbo.LiveAssesmentSkillChecks lass where lass.AssesmentGUID = e.AssesmentId and lass.EmployeeGUID = b.EmployeeId and lass.TrainingId = c.TrainingId group by lass.Attempts) as wTemp)) as varchar(10)) 
							ELSE
								'N/A'
							END)
					END ) as LatestScore,
					(SELECT CASE WHEN (MAX(h.FinalScore) is not null) 
						then 
							(SELECT CASE WHEN (e.IsByOption != 1) 
							then 
								(SELECT CAST(CONVERT(DECIMAL(18,2),OrdDet.FinalScore) as varchar(10)) FROM dbo.FinalScores AS OrdDet WHERE OrdDet.SetupModuleId = e.SetupModuleId and OrdDet.TrainingId = c.TrainingId and OrdDet.EmployeeId = b.EmployeeId and OrdDet.CreatedAt = (SELECT MAX(WO.CreatedAt) FROM dbo.FinalScores as WO WHERE WO.SetupModuleId = e.SetupModuleId and WO.EmployeeId = b.EmployeeId and WO.TrainingId = c.TrainingId))
							ELSE 
								'By Skill Check'
							end)
						ELSE 
							'N/A'
					end) AS FinalScore,
					(SELECT CASE WHEN ((SELECT top 1 OrdDet.PassedStatus FROM dbo.FinalScores AS OrdDet WHERE OrdDet.SetupModuleId = e.SetupModuleId and OrdDet.TrainingId = c.TrainingId and OrdDet.EmployeeId = b.EmployeeId and OrdDet.CreatedAt = (SELECT MAX(WO.CreatedAt) FROM dbo.FinalScores as WO WHERE WO.SetupModuleId = e.SetupModuleId and WO.EmployeeId = b.EmployeeId and WO.TrainingId = c.TrainingId)) is not null and e.IsByOption != 1 ) 
					then 
						(SELECT CASE WHEN ((SELECT top 1 OrdDet.PassedStatus FROM dbo.FinalScores AS OrdDet WHERE OrdDet.SetupModuleId = e.SetupModuleId and OrdDet.TrainingId = c.TrainingId and OrdDet.EmployeeId = b.EmployeeId and OrdDet.CreatedAt = (SELECT MAX(WO.CreatedAt) FROM dbo.FinalScores as WO WHERE WO.SetupModuleId = e.SetupModuleId and WO.EmployeeId = b.EmployeeId and WO.TrainingId = c.TrainingId)) = 1 ) 
						THEN 
							'LULUS' 
						ELSE 
							'TIDAK LULUS' 
						END)
					ELSE 
						(SELECT CASE when(e.ModuleId is not null) --check modul atau assesment
						Then --module
							(SELECT CASE WHEN ((SELECT top 1 taResult.TaskAnswerId FROM dbo.TaskAnswers AS taResult WHERE taResult.SetupModuleId = e.SetupModuleId and taResult.TrainingId = c.TrainingId and taResult.EmployeeId = b.EmployeeId) is not null) --module atau non module
							then --module
								(SELECT CASE WHEN ((SELECT top 1 tadResult.AnswerBlobId FROM dbo.TaskAnswers AS taResult join dbo.TaskAnswerDetails as tadResult on taResult.TaskAnswerId = tadResult.TaskAnswerDetailId WHERE taResult.SetupModuleId = e.SetupModuleId and taResult.TrainingId = c.TrainingId and taResult.EmployeeId = b.EmployeeId) is not null) 
									then 
										''
									ELSE
										(SELECT CASE WHEN ((SELECT top 1 OrdDet.PassedStatus FROM dbo.FinalScores AS OrdDet WHERE OrdDet.SetupModuleId = e.SetupModuleId and OrdDet.TrainingId = c.TrainingId and OrdDet.EmployeeId = b.EmployeeId and OrdDet.CreatedAt = (SELECT MAX(WO.CreatedAt) FROM dbo.FinalScores as WO WHERE WO.SetupModuleId = e.SetupModuleId and WO.EmployeeId = b.EmployeeId and WO.TrainingId = c.TrainingId)) = 1 ) 
											THEN 
												'LULUS' 
											ELSE 
												''
											END)
									end)
							ELSE --non module
								(SELECT CASE WHEN ((SELECT top 1 OrdDet.PassedStatus FROM dbo.FinalScores AS OrdDet WHERE OrdDet.SetupModuleId = e.SetupModuleId and OrdDet.TrainingId = c.TrainingId and OrdDet.EmployeeId = b.EmployeeId and OrdDet.CreatedAt = (SELECT MAX(WO.CreatedAt) FROM dbo.FinalScores as WO WHERE WO.SetupModuleId = e.SetupModuleId and WO.EmployeeId = b.EmployeeId and WO.TrainingId = c.TrainingId)) = 1 ) 
									THEN 
										'LULUS' 
									ELSE 
										'LULUS' 
									END)
							END)
						else --assesment
							(SELECT CASE WHEN ((SELECT top 1 OrdDet.PassedStatus FROM dbo.FinalScores AS OrdDet WHERE OrdDet.SetupModuleId = e.SetupModuleId and OrdDet.TrainingId = c.TrainingId and OrdDet.EmployeeId = b.EmployeeId and OrdDet.CreatedAt = (SELECT MAX(WO.CreatedAt) FROM dbo.FinalScores as WO WHERE WO.SetupModuleId = e.SetupModuleId and WO.EmployeeId = b.EmployeeId and WO.TrainingId = c.TrainingId)) = 1 ) 
							THEN 
								'LULUS' 
							ELSE 
								''
							END)
						end)
					END) as CompletionStatus,
					(SELECT CASE WHEN (e.IsByOption != 1) 
						then 
							(select CONVERT(varchar(10),CAST(sm3.MinimumScore as decimal(16,2))) as t3 from dbo.SetupModules sm3 where sm3.SetupModuleId = e.SetupModuleId)
					end) as MinimumScore,
					COALESCE(
						(SELECT CASE when(e.ModuleId is not null) 
						then 
							(select max(tad.Attempts) as t1 from dbo.TaskAnswers ta join dbo.TaskAnswerDetails tad on ta.TaskAnswerId = tad.TaskAnswerId where ta.SetupModuleId = e.SetupModuleId and ta.EmployeeId = b.EmployeeId and ta.TrainingId = c.TrainingId) 
						else 
							(select max(lasc.Attempts) as t2 from dbo.LiveAssesmentSkillChecks lasc where lasc.EmployeeGUID = b.EmployeeId and lasc.AssesmentGUID = e.AssesmentId and lasc.TrainingId = c.TrainingId) 
						END ),0) as Attempts
					from dbo.EnrollLearnings a
					join dbo.Employees b on a.EmployeeId = b.EmployeeId 
					join dbo.Trainings c on a.TrainingId = c.TrainingId
					join dbo.TrainingModuleMappings d on c.TrainingId = d.TrainingId 
					join dbo.SetupModules e on d.SetupModuleId = e.SetupModuleId
					join dbo.Courses g on e.CourseId = g.CourseId 
					left join dbo.FinalScores h on e.SetupModuleId = h.SetupModuleId and c.TrainingId = h.TrainingId and b.EmployeeId = h.EmployeeId
					left join dbo.EnrollLearningTimes f on a.EnrollLearningId = f.EnrollLearningId and e.SetupModuleId = f.SetupModuleId
					left join dbo.Modules i on e.ModuleId = i.ModuleId
					left join dbo.Assesments j on e.AssesmentId = j.GUID
					left join dbo.Outlets k on b.OutletId = k.OutletId
					join dbo.EmployeePositionMappings l on b.EmployeeId = l.EmployeeId
					left join dbo.Dealers n on k.DealerId = n.DealerId
					join dbo.ProgramTypes o on g.ProgramTypeId = o.ProgramTypeId
					left join dbo.Areas p on k.AreaId = p.AreaId
					join dbo.EmployeePositionMappings q on b.EmployeeId = q.EmployeeId
					left join dbo.Positions r on q.PositionId = r.PositionId
					left join dbo.AreaSpecialists s on k.AreaSpecialistId = s.AreaSpecialistId
					where e.IsDeleted = 'false' and b.IsDeleted = 'false' and c.isDeleted = 'false' and f.EndTime is not null and g.ProgramTypeId = { programTypeId } and c.TrainingId = { trainingId }
					group by e.SetupModuleId,g.ProgramTypeId,o.ProgramTypeName,b.EmployeeId,b.EmployeeName,k.OutletId,k.Name,n.DealerId,n.DealerName,c.TrainingId,g.CourseName,e.ModuleId,i.ModuleName,e.AssesmentId,j.Name,e.IsByOption,p.AreaId,p.Name,r.PositionId,r.PositionName,s.AreaSpecialistId,s.AreaSpecialistName,r.IsTamPeople
                ");

			return query;
		}

		public IQueryable<GetListAssesmentDoubleModel> GetAssesmentSkillCheckDouble(int trainingId, string employeeGuid, string skillCheckGuid, float attempts)
        {
			var query = this
				.AllAssesmentDoubleData
				.FromSql($@"select a.*
					from liveassesmentskillchecks a
					where a.employeeguid = {employeeGuid} and a.skillcheckguid = {skillCheckGuid}
					and a.trainingid = {trainingId}
					and a.Attempts = {attempts} ");

			return query;
        }

		public IQueryable<GetListModuleDoubleModel> GetModuleCheckDouble(int taskAnswerId, int taskId, float attempts)
        {
			var query = this
				.AllModuleDoubleData
				.FromSql($@"select tad.*
					from TaskAnswerDetails tad
					where tad.taskanswerid = { taskAnswerId } and tad.TaskId = {taskId}
					and tad.Attempts = {attempts}");

			return query;
		}

		public IQueryable<GetListDoubleDataModel> GetListDoubleDataByTraining(int trainingId)
        {
			var query = this
			.AllDoubleDataByTraining
			.FromSql($@"select employeeguid, skillcheckguid, attempts, count(skillcheckguid) as doubleData
				from liveassesmentskillchecks
				where trainingid = { trainingId }
				group by employeeguid, skillcheckguid, attempts
				having count(skillcheckguid) > 1");

			return query;
		}

		public IQueryable<GetListDoubleModuleDataModel> GetListDoubleDataModuleByTaskAnswerId()
        {
			var query = this
			.AllDoubleDataModuleByTaskAnswerId
			.FromSql($@"select taskanswerid, taskid, attempts, count(taskid) as doubleData
				from TaskAnswerDetails
				group by taskanswerid, taskid, attempts
				having count(taskid) > 1");

			return query;
		}

		public IQueryable<GetProgressTrackingModel> GetProgressTrackingData(int programTypeId, int trainingId, List<string> dealerId)
		{
			var query = $@"select
			e.SetupModuleId,
			g.ProgramTypeId,
			o.ProgramTypeName,
			b.EmployeeId,
			b.EmployeeName,
			(SELECT CASE WHEN (k.OutletId is null) 
				then 
					(SELECT CASE WHEN (r.IsTamPeople = 1) 
					then 
						'260' 
					else 
						k.OutletId 
					end) 
				else 
					k.OutletId 
			end) as OutletId,
			(SELECT CASE WHEN (k.OutletId is null) 
				then 
					(SELECT CASE WHEN (r.IsTamPeople = 1) 
					then 
						'TAM Workshop Sunter II' 
					else 
						k.Name 
					end)
				else 
					k.Name 
			end) as OutletName,
			(SELECT CASE WHEN (k.OutletId is null) 
				then 
					(SELECT CASE WHEN (r.IsTamPeople = 1) 
					then 
						(select ot.AreaId from dbo.Outlets ot where ot.OutletId = '260') 
					else 
						p.AreaId 
					end)
				else 
					p.AreaId 
			end) as AreaId,
			(SELECT CASE WHEN (k.OutletId is null) 
				then 
					(SELECT CASE WHEN (r.IsTamPeople = 1) 
					then 
						'TAM'
					else 
						p.Name 
					end)
				else 
					p.Name 
			end) as AreaName,
			s.AreaSpecialistId,
			s.AreaSpecialistName,
			r.PositionId,
			r.PositionName,
			(SELECT CASE WHEN (k.OutletId is null) 
				then 
					(SELECT CASE WHEN (r.IsTamPeople = 1) 
					then 
						'38'
					else 
						n.DealerId 
					end)
				else 
					n.DealerId 
			end) as DealerId,
			(SELECT CASE WHEN (k.OutletId is null) 
				then 
					(SELECT CASE WHEN (r.IsTamPeople = 1) 
					then 
						'TOYOTA ASTRA MOTOR' 
					else 
						n.DealerName 
					end)
				else 
					n.DealerName 
			end) as DealerName,
			(SELECT CASE when(e.ModuleId is not null) then 'Module' else 'Assesment' END ) as Flagging,
			c.TrainingId,
			g.CourseName,
			COALESCE(e.ModuleId,0) as ModuleId,
			coalesce(i.ModuleName,'') as ModuleName,
			e.AssesmentId,
			coalesce(j.Name,'') as AssesmentName,
			CONVERT(varchar,max(c.Batch),0) as Batch,
			CONVERT(varchar,min(f.StartTime),0) as FirstAccess,
			CONVERT(varchar,max(f.EndTime),0) as LastAccess,
			max(convert(varchar(5),DateDiff(s, f.StartTime, f.EndTime)/3600)+':'+convert(varchar(5),DateDiff(s, f.StartTime, f.EndTime)%3600/60)+':'+convert(varchar(5),(DateDiff(s, f.StartTime, f.EndTime)%60))) as TotalStudyTime,
			--CAST(SUM(h.FinalScore) / COUNT(h.FinalScore) AS DECIMAL(16,2)) as AverageScore,
			--CAST(max(h.FinalScore) AS DECIMAL(16,2)) as HighestScore,
			(SELECT CASE when(e.ModuleId is not null) 
				then 
					(SELECT CASE WHEN ((SELECT top 1 taResult.TaskAnswerId FROM dbo.TaskAnswers AS taResult WHERE taResult.SetupModuleId = e.SetupModuleId and taResult.TrainingId = c.TrainingId and taResult.EmployeeId = b.EmployeeId) is not null) 
					then 
						CAST((select CONVERT(DECIMAL(18,2),ROUND(AVG(taScore.Score + 0.0),2)) from (select tad.Attempts as Attempts ,SUM(tad.Score) as Score from dbo.TaskAnswers ta join dbo.TaskAnswerDetails tad on ta.TaskAnswerId = tad.TaskAnswerId where ta.SetupModuleId = e.SetupModuleId and ta.EmployeeId = b.EmployeeId and ta.TrainingId = c.TrainingId group by tad.Attempts) taScore ) as varchar(10)) 
					ELSE 
						'N/A' 
					END)
				else
					(SELECT CASE WHEN ((SELECT top 1 laResult.GUID FROM dbo.LiveAssesmentSkillChecks AS laResult WHERE laResult.EmployeeGUID = b.EmployeeId and laResult.AssesmentGUID = e.AssesmentId and laResult.TrainingId = c.TrainingId) is not null)
					THEN
						CAST((select CONVERT(DECIMAL(18,2),ROUND(AVG(laScore.Score + 0.0),2)) from (select la.Attempts as Attempts ,SUM(la.Score) / (select count(*) from dbo.AssesmentSkillChecks where AssesmentGUID = e.AssesmentId) as Score from dbo.LiveAssesmentSkillChecks la where la.EmployeeGUID = b.EmployeeId and la.AssesmentGUID = e.AssesmentId and la.TrainingId = c.TrainingId group by la.Attempts) laScore ) as varchar(10))
					ELSE
						'N/A'
					END) 
			END) as AverageScore,
			(SELECT CASE when(e.ModuleId is not null) 
				then 
					(SELECT CASE WHEN ((SELECT top 1 taResult.TaskAnswerId FROM dbo.TaskAnswers AS taResult WHERE taResult.SetupModuleId = e.SetupModuleId and taResult.TrainingId = c.TrainingId and taResult.EmployeeId = b.EmployeeId) is not null) 
					then 
						CAST((select CONVERT(DECIMAL(18,2),MAX(taScore.Score)) from (select tad.Attempts as Attempts ,SUM(tad.Score) as Score from dbo.TaskAnswers ta join dbo.TaskAnswerDetails tad on ta.TaskAnswerId = tad.TaskAnswerId where ta.SetupModuleId = e.SetupModuleId and ta.EmployeeId = b.EmployeeId and ta.TrainingId = c.TrainingId group by tad.Attempts) taScore) as varchar(10)) 
					ELSE 
						'N/A' 
					END)
				else
					(SELECT CASE WHEN ((SELECT top 1 laResult.GUID FROM dbo.LiveAssesmentSkillChecks AS laResult WHERE laResult.EmployeeGUID = b.EmployeeId and laResult.AssesmentGUID = e.AssesmentId and laResult.TrainingId = c.TrainingId) is not null)
					then
						CAST((select CONVERT(DECIMAL(18,2),MAX(laScore.Score)) from (select la.Attempts as Attempts ,SUM(la.Score) / (select count(*) from dbo.AssesmentSkillChecks where AssesmentGUID = e.AssesmentId) as Score from dbo.LiveAssesmentSkillChecks la where la.EmployeeGUID = b.EmployeeId and la.AssesmentGUID = e.AssesmentId and la.TrainingId = c.TrainingId group by la.Attempts) laScore ) as varchar(10)) 
					ELSE
						'N/A'
					END)
			END ) as HighestScore,
			(SELECT CASE when(e.ModuleId is not null) 
				then 
					(SELECT CASE WHEN ((SELECT top 1 taResult.TaskAnswerId FROM dbo.TaskAnswers AS taResult WHERE taResult.SetupModuleId = e.SetupModuleId and taResult.TrainingId = c.TrainingId and taResult.EmployeeId = b.EmployeeId) is not null) 
					then 
						CAST((select CONVERT(DECIMAL(18,2),taScore.Score) from (select tad.Attempts as Attempts ,SUM(tad.Score) as Score from dbo.TaskAnswers ta join dbo.TaskAnswerDetails tad on ta.TaskAnswerId = tad.TaskAnswerId where ta.SetupModuleId = e.SetupModuleId and ta.EmployeeId = b.EmployeeId and ta.TrainingId = c.TrainingId group by tad.Attempts) taScore where taScore.Attempts = (SELECT max(wTemp.Attempts) FROM (select tads.Attempts as Attempts ,SUM(tads.Score) as Score from dbo.TaskAnswers tas join dbo.TaskAnswerDetails tads on tas.TaskAnswerId = tads.TaskAnswerId where tas.SetupModuleId = e.SetupModuleId and tas.EmployeeId = b.EmployeeId and tas.TrainingId = c.TrainingId group by tads.Attempts) as wTemp)) as varchar(10)) 
					ELSE 
						'N/A' 
					END)
				else
					(SELECT CASE WHEN ((SELECT top 1 laResult.GUID FROM dbo.LiveAssesmentSkillChecks AS laResult WHERE laResult.EmployeeGUID = b.EmployeeId and laResult.AssesmentGUID = e.AssesmentId and laResult.TrainingId = c.TrainingId) is not null)
					THEN
						CAST((select CONVERT(DECIMAL(18,2),laScore.Score) from (select la.Attempts as Attempts ,SUM(la.Score) / (select count(*) from dbo.AssesmentSkillChecks where AssesmentGUID = e.AssesmentId) as Score from dbo.LiveAssesmentSkillChecks la where la.EmployeeGUID = b.EmployeeId and la.AssesmentGUID = e.AssesmentId and la.TrainingId = c.TrainingId group by la.Attempts) laScore where laScore.Attempts = (SELECT max(wTemp.Attempts) FROM (select lass.Attempts as Attempts ,AVG(lass.Score) as Score from dbo.LiveAssesmentSkillChecks lass where lass.AssesmentGUID = e.AssesmentId and lass.EmployeeGUID = b.EmployeeId and lass.TrainingId = c.TrainingId group by lass.Attempts) as wTemp)) as varchar(10)) 
					ELSE
						'N/A'
					END)
			END ) as LatestScore,
			(SELECT CASE WHEN (MAX(h.FinalScore) is not null) 
				then 
					(SELECT CASE WHEN (e.IsByOption != 1) 
					then 
						(SELECT CAST(CONVERT(DECIMAL(18,2),OrdDet.FinalScore) as varchar(10)) FROM dbo.FinalScores AS OrdDet WHERE OrdDet.SetupModuleId = e.SetupModuleId and OrdDet.TrainingId = c.TrainingId and OrdDet.EmployeeId = b.EmployeeId and OrdDet.CreatedAt = (SELECT MAX(WO.CreatedAt) FROM dbo.FinalScores as WO WHERE WO.SetupModuleId = e.SetupModuleId and WO.EmployeeId = b.EmployeeId and WO.TrainingId = c.TrainingId))
					ELSE 
						'By Skill Check'
					end)
				ELSE 
					'N/A'
			end) AS FinalScore,
			(SELECT CASE WHEN ((SELECT top 1 OrdDet.PassedStatus FROM dbo.FinalScores AS OrdDet WHERE OrdDet.SetupModuleId = e.SetupModuleId and OrdDet.TrainingId = c.TrainingId and OrdDet.EmployeeId = b.EmployeeId and OrdDet.CreatedAt = (SELECT MAX(WO.CreatedAt) FROM dbo.FinalScores as WO WHERE WO.SetupModuleId = e.SetupModuleId and WO.EmployeeId = b.EmployeeId and WO.TrainingId = c.TrainingId)) is not null and e.IsByOption != 1 ) 
			then 
				(SELECT CASE WHEN ((SELECT top 1 OrdDet.PassedStatus FROM dbo.FinalScores AS OrdDet WHERE OrdDet.SetupModuleId = e.SetupModuleId and OrdDet.TrainingId = c.TrainingId and OrdDet.EmployeeId = b.EmployeeId and OrdDet.CreatedAt = (SELECT MAX(WO.CreatedAt) FROM dbo.FinalScores as WO WHERE WO.SetupModuleId = e.SetupModuleId and WO.EmployeeId = b.EmployeeId and WO.TrainingId = c.TrainingId)) = 1 ) 
				THEN 
					'LULUS' 
				ELSE 
					'TIDAK LULUS' 
				END)
			ELSE 
				(SELECT CASE when(e.ModuleId is not null) --check modul atau assesment
				Then --module
					(SELECT CASE WHEN ((SELECT top 1 taResult.TaskAnswerId FROM dbo.TaskAnswers AS taResult WHERE taResult.SetupModuleId = e.SetupModuleId and taResult.TrainingId = c.TrainingId and taResult.EmployeeId = b.EmployeeId) is not null) --module atau non module
					then --module
						(SELECT CASE WHEN ((SELECT top 1 tadResult.AnswerBlobId FROM dbo.TaskAnswers AS taResult join dbo.TaskAnswerDetails as tadResult on taResult.TaskAnswerId = tadResult.TaskAnswerDetailId WHERE taResult.SetupModuleId = e.SetupModuleId and taResult.TrainingId = c.TrainingId and taResult.EmployeeId = b.EmployeeId) is not null) 
							then 
								''
							ELSE
								(SELECT CASE WHEN ((SELECT top 1 OrdDet.PassedStatus FROM dbo.FinalScores AS OrdDet WHERE OrdDet.SetupModuleId = e.SetupModuleId and OrdDet.TrainingId = c.TrainingId and OrdDet.EmployeeId = b.EmployeeId and OrdDet.CreatedAt = (SELECT MAX(WO.CreatedAt) FROM dbo.FinalScores as WO WHERE WO.SetupModuleId = e.SetupModuleId and WO.EmployeeId = b.EmployeeId and WO.TrainingId = c.TrainingId)) = 1 ) 
									THEN 
										'LULUS' 
									ELSE 
										''
									END)
							end)
					ELSE --non module
						(SELECT CASE WHEN ((SELECT top 1 OrdDet.PassedStatus FROM dbo.FinalScores AS OrdDet WHERE OrdDet.SetupModuleId = e.SetupModuleId and OrdDet.TrainingId = c.TrainingId and OrdDet.EmployeeId = b.EmployeeId and OrdDet.CreatedAt = (SELECT MAX(WO.CreatedAt) FROM dbo.FinalScores as WO WHERE WO.SetupModuleId = e.SetupModuleId and WO.EmployeeId = b.EmployeeId and WO.TrainingId = c.TrainingId)) = 1 ) 
							THEN 
								'LULUS' 
							ELSE 
								'LULUS' 
							END)
					END)
				else --assesment
					(SELECT CASE WHEN ((SELECT top 1 OrdDet.PassedStatus FROM dbo.FinalScores AS OrdDet WHERE OrdDet.SetupModuleId = e.SetupModuleId and OrdDet.TrainingId = c.TrainingId and OrdDet.EmployeeId = b.EmployeeId and OrdDet.CreatedAt = (SELECT MAX(WO.CreatedAt) FROM dbo.FinalScores as WO WHERE WO.SetupModuleId = e.SetupModuleId and WO.EmployeeId = b.EmployeeId and WO.TrainingId = c.TrainingId)) = 1 ) 
					THEN 
						'LULUS' 
					ELSE 
						''
					END)
				end)
			END) as CompletionStatus,
			(SELECT CASE WHEN (e.IsByOption != 1) 
				then 
					(select CONVERT(varchar(10),CAST(sm3.MinimumScore as decimal(16,2))) as t3 from dbo.SetupModules sm3 where sm3.SetupModuleId = e.SetupModuleId)
			end) as MinimumScore,
			COALESCE(
				(SELECT CASE when(e.ModuleId is not null) 
				then 
					(select max(tad.Attempts) as t1 from dbo.TaskAnswers ta join dbo.TaskAnswerDetails tad on ta.TaskAnswerId = tad.TaskAnswerId where ta.SetupModuleId = e.SetupModuleId and ta.EmployeeId = b.EmployeeId and ta.TrainingId = c.TrainingId) 
				else 
					(select max(lasc.Attempts) as t2 from dbo.LiveAssesmentSkillChecks lasc where lasc.EmployeeGUID = b.EmployeeId and lasc.AssesmentGUID = e.AssesmentId and lasc.TrainingId = c.TrainingId) 
				END ),0) as Attempts
			from dbo.EnrollLearnings a
			join dbo.Employees b on a.EmployeeId = b.EmployeeId 
			join dbo.Trainings c on a.TrainingId = c.TrainingId
			join dbo.TrainingModuleMappings d on c.TrainingId = d.TrainingId 
			join dbo.SetupModules e on d.SetupModuleId = e.SetupModuleId
			join dbo.Courses g on e.CourseId = g.CourseId 
			left join dbo.FinalScores h on e.SetupModuleId = h.SetupModuleId and c.TrainingId = h.TrainingId and b.EmployeeId = h.EmployeeId
			left join dbo.EnrollLearningTimes f on a.EnrollLearningId = f.EnrollLearningId and e.SetupModuleId = f.SetupModuleId
			left join dbo.Modules i on e.ModuleId = i.ModuleId
			left join dbo.Assesments j on e.AssesmentId = j.GUID
			left join dbo.Outlets k on b.OutletId = k.OutletId
			join dbo.EmployeePositionMappings l on b.EmployeeId = l.EmployeeId
			left join dbo.Dealers n on k.DealerId = n.DealerId
			join dbo.ProgramTypes o on g.ProgramTypeId = o.ProgramTypeId
			left join dbo.Areas p on k.AreaId = p.AreaId
			join dbo.EmployeePositionMappings q on b.EmployeeId = q.EmployeeId
			left join dbo.Positions r on q.PositionId = r.PositionId
			left join dbo.AreaSpecialists s on k.AreaSpecialistId = s.AreaSpecialistId
			where e.IsDeleted = 'false' and b.IsDeleted = 'false' and c.isDeleted = 'false' and f.EndTime is not null and g.ProgramTypeId = { programTypeId } and c.TrainingId = { trainingId } ";
			if (dealerId != null)
			{
				var contains = "'" + string.Join("','", dealerId) + "'";
				query = query + $@"AND n.DealerId in ({contains})";
			}
			query = query + $@" group by e.SetupModuleId,g.ProgramTypeId,o.ProgramTypeName,b.EmployeeId,b.EmployeeName,k.OutletId,k.Name,n.DealerId,n.DealerName,c.TrainingId,g.CourseName,e.ModuleId,i.ModuleName,e.AssesmentId,j.Name,e.IsByOption,p.AreaId,p.Name,r.PositionId,r.PositionName,s.AreaSpecialistId,s.AreaSpecialistName,r.IsTamPeople";

			var queryTest = (RawSqlString)query;

			var generateQuery = this
				.AllProgressTracking
				.FromSql(queryTest);

			return generateQuery;
		}


		public IQueryable<GetProgressTrackingDetailModel> getDetailProgressTrackingModule(int trainingId, int setupModuleId, string employeeId)
		{
			var query = this
				.AllProgressTrackingDetailModule
				.FromSql($@"
					select
					CAST(k.TaskAnswerDetailId as int) as TaskAnswerDetailId,
					o.ProgramTypeId,
					p.ProgramTypeName,
					a.EmployeeId,
					a.EmployeeName,
					CAST((SELECT CASE WHEN (b.OutletId is null) then CAST((SELECT CASE WHEN (d.IsTamPeople = 1) then '260' else b.OutletId end) as varchar(max)) else b.OutletId end) as varchar(max)) as OutletId,
					CAST((SELECT CASE WHEN (b.OutletId is null) then CAST((SELECT CASE WHEN (d.IsTamPeople = 1) then 'TAM Workshop Sunter II' else b.Name end) as varchar(max)) else b.Name end)as varchar(max)) as OutletName,
					CAST((SELECT CASE WHEN (b.OutletId is null) then CAST((SELECT CASE WHEN (d.IsTamPeople = 1) then (select ot.AreaId from dbo.Outlets ot where ot.OutletId = '260') else q.AreaId end) as varchar(max)) else q.AreaId end) as varchar(max)) as AreaId,
					CAST((SELECT CASE WHEN (b.OutletId is null) then CAST((SELECT CASE WHEN (d.IsTamPeople = 1) then (select ar.Name from dbo.Outlets ot join dbo.Areas ar on ot.AreaId = ar.AreaId where ot.OutletId = '260') else q.Name end) as varchar(max)) else q.Name end) as varchar(max)) as AreaName,
					r.AreaSpecialistId,
					r.AreaSpecialistName,
					d.PositionId,
					d.PositionName,
					CAST((SELECT CASE WHEN (b.OutletId is null) then CAST((SELECT CASE WHEN (d.IsTamPeople = 1) then (select ot.DealerId from dbo.Outlets ot where ot.OutletId = '260') else e.DealerId end) as varchar(max)) else e.DealerId end) as varchar(max)) as DealerId,
					CAST((SELECT CASE WHEN (b.OutletId is null) then CAST((SELECT CASE WHEN (d.IsTamPeople = 1) then (select dl.DealerName from dbo.Outlets ot join dbo.Dealers dl on ot.DealerId = dl.DealerId where ot.OutletId = '260') else e.DealerName end) as varchar(max)) else e.DealerName end) as varchar(max)) as DealerName,
					CAST(g.TrainingId as int) as TrainingId,
					k.TaskId,
					o.CourseName,
					mod.ModuleName,
					CAST(MAX(k.Attempts) as int) as AttemptedCount,
					CONVERT(varchar,k.CreatedAt,0) as AttemptedDate,
					CAST(k.Score as decimal(16,2)) as Score,
					CAST(i.MinimumScore as decimal(16,2)) as MinimumScore,
					SUM(CAST(k.Score as decimal(16,2))) OVER(PARTITION BY CAST(MAX(k.Attempts) as int)) AS TotalScore,
					n.QuestionTypeName as QuestionType,
					CAST(COALESCE(
						(SELECT CASE when(n.QuestionTypeId = 1) 
						then 
							(select a.Question from dbo.TaskTrueFalseTypes a where a.TaskId = k.TaskId)
						end ),
						(SELECT CASE when(n.QuestionTypeId = 2) 
						then
							(select a.Question from dbo.TaskMatchingTypes a where a.TaskId = k.TaskId)
						end ),
						(SELECT CASE when(n.QuestionTypeId = 3) 
						then
							(select a.Question from dbo.TaskSequenceTypes a where a.TaskId = k.TaskId)
						end ),
						(SELECT CASE when(n.QuestionTypeId = 4) 
						then
							(select a.Question from dbo.TaskTebakGambarTypes a where a.TaskId = k.TaskId)
						end ),
						(SELECT CASE when(n.QuestionTypeId = 5) 
						then
							(select a.Question from dbo.TaskHotSpotTypes a where a.TaskId = k.TaskId)
						end ),
						(SELECT CASE when(n.QuestionTypeId = 6) 
						then
							(select a.Question from dbo.TaskShortAnswerTypes a where a.TaskId = k.TaskId)
						end ),
						(SELECT CASE when(n.QuestionTypeId = 7) 
						then
							(select a.Question from dbo.TaskEssayTypes a where a.TaskId = k.TaskId)
						end ),
						(SELECT CASE when(n.QuestionTypeId = 8) 
						then
							(select a.Question from dbo.TaskChecklistTypes a where a.TaskId = k.TaskId)
						end ),
						(SELECT CASE when(n.QuestionTypeId = 9) 
						then
							(select a.Question from dbo.TaskRatingTypes a where a.TaskId = k.TaskId)
						end ),
						(SELECT CASE when(n.QuestionTypeId = 10) 
						then
							(select a.Question from dbo.TaskMultipleChoiceTypes a where a.TaskId = k.TaskId)
						end ),
						(SELECT CASE when(n.QuestionTypeId = 11) 
						then
							(select a.Question from dbo.TaskFileUploadTypes a where a.TaskId = k.TaskId)
						end ),
						(SELECT CASE when(n.QuestionTypeId = 12) 
						then
							(select a.Question from dbo.TaskMatrixTypes a where a.TaskId = k.TaskId)
						end ),n.QuestionTypeName) AS VARCHAR(MAX)) as Questions,
					CAST(COALESCE(
						(SELECT CASE when(TRIM(n.QuestionTypeName) != 'File Upload') 
						then 
							(SELECT CASE when(n.QuestionTypeId = 2 or n.QuestionTypeId = 5 or n.QuestionTypeId = 8 or n.QuestionTypeId = 9 or n.QuestionTypeId = 12)
							then 
								(select string_agg(ISNULL(ans, 'kosong'), ',') WITHIN GROUP (ORDER BY RowNum) FROM (SELECT Answer as ans,ROW_NUMBER() OVER (ORDER BY Number) AS RowNum FROM dbo.TaskSpecialAnswers a where a.TaskAnswerDetailId = k.TaskAnswerDetailId) main)
							else
								k.Answer 
							end)
						else 
							CONVERT(varchar(max),k.AnswerBlobId,0) 
						end ),'') AS VARCHAR(MAX)) as Answers
					from dbo.Employees a
					left join dbo.Outlets b on a.OutletId = b.OutletId
					join dbo.EmployeePositionMappings c on a.EmployeeId = c.EmployeeId
					left join dbo.Positions d on c.PositionId = d.PositionId 
					left join dbo.Dealers e on b.DealerId = e.DealerId
					join dbo.EnrollLearnings f on a.EmployeeId = f.EmployeeId
					join dbo.Trainings g on f.TrainingId = g.TrainingId
					join dbo.TrainingModuleMappings h on g.TrainingId = h.TrainingId 
					join dbo.SetupModules i on h.SetupModuleId = i.SetupModuleId
					join dbo.Modules mod on i.ModuleId = mod.ModuleId
					join dbo.TaskAnswers j on i.SetupModuleId = j.SetupModuleId and a.EmployeeId = j.EmployeeId and g.TrainingId = j.TrainingId
					join dbo.TaskAnswerDetails k on j.TaskAnswerId = k.TaskAnswerId
					left join dbo.FinalScores l on a.EmployeeId = l.EmployeeId
					left join dbo.Tasks m on k.TaskId = m.TaskId
					left join dbo.QuestionTypes n on m.QuestionTypeId = n.QuestionTypeId
					join dbo.Courses o on i.CourseId = o.CourseId
					join dbo.ProgramTypes p on o.ProgramTypeId = p.ProgramTypeId
					left join dbo.Areas q on b.AreaId = q.AreaId
					left join dbo.AreaSpecialists r on b.AreaSpecialistId = r.AreaSpecialistId
					where a.EmployeeId = { employeeId } and i.IsDeleted = 'FALSE' and f.IsEnrolled = 'TRUE' and g.isDeleted = 'FALSE' and i.SetupModuleId = { setupModuleId } and g.TrainingId = { trainingId }
					group by a.EmployeeId,g.TrainingId,a.EmployeeName,e.DealerName,b.Name,k.Score,k.CreatedAt,k.TaskAnswerDetailId,i.MinimumScore,k.TaskId,n.QuestionTypeName,k.Answer,k.AnswerBlobId,o.CourseName,b.OutletId,e.DealerId,o.ProgramTypeId,p.ProgramTypeName,d.PositionId,d.PositionName,b.AreaId,q.Name,r.AreaSpecialistId,r.AreaSpecialistName,mod.ModuleName,n.QuestionTypeId,d.IsTamPeople,q.AreaId
				");

			return query;
		}

		public IQueryable<GetProgressTrackingDetailAssesment> getDetailProgressTrackingAssesment(int trainingId, int setupModuleId, string employeeId)
		{
			var query = this
				.AllProgressTrackingDetailAssesment
				.FromSql($@"
					select
					j.GUID,
					l.ProgramTypeId,
					m.ProgramTypeName,
					a.EmployeeId,
					a.EmployeeName,
					CAST((SELECT CASE WHEN (b.OutletId is null) then CAST((SELECT CASE WHEN (d.IsTamPeople = 1) then '260' else b.OutletId end) as varchar(max)) else b.OutletId end) as varchar(max)) as OutletId,
					CAST((SELECT CASE WHEN (b.OutletId is null) then CAST((SELECT CASE WHEN (d.IsTamPeople = 1) then 'TAM Workshop Sunter II' else b.Name end) as varchar(max)) else b.Name end)as varchar(max)) as OutletName,
					CAST((SELECT CASE WHEN (b.OutletId is null) then CAST((SELECT CASE WHEN (d.IsTamPeople = 1) then (select ot.AreaId from dbo.Outlets ot where ot.OutletId = '260') else n.AreaId end) as varchar(max)) else n.AreaId end) as varchar(max)) as AreaId,
					CAST((SELECT CASE WHEN (b.OutletId is null) then CAST((SELECT CASE WHEN (d.IsTamPeople = 1) then (select ar.Name from dbo.Outlets ot join dbo.Areas ar on ot.AreaId = ar.AreaId where ot.OutletId = '260') else n.Name end) as varchar(max)) else n.Name end) as varchar(max)) as AreaName,
					r.AreaSpecialistId,
					r.AreaSpecialistName,
					d.PositionId,
					d.PositionName,
					CAST((SELECT CASE WHEN (b.OutletId is null) then CAST((SELECT CASE WHEN (d.IsTamPeople = 1) then (select ot.DealerId from dbo.Outlets ot where ot.OutletId = '260') else e.DealerId end) as varchar(max)) else e.DealerId end) as varchar(max)) as DealerId,
					CAST((SELECT CASE WHEN (b.OutletId is null) then CAST((SELECT CASE WHEN (d.IsTamPeople = 1) then (select dl.DealerName from dbo.Outlets ot join dbo.Dealers dl on ot.DealerId = dl.DealerId where ot.OutletId = '260') else e.DealerName end) as varchar(max)) else e.DealerName end) as varchar(max)) as DealerName,
					CAST(g.TrainingId as int) as TrainingId,
					l.CourseName,
					ass.Name as AssesmentName,
					CAST(MAX(j.Attempts) as int) as AttemptedCount,
					CONVERT(varchar,j.UpdatedAt,0) as AttemptedDate,
					CAST(j.Score AS DECIMAL(16,2)) as Score,
					CAST(COALESCE((SELECT CASE when(i.IsByOption = '1') then (select scs.MinimumScore as t1 from dbo.SkillChecks scs where scs.GUID = sc.GUID) else (select sm.MinimumScore as t2 from dbo.SetupModules sm where sm.SetupModuleId = i.SetupModuleId) END ),0) AS DECIMAL(16,2) ) as MinimumScore,
					i.IsByOption,
					CAST(AVG(CAST(j.Score as int)) OVER(PARTITION BY CAST(MAX(j.Attempts) as int)) AS DECIMAL(16,2)) AS TotalScore,
					sc.Name as SkillCheckName
					from dbo.Employees a
					left join dbo.Outlets b on a.OutletId = b.OutletId
					join dbo.EmployeePositionMappings c on a.EmployeeId = c.EmployeeId
					left join dbo.Positions d on c.PositionId = d.PositionId 
					left join dbo.Dealers e on b.DealerId = e.DealerId
					join dbo.EnrollLearnings f on a.EmployeeId = f.EmployeeId
					join dbo.Trainings g on f.TrainingId = g.TrainingId
					join dbo.TrainingModuleMappings h on g.TrainingId = h.TrainingId
					join dbo.SetupModules i on h.SetupModuleId = i.SetupModuleId
					join dbo.LiveAssesmentSkillChecks j on a.EmployeeId = j.EmployeeGUID and i.AssesmentId = j.AssesmentGUID and g.TrainingId = j.TrainingId
					join dbo.Assesments ass on i.AssesmentId = ass.GUID
					join dbo.SkillChecks sc on j.SkillCheckGUID = sc.GUID
					join dbo.FinalScores k on a.EmployeeId = k.EmployeeId and i.AssesmentId = k.AssesmentId
					join dbo.Courses l on i.CourseId = l.CourseId
					join dbo.ProgramTypes m on l.ProgramTypeId = m.ProgramTypeId
					left join dbo.Areas n on b.AreaId = n.AreaId
					left join dbo.AreaSpecialists r on b.AreaSpecialistId = r.AreaSpecialistId
					where a.EmployeeId = { employeeId } and i.IsDeleted = 'FALSE' and f.IsEnrolled = 'TRUE' and g.isDeleted = 'FALSE' and i.SetupModuleId = { setupModuleId } and g.TrainingId = { trainingId }
					group by a.EmployeeId,g.TrainingId,a.EmployeeName,e.DealerName,b.Name,j.UpdatedAt,j.Score,j.GUID,sc.Name,sc.GUID,i.IsByOption,i.SetupModuleId,l.ProgramTypeId,m.ProgramTypeName,b.OutletId,e.DealerId,n.AreaId,n.Name,d.PositionId,d.PositionName,l.CourseName,r.AreaSpecialistId,r.AreaSpecialistName,ass.Name,d.IsTamPeople
				");

			return query;
		}

		public IQueryable<GetCountTrainingQueryModel> GetCountTrainingCompleted(string employeeId)
		{
			var query = this
				.GetCountTrainingQuery
				.FromSql($@"

				SELECT
				[TrainingId] = CASE
					WHEN [t].[TrainingId] IS NULL
					THEN 0 ELSE [t].[TrainingId]
				END
				FROM  [Trainings] AS [t] 
				JOIN EnrollLearnings [el] ON t.TrainingId = el.TrainingId
				WHERE t.ApprovedAt IS NOT NULL AND el.EmployeeId = { employeeId } AND el.IsPassed IS NOT NULL

				UNION ALL

				SELECT
					CASE WHEN TrainingId is NULL THEN 0 END AS TrainingId
				FROM EnrollLearnings el
				JOIN [SetupModules] AS [sm] ON el.SetupModuleId = [sm].SetupModuleId 
				WHERE ((([sm].[CourseId] IS NULL AND ([sm].[IsDeleted] = 0)))) 
				AND ([el].[IsEnrolled] = 1) AND [el].[EmployeeId] = { employeeId } AND el.IsPassed IS NOT NULL
			");

			return query;
		}

		public IQueryable<GetCountTrainingQueryModel> GetCountTrainingContinued(string employeeId)
		{
			var query = this
				.GetCountTrainingContinueQuery
				.FromSql($@"
				select CASE WHEN TrainingId is NULL THEN 0 ELSE TrainingId END AS TrainingId from 
				(SELECT DISTINCT
					[TrainingId] = CASE
						WHEN [t].[TrainingId] IS NULL
						THEN 0 ELSE [t].[TrainingId]
					END,
					el.SetupModuleId
				FROM [Trainings] AS [t]
					JOIN EnrollLearnings [el] ON t.TrainingId = el.TrainingId
				WHERE t.ApprovedAt IS NOT NULL AND t.isDeleted = 0 AND el.IsEnrolled = 1 AND el.EmployeeId = { employeeId } AND (el.IsPassed = 0 OR el.IsPassed IS NULL)

				UNION ALL

				SELECT DISTINCT
					TrainingId = NULL,
					sm.SetupModuleId
				FROM [SetupModules] AS [sm] 
					JOIN [EnrollLearnings] AS [el] ON [sm].[SetupModuleId] = [sm].[SetupModuleId]
					JOIN EnrollLearningTimes [elt] ON [el].[EnrollLearningId] = [elt].[EnrollLearningId] 
				WHERE ((([sm].[CourseId] IS NULL AND ([sm].[IsDeleted] = 0))) AND ([el].[IsEnrolled] = 1) AND [el].[EmployeeId] = { employeeId } AND [elt].[EndTime] is null)
				) AS Trainings

				WHERE (TrainingId is null 
				AND setupmoduleid in (SELECT DISTINCT  el.SetupModuleId from EnrollLearnings el
						 join EnrollLearningTimes elt on el.EnrollLearningId = elt.EnrollLearningId
						 where el.EmployeeId = { employeeId } AND (elt.EndTime is null OR elt.StartTime is null) AND el.SetupModuleId is not null))
				OR 
				trainingId  in
				(SELECT DISTINCT el.TrainingId from EnrollLearnings el
						 join EnrollLearningTimes elt on el.EnrollLearningId = elt.EnrollLearningId
						 where el.EmployeeId = { employeeId } AND ((elt.EndTime is null OR elt.StartTime is null) OR el.IsPassed is null) AND el.TrainingId is not null)
			");

			return query;
		}

		/// <summary>
		/// Get Top 5 Coach for dashboard
		/// </summary>
		/// <returns></returns>
		public IQueryable<DashboardTop5CoachData> GetTop5Coach()
		{
			var query = this
				.DashboardTop5CoachData
				.FromSql($@"
                 SELECT TOP 5
					e.EmployeeName AS CoachName,
					CAST (AVG(cr.RatingScore * 1.0) AS FLOAT) AS Rating
				FROM CoachRatings cr
					JOIN Coaches c ON cr.CoachId = c.CoachId
					JOIN Employees e ON c.EmployeeId = e.EmployeeId
				WHERE cr.IsDeleted = 0 AND e.IsDeleted = 0
				GROUP BY e.EmployeeId, e.EmployeeName
				ORDER BY Rating DESC");

			return query;
		}

		/// <summary>
		/// Get Top 5 Events for Dashboard
		/// </summary>
		/// <returns></returns>
		public IQueryable<DashboardTop5EventData> GetTop5Events()
		{
			var query = this
				.DashboardTop5EventData
				.FromSql($@"
                    SELECT TOP 5
						e.EventId,
						e.Name,
						COUNT(evm.EmployeeId) AS TotalJoined
					FROM Events e
						JOIN EmployeeEventMappings evm ON e.EventId = evm.EventId
					WHERE evm.IsJoined = 1 AND e.IsDeleted = 0
					GROUP BY 
						e.EventId,
						e.Name
					ORDER BY TotalJoined DESC");

			return query;
		}

		/// <summary>
		/// Get Top 5 Reward Type for dashboard
		/// </summary>
		/// <returns></returns>
		public IQueryable<DashboardTop5RewardTypeData> GetTop5RewardType()
		{
			var query = this
				.DashboardTop5RewardTypeData
				.FromSql($@"
                   SELECT TOP 5
						rt.Name AS RewardType,
						COUNT(rt.RewardTypeId) AS TotalReward
				   FROM Rewards r
						JOIN EmployeeRewardMappings erm ON r.RewardId = erm.RewardId
						JOIN RewardTypes rt ON r.RewardTypeId = rt.RewardTypeId
				   WHERE r.IsDeleted = 0
				   GROUP BY rt.Name, rt.RewardTypeId
				   ORDER BY TotalReward DESC");

			return query;
		}

		/// <summary>
		/// Get NPS Report for dashboard
		/// </summary>
		/// <returns></returns>
		public IQueryable<DashboardNPSReportData> GetNPSReport()
		{
			var query = this
				.DashboardNPSReportData
				.FromSql($@"
                   SELECT 
                        SUM(CASE WHEN RatingScore >= 9 AND RatingScore <= 10 THEN 1 ELSE 0 END) AS Promotor,
						SUM(CASE WHEN RatingScore >= 7 AND RatingScore < 9 THEN 1 ELSE 0 END) AS Passive,
						SUM(CASE WHEN RatingScore >= 0 AND RatingScore < 7 THEN 1 ELSE 0 END) AS Detractors
                    FROM CoachRatings");

			return query;
		}

		public IQueryable<UserSideCourseData> GetUserSideCourse(int trainingId)
		{
			var query = this
				.UserSideCourseData
				.FromSql($@"
SELECT                             t.TrainingId,
                                   t.Batch,
                                   t.StartDate,
                                   t.EndDate,
                                   c.CourseId,
                                   c.CourseName,
                                   pt.ProgramTypeName,
                                   tp.Points,
                                   c.CoursePrice,
                                   tp.Time,
                                   topic.TopicName,
                                   c.BlobId,
                                   b.Mime,
                                   c.CreatedAt,
                                   c.UpdatedAt
FROM Trainings t
JOIN Courses c ON c.CourseId = t.CourseId
JOIN ApprovalToCourses atc ON c.CourseId = atc.CourseId
JOIN Approvals a ON a.ApprovalId = atc.ApprovalId
JOIN ProgramTypes pt ON pt.ProgramTypeId = c.ProgramTypeId
LEFT JOIN Blobs b ON b.BlobId = c.BlobId
JOIN SetupModules sm ON sm.CourseId = c.CourseId
JOIN TimePoints tp ON tp.TimePointId = sm.TimePointId
JOIN Modules m ON m.ModuleId = sm.ModuleId
JOIN ModuleTopicMappings mtm ON mtm.ModuleId = m.ModuleId
JOIN Topics topic ON topic.TopicId = mtm.TopicId
LEFT JOIN TrainingRatings tr ON tr.TrainingId = tr.TrainingId
WHERE a.ApprovalStatusId = 1 AND c.IsDeleted = 0 AND t.TrainingId = { trainingId }");

			return query;
		}

		public IQueryable<PrivilegePageMappingData> GetPrivilegePageMappingData(string employeeId)
		{
			var query = this
				.PrivilegePageMappingData
				.FromSql($@"
                    SELECT 
                        ppm.PageId AS PageId, 
                        CAST(MAX(CAST(ppm.IsCreate AS tinyint)) AS bit) AS IsCreate, 
                        CAST(MAX(CAST(ppm.IsRead AS tinyint)) AS bit) AS IsRead,
                        CAST(MAX(CAST(ppm.IsUpdate AS tinyint)) AS bit) AS IsUpdate, 
                        CAST(MAX(CAST(ppm.IsDelete AS tinyint)) AS bit) AS IsDelete
                    FROM EmployeePositionMappings epm
                    JOIN Roles r ON epm.PositionId = r.PositionId
                    JOIN PrivilegePageMappings ppm ON r.RoleId = ppm.RoleId
                    WHERE epm.EmployeeId = { employeeId }
                    GROUP BY PageId
                ");

			return query;
		}

		public IQueryable<CourseOverviewQueryModel> GetCourseOverviewQuery(string user, int trainingId)
		{
			var query = this
				.CourseOverviewQuery
				.FromSql($@"
select 
elt.SetupModuleId as SetupModuleId,
sm.[Order] as Orders,
sm.CourseId as CourseId,
m.ModuleId as ModuleId,
m.ModuleName as ContentName,
mt.MaterialTypeName as ModuleType,
 m.BlobId as BlobId,
m.MaterialBlobId as MaterialBlobId,
mbb.Mime as MaterialMime,
b.Mime as Mime,
tp.Time as ModuleTimeLength,
sm.IsForRemedial,
tp.Points as ModuleTotalPoints,
[StartTime] = CASE
	WHEN [elt].[StartTime] IS NULL
	THEN null ELSE [elt].[StartTime]
END,
[EndTime] = CASE
	WHEN [elt].[EndTime] IS NULL
	THEN null ELSE [elt].[EndTime]
END,
sm.IsPublished,
[ModuleStartTime] = tmm.TrainingStart,
[ModuleEndTime] = tmm.TrainingEnd
from Trainings t
join EnrollLearnings el on t.TrainingId = el.TrainingId
join EnrollLearningTimes elt on el.EnrollLearningId = elt.EnrollLearningId
join SetupModules sm on elt.SetupModuleId = sm.SetupModuleId
left join Modules m on sm.ModuleId = m.ModuleId
left join Blobs b on m.BlobId = b.BlobId
left join Blobs mbb on m.MaterialBlobId = mbb.BlobId
left join MaterialTypes mt on m.MaterialTypeId = mt.MaterialTypeId
left join TimePoints tp on sm.TimePointId = tp.TimePointId
left join TrainingModuleMappings tmm on sm.SetupModuleId = tmm.SetupModuleId and t.TrainingId = tmm.TrainingId
WHERE t.TrainingId = { trainingId } and sm.IsDeleted = 0 and el.EmployeeId = { user }");

			return query;
		}

		public IQueryable<CourseOverviewQueryModel> GetCourseOverviewPassedOrFailedQuery(string user, int trainingId)
		{
			var query = this
				.CourseOverviewQuery
				.FromSql($@"
select 
elt.SetupModuleId as SetupModuleId,
m.ModuleId as ModuleId,
t.CourseId as CourseId,
sm.[Order] as Orders, 
m.ModuleName as ContentName,
mt.MaterialTypeName as ModuleType,
 m.BlobId as BlobId,
m.MaterialBlobId as MaterialBlobId,
b.Mime as Mime,
mbb.Mime as MaterialMime,
tp.Time as ModuleTimeLength,
sm.IsForRemedial,
tp.Points as ModuleTotalPoints,
[StartTime] = CASE
	WHEN [elt].[StartTime] IS NULL
	THEN null ELSE [elt].[StartTime]
END,
[EndTime] = CASE
	WHEN [elt].[EndTime] IS NULL
	THEN null ELSE [elt].[EndTime]
END,
sm.IsPublished,
[ModuleStartTime] = tmm.TrainingStart,
[ModuleEndTime] = tmm.TrainingEnd
from Trainings t
join EnrollLearnings el on t.TrainingId = el.TrainingId
join EnrollLearningTimes elt on el.EnrollLearningId = elt.EnrollLearningId
join SetupModules sm on elt.SetupModuleId = sm.SetupModuleId
left join Modules m on sm.ModuleId = m.ModuleId
left join Blobs b on m.BlobId = b.BlobId
left join Blobs mbb on m.MaterialBlobId = mbb.BlobId
left join MaterialTypes mt on m.MaterialTypeId = mt.MaterialTypeId
left join TimePoints tp on sm.TimePointId = tp.TimePointId
left join TrainingModuleMappings tmm on sm.SetupModuleId = tmm.SetupModuleId and t.TrainingId = tmm.TrainingId
WHERE 
t.TrainingId = { trainingId } and 
el.IsPassed is not null and 
elt.StartTime is not null and
elt.EndTime is not null and
sm.ModuleId is not null and
el.EmployeeId = { user }");

			return query;
		}

		public IQueryable<CourseOverviewQueryWithoutElModel> GetCourseOverviewQueryWithoutEnrollLearnings(int trainingId)
		{
			var query = this
				.ModuleContentsWithoutEl
				.FromSql($@"
select 
sm.SetupModuleId as SetupModuleId,
sm.[Order] as Orders,
m.ModuleId as ModuleId,
m.ModuleName as ContentName,
mt.MaterialTypeName as ModuleType,
m.BlobId as BlobId,
b.Mime as Mime,
tp.Time as ModuleTimeLength,
tp.Points as ModuleTotalPoints
from SetupModules sm
left join Trainings t on sm.CourseId = t.CourseId
left join Modules m on sm.ModuleId = m.ModuleId
left join Blobs b on m.BlobId = b.BlobId
left join MaterialTypes mt on m.MaterialTypeId = mt.MaterialTypeId
left join TimePoints tp on sm.TimePointId = tp.TimePointId
where t.TrainingId = { trainingId } and sm.IsDeleted = 0 and sm.ModuleId is not null
order by sm.SetupModuleId asc");

			return query;
		}

		public IQueryable<CourseOverviewQueryWithoutElModel> GetCourseOverviewQueryWithoutEnrollLearningsBySetupModuleId(int setupModuleId)
		{
			var query = this
				.ModuleContentsWithoutEl
				.FromSql($@"
select 
sm.SetupModuleId as SetupModuleId,
sm.[Order] as Orders,
m.ModuleId as ModuleId,
m.ModuleName as ContentName,
mt.MaterialTypeName as ModuleType,
m.BlobId as BlobId,
b.Mime as Mime,
tp.Time as ModuleTimeLength,
tp.Points as ModuleTotalPoints
from SetupModules sm
left join Trainings t on sm.CourseId = t.CourseId
left join Modules m on sm.ModuleId = m.ModuleId
left join Blobs b on m.BlobId = b.BlobId
left join MaterialTypes mt on m.MaterialTypeId = mt.MaterialTypeId
left join TimePoints tp on sm.TimePointId = tp.TimePointId
where sm.SetupModuleId = { setupModuleId } and sm.IsDeleted = 0 and sm.ModuleId is not null
order by sm.SetupModuleId asc");

			return query;
		}

		//BUAT AMBIL MODULE CONTENT
		public IQueryable<CourseOverviewQueryWithoutElModel> GetSetupModuleOverviewQueryWithoutEnrollLearnings(int setupModuleId)
		{
			var query = this
				.ModuleContentsWithoutElForSetupModule
				.FromSql($@"
select 
sm.SetupModuleId as SetupModuleId,
sm.[Order] as Orders,
m.ModuleId as ModuleId,
m.ModuleName as ContentName,
mt.MaterialTypeName as ModuleType,
 m.BlobId as BlobId,
b.Mime as Mime,
tp.Time as ModuleTimeLength,
tp.Points as ModuleTotalPoints
from SetupModules sm
left join Trainings t on sm.CourseId = t.CourseId
left join Modules m on sm.ModuleId = m.ModuleId
left join Blobs b on m.BlobId = b.BlobId
left join MaterialTypes mt on m.MaterialTypeId = mt.MaterialTypeId
left join TimePoints tp on sm.TimePointId = tp.TimePointId
where sm.SetupModuleId = { setupModuleId } and sm.IsDeleted = 0 and sm.ModuleId is not null");

			return query;
		}
		//BUAT AMBIL MODULE CONTENT KALO DAH ENROLL
		public IQueryable<SetupModuleOverviewQueryModel> GetSetupModuleOverviewQuery(string user, int setupModuleId)
		{
			var query = this
				.SetupModuleOverviewQuery
				.FromSql($@"
select 
sm.SetupModuleId as SetupModuleId,
m.ModuleId as ModuleId,
sm.[Order] as Orders,
m.ModuleName as ContentName,
mt.MaterialTypeName as ModuleType,
 m.BlobId as BlobId,
b.Mime as Mime,
tp.Time as ModuleTimeLength,
[IsForRemedial] = CASE
	WHEN [sm].[IsForRemedial] IS NULL
	THEN null ELSE [sm].[IsForRemedial]
END,
tp.Points as ModuleTotalPoints,
elt.StartTime as StartTime,
elt.EndTime as EndTime
from SetupModules sm
left join Modules m on sm.ModuleId = m.ModuleId
left join EnrollLearnings el on sm.SetupModuleId = el.SetupModuleId
left join EnrollLearningTimes elt on el.EnrollLearningId = elt.EnrollLearningId
left join Blobs b on m.BlobId = b.BlobId
left join MaterialTypes mt on m.MaterialTypeId = mt.MaterialTypeId
left join TimePoints tp on sm.TimePointId = tp.TimePointId
where sm.SetupModuleId = { setupModuleId } and sm.IsDeleted = 0 and el.EmployeeId = { user } and sm.ModuleId is not null");

			return query;
		}


		public IQueryable<GetModuleContent> ModuleContentsQuery(int ModuleId, int SetupModuleId)
		{
			var query = this
				.ModuleContents
				.FromSql($@"select

sm.SetupModuleId as SetupModuleId,
m.ModuleName as ModuleName,
tp.Time as Duration,
tp.Points as point,
m.ModuleDescription as ModuleDescription,
sm.MinimumScore as MinimumScore,
mt.MaterialTypeName as MaterialType,
m.MaterialBlobId as FileUrl,
b.Mime as Mime,
elt.EndTime as EndTime,
elt.StartTime as StartTime,
st.TestTime as TestTime
from SetupModules sm
left join Modules m on sm.ModuleId = m.ModuleId
join TimePoints tp on sm.TimePointId = tp.TimePointId
join MaterialTypes mt on m.MaterialTypeId = mt.MaterialTypeId
left join EnrollLearningTimes elt on sm.SetupModuleId = elt.SetupModuleId
left join SetupTasks st on sm.SetupModuleId = st.SetupModuleId
left join Blobs b on m.MaterialBlobId = b.BlobId
WHERE sm.SetupModuleId = { SetupModuleId }  and m.ModuleId = { ModuleId } ");

			return query;
		}

		/// <summary>
		/// Get Approval Training
		/// </summary>
		/// <returns></returns>
		public IQueryable<ApprovalTraining> GetApprovalTraining()
		{
			var query = this
				.ApprovalTraining
				.FromSql($@"
                    SELECT
						t.TrainingId as [TrainingId],
						c.CourseName as [CourseName],
						pt.ProgramTypeName as [ProgramType],
						t.Batch as [Batch],
						e.Count as [Enrolment],
						t.Quota as [Quota],
						t.StartDate as [TrainingStartDate],
						t.EndDate as [TrainingEndDate],
						t.UpdatedAt as[TrainingUpdatedAt]
					FROM Trainings t
					JOIN Courses c ON t.CourseId = c.CourseId
					JOIN ProgramTypes pt on c.ProgramTypeId = pt.ProgramTypeId
					JOIN (
						SELECT [Count] = COUNT(TrainingId), TrainingId 
						FROM EnrollLearnings
						WHERE IsEnrolled = 1
						GROUP BY TrainingId
					) e on t.TrainingId = e.TrainingId
					WHERE (c.LearningTypeId = 2 or c.LearningTypeId = 3 OR c.CoursePrice > 0) AND t.isDeleted = 0");
			return query;
		}
        /// <summary>
        /// Get Training Process
        /// </summary>
        /// <returns></returns>
        public IQueryable<TrainingProcess> GetTrainingProcess()
        {
            var query = this
                .TrainingProcess
                .FromSql($@"
                    SELECT
						t.TrainingId as [TrainingId],
						c.CourseName as [CourseName], 
						pt.ProgramTypeName as [ProgramType], 
						t.Batch as [Batch], 
						SUM(
						CASE 
						WHEN tp.IsConfirmed = 1 THEN 1
						ELSE 0
						END
						) as [Confirmed], 
						t.Quota as [Quota], 
						t.StartDate as [TrainingStartDate], 
						t.EndDate as [TrainingEndDate],
						t.UpdatedAt as [TrainingUpdatedAt] 
					FROM Trainings t
					JOIN Courses c ON t.CourseId = c.CourseId
					JOIN ProgramTypes pt on c.ProgramTypeId = pt.ProgramTypeId
					JOIN TrainingInvitations ti on t.TrainingId = ti.TrainingId
					LEFT JOIN TrainingProcesses tp on ti.TrainingInvitationId = tp.TrainingInvitationId
					WHERE (c.LearningTypeId = 2 or c.LearningTypeId = 3  OR c.CoursePrice > 0) AND t.isDeleted = 0
					GROUP BY 
						t.TrainingId,
						c.CourseName,
						pt.ProgramTypeName,
						t.Batch,
						t.Quota,
						t.StartDate,
						t.EndDate,
						t.UpdatedAt");
			return query;
		}

		public IQueryable<StagingDealerEmployeeDistinctIdQueryModel> GetDistinctIdFromStagingDealerEmployee()
		{
			var query = this
				.StagingEmployeeDistinctIds
				.FromSql($@"
                    SELECT MAX(ID) as Id FROM 
					(SELECT ID,[EmployeeId] = 'OC' + o.OutletCode + 'EC' + sde.EmployeeId 
					FROM StagingDealerEmployee sde
					JOIN Outlets o on sde.OutletId = o.OutletId
					WHERE State = 'Active') x
					GROUP BY x.EmployeeId
                ");

			return query;
		}

		public IQueryable<StagingDealerEmployeeDistinctSupervisorIdQueryModel> GetDistinctSupervisorIdFromStagingDealerEmployee()
		{
			var query = this
				.StagingEmployeeDistinctSupervisorIds
				.FromSql($@"
                    SELECT ParentCode AS Id
					FROM StagingDealerEmployee sde
					JOIN Outlets o on sde.OutletId = o.OutletId
					WHERE State = 'Active') x
					GROUP BY x.ParentCode
                ");

			return query;
		}

		public IQueryable<GetContinuedLearningModel> GetContinuedLearningModel(string user)
		{
			var param = new SqlParameter("user", user);

			var query = this
				.ContentContinuedLearningModels
				.FromSql($@"SELECT * from
(
SELECT DISTINCT
	[TrainingId] = CASE
		WHEN [t].[TrainingId] IS NULL
		THEN 0 ELSE [t].[TrainingId]
	END, 
	[Batch] = CASE
		WHEN [t].[TrainingId] IS NULL
		THEN 0 ELSE [t].[Batch]
	END,
	t.StartDate,
	t.EndDate,
	c.[CourseId], 
	c.[CourseName], 
	pt.[ProgramTypeName], 
	SetupModuleId = NULL,
	ModuleName = NULL,
	c.[BlobId], 
	[b].[MIME], 
	MaterialTypeName = NULL,
	[CreatedAt] = CASE
		WHEN [t].[TrainingId] IS NULL
			THEN c.SetupCourseCreatedAt  
		ELSE t.CreatedAt
	END, 
	[UpdatedAt] = CASE
		WHEN [t].[TrainingId] IS NULL
			THEN c.SetupCourseUpdatedAt 
		ELSE t.UpdatedAt
	END, 
	[ApprovedAt] = CASE
		WHEN [t].[TrainingId] IS NULL
			THEN c.SetupCourseApprovedAt  
		ELSE t.ApprovedAt
	END
FROM [Courses] AS c 
	LEFT JOIN [ProgramTypes] AS [pt] ON c.[ProgramTypeId] = pt.[ProgramTypeId]
	INNER JOIN [Blobs] AS [b] ON c.[BlobId] = [b].[BlobId]
	LEFT JOIN [Trainings] AS [t] ON c.[CourseId] = [t].[CourseId]
	JOIN EnrollLearnings [el] ON t.TrainingId = el.TrainingId
	JOIN Employees [e] ON el.EmployeeId = e.EmployeeId
	JOIN ApprovalToCourses [atc] ON c.CourseId = atc.CourseId
WHERE t.ApprovedAt IS NOT NULL AND t.isDeleted = 0 AND el.IsEnrolled = 1 AND e.EmployeeId = @user AND (el.IsPassed = 0 OR el.IsPassed IS NULL)

UNION ALL

SELECT DISTINCT
	TrainingId = NULL,
	Batch = NULL,
	StartDate = NULL,
	EndDate = NULL,
	CourseId = NULL,
	CourseName = NULL,
	ProgramTypeName = NULL,
	sm.SetupModuleId,
	m.ModuleName,
	b.BlobId,
	b.MIME,
	mt.MaterialTypeName,
	sm.CreatedAt,
	sm.UpdatedAt,
	sm.ApprovedAt
FROM Modules m
	JOIN [SetupModules] AS [sm] ON [m].[ModuleId] = [sm].[ModuleId]
	JOIN [Blobs] AS [b] ON [m].[BlobId] = [b].[BlobId]
	JOIN [MaterialTypes] AS [mt] ON [m].[MaterialTypeId] = [mt].[MaterialTypeId]
	JOIN [EnrollLearnings] AS [el] ON [sm].[SetupModuleId] = [sm].[SetupModuleId]
	JOIN Employees [e] ON [el].[EmployeeId] = [e].[EmployeeId]
	JOIN EnrollLearningTimes [elt] ON [el].[EnrollLearningId] = [elt].[EnrollLearningId] 
	JOIN [ApprovalToSetupModules] AS [atm] ON [sm].[SetupModuleId] = [atm].[SetupModuleId]
	JOIN [Approvals] AS [a] ON [atm].[ApprovalId] = [a].[ApprovalId]
WHERE ((([sm].[CourseId] IS NULL AND ([sm].[IsDeleted] = 0)) AND ([m].[IsDeleted] = 0)) AND ([a].[ApprovalStatusId] = 1)) AND ([el].[IsEnrolled] = 1) AND [e].[EmployeeId] = @user AND [elt].[EndTime] is null
) AS Trainings


WHERE (TrainingId is null 
AND setupmoduleid in (SELECT DISTINCT  el.SetupModuleId from EnrollLearnings el
         join EnrollLearningTimes elt on el.EnrollLearningId = elt.EnrollLearningId
         where el.EmployeeId = @user AND (elt.EndTime is null OR elt.StartTime is null) AND el.SetupModuleId is not null))
OR 
trainingId  in
(SELECT DISTINCT el.TrainingId from EnrollLearnings el
         join EnrollLearningTimes elt on el.EnrollLearningId = elt.EnrollLearningId
         where el.EmployeeId = @user AND ((elt.EndTime is null OR elt.StartTime is null) OR el.IsPassed is null) AND el.TrainingId is not null)    
		
", param);

			return query;
		}

		//query for dealer people
		public IQueryable<GetRecommendedLearningModel> GetRecommendedLearningDealer(string outletId, List<int> positionIds, List<int> topicIds)
		{
			var positionIdString = String.Join(',', positionIds);
			var cmdAsString = $"(PositionId IN ({positionIdString}) OR TrainingId is NULL)";
			var topicIdString = String.Join(',', topicIds);

			var outletIdParam = new SqlParameter("outletId", outletId);

			var queryBuilder = $@"select TrainingId,Batch,CourseId,CourseName,ProgramTypeName,SetupModuleId,ModuleName,BlobId,MIME,MaterialTypeName,CreatedAt,UpdatedAt,ApprovedAt,MAX(TopicId) as TopicId,PositionId,OutletId from dbo.HomePage_AllRecommendedTraining
WHERE Id is not null and OutletId is not null
and {cmdAsString} ";
			if (topicIds.Count > 0)
			{
				queryBuilder = queryBuilder + $@"AND(TopicId in ({ topicIdString}) OR(CourseId is not null AND TrainingId is null)) ";
			}

			queryBuilder = queryBuilder + $@" group by PositionId,OutletId,TrainingId,Batch,CourseId,CourseName,ProgramTypeName,SetupModuleId,ModuleName,BlobId,MIME,MaterialTypeName,CreatedAt,UpdatedAt,ApprovedAt";

			var query = (RawSqlString)queryBuilder;

			var generateQuery = this
				.ContentRecommendedLearningModel
				.FromSql(query, outletIdParam);

			return generateQuery;
		}

		//query for TAM People
		public IQueryable<GetRecommendedLearningModel> GetRecommendedLearningTAM(List<int> positionIds, List<int> topicIds)
		{
			var positionIdString = String.Join(',', positionIds);
			var topicIdString = String.Join(',', topicIds);

            var queryBuilder = $@"select TrainingId,Batch,CourseId,CourseName,ProgramTypeName,SetupModuleId,ModuleName,BlobId,MIME,MaterialTypeName,CreatedAt,UpdatedAt,ApprovedAt,MAX(TopicId) as TopicId,PositionId,OutletId from dbo.HomePage_AllRecommendedTraining
WHERE PositionId IN ({positionIdString})";

			if (topicIds.Count > 0)
			{
				queryBuilder = queryBuilder + $@" and TopicId in ({ topicIdString}) ";
			}

			queryBuilder = queryBuilder + $@" group by PositionId,OutletId,TrainingId,Batch,CourseId,CourseName,ProgramTypeName,SetupModuleId,ModuleName,BlobId,MIME,MaterialTypeName,CreatedAt,UpdatedAt,ApprovedAt";

			var query = (RawSqlString)queryBuilder;

            var generateQuery = this
                .ContentRecommendedLearningModel
                .FromSql(query, 1);
            // JANGAN DITANYA '1' INI BUAT APA! INI SALAH EF CORE! JANGAN DIHAPUS!
            // Kalau dihapus, error 500. String interpolationnya ada yang error.
            // -Aldrian
            return generateQuery;
		}

		//query for dealer people
		public IQueryable<GetRecommendedLearningModel> GetRecommendedLearningModel(string outletId, List<int> positionIds, List<int> topicIds)
		{
			var positionIdString = String.Join(',', positionIds);
			var cmdAsString = $"(tpm.PositionId IN ({positionIdString}) OR t.TrainingId is NULL)";
			var topicIdString = String.Join(',', topicIds);

			var outletIdParam = new SqlParameter("outletId", outletId);

			var queryBuilder = $@"SELECT DISTINCT
	                CASE WHEN [t].[TrainingId] IS NULL THEN 0
	                ELSE [t].[TrainingId] END AS [TrainingId],
	                CASE WHEN [t].[TrainingId] IS NULL THEN 0
	                ELSE [t].[Batch] END AS [Batch],
	                [c].[CourseId],
	                [c].[CourseName],
	                [pt].[ProgramTypeName],
	                SetupModuleId = null,
	                ModuleName = null,
	                [c].[BlobId],
	                [b].[MIME],
	                MaterialTypeName = null,
	                CASE WHEN [t].[TrainingId] IS NULL THEN [c].SetupCourseCreatedAt
	                ELSE [t].CreatedAt END AS [CreatedAt],
	                CASE WHEN [t].[TrainingId] IS NULL THEN [c].SetupCourseUpdatedAt
	                ELSE [t].UpdatedAt END AS [UpdatedAt],
	                CASE WHEN [t].[TrainingId] IS NULL THEN [c].SetupCourseApprovedAt
	                ELSE [t].ApprovedAt END AS [ApprovedAt]
                FROM
	                SetupLearning sl WITH (NOLOCK)
                JOIN Courses c WITH (NOLOCK) ON
	                sl.CourseId = c.CourseId
                JOIN ProgramTypes pt WITH (NOLOCK) ON
	                c.ProgramTypeId = pt.ProgramTypeId
                LEFT JOIN Blobs b WITH (NOLOCK) ON
	                c.BlobId = b.BlobId
                LEFT JOIN Trainings t WITH (NOLOCK) ON
	                t.CourseId = c.CourseId
	                AND t.isDeleted = 0
                LEFT JOIN TrainingPositionMappings tpm WITH (NOLOCK) on tpm.TrainingId = t.TrainingId
                LEFT JOIN TrainingOutletMappings tom WITH (NOLOCK) ON
	                tom.TrainingId = t.TrainingId
	                AND @outletId = tom.OutletId
                LEFT JOIN TrainingModuleMappings tmm WITH (NOLOCK) on t.TrainingId = tmm.TrainingId
                LEFT JOIN SetupModules sm WITH (NOLOCK) on sm.SetupModuleId = tmm.SetupModuleId
                LEFT JOIN Modules m WITH (NOLOCK) on sm.ModuleId = m.ModuleId
                LEFT JOIN ModuleTopicMappings mtm WITH (NOLOCK) on m.ModuleId = mtm.ModuleId
                WHERE c.CourseApprovedAt IS NOT NULL
	                AND sl.CourseId IS NOT NULL
	                AND (c.IsDeleted = 0
	                OR t.isDeleted = 0 )
	                AND (tom.OutletId is not null
	                OR (t.TrainingId IS NULL
	                AND c.CourseId is not null ))
	                AND {cmdAsString}
                    AND (t.ApprovedAt is not null OR (c.CourseId is not null AND t.TrainingId is null)) ";

			if (topicIds.Count > 0)
			{
				queryBuilder = queryBuilder + $@"AND(mtm.TopicId in ({ topicIdString}) OR(c.CourseId is not null AND t.TrainingId is null)) ";
			}


			queryBuilder = queryBuilder + $@"
                UNION ALL

                SELECT DISTINCT
                    TrainingId = null,
	                Batch = null,
	                CourseId = null,
	                CourseName = null,
	                ProgramTypeName = null,
	                sm.SetupModuleId,
	                m.ModuleName,
	                b.BlobId,
	                b.MIME,
	                mt.MaterialTypeName,
	                sm.CreatedAt,
	                sm.UpdatedAt,
                    sm.Approvedat
                FROM
                    Modules m
                    JOIN SetupModules sm WITH (NOLOCK) ON m.ModuleId = sm.ModuleId
                    JOIN Blobs b WITH (NOLOCK) ON m.BlobId = b.BlobId
                    JOIN MaterialTypes mt WITH (NOLOCK) ON m.MaterialTypeId = mt.MaterialTypeId
                    JOIN ApprovalToSetupModules atm WITH (NOLOCK) ON sm.SetupModuleId = atm.SetupModuleId
                    JOIN Approvals a WITH (NOLOCK) ON atm.ApprovalId = a.ApprovalId
                    JOIN ModuleTopicMappings mtm WITH (NOLOCK) ON m.ModuleId = mtm.ModuleId";

			if (topicIds.Count > 0)
			{
				queryBuilder = queryBuilder + $@" and mtm.TopicId in ({ topicIdString})";
			}

			queryBuilder = queryBuilder + $@" WHERE sm.CourseId IS NULL AND sm.IsDeleted = 0 AND m.IsDeleted = 0 AND a.ApprovalStatusId = 1 AND sm.IsRecommendedForYou = 1";

			var query = (RawSqlString)queryBuilder;

			var generateQuery = this
				.ContentRecommendedLearningModel
				.FromSql(query, outletIdParam);

			return generateQuery;
		}

		//query for TAM people
		public IQueryable<GetRecommendedLearningModel> GetRecommendedLearningModel(List<int> positionIds, List<int> topicIds)
		{
			var positionIdString = String.Join(',', positionIds);
			var topicIdString = String.Join(',', topicIds);

			var queryBuilder = $@"SELECT DISTINCT
CASE
    WHEN [t].[TrainingId] IS NULL
    THEN 0 ELSE [t].[TrainingId]
END AS [TrainingId], 
CASE
    WHEN [t].[TrainingId] IS NULL
    THEN 0 ELSE [t].[Batch]
END AS [Batch],
[c].[CourseId],
[c].[CourseName],
[pt].[ProgramTypeName],
SetupModuleId = null,
ModuleName = null,
[c].[BlobId],
[b].[MIME],
MaterialTypeName = null,
CASE WHEN [t].[TrainingId] IS NULL THEN [c].SetupCourseCreatedAt
ELSE [t].CreatedAt END AS [CreatedAt],
CASE WHEN [t].[TrainingId] IS NULL THEN [c].SetupCourseUpdatedAt
ELSE [t].UpdatedAt END AS [UpdatedAt],
CASE WHEN [t].[TrainingId] IS NULL THEN [c].SetupCourseApprovedAt
ELSE [t].ApprovedAt END AS [ApprovedAt]
FROM 
SetupLearning sl WITH (NOLOCK)
JOIN Courses c WITH (NOLOCK) ON sl.CourseId = c.CourseId
JOIN ProgramTypes pt WITH (NOLOCK) ON c.ProgramTypeId = pt.ProgramTypeId
LEFT JOIN Blobs b WITH (NOLOCK) ON c.BlobId = b.BlobId
LEFT JOIN Trainings t WITH (NOLOCK) ON t.CourseId = c.CourseId AND t.isDeleted = 0
LEFT JOIN TrainingPositionMappings tpm WITH (NOLOCK) on tpm.TrainingId = t.TrainingId
LEFT JOIN TrainingModuleMappings tmm WITH (NOLOCK) on t.TrainingId = tmm.TrainingId
LEFT JOIN SetupModules sm WITH (NOLOCK) on sm.SetupModuleId = tmm.SetupModuleId
LEFT JOIN Modules m WITH (NOLOCK) on sm.ModuleId = m.ModuleId
LEFT JOIN ModuleTopicMappings mtm WITH (NOLOCK) on m.ModuleId = mtm.ModuleId
WHERE c.CourseApprovedAt IS NOT NULL 
	AND sl.CourseId IS NOT NULL 
	AND (c.IsDeleted = 0 OR t.isDeleted = 0 )
	AND (tpm.PositionId IN ({positionIdString}) OR t.TrainingId is NULL)
    AND (t.ApprovedAt is not null OR (c.CourseId is not null AND t.TrainingId is null))";

			if (topicIds.Count > 0)
			{
				queryBuilder = queryBuilder + $@" AND (mtm.TopicId in ({topicIdString}) OR (c.CourseId is not null AND t.TrainingId is null)) ";
			}

			queryBuilder = queryBuilder + $@"
UNION ALL

SELECT DISTINCT
	TrainingId = null,
	Batch = null,
	CourseId = null,
	CourseName = null,
	ProgramTypeName = null,
	sm.SetupModuleId,
	m.ModuleName,
	b.BlobId,
	b.MIME,
	mt.MaterialTypeName,
	sm.CreatedAt,
	sm.UpdatedAt,
    sm.Approvedat
FROM
	Modules m
	JOIN SetupModules sm WITH (NOLOCK) ON m.ModuleId = sm.ModuleId
	JOIN Blobs b WITH (NOLOCK) ON m.BlobId = b.BlobId
	JOIN MaterialTypes mt WITH (NOLOCK) ON m.MaterialTypeId = mt.MaterialTypeId
	JOIN ApprovalToSetupModules atm WITH (NOLOCK) ON sm.SetupModuleId = atm.SetupModuleId
	JOIN Approvals a WITH (NOLOCK) ON atm.ApprovalId = a.ApprovalId
	JOIN ModuleTopicMappings mtm WITH (NOLOCK) ON m.ModuleId = mtm.ModuleId";

			if (topicIds.Count > 0)
			{
				queryBuilder = queryBuilder + $@" and mtm.TopicId in ({ topicIdString}) ";
			}

			queryBuilder = queryBuilder + $@" WHERE sm.CourseId IS NULL AND sm.IsDeleted = 0 AND m.IsDeleted = 0 AND a.ApprovalStatusId = 1 AND sm.IsRecommendedForYou = 1";

			var query = (RawSqlString)queryBuilder;

			var generateQuery = this
				.ContentRecommendedLearningModel
				.FromSql(query, 1);
			// JANGAN DITANYA '1' INI BUAT APA! INI SALAH EF CORE! JANGAN DIHAPUS!
			// Kalau dihapus, error 500. String interpolationnya ada yang error.
			// -Aldrian
			return generateQuery;
		}

		public IQueryable<GetContinuedLearningModel> GetQueuedLearningModel(string user)
		{
			var param = new SqlParameter("user", user);

			var query = this
				.ContentContinuedLearningModels
				.FromSql($@"SELECT 
	[TrainingId] = CASE
		WHEN [t].[TrainingId] IS NULL
		THEN 0 ELSE [t].[TrainingId]
	END, 
	[Batch] = CASE
		WHEN [t].[TrainingId] IS NULL
		THEN 0 ELSE [t].[Batch]
	END,
	t.StartDate,
	t.EndDate,
	c.[CourseId], 
	c.[CourseName], 
	pt.[ProgramTypeName], 
	SetupModuleId = NULL,
	ModuleName = NULL,
	c.[BlobId], 
	[b].[MIME], 
	MaterialTypeName = NULL,
	[CreatedAt] = CASE
		WHEN [t].[TrainingId] IS NULL
			THEN c.SetupCourseCreatedAt  
		ELSE t.CreatedAt
	END, 
	[UpdatedAt] = CASE
		WHEN [t].[TrainingId] IS NULL
			THEN c.SetupCourseUpdatedAt 
		ELSE t.UpdatedAt
	END, 
	[ApprovedAt] = CASE
		WHEN [t].[TrainingId] IS NULL
			THEN c.SetupCourseApprovedAt  
		ELSE t.ApprovedAt
	END
FROM [Courses] AS c 
	LEFT JOIN [ProgramTypes] AS [pt] ON c.[ProgramTypeId] = pt.[ProgramTypeId]
	INNER JOIN [Blobs] AS [b] ON c.[BlobId] = [b].[BlobId]
	LEFT JOIN [Trainings] AS [t] ON c.[CourseId] = [t].[CourseId]
	JOIN EnrollLearnings [el] ON t.TrainingId = el.TrainingId
	JOIN Employees [e] ON el.EmployeeId = e.EmployeeId
	JOIN ApprovalToCourses [atc] ON c.CourseId = atc.CourseId
WHERE t.ApprovedAt IS NOT NULL AND t.isDeleted = 0 AND el.IsQueued = 1 AND e.EmployeeId = @user

UNION ALL

SELECT DISTINCT
	TrainingId = NULL,
	Batch = NULL,
	StartDate = NULL,
	EndDate = NULL,
	CourseId = NULL,
	CourseName = NULL,
	ProgramTypeName = NULL,
	sm.SetupModuleId,
	m.ModuleName,
	b.BlobId,
	b.MIME,
	mt.MaterialTypeName,
	sm.CreatedAt,
	sm.UpdatedAt,
	sm.ApprovedAt
FROM EnrollLearnings as el
	JOIN [SetupModules] AS [sm] ON [el].[SetupModuleId]= [sm].[SetupModuleId]
	JOIN Modules m ON sm.ModuleId = m.ModuleId
	JOIN [Blobs] AS [b] ON [m].[BlobId] = [b].[BlobId]
	JOIN [MaterialTypes] AS [mt] ON [m].[MaterialTypeId] = [mt].[MaterialTypeId]
	JOIN Employees [e] ON [el].[EmployeeId] = [e].[EmployeeId]
	JOIN EnrollLearningTimes [elt] ON [el].[EnrollLearningId] = [elt].[EnrollLearningId] 
	JOIN [ApprovalToSetupModules] AS [atm] ON [sm].[SetupModuleId] = [atm].[SetupModuleId]
	JOIN [Approvals] AS [a] ON [atm].[ApprovalId] = [a].[ApprovalId]
WHERE ((([sm].[CourseId] IS NULL AND ([sm].[IsDeleted] = 0)) AND ([m].[IsDeleted] = 0)) AND ([a].[ApprovalStatusId] = 1)) AND ([el].[IsQueued] = 1) AND [el].[EmployeeId] =  @user
", param);

			return query;
		}

		//query for dealer people
		public IQueryable<GetPopularLearningModel> GetPopularLearningDealer(int skip, int take, string outletId, List<int> positionId)
		{
			var contains = "";

			if (positionId.Count > 0)
			{
				contains = string.Join("','", positionId);
			}

			var query = this
	.ContentPopularLearningModel
	.FromSql($@"select TrainingId,Batch,CourseId,CourseName,ProgramTypeName,SetupModuleId,ModuleName,BlobId,MIME,MaterialTypeName,CreatedAt,UpdatedAt,ApprovedAt,Top5Course,WhoTookWhoLike,PositionId,OutletId from dbo.HomePage_AllPopularTraining
WHERE PositionId IN ({contains})
and OutletId = {outletId}
GROUP BY PositionId,OutletId,TrainingId,Batch,CourseId,CourseName,ProgramTypeName,SetupModuleId,ModuleName,BlobId,MIME,MaterialTypeName,CreatedAt,UpdatedAt,ApprovedAt,Top5Course,WhoTookWhoLike
ORDER BY
	Top5Course ASC,
	WhoTookWhoLike DESC
OFFSET {skip} ROWS
FETCH NEXT {take} ROWS ONLY");

			return query;
		}

		//query for TAM People
		public IQueryable<GetPopularLearningModel> GetPopularLearningTAM(int skip, int take, List<int> positionId)
        {
			var contains = "";

			if (positionId.Count > 0)
			{
				contains = string.Join("','", positionId);
			}

			var query = this
	.ContentPopularLearningModel
	.FromSql($@"select TrainingId,Batch,CourseId,CourseName,ProgramTypeName,SetupModuleId,ModuleName,BlobId,MIME,MaterialTypeName,CreatedAt,UpdatedAt,ApprovedAt,Top5Course,WhoTookWhoLike,PositionId,OutletId from dbo.HomePage_AllPopularTraining
WHERE PositionId IN ({contains})
and OutletId = 260
GROUP BY PositionId,OutletId,TrainingId,Batch,CourseId,CourseName,ProgramTypeName,SetupModuleId,ModuleName,BlobId,MIME,MaterialTypeName,CreatedAt,UpdatedAt,ApprovedAt,Top5Course,WhoTookWhoLike
ORDER BY
	Top5Course ASC,
	WhoTookWhoLike DESC
OFFSET {skip} ROWS
FETCH NEXT {take} ROWS ONLY");

			return query;
		}

        //query for dealer people
        public IQueryable<GetPopularLearningModel> GetPopularLearningModel(int skip, int take, string outletId, List<int> positionId)
        {
            var contains = "";

            if (positionId.Count > 0)
            {
                contains = string.Join("','", positionId);
            }

            var query = this
                .ContentPopularLearningModel
                .FromSql($@"SELECT DISTINCT
	CASE 
		WHEN t.TrainingId IS NULL THEN 0
		ELSE t.TrainingId END AS TrainingId,
		CASE WHEN t.TrainingId IS NULL THEN 0
		ELSE t.Batch 
	END AS Batch,
	c.CourseId,
	c.CourseName,
	pt.ProgramTypeName,
	SetupModuleId = null,
	ModuleName = null,
	c.BlobId,
	b.MIME,
	MaterialTypeName = null,
	[CreatedAt] = CASE
		WHEN [t].[TrainingId] IS NULL
			THEN c.SetupCourseCreatedAt  
		ELSE t.CreatedAt
	END, 
	[UpdatedAt] = CASE
		WHEN [t].[TrainingId] IS NULL
			THEN c.SetupCourseUpdatedAt 
		ELSE t.UpdatedAt
	END, 
	[ApprovedAt] = CASE
		WHEN [t].[TrainingId] IS NULL
			THEN c.SetupCourseApprovedAt  
		ELSE t.ApprovedAt
	END,
	t.Top5Course,
	WhoTookWhoLike = 0
FROM
	SetupLearning sl WITH (NOLOCK)
	JOIN Courses c WITH (NOLOCK) ON sl.CourseId = c.CourseId
	JOIN ProgramTypes pt WITH (NOLOCK) ON c.ProgramTypeId = pt.ProgramTypeId
	JOIN Blobs b WITH (NOLOCK) ON c.BlobId = b.BlobId
	LEFT JOIN Trainings t WITH (NOLOCK) ON t.CourseId = c.CourseId
	AND t.isdeleted = 0
	JOIN TrainingPositionMappings tpm WITH (NOLOCK) ON t.TrainingId = tpm.TrainingId
	JOIN TrainingOutletMappings tom WITH (NOLOCK) ON t.TrainingId = tom.TrainingId
WHERE
	t.ApprovedAt is not null
	AND t.Top5Course > 0 
	AND tpm.PositionId IN ({contains})
	AND tom.OutletId = {outletId}

UNION ALL

SELECT DISTINCT
	CASE 
		WHEN t.TrainingId IS NULL THEN 0
		ELSE t.TrainingId END AS TrainingId,
		CASE WHEN t.TrainingId IS NULL THEN 0
		ELSE t.Batch 
	END AS Batch,
	c.CourseId,
	c.CourseName,
	pt.ProgramTypeName,
	SetupModuleId = null,
	ModuleName = null,
	c.BlobId,
	b.MIME,
	MaterialTypeName = null,
	[CreatedAt] = CASE
		WHEN [t].[TrainingId] IS NULL
			THEN c.SetupCourseCreatedAt  
		ELSE t.CreatedAt
	END, 
	[UpdatedAt] = CASE
		WHEN [t].[TrainingId] IS NULL
			THEN c.SetupCourseUpdatedAt 
		ELSE t.UpdatedAt
	END, 
	[ApprovedAt] = CASE
		WHEN [t].[TrainingId] IS NULL
			THEN c.SetupCourseApprovedAt  
		ELSE t.ApprovedAt
	END,
	Top5Course = 10,
	(CASE WHEN tooks.TookCount IS NULL THEN 0 ELSE tooks.TookCount END) + (CASE WHEN likes.LikesCount IS NULL THEN 0 ELSE likes.LikesCount END) as WhoTookWhoLike
FROM
	SetupLearning sl WITH (NOLOCK)
	JOIN Courses c WITH (NOLOCK) ON sl.CourseId = c.CourseId
	JOIN ProgramTypes pt WITH (NOLOCK) ON c.ProgramTypeId = pt.ProgramTypeId
	JOIN Blobs b WITH (NOLOCK) ON c.BlobId = b.BlobId
	LEFT JOIN Trainings t WITH (NOLOCK) ON t.CourseId = c.CourseId
	AND t.isdeleted = 0
	LEFT JOIN (
        SELECT
            SUM(tr.RatingScore) AS LikesCount,
            t.trainingId
        FROM
            Trainings t WITH (NOLOCK)
            JOIN TrainingRatings AS tr WITH (NOLOCK) ON t.TrainingId = tr.TrainingId
        WHERE
            (tr.RatingScore > 0)
        GROUP BY
            t.TrainingId
        ) likes on likes.TrainingId = t.TrainingId
    LEFT JOIN (
        SELECT
            COUNT(e.EmployeeId) as TookCount,
            el.TrainingId
        FROM
            EnrollLearnings el WITH (NOLOCK)
            JOIN Employees e WITH (NOLOCK) ON el.EmployeeId = e.EmployeeId
        WHERE el.TrainingId IS NOT NULL
        GROUP BY
            el.TrainingId
        ) tooks on tooks.TrainingId = t.TrainingId
	LEFT JOIN TrainingPositionMappings tpm WITH (NOLOCK) ON t.TrainingId = tpm.TrainingId
	LEFT JOIN TrainingOutletMappings tom WITH (NOLOCK) ON t.TrainingId = tom.TrainingId
WHERE
	(t.ApprovedAt is not null OR (c.CourseId is not null AND t.TrainingId is null))
	AND (t.Top5Course = 0 OR (c.CourseId is not null AND t.TrainingId is null))
	AND c.IsPopular = 1 
	AND (tpm.PositionId IN ({contains}) OR (c.CourseId is not null AND t.TrainingId is null))
	AND (tom.OutletId = {outletId} OR (c.CourseId is not null AND t.TrainingId is null))

UNION ALL

SELECT
	DISTINCT TrainingId = null,
	Batch = null,
	CourseId = null,
	CourseName = null,
	ProgramTypeName = null,
	sm.SetupModuleId,
	m.ModuleName,
	b.BlobId,
	b.MIME,
	mt.MaterialTypeName,
	sm.CreatedAt,
	sm.UpdatedAt,
	sm.ApprovedAt,
	Top5Course = 10,
	WhoTookWhoLike = 0
FROM
	Modules m WITH (NOLOCK)
	JOIN SetupModules sm WITH (NOLOCK) ON m.ModuleId = sm.ModuleId
	JOIN Blobs b WITH (NOLOCK) ON m.BlobId = b.BlobId
	JOIN MaterialTypes mt WITH (NOLOCK) ON m.MaterialTypeId = mt.MaterialTypeId
	JOIN ApprovalToSetupModules atm WITH (NOLOCK) ON sm.SetupModuleId = atm.SetupModuleId
	JOIN Approvals a WITH (NOLOCK) ON atm.ApprovalId = a.ApprovalId
WHERE
	sm.CourseId IS NULL
	AND sm.IsDeleted = 0
	AND m.IsDeleted = 0
	AND a.ApprovalStatusId = 1
	AND sm.IsPopular = 1
	AND m.ApprovedAt is not null 
	AND sm.ApprovedAt is not null 
ORDER BY
	Top5Course ASC,
	WhoTookWhoLike DESC
OFFSET {skip} ROWS
FETCH NEXT {take} ROWS ONLY
");

            string text = query.ToString();

            return query;
        }

		//query for TAM people
		public IQueryable<GetPopularLearningModel> GetPopularLearningModel(int skip, int take, List<int> positionId)
        {
            var contains = "";

            if (positionId.Count > 0)
            {
                contains = string.Join("','", positionId);
            }

            var query = this
                .ContentPopularLearningModel
                .FromSql($@"SELECT DISTINCT
	CASE 
		WHEN t.TrainingId IS NULL THEN 0
		ELSE t.TrainingId END AS TrainingId,
		CASE WHEN t.TrainingId IS NULL THEN 0
		ELSE t.Batch 
	END AS Batch,
	c.CourseId,
	c.CourseName,
	pt.ProgramTypeName,
	SetupModuleId = null,
	ModuleName = null,
	c.BlobId,
	b.MIME,
	MaterialTypeName = null,
	[CreatedAt] = CASE
		WHEN [t].[TrainingId] IS NULL
			THEN c.SetupCourseCreatedAt  
		ELSE t.CreatedAt
	END, 
	[UpdatedAt] = CASE
		WHEN [t].[TrainingId] IS NULL
			THEN c.SetupCourseUpdatedAt 
		ELSE t.UpdatedAt
	END, 
	[ApprovedAt] = CASE
		WHEN [t].[TrainingId] IS NULL
			THEN c.SetupCourseApprovedAt  
		ELSE t.ApprovedAt
	END,
	t.Top5Course,
	WhoTookWhoLike = 0
FROM
	SetupLearning sl
	JOIN Courses c ON sl.CourseId = c.CourseId
	JOIN ProgramTypes pt ON c.ProgramTypeId = pt.ProgramTypeId
	JOIN Blobs b ON c.BlobId = b.BlobId
	LEFT JOIN Trainings t ON t.CourseId = c.CourseId
	AND t.isdeleted = 0
	JOIN TrainingPositionMappings tpm ON t.TrainingId = tpm.TrainingId
	JOIN TrainingOutletMappings tom ON t.TrainingId = tom.TrainingId
WHERE
	t.ApprovedAt is not null
	AND t.Top5Course > 0 
	AND tpm.PositionId IN ({contains})

UNION ALL

SELECT DISTINCT
	CASE 
		WHEN t.TrainingId IS NULL THEN 0
		ELSE t.TrainingId END AS TrainingId,
		CASE WHEN t.TrainingId IS NULL THEN 0
		ELSE t.Batch 
	END AS Batch,
	c.CourseId,
	c.CourseName,
	pt.ProgramTypeName,
	SetupModuleId = null,
	ModuleName = null,
	c.BlobId,
	b.MIME,
	MaterialTypeName = null,
	[CreatedAt] = CASE
		WHEN [t].[TrainingId] IS NULL
			THEN c.SetupCourseCreatedAt  
		ELSE t.CreatedAt
	END, 
	[UpdatedAt] = CASE
		WHEN [t].[TrainingId] IS NULL
			THEN c.SetupCourseUpdatedAt 
		ELSE t.UpdatedAt
	END, 
	[ApprovedAt] = CASE
		WHEN [t].[TrainingId] IS NULL
			THEN c.SetupCourseApprovedAt  
		ELSE t.ApprovedAt
	END,
	Top5Course = 10,
	(CASE WHEN tooks.TookCount IS NULL THEN 0 ELSE tooks.TookCount END) + (CASE WHEN likes.LikesCount IS NULL THEN 0 ELSE likes.LikesCount END) as WhoTookWhoLike
FROM
	SetupLearning sl
	JOIN Courses c ON sl.CourseId = c.CourseId
	JOIN ProgramTypes pt ON c.ProgramTypeId = pt.ProgramTypeId
	JOIN Blobs b ON c.BlobId = b.BlobId
	LEFT JOIN Trainings t ON t.CourseId = c.CourseId
	AND t.isdeleted = 0
	LEFT JOIN (
        SELECT
            SUM(tr.RatingScore) AS LikesCount,
            t.trainingId
        FROM
            Trainings t
            JOIN TrainingRatings AS tr ON t.TrainingId = tr.TrainingId
        WHERE
            (tr.RatingScore > 0)
        GROUP BY
            t.TrainingId
        ) likes on likes.TrainingId = t.TrainingId
    LEFT JOIN (
        SELECT
            COUNT(e.EmployeeId) as TookCount,
            el.TrainingId
        FROM
            EnrollLearnings el
            JOIN Employees e ON el.EmployeeId = e.EmployeeId
        WHERE el.TrainingId IS NOT NULL
        GROUP BY
            el.TrainingId
        ) tooks on tooks.TrainingId = t.TrainingId
	LEFT JOIN TrainingPositionMappings tpm ON t.TrainingId = tpm.TrainingId
WHERE
	(t.ApprovedAt is not null OR (c.CourseId is not null AND t.TrainingId is null))
	AND (t.Top5Course = 0 OR (c.CourseId is not null AND t.TrainingId is null))
	AND c.IsPopular = 1 
	AND (tpm.PositionId IN ({contains}) OR (c.CourseId is not null AND t.TrainingId is null))

UNION ALL

SELECT
	DISTINCT TrainingId = null,
	Batch = null,
	CourseId = null,
	CourseName = null,
	ProgramTypeName = null,
	sm.SetupModuleId,
	m.ModuleName,
	b.BlobId,
	b.MIME,
	mt.MaterialTypeName,
	sm.CreatedAt,
	sm.UpdatedAt,
	sm.ApprovedAt,
	Top5Course = 10,
	WhoTookWhoLike = 0
FROM
	Modules m
	JOIN SetupModules sm ON m.ModuleId = sm.ModuleId
	JOIN Blobs b ON m.BlobId = b.BlobId
	JOIN MaterialTypes mt ON m.MaterialTypeId = mt.MaterialTypeId
	JOIN ApprovalToSetupModules atm ON sm.SetupModuleId = atm.SetupModuleId
	JOIN Approvals a ON atm.ApprovalId = a.ApprovalId
WHERE
	sm.CourseId IS NULL
	AND sm.IsDeleted = 0
	AND m.IsDeleted = 0
	AND a.ApprovalStatusId = 1
	AND sm.IsPopular = 1
	AND m.ApprovedAt is not null 
	AND sm.ApprovedAt is not null 
ORDER BY
	Top5Course ASC,
	WhoTookWhoLike DESC
OFFSET {skip} ROWS
FETCH NEXT {take} ROWS ONLY
");

            return query;
        }

		//query for dealer people
		public IQueryable<GetPopularLearningModel> GetLatestLearningDealer(int skip, int take, string outletId, List<int> positionId)
        {
			var contains = "";

			if (positionId.Count > 0)
			{
				contains = string.Join("','", positionId);
			}

			var query = this
				.ContentLatestLearningModel
				.FromSql($@"select TrainingId,Batch,CourseId,CourseName,ProgramTypeName,SetupModuleId,ModuleName,BlobId,MIME,MaterialTypeName,CreatedAt,UpdatedAt,ApprovedAt,Top5Course,WhoTookWhoLike,PositionId,OutletId from dbo.HomePage_AllLatestTraining
WHERE PositionId IN ({contains})
and OutletId = {outletId}
group by PositionId,OutletId,TrainingId,Batch,CourseId,CourseName,ProgramTypeName,SetupModuleId,ModuleName,BlobId,MIME,MaterialTypeName,CreatedAt,UpdatedAt,ApprovedAt,Top5Course,WhoTookWhoLike
ORDER BY
	CreatedAt DESC
OFFSET {skip} ROWS
FETCH NEXT {take} ROWS ONLY");

			return query;
		}

		//query for TAM People
		public IQueryable<GetPopularLearningModel> GetLatestLearningTAM(int skip, int take, List<int> positionId)
        {
			var contains = "";

			if (positionId.Count > 0)
			{
				contains = string.Join("','", positionId);
			}

			var query = this
				.ContentLatestLearningModel
				.FromSql($@"select TrainingId,Batch,CourseId,CourseName,ProgramTypeName,SetupModuleId,ModuleName,BlobId,MIME,MaterialTypeName,CreatedAt,UpdatedAt,ApprovedAt,Top5Course,WhoTookWhoLike,PositionId,OutletId from dbo.HomePage_AllLatestTraining
WHERE PositionId IN ({contains})
and OutletId = 260
group by PositionId,OutletId,TrainingId,Batch,CourseId,CourseName,ProgramTypeName,SetupModuleId,ModuleName,BlobId,MIME,MaterialTypeName,CreatedAt,UpdatedAt,ApprovedAt,Top5Course,WhoTookWhoLike
ORDER BY
	CreatedAt DESC
OFFSET {skip} ROWS
FETCH NEXT {take} ROWS ONLY");

			return query;
		}

		//query for dealer people
		public IQueryable<GetPopularLearningModel> GetLatestLearningModel(int skip, int take, string outletId, List<int> positionId)
		{
			var contains = "";

			if (positionId.Count > 0)
			{
				contains = string.Join("','", positionId);
			}

			var query = this
				.ContentLatestLearningModel
				.FromSql($@"SELECT DISTINCT
	CASE 
		WHEN t.TrainingId IS NULL THEN 0
		ELSE t.TrainingId END AS TrainingId,
		CASE WHEN t.TrainingId IS NULL THEN 0
		ELSE t.Batch 
	END AS Batch,
	c.CourseId,
	c.CourseName,
	pt.ProgramTypeName,
	SetupModuleId = null,
	ModuleName = null,
	c.BlobId,
	b.MIME,
	MaterialTypeName = null,
	[CreatedAt] = CASE
		WHEN [t].[TrainingId] IS NULL
			THEN c.SetupCourseCreatedAt  
		ELSE t.CreatedAt
	END, 
	[UpdatedAt] = CASE
		WHEN [t].[TrainingId] IS NULL
			THEN c.SetupCourseUpdatedAt 
		ELSE t.UpdatedAt
	END, 
	[ApprovedAt] = CASE
		WHEN [t].[TrainingId] IS NULL
			THEN c.SetupCourseApprovedAt  
		ELSE t.ApprovedAt
	END,
	t.Top5Course,
	WhoTookWhoLike = 0
FROM
	SetupLearning sl WITH (NOLOCK)
	JOIN Courses c WITH (NOLOCK) ON sl.CourseId = c.CourseId
	JOIN ProgramTypes pt WITH (NOLOCK) ON c.ProgramTypeId = pt.ProgramTypeId
	JOIN Blobs b WITH (NOLOCK) ON c.BlobId = b.BlobId
	LEFT JOIN Trainings t WITH (NOLOCK) ON t.CourseId = c.CourseId
	AND t.isdeleted = 0
	JOIN TrainingPositionMappings tpm WITH (NOLOCK) ON t.TrainingId = tpm.TrainingId
	JOIN TrainingOutletMappings tom WITH (NOLOCK) ON t.TrainingId = tom.TrainingId
WHERE
	t.ApprovedAt is not null
	AND t.Top5Course > 0 
	AND tpm.PositionId IN ({contains})
	AND tom.OutletId = {outletId}

UNION ALL

SELECT DISTINCT
	CASE 
		WHEN t.TrainingId IS NULL THEN 0
		ELSE t.TrainingId END AS TrainingId,
		CASE WHEN t.TrainingId IS NULL THEN 0
		ELSE t.Batch 
	END AS Batch,
	c.CourseId,
	c.CourseName,
	pt.ProgramTypeName,
	SetupModuleId = null,
	ModuleName = null,
	c.BlobId,
	b.MIME,
	MaterialTypeName = null,
	[CreatedAt] = CASE
		WHEN [t].[TrainingId] IS NULL
			THEN c.SetupCourseCreatedAt  
		ELSE t.CreatedAt
	END, 
	[UpdatedAt] = CASE
		WHEN [t].[TrainingId] IS NULL
			THEN c.SetupCourseUpdatedAt 
		ELSE t.UpdatedAt
	END, 
	[ApprovedAt] = CASE
		WHEN [t].[TrainingId] IS NULL
			THEN c.SetupCourseApprovedAt  
		ELSE t.ApprovedAt
	END,
	Top5Course = 10,
	(CASE WHEN tooks.TookCount IS NULL THEN 0 ELSE tooks.TookCount END) + (CASE WHEN likes.LikesCount IS NULL THEN 0 ELSE likes.LikesCount END) as WhoTookWhoLike
FROM
	SetupLearning sl WITH (NOLOCK)
	JOIN Courses c WITH (NOLOCK) ON sl.CourseId = c.CourseId
	JOIN ProgramTypes pt WITH (NOLOCK) ON c.ProgramTypeId = pt.ProgramTypeId
	JOIN Blobs b WITH (NOLOCK) ON c.BlobId = b.BlobId
	LEFT JOIN Trainings t WITH (NOLOCK) ON t.CourseId = c.CourseId
	AND t.isdeleted = 0
	LEFT JOIN (
        SELECT
            SUM(tr.RatingScore) AS LikesCount,
            t.trainingId
        FROM
            Trainings t WITH (NOLOCK)
            JOIN TrainingRatings AS tr WITH (NOLOCK) ON t.TrainingId = tr.TrainingId
        WHERE
            (tr.RatingScore > 0)
        GROUP BY
            t.TrainingId
        ) likes on likes.TrainingId = t.TrainingId
    LEFT JOIN (
        SELECT
            COUNT(e.EmployeeId) as TookCount,
            el.TrainingId
        FROM
            EnrollLearnings el WITH (NOLOCK)
            JOIN Employees e WITH (NOLOCK) ON el.EmployeeId = e.EmployeeId
        WHERE el.TrainingId IS NOT NULL
        GROUP BY
            el.TrainingId
        ) tooks on tooks.TrainingId = t.TrainingId
	LEFT JOIN TrainingPositionMappings tpm WITH (NOLOCK) ON t.TrainingId = tpm.TrainingId
	LEFT JOIN TrainingOutletMappings tom WITH (NOLOCK) ON t.TrainingId = tom.TrainingId
WHERE
	(t.ApprovedAt is not null OR (c.CourseId is not null AND t.TrainingId is null))
	AND (t.Top5Course = 0 OR (c.CourseId is not null AND t.TrainingId is null))
	AND (tpm.PositionId IN ({contains}) OR (c.CourseId is not null AND t.TrainingId is null))
	AND (tom.OutletId = {outletId} OR (c.CourseId is not null AND t.TrainingId is null))

UNION ALL

SELECT
	DISTINCT TrainingId = null,
	Batch = null,
	CourseId = null,
	CourseName = null,
	ProgramTypeName = null,
	sm.SetupModuleId,
	m.ModuleName,
	b.BlobId,
	b.MIME,
	mt.MaterialTypeName,
	sm.CreatedAt,
	sm.UpdatedAt,
	sm.ApprovedAt,
	Top5Course = 10,
	WhoTookWhoLike = 0
FROM
	Modules m WITH (NOLOCK)
	JOIN SetupModules sm WITH (NOLOCK) ON m.ModuleId = sm.ModuleId
	JOIN Blobs b WITH (NOLOCK) ON m.BlobId = b.BlobId
	JOIN MaterialTypes mt WITH (NOLOCK) ON m.MaterialTypeId = mt.MaterialTypeId
	JOIN ApprovalToSetupModules atm WITH (NOLOCK) ON sm.SetupModuleId = atm.SetupModuleId
	JOIN Approvals a WITH (NOLOCK) ON atm.ApprovalId = a.ApprovalId
WHERE
	sm.CourseId IS NULL
	AND sm.IsDeleted = 0
	AND m.IsDeleted = 0
	AND a.ApprovalStatusId = 1
	AND m.ApprovedAt is not null 
	AND sm.ApprovedAt is not null 
ORDER BY
	CreatedAt DESC
OFFSET {skip} ROWS
FETCH NEXT {take} ROWS ONLY
");

			return query;
		}

        //query for TAM people
        public IQueryable<GetPopularLearningModel> GetLatestLearningModel(int skip, int take, List<int> positionId)
        {
            var contains = "";

            if (positionId.Count > 0)
            {
                contains = string.Join("','", positionId);
            }

            var query = this
                .ContentLatestLearningModel
                .FromSql($@"SELECT DISTINCT
	CASE 
		WHEN t.TrainingId IS NULL THEN 0
		ELSE t.TrainingId END AS TrainingId,
		CASE WHEN t.TrainingId IS NULL THEN 0
		ELSE t.Batch 
	END AS Batch,
	c.CourseId,
	c.CourseName,
	pt.ProgramTypeName,
	SetupModuleId = null,
	ModuleName = null,
	c.BlobId,
	b.MIME,
	MaterialTypeName = null,
	[CreatedAt] = CASE
		WHEN [t].[TrainingId] IS NULL
			THEN c.SetupCourseCreatedAt  
		ELSE t.CreatedAt
	END, 
	[UpdatedAt] = CASE
		WHEN [t].[TrainingId] IS NULL
			THEN c.SetupCourseUpdatedAt 
		ELSE t.UpdatedAt
	END, 
	[ApprovedAt] = CASE
		WHEN [t].[TrainingId] IS NULL
			THEN c.SetupCourseApprovedAt  
		ELSE t.ApprovedAt
	END,
	t.Top5Course,
	WhoTookWhoLike = 0
FROM
	SetupLearning sl WITH (NOLOCK)
	JOIN Courses c WITH (NOLOCK) ON sl.CourseId = c.CourseId
	JOIN ProgramTypes pt WITH (NOLOCK) ON c.ProgramTypeId = pt.ProgramTypeId
	JOIN Blobs b WITH (NOLOCK) ON c.BlobId = b.BlobId
	LEFT JOIN Trainings t WITH (NOLOCK) ON t.CourseId = c.CourseId
	AND t.isdeleted = 0
	JOIN TrainingPositionMappings tpm WITH (NOLOCK) ON t.TrainingId = tpm.TrainingId
	JOIN TrainingOutletMappings tom WITH (NOLOCK) ON t.TrainingId = tom.TrainingId
WHERE
	t.ApprovedAt is not null
	AND t.Top5Course > 0 
	AND tpm.PositionId IN ({contains})

UNION ALL

SELECT DISTINCT
	CASE 
		WHEN t.TrainingId IS NULL THEN 0
		ELSE t.TrainingId END AS TrainingId,
		CASE WHEN t.TrainingId IS NULL THEN 0
		ELSE t.Batch 
	END AS Batch,
	c.CourseId,
	c.CourseName,
	pt.ProgramTypeName,
	SetupModuleId = null,
	ModuleName = null,
	c.BlobId,
	b.MIME,
	MaterialTypeName = null,
	[CreatedAt] = CASE
		WHEN [t].[TrainingId] IS NULL
			THEN c.SetupCourseCreatedAt  
		ELSE t.CreatedAt
	END, 
	[UpdatedAt] = CASE
		WHEN [t].[TrainingId] IS NULL
			THEN c.SetupCourseUpdatedAt 
		ELSE t.UpdatedAt
	END, 
	[ApprovedAt] = CASE
		WHEN [t].[TrainingId] IS NULL
			THEN c.SetupCourseApprovedAt  
		ELSE t.ApprovedAt
	END,
	Top5Course = 10,
	(CASE WHEN tooks.TookCount IS NULL THEN 0 ELSE tooks.TookCount END) + (CASE WHEN likes.LikesCount IS NULL THEN 0 ELSE likes.LikesCount END) as WhoTookWhoLike
FROM
	SetupLearning sl WITH (NOLOCK)
	JOIN Courses c WITH (NOLOCK) ON sl.CourseId = c.CourseId
	JOIN ProgramTypes pt WITH (NOLOCK) ON c.ProgramTypeId = pt.ProgramTypeId
	JOIN Blobs b WITH (NOLOCK) ON c.BlobId = b.BlobId
	LEFT JOIN Trainings t WITH (NOLOCK) ON t.CourseId = c.CourseId
	AND t.isdeleted = 0
	LEFT JOIN (
        SELECT
            SUM(tr.RatingScore) AS LikesCount,
            t.trainingId
        FROM
            Trainings t WITH (NOLOCK)
            JOIN TrainingRatings AS tr WITH (NOLOCK) ON t.TrainingId = tr.TrainingId
        WHERE
            (tr.RatingScore > 0)
        GROUP BY
            t.TrainingId
        ) likes on likes.TrainingId = t.TrainingId
    LEFT JOIN (
        SELECT
            COUNT(e.EmployeeId) as TookCount,
            el.TrainingId
        FROM
            EnrollLearnings el WITH (NOLOCK)
            JOIN Employees e WITH (NOLOCK) ON el.EmployeeId = e.EmployeeId
        WHERE el.TrainingId IS NOT NULL
        GROUP BY
            el.TrainingId
        ) tooks on tooks.TrainingId = t.TrainingId
	LEFT JOIN TrainingPositionMappings tpm WITH (NOLOCK) ON t.TrainingId = tpm.TrainingId
WHERE
	(t.ApprovedAt is not null OR (c.CourseId is not null AND t.TrainingId is null))
	AND (t.Top5Course = 0 OR (c.CourseId is not null AND t.TrainingId is null))
	AND (tpm.PositionId IN ({contains}) OR (c.CourseId is not null AND t.TrainingId is null))

UNION ALL

SELECT
	DISTINCT TrainingId = null,
	Batch = null,
	CourseId = null,
	CourseName = null,
	ProgramTypeName = null,
	sm.SetupModuleId,
	m.ModuleName,
	b.BlobId,
	b.MIME,
	mt.MaterialTypeName,
	sm.CreatedAt,
	sm.UpdatedAt,
	sm.ApprovedAt,
	Top5Course = 10,
	WhoTookWhoLike = 0
FROM
	Modules m WITH (NOLOCK)
	JOIN SetupModules sm WITH (NOLOCK) ON m.ModuleId = sm.ModuleId
	JOIN Blobs b WITH (NOLOCK) ON m.BlobId = b.BlobId
	JOIN MaterialTypes mt WITH (NOLOCK) ON m.MaterialTypeId = mt.MaterialTypeId
	JOIN ApprovalToSetupModules atm WITH (NOLOCK) ON sm.SetupModuleId = atm.SetupModuleId
	JOIN Approvals a WITH (NOLOCK) ON atm.ApprovalId = a.ApprovalId
WHERE
	sm.CourseId IS NULL
	AND sm.IsDeleted = 0
	AND m.IsDeleted = 0
	AND a.ApprovalStatusId = 1
	AND m.ApprovedAt is not null 
	AND sm.ApprovedAt is not null 
ORDER BY
	CreatedAt DESC
OFFSET {skip} ROWS
FETCH NEXT {take} ROWS ONLY
");

            return query;
        }

        public IQueryable<GetAllLearningModel> GetAllLearningModels()
        {
            var query = this
                .AllLearningQuery
                .FromSql($@"SELECT *
FROM (
SELECT
	DISTINCT
	CASE WHEN [t].[TrainingId] IS NULL THEN NULL
	ELSE [t].[TrainingId] END AS [TrainingId],
	CASE WHEN [t].[TrainingId] IS NULL THEN NULL
	ELSE [t].[Batch] END AS [Batch],
	[c].[CourseId],
	[c].[CourseName],
	[pt].[ProgramTypeName],
	[pt].[ProgramTypeId],
	[c].[LearningTypeId],
	SetupModuleId = null,
	ModuleName = null,
	[c].[BlobId],
	[b].[MIME],
	MaterialTypeName = null,
	tmt.MaterialTypeId,
	[c].[CreatedAt],
	[c].[UpdatedAt],
	[c].[CourseApprovedAt] AS [ApprovedAt],
	[c].IsPopular
FROM
	SetupLearning sl
JOIN Courses c ON
	sl.CourseId = c.CourseId
	and c.IsDeleted = 0
LEFT JOIN Trainings t ON
	t.CourseId = c.CourseId
	AND t.isDeleted = 0
left Join Trainings t2 on
	t.CourseId = t2.CourseId
	and t.Batch < t2.Batch
JOIN ProgramTypes pt ON
	c.ProgramTypeId = pt.ProgramTypeId
JOIN Blobs b ON
	c.BlobId = b.BlobId
left join (
	SELECT
		tmt.TrainingId ,
		STRING_AGG((tmt.MaterialTypeId),
		',') as MaterialTypeId
	from
		(
		SELECT
			distinct t.TrainingId ,
			m.MaterialTypeId
		from
			Trainings t
		join TrainingModuleMappings tmm on
			t.TrainingId = tmm.TrainingId
		join SetupModules sm on
			sm.SetupModuleId = tmm.SetupModuleId
		join Modules m on
			sm.ModuleId = m.ModuleId
		where
			sm.IsForRemedial = 0) as tmt
	Group by
		tmt.TrainingId) as tmt on
	t.TrainingId = tmt.TrainingId
WHERE
	c.CourseApprovedAt IS NOT NULL
	AND sl.CourseId IS NOT NULL
	and t2.Batch is null
UNION ALL
SELECT
	DISTINCT TrainingId = null,
	Batch = null,
	CourseId = null,
	CourseName = null,
	ProgramTypeName = null,
	ProgramTypeId = null,
	LearningTypeId = null,
	sm.SetupModuleId,
	m.ModuleName,
	b.BlobId,
	b.MIME,
	mt.MaterialTypeName,
	CAST( m.MaterialTypeId as NVARCHAR(5)),
	m.CreatedAt,
	m.UpdatedAt,
	m.Approvedat,
	sm.IsPopular
FROM
	Modules m
JOIN SetupModules sm ON
	m.ModuleId = sm.ModuleId
JOIN Blobs b ON
	m.BlobId = b.BlobId
JOIN MaterialTypes mt ON
	m.MaterialTypeId = mt.MaterialTypeId
JOIN ApprovalToSetupModules atm ON
	sm.SetupModuleId = atm.SetupModuleId
JOIN Approvals a ON
	atm.ApprovalId = a.ApprovalId
WHERE
	sm.CourseId IS NULL
	AND sm.IsDeleted = 0
	AND m.IsDeleted = 0
	AND a.ApprovalStatusId = 1
) AS [a]
");

            return query;
        }

        public IQueryable<GetAllLearningUpdateModel> GetAllLearningUpdateModels(List<int> positionIds)
        {
            var positionIdString = String.Join(',', positionIds);
            var query = this
                .AllLearningQueryUpdate
                .FromSql($@"
SELECT *
FROM (
SELECT DISTINCT
	CASE 
		WHEN [t].[TrainingId] IS NULL THEN NULL
		ELSE [t].[TrainingId] 
	END AS [TrainingId],
	CASE 
		WHEN [t].[TrainingId] IS NULL THEN NULL
		ELSE [t].[Batch]
	END AS [Batch],
	CASE 
		WHEN [t].[TrainingId] IS NULL THEN NULL
		ELSE rt.Ratings 
	END AS [Ratings],
	[c].[CourseId],
	[c].[CourseName],
	[pt].[ProgramTypeName],
	[pt].[ProgramTypeId],
	[c].[LearningTypeId],
	SetupModuleId = null,
	ModuleName = null,
	[c].[BlobId],
	[b].[MIME],
	MaterialTypeName = null,
	cmtt.MaterialTypeId,
	CASE 
		WHEN [t].[TrainingId] IS NULL THEN [c].[SetupCourseCreatedAt]
		ELSE [t].[CreatedAt] 
	END AS [CreatedAt],
	CASE 
		WHEN [t].[TrainingId] IS NULL THEN [c].[SetupCourseUpdatedAt]
		ELSE [t].[UpdatedAt] 
	END AS [UpdatedAt],
	CASE 
		WHEN [t].[TrainingId] IS NULL THEN [c].[SetupCourseApprovedAt]
		ELSE [t].[ApprovedAt] 
	END AS [ApprovedAt],
	CASE
	    WHEN [c].IsPopular = 1 THEN CAST([c].IsPopular AS BIT)
	    WHEN [c].IsPopular = 0 THEN CAST([c].IsPopular AS BIT)
	    ELSE CAST(0 AS BIT)
	END AS IsPopular,
	(CASE WHEN tooks.TookCount IS NULL THEN 0 ELSE tooks.TookCount END) + (CASE WHEN likes.LikesCount IS NULL THEN 0 ELSE likes.LikesCount END) as WhoTookWhoLike,
	CASE 
		WHEN [t].[TrainingId] IS NULL THEN 10
		WHEN [t].[Top5Course] = 0 THEN 10
		ELSE [t].[Top5Course]
	END AS [Top5Course],
	CASE 
		WHEN [c].CoursePrice > 0 THEN 1
		ELSE 0
	END AS [CoursePrice]
FROM SetupLearning sl
JOIN Courses c ON sl.CourseId = c.CourseId and c.IsDeleted = 0 AND c.SetupCourseApprovedAt IS NOT NULL
LEFT JOIN Trainings t ON t.CourseId = c.CourseId AND t.ApprovedAt IS NOT NULL AND t.isDeleted = 0
LEFT JOIN Trainings t2 ON t.CourseId = t2.CourseId AND t.Batch < t2.Batch AND t2.isDeleted = 0
JOIN ProgramTypes pt ON c.ProgramTypeId = pt.ProgramTypeId
JOIN Blobs b ON c.BlobId = b.BlobId
LEFT JOIN (
	SELECT t.TrainingId, CAST((SUM(tr.RatingScore * 1.0)/ COUNT(tr.TrainingId)) AS float) AS Ratings
		FROM Trainings t
		JOIN TrainingRatings tr ON t.TrainingId = tr.TrainingId
		GROUP BY t.TrainingId
) AS rt ON t.TrainingId = rt.TrainingId
LEFT JOIN TrainingPositionMappings tpm on tpm.TrainingId = t.TrainingId
LEFT JOIN (
	SELECT cmt.CourseId,
	STRING_AGG((cmt.MaterialTypeId),',') as MaterialTypeId
	FROM
	(SELECT DISTINCT sm.CourseId, m.MaterialTypeId
	FROM SetupModules sm
	JOIN Modules m ON sm.ModuleId = m.ModuleId
	WHERE CourseId IS NOT NULL) AS cmt
	GROUP BY cmt.CourseId
) AS cmtt ON cmtt.CourseId = c.CourseId
LEFT JOIN (
	SELECT
	SUM(tr.RatingScore) AS LikesCount,
	t.trainingId
	FROM
	Trainings t
	JOIN TrainingRatings AS tr ON t.TrainingId = tr.TrainingId
	WHERE
	(tr.RatingScore > 0)
	GROUP BY
	t.TrainingId
) likes on likes.TrainingId = t.TrainingId
LEFT JOIN (
	SELECT
	COUNT(e.EmployeeId) as TookCount,
	el.TrainingId
	FROM
	EnrollLearnings el
	JOIN Employees e ON el.EmployeeId = e.EmployeeId
	WHERE el.TrainingId IS NOT NULL
	GROUP BY
	el.TrainingId
) tooks on tooks.TrainingId = t.TrainingId
WHERE
	c.CourseApprovedAt IS NOT NULL
	AND sl.CourseId IS NOT NULL
	AND t2.Batch IS NULL
	AND (tpm.PositionId IN ({positionIdString}) OR t.TrainingId is NULL)

UNION ALL
SELECT
	DISTINCT TrainingId = null,
	Batch = null,
	Ratings = null,
	CourseId = null,
	CourseName = null,
	ProgramTypeName = null,
	ProgramTypeId = null,
	LearningTypeId = null,
	sm.SetupModuleId,
	m.ModuleName,
	b.BlobId,
	b.MIME,
	mt.MaterialTypeName,
	CAST( m.MaterialTypeId as NVARCHAR(5)),
	sm.CreatedAt,
	sm.UpdatedAt,
	sm.Approvedat,
	sm.IsPopular,
	WhoTookWhoLike = 0,
	Top5Course = 10,
	CoursePrice = 0
FROM
	Modules m
JOIN SetupModules sm ON
	m.ModuleId = sm.ModuleId
JOIN Blobs b ON
	m.BlobId = b.BlobId
JOIN MaterialTypes mt ON
	m.MaterialTypeId = mt.MaterialTypeId
JOIN ApprovalToSetupModules atm ON
	sm.SetupModuleId = atm.SetupModuleId
JOIN Approvals a ON
	atm.ApprovalId = a.ApprovalId
WHERE
	sm.CourseId IS NULL
	AND sm.ApprovedAt IS NOT NULL
	AND sm.IsDeleted = 0
	AND m.IsDeleted = 0
	AND a.ApprovalStatusId = 1
) AS [a]
", 1);

            return query;
        }
        public IQueryable<GetAllLearningUpdateModel> GetAllLearningUpdateModels(String outletId, List<int> positionIds)
        {
            var positionIdString = String.Join(',', positionIds);
            var cmdAsString = $"(tpm.PositionId IN ({positionIdString}) OR t.TrainingId is NULL)";
            var outletIdParam = new SqlParameter("outletId", outletId);

            var query = this
                .AllLearningQueryUpdate
                .FromSql($@"
SELECT *
FROM (
SELECT DISTINCT
	CASE 
		WHEN [t].[TrainingId] IS NULL THEN NULL
		ELSE [t].[TrainingId] 
	END AS [TrainingId],
	CASE 
		WHEN [t].[TrainingId] IS NULL THEN NULL
		ELSE [t].[Batch]
	END AS [Batch],
	CASE 
		WHEN [t].[TrainingId] IS NULL THEN NULL
		ELSE rt.Ratings 
	END AS [Ratings],
	[c].[CourseId],
	[c].[CourseName],
	[pt].[ProgramTypeName],
	[pt].[ProgramTypeId],
	[c].[LearningTypeId],
	SetupModuleId = NULL,
	ModuleName = NULL,
	[c].[BlobId],
	[b].[MIME],
	MaterialTypeName = NULL,
	cmtt.MaterialTypeId,
	CASE
		WHEN [t].[TrainingId] IS NULL THEN [c].[SetupCourseCreatedAt]
		ELSE [t].[CreatedAt] 
	END AS [CreatedAt],
	CASE
		WHEN [t].[TrainingId] IS NULL THEN [c].[SetupCourseUpdatedAt]
		ELSE [t].[UpdatedAt] 
	END AS [UpdatedAt],
	CASE
		WHEN [t].[TrainingId] IS NULL THEN [c].[SetupCourseApprovedAt]
		ELSE [t].[ApprovedAt] 
	END AS [ApprovedAt],
	CASE
	    WHEN [c].IsPopular = 1 THEN CAST([c].IsPopular AS BIT)
	    WHEN [c].IsPopular = 0 THEN CAST([c].IsPopular AS BIT)
	    ELSE CAST(0 AS BIT)
	END AS IsPopular,
	(CASE WHEN tooks.TookCount IS NULL THEN 0 ELSE tooks.TookCount END) + (CASE WHEN likes.LikesCount IS NULL THEN 0 ELSE likes.LikesCount END) as WhoTookWhoLike,
	CASE 
		WHEN [t].[TrainingId] IS NULL THEN 10
		WHEN [t].[Top5Course] = 0 THEN 10
		ELSE [t].[Top5Course]
	END AS [Top5Course],
	CASE 
		WHEN [c].CoursePrice > 0 THEN 1
		ELSE 0
	END AS [CoursePrice]
FROM SetupLearning sl
JOIN Courses c ON sl.CourseId = c.CourseId AND c.IsDeleted = 0 AND c.SetupCourseApprovedAt IS NOT NULL
LEFT JOIN Trainings t ON t.CourseId = c.CourseId AND t.isDeleted = 0 AND t.ApprovedAt IS NOT NULL
LEFT JOIN Trainings t2 ON t.CourseId = t2.CourseId AND t.Batch < t2.Batch AND t2.isDeleted = 0
JOIN ProgramTypes pt ON c.ProgramTypeId = pt.ProgramTypeId
JOIN Blobs b ON c.BlobId = b.BlobId
LEFT JOIN TrainingPositionMappings tpm on tpm.TrainingId = t.TrainingId
LEFT JOIN TrainingOutletMappings tom ON tom.TrainingId = t.TrainingId AND @outletId = tom.OutletId
LEFT JOIN (
	SELECT t.TrainingId, CAST((SUM(tr.RatingScore * 1.0)/ COUNT(tr.TrainingId)) AS float) AS Ratings
	FROM Trainings t
	JOIN TrainingRatings tr ON t.TrainingId = tr.TrainingId
	GROUP BY t.TrainingId
) AS rt ON t.TrainingId = rt.TrainingId
LEFT JOIN (
	SELECT cmt.CourseId,
	STRING_AGG((cmt.MaterialTypeId),',') as MaterialTypeId
	FROM
	(SELECT DISTINCT sm.CourseId, m.MaterialTypeId
	FROM SetupModules sm
	JOIN Modules m ON sm.ModuleId = m.ModuleId
	WHERE CourseId IS NOT NULL) AS cmt
	GROUP BY cmt.CourseId
) AS cmtt ON cmtt.CourseId = c.CourseId
LEFT JOIN (
	SELECT
	SUM(tr.RatingScore) AS LikesCount,
	t.trainingId
	FROM
	Trainings t
	JOIN TrainingRatings AS tr ON t.TrainingId = tr.TrainingId
	WHERE
	(tr.RatingScore > 0)
	GROUP BY
	t.TrainingId
) likes on likes.TrainingId = t.TrainingId
LEFT JOIN (
	SELECT
	COUNT(e.EmployeeId) as TookCount,
	el.TrainingId
	FROM
	EnrollLearnings el
	JOIN Employees e ON el.EmployeeId = e.EmployeeId
	WHERE el.TrainingId IS NOT NULL
	GROUP BY
	el.TrainingId
) tooks on tooks.TrainingId = t.TrainingId
WHERE
	c.CourseApprovedAt IS NOT NULL
	AND sl.CourseId IS NOT NULL
	AND (tom.OutletId IS NOT NULL OR (t.TrainingId IS NULL AND c.CourseId IS NOT NULL))
	AND {cmdAsString}

UNION ALL

SELECT
	DISTINCT TrainingId = null,
	Batch = null,
	Ratings = null,
	CourseId = null,
	CourseName = null,
	ProgramTypeName = null,
	ProgramTypeId = null,
	LearningTypeId = null,
	sm.SetupModuleId,
	m.ModuleName,
	b.BlobId,
	b.MIME,
	mt.MaterialTypeName,
	CAST( m.MaterialTypeId as NVARCHAR(5)),
	sm.CreatedAt,
	sm.UpdatedAt,
	sm.Approvedat,
	sm.IsPopular,
	WhoTookWhoLike = 0,
	Top5Course = 10,
	CoursePrice = 0
FROM
	Modules m
JOIN SetupModules sm ON
	m.ModuleId = sm.ModuleId
JOIN Blobs b ON
	m.BlobId = b.BlobId
JOIN MaterialTypes mt ON
	m.MaterialTypeId = mt.MaterialTypeId
JOIN ApprovalToSetupModules atm ON
	sm.SetupModuleId = atm.SetupModuleId
JOIN Approvals a ON
	atm.ApprovalId = a.ApprovalId
WHERE
	sm.CourseId IS NULL
	AND sm.ApprovedAt IS NOT NULL
	AND sm.IsDeleted = 0
	AND m.IsDeleted = 0
	AND a.ApprovalStatusId = 1
) AS [a]
", outletIdParam);

            return query;
        }

        public IQueryable<GetContinuedLearningModel> GetCompletedLearningModel(string user)
        {
            var param = new SqlParameter("user", user);

            var query = this
                .ContentContinuedLearningModels
                .FromSql($@"SELECT * from
(SELECT DISTINCT
	[TrainingId] = CASE
		WHEN [t].[TrainingId] IS NULL
		THEN 0 ELSE [t].[TrainingId]
	END, 
	[Batch] = CASE
		WHEN [t].[TrainingId] IS NULL
		THEN 0 ELSE [t].[Batch]
	END,
	t.StartDate,
	t.EndDate,
	c.[CourseId], 
	c.[CourseName], 
	pt.[ProgramTypeName], 
	SetupModuleId = NULL,
	ModuleName = NULL,
	c.[BlobId], 
	[b].[MIME], 
	MaterialTypeName = NULL,
	c.[CreatedAt], 
	c.[UpdatedAt], 
	[t].[ApprovedAt]
FROM [Courses] AS c 
	LEFT JOIN [ProgramTypes] AS [pt] ON c.[ProgramTypeId] = pt.[ProgramTypeId]
	INNER JOIN [Blobs] AS [b] ON c.[BlobId] = [b].[BlobId]
	LEFT JOIN [Trainings] AS [t] ON c.[CourseId] = [t].[CourseId]
	JOIN EnrollLearnings [el] ON t.TrainingId = el.TrainingId
	LEFT JOIN EnrollLearningTimes [elt] ON el.EnrollLearningId = elt.EnrollLearningId
	JOIN Employees [e] ON el.EmployeeId = e.EmployeeId
	JOIN ApprovalToCourses [atc] ON c.CourseId = atc.CourseId
	JOIN Approvals a ON atc.ApprovalId = a.ApprovalId
WHERE a.ApprovalStatusId = 1 AND t.ApprovedAt IS NOT NULL AND c.CourseApprovedAt is not null AND elt.StartTime IS NOT NULL AND e.EmployeeId = @user AND elt.EndTime IS NOT NULL AND el.IsPassed IS NOT NULL

UNION ALL

SELECT DISTINCT
	TrainingId = NULL,
	Batch = NULL,
	StartDate = NULL,
	EndDate = NULL,
	CourseId = NULL,
	CourseName = NULL,
	ProgramTypeName = NULL,
	sm.SetupModuleId,
	m.ModuleName,
	b.BlobId,
	b.MIME,
	mt.MaterialTypeName,
	sm.CreatedAt,
	sm.UpdatedAt,
	sm.ApprovedAt
FROM EnrollLearnings el
JOIN [SetupModules] AS [sm] ON el.SetupModuleId = [sm].SetupModuleId 
JOIN Modules AS m ON [m].[ModuleId] = [sm].[ModuleId]
JOIN [Blobs] AS [b] ON [m].[BlobId] = [b].[BlobId]
JOIN [MaterialTypes] AS [mt] ON [m].[MaterialTypeId] = [mt].[MaterialTypeId]
JOIN [ApprovalToSetupModules] AS [atm] ON [sm].[SetupModuleId] = [atm].[SetupModuleId]
JOIN [Approvals] AS [a] ON [atm].[ApprovalId] = [a].[ApprovalId]
WHERE ((([sm].[CourseId] IS NULL AND ([sm].[IsDeleted] = 0)) AND ([m].[IsDeleted] = 0)) AND ([a].[ApprovalStatusId] = 1)) 
	AND ([el].[IsEnrolled] = 1) AND [el].[EmployeeId] = @user AND el.IsPassed IS NOT NULL
) AS Trainings
		", param);

            return query;
        }

        public IQueryable<LearningViewModel> GetAllLearning()
        {
            var query = this
                .LearningSearch
                .FromSql($@"select DISTINCT
t.TrainingId as TrainingId ,
t.Batch as TrainingBatch ,
c.BlobId as ImageUrl ,
c.CourseName as TrainingName ,
tr.RatingScore as Rating ,
pt.ProgramTypeName as ProgramTypeName ,
c.IsPopular as IsPopular,
t.StartDate as StartDate,
t.EndDate as EndDate,
t.CreatedAt as CreatedAt,
t.UpdatedAt as UpdatedAt,
b.Mime as Mime
from courses c 
join ApprovalToCourses atc on c.CourseId = atc.CourseId
join Approvals a on atc.ApprovalId = atc.ApprovalId
join ProgramTypes pt on c.ProgramTypeId = pt.ProgramTypeId
left join blobs b on c.BlobId = b.BlobId
join trainings t on c.CourseId = t.CourseId
left join TrainingRatings tr on t.TrainingId = tr.TrainingId
join LearningTypes lt on c.LearningTypeId = lt.LearningTypeId
left join SetupModules sm on c.CourseId = sm.CourseId
left join Modules m on sm.ModuleId = m.ModuleId
join MaterialTypes mt on m.MaterialTypeId = mt.MaterialTypeId
where a.ApprovalStatusId = 1 and c.IsDeleted = 0");

            return query;
        }

        //Badges
        public IQueryable<GetLearningBadges> GetCompletedBadgesModel(string user)
        {
            var param = new SqlParameter("user", user);

            var query = this.LearningBadgesQuery
                .FromSql($@"SELECT * from
(SELECT DISTINCT
	[TrainingId] = CASE
		WHEN [t].[TrainingId] IS NULL
		THEN 0 ELSE [t].[TrainingId]
	END, 
	[Batch] = CASE
		WHEN [t].[TrainingId] IS NULL
		THEN 0 ELSE [t].[Batch]
	END,
	t.StartDate,
	t.EndDate,
	c.[CourseId], 
	c.[CourseName], 
	pt.[ProgramTypeName], 
	SetupModuleId = NULL,
	ModuleName = NULL,
	tp.[BlobId], 
	[bt].[MIME], 
	MaterialTypeName = NULL,
	CASE WHEN [t].[TrainingId] IS NULL THEN [c].SetupCourseCreatedAt
	ELSE [t].CreatedAt END AS [CreatedAt],
	CASE WHEN [t].[TrainingId] IS NULL THEN [c].SetupCourseUpdatedAt
	ELSE [t].UpdatedAt END AS [UpdatedAt],
	CASE WHEN [t].[TrainingId] IS NULL THEN [c].SetupCourseApprovedAt
	ELSE [t].ApprovedAt END AS [ApprovedAt],
	tp.TopicId AS [TopicId],
	eb.EBadgeId as [BadgeId],
	tp.TopicName
FROM [Courses] AS c 
	LEFT JOIN [ProgramTypes] AS [pt] ON c.[ProgramTypeId] = pt.[ProgramTypeId]
	INNER JOIN [Blobs] AS [b] ON c.[BlobId] = [b].[BlobId]
	LEFT JOIN [Trainings] AS [t] ON c.[CourseId] = [t].[CourseId]
	JOIN EnrollLearnings [el] ON t.TrainingId = el.TrainingId
	LEFT JOIN EnrollLearningTimes [elt] ON el.EnrollLearningId = elt.EnrollLearningId
	JOIN Employees [e] ON el.EmployeeId = e.EmployeeId
	JOIN ApprovalToCourses [atc] ON c.CourseId = atc.CourseId
	JOIN Approvals a ON atc.ApprovalId = a.ApprovalId
	JOIN SetupModules sm ON c.CourseId = sm.CourseId
	JOIN Modules m ON sm.ModuleId = m.ModuleId
	JOIN ModuleTopicMappings mtm ON m.ModuleId = mtm.ModuleId
	JOIN Topics tp ON mtm.TopicId = tp.TopicId
	JOIN [Blobs] AS [bt] ON [tp].[BlobId] = [bt].[BlobId]
	JOIN EBadges eb ON tp.EBadgeId = eb.EBadgeId
WHERE a.ApprovalStatusId = 1 AND t.ApprovedAt IS NOT NULL AND c.CourseApprovedAt is not null AND elt.StartTime IS NOT NULL AND e.EmployeeId = @user AND elt.EndTime IS NOT NULL AND el.IsPassed IS NOT NULL

UNION ALL

SELECT DISTINCT
	TrainingId = NULL,
	Batch = NULL,
	StartDate = NULL,
	EndDate = NULL,
	CourseId = NULL,
	CourseName = NULL,
	ProgramTypeName = NULL,
	sm.SetupModuleId,
	m.ModuleName,
	tp.BlobId,
	bt.MIME,
	mt.MaterialTypeName,
	sm.CreatedAt,
	sm.UpdatedAt,
	sm.ApprovedAt,
	tp.TopicId AS [TopicId],
	eb.EBadgeId as [BadgeId],
	tp.TopicName
FROM EnrollLearnings el
JOIN [SetupModules] AS [sm] ON el.SetupModuleId = [sm].SetupModuleId 
JOIN Modules AS m ON [m].[ModuleId] = [sm].[ModuleId]
JOIN [Blobs] AS [b] ON [m].[BlobId] = [b].[BlobId]
JOIN [MaterialTypes] AS [mt] ON [m].[MaterialTypeId] = [mt].[MaterialTypeId]
JOIN [ApprovalToSetupModules] AS [atm] ON [sm].[SetupModuleId] = [atm].[SetupModuleId]
JOIN [Approvals] AS [a] ON [atm].[ApprovalId] = [a].[ApprovalId]
JOIN ModuleTopicMappings mtm ON m.ModuleId = mtm.ModuleId
JOIN Topics tp ON mtm.TopicId = tp.TopicId
JOIN [Blobs] AS [bt] ON [tp].[BlobId] = [bt].[BlobId]
JOIN EBadges eb ON tp.EBadgeId = eb.EBadgeId
WHERE ((([sm].[CourseId] IS NULL AND ([sm].[IsDeleted] = 0)) AND ([m].[IsDeleted] = 0)) AND ([a].[ApprovalStatusId] = 1)) 
	AND ([el].[IsEnrolled] = 1) AND [el].[EmployeeId] = @user AND el.IsPassed IS NOT NULL
) AS Trainings", param);

            return query;
        }

        public IQueryable<GetLearningBadges> GetContinuedBadgesModel(string user)
        {
            var param = new SqlParameter("user", user);

            var query = this
                .LearningBadgesQuery
                .FromSql($@"SELECT * from
(
SELECT DISTINCT
	[TrainingId] = CASE
		WHEN [t].[TrainingId] IS NULL
		THEN 0 ELSE [t].[TrainingId]
	END, 
	[Batch] = CASE
		WHEN [t].[TrainingId] IS NULL
		THEN 0 ELSE [t].[Batch]
	END,
	t.StartDate,
	t.EndDate,
	c.[CourseId], 
	c.[CourseName], 
	pt.[ProgramTypeName], 
	SetupModuleId = NULL,
	ModuleName = NULL,
	tp.[BlobId], 
	[bt].[MIME], 
	MaterialTypeName = NULL,
	CASE WHEN [t].[TrainingId] IS NULL THEN [c].SetupCourseCreatedAt
	ELSE [t].CreatedAt END AS [CreatedAt],
	CASE WHEN [t].[TrainingId] IS NULL THEN [c].SetupCourseUpdatedAt
	ELSE [t].UpdatedAt END AS [UpdatedAt],
	CASE WHEN [t].[TrainingId] IS NULL THEN [c].SetupCourseApprovedAt
	ELSE [t].ApprovedAt END AS [ApprovedAt],
	tp.TopicId AS [TopicId],
	eb.EBadgeId as [BadgeId],
	tp.TopicName
FROM [Courses] AS c 
	LEFT JOIN [ProgramTypes] AS [pt] ON c.[ProgramTypeId] = pt.[ProgramTypeId]
	INNER JOIN [Blobs] AS [b] ON c.[BlobId] = [b].[BlobId]
	LEFT JOIN [Trainings] AS [t] ON c.[CourseId] = [t].[CourseId]
	JOIN EnrollLearnings [el] ON t.TrainingId = el.TrainingId
	JOIN Employees [e] ON el.EmployeeId = e.EmployeeId
	JOIN ApprovalToCourses [atc] ON c.CourseId = atc.CourseId
	JOIN SetupModules sm ON c.CourseId = sm.CourseId
	JOIN Modules m ON sm.ModuleId = m.ModuleId
	JOIN ModuleTopicMappings mtm ON m.ModuleId = mtm.ModuleId
	JOIN Topics tp ON mtm.TopicId = tp.TopicId
	JOIN [Blobs] AS [bt] ON tp.[BlobId] = [bt].[BlobId]
	JOIN EBadges eb ON tp.EBadgeId = eb.EBadgeId
WHERE t.ApprovedAt IS NOT NULL AND t.isDeleted = 0 AND el.IsEnrolled = 1 AND e.EmployeeId = @user AND (el.IsPassed = 0 OR el.IsPassed IS NULL)

UNION ALL

SELECT DISTINCT
	TrainingId = NULL,
	Batch = NULL,
	StartDate = NULL,
	EndDate = NULL,
	CourseId = NULL,
	CourseName = NULL,
	ProgramTypeName = NULL,
	sm.SetupModuleId,
	m.ModuleName,
	tp.BlobId,
	bt.MIME,
	mt.MaterialTypeName,
	sm.CreatedAt,
	sm.UpdatedAt,
	sm.ApprovedAt,
	tp.TopicId AS [TopicId],
	eb.EBadgeId as [BadgeId],
	tp.TopicName
FROM Modules m
	JOIN [SetupModules] AS [sm] ON [m].[ModuleId] = [sm].[ModuleId]
	JOIN [Blobs] AS [b] ON [m].[BlobId] = [b].[BlobId]
	JOIN [MaterialTypes] AS [mt] ON [m].[MaterialTypeId] = [mt].[MaterialTypeId]
	JOIN [EnrollLearnings] AS [el] ON [sm].[SetupModuleId] = [sm].[SetupModuleId]
	JOIN Employees [e] ON [el].[EmployeeId] = [e].[EmployeeId]
	JOIN EnrollLearningTimes [elt] ON [el].[EnrollLearningId] = [elt].[EnrollLearningId] 
	JOIN [ApprovalToSetupModules] AS [atm] ON [sm].[SetupModuleId] = [atm].[SetupModuleId]
	JOIN [Approvals] AS [a] ON [atm].[ApprovalId] = [a].[ApprovalId]
	JOIN ModuleTopicMappings mtm ON m.ModuleId = mtm.ModuleId
	JOIN Topics tp ON mtm.TopicId = tp.TopicId
	JOIN [Blobs] AS [bt] ON [tp].[BlobId] = [bt].[BlobId]
	JOIN EBadges eb ON tp.EBadgeId = eb.EBadgeId
WHERE ((([sm].[CourseId] IS NULL AND ([sm].[IsDeleted] = 0)) AND ([m].[IsDeleted] = 0)) AND ([a].[ApprovalStatusId] = 1)) AND ([el].[IsEnrolled] = 1) AND [e].[EmployeeId] = @user AND [elt].[EndTime] is null
) AS Trainings


WHERE (TrainingId is null 
AND setupmoduleid in (SELECT DISTINCT  el.SetupModuleId from EnrollLearnings el
         join EnrollLearningTimes elt on el.EnrollLearningId = elt.EnrollLearningId
         where el.EmployeeId = @user AND (elt.EndTime is null OR elt.StartTime is null) AND el.SetupModuleId is not null))
OR 
trainingId  in
(SELECT DISTINCT el.TrainingId from EnrollLearnings el
         join EnrollLearningTimes elt on el.EnrollLearningId = elt.EnrollLearningId
         where el.EmployeeId = @user AND (elt.EndTime is null OR elt.StartTime is null) AND el.TrainingId is not null)", param);

            return query;
        }
        public IQueryable<GetLearningBadges> GetQueuedBadgesModel(string user)
        {
            var param = new SqlParameter("user", user);

            var query = this
                .LearningBadgesQuery
                .FromSql($@"SELECT DISTINCT
	[TrainingId] = CASE
		WHEN [t].[TrainingId] IS NULL
		THEN 0 ELSE [t].[TrainingId]
	END, 
	[Batch] = CASE
		WHEN [t].[TrainingId] IS NULL
		THEN 0 ELSE [t].[Batch]
	END,
	t.StartDate,
	t.EndDate,
	c.[CourseId], 
	c.[CourseName], 
	pt.[ProgramTypeName], 
	SetupModuleId = NULL,
	ModuleName = NULL,
	tp.[BlobId], 
	[bt].[MIME], 
	MaterialTypeName = NULL,
	CASE WHEN [t].[TrainingId] IS NULL THEN [c].SetupCourseCreatedAt
	ELSE [t].CreatedAt END AS [CreatedAt],
	CASE WHEN [t].[TrainingId] IS NULL THEN [c].SetupCourseUpdatedAt
	ELSE [t].UpdatedAt END AS [UpdatedAt],
	CASE WHEN [t].[TrainingId] IS NULL THEN [c].SetupCourseApprovedAt
	ELSE [t].ApprovedAt END AS [ApprovedAt],
	tp.TopicId AS [TopicId],
	eb.EBadgeId as [BadgeId],
	tp.TopicName
FROM [Courses] AS c 
	LEFT JOIN [ProgramTypes] AS [pt] ON c.[ProgramTypeId] = pt.[ProgramTypeId]
	INNER JOIN [Blobs] AS [b] ON c.[BlobId] = [b].[BlobId]
	LEFT JOIN [Trainings] AS [t] ON c.[CourseId] = [t].[CourseId]
	JOIN EnrollLearnings [el] ON t.TrainingId = el.TrainingId
	JOIN Employees [e] ON el.EmployeeId = e.EmployeeId
	JOIN ApprovalToCourses [atc] ON c.CourseId = atc.CourseId
	JOIN SetupModules sm ON c.CourseId = sm.CourseId
	JOIN Modules m ON sm.ModuleId = m.ModuleId
	JOIN ModuleTopicMappings mtm ON m.ModuleId = mtm.ModuleId
	JOIN Topics tp ON mtm.TopicId = tp.TopicId
	JOIN [Blobs] AS [bt] ON tp.[BlobId] = [bt].[BlobId]
	JOIN EBadges eb ON tp.EBadgeId = eb.EBadgeId
WHERE t.ApprovedAt IS NOT NULL AND t.isDeleted = 0 AND el.IsQueued = 1 AND e.EmployeeId = @user

UNION ALL

SELECT DISTINCT
	TrainingId = NULL,
	Batch = NULL,
	StartDate = NULL,
	EndDate = NULL,
	CourseId = NULL,
	CourseName = NULL,
	ProgramTypeName = NULL,
	sm.SetupModuleId,
	m.ModuleName,
	tp.BlobId,
	bt.MIME,
	mt.MaterialTypeName,
	sm.CreatedAt,
	sm.UpdatedAt,
	sm.ApprovedAt,
	tp.TopicId AS [TopicId],
	eb.EBadgeId as [BadgeId],
	tp.TopicName
FROM Modules m
	JOIN [SetupModules] AS [sm] ON [m].[ModuleId] = [sm].[ModuleId]
	JOIN [Blobs] AS [b] ON [m].[BlobId] = [b].[BlobId]
	JOIN [MaterialTypes] AS [mt] ON [m].[MaterialTypeId] = [mt].[MaterialTypeId]
	JOIN [EnrollLearnings] AS [el] ON [sm].[SetupModuleId] = [sm].[SetupModuleId]
	JOIN Employees [e] ON [el].[EmployeeId] = [e].[EmployeeId]
	JOIN EnrollLearningTimes [elt] ON [el].[EnrollLearningId] = [elt].[EnrollLearningId] 
	JOIN [ApprovalToSetupModules] AS [atm] ON [sm].[SetupModuleId] = [atm].[SetupModuleId]
	JOIN [Approvals] AS [a] ON [atm].[ApprovalId] = [a].[ApprovalId]
	JOIN ModuleTopicMappings mtm ON m.ModuleId = mtm.ModuleId
	JOIN Topics tp ON mtm.TopicId = tp.TopicId
	JOIN [Blobs] AS [bt] ON [tp].[BlobId] = [bt].[BlobId]
	JOIN EBadges eb ON tp.EBadgeId = eb.EBadgeId
WHERE ((([sm].[CourseId] IS NULL AND ([sm].[IsDeleted] = 0)) AND ([m].[IsDeleted] = 0)) AND ([a].[ApprovalStatusId] = 1)) AND ([el].[IsQueued] = 1) AND [e].[EmployeeId] = @user", param);

            return query;
        }

        public IQueryable<EventsModel> GetEvents(string user, string outletId, List<int> positionId)
        {

            var queryBuilder = $@"
SELECT 
	eom.EventId AS EventId,
	e.Name AS Title,
	e.Description AS Description,
	e.StartDateTime AS EventDate,
    e.TotalView AS TotalView,
    e.Location AS location,
    e.StartDateTime AS startDate,
    e.EndDateTime AS endDate,
    e.StartDateTime AS startTime,
    e.EndDateTime AS endTime,
    e.ApprovedAt AS approvedAt,
    CASE WHEN e.BlobId IS NOT NULL THEN CAST(e.BlobId as varchar(255)) ELSE '' END AS BlobId,
    eem.IsJoined AS isJoin,
    eem.IsSaved AS isSave
FROM Events e 
	JOIN EventOutletMappings eom ON e.EventId = eom.EventId
    LEFT JOIN EventPositionMappings epm on e.EventId = epm.EventId
    LEFT JOIN ApprovalToEvents ate ON eom.EventId = ate.EventId
    JOIN Approvals a ON ate.ApprovalId = a.ApprovalId
	LEFT JOIN EmployeeEventMappings eem ON eom.EventId = eem.EventId and eem.EmployeeId = '{user}'
where e.IsDeleted = 0 
	AND a.ApprovalStatusId = 1 
	AND eom.OutletId = '{outletId}'
	AND e.ApprovedAt is not null
";

            if (positionId.Count > 0)
            {
                var positionIdString = String.Join(',', positionId);
                queryBuilder = queryBuilder + $@"AND (epm.PositionId in ({positionIdString}) OR epm.EventId is null AND e.Source = 'mobile')";
            }

            var query = (RawSqlString)queryBuilder;

            var generateQuery = this
                .EventModel
                .FromSql(query);

            return generateQuery;
        }

        public IQueryable<EventSearchModel> GetEventSearch(string user, string outletId, List<string> positionId)
        {

            var queryBuilder = $@"SELECT DISTINCT
 e.EventId,
 Title = e.Name,
 e.Description,
 e.HostName,
 startDateTime = e.StartDateTime,
 EndDateTime = e.EndDateTime,
 b.MIME,
 [start] = e.StartDateTime,
 [end] = e.EndDateTime,
 [location] = e.Location,
 BlobId = CASE WHEN (e.BlobId is null) THEN '' ELSE CAST (e.BlobId AS VARCHAR(max)) END,
 isJoin = CASE WHEN (eem.IsJoined is null) THEN CONVERT(BIT,0) ELSE CONVERT(BIT,1) END,
 IsSave = CASE WHEN (eem.IsSaved is null) THEN CONVERT(BIT,0) ELSE CONVERT(BIT,1) END,
 TotalSave = saved.TotalSaved,
 TotalJoin = joined.TotalJoined,
 UpdatedAt = e.UpdatedAt
FROM Events e
LEFT JOIN Blobs b on e.BlobId = b.BlobId 
LEFT JOIN EmployeeEventMappings eem on e.EventId = eem.EventId and EmployeeId = '{user}'
LEFT JOIN EventPositionMappings epm on e.EventId = epm.EventId
JOIN EventOutletMappings eom on e.EventId = eom.EventId
LEFT JOIN (
	SELECT 
	ev.EventId,
	TotalSaved = COUNT(ev.EventId)
	FROM Events ev 
	JOIN EmployeeEventMappings eems on  ev.EventId = eems.EventId
	where eems.IsSaved = 1 
	GROUP BY ev.EventId
) saved on e.EventId = saved.EventId
LEFT JOIN (
	SELECT 
	ev.EventId,
	TotalJoined = COUNT(ev.EventId)
	FROM Events ev 
	JOIN EmployeeEventMappings eems on  ev.EventId = eems.EventId
	where eems.IsJoined = 1 
	GROUP BY ev.EventId
) joined on e.EventId = joined.EventId
where 
	e.IsDeleted = 0 
	and e.ApprovedAt is not null
	and eom.OutletId = '{outletId}'
";

            if (positionId.Count > 0)
            {
                var positionIdString = String.Join(',', positionId);
                queryBuilder = queryBuilder + $@"AND (epm.PositionId in ({positionIdString}) OR epm.EventId is null AND e.Source = 'mobile')";
            }

            var query = (RawSqlString)queryBuilder;

            var generateQuery = this
               .EventSearchModel
               .FromSql(query);

            return generateQuery;
        }

        public IQueryable<GetUserSideCourseLikePeopleListModel> GetUserSideCourseLikePeopleList(int trainingId)
        {
            var query = this
                .UserSideCourseLikePeopleListModel
                .FromSql($@"
SELECT [X].[EmployeeId], [X].[EmployeeName],STRING_AGG ([X].[PositionName], ', ') AS PositionName, [X].[BlobId], [X].[MIME] 
FROM (
	SELECT DISTINCT [e].[EmployeeId], [e].[EmployeeName],[p].[PositionName], [e].[BlobId], [b].[MIME]
	FROM [Courses] AS [c]
	INNER JOIN [Trainings] AS [t] ON [c].[CourseId] = [t].[CourseId]
	LEFT JOIN [TrainingRatings] AS [tr] ON [c].[CourseId] = [tr].[CourseId]
	LEFT JOIN [Employees] AS [e] ON [tr].[EmployeeId] = [e].[EmployeeId]
	LEFT JOIN [EmployeePositionMappings] AS [epm] ON [e].[EmployeeId] = [epm].[EmployeeId]
	INNER JOIN [Positions] AS [p] ON [epm].[PositionId] = [p].[PositionId]
	LEFT JOIN [Blobs] AS [b] ON [e].[BlobId] = [b].[BlobId]
	WHERE ([t].[TrainingId] = {trainingId}) AND ([tr].[RatingScore] > 0)
) X
GROUP BY [X].[EmployeeId], [X].[EmployeeName], [X].[BlobId], [X].[MIME]
");

            return query;
        }

        public IQueryable<TotalActiveUser> GetTotalActiveUser()
        {
            var query = this
                .TotalActiveUser
                .FromSql($@"
SELECT
    COUNT(*) AS [TotalActiveUserCount]
FROM Employees e
WHERE LastSeenAt IS NOT NULL
AND DATEDIFF(DAY, LastSeenAt, GETDATE()) BETWEEN 0 AND 30
");

            return query;
        }

        public IQueryable<AverageAccessTime> GetAverageAccessTimes()
        {
            var query = this.
                AverageAccessTime
                .FromSql($@"
SELECT (SUM(DATEDIFF(MINUTE, StartTime, EndTime))) AS [AverageAccessCount]
FROM EmployeeAccessTimes
WHERE DATEDIFF(DAY, EndTime, GETDATE()) BETWEEN 0 AND 30
"
                );

            return query;
        }


        public IQueryable<GetUserSidePeopleWhoTookTheCourseListModel> GetUserSidePeopleWhoTookTheCourseList(int trainingId)
        {
            var query = this
                .UserSidePeopleWhoTookTheCourseListModel
                .FromSql($@"select x.EmployeeId, x.EmployeeName, [Position] = STRING_AGG(x.PositionName, ', '),x.BlobId, x.MIME
from
(select distinct el.EmployeeId, e.EmployeeName, p.PositionName, b.BlobId, b.MIME
 from 
EnrollLearnings el join Employees e on el.EmployeeId = e.EmployeeId
 join EmployeePositionMappings epm on e.EmployeeId = epm.EmployeeId
join Positions p on epm.PositionId = p.PositionId
LEFT join Blobs b on e.BlobId = b.BlobId
where TrainingId = {trainingId}) x
group by x.EmployeeId, x.EmployeeName, x.BlobId, x.MIME");

            return query;
        }

        public IQueryable<GetUserSideRankModel> GetGenerateRank(string employeeId, List<string> AreaIds, List<string> DealerIds, List<int?> PositionIds, string SortBy)
        {


            var queryBuilder = $@"with Employee_Dealer_CTE (EmployeeId, EmployeeExperience)
AS
(SELECT E.EmployeeId, E.EmployeeExperience FROM Employees E
LEFT JOIN Outlets O on E.OutletId = O.OutletId
LEFT JOIN Dealers D on O.DealerId = D.DealerId
";

            if (DealerIds != null)
            {
                var contains = "'" + string.Join("','", DealerIds) + "'";
                queryBuilder = queryBuilder + $@"where D.DealerId in ({contains})";
            }

            queryBuilder = queryBuilder + $@"),

Employee_Areas_CTE (EmployeeId, EmployeeExperience)
AS
(SELECT E.EmployeeId, E.EmployeeExperience FROM Employees E
LEFT JOIN Outlets O on E.OutletId = O.OutletId
LEFT JOIN Areas A on A.AreaId = O.AreaId
";
            if (AreaIds != null)
            {
                var contains = "'" + string.Join("','", AreaIds) + "'";
                queryBuilder = queryBuilder + $@"where A.AreaId in ({contains})";
            }

            queryBuilder = queryBuilder + $@"),

Employee_Position_CTE (EmployeeId, EmployeeExperience)
AS
(SELECT E.EmployeeId, E.EmployeeExperience FROM Employees E
LEFT JOIN EmployeePositionMappings EPM on E.EmployeeId = EPM.EmployeeId
";
            if (PositionIds != null)
            {
                var contains = "'" + string.Join("','", PositionIds) + "'";
                queryBuilder = queryBuilder + $@"where EPM.PositionId in ({contains})";
            }

            queryBuilder = queryBuilder + $@"),

Employee_UNION_CTE (EmployeeId, EmployeeExperience)
AS
(SELECT EmployeeId, EmployeeExperience
FROM
Employee_Dealer_CTE
INTERSECT
SELECT EmployeeId, EmployeeExperience
FROM
Employee_Areas_CTE
INTERSECT
SELECT EmployeeId, EmployeeExperience
FROM
Employee_Position_CTE
),

Employee_Rank_CTE (Rank,EmployeeId, EmployeeExperience)
AS
(SELECT CAST( ROW_NUMBER() OVER(ORDER BY EmployeeExperience DESC) AS int) [Rank], EmployeeId, EmployeeExperience FROM 
 Employee_UNION_CTE)
 SELECT * FROM Employee_Rank_CTE
";
            if (!string.IsNullOrEmpty(employeeId))
            {
                queryBuilder = queryBuilder + $@"where EmployeeId = '" + employeeId + "'";
            };
            var x = (RawSqlString)queryBuilder;



            var generateQuery = this.GenerateRank.FromSql(x);
            return generateQuery;
        }

        public IQueryable<SuperiorEmployee> GetAllSuperiorEmployees(string noReg)
        {
            var query = this
                .GetSuperiorEmployees
                .FromSql($@"select CONVERT(INT,row_number() OVER(order by par.OrgLevel DESC)) AS OrderRank, par.NoReg, par.Name, par.PostName, par.JobName , par.TalentLevel
from TAMEmployeeStructure aos
left join TAMEmployeeStructure par on (par.OrgCode = aos.OrgCode or aos.Structure like '%(' + par.OrgCode + ')%') and par.Chief = 1 and par.OrgLevel < aos.OrgLevel
where aos.NoReg = {noReg} and par.NoReg IS NOT NULL and par.WorkContract <> 'Z4' and par.OrgLevel > 3 and aos.Staffing=100 order by TalentLevel");

            return query;
        }
        public IQueryable<ReportNPSModel> GetReportNPS()
        {
            var query = this.ReportNPS.FromSql($@"
				SELECT
				t.TrainingId AS [TrainingId],
				c.CourseId AS [CourseId],
				c.CourseName AS [CourseName] ,
				c.ProgramTypeId AS [ProgramTypeId],
				pt.ProgramTypeName AS [ProgramTypeName],
				t.StartDate AS [StartDate],
				t.EndDate AS [EndDate],
				t.Batch AS [Batch],
				CASE 
				WHEN t.StartDate > GETDATE() THEN 'Not Started'
				WHEN t.StartDate < GETDATE() AND t.EndDate >= GETDATE() THEN 'On Going'
				WHEN t.EndDate < GETDATE() THEN 'Done'
				ELSE 'Not Set'
				END AS [Status]    
				FROM Trainings t 
				JOIN Courses c ON c.CourseId = t.CourseId 
				JOIN ProgramTypes pt ON pt.ProgramTypeId = c.ProgramTypeId 
                WHERE t.IsDeleted = 0
			"
            );
            return query;

        }
        public IQueryable<TrainingScoreReportViewQueryModel> GetAllTrainingScoreReportView()
        {
            var query = this.GetTrainingScoreReport.FromSql($@"
            SELECT DISTINCT 
				t.TrainingId AS [TrainingId],
				c.CourseId AS [CourseId],
				c.CourseName AS [CourseName],
				pt.ProgramTypeId AS [ProgramTypeId],
				pt.ProgramTypeName AS [ProgramTypeName],
				t.Batch AS [Batch],
				par.Participant AS [Participant],
				CAST(CAST(par.Participant AS decimal)/CAST(NULLIF(t.Quota,0) AS decimal) as numeric(36,2)) AS [ParticipantRate],
				CASE 
				    WHEN t.StartDate < CURRENT_TIMESTAMP AND t.EndDate > CURRENT_TIMESTAMP THEN 'On Going'
				    WHEN t.EndDate < CURRENT_TIMESTAMP THEN 'Done'
				    ELSE ''
				END AS [Status],
				t.StartDate AS [StartDate],
				t.EndDate AS [EndDate]
			FROM Trainings t 
			JOIN Courses c ON c.CourseId = t.CourseId 
			JOIN LearningTypes lt ON lt.LearningTypeId = c.LearningTypeId 
			JOIN ProgramTypes pt ON pt.ProgramTypeId = c.ProgramTypeId
			JOIN (
			SELECT COUNT(el.EmployeeId) AS Participant , t.TrainingId 
			    FROM EnrollLearnings el
			    JOIN Trainings t ON el.TrainingId = t.TrainingId
			    JOIN Courses c ON c.CourseId=t.CourseId
			    where el.IsJoined = 1 
			    GROUP BY t.TrainingId
) AS par ON par.TrainingId = t.TrainingId
            ");

            return query;
        }

        public IQueryable<TrainingScoreReportDownloadQueryModel> GetTrainingReportDownload(int trainingId)
        {
            var query = this.GetTrainingScoreReportDownload.FromSql($@"
			SELECT 
	            t.TrainingId,
	            taa.TaskAnswerId,
	            tad.TaskAnswerDetailId,
	            taa.CreatedAt AS [Timestamp],
	            c.CourseName AS [CourseName],
	            tad.Score AS [TotalScore],
	            tam.TotalScoreModule AS [TotalScoreModule],
				tad.Attempts,
	            m2.ModuleName AS [Module],
	            ct.CompetencyTypeName + '-' + co.PrefixCode + '-' + et.EvaluationTypeName + '-' + CAST(ta.TaskNumber as VARCHAR(MAX)) + '-' + m.ModuleName AS [CompetencyCode],
	            qt.QuestionTypeName AS [TypeofQuestion],
	            q.Question,
	            CASE 
		            WHEN ta.QuestionTypeId IN (2,5,8,9,12) THEN STRING_AGG((tsa.Answer),',')
		            ELSE tad.Answer 
	            END as Answer,
	            tad.AnswerBlobId,
	            b.MIME,
	            ta.QuestionTypeId,
	            e.EmployeeName AS [EmployeeName],
	            a.Name AS [Area],
	            pr.ProvinceName AS [Province],
	            ci.CityName AS [City],
	            d.DealerName AS [Dealer],
	            o.Name AS [Outlet] 
            FROM Trainings t 
            JOIN Courses c ON c.CourseId = t.CourseId 
            JOIN EnrollLearnings el ON el.TrainingId = t.TrainingId 
            JOIN Employees e ON e.EmployeeId = el.EmployeeId 
            JOIN TaskAnswers taa ON t.TrainingId = taa.TrainingId and taa.EmployeeId = el.EmployeeId
            JOIN TaskAnswerDetails tad ON tad.TaskAnswerId = taa.TaskAnswerId
            LEFT JOIN TaskSpecialAnswers tsa ON tad.TaskAnswerDetailId = tsa.TaskAnswerDetailId
            LEFT JOIN Blobs b ON b.BlobId = tad.AnswerBlobId
            JOIN Tasks ta ON ta.TaskId = tad.TaskId
            JOIN Modules m ON ta.ModuleId = m.ModuleId
			JOIN SetupModules sm ON taa.SetupModuleId = sm.SetupModuleId
			JOIN Modules m2 ON sm.ModuleId = m2.ModuleId
            JOIN Competencies co ON co.CompetencyId = ta.CompetencyId
			JOIN CompetencyTypes ct ON ct.CompetencyTypeId = co.CompetencyTypeId
			JOIN EvaluationTypes et ON ta.EvaluationTypeId = et.EvaluationTypeId
            JOIN QuestionTypes qt ON qt.QuestionTypeId = ta.QuestionTypeId 
            LEFT JOIN Outlets o ON o.OutletId = e.OutletId 
            LEFT JOIN Areas a ON a.AreaId = o.AreaId 
            LEFT JOIN Provinces pr ON pr.ProvinceId = o.ProvinceId 
            LEFT JOIN Cities ci ON ci.CityId = o.CityId 
            LEFT JOIN Dealers d ON d.DealerId = o.DealerId 
            JOIN (
	            SELECT
		            TaskId ,
		            Question ,
		            CAST (Answer AS varchar) AS Answer
		            FROM TaskTebakGambarTypes
	            UNION
	            SELECT
		            TaskId ,
		            Question ,
		            [Answer] = CASE 
		            WHEN Answer = 1 THEN 'True'
		            WHEN Answer = 0 THEN 'False'
		            END 
		            FROM TaskTrueFalseTypes
	            UNION
	            SELECT
		            TaskId ,
		            Question ,
		            CAST (AnswerId AS varchar) AS Answer
		            FROM TaskMultipleChoiceTypes
	            UNION
	            SELECT
		            TaskId ,
		            Question ,
		            CAST (Answer AS varchar) AS Answer
		            FROM TaskSequenceTypes
	            UNION
	            SELECT
		            TaskId ,
		            Question ,
		            CAST (Answer AS varchar) AS Answer
		            FROM TaskMatchingTypes tmt
	            UNION
	            SELECT
		            TaskId ,
		            Question ,
		            NULL AS Answer
		            FROM TaskShortAnswerTypes tsat
	            UNION
	            SELECT
		            TaskId ,
		            Question ,
		            NULL AS Answer
		            FROM TaskHotSpotTypes thst
	            UNION
	            SELECT
		            TaskId ,
		            Question ,
		            NULL AS Answer
		            FROM TaskRatingTypes trt 
	            UNION
	            SELECT
		            TaskId ,
		            Question ,
		            NULL AS Answer
		            FROM TaskFileUploadTypes tfut 
	            UNION
	            SELECT
		            TaskId ,
		            Question ,
		            NULL AS Answer
		            FROM TaskMatrixTypes tmt2 
	            UNION
	            SELECT
		            TaskId ,
		            Question ,
		            NULL AS Answer
		            FROM TaskChecklistTypes tct 
	            UNION
	            SELECT
		            TaskId ,
		            Question ,
		            NULL AS Answer
		            FROM TaskEssayTypes tet 
            ) q ON q.TaskId = ta.TaskId
            JOIN (
	            SELECT SUM(Score) AS TotalScoreModule, TaskAnswerId, Attempts
	            FROM TaskAnswerDetails
	            GROUP BY TaskAnswerId, Attempts
            ) tam ON tam.TaskAnswerId = tad.TaskAnswerId and tam.Attempts = tad.Attempts
			WHERE t.TrainingId = {trainingId}
            GROUP BY
	            t.TrainingId,
	            tad.TaskAnswerDetailId,
	            taa.CreatedAt ,
	            c.CourseName ,
				m.ModuleName ,
	            m2.ModuleName ,
	            co.PrefixCode ,
				ct.CompetencyTypeName ,
	            qt.QuestionTypeName ,
				et.EvaluationTypeName,
	            q.Question ,
	            tad.Answer ,
				ta.TaskNumber,
	            e.EmployeeName ,
	            a.Name ,
	            pr.ProvinceName ,
	            ci.CityName ,
	            d.DealerName ,
	            o.Name ,
	            taa.TaskAnswerId,
	            tam.TotalScoreModule,
	            tad.AnswerBlobId,
	            ta.QuestionTypeId,
	            tad.Score,
	            b.MIME,
				tad.Attempts
            ");
            return query;
        }

        public IQueryable<SurveyReport> GetAllSurveyReports()
        {
            var query = this
                .GetSurveyReports
                .FromSql($@"SELECT
	                        s.SurveyId,
                            s.Title as [SurveyTitle],
                            sr.Respondent ,
                            s.StartDate as [StartDate],
                            s.EndDate as [EndDate],
	                        CASE 
		                        WHEN s.StartDate < CURRENT_TIMESTAMP AND s.EndDate > CURRENT_TIMESTAMP THEN 'On Going'
		                        WHEN s.EndDate < CURRENT_TIMESTAMP THEN 'Done'
		                        ELSE ''
	                        END AS [Status],
	                        CAST (CAST (sr.Respondent as decimal) / CAST(tr.TotalResponden as decimal) as numeric(36,2)) as [RespondentRate]
                        FROM Surveys s
                        JOIN(
                            SELECT
                                sa.SurveyId ,
                                COUNT(SurveyAnswerId) AS Respondent
                            FROM SurveyAnswers sa
                            GROUP BY sa.SurveyId
                        ) AS sr ON sr.SurveyId = s.SurveyId
                       JOIN (
                            SELECT spm.SurveyId, COUNT(epm.EmployeeId) AS [TotalResponden] 
                            FROM SurveyPositionMappings spm
                            JOIN EmployeePositionMappings epm ON epm.PositionId = spm.PositionId
                            GROUP BY spm.SurveyId
                        ) AS tr ON tr.SurveyId = s.SurveyId
						WHERE s.IsDeleted = 0
                        group by s.SurveyId , s.Title , sr.Respondent , s.StartDate , s.EndDate, tr.TotalResponden");

            return query;
        }

        public IQueryable<LearningHistoriesQueryModel> GetAllLearningHistories()
        {
            var query = this.GetLearningHistories.
                FromSql($@"
					SELECT
						LearningHistoryId,
						EmployeeId,
						LHT.TrainingId,
						SetupModuleId,
						PopQuizId,
						CourseName, 
						CourseId, 
						BlobId,
						LearningTypeId,
						ProgramTypeId,
						CoursePrice,
						Mime,
						Name,
						RT.Ratings AS RatingScore,
						CreatedAt,
						SetupCourseApprovedAt,
						STRING_AGG((MaterialTypeId),',') as MaterialTypeId
					FROM(
						SELECT DISTINCT
							lh.LearningHistoryId,
							lh.EmployeeId,
							lh.TrainingId,
							lh.SetupModuleId,
							lh.PopQuizId,
							c.CourseName, 
							c.CourseId,
							c.SetupCourseApprovedAt,
							CASE
								WHEN lh.TrainingId IS NOT NULL THEN b.BlobId
								WHEN lh.SetupModuleId IS NOT NULL THEN mb.BlobId
							END AS BlobId,
							c.LearningTypeId,
							c.ProgramTypeId,
							c.CoursePrice,
							CASE
								WHEN lh.TrainingId IS NOT NULL THEN b.MIME
								WHEN lh.SetupModuleId IS NOT NULL THEN mb.MIME
							END AS Mime,
							lh.Name,
							lh.CreatedAt,
							CASE
								WHEN lh.TrainingId IS NOT NULL THEN tm.MaterialTypeId
								WHEN lh.SetupModuleId IS NOT NULL THEN m.MaterialTypeId
							END AS MaterialTypeId
						FROM LearningHistories lh
						LEFT JOIN Trainings t ON t.TrainingId = lh.TrainingId
						LEFT JOIN TrainingModuleMappings tmm ON t.TrainingId = tmm.TrainingId
						LEFT JOIN Courses c ON t.CourseId = c.CourseId
						LEFT JOIN SetupModules tsm ON tsm.SetupModuleId = tmm.SetupModuleId
						LEFT JOIN Modules tm ON tm.ModuleId = tsm.ModuleId
						LEFT JOIN Blobs b ON b.BlobId = c.BlobId
						LEFT JOIN TrainingRatings tr ON tr.TrainingId = t.TrainingId
						LEFT JOIN SetupModules sm ON sm.SetupModuleId = lh.SetupModuleId
						LEFT JOIN Modules m ON m.ModuleId = sm.ModuleId
						LEFT JOIN Blobs mb ON m.BlobId = mb.BlobId
					) AS LHT
					LEFT JOIN (
						SELECT t.TrainingId, CAST((SUM(tr.RatingScore)/ COUNT(tr.TrainingId)) AS float) AS Ratings
						FROM Trainings t
						JOIN TrainingRatings tr ON t.TrainingId = tr.TrainingId
						GROUP BY t.TrainingId
					) AS RT ON LHT.TrainingId = RT.TrainingId
					GROUP BY LearningHistoryId,
						LHT.TrainingId,
						EmployeeId,
						SetupModuleId,
						PopQuizId,
						CourseName, 
						CourseId, 
						BlobId,
						LearningTypeId,
						ProgramTypeId,
						CoursePrice,
						MIME,
						Name,
						CreatedAt,
						RT.Ratings,
						SetupCourseApprovedAt
				");
            return query;
        }

        public IQueryable<CoachSearchQueryModel> GetCoachSearchList()
        {
            var query = this.GetCoachSearch.
                FromSql($@"
					SELECT 
    Coach.CoachId,
    Coach.EmployeeName,
    Coach.EmployeeId,
    Coach.AreaId,
    Coach.OutletId,
    Coach.CityId,
    Coach.DealerId,
    Coach.ProvinceId,
    Coach.BlobId,
    Coach.MIME,
    '*'+STRING_AGG((Coach.TopicId),'*,*') +'*' as TopicId,
    '*'+STRING_AGG((Coach.EBadgeId),'*,*') +'*' as EbadgeId,
    '*'+STRING_AGG((Coach.PositionId),'*,*') +'*' as PositionId
FROM(
    SELECT
        c.CoachId,
        p.PositionId,
        e.EmployeeName,
        e.EmployeeId,
        o.AreaId,
        o.OutletId,
        o.CityId,
        o.DealerId,
        o.ProvinceId,
        e.BlobId,
        b.MIME,
        t.TopicId,
        t.EBadgeId
    FROM Coaches c
    JOIN Employees e ON c.EmployeeId = e.EmployeeId
    LEFT JOIN EmployeePositionMappings epm ON e.EmployeeId = epm.EmployeeId
    LEFT JOIN Positions p ON p.PositionId = epm.PositionId
    LEFT JOIN Outlets o ON o.OutletId = e.OutletId
    LEFT JOIN Blobs b ON e.BlobId = b.BlobId
    JOIN CoachTopicMappings ctm ON c.CoachId = ctm.CoachId
    JOIN Topics t ON t.TopicId = ctm.TopicId
    WHERE c.CoachIsActive = 1
) AS Coach
GROUP BY
    Coach.CoachId,
    Coach.EmployeeName,
    Coach.EmployeeId,
    Coach.AreaId,
    Coach.OutletId,
    Coach.CityId,
    Coach.DealerId,
    Coach.ProvinceId,
    Coach.BlobId,
    Coach.MIME
				");

            return query;
        }

        public IQueryable<EmployeePositionAggregateQueryModel> GetEmployeePositionAggregateList()
        {
            var query = this.GetEmployeePositionAggregate.
                FromSql($@"
					SELECT 
						pn.EmployeeId,
						STRING_AGG((pn.PositionName),',') as PositionName
					FROM (
						SELECT 
							p.PositionName, 
							epm.EmployeeId
						FROM Employees e
						LEFT JOIN EmployeePositionMappings epm ON e.EmployeeId = epm.EmployeeId
						LEFT JOIN Positions p ON epm.PositionId = p.PositionId
					) AS pn
					GROUP BY pn.EmployeeId
				");

            return query;
        }

        public IQueryable<PeopleSearchQueryModel> GetPeopleSearchList()
        {
            var query = this.GetPeopleSearch.
                FromSql($@"
					SELECT
						p.EmployeeId,
						p.EmployeeName,
						p.AreaId,
						p.OutletId,
						p.CityId,
						p.DealerId,
						p.ProvinceId,
						p.BlobId,
						p.MIME,
						'*'+STRING_AGG((p.PositionId),'*,*') +'*' as PositionId
					FROM (
						SELECT
							e.EmployeeId,
							e.EmployeeName,
							epm.PositionId,
							o.AreaId,
							o.OutletId,
							o.CityId,
							o.DealerId,
							o.ProvinceId,
							e.BlobId,
							b.MIME
						FROM Employees e
						LEFT JOIN Outlets o ON e.OutletId = o.OutletId
						LEFT JOIN EmployeePositionMappings epm ON e.EmployeeId = epm.EmployeeId
						LEFT JOIN Blobs b ON b.BlobId = e.BlobId
						WHERE e.IsDeleted = 0
					) AS p
					GROUP BY
						p.EmployeeId,
						p.EmployeeName,
						p.AreaId,
						p.OutletId,
						p.CityId,
						p.DealerId,
						p.ProvinceId,
						p.BlobId,
						p.MIME
				");

            return query;
        }

        public IQueryable<SurveyReportDetailModel> GetSurveyReportDetailDownload(int surveyId)
        {
            var query = this.GetSurveyReportDetail.
                FromSql($@"

SELECT DISTINCT 
	sa.CreatedAt AS TimeStamp, 
	s.SurveyId, 
	s.Title AS SurveyTitle, 
	sqt.Name AS TypeOfQuestion,
	sq.Question, 
	CASE 
		WHEN sqt.SurveyQuestionTypeId IN (2,5,8,9,12) THEN  STRING_AGG(spa.answer, ', ')
		ELSE sad.Answer
	END AS Answer,
	e.EmployeeName, 
	a.Name AS Area, 
	p.ProvinceName AS Province, 
	c.CityName AS City, 
	d.DealerName AS Dealer, 
	o.Name AS Outlet, 
	b.MIME,
	sqt.SurveyQuestionTypeId,
	sad.BlobId
FROM Surveys s
INNER JOIN SurveyAnswers sa ON s.SurveyId = sa.SurveyId
INNER JOIN SurveyAnswerDetails sad ON sa.SurveyAnswerId = sad.SurveyAnswerId
LEFT JOIN SurveySpecialAnswers spa ON sad.SurveyAnswerDetailId = spa.SurveyAnswerDetailId
INNER JOIN SurveyQuestions sq ON sad.SurveyQuestionId = sq.SurveyQuestionId
INNER JOIN SurveyQuestionTypes sqt ON sq.SurveyQuestionTypeId = sqt.SurveyQuestionTypeId
INNER JOIN Employees e ON sa.EmployeeId = e.EmployeeId
LEFT JOIN Outlets o ON e.OutletId = o.OutletId
LEFT JOIN Dealers d ON o.DealerId = d.DealerId
LEFT JOIN Areas a ON o.AreaId = a.AreaId
LEFT JOIN Provinces p ON o.ProvinceId = p.ProvinceId
LEFT JOIN Cities c ON o.CityId = c.CityId
LEFT JOIn Blobs b on sad.BlobId = b.BlobId
WHERE s.SurveyId = {surveyId}
GROUP BY 
	sa.CreatedAt,  
	s.SurveyId, 
	s.Title, 
	sqt.Name, 
	sq.Question, 
	sad.Answer, 
	e.EmployeeName, 
	a.Name, 
	p.ProvinceName, 
	c.CityName, 
	d.DealerName, 
	o.Name, 
	b.MIME,
	sad.BlobId,
	sqt.SurveyQuestionTypeId
            ");

            return query;
        }

        public IQueryable<SetupModuleCalculate> GetSetupModuleCalculateList(int EnrollLearningId)
        {
            var query = this.GetSetupModuleCalculate.
                FromSql($@"  Select c.CourseName, sm.SetupModuleId, sm.MinimumScore, 
								CASE WHEN SUM(tad.Score) IS NULL THEN 0 ELSE SUM(tad.Score) END AS EmployeeScore, 
								CASE WHEN SUM(tad.Point) IS NULL THEN 0 ELSE SUM(tad.Point) END AS EmployeePoint, 
								sm.IsForRemedial
							from Trainings  t
							join Courses c on c.CourseId = t.CourseId
							join SetupModules sm on t.CourseId = sm.CourseId
							join EnrollLearnings el on t.TrainingId =  el.TrainingId 
							left join TaskAnswers ta on ta.SetupModuleId = sm.SetupModuleId AND ta.TrainingId = el.TrainingId AND ta.EmployeeId = el.EmployeeId
							left join TaskAnswerDetails tad on ta.TaskAnswerId = tad.TaskAnswerId
							where sm.IsDeleted = 0 AND el.EnrollLearningId = {EnrollLearningId}
							Group By sm.SetupModuleId, sm.MinimumScore, sm.IsForRemedial, c.CourseName
							");

            return query;
        }

        public IQueryable<WeightedModel> GetWeightedModelCalculate(int EnrollLearningId)
        {
            var query = this.GetWeightedModel.
               FromSql($@" SELECT
dtt.TrainingId,
dtt.EnrollLearningId,
dtt.EmployeeId,
SUM(dtt.Weighted) AS [Weighted]
FROM(
SELECT DISTINCT 
	el.EnrollLearningId,
	el.TrainingId,
    el.EmployeeId,
	tt.TrainingTypeName,
	tt.TrainingTypeId,
	cttm.MinimumScore,
	cttm.Percentage * 1.0 / 100 as [Percentage] ,
	COALESCE(dt.[Avg Score], 0) AS [Avg Score],
	CASE WHEN cttm.MinimumScore = 0 
		THEN 0 
		ELSE COALESCE((dt.[Avg Score] * 1.0 / cttm.MinimumScore) * 1.0, 0) 
	END AS [% Ach],
	CASE 
	WHEN [Avg Score] = 0 THEN 0
	WHEN cttm.Percentage != 0 AND [Avg Score] != 0
	THEN (dt.[Avg Score] * 1.0 * (cttm.Percentage * 1.0/100))
	ELSE 0
	END AS Weighted
FROM
Trainings t 
JOIN CourseTrainingTypeMappings cttm ON t.CourseId = cttm.CourseId
JOIN TrainingTypes tt ON cttm.TrainingTypeId = tt.TrainingTypeId
JOIN EnrollLearnings el on el.TrainingId = t.TrainingId 
JOIN Employees e ON el.EmployeeId = e.EmployeeId
JOIN (
SELECT 
	IT.TrainingId,
	IT.EmployeeId,
	IT.TrainingTypeId,
	IT.RemedialLevel,
	CASE
		WHEN IT.RemedialLevel > 0 THEN 
			CASE WHEN COUNT(IT.SetupModuleId) = 0 
				THEN 0 
				ELSE (SUM(TotalScoreModule * 1.00) / COUNT(IT.SetupModuleId)) * 1.00 
			END 
		ELSE SUM(CASE WHEN IT.IsForRemedial = 0 THEN TotalScoreModule * 1.00 ELSE 0 END) / SUM(CASE WHEN IT.IsForRemedial = 0 THEN 1 ELSE NULL END) * 1.00
	END AS [Avg Score]
	FROM (
		SELECT
			el.EnrollLearningId,
			elt.EnrollLearningTimeId,
			SUM(COALESCE(tad.score, 0)) AS TotalScoreModule, 
			ta.TaskAnswerId, 
			el.TrainingId,
			el.RemedialLevel,
			elt.SetupModuleId,
			el.EmployeeId,
			sm.TrainingTypeId,
			tt.TrainingTypeName,
			sm.MinimumScore,
			m.ModuleName,
			sm.IsForRemedial,
			CASE
				WHEN sm.MinimumScore = 0 THEN CAST(100 AS NUMERIC(36,2))
				WHEN sm.MinimumScore IS NULL THEN CAST(100 AS NUMERIC(36,2))
				ELSE CAST(SUM(COALESCE(tad.Score, 0)) * 1.00 / sm.MinimumScore AS NUMERIC(36,2))
			END AS Percentages
		FROM EnrollLearnings el
		JOIN EnrollLearningTimes elt ON elt.EnrollLearningId = el.EnrollLearningId
		JOIN SetupModules sm ON elt.SetupModuleId = sm.SetupModuleId
		JOIN TrainingTypes tt ON tt.TrainingTypeId = sm.TrainingTypeId
		JOIN Modules m ON m.ModuleId = sm.ModuleId
		LEFT JOIN TaskAnswers ta ON ta.SetupModuleId = elt.SetupModuleId AND el.EmployeeId = ta.EmployeeId AND ta.TrainingId = el.TrainingId
		LEFT JOIN TaskAnswerDetails tad ON tad.TaskAnswerId = ta.TaskAnswerId
		WHERE sm.MinimumScore != 0 AND sm.IsDeleted = 0
		GROUP BY 
		el.EnrollLearningId,
			ta.TaskAnswerId, 
			el.TrainingId, 
			ta.SetupModuleId,
			el.EmployeeId,
			elt.SetupModuleId,
			sm.TrainingTypeId,
			tt.TrainingTypeName,
			sm.MinimumScore,
			m.ModuleName,
			el.EnrollLearningId,
			el.RemedialLevel,
			sm.IsForRemedial,
			elt.EnrollLearningTimeId
	) AS IT
	GROUP BY 
		IT.TrainingId,
		IT.EmployeeId,
		IT.TrainingTypeId,
		IT.RemedialLevel
) AS dt ON t.TrainingId = dt.TrainingId AND el.EmployeeId = dt.EmployeeId AND tt.TrainingTypeId = dt.TrainingTypeId
WHERE tt.TrainingTypeId != 4 AND el.IsJoined = 1 AND t.isDeleted = 0 AND e.IsDeleted = 0
) AS dtt
WHERE dtt.EnrollLearningId = {EnrollLearningId}
GROUP BY
dtt.TrainingId,
dtt.EnrollLearningId,
dtt.EmployeeId");

            return query;
        }

        public IQueryable<SetupModuleCalculate> GetSpecificSetupModuleCalculate(int EnrollLearningId, int SetupModuleId, int? TrainingId)
        {
            var query = this.GetSetupModuleCalculate.
                FromSql($@" Select c.CourseName, sm.SetupModuleId, sm.MinimumScore, 
							CASE WHEN SUM(tad.Score) IS NULL THEN 0 ELSE SUM(tad.Score) END AS EmployeeScore, 
							CASE WHEN SUM(tad.Point) IS NULL THEN 0 ELSE SUM(tad.Point) END AS EmployeePoint, 
							sm.IsForRemedial
							from Trainings  t
							join Courses c on c.CourseId = t.CourseId
							join SetupModules sm on t.CourseId = sm.CourseId
							join EnrollLearnings el on t.TrainingId =  el.TrainingId
							left join TaskAnswers ta on ta.SetupModuleId = sm.SetupModuleId 
							left join TaskAnswerDetails tad on ta.TaskAnswerId = tad.TaskAnswerId
							where sm.IsDeleted = 0 AND el.EnrollLearningId = {EnrollLearningId} AND sm.SetupModuleId = {SetupModuleId} AND ta.TrainingId = {TrainingId}
							Group By sm.SetupModuleId, sm.MinimumScore, sm.IsForRemedial, c.CourseName
							");

            return query;
        }

        public IQueryable<TaskScoreQueryModel> GetAllTaskScore()
        {
            var query = this.GetTaskScore.
                FromSql($@"SELECT 
    Q.TaskId, 
    SUM(tp.Score) AS Score 
FROM TimePoints tp 
JOIN (
SELECT
    TaskId, Score
    FROM TaskTebakGambarTypes
UNION
SELECT
    TaskId, Score
    FROM TaskTrueFalseTypes
UNION
SELECT
    TaskId, Score
    FROM TaskMultipleChoiceTypes
UNION
SELECT
    TaskId, Score
    FROM TaskMatchingTypes tmt
UNION ALL
SELECT
    TaskId, Score AS Score
    FROM TaskHotSpotAnswers
UNION ALL
SELECT
    TaskId, Score AS Score
    FROM TaskChecklistChoices
UNION ALL
SELECT
    TaskId, Score AS Score
    FROM TaskSequenceChoices
) AS Q on Q.Score = tp.TimePointId
GROUP BY Q.TaskId
UNION
SELECT 
    trt.TaskId, (tp1.Score + tp2.Score + tp3.Score + tp4.Score + tp5.Score) AS [Score] 
FROM TaskRatingTypes trt 
JOIN TimePoints tp1 ON trt.Score0To20 = tp1.TimePointId
JOIN TimePoints tp2 ON trt.Score21To40 = tp2.TimePointId
JOIN TimePoints tp3 ON trt.Score41To60 = tp3.TimePointId
JOIN TimePoints tp4 ON trt.Score61To80 = tp4.TimePointId
JOIN TimePoints tp5 On trt.Score81To100 = tp5.TimePointId
UNION
SELECT 
    TaskId,
    (tp1.Score + tp2.Score + tp3.Score + tp4.Score + tp5.Score) as TotalScore
FROM TaskMatrixTypes tmt
JOIN TimePoints tp1 on tmt.ScoreColumn1 = tp1.TimePointId
JOIN TimePoints tp2 on tmt.ScoreColumn2 = tp2.TimePointId
JOIN TimePoints tp3 on tmt.ScoreColumn3 = tp3.TimePointId
JOIN TimePoints tp4 on tmt.ScoreColumn4 = tp4.TimePointId
JOIN TimePoints tp5 on tmt.ScoreColumn5 = tp5.TimePointId
UNION
SELECT
    TaskId, 100 AS Score
    FROM TaskShortAnswerTypes tsat
UNION
SELECT
    TaskId, 100 AS Score
    FROM TaskFileUploadTypes tfut 
UNION
SELECT
    TaskId, 100 AS Score
    FROM TaskEssayTypes tet
            ");

            return query;
        }
        public IQueryable<ModuleScores> GetModuleScoresWithParam(string employeeId, int trainingId)
        {

            return this
            .GetModuleScores
            .FromSql($@"
SELECT Scores.SetupModuleId, M.ModuleName, Scores.TotalScore as Score
FROM EnrollLearnings EL
         JOIN EnrollLearningTimes ELT ON EL.EnrollLearningId = ELT.EnrollLearningId
         JOIN(SELECT TA.SetupModuleId,
                     SM.IsForRemedial,
                     SM.ModuleId,
                     TA.TrainingId,
                     EmployeeId,
                     SUM(TAD.Score) AS [TotalScore]
              FROM TaskAnswers TA
                       JOIN TaskAnswerDetails TAD ON TA.TaskAnswerId = TAD.TaskAnswerId
                       JOIN SetupModules SM ON TA.SetupModuleId = SM.SetupModuleId
              GROUP BY TA.SetupModuleId, TA.TrainingId, EmployeeId, IsForRemedial, SM.ModuleId) [Scores]
             ON ELT.SetupModuleId = Scores.SetupModuleId AND EL.TrainingId = Scores.TrainingId AND
                Scores.EmployeeId = El.EmployeeId
         JOIN Modules M ON Scores.ModuleId = M.ModuleId
WHERE EL.EmployeeId = '{employeeId}'
  AND EL.TrainingId = {trainingId}
		", employeeId, trainingId);

        }



        public IQueryable<PersonalMappingModel> GetPersonalMapping(string employeeId)
        {
            var query = this.GetPersonalMappings.
                FromSql($@"
SELECT 
	c.PrefixCode, 
	c.CompetencyName,
	c.CompetencyDescription,
    ct.CompetencyTypeId,
	ct.CompetencyTypeName,
	pcm.ProficiencyLevel,
	ka.KeyActionCode, 
	ka.KeyActionName,
	ka.KeyActionDescription,
	et.EvaluationTypeName,
	et.EvaluationTypeId,
	SUM(tad.score) AS Score, 
	SUM(maxScore.Score) AS [MaxScore]
FROM trainings t
JOIN SetupModules sm ON t.CourseId = sm.CourseId
JOIN ModuleTopicMappings mtm ON mtm.ModuleId = sm.ModuleId
JOIN EnrollLearnings el ON t.TrainingId = el.TrainingId
JOIN TaskAnswers ta ON sm.SetupModuleId = ta.SetupModuleId and ta.EmployeeId = el.EmployeeId
JOIN TaskAnswerDetails tad ON ta.TaskAnswerId = tad.TaskAnswerId
JOIN Tasks tas ON tad.TaskId = tas.TaskId
JOIN Competencies c ON c.CompetencyId = tas.CompetencyId
JOIN CompetencyKeyActionMappings ckam ON c.CompetencyId = ckam.CompetencyId
JOIN KeyActions ka ON ckam.KeyActionId = ka.KeyActionId
JOIN EvaluationTypes et ON tas.EvaluationTypeId = et.EvaluationTypeId
JOIN CompetencyTypes ct ON c.CompetencyTypeId = ct.CompetencyTypeId
JOIN EmployeePositionMappings epm ON el.EmployeeId = epm.EmployeeId 
JOIN Positions p ON epm.PositionId = p.PositionId 
JOIN PositionCompentencyMappings pcm ON tas.CompetencyId = pcm.CompetencyId and p.PositionId = pcm.PositionId
JOIN (
	SELECT 
		Q.TaskId, 
		SUM(tp.Score) AS Score 
	FROM TimePoints tp 
	JOIN (
	SELECT
		TaskId, Score
		FROM TaskTebakGambarTypes
	UNION
	SELECT
		TaskId, Score
		FROM TaskTrueFalseTypes
	UNION
	SELECT
		TaskId, Score
		FROM TaskMultipleChoiceTypes
	UNION
	SELECT
		TaskId, Score
		FROM TaskMatchingTypes tmt
	UNION
	SELECT
		TaskId, null AS Score
		FROM TaskShortAnswerTypes tsat
	UNION
	SELECT
		TaskId, null AS Score
		FROM TaskFileUploadTypes tfut 
	UNION
	SELECT
		TaskId, null AS Score
		FROM TaskEssayTypes tet
	UNION ALL
	SELECT
		TaskId, Score AS Score
		FROM TaskHotSpotAnswers
	UNION ALL
	SELECT
		TaskId, Score AS Score
		FROM TaskChecklistChoices
	UNION ALL
	SELECT
		TaskId, Score AS Score
		FROM TaskSequenceChoices
	) AS Q on Q.Score = tp.TimePointId
	GROUP BY Q.TaskId
	UNION
	SELECT 
		trt.TaskId, (tp1.Score + tp2.Score + tp3.Score + tp4.Score + tp5.Score) AS [Score] 
	FROM TaskRatingTypes trt 
	JOIN TimePoints tp1 ON trt.Score0To20 = tp1.TimePointId
	JOIN TimePoints tp2 ON trt.Score21To40 = tp2.TimePointId
	JOIN TimePoints tp3 ON trt.Score41To60 = tp3.TimePointId
	JOIN TimePoints tp4 ON trt.Score61To80 = tp4.TimePointId
	JOIN TimePoints tp5 On trt.Score81To100 = tp5.TimePointId
	UNION
	SELECT 
		TaskId,
		(tp1.Score + tp2.Score + tp3.Score + tp4.Score + tp5.Score) as TotalScore
	FROM TaskMatrixTypes tmt
	JOIN TimePoints tp1 on tmt.ScoreColumn1 = tp1.TimePointId
	JOIN TimePoints tp2 on tmt.ScoreColumn2 = tp2.TimePointId
	JOIN TimePoints tp3 on tmt.ScoreColumn3 = tp3.TimePointId
	JOIN TimePoints tp4 on tmt.ScoreColumn4 = tp4.TimePointId
	JOIN TimePoints tp5 on tmt.ScoreColumn5 = tp5.TimePointId
	) maxScore on tas.TaskId = maxScore.TaskId
WHERE el.EmployeeId = {employeeId}
GROUP BY
	c.PrefixCode, 
	ka.KeyActionCode, 
	et.EvaluationTypeName, 
	et.EvaluationTypeId,
	c.CompetencyName,
	c.CompetencyDescription,
	ct.CompetencyTypeName,
	ct.CompetencyTypeId,
	pcm.ProficiencyLevel,
	ka.KeyActionName,
	ka.KeyActionDescription");


            return query;
        }

        public IQueryable<TeamMappingDemandedModel> GetDemandedTeamMapping(int teamId, int positionId, List<int> competencyIds)
        {
            var queryBuilder = $@"
SELECT
	c.PrefixCode,
	c.CompetencyId,
	c.CompetencyName,
	c.CompetencyDescription,
	ct.CompetencyTypeId,
	ct.CompetencyTypeName,
	Q1.PositionId,
	Q1.KeyActionCode,
	Q1.KeyActionName,
	Q1.KeyActionDescription,
	Q2.EmployeeId,
	Q2.ProficiencyLevel,
	Q2.BlobId,
	Q2.TeamId
FROM Competencies c 
JOIN CompetencyTypes ct ON c.CompetencyTypeId = ct.CompetencyTypeId
JOIN (
	SELECT 
		ckam.CompetencyId,
		epm.PositionId,
		ka.KeyActionCode,
		ka.KeyActionName,
		ka.KeyActionDescription
	FROM CompetencyKeyActionMappings ckam 
	JOIN KeyActions ka ON ckam.KeyActionId = ka.KeyActionId
	CROSS JOIN EmployeePositionMappings epm
	JOIN Positions p ON epm.PositionId = p.PositionId
	JOIN TeamDetails td ON epm.EmployeeId = td.EmployeeId
	WHERE td.TeamId = {teamId} and p.PositionId = {positionId}
	GROUP BY 
		ckam.CompetencyId,
		epm.PositionId,
		ka.KeyActionCode,
		ka.KeyActionName,
		ka.KeyActionDescription
) Q1 on Q1.CompetencyId = c.CompetencyId
JOIN (
	SELECT 
		pcm.CompetencyId,
		epm.EmployeeId,
		pcm.ProficiencyLevel,
		e.BlobId,
		td.TeamId,
		epm.PositionId
	FROM PositionCompentencyMappings pcm 
	JOIN EmployeePositionMappings epm ON pcm.PositionId = epm.PositionId
	JOIN Employees e ON epm.EmployeeId = e.EmployeeId
	JOIN TeamDetails td ON e.EmployeeId = td.EmployeeId
	WHERE td.TeamId = {teamId} and epm.PositionId = {positionId}
) Q2 ON c.CompetencyId = Q2.CompetencyId 
";


            var competencyIdString = String.Join(',', competencyIds);
            queryBuilder = queryBuilder + $@"WHERE c.CompetencyId IN ({competencyIdString})";

            var query = (RawSqlString)queryBuilder;

            var generateQuery = this
               .GetTeamMappingDemanded
               .FromSql(query);


            return generateQuery;
        }

        public IQueryable<TeamMappingEvaluatedModel> GetEvaluatedTeamMapping(int teamId, int positionId, List<int> competencyIds)
        {
            var queryBuilder = $@"
SELECT 
	c.PrefixCode, 
	c.CompetencyId,
	c.CompetencyName,
	c.CompetencyDescription,
    ct.CompetencyTypeId,
	ct.CompetencyTypeName,
	epm.PositionId,
	ka.KeyActionCode, 
	ka.KeyActionName,
	ka.KeyActionDescription,
	epm.EmployeeId,
	pcm.ProficiencyLevel,
	e.BlobId,
	td.TeamId,
	et.EvaluationTypeName,
	et.EvaluationTypeId,
	SUM(tad.score) AS Score, 
	SUM(maxScore.Score) AS [MaxScore]
FROM trainings t
JOIN SetupModules sm ON t.CourseId = sm.CourseId
JOIN ModuleTopicMappings mtm ON mtm.ModuleId = sm.ModuleId
JOIN EnrollLearnings el ON t.TrainingId = el.TrainingId 
JOIN TeamDetails td ON el.EmployeeId = td.EmployeeId
JOIN TaskAnswers ta ON sm.SetupModuleId = ta.SetupModuleId and ta.EmployeeId = el.EmployeeId
JOIN TaskAnswerDetails tad ON ta.TaskAnswerId = tad.TaskAnswerId
JOIN Tasks tas ON tad.TaskId = tas.TaskId
JOIN Competencies c ON c.CompetencyId = tas.CompetencyId
JOIN CompetencyKeyActionMappings ckam ON c.CompetencyId = ckam.CompetencyId
JOIN KeyActions ka ON ckam.KeyActionId = ka.KeyActionId
JOIN EvaluationTypes et ON tas.EvaluationTypeId = et.EvaluationTypeId
JOIN CompetencyTypes ct ON c.CompetencyTypeId = ct.CompetencyTypeId
JOIN EmployeePositionMappings epm ON el.EmployeeId = epm.EmployeeId 
JOIN Employees e ON epm.EmployeeId = e.EmployeeId
JOIN Positions p ON epm.PositionId = p.PositionId 
JOIN PositionCompentencyMappings pcm ON tas.CompetencyId = pcm.CompetencyId and p.PositionId = pcm.PositionId
JOIN (
	SELECT 
		Q.TaskId, 
		SUM(tp.Score) AS Score 
	FROM TimePoints tp 
	JOIN (
	SELECT
		TaskId, Score
		FROM TaskTebakGambarTypes
	UNION
	SELECT
		TaskId, Score
		FROM TaskTrueFalseTypes
	UNION
	SELECT
		TaskId, Score
		FROM TaskMultipleChoiceTypes
	UNION
	SELECT
		TaskId, Score
		FROM TaskMatchingTypes tmt
	UNION
	SELECT
		TaskId, null AS Score
		FROM TaskShortAnswerTypes tsat
	UNION
	SELECT
		TaskId, null AS Score
		FROM TaskFileUploadTypes tfut 
	UNION
	SELECT
		TaskId, null AS Score
		FROM TaskEssayTypes tet
	UNION ALL
	SELECT
		TaskId, Score AS Score
		FROM TaskHotSpotAnswers
	UNION ALL
	SELECT
		TaskId, Score AS Score
		FROM TaskChecklistChoices
	UNION ALL
	SELECT
		TaskId, Score AS Score
		FROM TaskSequenceChoices
	) AS Q on Q.Score = tp.TimePointId
	GROUP BY Q.TaskId
	UNION
	SELECT 
		trt.TaskId, (tp1.Score + tp2.Score + tp3.Score + tp4.Score + tp5.Score) AS [Score] 
	FROM TaskRatingTypes trt 
	JOIN TimePoints tp1 ON trt.Score0To20 = tp1.TimePointId
	JOIN TimePoints tp2 ON trt.Score21To40 = tp2.TimePointId
	JOIN TimePoints tp3 ON trt.Score41To60 = tp3.TimePointId
	JOIN TimePoints tp4 ON trt.Score61To80 = tp4.TimePointId
	JOIN TimePoints tp5 On trt.Score81To100 = tp5.TimePointId
	UNION
	SELECT 
		TaskId,
		(tp1.Score + tp2.Score + tp3.Score + tp4.Score + tp5.Score) as TotalScore
	FROM TaskMatrixTypes tmt
	JOIN TimePoints tp1 on tmt.ScoreColumn1 = tp1.TimePointId
	JOIN TimePoints tp2 on tmt.ScoreColumn2 = tp2.TimePointId
	JOIN TimePoints tp3 on tmt.ScoreColumn3 = tp3.TimePointId
	JOIN TimePoints tp4 on tmt.ScoreColumn4 = tp4.TimePointId
	JOIN TimePoints tp5 on tmt.ScoreColumn5 = tp5.TimePointId
	) maxScore on tas.TaskId = maxScore.TaskId
WHERE td.TeamId = {teamId} and p.PositionId = {positionId} 
";


            var competencyIdString = String.Join(',', competencyIds);
            queryBuilder = queryBuilder + $@"AND c.CompetencyId IN ({competencyIdString}) 
GROUP BY
	c.PrefixCode, 
	ka.KeyActionCode, 
	et.EvaluationTypeName, 
	et.EvaluationTypeId,
	c.CompetencyName,
	c.CompetencyDescription,
	ct.CompetencyTypeName,
	ct.CompetencyTypeId,
	pcm.ProficiencyLevel,
	ka.KeyActionName,
	epm.EmployeeId,
	c.CompetencyId,
	epm.PositionId,
	td.TeamId,
	e.BlobId,
	ka.KeyActionDescription";

            var query = (RawSqlString)queryBuilder;

            var generateQuery = this
               .GetTeamMappingEvaluated
               .FromSql(query);


            return generateQuery;
        }

        public IQueryable<DashboardEvaluatedCompetencyMappingModel> GetDashboardEvaluatedCompetencyMapping(string positionName)
        {
            var query = this.GetEvaluatedCompetencyMappings.
                FromSql($@"
SELECT 
	c.PrefixCode, 
    ct.CompetencyTypeId,
	pcm.ProficiencyLevel,
	ka.KeyActionCode, 
	et.EvaluationTypeId,
	SUM(tad.score) AS Score, 
	SUM(maxScore.Score) AS [MaxScore]
FROM trainings t
JOIN SetupModules sm ON t.CourseId = sm.CourseId
JOIN ModuleTopicMappings mtm ON mtm.ModuleId = sm.ModuleId
JOIN EnrollLearnings el ON t.TrainingId = el.TrainingId
JOIN TaskAnswers ta ON sm.SetupModuleId = ta.SetupModuleId and ta.EmployeeId = el.EmployeeId
JOIN TaskAnswerDetails tad ON ta.TaskAnswerId = tad.TaskAnswerId 
JOIN Tasks tas ON tad.TaskId = tas.TaskId
JOIN Competencies c ON c.CompetencyId = tas.CompetencyId
JOIN CompetencyKeyActionMappings ckam ON c.CompetencyId = ckam.CompetencyId
JOIN KeyActions ka ON ckam.KeyActionId = ka.KeyActionId
JOIN EvaluationTypes et ON tas.EvaluationTypeId = et.EvaluationTypeId
JOIN CompetencyTypes ct ON c.CompetencyTypeId = ct.CompetencyTypeId
JOIN EmployeePositionMappings epm ON el.EmployeeId = epm.EmployeeId 
JOIN Positions p ON epm.PositionId = p.PositionId 
JOIN PositionCompentencyMappings pcm ON tas.CompetencyId = pcm.CompetencyId and p.PositionId = pcm.PositionId
JOIN (
	SELECT 
		Q.TaskId, 
		SUM(tp.Score) AS Score 
	FROM TimePoints tp 
	JOIN (
	SELECT
		TaskId, Score
		FROM TaskTebakGambarTypes
	UNION
	SELECT
		TaskId, Score
		FROM TaskTrueFalseTypes
	UNION
	SELECT
		TaskId, Score
		FROM TaskMultipleChoiceTypes
	UNION
	SELECT
		TaskId, Score
		FROM TaskMatchingTypes tmt
	UNION
	SELECT
		TaskId, null AS Score
		FROM TaskShortAnswerTypes tsat
	UNION
	SELECT
		TaskId, null AS Score
		FROM TaskFileUploadTypes tfut 
	UNION
	SELECT
		TaskId, null AS Score
		FROM TaskEssayTypes tet
	UNION ALL
	SELECT
		TaskId, Score AS Score
		FROM TaskHotSpotAnswers
	UNION ALL
	SELECT
		TaskId, Score AS Score
		FROM TaskChecklistChoices
	UNION ALL
	SELECT
		TaskId, Score AS Score
		FROM TaskSequenceChoices
	) AS Q on Q.Score = tp.TimePointId
	GROUP BY Q.TaskId
	UNION
	SELECT 
		trt.TaskId, (tp1.Score + tp2.Score + tp3.Score + tp4.Score + tp5.Score) AS [Score] 
	FROM TaskRatingTypes trt 
	JOIN TimePoints tp1 ON trt.Score0To20 = tp1.TimePointId
	JOIN TimePoints tp2 ON trt.Score21To40 = tp2.TimePointId
	JOIN TimePoints tp3 ON trt.Score41To60 = tp3.TimePointId
	JOIN TimePoints tp4 ON trt.Score61To80 = tp4.TimePointId
	JOIN TimePoints tp5 On trt.Score81To100 = tp5.TimePointId
	UNION
	SELECT 
		TaskId,
		(tp1.Score + tp2.Score + tp3.Score + tp4.Score + tp5.Score) as TotalScore
	FROM TaskMatrixTypes tmt
	JOIN TimePoints tp1 on tmt.ScoreColumn1 = tp1.TimePointId
	JOIN TimePoints tp2 on tmt.ScoreColumn2 = tp2.TimePointId
	JOIN TimePoints tp3 on tmt.ScoreColumn3 = tp3.TimePointId
	JOIN TimePoints tp4 on tmt.ScoreColumn4 = tp4.TimePointId
	JOIN TimePoints tp5 on tmt.ScoreColumn5 = tp5.TimePointId
	) maxScore on tas.TaskId = maxScore.TaskId
WHERE p.PositionName = {positionName}
GROUP BY
	c.PrefixCode, 
	ka.KeyActionCode, 
	et.EvaluationTypeId,
	ct.CompetencyTypeId,
	pcm.ProficiencyLevel
");


            return query;
        }

        public IQueryable<PositionCoachSearchQueryModel> GetPositionCoachSearchList(List<int> positionIds)
        {

            var queryBuilder = $@"SELECT '*'+STRING_AGG((p.PositionId),'*,*') +'*' as PositionId, c.CoachId
FROM Coaches c
JOIN Employees e ON c.EmployeeId = e.EmployeeId
LEFT JOIN EmployeePositionMappings epm ON e.EmployeeId = epm.EmployeeId
LEFT JOIN Positions p ON p.PositionId = epm.PositionId
JOIN CoachTopicMappings ctm ON c.CoachId = ctm.CoachId ";

            var positionIdString = String.Join(',', positionIds);
            queryBuilder = queryBuilder + $@"WHERE p.PositionId in ({positionIdString}) GROUP BY c.CoachId";

            var query = (RawSqlString)queryBuilder;

            var generateQuery = this
               .GetPositionCoachSearch
               .FromSql(query);

            return generateQuery;
        }

        public IQueryable<TopicCoachSearchQueryModel> GetTopicCoachSearchList(List<int> topicsIds)
        {

            var queryBuilder = $@"SELECT DISTINCT
		 c.CoachId
    FROM Coaches c
    JOIN Employees e ON c.EmployeeId = e.EmployeeId
    LEFT JOIN EmployeePositionMappings epm ON e.EmployeeId = epm.EmployeeId
    LEFT JOIN Positions p ON p.PositionId = epm.PositionId
    LEFT JOIN Outlets o ON o.OutletId = e.OutletId
    LEFT JOIN Blobs b ON e.BlobId = b.BlobId
    JOIN CoachTopicMappings ctm ON c.CoachId = ctm.CoachId
    JOIN Topics t ON t.TopicId = ctm.TopicId
	LEFT JOIN EmployeeBadges eb ON e.EmployeeId = eb.EmployeeId
	LEFT JOIN Topics ebt ON eb.TopicId = ebt.TopicId ";

            var topicIdString = String.Join(',', topicsIds);
            queryBuilder = queryBuilder + $@"WHERE (ctm.TopicId IN ({topicIdString}) OR ebt.TopicId IN ({topicIdString})) GROUP BY c.CoachId";

            var query = (RawSqlString)queryBuilder;

            var generateQuery = this
               .GetTopicCoachSearch
               .FromSql(query);

            return generateQuery;
        }

        public IQueryable<EbadgeCoachSearchQueryModel> GetEbadgeCoachSearchList(List<int> ebadgeIds)
        {

            var queryBuilder = $@"SELECT DISTINCT
		c.CoachId
    FROM Coaches c
    JOIN Employees e ON c.EmployeeId = e.EmployeeId
    LEFT JOIN EmployeePositionMappings epm ON e.EmployeeId = epm.EmployeeId
    LEFT JOIN Positions p ON p.PositionId = epm.PositionId
    LEFT JOIN Outlets o ON o.OutletId = e.OutletId
    LEFT JOIN Blobs b ON e.BlobId = b.BlobId
    JOIN CoachTopicMappings ctm ON c.CoachId = ctm.CoachId
    JOIN Topics t ON t.TopicId = ctm.TopicId
	LEFT JOIN EmployeeBadges eb ON e.EmployeeId = eb.EmployeeId
	LEFT JOIN Topics ebt ON ebt.TopicId = eb.TopicId ";

            var ebadgeIdString = String.Join(',', ebadgeIds);
            queryBuilder = queryBuilder + $@"WHERE (t.EBadgeId IN ({ebadgeIdString}) OR ebt.EBadgeId IN ({ebadgeIdString})) GROUP BY c.CoachId";

            var query = (RawSqlString)queryBuilder;

            var generateQuery = this
               .GetEbadgeCoachSearch
               .FromSql(query);

            return generateQuery;
        }

        public IQueryable<ReportCompetencyTracking> GetCompetencyTracking(String trainingID, String dealerID, String outletID, String areaID, String employeeID, String employeeName, String positions, String status)
        {
            string query = "SELECT * FROM [dbo].[FReport_Competency_Track](" +
                    "" + trainingID + "," +
                    "'" + dealerID + "'," +
                    "'" + outletID + "'," +
                    "'" + areaID + "'," +
                    "'" + employeeID + "'," +
                    "'" + employeeName + "'," +
                    "'" + positions + "'" +
                    ") ";

            if (status != "*")
            {
                if (status.Contains(","))
                {
                    string[] statuses = status.TrimStart(',').Split(',');

                    if (Array.IndexOf(statuses, "BELUM") != -1)
                        statuses[Array.IndexOf(statuses, "BELUM")] = "";

                    for (int i = 0; i < statuses.Length; i++)
                        statuses[i] = "'" + statuses[i] + "'";

                    query += "WHERE Video IN(" + string.Join(",", statuses) + ")";
                } else if (status != "*")
                {
                    if (status == "BELUM")
                        query += "WHERE Video = ''";
                    else
                        query += "WHERE Video = '" + status + "'";
                }
            }
            
            Debug.WriteLine(query);

            return this.ReportCompetencyTracking.FromSql(query);
        }

        public IQueryable<ReportKPITracking> GetKPITracking(String trainingID, String dealerID, String areaID)
        {
            var query = this
                .ReportKPITracking
                .FromSql($@"SELECT * FROM [dbo].[FReport_KPI](" +
                    "" + trainingID + "," +
                    "'" + dealerID + "'," +
                    "'" + areaID + "'" +
                    ");");

            return query;
        }

        public IQueryable<ReportKPITracking2> GetKPITracking2(String trainingID, String dealerID, String areaID)
        {
            var query = this
                .ReportKPITracking2
                .FromSql($@"SELECT * FROM [dbo].[FReport_KPI3](" +
                    "" + trainingID + "," +
                    "'" + dealerID + "'," +
                    "'" + areaID + "'" +
                    ");");

            return query;
        }

        public IQueryable<EmployeeByCoach> GetEmployeeListByCoach(string trainingId)
        {
            var query = this.EmployeeByCoach.FromSql($@"
				SELECT tpm.[TrainingId] AS TrainingId
				,tpm.[PositionId] AS PositionId
				,p.positionname AS PositionName
				,e.employeename AS EmployeeName
				,e.employeeid AS EmployeeId
				,o.outletid AS OutletId
				,o.DealerId AS DealerId
                ,o.[Name] AS OutletName
			FROM [TrainingPositionMappings] tpm
			join employeepositionmappings epm on tpm.positionid = epm.positionid
			join employees e on epm.employeeid = e.employeeid
			join positions p on epm.positionid = p.positionid
			left join outlets o on e.outletid = o.outletid
			where tpm.trainingid = {trainingId} AND  e.IsDeleted = 0 
			GROUP BY tpm.[TrainingId]
				,tpm.[PositionId]
				,p.positionname
				,e.employeename
				,e.employeeid
				,o.outletid
				,o.DealerId
                ,o.[Name]");
            return query;
        }

        public IQueryable<EmployeeByCoach> GetEmployeeListByCoach(string trainingId, string dealerId)
        {
            var query = this.EmployeeByCoach.FromSql($@"
				SELECT tpm.[TrainingId] AS TrainingId
				,tpm.[PositionId] AS PositionId
				,p.positionname AS PositionName
				,e.employeename AS EmployeeName
				,e.employeeid AS EmployeeId
				,o.outletid AS OutletId
				,o.DealerId AS DealerId
                ,o.[Name] AS OutletName
			FROM [TrainingPositionMappings] tpm
			join employeepositionmappings epm on tpm.positionid = epm.positionid
			join employees e on epm.employeeid = e.employeeid
			join positions p on epm.positionid = p.positionid
			left join outlets o on e.outletid = o.outletid
            join trainingoutletmappings tom on o.outletid = tom.outletid
			where tpm.trainingid = {trainingId} AND  e.IsDeleted = 0  AND o.DealerId = {dealerId}
            AND tom.trainingid = {trainingId}
			GROUP BY tpm.[TrainingId]
				,tpm.[PositionId]
				,p.positionname
				,e.employeename
				,e.employeeid
				,o.outletid
				,o.DealerId
                ,o.[Name]");
            return query;
        }

        public IQueryable<EmployeeByCoach> GetEmployeeListByCoach(string trainingId, string dealerId, string outletId)
        {
            var query = this.EmployeeByCoach.FromSql($@"
			SELECT tpm.[TrainingId] AS TrainingId
				,tpm.[PositionId] AS PositionId
				,p.positionname AS PositionName
				,e.employeename AS EmployeeName
				,e.employeeid AS EmployeeId
				,o.outletid AS OutletId
				,o.DealerId AS DealerId
                ,o.[Name] AS OutletName
			FROM [TrainingPositionMappings] tpm
			join employeepositionmappings epm on tpm.positionid = epm.positionid
			join employees e on epm.employeeid = e.employeeid
			join positions p on epm.positionid = p.positionid
			left join outlets o on e.outletid = o.outletid
            join trainingoutletmappings tom on o.outletid = tom.outletid
			where tpm.trainingid = {trainingId} AND  e.IsDeleted = 0  AND o.DealerId = {dealerId} AND o.OutletId={outletId}
            AND tom.trainingid = {trainingId}
			GROUP BY tpm.[TrainingId]
				,tpm.[PositionId]
				,p.positionname
				,e.employeename
				,e.employeeid
				,o.outletid
				,o.DealerId
                ,o.[Name]");
            return query;
        }

		public IQueryable<GetDataMyCoachDetail> GetDataMyCoachDetailSummary(int trainingId)
        {
			var query = this.GetDataMyCoachDetail.FromSql($@"
			SELECT *
			FROM [GetAllMyCoachReport] coach
			where coach.trainingid = {trainingId}");
			return query;
		}
    }
}
