using System.Net.Http.Json;
using RESTfull.Domain.Entities;

namespace RESTfull.Client.Services
{
    public class StudentService
    {
        private readonly HttpClient _http;

        public StudentService(HttpClient http)
        {
            _http = http;
            
        }

        public async Task<List<Student>> GetStudentsAsync()
        {
            try
            {
                Console.WriteLine("Attempting to fetch students...");
                var response = await _http.GetAsync("api/students");
                Console.WriteLine($"Response status: {response.StatusCode}");

                if (response.IsSuccessStatusCode)
                {
                    var students = await response.Content.ReadFromJsonAsync<List<Student>>();
                    Console.WriteLine($"Fetched {students?.Count ?? 0} students");
                    return students ?? new List<Student>();
                }
                else
                {
                    Console.WriteLine($"Error: {response.StatusCode}");
                    return new List<Student>();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching students: {ex.Message}");
                Console.WriteLine($"Stack trace: {ex.StackTrace}");
                return new List<Student>();
            }
        }

        public async Task<Student?> GetStudentByIdAsync(Guid id)
        {
            try
            {
                return await _http.GetFromJsonAsync<Student>($"api/students/{id}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching student: {ex.Message}");
                return null;
            }
        }


        public async Task<bool> CreateStudentAsync(string firstName, string lastName, string patronymic, string studentCardNumber)
        {
            try
            {
                var response = await _http.PostAsJsonAsync("api/students", new
                {
                    firstName,
                    lastName,
                    patronymic,
                    studentCardNumber
                });

                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error creating student: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> DeleteStudentAsync(Guid studentId)
        {
            try
            {
                var response = await _http.DeleteAsync($"api/students/{studentId}");
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting student: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> UpdateStudentAsync(Guid id, string firstName, string lastName, string patronymic, string studentCardNumber)
        {
            try
            {
                // Обновляем персональную информацию
                var personalInfoResponse = await _http.PutAsJsonAsync($"api/students/{id}/personal-info", new
                {
                    firstName,
                    lastName,
                    patronymic
                });

                if (!personalInfoResponse.IsSuccessStatusCode)
                {
                    return false;
                }

                var studentCardResponse = await _http.PutAsJsonAsync($"api/students/{id}/student-card", new
                {
                    studentCardNumber
                });

                return studentCardResponse.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating student: {ex.Message}");
                return false;
            }
        }


    }
}
