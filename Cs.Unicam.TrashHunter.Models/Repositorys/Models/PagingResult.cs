using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cs.Unicam.TrashHunter.Models.Repositorys.Models
{
    public class PagingResult<T>
    {
        public int Page { get; set; }
        public int PageSize { get; set; }
        public int TotalPages { get; set; }
        public int TotalItems { get; set; }
        public IEnumerable<T> Items { get; set; }

        public PagingResult()
        {
            Items = new List<T>();
        }

        public PagingResult<T2> Select<T2>(Func<T, T2> selector)
        {
            return new PagingResult<T2>
            {
                Page = Page,
                PageSize = PageSize,
                TotalPages = TotalPages,
                TotalItems = TotalItems,
                Items = Items.Select(selector).ToList()
            };
        }
    }
}
