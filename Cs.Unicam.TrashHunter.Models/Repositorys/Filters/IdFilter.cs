using Cs.Unicam.TrashHunter.Models.Abstractions.Repositorys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Cs.Unicam.TrashHunter.Models.Repositorys.Filters
{
    internal class IdFilter<T> : IFilter where T : notnull
    {
        private readonly T _value;
        private readonly string _property;

        public IdFilter(T value, string property)
        {
            _value = value;
            _property = property;
        }
        public Expression<Func<T, bool>> GetFilter<T>()
        {
            var parameter = Expression.Parameter(typeof(T), "x");
            var property = Expression.Property(parameter, _property);
            var value = Expression.Constant(_value);
            var equals = Expression.Equal(property, value);
            return Expression.Lambda<Func<T, bool>>(equals, parameter);
        }
    }
}
