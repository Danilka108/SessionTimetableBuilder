namespace Adapters.Project.Browser;

public abstract class BrowsingChange
{
    private BrowsingChange()
    {
    }

    public sealed class Browse : BrowsingChange
    {
        public Browse(IBrowserPage page)
        {
            Page = page;
        }

        public IBrowserPage Page { get; }
    }

    public sealed class Close : BrowsingChange
    {
        public Close(IBrowserPage page)
        {
            Page = page;
        }

        public IBrowserPage Page { get; }
    }
}