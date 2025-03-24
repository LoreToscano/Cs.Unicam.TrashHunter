using Cs.Unicam.TrashHunter.Models.Abstractions.Repositorys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Cs.Unicam.TrashHunter.Models.Repositorys.Filters
{
    internal class EmptyFilter : IFilter
    {
        public EmptyFilter()
        {
            
        }

        public Expression<Func<T, bool>> GetFilter<T>()
        {
            return Expression.Lambda<Func<T, bool>>(Expression.Constant(true), Expression.Parameter(typeof(T), "x"));
        }

        /// <summary>
        /// Returns a new filter that is the logical OR, because the filter is always true it returns a new EmptyFilter
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public IFilter Or(IFilter filter)
        {
            return new EmptyFilter();
        }
    }
}
