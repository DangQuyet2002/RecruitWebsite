using LuduStack.Application.ViewModels.Game;
using LuduStack.Domain.Models;
using LuduStack.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetaG.Application.ViewModels
{
    public class DataHomeViewModel
    {
        public List<Game> MostPlayers { get; set; }
        public List<GameFeeViewModel> GetPaidGames { get; set; }
        public List<GameCategoryViewModel> ListCategory { get; set; }
        public List<Game> GameOutstanding { get; set; }
        public List<BannerViewModel> ListBanner { get; set; }
        public OperationResultListVo<int> CountUser { get; set; }

    }
}
