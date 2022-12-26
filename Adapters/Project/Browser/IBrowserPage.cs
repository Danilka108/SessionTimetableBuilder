namespace Adapters.Project.Browser;

public interface IBrowserPage
{
    public string PageName { get; }

    public Task<bool> ConfirmPageClosingAsync();
}