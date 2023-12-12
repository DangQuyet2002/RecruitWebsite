using LuduStack.Application.ViewModels;
using LuduStack.Application.ViewModels.Game;
using LuduStack.Domain.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetaG.Application.ViewModels
{
	public class GameCategoryViewModel : BaseViewModel 
    {
		public GameCategoryViewModel()
		{
			LatestGames = new List<GameViewModel>();
		}

		public Guid ParentId { get; set; }
		public GameGenre Genre { get; set; }
		public string Name { get; set; } = null!;
		public string? Description { get; set; }
		public string? CoverImageUrl { get; set; }
		public string? LargeThumbImageUrl { get; set; }
        public string? GameThumbnail { get; set; }

        public string? IconUrl { get; set; }
		public bool IsHidden { get; set; }
		public int SortOrder { get; set; }

		public IList<GameViewModel> LatestGames { get; set; }
	}
}
