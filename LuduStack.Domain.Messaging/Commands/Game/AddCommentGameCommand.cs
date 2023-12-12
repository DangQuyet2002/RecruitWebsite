using LuduStack.Domain.Interfaces;
using LuduStack.Domain.Interfaces.Repository;
using LuduStack.Domain.Messaging.Queries.UserContent;
using LuduStack.Domain.Models;
using LuduStack.Infra.CrossCutting.Messaging;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace LuduStack.Domain.Messaging
{
    public class AddCommentGameCommand : BaseUserCommand
    {
        public Guid? ParentCommentId { get; set; }

        public string Text { get; set; }
        public string StarRating { get; set; }


        public AddCommentGameCommand(Guid userId, Guid GameId, Guid? parentCommentId, string text , string starRating) : base(userId, GameId)
        {
            ParentCommentId = parentCommentId;
            Text = text;
            StarRating = starRating;
        }

        public override bool IsValid()
        {
            Result.Validation = new AddCommentGameCommandValidation().Validate(this);
            return Result.Validation.IsValid;
        }
    }

    public class AddCommentGameCommandHandler : CommandHandler, IRequestHandler<AddCommentGameCommand, CommandResult>
    {
        private readonly IMediatorHandler mediator;
        protected readonly IUnitOfWork unitOfWork;
        protected readonly IGameRepository repository;

        public AddCommentGameCommandHandler(IMediatorHandler mediator, IUnitOfWork unitOfWork, IGameRepository repository)
        {
            this.mediator = mediator;
            this.unitOfWork = unitOfWork;
            this.repository = repository;
        }

        public async Task<CommandResult> Handle(AddCommentGameCommand request, CancellationToken cancellationToken)
        {
            if (!request.IsValid()) { return request.Result; }

            Game userContent = await repository.GetById(request.Id);
            if (userContent is null)
            {
                AddError("The content doesn't exist.");

                return new CommandResult(ValidationResult);
            }

            bool commentAlreadyExists = await mediator.Query<CheckIfCommentExistsQuery, bool>(new CheckIfCommentExistsQuery(x => x.UserContentId == request.Id && x.UserId == request.UserId && x.Text.Equals(request.Text)));

            if (commentAlreadyExists)
            {
                AddError("Duplicated Comment");

                return new CommandResult(ValidationResult);
            }

            GameComment model = new GameComment
            {
                UserGameId = request.Id,
                ParentCommentId = request.ParentCommentId,
                Text = request.Text,
                UserId = request.UserId
            };

            await repository.AddComment(model);

            request.Result.Validation = await Commit(unitOfWork);

            return request.Result;
        }
    }
}