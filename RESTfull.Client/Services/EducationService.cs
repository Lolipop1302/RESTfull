using System.Net.Http.Json;
using RESTfull.Domain.Entities;

namespace RESTfull.Client.Services
{
    public class EducationService
    {
        private readonly HttpClient _http;

        public EducationService(HttpClient http)
        {
            _http = http;
        }

        public async Task<bool> CreateEducationAsync(Guid studentId, string institution, string faculty, string specialty,
    string profile, string form, string qualification, string group, int startYear, int? endYear) // Добавьте endYear
        {
            try
            {
                var response = await _http.PostAsJsonAsync("api/educations", new
                {
                    studentId,
                    institution,
                    faculty,
                    specialty,
                    profile,
                    form,
                    qualification,
                    group,
                    startYear,
                    endYear // Добавьте это поле
                });

                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error creating education: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> UpdateEducationAsync(Guid id, string faculty, string specialty, string profile,
            string form, string qualification, string group, int startYear, int? endYear)
        {
            try
            {
                var response = await _http.PutAsJsonAsync($"api/educations/{id}", new
                {
                    faculty,
                    specialty,
                    profile,
                    form,
                    qualification,
                    group,
                    startYear,
                    endYear
                });

                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating education: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> UpdateEducationStatusAsync(Guid id, string status)
        {
            try
            {
                var response = await _http.PutAsJsonAsync($"api/educations/{id}/status", new
                {
                    status
                });

                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating education status: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> DeleteEducationAsync(Guid educationId)
        {
            try
            {
                var response = await _http.DeleteAsync($"api/educations/{educationId}");
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting education: {ex.Message}");
                return false;
            }
        }
    }
}