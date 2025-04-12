
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using SchoolRedisManagement.Data;
using SchoolRedisManagement.Interface;
using SchoolRedisManagement.Models;
using StackExchange.Redis;


namespace SchoolRedisManagement.Service
{
    public class StudentService : IStudentService
    {
        private readonly ApplicationDbContext _context;
        private readonly IDatabase _redisDb;
        public StudentService(ApplicationDbContext context , IConnectionMultiplexer redis)
        {
            _context = context;
            _redisDb = redis.GetDatabase();
            
        }

        public async Task<IEnumerable<Student>> GetAllAsync()
        {
            var cacheKey = "students:all";
            var cached = await _redisDb.StringGetAsync(cacheKey);
            if (cached.HasValue)
                return JsonSerializer.Deserialize<IEnumerable<Student>>(cached);

            var students = await _context.Students.Include(x => x.Teacher).ToListAsync();

            await _redisDb.StringSetAsync(cacheKey, JsonSerializer.Serialize(students));

            return students;
        }

        public async Task<Student> GetByIdAsync(int id)
        {
            var student = await _context.Students.Include(x => x.Teacher).FirstOrDefaultAsync(x => x.Id == id);

            return student;
        }

        public async Task AddAsync(Student student)
        {
            _context.Students.Add(student);
            await _context.SaveChangesAsync();
            await _redisDb.KeyDeleteAsync("students:all");
        }

        public async Task DeleteAsync(int id)
        {
            var student = await _context.Students.FindAsync(id);
            await _context.SaveChangesAsync();
            await _redisDb.KeyDeleteAsync("students:all");
        }
    }
}
