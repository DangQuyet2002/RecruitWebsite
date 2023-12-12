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
	public class SaveGameCategoryCommand : BaseUserCommand
	{
		public GameCategory GameCategory { get; }

		public SaveGameCategoryCommand(GameCategory category) : base(category.UserId, category.Id)
		{
			GameCategory = category;
		}

		public override bool IsValid()
		{
			Result.Validation = new SaveGameCategoryCommandValidation().Validate(this);
			return Result.Validation.IsValid;
		}
	}

    public class SaveCategoryCommandHandler : CommandHandler, IRequestHandler<SaveGameCategoryCommand, CommandResult>
    {
        private readonly IMediatorHandler mediator;
        private readonly IUnitOfWork unitOfWork;
        private readonly IGameCategoryRepository gamecategoryRepository;
        private readonly IGamificationDomainService gamificationDomainService;
        private readonly ITeamDomainService teamDomainService;

        public SaveCategoryCommandHandler(IMediatorHandler mediator, IUnitOfWork unitOfWork, IGameCategoryRepository gamecategoryRepository, IGamificationDomainService gamificationDomainService, ITeamDomainService teamDomainService)
        {
            this.mediator = mediator;
            this.unitOfWork = unitOfWork;
            this.gamecategoryRepository = gamecategoryRepository;
            this.gamificationDomainService = gamificationDomainService;
            this.teamDomainService = teamDomainService;
        }

        public async Task<CommandResult> Handle(SaveGameCategoryCommand request, CancellationToken cancellationToken)
        {
            CommandResult result = request.Result;

            if (request.GameCategory.Id == Guid.Empty)
            {
                await gamecategoryRepository.Add(request.GameCategory);
                result.PointsEarned += gamificationDomainService.ProcessAction(request.UserId, PlatformAction.GameAdd);
            }
            else
            {
                gamecategoryRepository.Update(request.GameCategory);
            }

            result.Validation = await Commit(unitOfWork);

            return result;
        }
    }
}