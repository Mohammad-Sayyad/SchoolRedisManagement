using Microsoft.EntityFrameworkCore;
using SchoolRedisManagement.Models;
using System.Collections.Generic;

namespace SchoolRedisManagement.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Student> Students { get; set; }
        public DbSet<Teacher> Teachers { get; set; }
    }
}
