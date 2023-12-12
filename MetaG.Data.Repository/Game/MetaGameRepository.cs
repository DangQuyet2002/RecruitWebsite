using LuduStack.Infra.Data.MongoDb.Interfaces;
using LuduStack.Infra.Data.MongoDb.Repository;
using System.Linq.Expressions;

namespace MetaG.Data.Repository.Game
{
	public class MetaGameRepository : GameRepository
	{
		public MetaGameRepository(IMongoContext context) : base(context)
		{
		}
	}
}