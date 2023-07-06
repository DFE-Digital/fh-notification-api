
namespace FamilyHubs.Notification.Api.Client.Templates;

//todo: helper to set as a singleton?
public interface INotificationTemplates<in T>
    where T : struct, Enum, IConvertible
{
    string GetTemplateId(T templateEnum);
}