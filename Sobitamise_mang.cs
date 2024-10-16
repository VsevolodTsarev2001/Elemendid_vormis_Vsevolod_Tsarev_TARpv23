using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Elemendid_vormis_Vsevolod_Tsarev_TARpv23
{
    public partial class Sobitamise_mang : Form
    {
        List<int> numbers = new List<int> { 1, 1, 2, 2, 3, 3, 4, 4, 5, 5, 6, 6 };
        string firstChoice;
        string secondChoice;
        int tries;
        List<PictureBox> pictures = new List<PictureBox>();
        PictureBox picA;
        PictureBox picB;
        Label lblStatus;
        Label lblTimeLeft;
        System.Windows.Forms.Timer GameTimer;
        int totalTime = 60;
        int countDownTime;
        bool gameOver = false;
        public Sobitamise_mang(int w, int h)
        {
            InitializeComponent(); // Инициализация компонентов
            this.ClientSize = new Size(w, h); // Устанавливаем размер окна
            GameTimer = new System.Windows.Forms.Timer(); // Инициализация таймера
            GameTimer.Interval = 1000; // Установка интервала в 1 секунду
            GameTimer.Tick += TimerEvent; // Подписка на событие таймера

            lblStatus = new Label
            {
                Location = new Point(20, 200),
                Size = new Size(200, 30)
            };
            lblTimeLeft = new Label
            {
                Location = new Point(20, 230),
                Size = new Size(200, 30)
            };
            this.Controls.Add(lblStatus);
            this.Controls.Add(lblTimeLeft);

            LoadPictures(); // Загружаем картинки
        }


        private void TimerEvent(object sender, EventArgs e)
        {
            countDownTime--;
            lblTimeLeft.Text = "Time Left: " + countDownTime;
            if (countDownTime < 1)
            {
                GameOver("Time's Up, You Lose");
                foreach (PictureBox x in pictures)
                {
                    if (x.Tag != null)
                    {
                        x.Image = Image.FromFile(@"..\..\..\" + (string)x.Tag + ".png");
                    }
                }
            }
        }

        private void LoadPictures()
        {
            int leftPos = 20;
            int topPos = 20;
            int rows = 0;
            for (int i = 0; i < 12; i++)
            {
                PictureBox newPic = new PictureBox();
                newPic.Height = 50;
                newPic.Width = 50;
                newPic.BackColor = Color.LightGray;
                newPic.SizeMode = PictureBoxSizeMode.StretchImage;
                newPic.Click += NewPic_Click;
                pictures.Add(newPic);
                if (rows < 4)
                {
                    rows++;
                    newPic.Left = leftPos;
                    newPic.Top = topPos;
                    this.Controls.Add(newPic);
                    leftPos += 60;
                }
                if (rows == 4)
                {
                    leftPos = 20;
                    topPos += 60;
                    rows = 0;
                }
            }
            RestartGame();
        }

        private void NewPic_Click(object sender, EventArgs e)
        {
            {
                if (gameOver)
                    // Не регистрировать клик, если игра окончена
                    return;
            }
            if (firstChoice == null)
            {
                picA = sender as PictureBox;
                if (picA.Tag != null && picA.Image == null)
                {
                    picA.Image = Image.FromFile(@"..\..\..\" + (string)picA.Tag + ".png");
                    firstChoice = (string)picA.Tag;
                }
            }
            else if (secondChoice == null)
            {
                picB = sender as PictureBox;
                if (picB.Tag != null && picB.Image == null)
                {
                    picB.Image = Image.FromFile(@"..\..\..\" + (string)picB.Tag + ".png");
                    secondChoice = (string)picB.Tag;
                }
            }
            else
            {
                CheckPictures(picA, picB);
            }
        }

        private void RestartGame()
        {
            // Перемешиваем оригинальный список
            var randomList = numbers.OrderBy(x => Guid.NewGuid()).ToList();
            // Назначаем перемешанный список оригинальному
            numbers = randomList;
            for (int i = 0; i < pictures.Count; i++)
            {
                pictures[i].Image = null;
                pictures[i].Tag = numbers[i].ToString();
            }
            tries = 0;
            lblStatus.Text = "Mismatched: " + tries + " times.";
            lblTimeLeft.Text = "Time Left: " + totalTime;
            gameOver = false;
            countDownTime = totalTime;
            GameTimer.Start(); // Запуск таймера
        }

        private void CheckPictures(PictureBox A, PictureBox B)
        {
            if (firstChoice == secondChoice)
            {
                A.Tag = null;
                B.Tag = null;
            }
            else
            {
                tries++;
                lblStatus.Text = "Mismatched " + tries + " times.";
            }
            firstChoice = null;
            secondChoice = null;
            foreach (PictureBox pics in pictures.ToList())
            {
                if (pics.Tag != null)
                {
                    pics.Image = null;
                }
            }
            // Проверяем, решены ли все элементы
            if (pictures.All(o => o.Tag == null))
            {
                GameOver("Great Work, You Win!!!!");
            }
        }

        private void GameOver(string msg)
        {
            GameTimer.Stop(); // Остановка таймера
            gameOver = true; // Игра окончена
            MessageBox.Show(msg + " Click Restart to Play Again.", "Gleb Says: ");
        }
    }
}
