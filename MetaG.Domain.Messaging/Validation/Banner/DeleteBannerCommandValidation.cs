using LuduStack.Domain.Messaging;

namespace MetaG.Domain.Messaging.Validation.Banner
{
    public class DeleteBannerCommandValidation : BaseCommandValidation<DeleteBannerCommand>
    {
        public DeleteBannerCommandValidation()
        {
            ValidateId();
        }
    }
}