namespace Adapters.Project.Browser;

public abstract class BrowsingChange
{
    private BrowsingChange()
    {
    }

    public sealed class Browse : BrowsingChange
    {
        public IBrowserPage Page { get; }

        public Browse(IBrowserPage page)
        {
            Page = page;
        }
    }

    public sealed class Close : BrowsingChange
    {
        public IBrowserPage Page { get; }

        public Close(IBrowserPage page)
        {
            Page = page;
        }
    }
}
