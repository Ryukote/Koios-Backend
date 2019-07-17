using System.Collections.Generic;

namespace KoiosOffers.ViewModels
{
    public class PagedViewModel<T>
    {
        public int TotalCount { get; set; }
        public IEnumerable<T> Results { get; set; }
        public int PageNumber { get; set; }
    }
}
