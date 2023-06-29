using FluentValidation;

namespace FamilyHubs.Notification.Core.Commands.CreateNotification;

public class CreateNotificationCommandValidator : AbstractValidator<CreateNotificationCommand>
{
    public CreateNotificationCommandValidator()
    {
        RuleFor(v => v.MessageDto)
            .NotNull();

        RuleFor(v => v.MessageDto.ApiKeyType).IsInEnum();

        RuleFor(v => v.MessageDto.NotificationEmails).Must(x => x != null && x.Any());

        RuleForEach(v => v.MessageDto.NotificationEmails)
            .NotNull()
            .WithMessage("Please fill all items");

        RuleFor(v => v.MessageDto.TemplateId)
            .NotEmpty()
            .NotNull();
    }
}
