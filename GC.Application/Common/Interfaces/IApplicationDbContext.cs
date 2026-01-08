using GlobalConnect.Domain.Models;

namespace GlobalConnect.Application.Common.Interfaces
{
    public interface IApplicationDbContext
    {
        Task<List<User>> ListAllAsync(string? searchFilter);
        Task<User> GetByIdAsync(int id);
        void Add(User entity);
        void Update(int id, User entity);
        void Delete(int id);
    }
}
