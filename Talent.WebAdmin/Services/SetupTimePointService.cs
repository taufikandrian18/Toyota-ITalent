using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Talent.Entities.Entities;
using Talent.WebAdmin.Helpers;
using Talent.WebAdmin.Models;

namespace Talent.WebAdmin.Services
{
    public class SetupTimePointService
    {
        private readonly TalentContext DB;
        private readonly PointTypeService _PointTypeMan;
        private readonly ClaimHelper ClaimMan;

        public SetupTimePointService(TalentContext talentContext, PointTypeService pointTypeService, ClaimHelper claim)
        {
            this.DB = talentContext;
            this._PointTypeMan = pointTypeService;
            this.ClaimMan = claim;
        }

        /// <summary>
        /// Function untuk menjoin table time points dan point types
        /// </summary>
        /// <returns></returns>
        public IQueryable<SetupTimePoint> JoinTable()
        {
            var query = (from stp in this.DB.TimePoints
                         join pt in this.DB.PointTypes on stp.PointTypeId equals pt.PointTypeId
                         where stp.IsDeleted == false
                         select new SetupTimePoint
                         {
                             TimePointId = stp.TimePointId,
                             PointTypeId = stp.PointTypeId,
                             PointTypeName = pt.PointTypeName,
                             Time = stp.Time,
                             Points = stp.Points,
                             Score = stp.Score,
                             CreatedDate = stp.CreatedAt,
                             UpdatedDate = stp.UpdatedAt
                         }).AsNoTracking().AsQueryable();
            return query;
        }

        public IQueryable<SetupTimePoint> JoinTableWithDeleted()
        {
            var query = (from stp in this.DB.TimePoints
                         join pt in this.DB.PointTypes on stp.PointTypeId equals pt.PointTypeId
                         select new SetupTimePoint
                         {
                             TimePointId = stp.TimePointId,
                             PointTypeId = stp.PointTypeId,
                             PointTypeName = pt.PointTypeName,
                             Time = stp.Time,
                             Points = stp.Points,
                             Score = stp.Score,
                             CreatedDate = stp.CreatedAt,
                             UpdatedDate = stp.UpdatedAt
                         }).AsNoTracking().AsQueryable();
            return query;
        }

        public async Task<SetupTimePointPaginate> GetAllSetupTimePointPaginateAsync(SetupTimePointGridFilter filter)
        {
            var query = JoinTable();

            //search
            if (filter.StartDate != null && filter.EndDate != null)
            {
                var newStartDate = filter.StartDate.Value.UtcDateTime.ToIndonesiaTimeZone();
                var startDate = new DateTime(newStartDate.Year, newStartDate.Month, newStartDate.Day, 0, 0, 0);

                var newEndDate = filter.EndDate.Value.UtcDateTime.ToIndonesiaTimeZone();
                var endDate = new DateTime(newEndDate.Year, newEndDate.Month, newEndDate.Day, 23, 59, 59);

                query = query.Where(Q => (Q.CreatedDate >= startDate && Q.CreatedDate <= endDate) || (Q.UpdatedDate >= startDate && Q.UpdatedDate <= endDate));
            }


            if (filter.Time != null)
            {
                query = query.Where(Q => Q.Time == filter.Time);
            }

            if (filter.Points != null)
            {
                query = query.Where(Q => Q.Points == filter.Points);
            }

            if (filter.TypeofPoints != null)
            {
                query = query.Where(Q => Q.PointTypeName.Contains(filter.TypeofPoints));
            }

            if (filter.Score != null)
            {
                query = query.Where(Q => Q.Score == filter.Score);
            }

            //sort
            switch (filter.SortBy)
            {
                case "TypeofPointAsc":
                    query = query.OrderBy(Q => Q.PointTypeName);
                    break;
                case "TimeAsc":
                    query = query.OrderBy(Q => Q.Time);
                    break;
                case "ScoreAsc":
                    query = query.OrderBy(Q => Q.Score);
                    break;
                case "PointAsc":
                    query = query.OrderBy(Q => Q.Points);
                    break;
                case "CreatedDateAsc":
                    query = query.OrderBy(Q => Q.CreatedDate);
                    break;
                case "UpdatedDateAsc":
                    query = query.OrderBy(Q => Q.UpdatedDate);
                    break;
                case "TypeofPointDesc":
                    query = query.OrderByDescending(Q => Q.PointTypeName);
                    break;
                case "TimeDesc":
                    query = query.OrderByDescending(Q => Q.Time);
                    break;
                case "ScoreDesc":
                    query = query.OrderByDescending(Q => Q.Score);
                    break;
                case "PointDesc":
                    query = query.OrderByDescending(Q => Q.Points);
                    break;
                case "CreatedDateDesc":
                    query = query.OrderByDescending(Q => Q.CreatedDate);
                    break;
                case "UpdatedDateDesc":
                    query = query.OrderByDescending(Q => Q.UpdatedDate);
                    break;
                default:
                    query = query.OrderByDescending(Q => Q.UpdatedDate);
                    break;
            }

            var setupTimePoint = await query.Skip((filter.PageIndex - 1) * filter.PageSize).Take(filter.PageSize).ToListAsync();

            var totalData = await query.CountAsync();

            return new SetupTimePointPaginate
            {
                Data = setupTimePoint,
                TotalData = totalData
            };
        }

        public async Task<SetupTimePoint> GetSetupTimePointGetIdAsync(int timePointId)
        {
            var query = JoinTable();
            var data = await query.FirstOrDefaultAsync(Q => Q.TimePointId == timePointId);
            return data;
        }

        public async Task<TimePointTaskModel> GetSetupTimePointByScoreAsync(int score)
        {
            //var query = JoinTableWithDeleted();
            //var data = await query.Where(Q => Q.Score == score).Select(Q => new TimePointTaskModel
            //{
            //    Points = Q.Points,
            //    Score = Q.Score,
            //    TimePointId = Q.TimePointId
            //}).FirstOrDefaultAsync();

            var data = await this.DB.TimePoints.Where(Q => Q.Score == score && Q.IsDeleted == false && Q.PointTypeId == 5).Select(Q => new TimePointTaskModel
            {
                Points = Q.Points,
                Score = Q.Score,
                TimePointId = Q.TimePointId
            }).FirstOrDefaultAsync();

            return data;
        }

        public async Task<bool> TimePointIsExist(int PointTypeId, int time, int score)
        {
            if (PointTypeId == 1 || PointTypeId == 2 || PointTypeId == 3)
            {
                var data1 = await this.DB.TimePoints.Where(Q => Q.IsDeleted == false).FirstOrDefaultAsync(Q => Q.PointTypeId == PointTypeId && Q.Time == time);

                if (data1 == null)
                {
                    return false;
                }

                return true;
            }

            var data = await this.DB.TimePoints.Where(Q => Q.IsDeleted == false).FirstOrDefaultAsync(Q => Q.PointTypeId == PointTypeId && Q.Score == score);

            if (data == null)
            {
                return false;
            }

            return true;

        }

        public async Task<bool> InsertTimePointAsync(SetupTimePointInsertModel insert)
        {
            //var thisDate = DateTime.UtcNow.AddHours(7);
            var thisDate = DateTime.UtcNow.ToIndonesiaTimeZone();
            //ToIndonesiaTimeZone()

            if (insert.Time < 0 || insert.Score < 0 || insert.Points < 0)
            {
                return false;
            }

            var exist = await this.TimePointIsExist(insert.PointTypeId, insert.Time, insert.Score);
            if (exist == true)
            {
                return false;
            }

            var name = await this._PointTypeMan.GetPointTypeById(insert.PointTypeId);

            if (name == "Learning Time" || name == "Teaching Time" || name == "Coaching Time")
            {
                var getPoint = await this.DB.TimePoints.Where(Q => Q.Time == insert.Time && Q.PointTypeId == insert.PointTypeId).FirstOrDefaultAsync();

                //INSERT KALAU DATA PERNAH ADA TAPI DI SOFT DELETE
                if (getPoint != null)
                {
                    getPoint.IsDeleted = false;
                    getPoint.Score = 0;
                    getPoint.Points = insert.Points;
                    getPoint.CreatedAt = thisDate;
                    getPoint.UpdatedAt = thisDate;
                    getPoint.CreatedBy = ClaimMan.GetLoginUserId();
                    getPoint.UpdatedBy = ClaimMan.GetLoginUserId();

                    await this.DB.SaveChangesAsync();
                    return true;
                }

                var point = await this.DB.TimePoints.AddAsync(new TimePoints
                {
                    PointTypeId = insert.PointTypeId,
                    Time = insert.Time,
                    Points = insert.Points,
                    Score = 0,
                    CreatedAt = thisDate,
                    UpdatedAt = thisDate,
                    CreatedBy = ClaimMan.GetLoginUserId(),
                    UpdatedBy = ClaimMan.GetLoginUserId(),
                    IsDeleted = false
                });
                await this.DB.SaveChangesAsync();
                return true;
            }

            var getScore = await this.DB.TimePoints.Where(Q => Q.Score == insert.Score && Q.PointTypeId == insert.PointTypeId).FirstOrDefaultAsync();

            //INSERT KALAU DATA PERNAH ADA TAPI DI SOFT DELETE
            if (getScore != null)
            {
                getScore.PointTypeId = insert.PointTypeId;
                getScore.IsDeleted = false;
                getScore.Time = 0;
                getScore.Points = insert.Points;
                getScore.CreatedAt = thisDate;
                getScore.UpdatedAt = thisDate;
                getScore.CreatedBy = ClaimMan.GetLoginUserId();
                getScore.UpdatedBy = ClaimMan.GetLoginUserId();

                await this.DB.SaveChangesAsync();
                return true;
            }

            var score = await this.DB.TimePoints.AddAsync(new TimePoints
            {
                PointTypeId = insert.PointTypeId,
                Time = 0,
                Points = insert.Points,
                Score = insert.Score,
                CreatedAt = thisDate,
                UpdatedAt = thisDate,
                CreatedBy = ClaimMan.GetLoginUserId(),
                UpdatedBy = ClaimMan.GetLoginUserId(),
                IsDeleted = false
            });
            await this.DB.SaveChangesAsync();
            return true;
        }

        public async Task<int> GetTimePointIdAsync(int id)
        {
            return await this.DB.TimePoints.Where(Q => Q.TimePointId == id).Select(Q => Q.PointTypeId).FirstOrDefaultAsync();
        }

        public async Task<bool> UpdateTimePointAsync(int id, SetupTimePointInsertModel update)
        {
            var thisDate = DateTime.UtcNow.ToIndonesiaTimeZone();

            var timeId = await GetTimePointIdAsync(id);

            if (update.PointTypeId == timeId)
            {
                if (update.Time < 0 || update.Score < 0 || update.Points < 0)
                {
                    return false;
                }

                var sameData = await this.DB.TimePoints.Where(Q => Q.TimePointId == id).FirstOrDefaultAsync();

                if (sameData.Score != update.Score)
                {
                    var alreadyExist = await this.TimePointIsExist(update.PointTypeId, update.Time, update.Score);
                    if (alreadyExist == true) return false;


                    if (sameData.PointTypeId == 5)
                    {
                        var isUsed = await this.IsScoreUsed(sameData.TimePointId);
                        if (isUsed) return false;
                    }
                }


                if (update.PointTypeId == 1 || update.PointTypeId == 2 || update.PointTypeId == 3)
                {
                    sameData.PointTypeId = update.PointTypeId;
                    sameData.Time = update.Time;
                    sameData.Points = update.Points;
                    sameData.Score = 0;
                    sameData.UpdatedAt = thisDate;
                    sameData.UpdatedBy = ClaimMan.GetLoginUserId();

                    await this.DB.SaveChangesAsync();
                    return true;
                }

                sameData.PointTypeId = update.PointTypeId;
                sameData.Time = 0;
                sameData.Points = update.Points;
                sameData.Score = update.Score;
                sameData.UpdatedAt = thisDate;
                sameData.UpdatedBy = ClaimMan.GetLoginUserId();

                await this.DB.SaveChangesAsync();
                return true;
            }

            if (update.Time < 0 || update.Score < 0 || update.Points < 0)
            {
                return false;
            }

            var exist = await this.TimePointIsExist(update.PointTypeId, update.Time, update.Score);
            if (exist == true)
            {
                return false;
            }

            var data = await this.DB.TimePoints.Where(Q => Q.TimePointId == id).FirstOrDefaultAsync();
            //if (data.PointTypeId == 5)
            //{
            //    var isUsed = await this.IsScoreUsed(data.Score.Value);
            //    if (isUsed) return false;
            //}

            //var name = await this._PointTypeMan.GetPointTypeById(update.PointTypeId);

            if (update.PointTypeId == 1 || update.PointTypeId == 2 || update.PointTypeId == 3)
            {
                data.PointTypeId = update.PointTypeId;
                data.Time = update.Time;
                data.Points = update.Points;
                data.Score = 0;
                data.UpdatedAt = thisDate;
                data.UpdatedBy = ClaimMan.GetLoginUserId();

                await this.DB.SaveChangesAsync();
                return true;
            }

            data.PointTypeId = update.PointTypeId;
            data.Time = 0;
            data.Points = update.Points;
            data.Score = update.Score;
            data.UpdatedAt = thisDate;
            data.UpdatedBy = ClaimMan.GetLoginUserId();

            await this.DB.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteTimePointAsync(int id)
        {
            var delete = await this.DB.TimePoints.FirstOrDefaultAsync(Q => Q.TimePointId == id);

            if (delete == null)
            {
                return false;
            }

            if (delete.PointTypeId == 5)
            {
                var scoreUsed = await IsScoreUsed(delete.TimePointId);
                if (scoreUsed == true)
                {
                    return false;
                }
            }

            delete.IsDeleted = true;

            //this._Db.Remove(delete);
            await this.DB.SaveChangesAsync();
            return true;
        }

        public async Task<bool> IsScoreUsed(int timePointId)
        {

            var isUsed = await this.DB.TaskMultipleChoiceTypes.AnyAsync(Q => Q.Score == timePointId);
            if (isUsed) return true;

            isUsed = await this.DB.TaskTrueFalseTypes.AnyAsync(Q => Q.Score == timePointId);
            if (isUsed) return true;

            isUsed = await this.DB.TaskSequenceChoices.AnyAsync(Q => Q.Score == timePointId);
            if (isUsed) return true;

            isUsed = await this.DB.TaskTebakGambarTypes.AnyAsync(Q => Q.Score == timePointId);
            if (isUsed) return true;

            isUsed = await this.DB.TaskHotSpotAnswers.AnyAsync(Q => Q.Score == timePointId);
            if (isUsed) return true;

            isUsed = await this.DB.TaskChecklistChoices.AnyAsync(Q => Q.Score == timePointId);
            if (isUsed) return true;

            isUsed = await this.DB.TaskMatchingTypes.AnyAsync(Q => Q.Score == timePointId);
            if (isUsed) return true;

            isUsed = await this.DB.TaskMatrixTypes.AnyAsync(Q => Q.ScoreColumn1 == timePointId || Q.ScoreColumn2 == timePointId || Q.ScoreColumn3 == timePointId || Q.ScoreColumn4 == timePointId || Q.ScoreColumn5 == timePointId);
            if (isUsed) return true;

            isUsed = await this.DB.TaskRatingTypes.AnyAsync(Q => Q.Score0To20 == timePointId || Q.Score21To40 == timePointId || Q.Score41To60 == timePointId || Q.Score61To80 == timePointId || Q.Score81To100 == timePointId);
            if (isUsed) return true;


            return false;
        }
    }
}
