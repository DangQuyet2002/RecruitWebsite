using AutoMapper.QueryableExtensions;
using LuduStack.Application;
using LuduStack.Application.Formatters;
using LuduStack.Application.Interfaces;
using LuduStack.Application.Services;
using LuduStack.Application.ViewModels.Game;
using LuduStack.Domain.Core.Enums;
using LuduStack.Domain.Core.Extensions;
using LuduStack.Domain.Core.Interfaces;
using LuduStack.Domain.Interfaces.Repository;
using LuduStack.Domain.Interfaces.Services;
using LuduStack.Domain.Messaging;
using LuduStack.Domain.Messaging.Queries.Game;
using LuduStack.Domain.Models;
using LuduStack.Domain.Services;
using LuduStack.Domain.Specifications;
using LuduStack.Domain.ValueObjects;
using LuduStack.Infra.CrossCutting.Messaging;
using MetaG.Application.Interfaces;
using MetaG.Application.ViewModels;
using MetaG.Domain.Messaging.Commands.GameCat;
using MetaG.Domain.Messaging.Queries.Game;
using MetaG.Domain.Messaging.Queries.GameCat;
using MetaG.Domain.Models;
using Microsoft.AspNetCore.Http.HttpResults;

namespace MetaG.Application.Services
{
	public class MetaGameAppService : GameAppService, IMetaGameAppService
	{
        private readonly IPollDomainService pollDomainService;

        public MetaGameAppService(IProfileBaseAppServiceCommon profileBaseAppServiceCommon 
            , IPollDomainService pollDomainService) :
             base(profileBaseAppServiceCommon)
		{
            this.pollDomainService = pollDomainService;
        }

        public async Task<List<GameCategoryViewModel>> GetGameCategory(Guid? categoryId)
        {
            try
            {
                IEnumerable<GameCategory> allModels = await mediator.Query<GetListGameCategoryQuery, IEnumerable<GameCategory>>(new GetListGameCategoryQuery(categoryId));

                IEnumerable<GameCategoryViewModel> vms = mapper.Map<IEnumerable<GameCategory>, IEnumerable<GameCategoryViewModel>>(allModels);

                return new List<GameCategoryViewModel>(vms);
            }
            catch (Exception ex)
            {
                return new List<GameCategoryViewModel>();
            }
        }
        public async Task<List<GameFeeViewModel>> GetPaidGames()
        {
            try
            {
                IEnumerable<GameFee> allModels = await mediator.Query<GetPaidGamesQuery, IEnumerable<GameFee>>(new GetPaidGamesQuery());

                List<GameFeeViewModel> vms = mapper.Map<IEnumerable<GameFee>, IEnumerable<GameFeeViewModel>>(allModels).ToList();

                return new List<GameFeeViewModel>(vms);
            }
            catch (Exception ex)
            {
                return new List<GameFeeViewModel>();
            }

        }

        public async Task<List<GameViewModel>> GetGameOfCategory(GameGenre genre, int take)
		{
			try
			{
				var games = await mediator.Query<GetGameQuery, IEnumerable<Game>>(new GetGameQuery(take, genre, Guid.Empty, null));
				IEnumerable<GameViewModel> vms = mapper.Map<IEnumerable<Game>, IEnumerable<GameViewModel>>(games);

                IEnumerable<Guid> userIds = vms.Select(x => x.UserId);

                List<UserProfileEssentialVo> authorProfiles = await GetCachedEssentialProfilesByUserIds(userIds);
                foreach(GameViewModel item in vms)
                {
                    item.ThumbnailUrl = SetFeaturedImage(item.UserId, item.ThumbnailUrl, ImageRenderType.Full);
                    item.CoverImageUrl = SetFeaturedImage(item.UserId, item.CoverImageUrl, ImageRenderType.Full);

                    UserProfileEssentialVo authorProfile = authorProfiles.FirstOrDefault(x => x.UserId == item.UserId);
                    if (authorProfile != null)
                    {                 
                        item.DeveloperName = authorProfile?.Name;
                    }
                }
                return new List<GameViewModel>(vms);
			}
			catch (Exception ex)
			{
				return new List<GameViewModel>();
			}
		}

        public async Task<List<Game>> GetGameOfGameFee(Guid id)
        {
            try
            {
                var games = await mediator.Query<GetGameQuery, IEnumerable<Game>>(new GetGameQuery(id));
                
                List<Game> vms = mapper.Map<IEnumerable<Game>, IEnumerable<Game>>(games).ToList();
                IEnumerable<Guid> userIds = vms.Select(x => x.UserId);

                List<UserProfileEssentialVo> authorProfiles = await GetCachedEssentialProfilesByUserIds(userIds);
                foreach (Game item in vms)
                {
                    item.ThumbnailUrl = SetFeaturedImage(item.UserId, item.ThumbnailUrl, ImageRenderType.Full);

                    UserProfileEssentialVo authorProfile = authorProfiles.FirstOrDefault(x => x.UserId == item.UserId);
                    if (authorProfile != null)
                    {
                        item.DeveloperName = authorProfile?.Name;
                    }
                }

                return new List<Game>(vms);
            }
            catch (Exception ex)
            {
                return new List<Game>();
            }
        }


        public async Task<OperationResultVo<int>> CountCategory(Guid currentUserId)
        {
            try
            {
                int count = await mediator.Query<CountCategoryQuery, int>(new CountCategoryQuery());

                return new OperationResultVo<int>(count);
            }
            catch (Exception ex)
            {
                return new OperationResultVo<int>(ex.Message);
            }
        }

        public OperationResultVo<GameCategoryViewModel> CreateNew(Guid currentUserId)
        {
            GameCategoryViewModel vm = new GameCategoryViewModel
            {
                UserId = currentUserId,
                CoverImageUrl = Constants.DefaultGameCoverImage,
                LargeThumbImageUrl = Constants.DefaultGameThumbnail
            };

            return new OperationResultVo<GameCategoryViewModel>(vm);
        }

        public async Task<OperationResultVo<GameCategory>> Save(Guid currentUserId, GameCategoryViewModel viewModel)
        {
            GameCategory model = null;
            try
            {

                ISpecification<GameCategoryViewModel> spec = new UserMustBeAuthenticatedSpecification<GameCategoryViewModel>(currentUserId)
                    .And(new UserIsOwnerSpecification<GameCategoryViewModel>(currentUserId));

                GameCategory existing = await mediator.Query<GetGameCategoryByIdQuery, GameCategory>(new GetGameCategoryByIdQuery(viewModel.Id));

                if (existing != null)
                {
                    model = mapper.Map(viewModel, existing);
                }
                else
                {
                    model = mapper.Map<GameCategory>(viewModel);
                }

                CommandResult result = await mediator.SendCommand(new SaveGameCategoryCommand(model));


                return new OperationResultVo<GameCategory>(model, "Category saved!");
            }
            catch (Exception ex)
            {
                string errorMessage = ex.Message ?? "An error occurred while saving the Category.";
                return new OperationResultVo<GameCategory>(errorMessage);
            }
        }

        public async Task<OperationResultVo<GameCategoryViewModel>> GetById(Guid currentUserId, Guid id, bool forEdit)
        {
            try
            {
                GameCategory model = await mediator.Query<GetGameCategoryByIdQuery, GameCategory>(new GetGameCategoryByIdQuery(id));

                GameCategoryViewModel vm = mapper.Map<GameCategoryViewModel>(model);

                
                UserProfileEssentialVo authorProfile = await GetCachedEssentialProfileByUserId(vm.UserId);
 
                if (string.IsNullOrWhiteSpace(vm.LargeThumbImageUrl) || Constants.DefaultGameThumbnail.Contains(vm.LargeThumbImageUrl) || vm.LargeThumbImageUrl.Contains(Constants.DefaultGameThumbnail))
                {
                    vm.LargeThumbImageUrl = Constants.DefaultGameThumbnail;
                }

                return new OperationResultVo<GameCategoryViewModel>(vm);
            }
            catch (Exception ex)
            {
                return new OperationResultVo<GameCategoryViewModel>(ex.Message);
            }
        }
        public async Task<OperationResultVo> Remove(Guid currentUserId, Guid id)
        {
            try
            {

                await mediator.SendCommand(new DeleteGameCategoryCommand(currentUserId, id));

                return new OperationResultVo(true);
            }
            catch (Exception ex)
            {
                return new OperationResultVo(ex.Message);
            }
        }

        public async Task<List<GameCategoryViewModel>> GetListCategory(Guid currentUserId, int count, Guid userId, GameGenre genre)
        {
            IEnumerable<GameCategory> allModels = await mediator.Query<GetListGameCategoryQuery, IEnumerable<GameCategory>>(new GetListGameCategoryQuery(count, genre, userId));

            List<GameCategoryViewModel> vms = allModels.AsQueryable().ProjectTo<GameCategoryViewModel>(mapper.ConfigurationProvider).ToList();

            IEnumerable<Guid> userIds = allModels.Select(x => x.UserId);
            List<UserProfileEssentialVo> authorProfiles = await GetCachedEssentialProfilesByUserIds(userIds);

            foreach (GameCategoryViewModel item in vms)                                                                                                                           
            {
                item.LargeThumbImageUrl = SetFeaturedImage(item.UserId, item.LargeThumbImageUrl, ImageRenderType.Full);
                UserProfileEssentialVo authorProfile = authorProfiles.FirstOrDefault(x => x.UserId == item.UserId);
            }
            return vms;
        }

        public async Task<List<Game>> GetListGameOutstanding()
        {
            IEnumerable<Game> allModels = await mediator.Query<GetGameOutstandingQuery, IEnumerable<Game>>(new GetGameOutstandingQuery());

            List<Game> vms = mapper.Map<IEnumerable<Game>, IEnumerable<Game>>(allModels).ToList();

            IEnumerable<Guid> userIds = vms.Select(x => x.UserId);

            List<UserProfileEssentialVo> authorProfiles = await GetCachedEssentialProfilesByUserIds(userIds);
            foreach (Game item in vms)
            {
                item.ThumbnailUrl = SetFeaturedImage(item.UserId, item.ThumbnailUrl, ImageRenderType.Full);

                UserProfileEssentialVo authorProfile = authorProfiles.FirstOrDefault(x => x.UserId == item.UserId);
                if (authorProfile != null)
                {
                    item.DeveloperName = authorProfile?.Name;
                }
            }

            return vms;
        }


        

        public async Task<List<Game>> GetListGameMostplayers(List<Guid> id)
        {  
            IEnumerable<Game> allModels = await mediator.Query<GetGameTopLikeQuery, IEnumerable<Game>>(new GetGameTopLikeQuery(id));

            List<Game> vms = mapper.Map<IEnumerable<Game>, IEnumerable<Game>>(allModels).ToList();

            IEnumerable<Guid> userIds = vms.Select(x => x.UserId);

            List<UserProfileEssentialVo> authorProfiles = await GetCachedEssentialProfilesByUserIds(userIds);
            foreach (Game item in vms)
            {
                item.ThumbnailUrl = SetFeaturedImage(item.UserId, item.ThumbnailUrl, ImageRenderType.Full);

                UserProfileEssentialVo authorProfile = authorProfiles.FirstOrDefault(x => x.UserId == item.UserId);
                if (authorProfile != null)
                {
                    item.DeveloperName = authorProfile?.Name;
                }
            }

            return vms;
        }

        private static string SetFeaturedImage(Guid userId, string thumbnailUrl, ImageRenderType imageType)
        {
            if (string.IsNullOrWhiteSpace(thumbnailUrl) || Constants.DefaultGameThumbnail.NoExtension().Contains(thumbnailUrl.NoExtension()))
            {
                return Constants.DefaultGameThumbnail;
            }
            else
            {
                switch (imageType)
                {
                    case ImageRenderType.LowQuality:
                        return UrlFormatter.Image(userId, ImageType.GameThumbnail, thumbnailUrl, 278, 10);

                    case ImageRenderType.Responsive:
                        return UrlFormatter.Image(userId, ImageType.GameThumbnail, thumbnailUrl, true, 0, 0);

                    case ImageRenderType.Full:
                    default:
                        return UrlFormatter.Image(userId, ImageType.GameThumbnail, thumbnailUrl);
                }
            }
        }

        
    }
}