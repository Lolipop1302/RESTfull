using Microsoft.AspNetCore.Mvc;
using RESTfull.Domain.Entities;
using RESTfull.Domain.Interfaces;

namespace RESTfull.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EducationsController : ControllerBase
    {
        private readonly IEducationRepository _educationRepository;
        private readonly IStudentRepository _studentRepository;

        public EducationsController(IEducationRepository educationRepository, IStudentRepository studentRepository)
        {
            _educationRepository = educationRepository;
            _studentRepository = studentRepository;
        }

        // POST: api/educations
        [HttpPost]
        public async Task<ActionResult<object>> CreateEducation(CreateEducationDto request)
        {
            try
            {
                // Проверяем существование студента
                var student = await _studentRepository.GetByIdAsync(request.StudentId);
                if (student == null)
                {
                    return NotFound("Student not found");
                }

                var education = new Education(
                    request.Institution,
                    request.Faculty,
                    request.Specialty,
                    request.Profile,
                    request.Form,
                    request.Qualification,
                    request.Group,
                    request.StartYear)
                {
                    StudentId = request.StudentId,
                    EndYear = request.EndYear
                };

                var createdEducation = await _educationRepository.AddAsync(education);

                return Ok(new
                {
                    createdEducation.Id,
                    createdEducation.Institution,
                    createdEducation.Faculty,
                    createdEducation.Specialty,
                    createdEducation.Group,
                    message = "Education created successfully"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        // GET: api/educations/5/edit
        [HttpGet("{id}/edit")]
        public async Task<ActionResult<object>> GetEducationForEdit(Guid id)
        {
            try
            {
                var education = await _educationRepository.GetByIdAsync(id);
                if (education == null)
                {
                    return NotFound("Education not found");
                }

                var result = new
                {
                    education.Id,
                    education.Institution,
                    education.Faculty,
                    education.Specialty,
                    education.Profile,
                    education.Form,
                    education.Qualification,
                    education.Group,
                    education.StartYear,
                    education.EndYear,
                    education.StudentId
                };

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        // PUT: api/educations/5/status
        [HttpPut("{id}/status")]
        public async Task<IActionResult> UpdateStatus(Guid id, UpdateStatusDto request)
        {
            try
            {
                var education = await _educationRepository.GetByIdAsync(id);
                if (education == null)
                {
                    return NotFound();
                }

                education.ChangeStatus(request.Status);
                await _educationRepository.UpdateAsync(education);

                return NoContent();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating status: {ex.Message}");
                return StatusCode(500, new { error = "Произошла внутренняя ошибка сервера." });
            }
        }

        // PUT: api/educations/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateEducation(Guid id, UpdateEducationDto request)
        {
            try
            {
                var education = await _educationRepository.GetByIdAsync(id);
                if (education == null)
                {
                    return NotFound("Education not found");
                }

                education.UpdateAcademicInfo(request.Faculty, request.Specialty, request.Profile, request.Qualification, request.Group);
                education.UpdateEducationDates(request.StartYear, request.EndYear, request.Form);

                await _educationRepository.UpdateAsync(education);

                return Ok(new
                {
                    message = "Education updated successfully",
                    educationId = education.Id
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        // GET: api/educations/5
        [HttpGet("{id}")]
        public async Task<ActionResult<object>> GetEducation(Guid id)
        {
            var education = await _educationRepository.GetByIdAsync(id);
            if (education == null)
            {
                return NotFound();
            }

            var result = new
            {
                education.Id,
                education.Institution,
                education.Faculty,
                education.Specialty,
                education.Profile,
                education.Form,
                education.Qualification,
                education.Group,
                education.Status,
                education.StartYear,
                education.EndYear,
                education.StudentId
            };

            return result;
        }

        // DELETE: api/educations/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEducation(Guid id)
        {
            try
            {
                var result = await _educationRepository.DeleteAsync(id);
                if (!result)
                {
                    return NotFound("Education not found");
                }

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        // GET: api/educations/student/{studentId}
        [HttpGet("student/{studentId}")]
        public async Task<ActionResult<IEnumerable<object>>> GetEducationsByStudent(Guid studentId)
        {
            var educations = await _educationRepository.GetByStudentIdAsync(studentId);

            var result = educations.Select(e => new
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
                e.EndYear,
                e.StudentId
            }).ToList();

            return Ok(result);
        }
    }

    public class CreateEducationDto
    {
        public Guid StudentId { get; set; }
        public string Institution { get; set; } = string.Empty;
        public string Faculty { get; set; } = string.Empty;
        public string Specialty { get; set; } = string.Empty;
        public string Profile { get; set; } = string.Empty;
        public string Form { get; set; } = string.Empty;
        public string Qualification { get; set; } = string.Empty;
        public string Group { get; set; } = string.Empty;
        public int StartYear { get; set; }
        public int? EndYear { get; set; }
    }

    public class UpdateStatusDto
    {
        public string Status { get; set; } = string.Empty;
    }

    public class UpdateEducationDto
    {
        public string Faculty { get; set; } = string.Empty;
        public string Specialty { get; set; } = string.Empty;
        public string? Profile { get; set; }
        public string Form { get; set; } = string.Empty;
        public string? Qualification { get; set; }
        public string Group { get; set; } = string.Empty;
        public int StartYear { get; set; }
        public int? EndYear { get; set; }
    }
}