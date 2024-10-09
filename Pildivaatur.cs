using System;
using System.Drawing;
using System.Windows.Forms;

namespace Elemendid_vormis_Vsevolod_Tsarev_TARpv23
{
    public partial class Pildivaatur : Form
    {
        CheckBox chb1;
        PictureBox pictureBox;
        Button btnOpen, btn1, btn2, btn3;
        ColorDialog colorDialog1;

        public Pildivaatur(int w, int h)
        {
            InitializeComponent();
            this.Width = w;
            this.Height = h;
            this.Text = "Pildivaatur";
            this.BackColor = Color.White;

            // Инициализация ColorDialog
            colorDialog1 = new ColorDialog();

            // Создаем PictureBox
            pictureBox = new PictureBox();
            pictureBox.Dock = DockStyle.Fill;
            pictureBox.SizeMode = PictureBoxSizeMode.Zoom;
            this.Controls.Add(pictureBox);

            FlowLayoutPanel flp = new FlowLayoutPanel();
            flp.Dock = DockStyle.Bottom;
            this.Controls.Add(flp);

            // Создаем кнопку для открытия файла
            btnOpen = new Button();
            btnOpen.Text = "Avatud pilt";
            btnOpen.Click += BtnOpen_Click;
            flp.Controls.Add(btnOpen);

            btn1 = new Button();
            btn1.Text = "Määrake taustavärv";
            btn1.Click += new EventHandler(BackgroundButton_Click); // Исправлено
            flp.Controls.Add(btn1);

            btn2 = new Button();
            btn2.Text = "Tühjenda pilt";
            btn2.Click += new EventHandler(ClearButton_Click); // Исправлено
            flp.Controls.Add(btn2);

            btn3 = new Button();
            btn3.Text = "Sule";
            btn3.Click += new EventHandler(CloseButton_Click); // Исправлено
            flp.Controls.Add(btn3);

            // Инициализация CheckBox
            chb1 = new CheckBox();
            chb1.Text = "Stretch Image";
            chb1.CheckedChanged += CheckBox1_CheckedChanged;
            flp.Controls.Add(chb1);
        }

        private void BtnOpen_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.InitialDirectory = "c:\\"; // Начальная директория
                openFileDialog.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp;*.gif"; // Фильтр файлов
                openFileDialog.Title = "Valige pildi fail"; // Заголовок диалога

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    // Загружаем выбранное изображение
                    pictureBox.Image = new Bitmap(openFileDialog.FileName);
                }
            }
        }

        private void ClearButton_Click(object sender, EventArgs e)
        {
            // Очистить изображение.
            pictureBox.Image = null;
        }

        private void BackgroundButton_Click(object sender, EventArgs e)
        {
            // Показать диалог выбора цвета. Если пользователь нажимает OK, изменить
            // фон PictureBox на выбранный цвет.
            if (colorDialog1.ShowDialog() == DialogResult.OK)
                pictureBox.BackColor = colorDialog1.Color;
        }

        private void CloseButton_Click(object sender, EventArgs e)
        {
            // Закрыть форму.
            this.Close();
        }

        private void CheckBox1_CheckedChanged(object sender, EventArgs e)
        {
            // Если пользователь выбирает чекбокс Stretch, 
            // изменить свойство SizeMode PictureBox на "Stretch". Если чекбокс очищен, 
            // изменить на "Normal".
            pictureBox.SizeMode = chb1.Checked ? PictureBoxSizeMode.StretchImage : PictureBoxSizeMode.Normal;
        }
    }
}
