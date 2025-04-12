using SchoolRedisManagement.Models;

namespace SchoolRedisManagement.Interface
{
    public interface ITeacherService
    {
        Task<IEnumerable<Teacher>> GetAllAsync();
        Task<Teacher> GetByIdAsync(int id);
        Task AddAsync(Teacher teacher);
        Task DeleteAsync(int id);
    }
}
