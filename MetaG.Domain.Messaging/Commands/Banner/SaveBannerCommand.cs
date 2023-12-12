using FluentValidation.Results;
using LuduStack.Domain.Core.Enums;
using LuduStack.Domain.Interfaces;
using LuduStack.Domain.Interfaces.Repository;
using LuduStack.Domain.Interfaces.Services;
using LuduStack.Domain.Models;
using LuduStack.Infra.CrossCutting.Messaging;
using MediatR;
using MetaG.Domain.Interfaces.Repository;
using MetaG.Domain.Messaging.Validation.Banner;
using MetaG.Domain.Models;
using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace LuduStack.Domain.Messaging
{
    public class SaveBannerCommand : BaseUserCommand
    {
        public Banner Banner { get; }

        public bool IsComplex { get; }

        public Poll Poll { get; }

        public SaveBannerCommand(Guid userId, Banner banner) : base(userId, banner.Id)
        {
            Banner = banner;
        }

        public override bool IsValid()
        {
            Result.Validation = new SaveBannerCommandValidation().Validate(this);
            return Result.Validation.IsValid;
        }
    }

    public class SaveBannerCommandHandler : CommandHandler, IRequestHandler<SaveBannerCommand, CommandResult>
    {
        protected readonly IMediatorHandler mediator;
        protected readonly IUnitOfWork unitOfWork;
        protected readonly IBannerRepository bannerRepository;
        protected readonly IGamificationDomainService gamificationDomainService;

        public SaveBannerCommandHandler(IMediatorHandler mediator, IUnitOfWork unitOfWork, IBannerRepository bannerRepository, IGamificationDomainService gamificationDomainService)
        {
            this.mediator = mediator;
            this.unitOfWork = unitOfWork;
            this.bannerRepository = bannerRepository;
            this.gamificationDomainService = gamificationDomainService;
        }

        public async Task<CommandResult> Handle(SaveBannerCommand request, CancellationToken cancellationToken)
        {
            CommandResult result = request.Result;

            if (!request.IsValid()) { return request.Result; }

            string youtubePattern = @"(https?\:\/\/)?(www\.youtube\.com|youtu\.?be)\/.+";

            request.Banner.Description = Regex.Replace(request.Banner.Description, youtubePattern, delegate (Match match)
            {
                string v = match.ToString();
                if (match.Index == 0 && string.IsNullOrWhiteSpace(request.Banner.FeaturedImage))
                {
                    request.Banner.FeaturedImage = v;
                }
                return v;
            });

            if (request.Banner.Id == Guid.Empty)
            {
                request.Banner.UserId = request.UserId;

                await bannerRepository.Add(request.Banner);
            }
            else
            {
                bannerRepository.Update(request.Banner);
            }

            result.Validation = await Commit(unitOfWork);

            return result;

        }
    }
}