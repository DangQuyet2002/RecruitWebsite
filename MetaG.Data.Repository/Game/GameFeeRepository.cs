using LuduStack.Infra.Data.MongoDb.Interfaces;
using LuduStack.Infra.Data.MongoDb.Repository.Base;
using MetaG.Domain.Interfaces.Repository;
using MetaG.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetaG.Data.Repository.Game
{
    public class GameFeeRepository : BaseRepository<GameFee>, IGameFeeRepository
    {
        public GameFeeRepository(IMongoContext context) : base(context)
        {
        }
    }
}
