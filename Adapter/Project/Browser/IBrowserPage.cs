namespace Adapter.Project.Browser;

public interface IBrowserPage
{
    public string PageName { get; }

    public class Comparer : EqualityComparer<IBrowserPage>
    {
        public override bool Equals(IBrowserPage? x, IBrowserPage? y)
        {
            return x switch
            {
                null when y is null => true,
                not null when y is not null => x.PageName.Equals(y.PageName),
                _ => false
            };
        }

        public override int GetHashCode(IBrowserPage page)
        {
            return page.PageName.GetHashCode();
        }
    }
}