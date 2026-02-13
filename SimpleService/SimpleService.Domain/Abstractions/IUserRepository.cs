using System.Linq.Expressions;
using SimpleService.Domain.Entities;
using SimpleService.Domain.Models;

namespace SimpleService.Domain.Abstractions;

public interface IUserRepository
{
    int UsersCount { get; }
    
    Task<int> Add(User user);
    Task<User?> GetByFilter(Expression<Func<UserEntity, bool>> predicate);
    Task<IEnumerable<User>> GetAllByFilter(Expression<Func<UserEntity, bool>> predicate);
    Task Update(int userId, Action<UserEntity> updateAction);
    Task Remove(int userId);
    
    Task<DateOnly?> GetMinRegistrationDate();
    Task<DateOnly?> GetMaxRegistrationDate();
    Task<IEnumerable<User>> GetAllByRange(int startIndex, int range);
}