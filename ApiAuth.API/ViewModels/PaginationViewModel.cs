using System;
using System.Collections.Generic;

namespace ApiAuth.API.ViewModels
{
    public class PaginationViewModel
    {
        public int? CurrentPage { get; set; } = 1;
        public int Count { get; set; }
        public int PageSize { get; set; } = 10;

        public int TotalPages => (int)Math.Ceiling(decimal.Divide(Count, PageSize));

        public bool ShowPrevious => CurrentPage > 1;
        public bool ShowNext => CurrentPage < TotalPages;
        public bool ShowFirst => CurrentPage != 1;
        public bool ShowLast => CurrentPage != TotalPages && TotalPages >= 1;
    }

    public class PaginationWrapper<TEntity> where TEntity : class
    {
        public PaginationViewModel Paginator { get; set; }
        public List<TEntity> Data { get; set; }
    }
}
