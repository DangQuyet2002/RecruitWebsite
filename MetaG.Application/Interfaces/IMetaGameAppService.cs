using LuduStack.Application.Interfaces;
using LuduStack.Application.ViewModels.Game;
using LuduStack.Domain.Core.Enums;
using LuduStack.Domain.Models;
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
	public interface IMetaGameAppService : IGameAppService
	{
		/// <summary>
		/// Get all game categories or child category of given categoryId
		/// </summary>
		/// <param name="categoryId"></param>
		/// <returns></returns>
		Task<List<GameCategoryViewModel>> GetGameCategory(Guid? categoryId);
		/// <summary>
		/// Get Game Categories with top number of given games
		/// </summary>
		/// <param name="genre">Category type</param>
		/// <param name="nGames">Number of Games to be collected</param>
		/// <returns></returns>
		Task<List<GameViewModel>> GetGameOfCategory(GameGenre genre, int nGames);

        Task<OperationResultVo<int>> CountCategory(Guid currentUserId);
        OperationResultVo<GameCategoryViewModel> CreateNew(Guid currentUserId);
        Task<OperationResultVo<GameCategory>> Save(Guid currentUserId, GameCategoryViewModel viewModel);

        Task<OperationResultVo<GameCategoryViewModel>> GetById(Guid currentUserId, Guid id, bool forEdit);
        Task<OperationResultVo> Remove(Guid currentUserId, Guid id);
        Task<List<GameCategoryViewModel>> GetListCategory(Guid currentUserId, int count, Guid userId, GameGenre genre);
        Task<List<Game>> GetListGameOutstanding();
        Task<List<Game>> GetListGameMostplayers(List<Guid> gameId);
        Task<List<GameFeeViewModel>> GetPaidGames();
		Task<List<Game>> GetGameOfGameFee(Guid gameId);
    }
}
