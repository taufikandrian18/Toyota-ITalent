using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Talent.Entities.Entities;
using Talent.WebAdmin.Helpers;
using Talent.WebAdmin.Services;
using Talent.WebAdmin.UserSide.Models;
using TAM.Talent.Commons.Core.Interfaces;

namespace Talent.WebAdmin.UserSide.Services
{
    public class UserSideProfileService
    {
        private readonly TalentContext DB;
        private readonly IFileStorageService FileService;
        private readonly BlobService BlobService;
        private readonly UserSideRankService RankService;
        private readonly UserSideCertificateService CertificateService;

        public UserSideProfileService(TalentContext talentContext, IFileStorageService fileService, BlobService blobService, UserSideRankService rankService, UserSideCertificateService certificateService)
        {
            this.DB = talentContext;
            this.FileService = fileService;
            this.BlobService = blobService;
            this.RankService = rankService;
            this.CertificateService = certificateService;
        }

        /// <summary>
        /// Get detail data employee for My Profile.
        /// </summary>
        /// <param name="employeeId"></param>
        /// <returns></returns>
        public async Task<ProfileViewModel> GetProfile(string employeeId)
        {
            var queryProfile = await (from e in this.DB.Employees.AsNoTracking()

                                      join o in this.DB.Outlets.AsNoTracking()
                                      on e.OutletId equals o.OutletId into ao
                                      from aoa in ao.DefaultIfEmpty()

                                      join ar in this.DB.Areas.AsNoTracking()
                                      on aoa.AreaId equals ar.AreaId into aar
                                      from aarn in aar.DefaultIfEmpty()

                                      join pr in this.DB.Provinces.AsNoTracking()
                                      on aoa.ProvinceId equals pr.ProvinceId into apr
                                      from aprn in apr.DefaultIfEmpty()

                                      join d in this.DB.Dealers.AsNoTracking()
                                      on aoa.DealerId equals d.DealerId into ad
                                      from adn in ad.DefaultIfEmpty()

                                      join c in this.DB.Coaches.AsNoTracking()
                                      on e.EmployeeId equals c.EmployeeId into ac
                                      from cn in ac.DefaultIfEmpty()

                                      join td in this.DB.TeamDetails.AsNoTracking()
                                      on e.EmployeeId equals td.EmployeeId into atd
                                      from tdn in atd.DefaultIfEmpty()

                                      where e.EmployeeId == employeeId
                                      select new
                                      {
                                          Employee = e,
                                          Outlet = aoa,
                                          Area = aarn,
                                          Province = aprn,
                                          Dealer = adn,
                                          CoacheId = (int?)cn.CoachId > 0 ? (int?)cn.CoachId : null,
                                          TeamId = (int?)tdn.TeamId > 0 ? (int?)tdn.TeamId : null
                                      }).FirstOrDefaultAsync();

            // For data Employee.
            var dataEmployee = new UserSideEmployeeModel
            {
                EmployeeId = queryProfile.Employee.EmployeeId,
                EmployeeName = queryProfile.Employee.EmployeeName,
                Nickname = queryProfile.Employee.Nickname,
                EmployeeUsername = queryProfile.Employee.EmployeeUsername,
                DateOfBirth = queryProfile.Employee.DateOfBirth,
                Description = queryProfile.Employee.Description,
                EmployeeEmail = queryProfile.Employee.EmployeeEmail,
                EmployeePhone = queryProfile.Employee.EmployeePhone,
                Gender = queryProfile.Employee.Gender,
                ImageUrl = queryProfile.Employee.BlobId.ToString(),
                IsCoach = queryProfile.Employee.IsCoach,
                CoachId = queryProfile.CoacheId,
                TeamId = queryProfile.TeamId,
                Ktp = queryProfile.Employee.Ktp,
                Status = queryProfile.Employee.Status,
                StartDate = queryProfile.Employee.CreatedAt,
                EndDate = queryProfile.Employee.AccountValid,
                Religion = queryProfile.Employee.Religion,
                Address = queryProfile.Employee.Addresses,
                ManpowerStatus = queryProfile.Employee.ManpowerStatusType,
                ManpowerCode = queryProfile.Employee.ManpowerCode,
                IsRequestUpgrade = queryProfile.Employee.IsRequestUpgrade,
                IsSuspended = queryProfile.Employee.IsSuspended,
                NIP = queryProfile.Employee.NIP != null ? queryProfile.Employee.NIP : queryProfile.Employee.EmployeeId,
                MDMCode = queryProfile.Employee.EmployeeMDMCode,
                IsTeamLeader = DB.Teams.Where(x => x.TeamLeaderId == queryProfile.Employee.EmployeeId).Any(),
                PositionNote = queryProfile.Employee.PoistionNote
            };

            // For data Outlet.
            var dataOutlet = new UserSideOutletModel();
            if (queryProfile.Outlet != null)
            {
                dataOutlet.OutletId = queryProfile.Outlet.OutletId;
                dataOutlet.Name = queryProfile.Outlet.Name;
                dataOutlet.Phone = queryProfile.Outlet.Phone;
            }

            // For data Area.
            var dataArea = new UserSideAreaModel();
            if (queryProfile.Area != null)
            {
                dataArea.AreaId = queryProfile.Area.AreaId;
                dataArea.AreaName = queryProfile.Area.Name;
            }

            // For data province.
            var dataPovince = new ProvinceViewModel();
            if (queryProfile.Province != null)
            {
                dataPovince.ProvinceId = queryProfile.Province.ProvinceId;
                dataPovince.ProvinceName = queryProfile.Province.ProvinceName;
            }

            // For data dealer.
            var dataDealer = new UserSideDealerModel();
            if (queryProfile.Dealer != null)
            {
                dataDealer.DealerId = queryProfile.Dealer.DealerId;
                dataDealer.DealerName = queryProfile.Dealer.DealerName;
                dataDealer.IsOthers = queryProfile.Dealer.OtherCompany;
            }

            // Get image if blobId not null.
            if (!string.IsNullOrEmpty(dataEmployee.ImageUrl))
            {
                var imageUrlEmployee = "";
                var blobData = await this.BlobService.GetBlobById(Guid.Parse(dataEmployee.ImageUrl));

                imageUrlEmployee = await this.FileService.GetFileAsync(blobData.BlobId.ToString(), blobData.Mime);

                // Change BlobId with ImageUrl.
                dataEmployee.ImageUrl = imageUrlEmployee;
            }

            // For data Certificates.
            var dataCertificates = await this.CertificateService.GetCertificateByEmployeeId(employeeId);

            // For data Hobies.
            var dataHobbies = await (from ehm in this.DB.EmployeeHobbyMappings.AsNoTracking()
                                     join h in this.DB.Hobbies.AsNoTracking() on ehm.HobbyId equals h.HobbyId
                                     where ehm.EmployeeId == dataEmployee.EmployeeId
                                     select new UserSideHobbyModel
                                     {
                                         HobbyId = h.HobbyId,
                                         Name = h.Name,
                                         Description = h.Name
                                     }).ToListAsync();

            // For data interest
            var dataInterests = await (from eim in this.DB.EmployeeInterests.AsNoTracking()
                                       join t in this.DB.Topics.AsNoTracking() on eim.TopicId equals t.TopicId
                                       where eim.EmployeeId == dataEmployee.EmployeeId
                                       select new UserSideInterestModel
                                       {
                                           TopicId = t.TopicId,
                                           TopicName = t.TopicName,
                                           TopicDescription = t.TopicDescription
                                       }).ToListAsync();

            // For data total badge.
            var queryBadge = await this
                .DB
                .EmployeeBadges
                .AsNoTracking()
                .Where(Q => Q.EmployeeId == dataEmployee.EmployeeId)
                .Select(Q => Q.EbadgeId)
                .ToListAsync();

            var bronzeBadgesAmount = queryBadge.Where(Q => Q.Value == (int)Enums.Badge.Bronze).Count();
            var silverBadgesAmount = queryBadge.Where(Q => Q.Value == (int)Enums.Badge.Silver).Count();
            var goldBadgesAmount = queryBadge.Where(Q => Q.Value == (int)Enums.Badge.Gold).Count();

            var coach = await this.DB.Coaches.Where(Q => Q.EmployeeId.ToLower() == employeeId.ToLower() && Q.CoachIsActive == true).FirstOrDefaultAsync();

            if (coach != null)
            {
                var coachQueryEbadges = await (
                    from ctm in this.DB.CoachTopicMappings
                    join t in this.DB.Topics on ctm.TopicId equals t.TopicId
                    join eb in this.DB.Ebadges on t.EbadgeId equals eb.EbadgeId
                    where ctm.CoachId == coach.CoachId
                    select eb.EbadgeId).ToListAsync();

                goldBadgesAmount += coachQueryEbadges.Where(Q => Q == (int)Enums.Badge.Gold).Count();
                silverBadgesAmount += coachQueryEbadges.Where(Q => Q == (int)Enums.Badge.Silver).Count();
                bronzeBadgesAmount += coachQueryEbadges.Where(Q => Q == (int)Enums.Badge.Bronze).Count();
            }

            var dataTotalBadge = new UserSideTotalBadgeModel
            {
                Bronze = bronzeBadgesAmount,
                Silver = silverBadgesAmount,
                Gold = goldBadgesAmount
            };

            // For data Position.
            var dataPosition = await (from epm in this.DB.EmployeePositionMappings.AsNoTracking()
                                      join p in this.DB.Positions.AsNoTracking()
                                      on epm.PositionId equals p.PositionId into ap
                                      from apn in ap.DefaultIfEmpty()
                                      where epm.EmployeeId == dataEmployee.EmployeeId
                                      select new UserSidePositionModel
                                      {
                                          PositionId = apn.PositionId,
                                          PositionName = apn.PositionName,
                                          PositionDescription = apn.PositionDescription,
                                          IsOtherDealer = apn.IsOtherDealer,
                                      }).ToListAsync();

            //Data position for guest account
            //Default assign because auto deleted by worker
            if (dataPosition.Count == 0 && dataEmployee.ManpowerStatus == "GuestAccount")
            {
                dataPosition = await(from p in this.DB.Positions.AsNoTracking()
                                     where p.PositionName == dataEmployee.PositionNote
                                     select new UserSidePositionModel
                                     {
                                         PositionId = p.PositionId,
                                         PositionName = p.PositionName,
                                         PositionDescription = p.PositionDescription,
                                         IsOtherDealer = p.IsOtherDealer,
                                     }).ToListAsync();
            }

            // Storing all data.
            var dataProfile = new ProfileViewModel
            {
                Employee = dataEmployee,
                Position = dataPosition,
                Outlet = dataOutlet,
                Area = dataArea,
                Province = dataPovince,
                Dealer = dataDealer,
                Certificates = dataCertificates,
                Hobbies = dataHobbies,
                Interests = dataInterests,
                TotalBadge = dataTotalBadge
            };


            //if (coach != null)
            //{
            //var isDataMember = await this.DB.Teams.Where(Q => Q.TeamLeaderId == employeeId.Trim()).Include(Q => Q.TeamDetails).Select(Q => Q.TeamDetails).ToListAsync();
            var isDataMember = await (from t in DB.Teams
                                      join d in DB.TeamDetails on t.TeamId equals d.TeamId
                                      where t.TeamLeaderId.ToLower() == employeeId.ToLower()    
                                      select d).ToListAsync();

            if (isDataMember.Count() > 0)
            {
                dataProfile.isDataMember = true;
            }
            else
            {
                dataProfile.isDataMember = false;
            }
            //}

            return dataProfile;
        }

        /// <summary>
        /// Update profile employee.
        /// </summary>
        /// <param name="newProfile"></param>
        /// <returns></returns>
        public async Task UpdateProfile(ProfileUpdateModel newProfile, string employeeId)
        {
            // Set the fixed date & time value for this current method.
            var thisDate = DateTime.UtcNow.ToIndonesiaTimeZone();

            // Get old data employee.
            var oldProfile = await this
                .DB
                .Employees
                .Where(Q => Q.EmployeeId == employeeId)
                .FirstOrDefaultAsync();

            if (oldProfile.BlobId != null && newProfile.FileUploaded != null)
            {
                // delete file & blob from employee
                var oldBlob = await this.BlobService.GetBlobById(oldProfile.BlobId.Value);

                await this.BlobService.DeleteBlob(oldBlob.BlobId);

                await this.FileService.DeleteFileAsync(oldBlob.BlobId, oldBlob.Mime);
            }


            if (newProfile.FileUploaded != null && string.IsNullOrEmpty(newProfile.FileUploaded.Base64) == false)
            {
                // Assign content-type with file extension name because of misleading Upload File method's arguments.
                var fileExtension = Path.GetExtension(newProfile.FileUploaded.FileName);
                newProfile.FileUploaded.ContentType = fileExtension.Substring(1, fileExtension.Length - 1);

                var newGuid = await FileService.UploadFileFromBase64(newProfile.FileUploaded);

                oldProfile.BlobId = newGuid;
            }

            if (!String.IsNullOrWhiteSpace(newProfile.NickName))
            {
                oldProfile.Nickname = newProfile.NickName;
            }
            if (!String.IsNullOrWhiteSpace(newProfile.Name))
            {
                oldProfile.EmployeeName = newProfile.Name;
            }
            if (!String.IsNullOrWhiteSpace(newProfile.Phone))
            {
                oldProfile.EmployeePhone = newProfile.Phone;
            }
            if (!String.IsNullOrWhiteSpace(newProfile.Email))
            {
                oldProfile.EmployeeEmail = newProfile.Email;
            }
            if (newProfile.DateOfBirth.HasValue)
            {
                oldProfile.DateOfBirth = newProfile.DateOfBirth;
            }
            if (!String.IsNullOrWhiteSpace(newProfile.BioDescription))
            {
                oldProfile.Description = newProfile.BioDescription;
            }
            if (!String.IsNullOrWhiteSpace(newProfile.Status))
            {
                oldProfile.Status = newProfile.Status;
            }

            oldProfile.UpdatedAt = thisDate;

            await this.DB.SaveChangesAsync();
        }

        /// <summary>
        /// Insert, Update & Delete interests for employee profile.
        /// </summary>
        /// <param name="employeId"></param>
        /// <param name="listInterest"></param>
        /// <returns></returns>
        public async Task InterestProfile(string employeId, List<int> listInterest)
        {
            // Set the fixed date & time value for this current method.
            var thisDate = DateTime.UtcNow.ToIndonesiaTimeZone();

            var dataEmployee = await this
                .DB
                .Employees
                .Where(Q => Q.EmployeeId == employeId)
                .FirstOrDefaultAsync();

            dataEmployee.UpdatedAt = thisDate;

            var dataInterests = await this
                .DB
                .EmployeeInterests
                .Where(Q => Q.EmployeeId == dataEmployee.EmployeeId)
                .ToListAsync();

            // Create new of employee interest type entity object variable.
            // This variable will store interest data based on the user's inputs and will be added to the database.
            var newInterest = new List<EmployeeInterests>();

            // Loop each interests from the user's inputs into list.
            foreach (var interest in listInterest)
            {
                newInterest.Add(new EmployeeInterests
                {
                    EmployeeId = employeId,
                    TopicId = interest
                });
            }

            // Remove old interest from Db.
            if (dataInterests.Count() > 0)
            {
                this.DB.EmployeeInterests.RemoveRange(dataInterests);
            }

            // Insert new interest to Db.
            if (newInterest.Count() > 0)
            {
                this.DB.EmployeeInterests.AddRange(newInterest);
            }

            await this.DB.SaveChangesAsync();
        }

        /// <summary>
        /// Insert, Update & Delete hobbies for employee profile.
        /// </summary>
        /// <param name="employeId"></param>
        /// <param name="listHobby"></param>
        /// <returns></returns>
        public async Task HobbyProfile(string employeId, List<int> listHobby)
        {
            // Set the fixed date & time value for this current method.
            var thisDate = DateTime.UtcNow.ToIndonesiaTimeZone();

            var dataEmployee = await this
                .DB
                .Employees
                .Where(Q => Q.EmployeeId == employeId)
                .FirstOrDefaultAsync();

            dataEmployee.UpdatedAt = thisDate;

            var dataHobbies = await this
                .DB
                .EmployeeHobbyMappings
                .Where(Q => Q.EmployeeId == dataEmployee.EmployeeId)
                .ToListAsync();

            // Create new of employee hobby type entity object variable.
            // This variable will store interest data based on the user's inputs and will be added to the database.
            var newHobbies = new List<EmployeeHobbyMappings>();

            // Loop each interests from the user's inputs into list.
            foreach (var hobby in listHobby)
            {
                newHobbies.Add(new EmployeeHobbyMappings
                {
                    EmployeeId = employeId,
                    HobbyId = hobby
                });
            }

            // Remove old hobbies from Db.
            if (dataHobbies.Count() > 0)
            {
                this.DB.EmployeeHobbyMappings.RemoveRange(dataHobbies);
            }

            // Insert new hobbies to Db.
            if (newHobbies.Count() > 0)
            {
                this.DB.EmployeeHobbyMappings.AddRange(newHobbies);
            }

            await this.DB.SaveChangesAsync();
        }

        public async Task<List<UserSideHobbyProfileFormModel>> GetHobbyEmployee(string employeeId)
        {
            var data = await this
                .DB
                .Hobbies
                .AsNoTracking()
                .Select(Q => new UserSideHobbyProfileFormModel
                {
                    HobbyId = Q.HobbyId,
                    Name = Q.Name,
                    Description = Q.Description,
                    IsChecked =
                    (from ehm in this.DB.EmployeeHobbyMappings.AsNoTracking()
                     where ehm.EmployeeId == employeeId && ehm.HobbyId == Q.HobbyId
                     select ehm
                     ).Any(),
                }).ToListAsync();

            return data;
        }

        public async Task<List<UserSideInterestProfileFormModel>> GetInterestEmployee(string employeeId)
        {
            var data = await this
                .DB
                .Topics
                .AsNoTracking()
                .Select(Q => new UserSideInterestProfileFormModel
                {
                    TopicId = Q.TopicId,
                    TopicDescription = Q.TopicDescription,
                    TopicName = Q.TopicName,
                    IsChecked =
                    (from eim in this.DB.EmployeeInterests.AsNoTracking()
                     where eim.EmployeeId == employeeId && eim.TopicId == Q.TopicId
                     select eim
                     ).Any(),
                }).ToListAsync();

            return data;
        }

        public async Task<ProfileHomePage> GetProfileHomePage(string employeeId)
        {

            var outletData = this.DB.Employees.Where(x => x.EmployeeId == employeeId).FirstOrDefault();

            if (outletData == null)
            {
                return null;
            }


            var data = await (from e in this.DB.Employees.AsNoTracking()

                              join blb in this.DB.Blobs.AsNoTracking()
                              on e.BlobId equals blb.BlobId into ablb
                              from blbn in ablb.DefaultIfEmpty()
                              where e.IsDeleted == false
                              & e.OutletId == outletData.OutletId
                              select new
                              {
                                  e.EmployeeId,
                                  e.EmployeeName,
                                  e.Nickname,
                                  BlobId = e.BlobId.HasValue ? e.BlobId.Value : Guid.Empty,
                                  blbn.Mime,
                                  e.IsCoach,
                                  e.LearningPoint,
                                  e.TeachingPoint,
                                  e.EmployeeExperience
                              }).ToListAsync();

            var queryLevel = await
                DB
                .EmployeeLevels
                .AsNoTracking()
                .Select(Q => Q.MinValue.HasValue ? Q.MinValue.Value : 0)
                .ToListAsync();

            var rankList = data
                .Select(X => new UserSideRankModel
                {
                    EmployeeId = X.EmployeeId,
                    EmployeePoint = X.LearningPoint + X.TeachingPoint,
                    Level = this.RankService.GenerateLevel(queryLevel, X.EmployeeExperience),
                    TotalExperience = X.EmployeeExperience
                })
                .OrderByDescending(Q => Q.TotalExperience)
                .ToList();

            var dataRank = this.RankService.GenerateRank(rankList, employeeId)
                .Where(Q => Q.EmployeeId.ToLower() == employeeId.ToLower())
                .Select(Q => new UserSideRankProfileHome
                {
                    EmployeePoint = Q.EmployeePoint,
                    Level = Q.Level,
                    Rank = Q.Rank,
                    TotalExperience = Q.TotalExperience
                })
                .FirstOrDefault();

            if (dataRank == null)
            {
                dataRank = new UserSideRankProfileHome
                {
                    TotalExperience = 0,
                    Rank = 0,
                    Level = 0,
                    EmployeePoint = 0
                };
            }

            var profile = data
                .Where(Q => Q.EmployeeId.ToLower() == employeeId.ToLower())
                .Select(async Q => new ProfileHomePage
                {
                    EmployeeId = Q.EmployeeId,
                    EmployeeName = Q.EmployeeName,
                    EmployeeNickName = Q.Nickname,
                    IsCoach = Q.IsCoach,
                    ImageUrl = Q.BlobId == Guid.Empty ? null : await this.FileService.GetFileAsync(Q.BlobId.ToString(), Q.Mime),
                    RankUser = dataRank
                })
                .Select(Q => Q.Result)
                .FirstOrDefault();

            return profile;
        }

        public async Task<List<EmployeeLevelModel>> GetEmployeeLevelMinValue(int point)
        {
            var data = await this
                .DB
                .EmployeeLevels
                .Where(Q => Q.MinValue <= point)
                .Select(Q => new EmployeeLevelModel
                {
                    LevelId = Q.EmployeeLevelId,
                    Exp = Q.MinValue == null ? 0 : Q.MinValue
                }).OrderBy(Q => Q.LevelId)
                .LastOrDefaultAsync();

            var data2 = await this
                .DB
                .EmployeeLevels
                .Where(Q => Q.MinValue > point)
                .Select(Q => new EmployeeLevelModel
                {
                    LevelId = Q.EmployeeLevelId,
                    Exp = Q.MinValue == null ? 0 : Q.MinValue
                }).OrderBy(Q => Q.LevelId)
                .FirstOrDefaultAsync();

            var tempList = new List<EmployeeLevelModel>();
            tempList.Add(data);
            tempList.Add(data2);

            return tempList;
        }

        public async Task<List<ProfileRankModel>> GetProfileRank(string employeeId, int teamId)
        {

            var queryRankUser = await (from e in DB.Employees.AsNoTracking()

                                       join b in DB.Blobs.AsNoTracking()
                                       on e.BlobId equals b.BlobId into ab
                                       from b in ab.DefaultIfEmpty()

                                       join epm in DB.EmployeePositionMappings.AsNoTracking()
                                       on e.EmployeeId equals epm.EmployeeId into aepm
                                       from epm in aepm.DefaultIfEmpty()

                                       join p in DB.Positions.AsNoTracking()
                                       on epm.PositionId equals p.PositionId into ap
                                       from p in ap.DefaultIfEmpty()

                                       join o in DB.Outlets.AsNoTracking()
                                       on e.OutletId equals o.OutletId into ao
                                       from o in ao.DefaultIfEmpty()

                                       join td in DB.TeamDetails.AsNoTracking()
                                       on e.EmployeeId equals td.EmployeeId into atd
                                       from td in atd.DefaultIfEmpty()

                                       orderby e.EmployeeExperience descending

                                       where td.TeamId == teamId

                                       select new
                                       {
                                           e.EmployeeId,
                                           e.EmployeeName,
                                           BlobId = e.BlobId == null ? Guid.Empty : e.BlobId,
                                           e.LearningPoint,
                                           e.TeachingPoint,
                                           e.EmployeeExperience,
                                           Mime = string.IsNullOrEmpty(b.Mime) ? "" : b.Mime,
                                           PositionName = string.IsNullOrEmpty(p.PositionName) ? "" : p.PositionName,
                                           OutletName = string.IsNullOrEmpty(o.Name) ? "" : o.Name
                                       })
                                        .ToListAsync();

            var queryLevel = await
                DB
                .EmployeeLevels
                .AsNoTracking()
                .Select(Q => Q.MinValue.HasValue ? Q.MinValue.Value : 0)
                .ToListAsync();

            var dataRankUser = queryRankUser
                .GroupBy(Q => Q.EmployeeId)
                .Select(async X => new ProfileRankModel
                {
                    EmployeeId = X.First().EmployeeId,
                    EmployeeName = X.First().EmployeeName,
                    PositionName = X.First().PositionName,
                    ImageUrl = X.First().BlobId == Guid.Empty ? null : await this.FileService.GetFileAsync(X.First().BlobId.ToString(), X.First().Mime),
                    Name = X.First().OutletName,
                    EmployeePoint = X.First().LearningPoint + X.First().TeachingPoint,
                    Level = GenerateLevel(queryLevel, X.First().EmployeeExperience),
                    TotalExperience = X.First().EmployeeExperience
                })
                .Select(Q => Q.Result)
                .OrderByDescending(Q => Q.TotalExperience)
                .ToList();

            return GenerateRank(dataRankUser, employeeId);

        }

        public int GenerateLevel(List<int> minLvl, int exp)
        {
            var resLvl = 0;
            foreach (var minlevel in minLvl)
            {
                if (exp > minlevel)
                {
                    resLvl++;
                }
                else
                {
                    break;
                }
            }

            return resLvl;
        }

        /// <summary>
        /// Generate rank of employee and customize list of rank
        /// </summary>
        /// <param name="dataRank"></param>
        /// <param name="employeeId"></param>
        /// <returns></returns>
        public List<ProfileRankModel> GenerateRank(List<ProfileRankModel> dataRank, string employeeId)
        {
            var rankUserLoggedIn = 0;

            var rank = 1;
            foreach (var value in dataRank)
            {
                value.Rank = rank;
                rank += 1;

                if (value.EmployeeId.ToLower() == employeeId.ToLower())
                {
                    value.IsLoggedInUser = true;
                    rankUserLoggedIn = value.Rank;
                }
                else
                {
                    value.IsLoggedInUser = false;
                }
            }

            // List Rank of user to show (Rank 1, 2, 3. and skip until spesific employee id)
            var rankUserToShow = new List<ProfileRankModel>();

            // Get Rank 1 2 3
            rankUserToShow.AddRange(dataRank.Take(3));

            if (rankUserLoggedIn > 3)
            {
                // Get rank after spesific emloyeeId
                rankUserToShow
                    .AddRange(dataRank.SkipWhile(Q => Q.EmployeeId.ToLower() != employeeId.ToLower()).ToList());
            }
            else
            {
                rankUserToShow.AddRange(dataRank.Skip(3).ToList());
            }

            return rankUserToShow;

        }
    }
}