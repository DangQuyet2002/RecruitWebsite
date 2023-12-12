using FluentValidation;
using LuduStack.Domain.Messaging;
using MetaG.Domain.Messaging.Commands.GameCat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetaG.Domain.Messaging.Validation.Banner
{
    public class SaveBannerCommandValidation: BaseCommandValidation<SaveBannerCommand>
    {
        public SaveBannerCommandValidation()
        {
            ValidateEntity();
        }

        protected void ValidateEntity()
        {
            RuleFor(c => c.Banner.Id)
                .NotNull()
                .WithMessage("No Game to save.");
        }
    }
}
