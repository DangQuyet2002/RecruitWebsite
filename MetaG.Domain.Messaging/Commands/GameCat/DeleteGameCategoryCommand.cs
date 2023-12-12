using FluentValidation;
using LuduStack.Domain.Core.Enums;
using LuduStack.Domain.Interfaces.Services;
using LuduStack.Domain.Interfaces;
using LuduStack.Domain.Messaging;
using LuduStack.Infra.CrossCutting.Messaging;
using MediatR;
using MetaG.Domain.Messaging.Validation;
using MetaG.Domain.Messaging.Validation.GameCat;
using MetaG.Domain.Models;
using MetaG.Domain.Interfaces.Repository;

namespace MetaG.Domain.Messaging.Commands.GameCat
{
    public class DeleteGameCategoryCommand : BaseUserCommand
    {
        public GameCategory GameCategory { get; }

        public DeleteGameCategoryCommand(Guid userId, Guid id) : base(userId, id)
        {
            
        }

        public override bool IsValid()
        {
            Result.Validation = new DeleteGameCategoryCommandValidation().Validate(this);
            return Result.Validation.IsValid;
        }
    }

    public class DeleteGameCategoryCommandHandler : CommandHandler, IRequestHandler<DeleteGameCategoryCommand, CommandResult>
    {
        private readonly IMediatorHandler mediator;
        private readonly IUnitOfWork unitOfWork;
        private readonly IGameCategoryRepository gamecategoryRepository;
        private readonly IGamificationDomainService gamificationDomainService;
        private readonly ITeamDomainService teamDomainService;


        public DeleteGameCategoryCommandHandler(IMediatorHandler mediator, IUnitOfWork unitOfWork, IGameCategoryRepository gamecategoryRepository, IGamificationDomainService gamificationDomainService, ITeamDomainService teamDomainService)
        {
            this.mediator = mediator;
            this.unitOfWork = unitOfWork;
            this.gamecategoryRepository = gamecategoryRepository;
            this.gamificationDomainService = gamificationDomainService;
            this.teamDomainService = teamDomainService;
        }

        public async Task<CommandResult> Handle(DeleteGameCategoryCommand request, CancellationToken cancellationToken)
        {

            GameCategory category = await gamecategoryRepository.GetById(request.Id);

            if (category is null)
            {
                AddError("The Forum Category doesn't exist.");
                return request.Result;
            }

            gamecategoryRepository.Remove(category.Id);

            FluentValidation.Results.ValidationResult validation = await Commit(unitOfWork);

            request.Result.Validation = validation;

            return request.Result;
        }
    }
}