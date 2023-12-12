using LuduStack.Domain.Interfaces.Services;
using LuduStack.Domain.ValueObjects;
using MetaG.Application.ViewModels;
using MetaG.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetaG.Application.Interfaces
{
    public interface IBannerAppService : IProfileBaseAppService
    {
        Task<OperationResultListVo<BannerViewModel>> GetAll();
        Task<OperationResultVo<BannerViewModel>> GetById(Guid currentUserId, Guid id);
        Task<OperationResultVo<Guid>> Save(Guid currentUserId, BannerViewModel viewModel);
        Task<OperationResultVo> Remove(Guid currentUserId, Guid id);
    }
}
