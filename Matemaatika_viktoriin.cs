using System;
using System.Drawing;
using System.IO;
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
        Button startButton, exitButton, endQuizButton; // Uus nupp
        Button checkPlusButton, checkMinusButton, checkTimesButton, checkDividedButton;
        Button showPlusAnswerButton, showMinusAnswerButton, showTimesAnswerButton, showDividedAnswerButton;
        System.Windows.Forms.Timer quizTime;
        ListBox resultsListBox;
        ComboBox difficultyComboBox;
        int timeLeft;

        int plusAnswer, minusAnswer, timesAnswer, dividedAnswer;
        int correctAnswers, incorrectAnswers;
        int score;
        private DateTime startTime;

        public Matemaatika_viktoriin(int h, int w)
        {
            this.Height = h;
            this.Width = w;
            this.Text = "Matemaatika viktoriin";

            timeLabel = new Label();
            timeLabel.Text = "Jääk: ";
            timeLabel.Font = new Font("Arial", 20, FontStyle.Bold);
            timeLabel.Size = new Size(200, 50);
            timeLabel.Location = new Point(50, 20);
            Controls.Add(timeLabel);

            CreateMathQuestion(out plusLeftLabel, out plusRightLabel, out sum, "+", 50, 100, out checkPlusButton, out showPlusAnswerButton);
            CreateMathQuestion(out minusLeftLabel, out minusRightLabel, out difference, "-", 50, 150, out checkMinusButton, out showMinusAnswerButton);
            CreateMathQuestion(out timesLeftLabel, out timesRightLabel, out product, "×", 50, 200, out checkTimesButton, out showTimesAnswerButton);
            CreateMathQuestion(out dividedLeftLabel, out dividedRightLabel, out quotient, "÷", 50, 250, out checkDividedButton, out showDividedAnswerButton);

            // Raskusaste
            difficultyComboBox = new ComboBox();
            difficultyComboBox.Items.Add("Lihtne");
            difficultyComboBox.Items.Add("Keskmine");
            difficultyComboBox.Items.Add("Raske");
            difficultyComboBox.SelectedIndex = 0; // Vaikimisi "Lihtne"
            difficultyComboBox.Location = new Point(200, 350);
            Controls.Add(difficultyComboBox);

            // Alusta nupp
            startButton = new Button();
            startButton.Text = "Alusta viktoriini";
            startButton.Size = new Size(100, 50);
            startButton.Location = new Point(200, 400);
            startButton.Click += new EventHandler(StartButton_Click);
            Controls.Add(startButton);

            // Lõpeta viktoriin nupp
            endQuizButton = new Button();
            endQuizButton.Text = "Lõpeta viktoriin varem";
            endQuizButton.Size = new Size(150, 50);
            endQuizButton.Location = new Point(350, 400);
            endQuizButton.Click += new EventHandler(EndQuizButton_Click);
            Controls.Add(endQuizButton);

            // Välju nupp
            exitButton = new Button();
            exitButton.Text = "Välju";
            exitButton.Size = new Size(100, 50);
            exitButton.Location = new Point(500, 400);
            exitButton.Click += new EventHandler(ExitButton_Click);
            Controls.Add(exitButton);

            // Taimer
            quizTime = new System.Windows.Forms.Timer();
            quizTime.Interval = 1000; // 1 sekund
            quizTime.Tick += new EventHandler(Timer_Tick);

            // Tulemuste ListBox
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
            answerBox.Maximum = 5000;
            Controls.Add(answerBox);

            checkButton = new Button();
            checkButton.Text = "Kontrolli";
            checkButton.Size = new Size(75, 30);
            checkButton.Location = new Point(x + 310, y + 10);
            Controls.Add(checkButton);

            showAnswerButton = new Button();
            showAnswerButton.Text = "Näita vastust";
            showAnswerButton.Size = new Size(100, 30);
            showAnswerButton.Location = new Point(x + 390, y + 10);
            showAnswerButton.Visible = false;
            Controls.Add(showAnswerButton);
        }

        private void StartButton_Click(object sender, EventArgs e)
        {
            score = 0;
            correctAnswers = 0;
            incorrectAnswers = 0;

            resultsListBox.Items.Clear();

            Random random = new Random();
            int range = GetNumberRange();

            showPlusAnswerButton.Visible = false;
            showMinusAnswerButton.Visible = false;
            showTimesAnswerButton.Visible = false;
            showDividedAnswerButton.Visible = false;

            sum.Value = 0;
            difference.Value = 0;
            product.Value = 0;
            quotient.Value = 0;

            int plusLeft = random.Next(1, range);
            int plusRight = random.Next(1, range);
            plusLeftLabel.Text = plusLeft.ToString();
            plusRightLabel.Text = plusRight.ToString();
            plusAnswer = plusLeft + plusRight;

            int minusLeft = random.Next(1, range);
            int minusRight = random.Next(1, minusLeft + 1);
            minusLeftLabel.Text = minusLeft.ToString();
            minusRightLabel.Text = minusRight.ToString();
            minusAnswer = minusLeft - minusRight;

            int timesLeft = random.Next(1, range);
            int timesRight = random.Next(1, range);
            timesLeftLabel.Text = timesLeft.ToString();
            timesRightLabel.Text = timesRight.ToString();
            timesAnswer = timesLeft * timesRight;

            int dividedLeft = random.Next(1, range);
            int dividedRight = random.Next(1, dividedLeft + 1);
            dividedLeftLabel.Text = (dividedLeft * dividedRight).ToString();
            dividedRightLabel.Text = dividedRight.ToString();
            dividedAnswer = dividedLeft * dividedRight / dividedRight;

            timeLeft = 30;
            timeLabel.Text = "Jääk: " + timeLeft + " sekundit";
            quizTime.Start();
            startTime = DateTime.Now;

            checkPlusButton.Click += (s, args) => CheckAnswer(sum, plusAnswer, showPlusAnswerButton);
            checkMinusButton.Click += (s, args) => CheckAnswer(difference, minusAnswer, showMinusAnswerButton);
            checkTimesButton.Click += (s, args) => CheckAnswer(product, timesAnswer, showTimesAnswerButton);
            checkDividedButton.Click += (s, args) => CheckAnswer(quotient, dividedAnswer, showDividedAnswerButton);
        }

        private int GetNumberRange()
        {
            switch (difficultyComboBox.SelectedItem.ToString())
            {
                case "Lihtne": return 10;
                case "Keskmine": return 20;
                case "Raske": return 50;
                default: return 10;
            }
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            if (timeLeft > 0)
            {
                timeLeft--;
                timeLabel.Text = "Jääk: " + timeLeft + " sekundit";
            }
            else
            {
                quizTime.Stop();
                CheckAllAnswers();
                TimeSpan duration = DateTime.Now - startTime;
                MessageBox.Show($"Aeg on läbi! Sa veetsid: {duration.TotalSeconds} sekundit.");
            }
        }

        private void CheckAnswer(NumericUpDown answerBox, int correctAnswer, Button showAnswerButton)
        {
            if ((int)answerBox.Value == correctAnswer)
            {
                MessageBox.Show("Õige!");
                LogResult("Õige");
                correctAnswers++;
                score += 10;
            }
            else
            {
                MessageBox.Show("Vale!");
                showAnswerButton.Visible = true;
                showAnswerButton.Click += (s, args) => MessageBox.Show("Õige vastus on: " + correctAnswer);
                incorrectAnswers++;
                score -= 5;
            }

            UpdateResultsList();
        }

        private void CheckAllAnswers()
        {
            CheckAnswer(sum, plusAnswer, showPlusAnswerButton);
            CheckAnswer(difference, minusAnswer, showMinusAnswerButton);
            CheckAnswer(product, timesAnswer, showTimesAnswerButton);
            CheckAnswer(quotient, dividedAnswer, showDividedAnswerButton);

            string results = "Teie vastused:\n";
            results += $"Suumerimine: {sum.Value} (Õige: {plusAnswer})\n";
            results += $"Lahetumine: {difference.Value} (Õige: {minusAnswer})\n";
            results += $"Korrutamine: {product.Value} (Õige: {timesAnswer})\n";
            results += $"Jagamine: {quotient.Value} (Õige: {dividedAnswer})\n";

            MessageBox.Show(results);
            UpdateResultsList();
        }

        private void UpdateResultsList()
        {
            resultsListBox.Items.Clear();
            resultsListBox.Items.Add("Õiged vastused: " + correctAnswers);
            resultsListBox.Items.Add("Vale vastused: " + incorrectAnswers);
            resultsListBox.Items.Add("Skoor: " + score);
            LogResult("Viktoriin lõpetatud: " + correctAnswers + " õiget, " + incorrectAnswers + " vale, Skoor: " + score);
        }

        private void LogResult(string result)
        {
            string filePath = "quiz_results.txt";
            using (StreamWriter writer = new StreamWriter(filePath, true))
            {
                writer.WriteLine(DateTime.Now.ToString() + ": " + result);
            }
        }

        private void EndQuizButton_Click(object sender, EventArgs e)
        {
            quizTime.Stop();
            CheckAllAnswers();
            MessageBox.Show("Viktoriin lõpetatud.");
        }

        private void ExitButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
