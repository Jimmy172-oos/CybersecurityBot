using System;
using System.Collections.Generic;
using System.IO;

namespace CybersecurityBot
{
    public class DatabaseHelper
    {
        private string dataFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "tasks.txt");
        private List<Task> tasks = new List<Task>();

        public DatabaseHelper()
        {
            LoadTasks();
        }

        private void LoadTasks()
        {
            try
            {
                if (File.Exists(dataFile))
                {
                    string[] lines = File.ReadAllLines(dataFile);
                    tasks.Clear();
                    foreach (string line in lines)
                    {
                        if (!string.IsNullOrWhiteSpace(line))
                        {
                            string[] parts = line.Split('|');
                            if (parts.Length >= 4)
                            {
                                var task = new Task
                                {
                                    Id = int.Parse(parts[0]),
                                    Title = parts[1],
                                    Description = parts[2],
                                    IsCompleted = parts[3] == "True"
                                };
                                if (parts.Length > 4 && !string.IsNullOrEmpty(parts[4]))
                                {
                                    task.ReminderDate = DateTime.Parse(parts[4]);
                                }
                                tasks.Add(task);
                            }
                        }
                    }
                }
            }
            catch { tasks = new List<Task>(); }
        }

        private void SaveTasks()
        {
            try
            {
                List<string> lines = new List<string>();
                foreach (Task t in tasks)
                {
                    string reminder = t.ReminderDate.HasValue ? t.ReminderDate.Value.ToString() : "";
                    lines.Add($"{t.Id}|{t.Title}|{t.Description}|{t.IsCompleted}|{reminder}");
                }
                File.WriteAllLines(dataFile, lines);
            }
            catch { }
        }

        public void AddTask(string title, string description, DateTime? reminderDate)
        {
            int newId = 1;
            if (tasks.Count > 0)
            {
                newId = tasks[tasks.Count - 1].Id + 1;
            }

            var task = new Task
            {
                Id = newId,
                Title = title,
                Description = description ?? "",
                ReminderDate = reminderDate,
                IsCompleted = false
            };
            tasks.Add(task);
            SaveTasks();
        }

        public List<Task> GetTasks(bool onlyIncomplete = false)
        {
            if (onlyIncomplete)
                return tasks.FindAll(t => !t.IsCompleted);
            return tasks;
        }

        public void MarkTaskCompleted(int taskId)
        {
            var task = tasks.Find(t => t.Id == taskId);
            if (task != null)
            {
                task.IsCompleted = true;
                SaveTasks();
            }
        }

        public void DeleteTask(int taskId)
        {
            tasks.RemoveAll(t => t.Id == taskId);
            SaveTasks();
        }

        public string GetTasksAsString(bool onlyIncomplete = false)
        {
            var taskList = GetTasks(onlyIncomplete);
            if (taskList.Count == 0) return "You have no tasks.";

            string result = "";
            foreach (Task t in taskList)
            {
                string status = t.IsCompleted ? "COMPLETED" : "PENDING";
                string reminder = t.ReminderDate.HasValue ? $" Reminder: {t.ReminderDate.Value.ToShortDateString()}" : "";
                result += $"{t.Id}. [{status}] {t.Title}{reminder}\n";
            }
            return result;
        }
    }
}