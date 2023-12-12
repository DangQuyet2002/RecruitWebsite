using LuduStack.Domain.Core.Enums;
using LuduStack.Domain.Messaging.Queries.Base;
using LuduStack.Infra.CrossCutting.Messaging;
using MediatR;
using MetaG.Domain.Interfaces.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MetaG.Domain.Messaging.Queries.GameCat
{
	public class GetListGameCategoryQuery : GetBaseQuery<Models.GameCategory>
	{
		public GameGenre Genre { get; }
		public Guid UserId { get; }
		public Guid? CategoryId { get; }
		public GetListGameCategoryQuery()
		{
		}
		public GetListGameCategoryQuery(Guid? categoryId)
		{
			CategoryId = categoryId;
		}

		public GetListGameCategoryQuery(Expression<Func<Models.GameCategory, bool>> where) : base(where)
		{
		}

		public GetListGameCategoryQuery(Expression<Func<Models.GameCategory, bool>> where, int take) : base(where, take)
		{
		}

		public GetListGameCategoryQuery(int take, GameGenre genre, Guid userId)
		{
			Take = take;
			Genre = genre;
			UserId = userId;
		}
	}


	public class GetListGameCategoryQueryHandler : QueryHandler, IRequestHandler<GetListGameCategoryQuery, IEnumerable<Models.GameCategory>>
	{
		protected readonly IGameCategoryRepository repository;

		public GetListGameCategoryQueryHandler(IGameCategoryRepository repository)
		{
			this.repository = repository;
		}

		public Task<IEnumerable<Models.GameCategory>> Handle(GetListGameCategoryQuery request, CancellationToken cancellationToken)
		{
			if (request.Where != null)
			{
				IQueryable<Models.GameCategory> result = repository.Get(request.Where);

				if (request.Take > 0)
				{
					result = result.Take(request.Take);
				}

				return Task.FromResult(result.AsEnumerable());
			}
			else
			{
				IQueryable<Models.GameCategory> allModels = repository.Get();

				if (request.Genre != 0)
				{
					allModels = allModels.Where(x => x.Genre == request.Genre);
				}

                if (request.UserId != Guid.Empty)
				{
					allModels = allModels.Where(x => x.UserId == request.UserId);
				}

				if (request.CategoryId.HasValue && request.CategoryId != Guid.Empty)
				{
					allModels = allModels.Where(x => x.Id == request.CategoryId.Value);
				}

				IOrderedQueryable<Models.GameCategory> orderedResult = allModels.OrderByDescending(x => x.CreateDate);

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
