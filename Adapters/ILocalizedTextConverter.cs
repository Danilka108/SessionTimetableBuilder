namespace Adapters;

public interface ILocalizedMessageConverter
{
    public string Convert(LocalizedMessage message);

    public string Convert(LocalizedMessage.Letter letter);
}