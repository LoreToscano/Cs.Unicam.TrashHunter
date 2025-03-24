using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Cs.Unicam.TrashHunter.Models.Abstractions.Repositorys
{
    public interface IFilter
    {
        Expression<Func<T, bool>> GetFilter<T>();

        IFilter And(IFilter filter) => new CompositeFilter(this, filter, Expression.AndAlso);
        IFilter Or(IFilter filter) => new CompositeFilter(this, filter, Expression.OrElse);
    }
    public class CompositeFilter : IFilter
    {
        private readonly IFilter _left;
        private readonly IFilter _right;
        private readonly Func<Expression, Expression, BinaryExpression> _merge;

        public CompositeFilter(IFilter left, IFilter right, Func<Expression, Expression, BinaryExpression> merge)
        {
            _left = left;
            _right = right;
            _merge = merge;
        }

        public Expression<Func<T, bool>> GetFilter<T>()
        {
            var leftExpression = _left.GetFilter<T>();
            var rightExpression = _right.GetFilter<T>();

            var parameter = Expression.Parameter(typeof(T), "x");
            var body = _merge(
                Expression.Invoke(leftExpression, parameter),
                Expression.Invoke(rightExpression, parameter)
            );

            return Expression.Lambda<Func<T, bool>>(body, parameter);
        }

        public IFilter And(IFilter filter)
        {
            return new CompositeFilter(this, filter, Expression.AndAlso);
        }

        public IFilter Or(IFilter filter)
        {
            return new CompositeFilter(this, filter, Expression.OrElse);
        }
    }
}
