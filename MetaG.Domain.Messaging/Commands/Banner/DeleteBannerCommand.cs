using LuduStack.Domain.Interfaces;
using LuduStack.Domain.Interfaces.Repository;
using LuduStack.Infra.CrossCutting.Messaging;
using MediatR;
using MetaG.Domain.Interfaces.Repository;
using MetaG.Domain.Messaging.Validation.Banner;
using MetaG.Domain.Models;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace LuduStack.Domain.Messaging
{
    public class DeleteBannerCommand : BaseUserCommand
    {
        public DeleteBannerCommand(Guid userId, Guid id) : base(userId, id)
        {
        }

        public override bool IsValid()
        {
            Result.Validation = new DeleteBannerCommandValidation().Validate(this);
            return Result.Validation.IsValid;
        }
    }

    public class DeleteBannerCommandHandler : CommandHandler, IRequestHandler<DeleteBannerCommand, CommandResult>
    {
        protected readonly IUnitOfWork unitOfWork;
        protected readonly IBannerRepository bannerRepository;

        public DeleteBannerCommandHandler(IUnitOfWork unitOfWork, IBannerRepository bannerRepository)
        {
            this.unitOfWork = unitOfWork;
            this.bannerRepository = bannerRepository;
        }

        public async Task<CommandResult> Handle(DeleteBannerCommand request, CancellationToken cancellationToken)
        {
            if (!request.IsValid()) { return request.Result; }

            Banner userContent = await bannerRepository.GetById(request.Id);

            if (userContent is null)
            {
                AddError("The Banner doesn't exist.");

                return new CommandResult(ValidationResult);
            }

            bannerRepository.Remove(userContent.Id);

            FluentValidation.Results.ValidationResult validation = await Commit(unitOfWork);

            request.Result.Validation = validation;

            return request.Result;
        }
    }
}