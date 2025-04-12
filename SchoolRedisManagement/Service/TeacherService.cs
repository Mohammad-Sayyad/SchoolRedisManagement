using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using SchoolRedisManagement.Data;
using SchoolRedisManagement.Interface;
using SchoolRedisManagement.Models;
using StackExchange.Redis;

namespace SchoolRedisManagement.Service
{
    public class TeacherService : ITeacherService
    {
        private readonly ApplicationDbContext _context;
        private readonly IDatabase _redisDb;
        public TeacherService(ApplicationDbContext context, IConnectionMultiplexer redis)
        {
            _context = context;
            _redisDb = redis.GetDatabase();
        }
        public async Task AddAsync(Teacher teacher)
        {
            _context.Teachers.Add(teacher);
            await _context.SaveChangesAsync();
            await _redisDb.KeyDeleteAsync("teachers:all");
        }

        public async Task DeleteAsync(int id)
        {
            var student = await _context.Teachers.FindAsync(id);
            await _context.SaveChangesAsync();
            await _redisDb.KeyDeleteAsync("teachers:all");
        }

        public async Task<IEnumerable<Teacher>> GetAllAsync()
        {
            var cacheKey = "teachers:all";
            var cached = await _redisDb.StringGetAsync(cacheKey);
            if (cached.HasValue)
                return JsonSerializer.Deserialize<IEnumerable<Teacher>>(cached);
            var students = await _context.Teachers.Include(x => x.Students).ToListAsync();

            await _redisDb.StringSetAsync(cacheKey, JsonSerializer.Serialize(students));

            return students;
        }

        public async Task<Teacher> GetByIdAsync(int id)
        {
            var teacher = await _context.Teachers.Include(X => X.Students).FirstOrDefaultAsync();

            return teacher;
        }
    }
}
