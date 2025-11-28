using System;

namespace RESTfull.Domain.Events
{
    public abstract class DomainEvent
    {
        public DateTime OccurredOn { get; } = DateTime.UtcNow;
        public Guid EventId { get; } = Guid.NewGuid();
    }

    public class StudentPersonalInfoUpdatedEvent : DomainEvent
    {
        public Guid StudentId { get; }
        public string OldFirstName { get; }
        public string NewFirstName { get; }
        public string OldLastName { get; }
        public string NewLastName { get; }

        public StudentPersonalInfoUpdatedEvent(Guid studentId, string oldFirstName, string newFirstName, string oldLastName, string newLastName)
        {
            StudentId = studentId;
            OldFirstName = oldFirstName;
            NewFirstName = newFirstName;
            OldLastName = oldLastName;
            NewLastName = newLastName;
        }
    }

    public class StudentStatusChangedEvent : DomainEvent
    {
        public Guid StudentId { get; }
        public Guid EducationId { get; }
        public string OldStatus { get; }
        public string NewStatus { get; }

        public StudentStatusChangedEvent(Guid studentId, Guid educationId, string oldStatus, string newStatus)
        {
            StudentId = studentId;
            EducationId = educationId;
            OldStatus = oldStatus;
            NewStatus = newStatus;
        }
    }
}
