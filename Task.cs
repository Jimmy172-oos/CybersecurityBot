using System;

namespace CybersecurityBot
{
    public class Task
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime? ReminderDate { get; set; }
        public bool IsCompleted { get; set; }

        public Task()
        {
            Title = string.Empty;
            Description = string.Empty;
        }

        public Task(string title, string description, DateTime? reminderDate = null)
        {
            Title = title;
            Description = description ?? string.Empty;
            ReminderDate = reminderDate;
            IsCompleted = false;
        }

        public override string ToString()
        {
            string status = IsCompleted ? "✅ COMPLETED" : "⏳ PENDING";
            string reminder = ReminderDate.HasValue ? $" 📅 Reminder: {ReminderDate.Value.ToShortDateString()}" : "";
            return $"{Id}. [{status}] {Title} - {Description}{reminder}";
        }
    }
}