using Cs.Unicam.TrashHunter.Models.Abstractions.Repositorys;
using Cs.Unicam.TrashHunter.Models.Entities;
using Cs.Unicam.TrashHunter.Models.Repositorys.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cs.Unicam.TrashHunter.Services.Services.Filters
{
    public class PostFilters
    {
        private readonly string? _title;
        private readonly string? _description;
        private readonly string? _city;
        private readonly string? _email;
        private readonly DateTime? _creationDate;

        public PostFilters()
        {

        }

        public PostFilters(string? title, string? description, string? city, string? email, DateTime? creationDate)
        {
            _title = title;
            _description = description;
            _city = city;
            _email = email;
            _creationDate = creationDate;
        }

        public IFilter Filter()
        {
            FilterBuilder builder = new FilterBuilder();

            return builder
                .AddStringFilter(_title, nameof(Post.Title))
                .AddStringFilter(_description, nameof(Post.Description))
                .AddStringFilter(_city, nameof(Post.City))
                .AddStringFilter(_email, nameof(Post.UserCode))
                .AddDateFilter(_creationDate, nameof(Post.CreationDate))
                .Build();
        }

        


    }
}
