using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace CybersecurityBot
{
    public class MainForm : Form
    {
        // Controls
        private RichTextBox chatDisplay;
        private TextBox userInputBox;
        private Button sendButton;
        private Label titleLabel;
        private Label asciiLabel;
        private TextBox nameInputBox;
        private Button startButton;
        private Panel namePanel;
        private Panel chatPanel;

        // Logic
        private Response response = new Response();
        private SentimentDetector sentimentDetector = new SentimentDetector();
        private RandomResponses randomResponses = new RandomResponses();
        private string userName = "";
        private string lastTopic = "";

        // NEW: Database, Activity Log, NLP, and Quiz
        private DatabaseHelper db = new DatabaseHelper();
        private ActivityLog activityLog = new ActivityLog();
        private NLPHandler nlp = new NLPHandler();
        private QuizForm quizForm = null;

        public MainForm()
        {
            SetupForm();
            PlayVoiceGreeting();
        }

        private void SetupForm()
        {
            // Form settings
            this.Text = "Cybersecurity Awareness Bot";
            this.Size = new Size(800, 650);
            this.BackColor = Color.FromArgb(15, 15, 30);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;

            // ASCII Logo label
            asciiLabel = new Label();
            asciiLabel.Text =
                "  ____      _                      \n" +
                " / ___|   _| |__   ___ _ __ ___   \n" +
                "| |  | | | | '_ \\ / _ \\ '__/ __|  \n" +
                "| |__| |_| | |_) |  __/ |  \\__ \\  \n" +
                " \\____\\__, |_.__/ \\___|_|  |___/  \n" +
                "       |___/  Awareness Bot         ";
            asciiLabel.Font = new Font("Courier New", 9, FontStyle.Bold);
            asciiLabel.ForeColor = Color.Cyan;
            asciiLabel.BackColor = Color.Transparent;
            asciiLabel.Location = new Point(20, 10);
            asciiLabel.Size = new Size(760, 110);
            asciiLabel.AutoSize = false;

            // Title label
            titleLabel = new Label();
            titleLabel.Text = "~ Cybersecurity Awareness Bot ~";
            titleLabel.Font = new Font("Arial", 12, FontStyle.Bold);
            titleLabel.ForeColor = Color.LightCyan;
            titleLabel.BackColor = Color.Transparent;
            titleLabel.TextAlign = ContentAlignment.MiddleCenter;
            titleLabel.Location = new Point(20, 120);
            titleLabel.Size = new Size(745, 30);

            // Name panel - shown first
            namePanel = new Panel();
            namePanel.Location = new Point(20, 160);
            namePanel.Size = new Size(745, 80);
            namePanel.BackColor = Color.Transparent;

            Label nameLabel = new Label();
            nameLabel.Text = "Please enter your name to begin:";
            nameLabel.ForeColor = Color.LightGray;
            nameLabel.Font = new Font("Arial", 10);
            nameLabel.Location = new Point(0, 0);
            nameLabel.Size = new Size(745, 25);

            nameInputBox = new TextBox();
            nameInputBox.Location = new Point(0, 30);
            nameInputBox.Size = new Size(600, 30);
            nameInputBox.BackColor = Color.FromArgb(30, 30, 50);
            nameInputBox.ForeColor = Color.White;
            nameInputBox.Font = new Font("Arial", 10);
            nameInputBox.BorderStyle = BorderStyle.FixedSingle;

            startButton = new Button();
            startButton.Text = "Start Chat";
            startButton.Location = new Point(615, 28);
            startButton.Size = new Size(100, 30);
            startButton.BackColor = Color.DarkCyan;
            startButton.ForeColor = Color.White;
            startButton.FlatStyle = FlatStyle.Flat;
            startButton.Font = new Font("Arial", 9, FontStyle.Bold);
            startButton.Click += StartButton_Click;

            namePanel.Controls.Add(nameLabel);
            namePanel.Controls.Add(nameInputBox);
            namePanel.Controls.Add(startButton);

            // Chat panel - hidden until name entered
            chatPanel = new Panel();
            chatPanel.Location = new Point(20, 160);
            chatPanel.Size = new Size(745, 430);
            chatPanel.BackColor = Color.Transparent;
            chatPanel.Visible = false;

            chatDisplay = new RichTextBox();
            chatDisplay.Location = new Point(0, 0);
            chatDisplay.Size = new Size(745, 350);
            chatDisplay.BackColor = Color.FromArgb(20, 20, 40);
            chatDisplay.ForeColor = Color.LightGray;
            chatDisplay.Font = new Font("Courier New", 10);
            chatDisplay.ReadOnly = true;
            chatDisplay.BorderStyle = BorderStyle.None;
            chatDisplay.ScrollBars = RichTextBoxScrollBars.Vertical;

            userInputBox = new TextBox();
            userInputBox.Location = new Point(0, 365);
            userInputBox.Size = new Size(610, 35);
            userInputBox.BackColor = Color.FromArgb(30, 30, 50);
            userInputBox.ForeColor = Color.White;
            userInputBox.Font = new Font("Arial", 10);
            userInputBox.BorderStyle = BorderStyle.FixedSingle;
            userInputBox.KeyPress += UserInputBox_KeyPress;

            sendButton = new Button();
            sendButton.Text = "Send";
            sendButton.Location = new Point(625, 363);
            sendButton.Size = new Size(100, 35);
            sendButton.BackColor = Color.DarkCyan;
            sendButton.ForeColor = Color.White;
            sendButton.FlatStyle = FlatStyle.Flat;
            sendButton.Font = new Font("Arial", 10, FontStyle.Bold);
            sendButton.Click += SendButton_Click;

            chatPanel.Controls.Add(chatDisplay);
            chatPanel.Controls.Add(userInputBox);
            chatPanel.Controls.Add(sendButton);

            // Add everything to form
            this.Controls.Add(asciiLabel);
            this.Controls.Add(titleLabel);
            this.Controls.Add(namePanel);
            this.Controls.Add(chatPanel);
        }

        private void PlayVoiceGreeting()
        {
            try
            {
                string audioPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "greeting.wav.wav");

                if (File.Exists(audioPath))
                {
                    ProcessStartInfo psi = new ProcessStartInfo();
                    psi.FileName = "powershell";
                    psi.Arguments = $"-c (New-Object Media.SoundPlayer '{audioPath}').PlaySync()";
                    psi.UseShellExecute = false;
                    psi.CreateNoWindow = true;

                    Process.Start(psi);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("[ERROR] Could not play greeting: " + ex.Message);
            }
        }

        private void StartButton_Click(object sender, EventArgs e)
        {
            // Validate name input
            if (string.IsNullOrWhiteSpace(nameInputBox.Text))
            {
                MessageBox.Show("Please enter your name to continue.", "Name Required",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            userName = nameInputBox.Text.Trim();

            // Hide name panel, show chat panel
            namePanel.Visible = false;
            chatPanel.Visible = true;

            // Welcome message
            AppendBotMessage($"Hello, {userName}! Welcome to the Cybersecurity Awareness Bot.");
            AppendBotMessage("You can ask me about phishing, passwords, malware, scams, privacy, and more.");
            AppendBotMessage("Type 'exit' or 'bye' to leave.");
            AppendBotMessage("Type 'help' to see all the things I can do!");
        }

        private void UserInputBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Allow sending message by pressing Enter
            if (e.KeyChar == (char)Keys.Enter)
            {
                e.Handled = true;
                SendButton_Click(sender, e);
            }
        }

        private void SendButton_Click(object sender, EventArgs e)
        {
            string userInput = userInputBox.Text.Trim();

            // Validate empty input
            if (string.IsNullOrWhiteSpace(userInput))
            {
                AppendBotMessage("Please type something. I'm here to help you with cybersecurity!");
                return;
            }

            // Show user message
            AppendUserMessage(userInput);
            userInputBox.Clear();

            // Check for exit
            if (userInput.ToLower().Contains("bye") || userInput.ToLower().Contains("exit") || userInput.ToLower().Contains("quit"))
            {
                AppendBotMessage($"Stay safe online, {userName}! Goodbye!");
                Application.Exit();
                return;
            }

            // ============================================================
            // USE NLP TO DETECT INTENT
            // ============================================================
            string intent = nlp.DetectIntent(userInput);

            // Handle based on intent
            switch (intent)
            {
                case "help":
                    string helpMessage = @"🤖 I can help you with:

1️⃣ TASK MANAGEMENT (SQLite Database):
   • 'Add task - Review privacy settings'
   • 'Remind me to update password in 3 days'
   • 'Show my tasks' or 'List tasks'
   • 'Complete task 2' (use the task ID)
   • 'Delete task 1'

2️⃣ CYBERSECURITY QUIZ:
   • 'Start quiz' or 'Play game' (11 questions!)

3️⃣ ACTIVITY LOG:
   • 'Show activity log'
   • 'What have you done?'

4️⃣ CYBERSECURITY TIPS:
   • Ask about: phishing, passwords, scams, privacy, malware, safe browsing

5️⃣ SENTIMENT DETECTION:
   • I can detect if you're worried, frustrated, curious, or confused

🔐 Stay safe online!";
                    AppendBotMessage(helpMessage);
                    return;

                case "start_quiz":
                    activityLog.AddEntry("Quiz started by user");
                    quizForm = new QuizForm(activityLog);
                    quizForm.ShowDialog();
                    AppendBotMessage("🎮 Quiz completed! Check your score!");
                    return;

                case "show_log":
                    string log = activityLog.GetFormattedLog();
                    activityLog.AddEntry("Viewed activity log");
                    AppendBotMessage("📋 Here's your activity log:\n" + log);
                    return;

                case "add_task":
                case "view_tasks":
                case "complete_task":
                case "delete_task":
                    string taskResponse = HandleTaskCommand(userInput);
                    if (taskResponse != null)
                    {
                        AppendBotMessage(taskResponse);
                        return;
                    }
                    break;

                case "chat":
                default:
                    // Check for follow up questions
                    if (userInput.ToLower().Contains("tell me more") || userInput.ToLower().Contains("explain more") ||
                        userInput.ToLower().Contains("another tip"))
                    {
                        HandleFollowUp();
                        return;
                    }

                    // Detect sentiment
                    string sentiment = sentimentDetector.DetectSentiment(userInput);
                    if (sentiment != "neutral")
                    {
                        AppendBotMessage(sentimentDetector.GetSentimentResponse(sentiment));
                        return;
                    }

                    // Get regular keyword response
                    string botResponse = GetKeywordResponse(userInput);
                    AppendBotMessage(botResponse);
                    break;
            }
        }

        private string HandleTaskCommand(string input)
        {
            string lowered = input.ToLower();

            // Add a task
            if (lowered.Contains("add task") || lowered.Contains("new task") ||
                lowered.Contains("create task") || lowered.Contains("i need to") ||
                lowered.Contains("don't forget") || lowered.Contains("remind me") ||
                lowered.Contains("set reminder"))
            {
                // Try to extract task details
                string taskTitle = "";
                string description = "";
                DateTime? reminderDate = null;

                // Remove command words to get the task
                string[] removeWords = { "add task", "new task", "create task", "i need to",
                                         "don't forget", "remind me", "set reminder", "to" };
                string remaining = input;
                foreach (string word in removeWords)
                {
                    if (remaining.ToLower().Contains(word))
                    {
                        int idx = remaining.ToLower().IndexOf(word);
                        remaining = remaining.Substring(idx + word.Length).Trim();
                    }
                }

                if (!string.IsNullOrEmpty(remaining))
                {
                    // Check for "in X days" or "tomorrow"
                    if (remaining.ToLower().Contains("tomorrow"))
                    {
                        reminderDate = DateTime.Now.AddDays(1);
                        remaining = remaining.Replace("tomorrow", "").Trim();
                    }
                    else if (remaining.ToLower().Contains("in "))
                    {
                        // Try to parse "in 3 days"
                        string[] parts = remaining.Split(' ');
                        for (int i = 0; i < parts.Length; i++)
                        {
                            if (parts[i].ToLower() == "in" && i + 2 < parts.Length)
                            {
                                if (int.TryParse(parts[i + 1], out int days))
                                {
                                    if (parts[i + 2].ToLower().Contains("day") || parts[i + 2].ToLower().Contains("week"))
                                    {
                                        reminderDate = DateTime.Now.AddDays(days);
                                        break;
                                    }
                                }
                            }
                        }
                    }

                    taskTitle = remaining.Length > 50 ? remaining.Substring(0, 50) : remaining;
                    description = remaining;
                }

                if (!string.IsNullOrEmpty(taskTitle))
                {
                    db.AddTask(taskTitle, description, reminderDate);
                    activityLog.AddEntry($"Task added: '{taskTitle}'");
                    string reminderMsg = reminderDate.HasValue ? $" with reminder for {reminderDate.Value.ToShortDateString()}" : "";
                    return $"✅ Task added: '{taskTitle}'{reminderMsg}";
                }

                return "Please tell me what task you'd like to add. Example: 'Add task - Review privacy settings'";
            }

            // View tasks
            else if (lowered.Contains("show task") || lowered.Contains("my tasks") ||
                     lowered.Contains("view tasks") || lowered.Contains("list tasks") ||
                     lowered.Contains("what tasks") || lowered.Contains("pending tasks"))
            {
                bool showAll = lowered.Contains("all") || lowered.Contains("completed");
                string tasks = db.GetTasksAsString(!showAll);
                activityLog.AddEntry("Viewed tasks");
                return tasks;
            }

            // Mark as completed
            else if (lowered.Contains("complete") && lowered.Contains("task"))
            {
                int taskId = 0;
                string[] words = lowered.Split(' ');
                foreach (string word in words)
                {
                    if (int.TryParse(word, out taskId))
                        break;
                }

                if (taskId > 0)
                {
                    db.MarkTaskCompleted(taskId);
                    activityLog.AddEntry($"Task {taskId} marked as completed");
                    return $"✅ Task {taskId} marked as completed!";
                }
                else
                {
                    return "Please specify the task ID. Example: 'Complete task 2'";
                }
            }

            // Delete task
            else if (lowered.Contains("delete task") || lowered.Contains("remove task"))
            {
                int taskId = 0;
                string[] words = lowered.Split(' ');
                foreach (string word in words)
                {
                    if (int.TryParse(word, out taskId))
                        break;
                }

                if (taskId > 0)
                {
                    db.DeleteTask(taskId);
                    activityLog.AddEntry($"Task {taskId} deleted");
                    return $"🗑️ Task {taskId} has been deleted.";
                }
                else
                {
                    return "Please specify the task ID. Example: 'Delete task 1'";
                }
            }

            return null;
        }

        private string GetKeywordResponse(string input)
        {
            string lowered = input.ToLower();

            if (lowered.Contains("phishing"))
            {
                lastTopic = "phishing";
                return randomResponses.GetRandomPhishingTip();
            }
            else if (lowered.Contains("password"))
            {
                lastTopic = "password";
                return randomResponses.GetRandomPasswordTip();
            }
            else if (lowered.Contains("scam"))
            {
                lastTopic = "scam";
                return randomResponses.GetRandomScamTip();
            }
            else if (lowered.Contains("privacy"))
            {
                lastTopic = "privacy";
                return randomResponses.GetRandomPrivacyTip();
            }
            else if (lowered.Contains("malware"))
            {
                lastTopic = "malware";
                return "Malware is malicious software designed to damage your device. Keep your antivirus updated and never open attachments from unknown senders.";
            }
            else if (lowered.Contains("browsing") || lowered.Contains("safe browsing"))
            {
                lastTopic = "browsing";
                return "Always look for HTTPS in the URL before entering personal information. Avoid clicking pop-ups and never download software from unknown websites.";
            }
            else if (lowered.Contains("how are you"))
            {
                return "I'm functioning smoothly and ready to help keep you safe online!";
            }
            else if (lowered.Contains("purpose") || lowered.Contains("what can you do"))
            {
                return "I'm here to educate you about cybersecurity threats like phishing, scams, weak passwords, and unsafe browsing.";
            }
            else if (lowered.Contains("what can i ask"))
            {
                return "You can ask me about: phishing, passwords, scams, privacy, malware, and safe browsing!";
            }
            else if (lowered.Contains("hello") || lowered.Contains("hi"))
            {
                return $"Hello, {userName}! How can I help you stay safe online today?";
            }
            else
            {
                return response.GetResponse(input);
            }
        }

        private void HandleFollowUp()
        {
            // Memory - uses lastTopic to give another tip on the same topic
            if (lastTopic == "phishing")
            {
                AppendBotMessage("Here's another phishing tip: " + randomResponses.GetRandomPhishingTip());
            }
            else if (lastTopic == "password")
            {
                AppendBotMessage("Here's another password tip: " + randomResponses.GetRandomPasswordTip());
            }
            else if (lastTopic == "scam")
            {
                AppendBotMessage("Here's another scam tip: " + randomResponses.GetRandomScamTip());
            }
            else if (lastTopic == "privacy")
            {
                AppendBotMessage("Here's another privacy tip: " + randomResponses.GetRandomPrivacyTip());
            }
            else
            {
                AppendBotMessage("Which topic would you like more info on? Try asking about phishing, passwords, scams, or privacy.");
            }
        }

        private void AppendBotMessage(string message)
        {
            chatDisplay.SelectionColor = Color.Cyan;
            chatDisplay.AppendText("[BOT]: ");
            chatDisplay.SelectionColor = Color.LightGray;
            chatDisplay.AppendText(message + "\n\n");
            chatDisplay.ScrollToCaret();
        }

        private void AppendUserMessage(string message)
        {
            chatDisplay.SelectionColor = Color.MediumPurple;
            chatDisplay.AppendText($"[{userName}]: ");
            chatDisplay.SelectionColor = Color.White;
            chatDisplay.AppendText(message + "\n");
            chatDisplay.ScrollToCaret();
        }
    }
}