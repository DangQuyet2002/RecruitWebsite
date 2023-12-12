using LuduStack.Application.Interfaces;
using LuduStack.Domain.ValueObjects;
using LuduStack.Web.Controllers.Base;
using MetaG.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System;
using MetaG.Application.ViewModels;
using MetaG.Domain.Models;
using LuduStack.Application;
using LuduStack.Application.ViewModels.Game;
using LuduStack.Application.Services;
using LuduStack.Domain.Core.Attributes;
using LuduStack.Domain.Core.Enums;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
using LuduStack.Application.Formatters;
using LuduStack.Domain.Core.Extensions;
using Humanizer.Localisation;

namespace LuduStack.Web.Controllers
{
    [Authorize]
    public class GameCategoryController : SecureBaseController
    {
        private readonly IGameAppService gameAppService;
        private readonly INotificationAppService notificationAppService;
        private readonly ITeamAppService teamAppService;
        private readonly ILocalizationAppService translationAppService;
        private readonly IMetaGameAppService metaGameAppService;
        public GameCategoryController(IGameAppService gameAppService
            , INotificationAppService notificationAppService
            , ITeamAppService teamAppService
            , ILocalizationAppService translationAppService
            , IMetaGameAppService metaGameAppService)
        {
            this.gameAppService = gameAppService;
            this.notificationAppService = notificationAppService;
            this.teamAppService = teamAppService;
            this.translationAppService = translationAppService;
            this.metaGameAppService = metaGameAppService;
        }

        [AllowAnonymous]
        public async Task<IActionResult> List()
        {
            List<GameCategoryViewModel> categories = await metaGameAppService.GetListCategory(CurrentUserId, 200, Guid.Empty,0);

            ViewData["Categories"] = categories;

            return View();
        }

        [AllowAnonymous]
        public async Task<IActionResult> GetListCategories(GameGenre genre)
        {
            List<GameCategoryViewModel> categories = await metaGameAppService.GetListCategory(CurrentUserId, 200, Guid.Empty, genre);

            return View(categories);
        }



        [AllowAnonymous]
        public IActionResult Add()
        {
            OperationResultVo<GameCategoryViewModel> serviceResult = metaGameAppService.CreateNew(CurrentUserId);

            if (serviceResult.Success)
            {
                return View("CreateEdit", serviceResult.Value);
            }
            else
            {
                return View("CreateEdit", new GameCategoryViewModel());
            }
        }

        [AllowAnonymous]
        public async Task<IActionResult> Save(GameCategoryViewModel viewModel, IFormFile thumbnail)
        {
            try
            {
                bool isNew = viewModel.Id == Guid.Empty;

                if (string.IsNullOrWhiteSpace(viewModel.LargeThumbImageUrl) || Constants.DefaultGameThumbnail.Contains(viewModel.LargeThumbImageUrl) || viewModel.LargeThumbImageUrl.Contains(Constants.DefaultGameThumbnail))
                {
                    viewModel.LargeThumbImageUrl = Constants.DefaultGameThumbnail;
                    viewModel.CoverImageUrl = Constants.DefaultGameThumbnail;
                }

                OperationResultVo<GameCategory> saveResult = await metaGameAppService.Save(CurrentUserId , viewModel);
                
                if (!saveResult.Success)
                {
                    return Json(saveResult);
                }
                else
                {
                    return Json(saveResult);
                }
            }
            catch (Exception ex)
            {
                return Json(new OperationResultVo(ex.Message));
            }
        }

        [AllowAnonymous]
        public async Task<IActionResult> Edit(Guid id)
        {

            OperationResultVo<GameCategoryViewModel> serviceResult = await metaGameAppService.GetById(CurrentUserId, id, true);

            GameCategoryViewModel viewModel = serviceResult.Value;


            SetImages(viewModel);

            SetImagesToRefresh(viewModel, true);

            return View("CreateEdit", viewModel);

        }

        [AllowAnonymous]
        public async Task<IActionResult> Delete(Guid id)
        {
            OperationResultVo result = await metaGameAppService.Remove(CurrentUserId, id);

            if (result.Success)
            {
                result.Message = SharedLocalizer["Content deleted successfully!"];
            }
            else
            {
                result.Message = SharedLocalizer["Oops! The content was not deleted!"];
            }

            return Json(result);
        }

        private void SetImages(GameCategoryViewModel vm)
        {
            vm.LargeThumbImageUrl = string.IsNullOrWhiteSpace(vm.LargeThumbImageUrl) || Constants.DefaultGameThumbnail.NoExtension().Contains(vm.LargeThumbImageUrl.NoExtension()) ? Constants.DefaultGameThumbnail : UrlFormatter.Image(vm.UserId, ImageType.GameThumbnail, vm.LargeThumbImageUrl);

            vm.CoverImageUrl = string.IsNullOrWhiteSpace(vm.CoverImageUrl) || Constants.DefaultGameCoverImage.Contains(vm.CoverImageUrl) ? Constants.DefaultGameCoverImage : UrlFormatter.Image(vm.UserId, ImageType.GameCover, vm.CoverImageUrl);

        }

        private void SetImagesToRefresh(GameCategoryViewModel vm, bool refresh)
        {
            if (refresh)
            {
                vm.LargeThumbImageUrl = UrlFormatter.ReplaceCloudVersion(vm.LargeThumbImageUrl);
                vm.CoverImageUrl = UrlFormatter.ReplaceCloudVersion(vm.CoverImageUrl);
            }
        }
    }
}