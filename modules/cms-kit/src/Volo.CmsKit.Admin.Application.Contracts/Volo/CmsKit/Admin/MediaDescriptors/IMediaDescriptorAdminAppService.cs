using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace Volo.CmsKit.Admin.MediaDescriptors
{
    public interface IMediaDescriptorAdminAppService : IApplicationService
    {
        Task<MediaDescriptorDto> CreateAsync(CreateMediaInput input);
        
        Task DeleteAsync(Guid id);
    }
}