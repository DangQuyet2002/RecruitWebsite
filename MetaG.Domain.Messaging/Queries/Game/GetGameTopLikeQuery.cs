﻿using LuduStack.Domain.Core.Enums;
using LuduStack.Domain.Interfaces.Repository;
using LuduStack.Domain.Messaging.Queries.Base;
using LuduStack.Infra.CrossCutting.Messaging;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace LuduStack.Domain.Messaging.Queries.Game
{
    public class GetGameTopLikeQuery : GetBaseQuery<Models.Game>
    {
        public GameGenre Genre { get; }
        public Guid UserId { get; }
        public Guid? TeamId { get; }
        public List<Guid> GameId { get; }


        public GetGameTopLikeQuery()
        {
        }

        public GetGameTopLikeQuery(Expression<Func<Models.Game, bool>> where) : base(where)
        {
        }

        public GetGameTopLikeQuery(Expression<Func<Models.Game, bool>> where, int take) : base(where, take)
        {
        }

        public GetGameTopLikeQuery(int take, GameGenre genre, Guid userId, Guid? teamId)
        {
            Take = take;
            Genre = genre;
            UserId = userId;
            TeamId = teamId;
        }

        public GetGameTopLikeQuery(List<Guid> gameId)
        {
            GameId = gameId;
        }
    }

    public class GetGameTopLikeQueryHandler : QueryHandler, IRequestHandler<GetGameTopLikeQuery, IEnumerable<Models.Game>>
    {
        protected readonly IGameRepository repository;

        public GetGameTopLikeQueryHandler(IGameRepository repository)
        {
            this.repository = repository;
        }

        public Task<IEnumerable<Models.Game>> Handle(GetGameTopLikeQuery request, CancellationToken cancellationToken)
        {
            if (request.Where != null)
            {
                IQueryable<Models.Game> result = repository.Get(request.Where);

                if (request.Take > 0)
                {
                    result = result.Take(request.Take);
                }

                return Task.FromResult(result.AsEnumerable());
            }
            else
            {
                IQueryable<Models.Game> allModels = repository.Get();

                foreach (var gameIdToRemove in request.GameId)
                {
                    allModels = allModels.Where(x => x.Id != gameIdToRemove);
                }
                if (request.Genre != 0)
                {
                    allModels = allModels.Where(x => x.Genre == request.Genre);
                }

                if (request.UserId != Guid.Empty)
                {
                    allModels = allModels.Where(x => x.UserId == request.UserId);
                }

                if (request.TeamId.HasValue)
                {
                    allModels = allModels.Where(x => x.TeamId == request.TeamId);
                }

                IOrderedQueryable<Models.Game> orderedResult = allModels.OrderByDescending(x => x.Likes);

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