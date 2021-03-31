using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp;
using Volo.Abp.Content;
using Volo.Abp.GlobalFeatures;
using Volo.CmsKit.GlobalFeatures;

namespace Volo.CmsKit.Admin.MediaDescriptors
{
    [RequiresGlobalFeature(typeof(MediaFeature))]
    [RemoteService(Name = CmsKitAdminRemoteServiceConsts.RemoteServiceName)]
    [Area("cms-kit")]
    [Route("api/cms-kit-admin/media")]
    public class MediaDescriptorAdminController : CmsKitAdminController, IMediaDescriptorAdminAppService
    {
        protected readonly IMediaDescriptorAdminAppService MediaDescriptorAdminAppService;

        public MediaDescriptorAdminController(IMediaDescriptorAdminAppService mediaDescriptorAdminAppService)
        {
            MediaDescriptorAdminAppService = mediaDescriptorAdminAppService;
        }

        [HttpPost]
        public virtual Task<MediaDescriptorDto> CreateAsync(CreateMediaInput input)
        {
            return MediaDescriptorAdminAppService.CreateAsync(input);
        }

        [HttpDelete]
        [Route("{id}")]
        public virtual Task DeleteAsync(Guid id)
        {
            return MediaDescriptorAdminAppService.DeleteAsync(id);
        }

        [HttpPost]
        [Route("{entityType}")]
        public virtual async Task<IActionResult> UploadAsync(string entityType, IFormFile file)
        {
            if (file == null)
            {
                return BadRequest();
            }

            var input = new CreateMediaInput
            {
                EntityType = entityType,
                Name = file.FileName,
                StreamContent = new RemoteStreamContent(file.OpenReadStream())
                {
                    ContentType = file.ContentType
                }
            };

            var mediaDescriptorDto = await MediaDescriptorAdminAppService.CreateAsync(input);
            
            return StatusCode((int)HttpStatusCode.Created, mediaDescriptorDto);
        }
    }
}