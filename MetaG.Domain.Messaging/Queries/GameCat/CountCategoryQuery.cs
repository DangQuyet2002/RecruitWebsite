using LuduStack.Domain.Messaging.Queries.Base;
using MetaG.Domain.Interfaces.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MetaG.Domain.Messaging.Queries.GameCat
{
    public class CountCategoryQuery : CountBaseQuery<Models.GameCategory>
    {
        public CountCategoryQuery()
        {
        }

        public CountCategoryQuery(Expression<Func<Models.GameCategory, bool>> where) : base(where)
        {
        }
    }

    public class CountCategoryQueryHandler : CountBaseQueryHandler<CountCategoryQuery, Models.GameCategory, IGameCategoryRepository>
    {
        public CountCategoryQueryHandler(IGameCategoryRepository repository) : base(repository)
        {
        }
    }
}

