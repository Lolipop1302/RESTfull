using RESTfull.Domain.Entities;

namespace RESTfull.Domain.Interfaces
{
    public interface IEducationRepository
    {
        Task<Education> GetByIdAsync(Guid id);
        Task<IEnumerable<Education>> GetByStudentIdAsync(Guid studentId);
        Task<Education> AddAsync(Education education);
        Task<Education> UpdateAsync(Education education);
        Task<bool> DeleteAsync(Guid id);
        Task<bool> ExistsAsync(Guid id);
    }
}