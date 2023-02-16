using AutoMapper;
using FileSharingProject.Data;
using FileSharingProject.Models;

namespace FileSharingProject.Helpers.AutoMapper
{
    public class UploadProfile: Profile
    {
        public UploadProfile()
        {
            CreateMap<Upload, UploadViewModel>()
                .ForMember(des => des.UploadId, opt => opt.MapFrom(source => source.Id));
            CreateMap<InputUpload, Upload>()
                .ForMember(des => des.Id, op => op.Ignore())
                .ForMember(des => des.UploadDate, op => op.Ignore());
         // CreateMap<>
        }
    }
    public class UserProfile:Profile
    {
        public UserProfile()
        {
            CreateMap<ApplicationUser, UserViewModel>()
                .ForMember(x=>x.HasPassword,op=>op.MapFrom(x=>x.PasswordHash !=null));
           // CreateMap<>
        }

    }
}
