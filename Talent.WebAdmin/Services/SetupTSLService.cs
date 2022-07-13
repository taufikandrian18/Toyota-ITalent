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
    public class SetupTSLService
    {
        private readonly TalentContext _Db;
        private readonly ClaimHelper ClaimMan;

        public SetupTSLService(TalentContext talentContext, ClaimHelper claimHelper)
        {
            this._Db = talentContext;
            this.ClaimMan = claimHelper;
        }

        public async Task<SetupTSLViewModel> GetAllTSLData()
        {
            var getAllTSLData = await this._Db.TrainingServiceLevels.ToListAsync();

            var getSalesTSLData = getAllTSLData.Where(Q => Q.TrainingServiceLevelId == 1).FirstOrDefault();
            var salesTSLDataResult = new Sales
            {
                BasicLevel = getSalesTSLData.BasicLevel,
                FundamentalLevel = getSalesTSLData.FundamentalLevel,
                AdvanceLevel = getSalesTSLData.AdvanceLevel
            };

            var getAfterSalesTSLData = getAllTSLData.Where(Q => Q.TrainingServiceLevelId == 2).FirstOrDefault();
            var afterSalesTSLDataResult = new AfterSales
            {
                BasicLevel = getAfterSalesTSLData.BasicLevel,
                FundamentalLevel = getAfterSalesTSLData.FundamentalLevel,
                AdvanceLevel = getAfterSalesTSLData.AdvanceLevel
            };

            var getTotalTslData = getAllTSLData.Where(Q => Q.TrainingServiceLevelId == 3).FirstOrDefault();
            var totalTSLDataResult = new TotalSales
            {
                BasicLevel = getTotalTslData.BasicLevel,
                FundamentalLevel = getTotalTslData.FundamentalLevel,
                AdvanceLevel = getTotalTslData.AdvanceLevel
            };

            var tslDataResult = new SetupTSLViewModel
            {
                 Sales = salesTSLDataResult,
                 AfterSales = afterSalesTSLDataResult,
                 Total = totalTSLDataResult
            };

            return tslDataResult;
        }

        public async Task<bool> UpdateTrainingServiceLevel(SetupTSLViewModel model)
        {
            var getAllTSLData = await this._Db.TrainingServiceLevels.ToListAsync();

            var updateSalesTSL = getAllTSLData.Where(Q => Q.TrainingServiceLevelId == 1).FirstOrDefault();
            updateSalesTSL.BasicLevel = model.Sales.BasicLevel;
            updateSalesTSL.FundamentalLevel = model.Sales.FundamentalLevel;
            updateSalesTSL.AdvanceLevel = model.Sales.AdvanceLevel;
            updateSalesTSL.UpdatedAt = DateTime.UtcNow.ToIndonesiaTimeZone();
            updateSalesTSL.UpdatedBy = ClaimMan.GetLoginUserId();

            var updateAfterSalesTSL = getAllTSLData.Where(Q => Q.TrainingServiceLevelId == 2).FirstOrDefault();
            updateAfterSalesTSL.BasicLevel = model.AfterSales.BasicLevel;
            updateAfterSalesTSL.FundamentalLevel = model.AfterSales.FundamentalLevel;
            updateAfterSalesTSL.AdvanceLevel = model.AfterSales.AdvanceLevel;
            updateAfterSalesTSL.UpdatedAt = DateTime.UtcNow.ToIndonesiaTimeZone();
            updateAfterSalesTSL.UpdatedBy = ClaimMan.GetLoginUserId();

            var updateTotalTsl = getAllTSLData.Where(Q => Q.TrainingServiceLevelId == 3).FirstOrDefault();
            updateTotalTsl.BasicLevel = model.Total.BasicLevel;
            updateTotalTsl.FundamentalLevel = model.Total.FundamentalLevel;
            updateTotalTsl.AdvanceLevel = model.Total.AdvanceLevel;
            updateTotalTsl.UpdatedAt = DateTime.UtcNow.ToIndonesiaTimeZone();
            updateTotalTsl.UpdatedBy = ClaimMan.GetLoginUserId();

            await this._Db.SaveChangesAsync();
            return true;
        }
    }
}
