using System.Linq.Expressions;
using Tasker.Repository.Entity;

namespace Tasker.Repository.Interfaces;

public interface IBaseRepository<T> where T : class
{
    Task Add(T entity);
    Task Update(T entity);
    Task Delete(T entity);
    Task<T?> GetById(int id);
    Task<IEnumerable<T>> GetAll();
    Task<IEnumerable<T>> GetWhere(Expression<Func<T, bool>> predicate);
    Task SaveChanges();
}