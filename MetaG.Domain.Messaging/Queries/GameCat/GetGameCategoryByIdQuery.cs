using LuduStack.Domain.Interfaces.Repository;
using LuduStack.Domain.Messaging.Queries.Base;
using MetaG.Domain.Interfaces.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetaG.Domain.Messaging.Queries.GameCat
{
    public class GetGameCategoryByIdQuery : GetByIdBaseQuery<Models.GameCategory>
    {
        public GetGameCategoryByIdQuery(Guid id) : base(id)
        {
        }
    }

    public class GetGameCategoryByIdQueryHandler : GetByIdBaseQueryHandler<GetGameCategoryByIdQuery, Models.GameCategory, IGameCategoryRepository>
    {
        public GetGameCategoryByIdQueryHandler(IGameCategoryRepository repository) : base(repository)
        {
        }
    }
}