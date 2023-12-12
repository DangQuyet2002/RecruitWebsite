using LuduStack.Domain.Core.Enums;
using LuduStack.Domain.Interfaces.Repository;
using LuduStack.Domain.Messaging.Queries.Base;
using LuduStack.Domain.ValueObjects;
using LuduStack.Infra.CrossCutting.Messaging;
using MediatR;
using MetaG.Domain.Interfaces.Repository;
using MetaG.Domain.Messaging.Queries.GameCat;
using MetaG.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MetaG.Domain.Messaging.Queries.Game
{
    public class GetPaidGamesQuery : GetBaseQuery<GameFee>
    {
        public Guid UserId { get; }
        public int Take { get; }
        public Expression<Func<GameFee, bool>> Where { get; set; }

        public GetPaidGamesQuery()
        {
        }

        public GetPaidGamesQuery(Expression<Func<GameFee, bool>> where)
        {
            Where = where;
        }

        public GetPaidGamesQuery(Expression<Func<GameFee, bool>> where, int take)
        {
            Where = where;
            Take = take;
        }

        public GetPaidGamesQuery(int take)
        {
            Take = take;
        }
    }

    public class GetPaidGamesQueryHandler : QueryHandler, IRequestHandler<GetPaidGamesQuery, IEnumerable<GameFee>>
    {
        protected readonly IGameFeeRepository repository;
        protected readonly IUserProfileRepository userProfileRepository;
        protected readonly IGameRepository gameRepository;




        public GetPaidGamesQueryHandler(IGameFeeRepository repository, IUserProfileRepository userProfileRepository, IGameRepository gameRepository)
        {
            this.repository = repository;
            this.userProfileRepository = userProfileRepository;
            this.gameRepository = gameRepository;
        }

        public Task<IEnumerable<GameFee>> Handle(GetPaidGamesQuery request, CancellationToken cancellationToken)
        {
            if (request.Where != null)
            {
                IQueryable<GameFee> result = repository.Get(request.Where);

                if (request.Take > 0)
                {
                    result = result.Take(request.Take);
                }

                return Task.FromResult(result.AsEnumerable());
            }
            else
            {
                IQueryable<GameFee> allModels = repository.Get();

                if (request.UserId != Guid.Empty)
                {
                    allModels = allModels.Where(x => x.UserId == request.UserId);
                }

                IOrderedQueryable<GameFee> orderedResult = allModels.OrderByDescending(x => x.ApplyFrom);

                if (request.Take > 0)
                {
                    return Task.FromResult(orderedResult.Take(request.Take).AsEnumerable());
                }
                else
                {
                    return Task.FromResult(orderedResult.AsEnumerable());
                }
            }

        }
    }
}
