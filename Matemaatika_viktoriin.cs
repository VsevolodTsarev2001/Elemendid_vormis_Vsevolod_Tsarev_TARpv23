using System;
using System.Drawing;
using System.Windows.Forms;

namespace Elemendid_vormis_Vsevolod_Tsarev_TARpv23
{
    public partial class Matemaatika_viktoriin : Form
    {
        private Label lblQuestion;
        private TextBox txtAnswer;
        private Button btnSubmit, btnClose;
        private Label lblResult, lblTimer;
        private System.Windows.Forms.Timer timer; // Fully qualified name
        private int currentQuestionIndex = 0;
        private int score = 0;
        private int timeLeft = 15; // time limit for each question in seconds

        private readonly (string question, int answer)[] questions = {
            ("5 + 3", 8),
            ("12 - 4", 8),
            ("3 * 6", 18),
            ("15 / 3", 5),
            ("7 + 9", 16),
            ("18 - 7", 11),
            ("5 * 4", 20),
            ("36 / 6", 6),
            ("10 + 15", 25),
            ("9 - 3", 6)
        };

        // Добавленный конструктор с параметрами
        public Matemaatika_viktoriin(int w, int h)
        {
            InitializeComponent();
            this.Width = w;
            this.Height = h;
            this.Text = "Matemaatika viktoriin";
            this.BackColor = Color.White;

            lblQuestion = new Label { AutoSize = true, Location = new Point(10, 20) };
            this.Controls.Add(lblQuestion);

            txtAnswer = new TextBox { Location = new Point(10, 50) };
            this.Controls.Add(txtAnswer);

            btnSubmit = new Button { Text = "Submit Answer", Location = new Point(10, 80) };
            btnSubmit.Click += BtnSubmit_Click;
            this.Controls.Add(btnSubmit);

            lblResult = new Label { AutoSize = true, Location = new Point(10, 110) };
            this.Controls.Add(lblResult);

            lblTimer = new Label { AutoSize = true, Location = new Point(10, 140) };
            this.Controls.Add(lblTimer);

            btnClose = new Button { Text = "Close", Location = new Point(10, 170) };
            btnClose.Click += BtnClose_Click;
            this.Controls.Add(btnClose);

            timer = new System.Windows.Forms.Timer { Interval = 1000 }; // 1 second
            timer.Tick += Timer_Tick; // Use the Tick event of System.Windows.Forms.Timer

            LoadQuestion();
        }

        private void LoadQuestion()
        {
            if (currentQuestionIndex < questions.Length)
            {
                lblQuestion.Text = questions[currentQuestionIndex].question;
                txtAnswer.Clear();
                txtAnswer.Focus();
                timeLeft = 15; // reset timer for new question
                lblTimer.Text = $"Time left: {timeLeft} seconds";
                timer.Start();
            }
            else
            {
                lblQuestion.Text = "Küsimused on läbi!";
                txtAnswer.Visible = false;
                btnSubmit.Enabled = false;
                timer.Stop();
            }
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            timeLeft--;
            lblTimer.Text = $"Time left: {timeLeft} seconds";

            if (timeLeft <= 0)
            {
                timer.Stop();
                lblResult.Text = $"Aeg on läbi! Õige vastus on {questions[currentQuestionIndex].answer}.";
                currentQuestionIndex++;
                LoadQuestion();
            }
        }

        private void BtnSubmit_Click(object sender, EventArgs e)
        {
            if (int.TryParse(txtAnswer.Text, out int userAnswer))
            {
                timer.Stop();
                if (userAnswer == questions[currentQuestionIndex].answer)
                {
                    score++;
                    lblResult.Text = "Õige vastus!";
                }
                else
                {
                    lblResult.Text = $"Vale vastus! Õige vastus on {questions[currentQuestionIndex].answer}.";
                }
                currentQuestionIndex++;
                LoadQuestion();
            }
            else
            {
                MessageBox.Show("Palun sisestage kehtiv number.");
            }
        }

        private void BtnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
