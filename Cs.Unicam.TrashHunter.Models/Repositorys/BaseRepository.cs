using Cs.Unicam.TrashHunter.Models.Abstractions.Repositorys;
using Cs.Unicam.TrashHunter.Models.DB;
using Cs.Unicam.TrashHunter.Models.Exceptions;
using Cs.Unicam.TrashHunter.Models.Repositorys.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Cs.Unicam.TrashHunter.Models.Repositorys
{
    /// <summary>
    /// Class for a generic repository the CRUD operations are implemented, so an entity can extend this class only if needed to add custom methods
    /// The only rule is that you must not provide access to DB Layer to the upper layers, so no DbSet or IQueryable should be exposed without 
    /// a method that wraps it
    /// </summary>
    /// <typeparam name="T">The entity</typeparam>
    public class BaseRepository<T> : IRepository<T> where T : class
    {
        protected readonly TrashHunterContext _context;
        protected DbSet<T> DbSet => _context.Set<T>();
        public BaseRepository(TrashHunterContext context)
        {
            _context = context;
        }

        public void Add(T entity)
        {
            DbSet.Add(entity);
        }

        public void Update(T entity)
        {
            DbSet.Update(entity);
        }

        public void Delete(T entity)
        {
            DbSet.Remove(entity);
        }

        public async Task<bool> Exist(object id)
        {
            return await DbSet.FindAsync(id) != null;
        }

        public async Task<T> Find(object id)
        {
            if (id == null)
                throw new ArgumentNullException("Id non valido");
            var result = await DbSet.FindAsync(id);
            //if (result == null)
            //    throw new UserException($"Elemento non trovato con id = {id}");
            return result;
        }

        public async Task<IEnumerable<T>> GetAll()
        {
            return await DbSet.AsNoTracking().ToListAsync();
        }


        public async Task Save()
        {
            await _context.SaveChangesAsync();
        }

        public async Task<PagingResult<T>> GetFiltered(IFilter filters, int skip, int take)
        {
            IQueryable<T> query = DbSet;
            query = query
                .Where(filters.GetFilter<T>())
                .Skip(skip)
                .Take(take)
                .AsNoTracking();
            return new PagingResult<T>()
            {
                Items = await query.ToListAsync(),
                Page = skip / take,
                PageSize = take,
                TotalItems = await DbSet.CountAsync(),
                TotalPages = (int)Math.Ceiling(await DbSet.CountAsync() / (double)take)
            };
        }
    }
}
