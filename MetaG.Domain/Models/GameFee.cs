using LuduStack.Domain.Core.Models;
using LuduStack.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetaG.Domain.Models
{
    public enum FeeType
    {
        PerDuration = 0,
        Permanent = 1,
    }
    public enum CurrencyType
    {
        Fiat = 0,
        Coin = 1,
    }

    public class GameFee : Entity
    {
        public Guid GameId { get; set; }
        public Currency Currency { get; set; } = new Currency();
        public decimal Amount { get; set; }
        public FeeType FeeType { get; set; } = FeeType.Permanent;
        public DateTime? ApplyFrom { get; set; }
        public DateTime? ApplyTo { get; set; }
    }

    public class Currency : Entity
    {
        public Currency() { }

        public string Name { get; set; }
        public string Code { get; set; } 
        public CurrencyType CurrencyType { get; set; } = CurrencyType.Fiat;

    }
}
