using FluentValidation;
using LuduStack.Domain.Messaging;
using MetaG.Domain.Messaging.Commands.GameCat;

namespace MetaG.Domain.Messaging.Validation.GameCat
{
	public class SaveGameCategoryCommandValidation : BaseCommandValidation<SaveGameCategoryCommand>
	{
		public SaveGameCategoryCommandValidation()
		{
			ValidateEntity();
			ValidateContent();
		}

		protected void ValidateContent()
		{

			RuleFor(c => c.GameCategory.Name)
				.NotNull().NotEmpty()
				.WithMessage("Category Name is required.")
				.MinimumLength(8)
				.WithMessage("Category Name must tyoe at least 8 characters!");
		}

		protected void ValidateEntity()
		{
			RuleFor(c => c.GameCategory)
				.NotNull()
				.WithMessage("No Game to save.");
		}

	}
}
