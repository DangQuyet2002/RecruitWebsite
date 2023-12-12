using LuduStack.Infra.Data.MongoDb.Interfaces;
using LuduStack.Infra.Data.MongoDb.Repository.Base;
using MetaG.Domain.Interfaces.Repository;
using MetaG.Domain.Models;

namespace MetaG.Data.Repository.Game
{
	public class MetaGameCategoryRepository : BaseRepository<GameCategory>, IGameCategoryRepository
	{
		public MetaGameCategoryRepository(IMongoContext context) : base(context)
		{
		}
	}
}