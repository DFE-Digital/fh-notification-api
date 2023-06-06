namespace FamilyHubs.Notification.Data.Entities
{
    public class SentNotification : EntityBase<long>
    {
        public required string RecipientEmail { get; set; }
        public required string TemplateId { get; set; } = default!;
        public virtual IList<TokenValue> TokenValues { get; set; } = default!;
    }
}
