using System.ComponentModel.DataAnnotations;
using Volo.Abp.Content;
using Volo.Abp.Validation;
using Volo.CmsKit.MediaDescriptors;

namespace Volo.CmsKit.Admin.MediaDescriptors
{
    public class CreateMediaInput
    {
        [Required]
        [DynamicStringLength(typeof(MediaDescriptorConsts), nameof(MediaDescriptorConsts.MaxEntityTypeLength))]
        public string EntityType { get; set; }
        
        [Required]
        [DynamicStringLength(typeof(MediaDescriptorConsts), nameof(MediaDescriptorConsts.MaxNameLength))]
        public string Name { get; set; }
        
        [DisableValidation]
        [From]
        public RemoteStreamContent StreamContent { get; set; }
    }
}