using Microsoft.EntityFrameworkCore;
using RESTfull.Domain.Entities;
using RESTfull.Domain.Interfaces;
using RESTfull.Infrastructure.Data;

namespace RESTfull.Infrastructure.Repositories
{
    public class StudentRepository : IStudentRepository
    {
        private readonly Context _context;

        public StudentRepository(Context context)
        {
            _context = context;
        }

        public async Task<Student> GetByIdAsync(Guid id)
        {
            return await _context.Students
                .Include(s => s.Educations)
                .FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task<IEnumerable<Student>> GetAllAsync()
        {
            return await _context.Students
                .Include(s => s.Educations)
                .ToListAsync();
        }

        public async Task<Student> AddAsync(Student student)
        {
            if (student.Id == Guid.Empty)
                student.Id = Guid.NewGuid();

            await _context.Students.AddAsync(student);
            await _context.SaveChangesAsync();
            return student;
        }

        public async Task<Student> UpdateAsync(Student student)
        {
            _context.Students.Update(student);
            await _context.SaveChangesAsync();
            return student;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var student = await _context.Students.FindAsync(id);
            if (student == null)
                return false;

            _context.Students.Remove(student);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<Student> GetByStudentCardAsync(string studentCardNumber)
        {
            return await _context.Students
                .FirstOrDefaultAsync(s => s.StudentCardNumber == studentCardNumber);
        }

        public async Task<bool> ExistsAsync(Guid id)
        {
            return await _context.Students.AnyAsync(s => s.Id == id);
        }
    }
}