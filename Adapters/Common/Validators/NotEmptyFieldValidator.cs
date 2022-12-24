using ReactiveUI.Validation.Collections;
using ReactiveUI.Validation.States;

namespace Adapters.Common.Validators;

public class NotEmptyFieldValidator : IValidationState
{
    public delegate NotEmptyFieldValidator Factory(string value);

    public NotEmptyFieldValidator(string value, ILocalizedMessageConverter messageConverter)
    {
        var message = messageConverter.Convert(new LocalizedMessage.FieldError.CantBeEmpty());
        message = message[0].ToString().ToUpper() + message[1..];
        
        Text = ValidationText.Create(message);

        IsValid = value.Length > 0;
    }

    public IValidationText Text { get; }

    public bool IsValid { get; }
}