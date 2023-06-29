using FamilyHubs.Notification.Api.Contracts;

namespace FamilyHubs.Notification.Data.Entities
{
    public class SentNotification : EntityBase<long>
    {
        public required ApiKeyType ApiKeyType { get; set; }
        public required string TemplateId { get; set; } = default!;
        public required virtual  IList<Notified> Notified { get; set; }
        public virtual IList<TokenValue> TokenValues { get; set; } = default!;
    }
}
