using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;

namespace Elemendid_vormis_Vsevolod_Tsarev_TARpv23
{
    public partial class Pildivaatur : Form
    {
        CheckBox chb1;
        PictureBox pictureBox;
        Button btnOpen, btnSave, btnClear, btnBackground, btnClose, btnRotate, btnGrayscale;
        TrackBar opacityTrackBar;
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

            // Создаем кнопку для сохранения изображения
            btnSave = new Button();
            btnSave.Text = "Salvesta pilt";
            btnSave.Click += SaveButton_Click;
            flp.Controls.Add(btnSave);

            btnBackground = new Button();
            btnBackground.Text = "Määrake taustavärv";
            btnBackground.Click += BackgroundButton_Click;
            flp.Controls.Add(btnBackground);

            btnClear = new Button();
            btnClear.Text = "Tühjenda pilt";
            btnClear.Click += ClearButton_Click;
            flp.Controls.Add(btnClear);

            btnClose = new Button();
            btnClose.Text = "Sule";
            btnClose.Click += CloseButton_Click;
            flp.Controls.Add(btnClose);

            // Кнопка поворота изображения
            btnRotate = new Button();
            btnRotate.Text = "Pööra 90°";
            btnRotate.Click += RotateButton_Click;
            flp.Controls.Add(btnRotate);

            // Кнопка черно-белого фильтра
            btnGrayscale = new Button();
            btnGrayscale.Text = "Halltoon";
            btnGrayscale.Click += GrayscaleButton_Click;
            flp.Controls.Add(btnGrayscale);

            // Инициализация CheckBox
            chb1 = new CheckBox();
            chb1.Text = "Stretch Image";
            chb1.CheckedChanged += CheckBox1_CheckedChanged;
            flp.Controls.Add(chb1);

            // Инициализация TrackBar для регулировки прозрачности
            opacityTrackBar = new TrackBar();
            opacityTrackBar.Minimum = 0;
            opacityTrackBar.Maximum = 100;
            opacityTrackBar.Value = 100;
            opacityTrackBar.Scroll += (s, e) =>
            {
                if (pictureBox.Image != null)
                {
                    pictureBox.Image = AdjustOpacity(new Bitmap(pictureBox.Image), opacityTrackBar.Value / 100f);
                }
            };
            flp.Controls.Add(opacityTrackBar);
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

        private void SaveButton_Click(object sender, EventArgs e)
        {
            if (pictureBox.Image != null)
            {
                using (SaveFileDialog saveFileDialog = new SaveFileDialog())
                {
                    saveFileDialog.Filter = "PNG|*.png|JPEG|*.jpg|BMP|*.bmp";
                    saveFileDialog.Title = "Сохранить изображение как";
                    if (saveFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        pictureBox.Image.Save(saveFileDialog.FileName);
                    }
                }
            }
            else
            {
                MessageBox.Show("Нет изображения для сохранения.");
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

        private void RotateButton_Click(object sender, EventArgs e)
        {
            if (pictureBox.Image != null)
            {
                pictureBox.Image.RotateFlip(RotateFlipType.Rotate90FlipNone);
                pictureBox.Refresh();
            }
        }

        private void GrayscaleButton_Click(object sender, EventArgs e)
        {
            if (pictureBox.Image != null)
            {
                pictureBox.Image = ApplyGrayscale(new Bitmap(pictureBox.Image));
            }
        }

        private Bitmap ApplyGrayscale(Bitmap original)
        {
            for (int y = 0; y < original.Height; y++)
            {
                for (int x = 0; x < original.Width; x++)
                {
                    Color pixel = original.GetPixel(x, y);
                    int gray = (int)(pixel.R * 0.3 + pixel.G * 0.59 + pixel.B * 0.11);
                    Color newColor = Color.FromArgb(gray, gray, gray);
                    original.SetPixel(x, y, newColor);
                }
            }
            return original;
        }

        private Bitmap AdjustOpacity(Bitmap img, float opacity)
        {
            Bitmap result = new Bitmap(img.Width, img.Height);
            using (Graphics g = Graphics.FromImage(result))
            {
                ColorMatrix matrix = new ColorMatrix();
                matrix.Matrix33 = opacity; // Установка прозрачности
                using (ImageAttributes attrs = new ImageAttributes())
                {
                    attrs.SetColorMatrix(matrix);
                    g.DrawImage(img, new Rectangle(0, 0, img.Width, img.Height), 0, 0, img.Width, img.Height, GraphicsUnit.Pixel, attrs);
                }
            }
            return result;
        }
    }
}
