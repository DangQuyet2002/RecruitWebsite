using FluentValidation;
using LuduStack.Domain.Messaging;
using MetaG.Domain.Messaging.Commands.GameCat;

namespace MetaG.Domain.Messaging.Validation.GameCat
{
    public class DeleteGameCategoryCommandValidation : BaseCommandValidation<DeleteGameCategoryCommand>
    {
        public DeleteGameCategoryCommandValidation()
        {
            ValidateEntity();
        }

        protected void ValidateEntity()
        {
            RuleFor(c => c.GameCategory)
                .NotNull()
                .WithMessage("No Category to deleter.");
        }

    }
}
