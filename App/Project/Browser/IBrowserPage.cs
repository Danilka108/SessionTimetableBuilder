using System.Collections.Generic;

namespace App.Project.Browser;

public interface IBrowserPage
{
    public string Name { get; }

    public class Comparer : EqualityComparer<IBrowserPage>
    {
        public override bool Equals(IBrowserPage? x, IBrowserPage? y)
        {
            return x switch
            {
                null when y is null => true,
                not null when y is not null => x.Name.Equals(y.Name),
                _ => false
            };
        }

        public override int GetHashCode(IBrowserPage page)
        {
            return page.Name.GetHashCode();
        }
    }
}