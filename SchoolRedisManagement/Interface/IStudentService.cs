using SchoolRedisManagement.Models;

namespace SchoolRedisManagement.Interface
{
    public interface IStudentService
    {
        Task<IEnumerable<Student>> GetAllAsync();
        Task<Student> GetByIdAsync(int id);
        Task AddAsync(Student student);
        Task DeleteAsync(int id);
    }
}
