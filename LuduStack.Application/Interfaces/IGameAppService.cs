﻿using LuduStack.Application.ViewModels;
using LuduStack.Application.ViewModels.Game;
using LuduStack.Domain.Core.Enums;
using LuduStack.Domain.Models;
using LuduStack.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LuduStack.Application.Interfaces
{
    public interface IGameAppService
    {
        Task<OperationResultVo<int>> Count(Guid currentUserId);

        Task<OperationResultListVo<Guid>> GetAllIds(Guid currentUserId);

        Task<OperationResultVo<Guid>> Save(Guid currentUserId, GameViewModel viewModel);

        Task<OperationResultVo<GameViewModel>> GetById(Guid currentUserId, Guid id);

        Task<OperationResultVo<GameViewModel>> GetById(Guid currentUserId, Guid id, bool forEdit);

        OperationResultVo<GameViewModel> CreateNew(Guid currentUserId);

        Task<IEnumerable<GameListItemViewModel>> GetLatest(Guid currentUserId, int count, Guid userId, Guid? teamId, GameGenre genre);

        Task<IEnumerable<SelectListItemVo>> GetByUser(Guid userId);

        Task<OperationResultVo> GameFollow(Guid currentUserId, Guid gameId);

        Task<OperationResultVo> GameUnfollow(Guid currentUserId, Guid gameId);

        Task<OperationResultVo> GameLike(Guid currentUserId, Guid gameId);

        Task<OperationResultVo> GameUnlike(Guid currentUserId, Guid gameId);
        Task<OperationResultVo> Comment(Guid currentUserId, CommentViewModel vm);
    }
}