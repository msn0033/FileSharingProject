using AutoMapper;
using AutoMapper.QueryableExtensions;
using FileSharingProject.Data;
using FileSharingProject.Helpers.AutoMapper;
using FileSharingProject.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Specialized;
using System.Security.Claims;

namespace FileSharingProject.ServicesManager
{
    public class UploadService : IUploadService
    {
        private readonly AppDbContext _dbContext;

        public UploadService(AppDbContext dbContext)
        {
            this._dbContext = dbContext;
        }
        public async Task<Upload> CreateAsync(Upload model)
        {
             var entity= await _dbContext.Uploads.AddAsync(model);
            return entity.Entity;
        }

        public  Upload Delete(Upload model)
        {
            var remove = _dbContext.Uploads.Remove(model);
            return remove.Entity;
        }

        public async Task<Upload> FindAsync(string id)
        {
            return await _dbContext.Uploads.FindAsync(id);
        }

        public IQueryable<Upload> GetAll()
        {
            return _dbContext.Uploads;
        }

        public  IQueryable<Upload> GetAllByUserId(string userid)
        {
            return _dbContext.Uploads.Where(u => u.UserId == userid).OrderByDescending(x => x.DownloadCount);
        }
        public async Task<Upload>? GetByIdAsync(string id)
        {
            return await _dbContext.Uploads.FirstOrDefaultAsync(u=>u.Id== id);

        }

        public  async Task<int> GetUploadCount()
        {
            return await _dbContext.Uploads.CountAsync();
        }

        public void IncurmentDownloadCount(Upload model)
        {
            model.DownloadCount++;
            
        }
        public async Task SaveAsync()
        {
            await _dbContext.SaveChangesAsync();
        }

        public Upload Update(Upload model)
        {
            return _dbContext.Uploads.Update(model).Entity;
        }
    }
}
