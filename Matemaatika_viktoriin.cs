using System;
using System.Drawing;
using System.IO; // Для работы с файлами
using System.Windows.Forms;

namespace Elemendid_vormis_Vsevolod_Tsarev_TARpv23
{
    public partial class Matemaatika_viktoriin : Form
    {
        Label timeLabel;
        Label plusLeftLabel, plusRightLabel;
        Label minusLeftLabel, minusRightLabel;
        Label timesLeftLabel, timesRightLabel;
        Label dividedLeftLabel, dividedRightLabel;
        NumericUpDown sum, difference, product, quotient;
        Button startButton, exitButton;
        Button checkPlusButton, checkMinusButton, checkTimesButton, checkDividedButton;
        Button showPlusAnswerButton, showMinusAnswerButton, showTimesAnswerButton, showDividedAnswerButton;
        System.Windows.Forms.Timer quizTime;
        ListBox resultsListBox; // Список для отображения результатов теста
        int timeLeft;

        int plusAnswer, minusAnswer, timesAnswer, dividedAnswer;
        int correctAnswers, incorrectAnswers; // Счетчики правильных и неправильных ответов

        public Matemaatika_viktoriin(int h, int w)
        {
            this.Height = h;
            this.Width = w; 
            this.Text = "Math Quiz";

            timeLabel = new Label();
            timeLabel.Text = "Time Left: ";
            timeLabel.Font = new Font("Arial", 20, FontStyle.Bold);
            timeLabel.Size = new Size(200, 50);
            timeLabel.Location = new Point(50, 20);
            Controls.Add(timeLabel);

            CreateMathQuestion(out plusLeftLabel, out plusRightLabel, out sum, "+", 50, 100, out checkPlusButton, out showPlusAnswerButton);
            CreateMathQuestion(out minusLeftLabel, out minusRightLabel, out difference, "-", 50, 150, out checkMinusButton, out showMinusAnswerButton);
            CreateMathQuestion(out timesLeftLabel, out timesRightLabel, out product, "×", 50, 200, out checkTimesButton, out showTimesAnswerButton);
            CreateMathQuestion(out dividedLeftLabel, out dividedRightLabel, out quotient, "÷", 50, 250, out checkDividedButton, out showDividedAnswerButton);

            // Start button
            startButton = new Button();
            startButton.Text = "Start Quiz";
            startButton.Size = new Size(100, 50);
            startButton.Location = new Point(200, 400);
            startButton.Click += new EventHandler(StartButton_Click);
            Controls.Add(startButton);

            // Exit button
            exitButton = new Button();
            exitButton.Text = "Exit";
            exitButton.Size = new Size(100, 50);
            exitButton.Location = new Point(350, 400);
            exitButton.Click += new EventHandler(ExitButton_Click);
            Controls.Add(exitButton);

            // Timer
            quizTime = new System.Windows.Forms.Timer();
            quizTime.Interval = 1000; // 1 second
            quizTime.Tick += new EventHandler(Timer_Tick);

            // ListBox for results
            resultsListBox = new ListBox();
            resultsListBox.Location = new Point(500, 100);
            resultsListBox.Size = new Size(250, 300);
            Controls.Add(resultsListBox);
        }

        private void CreateMathQuestion(out Label leftLabel, out Label rightLabel, out NumericUpDown answerBox, string operation, int x, int y, out Button checkButton, out Button showAnswerButton)
        {
            leftLabel = new Label();
            leftLabel.Text = "?";
            leftLabel.Font = new Font("Arial", 18);
            leftLabel.Location = new Point(x, y);
            leftLabel.Size = new Size(60, 50);
            Controls.Add(leftLabel);

            rightLabel = new Label();
            rightLabel.Text = "?";
            rightLabel.Font = new Font("Arial", 18);
            rightLabel.Location = new Point(x + 100, y);
            rightLabel.Size = new Size(60, 50);
            Controls.Add(rightLabel);

            Label operatorLabel = new Label();
            operatorLabel.Text = operation;
            operatorLabel.Font = new Font("Arial", 18);
            operatorLabel.Location = new Point(x + 60, y);
            operatorLabel.Size = new Size(40, 50);
            Controls.Add(operatorLabel);

            answerBox = new NumericUpDown();
            answerBox.Size = new Size(100, 50);
            answerBox.Location = new Point(x + 200, y);
            answerBox.Minimum = -100;
            answerBox.Maximum = 100;
            Controls.Add(answerBox);

            // Button to check answer
            checkButton = new Button();
            checkButton.Text = "Check";
            checkButton.Size = new Size(75, 30);
            checkButton.Location = new Point(x + 310, y + 10);
            Controls.Add(checkButton);

            // Button to show correct answer, hidden initially
            showAnswerButton = new Button();
            showAnswerButton.Text = "Show Answer";
            showAnswerButton.Size = new Size(100, 30);
            showAnswerButton.Location = new Point(x + 390, y + 10);
            showAnswerButton.Visible = false; // Hidden initially
            Controls.Add(showAnswerButton);
        }

        private void StartButton_Click(object sender, EventArgs e)
        {
            // Reset score counters
            correctAnswers = 0;
            incorrectAnswers = 0;

            // Clear the results list box
            resultsListBox.Items.Clear();

            Random random = new Random();

            // Reset visibility of the "Show Answer" buttons and clear previous answers
            showPlusAnswerButton.Visible = false;
            showMinusAnswerButton.Visible = false;
            showTimesAnswerButton.Visible = false;
            showDividedAnswerButton.Visible = false;

            sum.Value = 0;
            difference.Value = 0;
            product.Value = 0;
            quotient.Value = 0;

            // Generate and calculate answers for questions
            int plusLeft = random.Next(1, 10);
            int plusRight = random.Next(1, 10);
            plusLeftLabel.Text = plusLeft.ToString();
            plusRightLabel.Text = plusRight.ToString();
            plusAnswer = plusLeft + plusRight;

            int minusLeft = random.Next(1, 10);
            int minusRight = random.Next(1, minusLeft + 1); // Ensure positive results
            minusLeftLabel.Text = minusLeft.ToString();
            minusRightLabel.Text = minusRight.ToString();
            minusAnswer = minusLeft - minusRight;

            int timesLeft = random.Next(1, 10);
            int timesRight = random.Next(1, 10);
            timesLeftLabel.Text = timesLeft.ToString();
            timesRightLabel.Text = timesRight.ToString();
            timesAnswer = timesLeft * timesRight;

            int dividedLeft = random.Next(1, 10);
            int dividedRight = random.Next(1, dividedLeft + 1); // Ensure whole number division
            dividedLeftLabel.Text = (dividedLeft * dividedRight).ToString(); // Generate correct dividend
            dividedRightLabel.Text = dividedRight.ToString();
            dividedAnswer = dividedLeft * dividedRight / dividedRight;

            timeLeft = 30; // 30 seconds for the quiz
            timeLabel.Text = "Time Left: " + timeLeft + " seconds";
            quizTime.Start();

            // Attach check button event handlers
            checkPlusButton.Click += (s, args) => CheckAnswer(sum, plusAnswer, showPlusAnswerButton);
            checkMinusButton.Click += (s, args) => CheckAnswer(difference, minusAnswer, showMinusAnswerButton);
            checkTimesButton.Click += (s, args) => CheckAnswer(product, timesAnswer, showTimesAnswerButton);
            checkDividedButton.Click += (s, args) => CheckAnswer(quotient, dividedAnswer, showDividedAnswerButton);
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            if (timeLeft > 0)
            {
                timeLeft--;
                timeLabel.Text = "Time Left: " + timeLeft + " seconds";
            }
            else
            {
                quizTime.Stop();
                CheckAllAnswers();
                MessageBox.Show("Time's up!");
            }
        }

        private void CheckAnswer(NumericUpDown answerBox, int correctAnswer, Button showAnswerButton)
        {
            if ((int)answerBox.Value == correctAnswer)
            {
                MessageBox.Show("Correct!");
                LogResult("Correct");
                correctAnswers++;
            }
            else
            {
                MessageBox.Show("Incorrect!");
                showAnswerButton.Visible = true; // Show the answer button if incorrect
                showAnswerButton.Click += (s, args) => MessageBox.Show("The correct answer is: " + correctAnswer);
                incorrectAnswers++;
            }

            // Update results list
            UpdateResultsList();
        }

        private void CheckAllAnswers()
        {
            CheckAnswer(sum, plusAnswer, showPlusAnswerButton);
            CheckAnswer(difference, minusAnswer, showMinusAnswerButton);
            CheckAnswer(product, timesAnswer, showTimesAnswerButton);
            CheckAnswer(quotient, dividedAnswer, showDividedAnswerButton);

            // Final update of the results list
            UpdateResultsList();
        }

        private void UpdateResultsList()
        {
            resultsListBox.Items.Clear();
            resultsListBox.Items.Add("Correct Answers: " + correctAnswers);
            resultsListBox.Items.Add("Incorrect Answers: " + incorrectAnswers);
            LogResult("Test completed: " + correctAnswers + " correct, " + incorrectAnswers + " incorrect");
        }

        private void LogResult(string result)
        {
            string filePath = "quiz_results.txt";
            using (StreamWriter writer = new StreamWriter(filePath, true))
            {
                writer.WriteLine(DateTime.Now.ToString() + ": " + result);
            }
        }

        private void ExitButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        
    }
}
