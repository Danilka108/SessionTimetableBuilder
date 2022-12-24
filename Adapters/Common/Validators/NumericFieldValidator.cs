using ReactiveUI.Validation.Collections;
using ReactiveUI.Validation.States;

namespace Adapters.Common.Validators;

public class NumericFieldValidator : IValidationState
{
    public delegate NumericFieldValidator Factory(string value);

    public NumericFieldValidator(string value, ILocalizedMessageConverter messageConverter)
    {
        var separator = messageConverter.Convert(new LocalizedMessage.FieldError.Separator());
        var emptyFieldMessage =
            messageConverter.Convert(new LocalizedMessage.FieldError.CantBeEmpty()); 
        var invalidNumericStringMessage =
            messageConverter.Convert(new LocalizedMessage.FieldError.InvalidNumericString());

        var errors = new List<string>();

        if (value.Length == 0) errors.Add(emptyFieldMessage);
        
        try
        {
            int.Parse(value);
        }
        catch (Exception)
        {
            errors.Add(invalidNumericStringMessage);
        }
        
        IsValid = errors.Count == 0;

        var fullMessage = string.Join(separator, errors);
        fullMessage = IsValid ? "" : fullMessage[0].ToString().ToUpper() + fullMessage[1..];

        Text = ValidationText.Create(fullMessage);
    }

    public IValidationText Text { get; }

    public bool IsValid { get; }
}