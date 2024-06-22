﻿using PayZe.Payment.Infrastructure.Persistence;
using PayZe.Payment.Infrastructure.Repositories.Repositories.Abstractions;
using PayZe.Shared.Abstractions;
using PayZe.Shared.Enums;
using System.Linq.Expressions;

namespace PayZe.Payment.Infrastructure.Repositories.Repositories
{
    public class EfRepository<TEntity> : IRepository<TEntity>
        where TEntity : Entity
    {
        private readonly PaymentDbContext _db;
        private readonly IQueryable<TEntity> _baseQuery;

        public EfRepository(PaymentDbContext db, IServiceProvider serviceProvider)
        {
            _db = db;
            _baseQuery = _db.Set<TEntity>().AsQueryable();
        }


        public virtual IQueryable<TEntity> Query(Expression<Func<TEntity, bool>>? expression = null, bool onlyActives = true)
        {
            var query = onlyActives
                            ? _baseQuery.Where(x => x.EntityStatus == EntityStatus.Active)
                            : _baseQuery;

            return expression == null ? query : query.Where(expression);
        }

        public virtual async Task Store(TEntity document)
        {
            await _db.Set<TEntity>().AddAsync(document);
        }
    }
}