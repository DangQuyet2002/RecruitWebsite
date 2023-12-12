using LuduStack.Domain.Interfaces.Repository;
using LuduStack.Domain.Messaging.Queries.Base;
using MetaG.Domain.Interfaces.Repository;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace MetaG.Domain.Messaging.Queries
{
    public class GetBannerQuery : GetBaseQuery<Models.Banner>
    {
        public GetBannerQuery()
        {
        }

        public GetBannerQuery(Expression<Func<Models.Banner, bool>> where) : base(where)
        {
        }
    }

    public class GetBannerQueryHandler : GetBaseQueryHandler<GetBannerQuery, Models.Banner, IBannerRepository>
    {
        public GetBannerQueryHandler(IBannerRepository repository) : base(repository)
        {
        }

        public new async Task<IEnumerable<Models.Banner>> Handle(GetBannerQuery request, CancellationToken cancellationToken)
        {
            IEnumerable<Models.Banner> all = await base.Handle(request, cancellationToken);

            return all;
        }
    }
}