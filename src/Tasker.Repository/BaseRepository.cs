using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Tasker.Repository.Context;
using Tasker.Repository.Interfaces;

namespace Tasker.Repository;

public class BaseRepository<T> : IBaseRepository<T>, IDisposable where T : class
{
    private readonly DatabaseContext _databaseContext;

    public BaseRepository(DatabaseContext databaseContext)
    {
        _databaseContext = databaseContext ?? throw new ArgumentNullException(nameof(databaseContext));
    }

    public async Task Add(T entity)
    {
        await _databaseContext.Set<T>().AddAsync(entity);
        await SaveChanges();
    }

    public async Task Update(T entity)
    {
        var existingEntity = await _databaseContext.Set<T>().FindAsync(GetId(entity));

        if (existingEntity == null)
        {
            throw new InvalidOperationException("Entity not found");
        }

        _databaseContext.Entry(existingEntity).CurrentValues.SetValues(entity);
        await SaveChanges();
    }

    public async Task Delete(T entity)
    {
        _databaseContext.Set<T>().Remove(entity);
        await SaveChanges();
    }

    public async Task<T?> GetById(int id)
    {
        return await _databaseContext.Set<T>().FindAsync(id);
    }

    public async Task<IEnumerable<T>> GetAll()
    {
        return await _databaseContext.Set<T>().ToListAsync();
    }

    public async Task<IEnumerable<T>> GetWhere(Expression<Func<T, bool>> predicate)
    {
        return await _databaseContext.Set<T>().Where(predicate).ToListAsync();
    }

    private int GetId(T entity)
    {
        var idProperty = typeof(T).GetProperty("Id");

        if (idProperty == null)
            throw new InvalidOperationException($"Entity of type {typeof(T).Name} does not have an Id property.");

        if (idProperty.GetValue(entity) is not int idValue)
            throw new InvalidOperationException($"Id property of entity {typeof(T).Name} is not of type int.");

        return idValue;
    }

    public async Task SaveChanges()
    {
        await _databaseContext.SaveChangesAsync();
    }

    public void Dispose()
    {
        _databaseContext.Dispose();
    }
}