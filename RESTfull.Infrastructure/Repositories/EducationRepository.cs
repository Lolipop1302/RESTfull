using Microsoft.EntityFrameworkCore;
using RESTfull.Domain.Entities;
using RESTfull.Domain.Interfaces;
using RESTfull.Infrastructure.Data;

namespace RESTfull.Infrastructure.Repositories
{
    public class EducationRepository : IEducationRepository
    {
        private readonly Context _context;

        public EducationRepository(Context context)
        {
            _context = context;
        }

        public async Task<Education> GetByIdAsync(Guid id)
        {
            return await _context.Educations
                .Include(e => e.Student)
                .FirstOrDefaultAsync(e => e.Id == id);
        }

        public async Task<IEnumerable<Education>> GetByStudentIdAsync(Guid studentId)
        {
            return await _context.Educations
                .Where(e => e.StudentId == studentId)
                .ToListAsync();
        }

        public async Task<Education> AddAsync(Education education)
        {
            if (education.Id == Guid.Empty)
                education.Id = Guid.NewGuid();

            await _context.Educations.AddAsync(education);
            await _context.SaveChangesAsync();
            return education;
        }

        public async Task<Education> UpdateAsync(Education education)
        {
            _context.Educations.Update(education);
            await _context.SaveChangesAsync();
            return education;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var education = await _context.Educations.FindAsync(id);
            if (education == null)
                return false;

            _context.Educations.Remove(education);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ExistsAsync(Guid id)
        {
            return await _context.Educations.AnyAsync(e => e.Id == id);
        }
    }
}