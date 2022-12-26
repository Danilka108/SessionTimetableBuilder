namespace Adapters.Project.Browser;

public abstract class BrowsingChange
{
    private BrowsingChange()
    {
    }

    public sealed class Add : BrowsingChange
    {
        public Add(IBrowserPage page)
        {
            Page = page;
        }

        public IBrowserPage Page { get; }
    }

    public sealed class Browse : BrowsingChange
    {
        public Browse(IBrowserPage page)
        {
            Page = page;
        }

        public IBrowserPage Page { get; }
    }

    public sealed class Remove : BrowsingChange
    {
        public Remove(IBrowserPage page)
        {
            Page = page;
        }

        public IBrowserPage Page { get; }
    }
    
    public sealed class BrowseDefault : BrowsingChange
    {
    }
}