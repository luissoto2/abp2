using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.BlobStoring;
using Volo.Abp.Content;
using Volo.Abp.GlobalFeatures;
using Volo.CmsKit.GlobalFeatures;
using Volo.CmsKit.MediaDescriptors;
using Volo.CmsKit.Permissions;

namespace Volo.CmsKit.Admin.MediaDescriptors
{
    [RequiresGlobalFeature(typeof(MediaFeature))]
    public class MediaDescriptorAdminAppService : CmsKitAdminAppServiceBase, IMediaDescriptorAdminAppService
    {
        protected IBlobContainer<MediaContainer> MediaContainer { get; }
        protected IMediaDescriptorRepository MediaDescriptorRepository { get; }
        protected MediaDescriptorManager MediaDescriptorManager { get; }
        protected IMediaDescriptorDefinitionStore MediaDescriptorDefinitionStore { get; }

        public MediaDescriptorAdminAppService(
            IBlobContainer<MediaContainer> mediaContainer,
            IMediaDescriptorRepository mediaDescriptorRepository,
            MediaDescriptorManager mediaDescriptorManager, 
            IMediaDescriptorDefinitionStore mediaDescriptorDefinitionStore)
        {
            MediaContainer = mediaContainer;
            MediaDescriptorRepository = mediaDescriptorRepository;
            MediaDescriptorManager = mediaDescriptorManager;
            MediaDescriptorDefinitionStore = mediaDescriptorDefinitionStore;
        }

        public virtual async Task<MediaDescriptorDto> CreateAsync(CreateMediaInput input)
        {
            var definition = await MediaDescriptorDefinitionStore.GetAsync(input.EntityType);

            /* TODO: Shouldn't CreatePolicies be a dictionary and we check for inputStream.EntityType? */
            await CheckAnyOfPoliciesAsync(definition.CreatePolicies);

            using (var stream = input.StreamContent.GetStream())
            {
                var newEntity = await MediaDescriptorManager.CreateAsync(input.EntityType, input.Name, input.StreamContent.ContentType, input.StreamContent.ContentLength ?? 0);

                await MediaContainer.SaveAsync(newEntity.Id.ToString(), stream);
                await MediaDescriptorRepository.InsertAsync(newEntity);

                return ObjectMapper.Map<MediaDescriptor, MediaDescriptorDto>(newEntity);
            }
        }

        public virtual async Task DeleteAsync(Guid id)
        {
            var mediaDescriptor = await MediaDescriptorRepository.GetAsync(id);

            var definition = await MediaDescriptorDefinitionStore.GetAsync(mediaDescriptor.EntityType);

            /* TODO: Shouldn't DeletePolicies be a dictionary and we check for inputStream.EntityType? */
            await CheckAnyOfPoliciesAsync(definition.DeletePolicies);

            await MediaContainer.DeleteAsync(id.ToString());
            await MediaDescriptorRepository.DeleteAsync(id);
        }
    }
}