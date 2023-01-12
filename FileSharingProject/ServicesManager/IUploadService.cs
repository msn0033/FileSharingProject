using FileSharingProject.Data;
using FileSharingProject.Models;

namespace FileSharingProject.ServicesManager
{
    public interface IUploadService
    {
        Task<Upload> CreateAsync(Upload model);
        IQueryable<Upload> GetAll();
        IQueryable<Upload> GetAllByUserId(string userid);
        Task<Upload> GetByIdAsync(string id);
         Upload Delete(Upload model);
        Task<Upload> FindAsync(string id);
        Upload Update(Upload model);
        Task SaveAsync();
        void IncurmentDownloadCount(Upload model);
        Task<int> GetUploadCount();
    }
}
