using LuduStack.Domain.Interfaces.Repository;
using LuduStack.Domain.Messaging.Queries.Base;
using MetaG.Domain.Interfaces.Repository;
using System;

namespace MetaG.Domain.Messaging.Queries
{
    public class GetBannerByIdQuery : GetByIdBaseQuery<Models.Banner>
    {
        public GetBannerByIdQuery(Guid id) : base(id)
        {
        }
    }

    public class GetBannerByIdQueryHandler : GetByIdBaseQueryHandler<GetBannerByIdQuery, Models.Banner, IBannerRepository>
    {
        public GetBannerByIdQueryHandler(IBannerRepository bannerRepository) : base(bannerRepository)
        {
        }
    }
}