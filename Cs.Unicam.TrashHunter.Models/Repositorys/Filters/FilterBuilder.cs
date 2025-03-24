using Cs.Unicam.TrashHunter.Models.Abstractions.Repositorys;
using Cs.Unicam.TrashHunter.Models.Repositorys.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cs.Unicam.TrashHunter.Services.Services.Filters
{
    public class FilterBuilder
    {
        List<IFilter> filters = new List<IFilter>();

        public FilterBuilder AddStringFilter(string? value, string propertyName)
        {
            if (value != null)
                filters.Add(new StringFilter(value, propertyName));
            return this;
        }

        public FilterBuilder AddDateFilter(DateTime? value, string propertyName)
        {
            if (value != null)
                filters.Add(new DateFilter(value, propertyName));
            return this;
        }


        public FilterBuilder AddIdFilter<T>(T value, string popertyName) where T : notnull
        {
            filters.Add(new IdFilter<T>(value, popertyName));
            return this;
        }

        public FilterBuilder AddFilter(IFilter filter)
        {
            filters.Add(filter);
            return this;
        }

        public IFilter Build()
        {
            if (filters.Count == 0)
                return new EmptyFilter();
            return filters.Aggregate((f1, f2) => f1.And(f2));
        }
    }
}
