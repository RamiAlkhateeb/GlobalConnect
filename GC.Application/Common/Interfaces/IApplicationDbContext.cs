using Domain.Models;
using GlobalConnect.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlobalConnect.Domain.Interfaces
{
    public interface IApplicationDbContext
    {
        Task<List<User>> ListAllAsync(string? searchFilter);
        Task<User> GetByIdAsync(int id);
        void Add(User entity);
        void Update(int id ,User entity);
        void Delete(int id);
    }
}
