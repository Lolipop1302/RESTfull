namespace RESTfull.Domain.Entities
{
    public class Education
    {
        public Education() { }

        public Education(string institution, string faculty, string specialty, string? profile,
                        string form, string? qualification, string group, int startYear)
        {
            Id = Guid.NewGuid();
            Institution = institution ?? throw new ArgumentNullException(nameof(institution));
            Faculty = faculty ?? throw new ArgumentNullException(nameof(faculty));
            Specialty = specialty ?? throw new ArgumentNullException(nameof(specialty));
            Profile = profile;
            Form = form ?? throw new ArgumentNullException(nameof(form));
            Qualification = qualification;
            Group = group ?? throw new ArgumentNullException(nameof(group));
            Status = "Обучается";
            StartYear = startYear;
        }

        public Guid Id { get; set; }
        public string Institution { get; set; } = string.Empty;
        public string Faculty { get; set; } = string.Empty;
        public string Specialty { get; set; } = string.Empty;
        public string? Profile { get; set; }
        public string Form { get; set; } = string.Empty;
        public string? Qualification { get; set; }
        public string Group { get; set; } = string.Empty;
        public string Status { get; set; } = "Обучается";
        public int StartYear { get; set; }
        public int? EndYear { get; set; }

        public Guid StudentId { get; set; }

        // УБЕДИТЕСЬ ЧТО ЭТО ВИРТУАЛЬНОЕ
        public virtual Student Student { get; set; } = null!;

        public void UpdateAcademicInfo(string faculty, string specialty, string profile, string qualification, string group)
        {
            Faculty = faculty;
            Specialty = specialty;
            Profile = profile;
            Qualification = qualification;
            Group = group;
        }

        public void ChangeStatus(string status)
        {
            Status = status;
        }

        public void UpdateEducationDates(int startYear, int? endYear, string form)
        {
            StartYear = startYear;
            EndYear = endYear;
            Form = form;
        }

        public int GetDuration()
        {
            if (!EndYear.HasValue)
                return DateTime.UtcNow.Year - StartYear;
            return EndYear.Value - StartYear;
        }
    }
}