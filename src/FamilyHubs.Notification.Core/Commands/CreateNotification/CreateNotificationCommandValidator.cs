using FluentValidation;

namespace FamilyHubs.Notification.Core.Commands.CreateNotification;

public class CreateNotificationCommandValidator : AbstractValidator<CreateNotificationCommand>
{
    public CreateNotificationCommandValidator()
    {
        RuleFor(v => v.MessageDto)
            .NotNull();

        RuleFor(v => v.MessageDto.RecipientEmail)
            .NotEmpty()
            .NotNull();

        RuleFor(v => v.MessageDto.TemplateId)
            .NotEmpty()
            .NotNull();
    }
}
