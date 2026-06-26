using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace CybersecurityBot
{
    public class QuizForm : Form
    {
        private Label questionLabel;
        private Label questionNumberLabel;
        private Button optionA;
        private Button optionB;
        private Button optionC;
        private Button optionD;
        private Label feedbackLabel;
        private Label scoreLabel;
        private Button nextButton;
        private Button closeButton;
        private ActivityLog activityLog;

        private int currentQuestion = 0;
        private int score = 0;
        private bool answered = false;

        private List<string[]> questions = new List<string[]>()
        {
            new string[] { "What should you do if you receive an email asking for your password?", "Reply with your password", "Delete the email", "Report it as phishing", "Ignore it", "C", "Reporting phishing emails helps protect others." },
            new string[] { "What does HTTPS in a website URL mean?", "The site is fast", "The site is secure and encrypted", "The site is free", "The site is popular", "B", "HTTPS means the connection is encrypted." },
            new string[] { "True or False: Using the same password for all accounts is safe.", "True", "False", "Only for work", "Only for social media", "B", "If one account is hacked, all are vulnerable." },
            new string[] { "What is social engineering?", "Building social media apps", "Tricking people into giving away info", "Engineering social networks", "A type of firewall", "B", "Social engineering manipulates people psychologically." },
            new string[] { "What is malware?", "A type of hardware", "A slow internet connection", "Malicious software", "A type of password", "C", "Malware includes viruses and ransomware." },
            new string[] { "True or False: Public Wi-Fi is always safe for banking.", "True", "False", "Only in coffee shops", "Only during the day", "B", "Public Wi-Fi is not secure for banking." },
            new string[] { "How long should a strong password be?", "4 characters", "6 characters", "8 characters", "12 characters", "D", "Strong passwords should be at least 12 characters." },
            new string[] { "What is two-factor authentication (2FA)?", "Using two passwords", "A second layer of security", "Two usernames", "A type of encryption", "B", "2FA adds an extra security step." },
            new string[] { "True or False: Antivirus eliminates all cybersecurity risks.", "True", "False", "Only on Windows", "Only with updates", "B", "Antivirus helps but safe browsing is also important." },
            new string[] { "What should you do before clicking a link in an email?", "Click immediately", "Forward to friends", "Hover to check the URL", "Reply to sender", "C", "Hovering reveals the real destination URL." },
            new string[] { "What is a phishing attack?", "Fishing online", "Fake message to steal info", "A computer virus", "Network monitoring", "B", "Phishing uses fake messages to steal information." }
        };

        public QuizForm(ActivityLog log)
        {
            activityLog = log;
            activityLog.AddEntry("Quiz started");
            SetupForm();
            LoadQuestion();
        }

        private void SetupForm()
        {
            this.Text = "Cybersecurity Quiz";
            this.Size = new Size(750, 550);
            this.BackColor = Color.FromArgb(15, 15, 30);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;

            Label titleLabel = new Label();
            titleLabel.Text = "Cybersecurity Knowledge Quiz";
            titleLabel.Font = new Font("Arial", 14, FontStyle.Bold);
            titleLabel.ForeColor = Color.Cyan;
            titleLabel.Location = new Point(20, 15);
            titleLabel.Size = new Size(700, 30);
            titleLabel.TextAlign = ContentAlignment.MiddleCenter;

            questionNumberLabel = new Label();
            questionNumberLabel.Font = new Font("Arial", 10, FontStyle.Bold);
            questionNumberLabel.ForeColor = Color.LightCyan;
            questionNumberLabel.Location = new Point(20, 55);
            questionNumberLabel.Size = new Size(700, 25);

            scoreLabel = new Label();
            scoreLabel.Font = new Font("Arial", 10);
            scoreLabel.ForeColor = Color.LightGreen;
            scoreLabel.Location = new Point(20, 75);
            scoreLabel.Size = new Size(700, 25);
            scoreLabel.Text = "Score: 0";

            questionLabel = new Label();
            questionLabel.Font = new Font("Arial", 11);
            questionLabel.ForeColor = Color.White;
            questionLabel.Location = new Point(20, 110);
            questionLabel.Size = new Size(700, 60);

            optionA = CreateOptionButton("", 20, 180, "A");
            optionB = CreateOptionButton("", 20, 230, "B");
            optionC = CreateOptionButton("", 20, 280, "C");
            optionD = CreateOptionButton("", 20, 330, "D");

            feedbackLabel = new Label();
            feedbackLabel.Font = new Font("Arial", 10, FontStyle.Italic);
            feedbackLabel.Location = new Point(20, 385);
            feedbackLabel.Size = new Size(700, 50);
            feedbackLabel.ForeColor = Color.Yellow;

            nextButton = new Button();
            nextButton.Text = "Next Question";
            nextButton.Location = new Point(520, 445);
            nextButton.Size = new Size(130, 35);
            nextButton.BackColor = Color.DarkCyan;
            nextButton.ForeColor = Color.White;
            nextButton.FlatStyle = FlatStyle.Flat;
            nextButton.Font = new Font("Arial", 9, FontStyle.Bold);
            nextButton.Enabled = false;
            nextButton.Click += NextButton_Click;

            closeButton = new Button();
            closeButton.Text = "Back to Chat";
            closeButton.Location = new Point(20, 445);
            closeButton.Size = new Size(120, 35);
            closeButton.BackColor = Color.DarkSlateGray;
            closeButton.ForeColor = Color.White;
            closeButton.FlatStyle = FlatStyle.Flat;
            closeButton.Font = new Font("Arial", 9, FontStyle.Bold);
            closeButton.Click += (s, e) => this.Close();

            this.Controls.Add(titleLabel);
            this.Controls.Add(questionNumberLabel);
            this.Controls.Add(scoreLabel);
            this.Controls.Add(questionLabel);
            this.Controls.Add(optionA);
            this.Controls.Add(optionB);
            this.Controls.Add(optionC);
            this.Controls.Add(optionD);
            this.Controls.Add(feedbackLabel);
            this.Controls.Add(nextButton);
            this.Controls.Add(closeButton);
        }

        private Button CreateOptionButton(string text, int x, int y, string tag)
        {
            Button btn = new Button();
            btn.Text = text;
            btn.Location = new Point(x, y);
            btn.Size = new Size(700, 40);
            btn.BackColor = Color.FromArgb(30, 30, 60);
            btn.ForeColor = Color.White;
            btn.FlatStyle = FlatStyle.Flat;
            btn.Font = new Font("Arial", 10);
            btn.TextAlign = ContentAlignment.MiddleLeft;
            btn.Tag = tag;
            btn.Click += OptionButton_Click;
            return btn;
        }

        private void LoadQuestion()
        {
            if (currentQuestion >= questions.Count)
            {
                ShowFinalScore();
                return;
            }

            answered = false;
            string[] q = questions[currentQuestion];

            questionNumberLabel.Text = $"Question {currentQuestion + 1} of {questions.Count}";
            questionLabel.Text = q[0];
            optionA.Text = "A) " + q[1];
            optionB.Text = "B) " + q[2];
            optionC.Text = "C) " + q[3];
            optionD.Text = "D) " + q[4];
            feedbackLabel.Text = "";
            nextButton.Enabled = false;

            optionA.BackColor = Color.FromArgb(30, 30, 60);
            optionB.BackColor = Color.FromArgb(30, 30, 60);
            optionC.BackColor = Color.FromArgb(30, 30, 60);
            optionD.BackColor = Color.FromArgb(30, 30, 60);
        }

        private void OptionButton_Click(object sender, EventArgs e)
        {
            if (answered) return;

            answered = true;
            Button clicked = (Button)sender;
            string selected = clicked.Tag.ToString();
            string correct = questions[currentQuestion][5];
            string explanation = questions[currentQuestion][6];

            if (selected == correct)
            {
                clicked.BackColor = Color.DarkGreen;
                feedbackLabel.ForeColor = Color.LightGreen;
                feedbackLabel.Text = "✓ Correct! " + explanation;
                score++;
                scoreLabel.Text = $"Score: {score}";
            }
            else
            {
                clicked.BackColor = Color.DarkRed;
                feedbackLabel.ForeColor = Color.Orange;
                feedbackLabel.Text = "✗ Incorrect. " + explanation;

                if (correct == "A") optionA.BackColor = Color.DarkGreen;
                else if (correct == "B") optionB.BackColor = Color.DarkGreen;
                else if (correct == "C") optionC.BackColor = Color.DarkGreen;
                else if (correct == "D") optionD.BackColor = Color.DarkGreen;
            }

            nextButton.Enabled = true;
        }

        private void NextButton_Click(object sender, EventArgs e)
        {
            currentQuestion++;
            LoadQuestion();
        }

        private void ShowFinalScore()
        {
            questionLabel.Text = "Quiz Complete!";
            questionNumberLabel.Text = $"Final Score: {score} out of {questions.Count}";
            optionA.Visible = false;
            optionB.Visible = false;
            optionC.Visible = false;
            optionD.Visible = false;
            nextButton.Visible = false;

            string feedback;
            if (score >= 9)
            {
                feedback = "Outstanding! You're a cybersecurity pro! 🏆";
                feedbackLabel.ForeColor = Color.LightGreen;
            }
            else if (score >= 7)
            {
                feedback = "Great job! You have solid cybersecurity knowledge!";
                feedbackLabel.ForeColor = Color.LightGreen;
            }
            else if (score >= 5)
            {
                feedback = "Good effort! Keep learning to stay safe online.";
                feedbackLabel.ForeColor = Color.Yellow;
            }
            else
            {
                feedback = "Keep studying — cybersecurity knowledge keeps you safe!";
                feedbackLabel.ForeColor = Color.Orange;
            }

            feedbackLabel.Text = feedback;
            activityLog.AddEntry($"Quiz completed - Score: {score}/{questions.Count}");
        }
    }
}