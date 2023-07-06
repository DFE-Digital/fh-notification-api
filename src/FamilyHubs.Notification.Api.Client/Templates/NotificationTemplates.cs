using System.Collections.Immutable;
using Microsoft.Extensions.Configuration;

namespace FamilyHubs.Notification.Api.Client.Templates;

public class NotificationTemplates<T> : INotificationTemplates<T>
    where T : struct, Enum, IConvertible
{
    private ImmutableDictionary<T, string> TemplateIds { get; }

    public NotificationTemplates(IConfiguration configuration)
    {
        //todo: use config exception
        TemplateIds = configuration.GetSection("Notification:TemplateIds").AsEnumerable(true)
            .ToImmutableDictionary(
                x => ConvertToEnum(x.Key),
                x => x.Value ?? throw new InvalidOperationException($"TemplateId is missing for {x.Key}"));
    }

    public string GetTemplateId(T templateEnum)
    {
        return TemplateIds[templateEnum];
    }

    private static T ConvertToEnum(string enumValueString)
    {
        if (Enum.TryParse(enumValueString, out T result))
        {
            return result;
        }
        throw new ArgumentException($"The template name '{enumValueString}' is not a valid representation of the {typeof(T).Name} enumeration.");
    }
}