using LuduStack.Application;
using LuduStack.Application.Formatters;
using LuduStack.Application.Interfaces;
using LuduStack.Application.Services;
using LuduStack.Application.ViewModels.Content;
using LuduStack.Application.ViewModels.Game;
using LuduStack.Application.ViewModels.User;
using LuduStack.Domain.Core.Enums;
using LuduStack.Domain.Models;
using LuduStack.Domain.ValueObjects;
using LuduStack.Infra.CrossCutting.Identity.Models;
using LuduStack.Web.Controllers.Base;
using LuduStack.Web.Extensions;
using MetaG.Application.Interfaces;
using MetaG.Application.Services;
using MetaG.Application.ViewModels;
using MetaG.Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LuduStack.Web.Controllers
{
    public class BannerController : SecureBaseController
    {
        private readonly IBannerAppService bannerAppService;
        private readonly IUserContentAppService userContentAppService;
        private readonly IGameAppService gameAppService;
        private readonly INotificationAppService notificationAppService;

        public BannerController(IUserContentAppService userContentAppService,IBannerAppService bannerAppService, IGameAppService gameAppService, INotificationAppService notificationAppService)
        {
            this.bannerAppService = bannerAppService;
            this.gameAppService = gameAppService;
            this.notificationAppService = notificationAppService;
            this.userContentAppService = userContentAppService;
        }

        [AllowAnonymous]
        public async Task<IActionResult> List()
        {
            OperationResultListVo<BannerViewModel> latest = await bannerAppService.GetAll();

            ViewData["Banner"] = latest.Value;

            return View();
        }

        public async Task<IActionResult> Add(Guid? gameId)
        {
            BannerViewModel vm = new BannerViewModel
            {
                UserId = CurrentUserId,
                FeaturedImage = Constants.DefaultFeaturedImage
            };

            return View("CreateEdit", vm);
        }

        [HttpPost]
        public async Task<IActionResult> Save(BannerViewModel viewModel)
        {
            if (!CurrentUserIsAdmin && viewModel.UserId != CurrentUserId)
            {
                return RedirectToAction("details", "content", new { viewModel.Id, msg = SharedLocalizer["You cannot edit someone else's content!"] });
            }

            try
            {
                bool isNew = viewModel.Id == Guid.Empty;
                OperationResultVo<Guid> saveResult = await bannerAppService.Save(CurrentUserId, viewModel);

                if (!saveResult.Success)
                {
                    return Json(saveResult);
                }
                else
                {
                    if (isNew && EnvName.Equals(Constants.ProductionEnvironmentName))
                    {  
                        await NotificationSender.SendTeamNotificationAsync("New complex post!");
                    }
                    return Json(saveResult);
                }
            }
            catch (Exception ex)
            {
                return Json(new OperationResultVo(ex.Message));
            }
        }


        public async Task<IActionResult> Edit(Guid id)
        {
            OperationResultVo<BannerViewModel> serviceResult = await bannerAppService.GetById(CurrentUserId, id);

            BannerViewModel viewModel = serviceResult.Value;

            if (!viewModel.HasFeaturedImage)
            {
                viewModel.FeaturedImage = Constants.DefaultFeaturedImage;
            }

            return View("CreateEdit", viewModel);
        }

        public async Task<IActionResult> Delete(Guid id)
        {
            OperationResultVo result = await bannerAppService.Remove(CurrentUserId, id);

            if (result.Success)
            {
                result.Message = SharedLocalizer["Banner deleted successfully!"];
            }
            else
            {
                result.Message = SharedLocalizer["Oops! The Banner was not deleted!"];
            }

            return RedirectToAction("List", "Banner");
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("banner/getallbanner")]
        public async Task<IActionResult> GetAllBanner() 
        {
            try
            {
                OperationResultListVo<BannerViewModel> latest = await bannerAppService.GetAll();

                var data = latest.Value;

                return Json(data);
            }
            catch (Exception ex)
            {
                return Json(new OperationResultListVo<BannerViewModel>(ex.Message));
            }
        }
    }
}
