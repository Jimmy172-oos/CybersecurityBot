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
        private string lastTopic = ""; // memory - remembers last topic discussed

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
                AppendBotMessage("It looks like you didn't type anything. Please ask me a cybersecurity question!");
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

            // Check for follow up questions
            if (userInput.ToLower().Contains("tell me more") || userInput.ToLower().Contains("explain more") || userInput.ToLower().Contains("another tip"))
            {
                HandleFollowUp();
                return;
            }

            // Detect sentiment first
            string sentiment = sentimentDetector.DetectSentiment(userInput);
            if (sentiment != "neutral")
            {
                AppendBotMessage(sentimentDetector.GetSentimentResponse(sentiment));
                AppendBotMessage("What cybersecurity topic can I help you with today?");
                return; // stops here, won't show the "didn't understand" message
            }

            // Get response based on keywords
            string botResponse = GetKeywordResponse(userInput);
            AppendBotMessage(botResponse);
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