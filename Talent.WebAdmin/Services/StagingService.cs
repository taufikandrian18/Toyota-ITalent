using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Talent.Entities.Entities;
using Talent.WebAdmin.Models;

namespace Talent.WebAdmin.Services
{
    public class StagingService
    {
        private readonly TalentContext DB;
        private readonly StagingToTableService StagingToTable;

        public StagingService(TalentContext db, StagingToTableService stagingToTable)
        {
            this.DB = db;
            this.StagingToTable = stagingToTable;
        }

        /// <summary>
        /// insert all User data from MDM to StagingUser
        /// </summary>
        /// <param name="models"></param>
        /// <returns></returns>
        public async Task StageUser(List<StagingUser> models)
        {
            var existing = DB.StagingUser.ToDictionary(Q => Q.Code, Q => Q);
            var existingIds = existing.Select(Q => Q.Key).ToList();

            var mdm = models.ToDictionary(Q => Q.Code, Q => Q);
            var mdmIds = mdm.Select(Q => Q.Key).ToList();

            var toUpdate = existingIds.Intersect(mdmIds).ToList();
            var toInsert = mdmIds.Except(existingIds).ToList();
            var toDelete = existingIds.Except(mdmIds).ToList();

            foreach (var id in toUpdate)
            {
                existing[id].Name = mdm[id].Name;
                existing[id].Email = mdm[id].Email;
                existing[id].State = mdm[id].State;
            }

            var stagingUsers = new List<StagingUser>();

            foreach (var id in toInsert)
            {
                var stagingUser = new StagingUser
                {
                    Id = mdm[id].Id,
                    Code = mdm[id].Code,
                    Name = mdm[id].Name,
                    NoReg = mdm[id].NoReg,
                    Email = mdm[id].Email,
                    State = mdm[id].State,
                };

                stagingUsers.Add(stagingUser);
            }

            DB.StagingUser.AddRange(stagingUsers);

            await DB.SaveChangesAsync();
        }

        /// <summary>
        /// insert all ActualOrganizationStructure data from MDM to StagingActualOrganizationStructure
        /// </summary>
        /// <param name="models"></param>
        /// <returns></returns>
        public async Task StageActualOrganizationStructure(List<StagingActualOrganizationStructure> models)
        {
            var existing = DB.StagingActualOrganizationStructure.ToDictionary(Q => Q.Code, Q => Q);
            var existingIds = existing.Select(Q => Q.Key).ToList();

            var mdm = models.ToDictionary(Q => Q.Code, Q => Q);
            var mdmIds = mdm.Select(Q => Q.Key).ToList();

            var toUpdate = existingIds.Intersect(mdmIds).ToList();
            var toInsert = mdmIds.Except(existingIds).ToList();
            var toDelete = existingIds.Except(mdmIds).ToList();

            foreach (var id in toUpdate)
            {
                existing[id].Muid = mdm[id].Muid;
                existing[id].VersionName = mdm[id].VersionName;
                existing[id].VersionNumber = mdm[id].VersionNumber;
                existing[id].VersionId = mdm[id].VersionId;
                existing[id].VersionFlag = mdm[id].VersionFlag;
                existing[id].Name = mdm[id].Name;
                existing[id].ChangeTrackingMask = mdm[id].ChangeTrackingMask;
                existing[id].NoReg = mdm[id].NoReg;
                existing[id].PostName = mdm[id].PostName;
                existing[id].PostCode = mdm[id].PostCode;
                existing[id].ParentOrgCode = mdm[id].ParentOrgCode;
                existing[id].JobName = mdm[id].JobName;
                existing[id].JobCode = mdm[id].JobCode;
                existing[id].OrgName = mdm[id].OrgName;
                existing[id].OrgCode = mdm[id].OrgCode;
                existing[id].Service = mdm[id].Service;
                existing[id].EmployeeGroup = mdm[id].EmployeeGroup;
                existing[id].EmployeeGroupText = mdm[id].EmployeeGroupText;
                existing[id].EmployeeSubgroup = mdm[id].EmployeeSubgroup;
                existing[id].EmployeeSubgroupText = mdm[id].EmployeeSubgroupText;
                existing[id].WorkContract = mdm[id].WorkContract;
                existing[id].WorkContractText = mdm[id].WorkContractText;
                existing[id].PersonalArea = mdm[id].PersonalArea;
                existing[id].PersonalSubarea = mdm[id].PersonalSubarea;
                existing[id].DepthLevel = mdm[id].DepthLevel;
                existing[id].Staffing = mdm[id].Staffing;
                existing[id].Chief = mdm[id].Chief;
                existing[id].OrgLevel = mdm[id].OrgLevel;
                existing[id].Period = mdm[id].Period;
                existing[id].Vacant = mdm[id].Vacant;
                existing[id].Structure = mdm[id].Structure;
                existing[id].ObjectDescription = mdm[id].ObjectDescription;
                existing[id].EnterDateTime = mdm[id].EnterDateTime;
                existing[id].EnterUserName = mdm[id].EnterUserName;
                existing[id].EnterVersionNumber = mdm[id].EnterVersionNumber;
                existing[id].LastChgDateTime = mdm[id].LastChgDateTime;
                existing[id].LastChgUserName = mdm[id].LastChgUserName;
                existing[id].LastChgVersionNumber = mdm[id].LastChgVersionNumber;
                existing[id].ValidationStatus = mdm[id].ValidationStatus;
                existing[id].State = mdm[id].State;
            }

            var stagingActualOrganizationStructures = new List<StagingActualOrganizationStructure>();

            foreach (var id in toInsert)
            {
                var stagingActualOrganizationStructure = new StagingActualOrganizationStructure
                {
                    Id = mdm[id].Id,
                    Muid = mdm[id].Muid,
                    VersionName = mdm[id].VersionName,
                    VersionNumber = mdm[id].VersionNumber,
                    VersionId = mdm[id].VersionId,
                    VersionFlag = mdm[id].VersionFlag,
                    Name = mdm[id].Name,
                    Code = mdm[id].Code,
                    ChangeTrackingMask = mdm[id].ChangeTrackingMask,
                    NoReg = mdm[id].NoReg,
                    PostName = mdm[id].PostName,
                    PostCode = mdm[id].PostCode,
                    ParentOrgCode = mdm[id].ParentOrgCode,
                    JobName = mdm[id].JobName,
                    JobCode = mdm[id].JobCode,
                    OrgName = mdm[id].OrgName,
                    OrgCode = mdm[id].OrgCode,
                    Service = mdm[id].Service,
                    EmployeeGroup = mdm[id].EmployeeGroup,
                    EmployeeGroupText = mdm[id].EmployeeGroupText,
                    EmployeeSubgroup = mdm[id].EmployeeSubgroup,
                    EmployeeSubgroupText = mdm[id].EmployeeSubgroupText,
                    WorkContract = mdm[id].WorkContract,
                    WorkContractText = mdm[id].WorkContractText,
                    PersonalArea = mdm[id].PersonalArea,
                    PersonalSubarea = mdm[id].PersonalSubarea,
                    DepthLevel = mdm[id].DepthLevel,
                    Staffing = mdm[id].Staffing,
                    Chief = mdm[id].Chief,
                    OrgLevel = mdm[id].OrgLevel,
                    Period = mdm[id].Period,
                    Vacant = mdm[id].Vacant,
                    Structure = mdm[id].Structure,
                    ObjectDescription = mdm[id].ObjectDescription,
                    EnterDateTime = mdm[id].EnterDateTime,
                    EnterUserName = mdm[id].EnterUserName,
                    EnterVersionNumber = mdm[id].EnterVersionNumber,
                    LastChgDateTime = mdm[id].LastChgDateTime,
                    LastChgUserName = mdm[id].LastChgUserName,
                    LastChgVersionNumber = mdm[id].LastChgVersionNumber,
                    ValidationStatus = mdm[id].ValidationStatus,
                    State = mdm[id].State
                };

                stagingActualOrganizationStructures.Add(stagingActualOrganizationStructure);
            }

            DB.StagingActualOrganizationStructure.AddRange(stagingActualOrganizationStructures);
            await DB.SaveChangesAsync();
        }

        /// <summary>
        /// insert all OrganizationObject data from MDM to StagingOrganizationObject
        /// </summary>
        /// <param name="models"></param>
        /// <returns></returns>
        public async Task StageOrganizationObject(List<StagingOrganizationObject> models)
        {
            var existing = DB.StagingOrganizationObject.ToDictionary(Q => Q.Code, Q => Q);
            var existingIds = existing.Select(Q => Q.Key).ToList();

            var mdm = models.ToDictionary(Q => Q.Code, Q => Q);
            var mdmIds = mdm.Select(Q => Q.Key).ToList();

            var toUpdate = existingIds.Intersect(mdmIds).ToList();
            var toInsert = mdmIds.Except(existingIds).ToList();
            var toDelete = existingIds.Except(mdmIds).ToList();

            foreach (var id in toUpdate)
            {
                existing[id].ObjectType = mdm[id].ObjectType;
                existing[id].ObjectId = mdm[id].ObjectId;
                existing[id].Abbreviation = mdm[id].Abbreviation;
                existing[id].ObjectText = mdm[id].ObjectText;
                existing[id].ObjectDescription = mdm[id].ObjectDescription;
                existing[id].State = mdm[id].State;
            }

            var stagingOrganizationObjects = new List<StagingOrganizationObject>();

            foreach (var id in toInsert)
            {
                var stagingOrganizationObject = new StagingOrganizationObject
                {
                    Id = mdm[id].Id,
                    Code = mdm[id].Code,
                    ObjectType = mdm[id].ObjectType,
                    ObjectId = mdm[id].ObjectId,
                    Abbreviation = mdm[id].Abbreviation,
                    ObjectText = mdm[id].ObjectText,
                    ObjectDescription = mdm[id].ObjectDescription,
                    State = mdm[id].State,
                };

                stagingOrganizationObjects.Add(stagingOrganizationObject);
            }

            DB.StagingOrganizationObject.AddRange(stagingOrganizationObjects);
            await DB.SaveChangesAsync();
        }

        /// <summary>
        /// insert all DealerEmployee data from MDM to StagingDealerEmployee
        /// </summary>
        /// <param name="models"></param>
        /// <returns></returns>
        public async Task StageDealerEmployee(List<StagingDealerEmployeeModel> models)
        {
            var existing = DB.StagingDealerEmployee.ToDictionary(Q => Q.Code, Q => Q);
            var existingIds = existing.Select(Q => Q.Key).ToList();

            var mdm = models.ToDictionary(Q => Q.Code, Q => Q);
            var mdmIds = mdm.Select(Q => Q.Key).ToList();

            var toUpdate = existingIds.Intersect(mdmIds).ToList();
            var toInsert = mdmIds.Except(existingIds).ToList();
            var toDelete = existingIds.Except(mdmIds).ToList();

            foreach (var id in toUpdate)
            {
                existing[id].Name = mdm[id].Name;
                existing[id].OutletId = mdm[id].OutletId;
                existing[id].ManpowerTypeId = mdm[id].ManpowerTypeId;
                existing[id].LastManpowerPositionTypeId = mdm[id].LastManpowerPositionTypeId;
                existing[id].ManpowerStatusTypeId = mdm[id].ManpowerStatusTypeId;
                existing[id].EmployeeId = mdm[id].EmployeeId;
                existing[id].Phone = mdm[id].Phone;
                existing[id].Email = mdm[id].Email;
                existing[id].DateOfBirth = mdm[id].DateOfBirth;
                existing[id].Sex = mdm[id].Sex;
                existing[id].Ktp = mdm[id].Ktp;
                existing[id].ParentCode = mdm[id].SupervisorId;

                if((mdm[id].ResignedAt != null) || (mdm[id].State.ToUpper() == "DELETED"))
                {
                    existing[id].State = "Deleted";
                }
                else
                {
                    existing[id].State = mdm[id].State;
                }

            }

            var stagingDealerEmployees = new List<StagingDealerEmployee>();

            foreach (var id in toInsert)
            {
                var stagingDealerEmployee = new StagingDealerEmployee
                {
                    Id = mdm[id].Id,
                    Code = mdm[id].Code,
                    Name = mdm[id].Name,
                    OutletId = mdm[id].OutletId,
                    ManpowerTypeId = mdm[id].ManpowerTypeId,
                    LastManpowerPositionTypeId = mdm[id].LastManpowerPositionTypeId,
                    ManpowerStatusTypeId = mdm[id].ManpowerStatusTypeId,
                    EmployeeId = mdm[id].EmployeeId,
                    Phone = mdm[id].Phone,
                    Email = mdm[id].Email,
                    Ktp = mdm[id].Ktp,
                    Sex = mdm[id].Sex,
                    DateOfBirth = mdm[id].DateOfBirth,
                    ParentCode = mdm[id].SupervisorId,
                };

                if ((mdm[id].ResignedAt != null) || (mdm[id].State.ToUpper() == "DELETED"))
                {
                    stagingDealerEmployee.State = "Deleted";
                }
                else
                {
                    stagingDealerEmployee.State = mdm[id].State;
                }

                stagingDealerEmployees.Add(stagingDealerEmployee);
            }

            DB.StagingDealerEmployee.AddRange(stagingDealerEmployees);
            await DB.SaveChangesAsync();
        }

        /// <summary>
        /// insert all ManpowerPositionType data from MDM to StagingManpowerPositionType
        /// </summary>
        /// <param name="models"></param>
        /// <returns></returns>
        public async Task StageManpowerPositionType(List<StagingManpowerPositionType> models)
        {
            var existing = DB.StagingManpowerPositionType.ToDictionary(Q => Q.Code, Q => Q);
            var existingIds = existing.Select(Q => Q.Key).ToList();

            var mdm = models.ToDictionary(Q => Q.Code, Q => Q);
            var mdmIds = mdm.Select(Q => Q.Key).ToList();

            var toUpdate = existingIds.Intersect(mdmIds).ToList();
            var toInsert = mdmIds.Except(existingIds).ToList();
            var toDelete = existingIds.Except(mdmIds).ToList();

            foreach (var id in toUpdate)
            {
                existing[id].Name = mdm[id].Name;
                existing[id].State = mdm[id].State;
            }

            var stagingManpowerPositionTypes = new List<StagingManpowerPositionType>();

            foreach (var id in toInsert)
            {
                var stagingManpowerPositionType = new StagingManpowerPositionType
                {
                    Id = mdm[id].Id,
                    Code = mdm[id].Code,
                    Name = mdm[id].Name,
                    State = mdm[id].State
                };

                stagingManpowerPositionTypes.Add(stagingManpowerPositionType);
            }

            DB.StagingManpowerPositionType.AddRange(stagingManpowerPositionTypes);
            await DB.SaveChangesAsync();
        }

        /// <summary>
        /// insert all ManpowerType data from MDM to StagingManpowerType
        /// </summary>
        /// <param name="models"></param>
        /// <returns></returns>
        public async Task StageManpowerType(List<StagingManpowerType> models)
        {
            var existing = DB.StagingManpowerType.ToDictionary(Q => Q.Code, Q => Q);
            var existingIds = existing.Select(Q => Q.Key).ToList();

            var mdm = models.ToDictionary(Q => Q.Code, Q => Q);
            var mdmIds = mdm.Select(Q => Q.Key).ToList();

            var toUpdate = existingIds.Intersect(mdmIds).ToList();
            var toInsert = mdmIds.Except(existingIds).ToList();
            var toDelete = existingIds.Except(mdmIds).ToList();

            foreach (var id in toUpdate)
            {
                existing[id].Name = mdm[id].Name;
                existing[id].State = mdm[id].State;
            }

            var stagingManpowerTypes = new List<StagingManpowerType>();

            foreach (var id in toInsert)
            {
                var stagingManpowerType = new StagingManpowerType
                {
                    Id = mdm[id].Id,
                    Code = mdm[id].Code,
                    Name = mdm[id].Name,
                    State = mdm[id].State
                };

                stagingManpowerTypes.Add(stagingManpowerType);
            }

            DB.StagingManpowerType.AddRange(stagingManpowerTypes);
            await DB.SaveChangesAsync();
        }

        /// <summary>
        /// insert all Outlet data from MDM to StagingOutlet
        /// </summary>
        /// <param name="models"></param>
        /// <returns></returns>
        public async Task StageOutlet(List<StagingOutlet> models)
        {
            var existing = DB.StagingOutlet.ToDictionary(Q => Q.Code, Q => Q);
            var existingIds = existing.Select(Q => Q.Key).ToList();

            var mdm = models.ToDictionary(Q => Q.Code, Q => Q);
            var mdmIds = mdm.Select(Q => Q.Key).ToList();

            var toUpdate = existingIds.Intersect(mdmIds).ToList();
            var toInsert = mdmIds.Except(existingIds).ToList();
            var toDelete = existingIds.Except(mdmIds).ToList();

            foreach (var id in toUpdate)
            {
                existing[id].Name = mdm[id].Name;
                existing[id].DealerCompanyId = mdm[id].DealerCompanyId;
                existing[id].OutletFunctionId = mdm[id].OutletFunctionId;
                existing[id].OutletCode = mdm[id].OutletCode;
                existing[id].PhoneNumber = mdm[id].PhoneNumber;
                existing[id].KabupatenId = mdm[id].KabupatenId;
                existing[id].RegionCode = mdm[id].RegionCode;
                existing[id].TamAreaId = mdm[id].TamAreaId;
                existing[id].TamAreaAfterSalesId = mdm[id].TamAreaAfterSalesId;
                existing[id].State = mdm[id].State;
            }

            var stagingOutlets = new List<StagingOutlet>();

            foreach (var id in toInsert)
            {
                var stagingOutlet = new StagingOutlet
                {
                    Id = mdm[id].Id,
                    Code = mdm[id].Code,
                    Name = mdm[id].Name,
                    DealerCompanyId = mdm[id].DealerCompanyId,
                    OutletFunctionId = mdm[id].OutletFunctionId,
                    OutletCode = mdm[id].OutletCode,
                    PhoneNumber = mdm[id].PhoneNumber,
                    KabupatenId = mdm[id].KabupatenId,
                    RegionCode = mdm[id].RegionCode,
                    TamAreaId = mdm[id].TamAreaId,
                    TamAreaAfterSalesId = mdm[id].TamAreaAfterSalesId,
                    State = mdm[id].State
                };

                stagingOutlets.Add(stagingOutlet);
            }

            DB.StagingOutlet.AddRange(stagingOutlets);
            await DB.SaveChangesAsync();
        }

        /// <summary>
        /// insert all Region data from MDM to StagingRegion
        /// </summary>
        /// <param name="models"></param>
        /// <returns></returns>
        public async Task StageRegion(List<StagingRegion> models)
        {
            var existing = DB.StagingRegion.ToDictionary(Q => Q.Code, Q => Q);
            var existingIds = existing.Select(Q => Q.Key).ToList();

            var mdm = models.ToDictionary(Q => Q.Code, Q => Q);
            var mdmIds = mdm.Select(Q => Q.Key).ToList();

            var toUpdate = existingIds.Intersect(mdmIds).ToList();
            var toInsert = mdmIds.Except(existingIds).ToList();
            var toDelete = existingIds.Except(mdmIds).ToList();

            foreach (var id in toUpdate)
            {
                existing[id].Name = mdm[id].Name;
                existing[id].ParentCode = mdm[id].ParentCode;
                existing[id].RegionType = mdm[id].RegionType;
                existing[id].State = mdm[id].State;
            }

            var stagingRegions = new List<StagingRegion>();

            foreach (var id in toInsert)
            {
                var stagingRegion = new StagingRegion
                {
                    Id = mdm[id].Id,
                    Code = mdm[id].Code,
                    RegionType = mdm[id].RegionType,
                    Name = mdm[id].Name,
                    ParentCode = mdm[id].ParentCode,
                    State = mdm[id].State
                };

                stagingRegions.Add(stagingRegion);
            }

            var tes = toInsert.Distinct();

            DB.StagingRegion.AddRange(stagingRegions);
            await DB.SaveChangesAsync();
        }

        /// <summary>
        /// insert all SalesArea data from MDM to StagingSalesArea
        /// </summary>
        /// <param name="models"></param>
        /// <returns></returns>
        public async Task StageSalesArea(List<StagingSalesArea> models)
        {
            var existing = DB.StagingSalesArea.ToDictionary(Q => Q.Code, Q => Q);
            var existingIds = existing.Select(Q => Q.Key).ToList();

            var mdm = models.ToDictionary(Q => Q.Code, Q => Q);
            var mdmIds = mdm.Select(Q => Q.Key).ToList();

            var toUpdate = existingIds.Intersect(mdmIds).ToList();
            var toInsert = mdmIds.Except(existingIds).ToList();
            var toDelete = existingIds.Except(mdmIds).ToList();

            foreach (var id in toUpdate)
            {
                existing[id].Name = mdm[id].Name;
                existing[id].State = mdm[id].State;
            }

            var stagingSalesAreas = new List<StagingSalesArea>();

            foreach (var id in toInsert)
            {
                var stagingSalesArea = new StagingSalesArea
                {
                    Id = mdm[id].Id,
                    Code = mdm[id].Code,
                    Name = mdm[id].Name,
                    State = mdm[id].State
                };

                stagingSalesAreas.Add(stagingSalesArea);
            }

            DB.StagingSalesArea.AddRange(stagingSalesAreas);
            await DB.SaveChangesAsync();
        }

        /// <summary>
        /// insert all AfterSalesArea data from MDM to StagingAfterSalesArea
        /// </summary>
        /// <param name="models"></param>
        /// <returns></returns>
        public async Task StageAfterSalesArea(List<StagingAfterSalesArea> models)
        {
            var existing = DB.StagingAfterSalesArea.ToDictionary(Q => Q.Code, Q => Q);
            var existingIds = existing.Select(Q => Q.Key).ToList();

            var mdm = models.ToDictionary(Q => Q.Code, Q => Q);
            var mdmIds = mdm.Select(Q => Q.Key).ToList();

            var toUpdate = existingIds.Intersect(mdmIds).ToList();
            var toInsert = mdmIds.Except(existingIds).ToList();
            var toDelete = existingIds.Except(mdmIds).ToList();

            foreach (var id in toUpdate)
            {
                existing[id].Name = mdm[id].Name;
                existing[id].State = mdm[id].State;
            }

            var stagingAfterSalesAreas = new List<StagingAfterSalesArea>();

            foreach (var id in toInsert)
            {
                var stagingAfterSalesArea = new StagingAfterSalesArea
                {
                    Id = mdm[id].Id,
                    Code = mdm[id].Code,
                    Name = mdm[id].Name,
                    State = mdm[id].State
                };

                stagingAfterSalesAreas.Add(stagingAfterSalesArea);
            }

            DB.StagingAfterSalesArea.AddRange(stagingAfterSalesAreas);
            await DB.SaveChangesAsync();
        }

        /// <summary>
        /// insert all DealerGroup data from MDM to StagingDealerGroup
        /// </summary>
        /// <param name="models"></param>
        /// <returns></returns>
        public async Task StageDealerGroup(List<StagingDealerGroup> models)
        {
            var existing = DB.StagingDealerGroup.ToDictionary(Q => Q.Code, Q => Q);
            var existingIds = existing.Select(Q => Q.Key).ToList();

            var mdm = models.ToDictionary(Q => Q.Code, Q => Q);
            var mdmIds = mdm.Select(Q => Q.Key).ToList();

            var toUpdate = existingIds.Intersect(mdmIds).ToList();
            var toInsert = mdmIds.Except(existingIds).ToList();
            var toDelete = existingIds.Except(mdmIds).ToList();

            foreach (var id in toUpdate)
            {
                existing[id].Name = mdm[id].Name;
                existing[id].DealerGroupCode = mdm[id].DealerGroupCode;
                existing[id].State = mdm[id].State;
            }

            var stagingDealerGroups = new List<StagingDealerGroup>();

            foreach (var id in toInsert)
            {
                var stagingDealerGroup = new StagingDealerGroup
                {
                    Id = mdm[id].Id,
                    Code = mdm[id].Code,
                    Name = mdm[id].Name,
                    DealerGroupCode = mdm[id].DealerGroupCode,
                    State = mdm[id].State
                };

                stagingDealerGroups.Add(stagingDealerGroup);
            }

            DB.StagingDealerGroup.AddRange(stagingDealerGroups);
            await DB.SaveChangesAsync();
        }

        /// <summary>
        /// insert all DealerCompany data from MDM to StagingDealerCompany
        /// </summary>
        /// <param name="models"></param>
        /// <returns></returns>
        public async Task StageDealerCompany(List<StagingDealerCompany> models)
        {
            var existing = DB.StagingDealerCompany.ToDictionary(Q => Q.Code, Q => Q);
            var existingIds = existing.Select(Q => Q.Key).ToList();

            var mdm = models.ToDictionary(Q => Q.Code, Q => Q);
            var mdmIds = mdm.Select(Q => Q.Key).ToList();

            var toUpdate = existingIds.Intersect(mdmIds).ToList();
            var toInsert = mdmIds.Except(existingIds).ToList();
            var toDelete = existingIds.Except(mdmIds).ToList();

            foreach (var id in toUpdate)
            {
                existing[id].Name = mdm[id].Name;
                existing[id].DealerGroupId = mdm[id].DealerGroupId;
                existing[id].State = mdm[id].State;
            }

            var stagingDealerCompanies = new List<StagingDealerCompany>();

            foreach (var id in toInsert)
            {
                var stagingDealerCompany = new StagingDealerCompany
                {
                    Id = mdm[id].Id,
                    Code = mdm[id].Code,
                    Name = mdm[id].Name,
                    DealerGroupId = mdm[id].DealerGroupId,
                    State = mdm[id].State
                };

                stagingDealerCompanies.Add(stagingDealerCompany);
            }

            DB.StagingDealerCompany.AddRange(stagingDealerCompanies);
            await DB.SaveChangesAsync();
        }

    }
}
