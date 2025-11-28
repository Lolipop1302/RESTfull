using RESTfull.Domain.Entities;

namespace RESTfull.Domain.Interfaces
{
    public interface IStudentRepository
    {
        Task<Student> GetByIdAsync(Guid id);
        Task<IEnumerable<Student>> GetAllAsync();
        Task<Student> AddAsync(Student student);
        Task<Student> UpdateAsync(Student student);
        Task<bool> DeleteAsync(Guid id);
        Task<Student> GetByStudentCardAsync(string studentCardNumber);
        Task<bool> ExistsAsync(Guid id);
    }
}