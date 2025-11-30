namespace RESTfull.Domain.Entities
{
    public class Student
    {
        public Student()
        {
            Educations = new List<Education>();
        }

        public Student(string firstName, string lastName, string patronymic, string studentCardNumber)
        {
            Id = Guid.NewGuid();
            FirstName = firstName ?? throw new ArgumentNullException(nameof(firstName));
            LastName = lastName ?? throw new ArgumentNullException(nameof(lastName));
            Patronymic = patronymic;
            StudentCardNumber = studentCardNumber ?? throw new ArgumentNullException(nameof(studentCardNumber));
            RegistrationDate = DateTime.UtcNow;
            Educations = new List<Education>();
        }

        public Guid Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string? Patronymic { get; set; }
        public string StudentCardNumber { get; set; } = string.Empty;
        public DateTime RegistrationDate { get; set; }

        public virtual ICollection<Education> Educations { get; set; }

        public void UpdatePersonalInfo(string firstName, string lastName, string patronymic)
        {
            if (string.IsNullOrWhiteSpace(firstName))
                throw new ArgumentException("Имя не может быть пустым", nameof(firstName));

            if (string.IsNullOrWhiteSpace(lastName))
                throw new ArgumentException("Фамилия не может быть пустой", nameof(lastName));

            FirstName = firstName;
            LastName = lastName;
            Patronymic = patronymic;
        }

        public void UpdateStudentCard(string studentCardNumber)
        {
            if (string.IsNullOrWhiteSpace(studentCardNumber))
                throw new ArgumentException("Номер студенческого билета не может быть пустым", nameof(studentCardNumber));

            StudentCardNumber = studentCardNumber;
        }

        public void AddEducation(Education education)
        {
            if (education == null)
                throw new ArgumentNullException(nameof(education));

            Educations.Add(education);
        }

        public Education? GetCurrentEducation()
        {
            return Educations.FirstOrDefault(e => e.Status == "Обучается");
        }

        public bool HasActiveEducation()
        {
            return Educations.Any(e => e.Status == "Обучается");
        }

        public int GetTotalEducationYears()
        {
            return Educations.Sum(e => e.GetDuration());
        }
    }
}
