namespace SchoolRedisManagement.Models
{
    public class Student
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public int TeacherId { get; set; }

        public Teacher Teacher { get; set; }
        
    }
}
