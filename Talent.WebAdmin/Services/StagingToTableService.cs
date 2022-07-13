using Microsoft.EntityFrameworkCore;
using Remotion.Linq.Clauses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Talent.Entities.Entities;

namespace Talent.WebAdmin.Services
{
    public class StagingToTableService
    {
        private readonly TalentContext DB;

        public StagingToTableService(TalentContext db)
        {
            this.DB = db;
        }

        /// <summary>
        /// Used to fill table Employees from StagingDealerEmployee
        /// </summary>
        /// <returns></returns>
        public async Task StageDealerEmployeeToEmployees()
        {
            try
            {
                var ids = new List<int>();

                ids = DB.GetDistinctIdFromStagingDealerEmployee()
                        .Select(Q => Q.Id)
                        .ToList();

                var stagings = await (from sde in DB.StagingDealerEmployee
                                      join o in DB.Outlets on sde.OutletId.ToString() equals o.OutletId
                                      where sde.State == "Active" && ids.Contains(sde.Id)
                                      select new Employees
                                      {
                                          EmployeeId = "OC" + o.OutletCode + "EC" + sde.EmployeeId,
                                          EmployeeName = sde.Name,
                                          EmployeeUsername = sde.Name,
                                          OutletId = o.OutletId,
                                          EmployeeEmail = sde.Email,
                                          EmployeePhone = CheckPhoneLength(sde.Phone),
                                          ManpowerStatusType = sde.ManpowerStatusTypeId,
                                          DealerPeopleCategoryCode = sde.ManpowerTypeId,
                                          IsDeleted = false,
                                          Nickname = sde.Name,
                                          Gender = GenerateGender(sde.Sex),
                                          DateOfBirth = sde.DateOfBirth,
                                          Ktp = sde.Ktp,
                                          EmployeeMDMCode = sde.Code,
                                          EmployeeMDMUsername = "OC" + o.OutletCode + "EC" + sde.EmployeeId,
                                          NIP = "OC" + o.OutletCode + "EC" + sde.EmployeeId,
                                      }).ToDictionaryAsync(Q => Q.EmployeeId, Q => Q);
                //}).ToDictionaryAsync(Q => Q.Ktp, Q => Q);

                var stagingIds = stagings.Select(Q => Q.Key).ToList();
                List<String> outlets = await DB.Outlets.Select(x => x.OutletId).ToListAsync();

                var existingData = await DB.Employees.Where(Q => outlets.Contains(Q.OutletId)).ToListAsync();

                var existing = existingData.ToDictionary(Q => Q.EmployeeId, Q => Q);

                //var existing = DB.Employees.Where(Q => Q.OutletId != null).ToDictionary(Q => Q.Ktp, Q => Q);
                var existIds = existing.Select(Q => Q.Key).ToList();

                var toUpdate = existIds.Intersect(stagingIds).ToList();
                var toInsert = stagingIds.Except(existIds).ToList();
                var toDelete = existIds.Except(stagingIds).ToList();

                //var toUpdate = existIds.Where(Q => stagingIds.Contains(Q)).ToList();
                //var toInsert = stagingIds.Where(Q => !existIds.Contains(Q)).ToList();
                //var toDelete = existIds.Where(Q => !stagingIds.Contains(Q)).ToList();

                //Update
                foreach (var id in toUpdate)
                {
                    if (existing[id].IsDataValidation || existing[id].ManpowerStatusType  != "GuestAccount")
                    {
                        existing[id].EmployeeUsername = stagings[id].EmployeeUsername;
                        existing[id].EmployeeName = stagings[id].EmployeeName;
                        existing[id].Nickname = stagings[id].EmployeeName;
                        existing[id].OutletId = stagings[id].OutletId;
                        existing[id].EmployeeEmail = stagings[id].EmployeeEmail;
                        existing[id].EmployeePhone = stagings[id].EmployeePhone;
                        existing[id].ManpowerStatusType = stagings[id].ManpowerStatusType;
                        existing[id].DealerPeopleCategoryCode = stagings[id].DealerPeopleCategoryCode;
                        existing[id].IsDeleted = stagings[id].IsDeleted;
                        existing[id].UpdatedAt = DateTime.Now;
                        existing[id].UpdatedBy = "MDM";
                        existing[id].Ktp = stagings[id].Ktp;
                        existing[id].DateOfBirth = stagings[id].DateOfBirth;
                        existing[id].Gender = stagings[id].Gender;
                        existing[id].EmployeeMDMCode = stagings[id].EmployeeMDMCode;
                        existing[id].EmployeeMDMUsername = stagings[id].EmployeeMDMUsername;
                        existing[id].IsRequestUpgrade = true;
                        existing[id].IsSuspended = false;
                        existing[id].AccountValid = null;
                        existing[id].NIP = stagings[id].EmployeeId;
                        //existing[id].Password = null;
                    }
                }

                //Insert
                var Employees = new List<Employees>();
                foreach (var id in toInsert)
                {
                    var checkIds = DB.Employees.Where(q => q.EmployeeId.ToLower() == id.ToLower()).FirstOrDefault();
                    if (checkIds == null)
                    {
                        var employee = new Employees
                        {
                            EmployeeId = stagings[id].EmployeeId,
                            EmployeeUsername = stagings[id].EmployeeUsername,
                            EmployeeName = stagings[id].EmployeeName,
                            OutletId = stagings[id].OutletId,
                            EmployeeEmail = stagings[id].EmployeeEmail,
                            EmployeePhone = stagings[id].EmployeePhone,
                            ManpowerStatusType = stagings[id].ManpowerStatusType,
                            DealerPeopleCategoryCode = stagings[id].DealerPeopleCategoryCode,
                            IsDeleted = stagings[id].IsDeleted,
                            Nickname = stagings[id].Nickname,
                            Gender = stagings[id].Gender,
                            DateOfBirth = stagings[id].DateOfBirth,
                            Ktp = stagings[id].Ktp,
                            EmployeeMDMUsername = stagings[id].EmployeeMDMUsername,
                            EmployeeMDMCode = stagings[id].EmployeeMDMCode,
                            IsRequestUpgrade = true,
                            IsSuspended = false,
                            AccountValid = null,
                            Password = null,
                            NIP = stagings[id].EmployeeId,
                        };
                        employee.IsCoach = employee.IsTeamLeader = false;
                        employee.CreatedAt = employee.UpdatedAt = DateTime.Now;
                        employee.CreatedBy = employee.UpdatedBy = "MDM";
                        employee.EmployeeExperience = employee.LearningPoint = employee.TeachingPoint = 0;

                        Employees.Add(employee); 
                    }
                }
                DB.Employees.AddRange(Employees);

                //string Ids = "";
                //foreach (var item in Employees)
                //{
                //    Ids = Ids + "," + item.EmployeeId;
                //}

                //var data = Ids;

                //Delete
                foreach (var id in toDelete)
                {
                    if (existing[id].ManpowerStatusType != "GuestAccount")
                    {
                        existing[id].IsDeleted = true;
                    }
                }



                await DB.SaveChangesAsync();
            }
            catch (Exception x)
            {
                Console.WriteLine(x.Message);
            }

        }

        public async Task StageDealerEmployeesToTeams()
        {
            try
            {
                // insert new teams
                var existingTeamLeaders  = await DB.Teams.Select(x=> x.TeamLeaderId).ToListAsync();
                var stagingTeamLeaders = await DB.StagingDealerEmployee.Where(x => !existingTeamLeaders.Contains(x.ParentCode) && x.ParentCode != null).Select(x=> x.ParentCode).Distinct().ToListAsync();
                var listOfTeams =  new List<Teams>();
                foreach (var item in stagingTeamLeaders)
                {
                    var team = new Teams{
                        TeamLeaderId = item,
                    };
                    listOfTeams.Add(team);
                }
                DB.Teams.AddRange(listOfTeams);
                await DB.SaveChangesAsync();

                var listOfEmployees = (from e in DB.Employees
                                       join t in DB.Teams on e.EmployeeId equals t.TeamLeaderId
                                       select e).ToList();

                foreach (var employee in listOfEmployees)
                {
                        employee.IsTeamLeader = true;
                        await DB.SaveChangesAsync();
                }
            }
            catch (Exception x)
            {
                Console.WriteLine(x.Message);
            }

        }

        public async Task ClearDataTeams(){
            try{
                await DB.Database.ExecuteSqlCommandAsync("TRUNCATE TABLE TeamDetails");
                await DB.Database.ExecuteSqlCommandAsync("TRUNCATE TABLE Teams");
            } catch(Exception x){
                Console.WriteLine(x.Message);
            }
        }

        public async Task StageDealerEmployeesToTeamMembers()
        {
            try
            {
                // insert new team member

                var stagingTeamMembers = await (  
                                            from e in DB.StagingDealerEmployee 
                                            join o in DB.Outlets on e.OutletId.ToString() equals o.OutletId
                                            join t in DB.Teams on e.ParentCode equals t.TeamLeaderId
                                            where e.State.ToLower() == "active"
                                            select new {
                                                EmployeeId = "OC" + o.OutletCode + "EC" + e.EmployeeId,
                                                TeamId = t.TeamId,
                                           }).ToListAsync();

                var listOfTeams = new List<TeamDetails>();
                foreach (var item in stagingTeamMembers)
                {
                    var existEmployee = (from e in DB.Employees
                                         where e.EmployeeId == item.EmployeeId && e.IsDeleted == false
                                         select e).FirstOrDefault();

                    if (existEmployee != null)
                    {
                        var memberData = new TeamDetails
                        {
                            TeamId = item.TeamId,
                            EmployeeId = item.EmployeeId,
                        };

                        listOfTeams.Add(memberData);
                    }            
                }
                DB.TeamDetails.AddRange(listOfTeams);
                await DB.SaveChangesAsync();
            }
            catch (Exception x)
            {
                Console.WriteLine(x.Message);
            }

        }

        /// <summary>
        /// check if phone length > 50 then cut 
        /// </summary>
        /// <param name="phone"></param>
        /// <returns></returns>
        private string CheckPhoneLength(string phone)
        {
            if (phone?.Length > 50 && !string.IsNullOrEmpty(phone))
            {
                phone = phone.Substring(0, 49);
            }

            return phone;
        }

        /// <summary>
        /// Used to fill table Employees from StagingUser
        /// </summary>
        /// <returns></returns>
        public async Task StageUserToEmployees()
        {
            var stagings = await (from su in DB.StagingUser
                                  where su.State == "Active"
                                  select new Employees
                                  {
                                      EmployeeId = su.NoReg,
                                      EmployeeName = su.Name,
                                      EmployeeUsername = su.Name,
                                      EmployeeEmail = su.Email,
                                      IsDeleted = su.State.Contains("Active") ? false : true,
                                      Nickname = su.Name,
                                      EmployeeMDMCode = su.Code,
                                      EmployeeMDMUsername = su.Email,
                                  }).ToDictionaryAsync(Q => Q.EmployeeId, Q => Q);
            var stagingIds = stagings.Select(Q => Q.Key).ToList();

            var existing = DB.Employees.Where(Q => Q.OutletId == null).ToDictionary(Q => Q.EmployeeId, Q => Q);
            var existIds = existing.Select(Q => Q.Key).ToList();

            var toUpdate = existIds.Intersect(stagingIds).ToList();
            var toInsert = stagingIds.Except(existIds).ToList();
            var toDelete = existIds.Except(stagingIds).ToList();

            //Update
            foreach (var id in toUpdate)
            {
                existing[id].EmployeeUsername = stagings[id].EmployeeUsername;
                existing[id].EmployeeName = stagings[id].EmployeeName;
                existing[id].Nickname = stagings[id].EmployeeName;
                existing[id].EmployeeEmail = stagings[id].EmployeeEmail;
                existing[id].IsDeleted = stagings[id].IsDeleted;
                existing[id].UpdatedAt = DateTime.Now;
                existing[id].UpdatedBy = "MDM";
                existing[id].EmployeeMDMCode = stagings[id].EmployeeMDMCode;
                existing[id].EmployeeMDMUsername = stagings[id].EmployeeMDMUsername;
                existing[id].ManpowerStatusType = "Permanent";
            }

            //Insert
            var Employees = new List<Employees>();
            foreach (var id in toInsert)
            {
                var employee = new Employees
                {
                    EmployeeId = stagings[id].EmployeeId,
                    EmployeeUsername = stagings[id].EmployeeUsername,
                    EmployeeName = stagings[id].EmployeeName,
                    EmployeeEmail = stagings[id].EmployeeEmail,
                    IsDeleted = stagings[id].IsDeleted,
                    Nickname = stagings[id].Nickname,
                    EmployeeMDMCode = stagings[id].EmployeeMDMCode,
                    EmployeeMDMUsername = stagings[id].EmployeeMDMUsername,
                    ManpowerStatusType = "Permanent",
                };
                employee.IsCoach = employee.IsTeamLeader = false;
                employee.CreatedAt = employee.UpdatedAt = DateTime.Now;
                employee.CreatedBy = employee.UpdatedBy = "MDM";
                employee.EmployeeExperience = employee.LearningPoint = employee.TeachingPoint = 0;

                Employees.Add(employee);

            }
            DB.Employees.AddRange(Employees);

            //Delete
            foreach (var id in toDelete)
            {
                existing[id].IsDeleted = true;
            }

            await DB.SaveChangesAsync();
        }

        /// <summary>
        /// Used to fill Positions from StagingOrganizationObject
        /// </summary>
        /// <returns></returns>
        public async Task StageOrganizationObjectToPosition()
        {
            var existing = DB.Positions.ToDictionary(Q => Q.PositionCode, Q => Q);
            var stagings = (from soo in DB.StagingOrganizationObject
                            where soo.ObjectType == "C"
                            group new
                            {
                                soo.ObjectId,
                                soo.ObjectText,
                                soo.ObjectDescription,
                            } by new
                            {
                                soo.ObjectId,
                                soo.ObjectText,
                                soo.ObjectDescription,
                            } into g
                            select new Positions
                            {
                                PositionCode = g.Key.ObjectId,
                                PositionName = g.Key.ObjectText,
                                PositionDescription = g.Key.ObjectDescription ?? "-",
                            }).ToDictionary(Q => Q.PositionCode, Q => Q);

            var existIds = existing.Select(Q => Q.Key).ToList();
            var stagingIds = stagings.Select(Q => Q.Key).ToList();

            var toUpdate = existIds.Intersect(stagingIds).ToList();
            var toInsert = stagingIds.Except(existIds).ToList();
            var toDelete = existIds.Except(stagingIds).ToList();

            //Update
            foreach (var id in toUpdate)
            {
                existing[id].PositionName = stagings[id].PositionName;
                existing[id].PositionDescription = stagings[id].PositionDescription;
            }

            //Insert
            var positions = new List<Positions>();
            foreach (var id in toInsert)
            {
                var position = new Positions
                {
                    PositionCode = stagings[id].PositionCode,
                    PositionName = stagings[id].PositionName,
                    PositionDescription = stagings[id].PositionDescription,
                    IsTamPeople = true
                };

                positions.Add(position);
            }
            DB.Positions.AddRange(positions);

            await DB.SaveChangesAsync();
        }

        /// <summary>
        /// Used to fill Positions from StagingManpowerPositionType
        /// StagingManpowerPositionType is Positions for DealerEmployee
        /// </summary>
        /// <returns></returns>
        public async Task StageManpowerPositionTypeToPosition()
        {
            var existing = DB.Positions.ToDictionary(Q => Q.PositionCode, Q => Q);
            var stagings = DB.StagingManpowerPositionType.ToDictionary(Q => Q.Code, Q => Q);

            var existIds = existing.Select(Q => Q.Key).ToList();
            var stagingIds = stagings.Select(Q => Q.Key).ToList();

            var toUpdate = existIds.Intersect(stagingIds).ToList();
            var toInsert = stagingIds.Except(existIds).ToList();
            var toDelete = existIds.Except(stagingIds).ToList();

            //Update
            foreach (var id in toUpdate)
            {
                existing[id].PositionName = stagings[id].Name;
                existing[id].PositionDescription = stagings[id].Name;
            }

            //Insert
            var positions = new List<Positions>();
            foreach (var id in toInsert)
            {
                var position = new Positions
                {
                    PositionCode = stagings[id].Code,
                    PositionName = stagings[id].Name,
                    PositionDescription = stagings[id].Name,
                    IsTamPeople = false
                };

                positions.Add(position);
            }
            DB.Positions.AddRange(positions);

            await DB.SaveChangesAsync();
        }

        /// <summary>
        /// Used to fill EmployeePositionMappings of DealerEmployee and User
        /// </summary>
        /// <returns></returns>
        public async Task StageEmployeeToEmployeePositionMapping()
        {
            DB.RemoveRange(DB.EmployeePositionMappings.Include(Q => Q.Employee).Where(Q => Q.EmployeeId != "italent001" || Q.Employee.ManpowerStatusType != "GuestAccount").Select(Q => Q));

            var ids = new List<int>();

            ids = DB.GetDistinctIdFromStagingDealerEmployee()
                    .Select(Q => Q.Id)
                    .ToList();

            var stagingsDealerEmployee = await (from sde in DB.StagingDealerEmployee
                                                join o in DB.Outlets on sde.OutletId.ToString() equals o.OutletId
                                                join p in DB.Positions on sde.LastManpowerPositionTypeId equals p.PositionCode
                                                join e in DB.Employees on ("OC" + o.OutletCode + "EC" + sde.EmployeeId) equals e.EmployeeId
                                                where sde.State == "Active" && ids.Contains(sde.Id)
                                                select new EmployeePositionMappings
                                                {
                                                    EmployeeId = e.EmployeeId,
                                                    PositionId = p.PositionId,
                                                    CreatedAt = DateTime.Now,
                                                    UpdatedAt = DateTime.Now,
                                                }).ToListAsync();

            var stagingsUser = await (from su in DB.StagingUser
                                      join saos in DB.StagingActualOrganizationStructure on su.NoReg equals saos.NoReg
                                      join soo in DB.StagingOrganizationObject on saos.JobCode equals soo.ObjectId
                                      join p in DB.Positions on saos.JobCode equals p.PositionCode
                                      join e in DB.Employees on su.NoReg equals e.EmployeeId
                                      where soo.ObjectType == "C" && saos.State == "Active"
                                      group new
                                      {
                                          e.EmployeeId,
                                          p.PositionId,
                                      } by new
                                      {
                                          soo.ObjectId,
                                          e.EmployeeId,
                                          p.PositionId,
                                      } into g
                                      select new EmployeePositionMappings
                                      {
                                          EmployeeId = g.Key.EmployeeId,
                                          PositionId = g.Key.PositionId,
                                          CreatedAt = DateTime.Now,
                                          UpdatedAt = DateTime.Now,
                                      }).ToListAsync();

            var stagingsCombined = new List<EmployeePositionMappings>(stagingsDealerEmployee.Count + stagingsUser.Count);
            stagingsCombined.AddRange(stagingsDealerEmployee);
            stagingsCombined.AddRange(stagingsUser);

            DB.EmployeePositionMappings.AddRange(stagingsCombined);

            await DB.SaveChangesAsync();
        }

        /// <summary>
        /// Used to fill DealerPeopleCategories from StagingManpowerType
        /// </summary>
        /// <returns></returns>
        public async Task StageManpowerTypeToDealerPeopleCategories()
        {
            var existing = DB.DealerPeopleCategories.ToDictionary(Q => Q.DealerPeopleCategoryCode, Q => Q);
            var stagings = DB.StagingManpowerType.ToDictionary(Q => Q.Code, Q => Q);

            var existIds = existing.Select(Q => Q.Key).ToList();
            var stagingIds = stagings.Select(Q => Q.Key).ToList();

            var toUpdate = existIds.Intersect(stagingIds).ToList();
            var toInsert = stagingIds.Except(existIds).ToList();
            var toDelete = existIds.Except(stagingIds).ToList();

            //Update
            foreach (var id in toUpdate)
            {
                existing[id].Name = stagings[id].Name;
            }

            //Insert
            var dealerPeopleCategories = new List<DealerPeopleCategories>();
            foreach (var id in toInsert)
            {
                var dealerPeopleCategory = new DealerPeopleCategories
                {
                    DealerPeopleCategoryCode = stagings[id].Code,
                    Name = stagings[id].Name
                };

                dealerPeopleCategories.Add(dealerPeopleCategory);
            }
            DB.DealerPeopleCategories.AddRange(dealerPeopleCategories);

            await DB.SaveChangesAsync();
        }

        /// <summary>
        /// Used to fill Outlets from StagingOutlet
        /// </summary>
        /// <returns></returns>
        public async Task StageToOutlet()
        {
            var existing = DB.Outlets.ToDictionary(Q => Q.OutletId, Q => Q);
            var oc = (from so in DB.StagingOutlet
                      join sr in DB.StagingRegion on so.KabupatenId.ToString() equals sr.Code
                      select so.OutletCode).ToList();
            var oupgraded = (from so in DB.StagingOutlet
                             join c in DB.Cities on so.RegionCode.ToString() equals c.CityId
                             join p in DB.Provinces on c.ParentCode equals p.ProvinceId
                             join sdc in DB.StagingDealerCompany on so.DealerCompanyId.ToString() equals sdc.Code
                             join d in DB.Dealers on sdc.DealerGroupId.ToString() equals d.DealerId
                             join a in DB.Areas on so.TamAreaId.ToString() equals a.AreaId
                             join cf in DB.Cfams on so.TamAreaAfterSalesId.ToString() equals cf.Cfamid into cf2
                             from cf in cf2.DefaultIfEmpty()
                             where !oc.Contains(so.OutletCode)
                             select new Outlets
                             {
                                 OutletId = so.Code,
                                 DealerId = d.DealerId, //DealerId
                                 Cfamid = cf.Cfamid, //CFAMId
                                 AreaId = a.AreaId, //AreaId
                                 ProvinceId = p.ProvinceId,
                                 CityId = c.CityId,
                                 OutletCode = so.OutletCode,
                                 Name = so.Name,
                                 Phone = so.PhoneNumber,
                                 IsBp = so.OutletFunctionId.Contains("BP"),
                                 IsGr = so.OutletFunctionId.Contains("SP"),
                                 IsSales = so.OutletFunctionId.Contains("V"),
                             }).ToList();

            var o = (from so in DB.StagingOutlet
                     join c in DB.Cities on so.KabupatenId.ToString() equals c.CityId
                     join p in DB.Provinces on c.ParentCode equals p.ProvinceId
                     join sdc in DB.StagingDealerCompany on so.DealerCompanyId.ToString() equals sdc.Code
                     join d in DB.Dealers on sdc.DealerGroupId.ToString() equals d.DealerId
                     join a in DB.Areas on so.TamAreaId.ToString() equals a.AreaId
                     join cf in DB.Cfams on so.TamAreaAfterSalesId.ToString() equals cf.Cfamid into cf2
                     from cf in cf2.DefaultIfEmpty()
                     select new Outlets
                     {
                         OutletId = so.Code,
                         DealerId = d.DealerId, //DealerId
                         Cfamid = cf.Cfamid, //CFAMId
                         AreaId = a.AreaId, //AreaId
                         ProvinceId = p.ProvinceId,
                         CityId = c.CityId,
                         OutletCode = so.OutletCode,
                         Name = so.Name,
                         Phone = so.PhoneNumber,
                         IsBp = so.OutletFunctionId.Contains("BP"),
                         IsGr = so.OutletFunctionId.Contains("SP"),
                         IsSales = so.OutletFunctionId.Contains("V"),
                     }).ToList();

            var stagingsList = new List<Outlets>(o.Count + oupgraded.Count);
            stagingsList.AddRange(oupgraded);
            stagingsList.AddRange(o);
            var stagings = stagingsList.ToDictionary(Q => Q.OutletId, Q => Q);

            var existIds = existing.Select(Q => Q.Key).ToList();
            var stagingIds = stagings.Select(Q => Q.Key).ToList();

            var toUpdate = existIds.Intersect(stagingIds).ToList();
            var toInsert = stagingIds.Except(existIds).ToList();
            var toDelete = existIds.Except(stagingIds).ToList();

            //Update
            foreach (var id in toUpdate)
            {
                existing[id].DealerId = stagings[id].DealerId;
                existing[id].Cfamid = stagings[id].Cfamid;
                existing[id].AreaId = stagings[id].AreaId;
                existing[id].ProvinceId = stagings[id].ProvinceId;
                existing[id].CityId = stagings[id].CityId;
                existing[id].OutletCode = stagings[id].OutletCode;
                existing[id].Name = stagings[id].Name;
                existing[id].Phone = stagings[id].Phone;
                existing[id].IsBp = stagings[id].IsBp;
                existing[id].IsGr = stagings[id].IsGr;
                existing[id].IsSales = stagings[id].IsSales;
            }

            //Insert
            var outlets = new List<Outlets>();
            foreach (var id in toInsert)
            {
                var outlet = new Outlets
                {
                    OutletId = stagings[id].OutletId,
                    DealerId = stagings[id].DealerId,
                    Cfamid = stagings[id].Cfamid,
                    AreaId = stagings[id].AreaId,
                    ProvinceId = stagings[id].ProvinceId,
                    CityId = stagings[id].CityId,
                    OutletCode = stagings[id].OutletCode,
                    Name = stagings[id].Name,
                    Phone = stagings[id].Phone,
                    IsBp = stagings[id].IsBp,
                    IsGr = stagings[id].IsGr,
                    IsSales = stagings[id].IsSales,
                    CreatedAt = DateTime.Now,
                    CreatedBy = "MDM"
                };

                outlets.Add(outlet);
            }
            DB.Outlets.AddRange(outlets);

            await DB.SaveChangesAsync();
        }

        /// <summary>
        /// Used to fill Provinces from StagingRegion
        /// </summary>
        /// <returns></returns>
        public async Task StageToProvince()
        {
            var existing = DB.Provinces.ToDictionary(Q => Q.ProvinceId, Q => Q);
            var stagings = DB.StagingRegion.Where(Q => Q.RegionType == "PROVINSI").ToDictionary(Q => Q.Code, Q => Q);

            var existIds = existing.Select(Q => Q.Key).ToList();
            var stagingIds = stagings.Select(Q => Q.Key).ToList();

            var toUpdate = existIds.Intersect(stagingIds).ToList();
            var toInsert = stagingIds.Except(existIds).ToList();
            var toDelete = existIds.Except(stagingIds).ToList();

            //Update
            foreach (var id in toUpdate)
            {
                existing[id].ProvinceName = stagings[id].Name;
            }

            //Insert
            var newProvinces = new List<Provinces>();
            foreach (var id in toInsert)
            {
                var newProvince = new Provinces
                {
                    ProvinceId = stagings[id].Code,
                    ProvinceName = stagings[id].Name,
                    ParentCode = Convert.ToInt32(stagings[id].ParentCode).ToString()
                };

                newProvinces.Add(newProvince);
            }
            DB.Provinces.AddRange(newProvinces);

            await DB.SaveChangesAsync();
        }

        /// <summary>
        /// Used to fill Cities from StagingRegion
        /// </summary>
        /// <returns></returns>
        public async Task StageToCity()
        {
            var existing = DB.Cities.ToDictionary(Q => Q.CityId, Q => Q);
            var stagings = DB.StagingRegion.Where(Q => Q.RegionType == "KABUPATEN").ToDictionary(Q => Q.Code, Q => Q);

            var existIds = existing.Select(Q => Q.Key).ToList();
            var stagingIds = stagings.Select(Q => Q.Key).ToList();

            var toUpdate = existIds.Intersect(stagingIds).ToList();
            var toInsert = stagingIds.Except(existIds).ToList();
            var toDelete = existIds.Except(stagingIds).ToList();

            //Update
            foreach (var id in toUpdate)
            {
                existing[id].CityName = stagings[id].Name;
            }

            //Insert
            var newCities = new List<Cities>();
            foreach (var id in toInsert)
            {
                var newCity = new Cities
                {
                    CityId = stagings[id].Code,
                    CityName = stagings[id].Name,
                    ParentCode = Convert.ToInt32(stagings[id].ParentCode).ToString()
                };

                newCities.Add(newCity);
            }
            DB.Cities.AddRange(newCities);

            await DB.SaveChangesAsync();
        }

        /// <summary>
        /// Used to fill Areas from StagingSalesArea
        /// </summary>
        /// <returns></returns>
        public async Task StageToSalesArea()
        {
            var existing = DB.Areas.ToDictionary(Q => Q.AreaId, Q => Q);
            var stagings = DB.StagingSalesArea
            .Where(Q => Q.State.ToLower().Equals("active"))
            .ToDictionary(Q => Q.Code, Q => Q);

            var existIds = existing.Select(Q => Q.Key).ToList();
            var stagingIds = stagings.Select(Q => Q.Key).ToList();

            var toUpdate = existIds.Intersect(stagingIds).ToList();
            var toInsert = stagingIds.Except(existIds).ToList();
            var toDelete = existIds.Except(stagingIds).ToList();

            //Update
            foreach (var id in toUpdate)
            {
                existing[id].Name = stagings[id].Name;
            }

            //Insert
            var newAreas = new List<Areas>();
            foreach (var id in toInsert)
            {
                var newArea = new Areas
                {
                    AreaId = stagings[id].Code,
                    Name = stagings[id].Name
                };

                newAreas.Add(newArea);
            }
            var listToDelete = new List<Areas>();
            foreach (var id in toDelete)
            {
                listToDelete.Add(new Areas
                {
                    AreaId = existing[id].AreaId,
                    Name = existing[id].Name
                });
            }
            DB.Areas.RemoveRange(listToDelete);
            DB.Areas.AddRange(newAreas);

            await DB.SaveChangesAsync();
        }

        /// <summary>
        /// Used to fill CFAMs from StagingAfterSalesArea
        /// </summary>
        /// <returns></returns>
        public async Task StageToAfterSalesArea()
        {
            var existing = DB.Cfams.ToDictionary(Q => Q.Cfamid, Q => Q);
            var stagings = DB.StagingAfterSalesArea
                            .Where(Q => Q.State.ToLower()
                            .Equals("active"))
                            .ToDictionary(Q => Q.Code, Q => Q);

            var existIds = existing.Select(Q => Q.Key).ToList();
            var stagingIds = stagings.Select(Q => Q.Key).ToList();

            var toUpdate = existIds.Intersect(stagingIds).ToList();
            var toInsert = stagingIds.Except(existIds).ToList();
            var toDelete = existIds.Except(stagingIds).ToList();

            //Update
            foreach (var id in toUpdate)
            {
                existing[id].Cfamname = stagings[id].Name;
            }

            //Insert
            var newCFAMs = new List<Cfams>();
            foreach (var id in toInsert)
            {
                var newCFAM = new Cfams
                {
                    Cfamid = stagings[id].Code,
                    Cfamname = stagings[id].Name
                };

                newCFAMs.Add(newCFAM);
            }
            var listToDelete = new List<Cfams>();
            foreach (var id in toDelete)
            {
                listToDelete.Add(new Cfams
                {
                    Cfamid = existing[id].Cfamid,
                    Cfamname = existing[id].Cfamname,
                });
            }
            DB.Cfams.RemoveRange(listToDelete);
            DB.Cfams.AddRange(newCFAMs);

            await DB.SaveChangesAsync();
        }

        /// <summary>
        /// Used to fill Dealers from StagingDealerGroup
        /// </summary>
        /// <returns></returns>
        public async Task StageToDealer()
        {
            var existing = DB.Dealers.ToDictionary(Q => Q.DealerId, Q => Q);
            var stagings = (from sdg in DB.StagingDealerGroup
                            select new Dealers
                            {
                                DealerId = sdg.Code,
                                DealerName = sdg.Name
                            }).ToDictionary(Q => Q.DealerId, Q => Q);

            var existIds = existing.Select(Q => Q.Key).ToList();
            var stagingIds = stagings.Select(Q => Q.Key).ToList();

            var toUpdate = existIds.Intersect(stagingIds).ToList();
            var toInsert = stagingIds.Except(existIds).ToList();
            var toDelete = existIds.Except(stagingIds).ToList();

            //Update
            foreach (var id in toUpdate)
            {
                existing[id].DealerName = stagings[id].DealerName;
            }

            //Insert
            var newDealers = new List<Dealers>();
            foreach (var id in toInsert)
            {
                var newDealer = new Dealers
                {
                    DealerId = stagings[id].DealerId,
                    DealerName = stagings[id].DealerName,
                    CreatedAt = DateTime.Now,
                    CreatedBy = "MDM"
                };

                newDealers.Add(newDealer);
            }
            DB.Dealers.AddRange(newDealers);

            ////Delete
            //var removeDealers = new List<Dealers>();
            //foreach (var id in toDelete)
            //{
            //    var dealer = new Dealers();

            //    dealer = existing[id];
            //    removeDealers.Add(dealer);
            //}
            //DB.Dealers.RemoveRange(removeDealers);

            await DB.SaveChangesAsync();
        }
        /// <summary>
        /// Used to fill TAMEmployeeStructure from StagingActualOrganizationStrucure
        /// </summary>
        /// <returns></returns>
        public async Task StageToTamEmployeeStructure()
        {
            var stagings = await (from aos in DB.StagingActualOrganizationStructure
                                  where aos.State == "Active" && !string.IsNullOrEmpty(aos.NoReg)
                                  select new TamemployeeStructure
                                  {
                                      Code = aos.Code,
                                      NoReg = aos.NoReg,
                                      Chief = aos.Chief,
                                      DepthLevel = aos.DepthLevel,
                                      EmployeeGroup = aos.EmployeeGroup,
                                      EmployeeGroupText = aos.EmployeeGroupText,
                                      EmployeeSubgroup = aos.EmployeeSubgroup,
                                      EmployeeSubgroupText = aos.EmployeeSubgroupText,
                                      Id = aos.Id,
                                      JobCode = aos.JobCode,
                                      JobName = aos.JobName,
                                      LastChgDateTime = aos.LastChgDateTime,
                                      Name = aos.Name,
                                      OrgCode = aos.OrgCode,
                                      OrgLevel = aos.OrgLevel,
                                      OrgName = aos.OrgName,
                                      ParentOrgCode = aos.ParentOrgCode,
                                      Period = aos.Period,
                                      PersonalArea = aos.PersonalArea,
                                      PersonalSubarea = aos.PersonalSubarea,
                                      PostCode = aos.PostCode,
                                      PostName = aos.PostName,
                                      Staffing = aos.Staffing,
                                      State = aos.State,
                                      Structure = aos.Structure,
                                      Vacant = aos.Vacant,
                                      WorkContract = aos.WorkContract,
                                      WorkContractText = aos.WorkContractText,
                                      TalentLevel = GetTalentLevel(aos.OrgLevel),
                                  }).ToDictionaryAsync(Q => Q.Code, Q => Q);

            var stagingCodes = stagings.Select(Q => Q.Key).ToList();
            var existing = DB.TamemployeeStructure.ToDictionary(Q => Q.Code, Q => Q);
            var existingCodes = existing.Select(Q => Q.Key).ToList();

            var toUpdate = existingCodes.Intersect(stagingCodes).ToList();
            var toInsert = stagingCodes.Except(existingCodes).ToList();
            var toDelete = existingCodes.Except(stagingCodes).ToList();

            //Update
            foreach (var code in toUpdate)
            {
                existing[code].NoReg = stagings[code].NoReg;
                existing[code].Chief = stagings[code].Chief;
                existing[code].DepthLevel = stagings[code].DepthLevel;
                existing[code].EmployeeGroup = stagings[code].EmployeeGroup;
                existing[code].EmployeeGroupText = stagings[code].EmployeeGroupText;
                existing[code].EmployeeSubgroup = stagings[code].EmployeeSubgroup;
                existing[code].EmployeeSubgroupText = stagings[code].EmployeeSubgroupText;
                existing[code].Id = stagings[code].Id;
                existing[code].JobCode = stagings[code].JobCode;
                existing[code].JobName = stagings[code].JobName;
                existing[code].LastChgDateTime = stagings[code].LastChgDateTime;
                existing[code].Name = stagings[code].Name;
                existing[code].OrgCode = stagings[code].OrgCode;
                existing[code].OrgLevel = stagings[code].OrgLevel;
                existing[code].OrgName = stagings[code].OrgName;
                existing[code].ParentOrgCode = stagings[code].ParentOrgCode;
                existing[code].Period = stagings[code].Period;
                existing[code].PersonalArea = stagings[code].PersonalArea;
                existing[code].PersonalSubarea = stagings[code].PersonalSubarea;
                existing[code].PostCode = stagings[code].PostCode;
                existing[code].PostName = stagings[code].PostName;
                existing[code].Staffing = stagings[code].Staffing;
                existing[code].State = stagings[code].State;
                existing[code].Structure = stagings[code].Structure;
                existing[code].Vacant = stagings[code].Vacant;
                existing[code].WorkContract = stagings[code].WorkContract;
                existing[code].WorkContractText = stagings[code].WorkContractText;
                existing[code].TalentLevel = stagings[code].TalentLevel;
            }

            //Insert
            var tamemployeeStructures = new List<TamemployeeStructure>();
            foreach (var code in toInsert)
            {
                var tamemployeeStructure = new TamemployeeStructure
                {
                    Code = stagings[code].Code,
                    NoReg = stagings[code].NoReg,
                    Chief = stagings[code].Chief,
                    DepthLevel = stagings[code].DepthLevel,
                    EmployeeGroup = stagings[code].EmployeeGroup,
                    EmployeeGroupText = stagings[code].EmployeeGroupText,
                    EmployeeSubgroup = stagings[code].EmployeeSubgroup,
                    EmployeeSubgroupText = stagings[code].EmployeeSubgroupText,
                    Id = stagings[code].Id,
                    JobCode = stagings[code].JobCode,
                    JobName = stagings[code].JobName,
                    LastChgDateTime = stagings[code].LastChgDateTime,
                    Name = stagings[code].Name,
                    OrgCode = stagings[code].OrgCode,
                    OrgLevel = stagings[code].OrgLevel,
                    OrgName = stagings[code].OrgName,
                    ParentOrgCode = stagings[code].ParentOrgCode,
                    Period = stagings[code].Period,
                    PersonalArea = stagings[code].PersonalArea,
                    PersonalSubarea = stagings[code].PersonalSubarea,
                    PostCode = stagings[code].PostCode,
                    PostName = stagings[code].PostName,
                    Staffing = stagings[code].Staffing,
                    State = stagings[code].State,
                    Structure = stagings[code].Structure,
                    Vacant = stagings[code].Vacant,
                    WorkContract = stagings[code].WorkContract,
                    WorkContractText = stagings[code].WorkContractText,
                    TalentLevel = stagings[code].TalentLevel,
                };

                tamemployeeStructures.Add(tamemployeeStructure);
            }
            DB.TamemployeeStructure.AddRange(tamemployeeStructures);

            //Delete
            var toBeDeleted = existing.Where(Q => toDelete.Contains(Q.Key)).Select(Q => Q.Value).ToList();
            DB.TamemployeeStructure.RemoveRange(toBeDeleted);
            await DB.SaveChangesAsync();
        }

        private int GetTalentLevel(decimal? orgLevel)
        {
            if (orgLevel >= 9) return 0;
            if (orgLevel == 8) return 1;
            if (orgLevel == 7) return 2;
            if (orgLevel < 7 && orgLevel >= 4) return 3;
            return 4;
        }
        public string GenerateGender(string sexFromMdm)
        {
            if (string.IsNullOrEmpty(sexFromMdm))
            {
                return null;
            }
            return sexFromMdm.ToLower() == "m" ? "Male" : "Female";

        }

        /// <summary>
        /// Stage to all tables with order
        /// </summary>
        /// <returns></returns>
        public async Task StageToAll()
        {
            // Outlet
            await StageToCity();
            await StageToProvince();
            await StageToSalesArea();
            await StageToAfterSalesArea();
            await StageToDealer();
            await StageToOutlet();
            // Dealer People Categories
            await StageManpowerTypeToDealerPeopleCategories();
            // Dealer Employee
            await StageDealerEmployeeToEmployees();
            // User
            await StageUserToEmployees();
            // Manpower Position Type
            await StageManpowerPositionTypeToPosition();
            // Organization Object
            await StageOrganizationObjectToPosition();
            // Employee Position Mapping
            await StageEmployeeToEmployeePositionMapping();
            // ActualOrganizationStructure to TAMEmployeeStructure
            await StageToTamEmployeeStructure();
            // Clear Team Data 
            await ClearDataTeams();
            // Team
            await StageDealerEmployeesToTeams();
            // Team Detail 
            await StageDealerEmployeesToTeamMembers();
        }

    }

}

