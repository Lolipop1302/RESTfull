using Microsoft.AspNetCore.Mvc;
using RESTfull.Domain.Entities;
using RESTfull.Domain.Interfaces;

namespace RESTfull.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StudentsController : ControllerBase
    {
        private readonly IStudentRepository _studentRepository;

        public StudentsController(IStudentRepository studentRepository)
        {
            _studentRepository = studentRepository;
        }

        // GET: api/students
        [HttpGet]
        public async Task<ActionResult<IEnumerable<object>>> GetStudents()
        {
            var students = await _studentRepository.GetAllAsync();

            var result = students.Select(s => new
            {
                s.Id,
                s.FirstName,
                s.LastName,
                s.Patronymic,
                s.StudentCardNumber,
                s.RegistrationDate,
                EducationsCount = s.Educations.Count
            }).ToList();

            return Ok(result);
        }

        // GET: api/students/5
        [HttpGet("{id}")]
        public async Task<ActionResult<object>> GetStudent(Guid id)
        {
            var student = await _studentRepository.GetByIdAsync(id);
            if (student == null)
            {
                return NotFound();
            }

            var result = new
            {
                student.Id,
                student.FirstName,
                student.LastName,
                student.Patronymic,
                student.StudentCardNumber,
                student.RegistrationDate,
                Educations = student.Educations.Select(e => new
                {
                    e.Id,
                    e.Institution,
                    e.Faculty,
                    e.Specialty,
                    e.Profile,
                    e.Form,
                    e.Qualification,
                    e.Group,
                    e.Status,
                    e.StartYear,
                    e.EndYear
                })
            };

            return result;
        }

        // POST: api/students
        [HttpPost]
        public async Task<ActionResult<object>> CreateStudent(CreateStudentDto request)
        {
            var existingStudent = await _studentRepository.GetByStudentCardAsync(request.StudentCardNumber);
            if (existingStudent != null)
            {
                return BadRequest("Ñòóäåíò ñ òàêèì íîìåðîì ñòóäåí÷åñêîãî óæå ñóùåñòâóåò");
            }

            var student = new Student(
                request.FirstName,
                request.LastName,
                request.Patronymic,
                request.StudentCardNumber);

            var createdStudent = await _studentRepository.AddAsync(student);

            return CreatedAtAction(nameof(GetStudent), new { id = createdStudent.Id }, new
            {
                createdStudent.Id,
                createdStudent.FirstName,
                createdStudent.LastName,
                createdStudent.StudentCardNumber,
                createdStudent.RegistrationDate
            });
        }

        // DELETE: api/students/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStudent(Guid id)
        {
            var result = await _studentRepository.DeleteAsync(id);
            if (!result)
            {
                return NotFound();
            }

            return NoContent();
        }

        // PUT: api/students/5/personal-info
        [HttpPut("{id}/personal-info")]
        public async Task<IActionResult> UpdatePersonalInfo(Guid id, UpdatePersonalInfoDto request)
        {
            var student = await _studentRepository.GetByIdAsync(id);
            if (student == null)
            {
                return NotFound();
            }

            student.UpdatePersonalInfo(request.FirstName, request.LastName, request.Patronymic);
            await _studentRepository.UpdateAsync(student);

            return NoContent();
        }

        // PUT: api/students/5/student-card
        [HttpPut("{id}/student-card")]
        public async Task<IActionResult> UpdateStudentCard(Guid id, UpdateStudentCardDto request)
        {
            var student = await _studentRepository.GetByIdAsync(id);
            if (student == null)
            {
                return NotFound();
            }

            var existingStudent = await _studentRepository.GetByStudentCardAsync(request.StudentCardNumber);
            if (existingStudent != null && existingStudent.Id != id)
            {
                return BadRequest("Ñòóäåíò ñ òàêèì íîìåðîì ñòóäåí÷åñêîãî óæå ñóùåñòâóåò");
            }

            student.UpdateStudentCard(request.StudentCardNumber);
            await _studentRepository.UpdateAsync(student);

            return NoContent();
        }
    }

    public class CreateStudentDto
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Patronymic { get; set; } = string.Empty;
        public string StudentCardNumber { get; set; } = string.Empty;
    }

    public class UpdatePersonalInfoDto
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Patronymic { get; set; } = string.Empty;
    }

    public class UpdateStudentCardDto
    {
        public string StudentCardNumber { get; set; } = string.Empty;
    }
}
