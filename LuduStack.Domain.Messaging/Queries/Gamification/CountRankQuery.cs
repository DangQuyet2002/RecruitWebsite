using LuduStack.Domain.Interfaces.Repository;
using LuduStack.Domain.Messaging.Queries.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace LuduStack.Domain.Messaging.Queries.Gamification
{
    public class CountRankQuery : CountBaseQuery<Models.Gamification>
    {
        public CountRankQuery()
        {
        }

        public CountRankQuery(Expression<Func<Models.Gamification, bool>> where) : base(where)
        {
        }
    }

    public class CountRankQueryHandler : CountBaseQueryHandler<CountRankQuery, Models.Gamification, IGamificationRepository>
    {
        public CountRankQueryHandler(IGamificationRepository repository) : base(repository)
        {
        }
    }
}
