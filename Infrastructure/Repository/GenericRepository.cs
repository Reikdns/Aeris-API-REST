using System.Linq.Expressions;
using Domain.Entities.Base;
using Domain.Repository;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repository;

public sealed class GenericRepository<T> : IGenericRepository<T> where T : DomainEntity
{
	private readonly AerisContext _context;

	public GenericRepository(AerisContext context)
	{
		_context = context ?? throw new ArgumentNullException(nameof(context), "Context not available");
	}

	public async Task<T> CreateAsync(T entity)
	{
		_context.Set<T>().Add(entity);
		await _context.CommitAsync();
		return entity;
	}

	public async Task<T> UpdateAsync(T entity)
	{
		_context.Set<T>().Update(entity);
		await _context.CommitAsync();
		return entity;
	}

	public async Task DeleteAsync(T entity)
	{
		_context.Set<T>().Remove(entity);
		await _context.CommitAsync();
	}

	public async Task<T?> FindAsync(object id)
	{
		return await _context.Set<T>().FindAsync(id);
	}

	public async Task<T?> FindAsync(Expression<Func<T, bool>>? filter = null,
		string includedStringProperties = "")
	{
		IQueryable<T> query = _context.Set<T>();
		if (filter != null)
			query = query.Where(filter);
		if (!string.IsNullOrEmpty(includedStringProperties))
		{
			query = includedStringProperties.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
				.Aggregate(query, (current, includeProperty) => current.Include(includeProperty));
		}

		return await query.FirstOrDefaultAsync();
	}

	public async Task<bool> ExistsAsync(object id)
	{
		return await _context.Set<T>().FindAsync(id) != null;
	}


	public async Task<IEnumerable<T>> GetAsync(Expression<Func<T, bool>>? filter = null,
		Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null, bool isTracking = false,
		string includedStringProperties = "")
	{
		IQueryable<T> query = _context.Set<T>();
		if (filter != null)
			query = query.Where(filter);
		if (!string.IsNullOrEmpty(includedStringProperties))
		{
			query = includedStringProperties.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
				.Aggregate(query, (current, includeProperty) => current.Include(includeProperty));
		}

		if (orderBy != null)
			return orderBy(query);
		return isTracking ? await query.ToListAsync() : query.AsNoTracking();
	}
}