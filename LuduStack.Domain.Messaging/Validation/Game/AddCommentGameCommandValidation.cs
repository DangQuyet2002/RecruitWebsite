using FluentValidation;
using LuduStack.Domain.Messaging;

namespace LuduStack.Domain.Messaging
{
    public class AddCommentGameCommandValidation : BaseUserCommandValidation<AddCommentGameCommand>
    {
        public AddCommentGameCommandValidation()
        {
            ValidateId();
            ValidateUserId();
            ValidateText();
        }

        protected void ValidateText()
        {
            RuleFor(c => c.Text)
                .NotEmpty()
                .WithMessage("You can't send an empty comment.");
        }
    }
}