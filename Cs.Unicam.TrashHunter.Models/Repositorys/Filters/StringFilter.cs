using Cs.Unicam.TrashHunter.Models.Abstractions.Repositorys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Cs.Unicam.TrashHunter.Models.Repositorys.Filters
{
    internal class StringFilter : IFilter
    {
        private readonly string? _value;
        private readonly string _property;

        public StringFilter(string? value, string property)
        {
            _value = value;
            _property = property;
        }

        public Expression<Func<T, bool>> GetFilter<T>()
        {
            var parameter = Expression.Parameter(typeof(T), "x");
            var property = Expression.Property(parameter, _property);
            var value = Expression.Constant(_value);
            var contains = Expression.Call(property, "Contains", Type.EmptyTypes, value);
            return Expression.Lambda<Func<T, bool>>(contains, parameter);
        }

    }
}
