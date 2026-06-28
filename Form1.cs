using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace CyberSecurityBotGUI
{
    public partial class Form1 : Form
    {
// VARIABLES

        private DatabaseHelper db = new DatabaseHelper();

        private List<string> activityLog = new List<string>();

        private List<QuizQuestion> quizQuestions =
            new List<QuizQuestion>();

        private int currentQuestion = 0;
        private int score = 0;

        //====================================================
        // CONSTRUCTOR
        //====================================================

        public Form1()
        {
            InitializeComponent();
            InitializeQuiz();
        }

        //====================================================
        // ACTIVITY LOG METHODS
        //====================================================

        private void AddActivityLog(string action)
        {
            activityLog.Add(
                DateTime.Now.ToString("g") + " - " + action);
        }

        private void ViewActivityLog()
        {
            AppendBotResponse("Recent Activities:");

            int start = Math.Max(0, activityLog.Count - 10);

            for (int i = start; i < activityLog.Count; i++)
            {
                AppendBotResponse(activityLog[i]);
            }
        }

        //====================================================
        // QUIZ INITIALIZATION
        //====================================================

        private void InitializeQuiz()
        {
            quizQuestions.Add(new QuizQuestion()
            {
                Question =
                    "What should you do if an email asks for your password?",

                Options = new string[]
                {
                "A. Reply",
                "B. Report as phishing",
                "C. Ignore",
                "D. Forward"
                },

                CorrectAnswer = "B",

                Explanation =
                    "Reporting phishing helps stop scams."
            });

            quizQuestions.Add(new QuizQuestion()
            {
                Question =
                    "True or False: Reusing passwords is safe.",

                Options = new string[]
                {
                "A. True",
                "B. False"
                },

                CorrectAnswer = "B",

                Explanation =
                    "Every account needs a unique password."
            });

            // Add remaining questions here...
        }

        //====================================================
        // SHOW QUIZ QUESTION
        //====================================================

        private void ShowQuestion()
        {
            if (currentQuestion >= quizQuestions.Count)
            {
                AppendBotResponse(
                    $"Quiz completed. Score: {score}/{quizQuestions.Count}");

                AddActivityLog("Quiz Completed");

                return;
            }

            QuizQuestion q = quizQuestions[currentQuestion];

            AppendBotResponse(q.Question);

            foreach (string option in q.Options)
            {
                AppendBotResponse(option);
            }
        }

        //====================================================
        // CHECK QUIZ ANSWER
        //====================================================

        private void CheckAnswer(string answer)
        {
            QuizQuestion q = quizQuestions[currentQuestion];

            if (answer.ToUpper() == q.CorrectAnswer.ToUpper()
                || answer == q.CorrectAnswer)
            {
                score++;
                AppendBotResponse("Correct!");
            }
            else
            {
                AppendBotResponse(
                    "Incorrect. " + q.Explanation);
            }

            currentQuestion++;

            ShowQuestion();
        }
        //====================================================
        // VIEW TASKS METHOD
        //====================================================

        private void ViewTasks()
        {
            List<TaskItem> tasks = db.GetTasks();

            if (tasks.Count == 0)
            {
                AppendBotResponse("No tasks found.");
                return;
            }

            foreach (TaskItem task in tasks)
            {
                AppendBotResponse(
                    $"Task: {task.Title}\n" +
                    $"Description: {task.Description}\n" +
                    $"Completed: {task.IsCompleted}");
            }

        }

        //====================================================
        // NLP SIMULATION / RESPOND METHOD
        //====================================================

        private void Respond(string input)
        {
            string lowerInput = input.ToLower();

               // Start Quiz
            if (lowerInput.Contains("start quiz") ||
                lowerInput.Contains("play quiz") ||
                lowerInput.Contains("begin quiz"))
            {
                currentQuestion = 0;
                score = 0;

                AddActivityLog("Quiz Started");

                AppendBotResponse("Quiz Started!");

                ShowQuestion();

                return;
            }

            // Show Activity Log
            if (lowerInput.Contains("show activity") ||
                lowerInput.Contains("activity log") ||
                lowerInput.Contains("what have you done for me"))
            {
                ViewActivityLog();
                return;
            }

            // Show Tasks
            if (lowerInput.Contains("show tasks") ||
                lowerInput.Contains("view tasks") ||
                lowerInput.Contains("my tasks"))
            {
                ViewTasks();
                return;
            }

            // Add Task
            if (lowerInput.Contains("add task") ||
                lowerInput.Contains("remind me") ||
                lowerInput.Contains("set reminder"))
            {
                TaskItem task = new TaskItem()
                {
                    Title = input,
                    Description = input,
                    IsCompleted = false
                };

                db.AddTask(task);

                AddActivityLog("Task Added: " + input);

                AppendBotResponse("Task added successfully.");

                return;
            }

            // Process Quiz Answers
            if (currentQuestion < quizQuestions.Count)
            {
                CheckAnswer(input);
                return;
            }

            // Default Response
            AppendBotResponse(
                "I did not understand that. Could you rephrase?");

}

        // SEND BUTTON EVENT
        

        private void btnSend_Click(object sender, EventArgs e)
        {
            string input = textBox1.Text.Trim();
   
            if (string.IsNullOrWhiteSpace(input))
            {
                AppendBotResponse("Please enter a message.");
                return;
            }

            AppendText("You: " + input + "\n", Color.White);

            textBox1.Clear();

            Respond(input);

        }

        // BOT OUTPUT METHODS

        private void AppendBotResponse(string message)
        {
            AppendText($"Bot: {message}\n\n", Color.Cyan);
        }

        private void AppendText(string text, Color color)
        {
            // Replace richTextBox1 with your actual RichTextBox name

            richTextbox1.SelectionStart =
                richTextBox1.TextLength;

            richTextBox1.SelectionLength = 0;

            richTextBox1.SelectionColor = color;

            richTextBox1.AppendText(text);

            richTextBox1.SelectionColor =
                richTextBox1.ForeColor;

            richTextBox1.ScrollToCaret();
        }

       
    }

}


