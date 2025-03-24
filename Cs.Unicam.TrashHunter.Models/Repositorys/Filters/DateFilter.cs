using Cs.Unicam.TrashHunter.Models.Abstractions.Repositorys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Cs.Unicam.TrashHunter.Models.Repositorys.Filters
{
    internal class DateFilter : IFilter
    {
        private readonly DateTime? _date;
        private readonly string _prop;
        public DateFilter(DateTime? date, string property)
        {
            _date = date;
            _prop = property;
        }

        public Expression<Func<T, bool>> GetFilter<T>()
        {
            var parameter = Expression.Parameter(typeof(T), "x");
            var property = Expression.Property(parameter, _prop);
            var value = Expression.Constant(_date, typeof(DateTime?));
            Expression body = Expression.Equal(property, value);
            return Expression.Lambda<Func<T, bool>>(body, parameter);
        }
    }
}
