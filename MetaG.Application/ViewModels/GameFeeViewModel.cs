using LuduStack.Application.ViewModels;
using LuduStack.Application.ViewModels.Game;
using LuduStack.Domain.Core.Models;
using LuduStack.Domain.Models;
using MetaG.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetaG.Application.ViewModels
{
    public class GameFeeViewModel : BaseViewModel
    {
        public GameFeeViewModel()
        {
            new List<Game>();
        }

        public Guid GameId { get; set; }
        public Currency Currency { get; set; }
        public int CurrencyType { get; set; }
        public decimal Amount { get; set; }
        public FeeType FeeType { get; set; }
        public DateTime? ApplyFrom { get; set; }
        public DateTime? ApplyTo { get; set; }
        public IList<Game> RelatedGames { get; set; }

    }
}
