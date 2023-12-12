using AutoMapper;
using LuduStack.Application.ViewModels.Content;
using LuduStack.Domain.Messaging.Queries.UserContent;
using LuduStack.Domain.Messaging;
using LuduStack.Domain.Models;
using LuduStack.Domain.ValueObjects;
using LuduStack.Infra.CrossCutting.Messaging;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LuduStack.Application.Interfaces;
using LuduStack.Application.Services;
using MetaG.Application.Interfaces;
using MetaG.Application.ViewModels;
using MetaG.Domain.Models;
using LuduStack.Domain.Messaging.Queries.UserProfile;
using MetaG.Domain.Messaging.Queries;
using LuduStack.Application;
using LuduStack.Domain.Core.Enums;
using LuduStack.Application.Formatters;
using LuduStack.Domain.Core.Extensions;
using LuduStack.Application.Helpers;
using LuduStack.Domain.Services;

namespace MetaG.Application.Services
{
    public class BannerAppService : ProfileBaseAppService, IBannerAppService
    {
        public BannerAppService(IProfileBaseAppServiceCommon profileBaseAppServiceCommon) : base(profileBaseAppServiceCommon)
        {
        }

        public async Task<OperationResultVo<BannerViewModel>> GetById(Guid currentUserId, Guid id)
        {
            try
            {
                Banner model = await mediator.Query<GetBannerByIdQuery, Banner>(new GetBannerByIdQuery(id));

                BannerViewModel v = mapper.Map<BannerViewModel>(model);

                v.HasFeaturedImage = !string.IsNullOrWhiteSpace(v.FeaturedImage) && !v.FeaturedImage.Contains(Constants.DefaultFeaturedImage);

                SetFeaturedMedia(v);

                return new OperationResultVo<BannerViewModel>(v);
            }
            catch (Exception ex)
            {
                return new OperationResultVo<BannerViewModel>(ex.Message);
            }
        }

        public async Task<OperationResultVo<Guid>> Save(Guid currentUserId, BannerViewModel viewModel)
        {
            try
            {
                Banner model;
                Poll pollModel = null;

                bool isNew = viewModel.Id == Guid.Empty;

                Banner existing = await mediator.Query<GetBannerByIdQuery, Banner>(new GetBannerByIdQuery(viewModel.Id));
                if (existing != null)
                {
                    model = mapper.Map(viewModel, existing);
                }
                else
                {
                    model = mapper.Map<Banner>(viewModel);
                }

                CommandResult result = await mediator.SendCommand(new SaveBannerCommand(currentUserId, model));

                if (!result.Validation.IsValid)
                {
                    string message = result.Validation.Errors.FirstOrDefault().ErrorMessage;
                    return new OperationResultVo<Guid>(model.Id, false, message);
                }
                else
                {
                    return new OperationResultVo<Guid>(model.Id, isNew ? "Post published!" : "Content saved!");
                }
            }
            catch (Exception ex)
            {
                return new OperationResultVo<Guid>(ex.Message);
            }
        }

        public async Task<OperationResultVo> Remove(Guid currentUserId, Guid id)
        {
            try
            {
                await mediator.SendCommand(new DeleteBannerCommand(currentUserId, id));

                return new OperationResultVo(true);
            }
            catch (Exception ex)
            {
                return new OperationResultVo(ex.Message);
            }
        }

        public async Task<OperationResultListVo<BannerViewModel>> GetAll()
        {
            try
            {
                IEnumerable<Banner> allModels = await mediator.Query<GetBannerQuery, IEnumerable<Banner>>(new GetBannerQuery());

                IEnumerable<BannerViewModel> vms = mapper.Map<IEnumerable<Banner>, IEnumerable<BannerViewModel>>(allModels);

                foreach (BannerViewModel v in vms)
                {
                    v.HasFeaturedImage = !string.IsNullOrWhiteSpace(v.FeaturedImage) && !v.FeaturedImage.Contains(Constants.DefaultFeaturedImage);
                    SetFeaturedMedia(v);
                }
                return new OperationResultListVo<BannerViewModel>(vms);
            }
            catch (Exception ex)
            {
                return new OperationResultListVo<BannerViewModel>(ex.Message);
            }
        }

        private static void SetFeaturedMedia(BannerViewModel item)
        {
            SetFeaturedImageUrls(item,item.FeaturedImage);
        }

        private static void SetFeaturedImageUrls(BannerViewModel item, string selectedFeaturedMedia)
        {
            item.FeaturedImage = UrlFormatter.FormatFeaturedImageUrl(item.UserId, selectedFeaturedMedia, ImageRenderType.Full);
            item.FeaturedImageResponsive = UrlFormatter.FormatFeaturedImageUrl(item.UserId, selectedFeaturedMedia, ImageRenderType.Responsive);
            item.FeaturedImageLquip = UrlFormatter.FormatFeaturedImageUrl(item.UserId, selectedFeaturedMedia, ImageRenderType.LowQuality);
        }

    }
}
