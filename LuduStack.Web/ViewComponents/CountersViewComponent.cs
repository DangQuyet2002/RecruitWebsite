using LuduStack.Application.Interfaces;
using LuduStack.Application.ViewModels.Home;
using LuduStack.Domain.ValueObjects;
using LuduStack.Web.ViewComponents.Base;
using MetaG.Application.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace LuduStack.Web.ViewComponents
{
    public class CountersViewComponent : BaseViewComponent
    {
        private readonly IGameAppService gameAppService;
        private readonly IMetaGameAppService metagameAppService;
        private readonly IProfileAppService profileAppService;
        private readonly IUserContentAppService contentService;
        private readonly ITeamAppService teamAppService;

        public CountersViewComponent(IHttpContextAccessor httpContextAccessor
            , IMetaGameAppService metaGameAppService
            , IGameAppService gameAppService
            , IProfileAppService profileAppService
            , IUserContentAppService contentService
            , ITeamAppService teamAppService) : base(httpContextAccessor)
        {
            this.metagameAppService = metaGameAppService;
            this.gameAppService = gameAppService;
            this.profileAppService = profileAppService;
            this.contentService = contentService;
            this.teamAppService = teamAppService;
        }

        public async Task<IViewComponentResult> InvokeAsync(bool placeholder)
        {
            CountersViewModel model = new CountersViewModel();

            if (placeholder)
            {
                ViewData["placeholder"] = true;
            }
            else
            {
                OperationResultVo<int> gamesCount = await gameAppService.Count(CurrentUserId);

                if (gamesCount.Success)
                {
                    model.GamesCount = gamesCount.Value;
                }

                OperationResultVo<int> usersCount = await profileAppService.Count(CurrentUserId);

                if (usersCount.Success)
                {
                    model.UsersCount = usersCount.Value;
                }

                OperationResultVo<int> categoryCount = await metagameAppService.CountCategory(CurrentUserId);

                if (categoryCount.Success)
                {
                    model.CategoryCount = categoryCount.Value;
                }

                model.ArticlesCount = await contentService.CountArticles();

                OperationResultVo<int> teamCount = await teamAppService.Count(CurrentUserId);

                if (teamCount.Success)
                {
                    model.TeamCount = teamCount.Value;
                }
            }

            return await Task.Run(() => View(model));
        }
    }
}