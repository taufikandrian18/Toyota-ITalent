using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Talent.Entities.Entities;
using Talent.WebAdmin.Models;

namespace Talent.WebAdmin.Services
{
    public class BlobService
    {
        private readonly TalentContext _Db;

        public BlobService(TalentContext talentContext)
        {
            this._Db = talentContext;
        }

        public async Task<BlobModel> GetBlobById(Guid? id)
        {
            if(id == null)
            {
                return null;
            }
            var data = await this._Db.Blobs.Where(Q => Q.BlobId == id).Select(Q => new BlobModel
            {
                BlobId = Q.BlobId,
                Name = Q.Name,
                Mime = Q.Mime
            }).FirstOrDefaultAsync(); 
            return data;
        }

        public async Task<bool> DeleteBlob(Guid id)
        {
            var data = await this._Db.Blobs.Where(Q => Q.BlobId == id).FirstOrDefaultAsync();

            this._Db.Remove(data);
            await this._Db.SaveChangesAsync();
            return true;
        }

        public async Task<BlobViewModel> GetAllBlobs()
        {
            var allBlobs = await this._Db.Blobs.Select(Q => new BlobModel
            {
                BlobId = Q.BlobId,
                Name = Q.Name,
                Mime = Q.Mime
            }).ToListAsync();
            var totalItem = await this._Db.Blobs.CountAsync();
            var result = new BlobViewModel
            {
                ListBlobModel = allBlobs,
                TotalItem = totalItem
            };
            return result;
        }
    }
}
