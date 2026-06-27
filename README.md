# Cybersecurity Awareness Bot

A cybersecurity awareness chatbot built in C# that educates users about 
cybersecurity threats. Part 1 was a console-based application and Part 2 
upgraded it into a full WinForms GUI application.

## How to Run
1. Clone this repository
2. Open `CybersecurityBot.sln` in Visual Studio 2022
3. Press F5 or click the run button at the top to run
4. Enter your name and click Start Chat to begin

## Part 1 Features
- Console-based chatbot
- ASCII art logo
- Voice greeting on startup
- Personalised user interaction
- Responses on: phishing, passwords, safe browsing, malware, social engineering
- Input validation
- Typing and thinking effects
- Coloured console text

## Part 2 Features
- WinForms GUI window
- Random responses using Lists for each topic
- Sentiment detection — detects worried, frustrated, curious, confused
- Memory — remembers last topic for follow up questions
- Type 'tell me more' or 'another tip' for follow up responses
- Input validation with error messages
- Coloured chat display

## Part 3 Advanced Features 
-  Task Assistant with reminders
-  Cybersecurity Quiz with 15+ questions
-  NLP simulation with keyword detection
-  Activity Log Feature
-  Database integration (file-based storage)

## Project Structure
- `Program.cs` — entry point, launches the WinForms window
- `MainForm.cs` — the GUI window and conversation logic
- `ChatBot.cs` — original console chatbot logic from Part 1
- `ConsoleUI.cs` — console display methods from Part 1
- `Response.cs` — all chatbot responses and keyword matching
- `SentimentDetector.cs` — detects user emotions from input
- `RandomResponses.cs` — stores multiple tips per topic using Lists

## Technologies Used 
- C# .NET 8.0
- Windows Forms
- File-based storage (JSON/TXT)
- No external dependencies

## Author

[Jimlongwe]

## License

This project is for educational purposes.


## CI Workflow
![CI Status](https://github.com/Jimmy172-oos/CybersecurityBot/actions/workflows/dotnet.yml/badge.svg)

<img width="1358" height="508" alt="CI workflow" src="https://github.com/user-attachments/assets/fe00df7e-a2a9-4483-b0d6-a371ea909cca" />
