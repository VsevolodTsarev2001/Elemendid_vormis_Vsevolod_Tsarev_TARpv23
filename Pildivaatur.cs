using System;
using System.Collections.Generic; // Added for Stack
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;

namespace Elemendid_vormis_Vsevolod_Tsarev_TARpv23
{
    public partial class Pildivaatur : Form
    {
        CheckBox chb1;
        PictureBox pictureBox;
        Button btnOpen, btnSave, btnClear, btnBackground, btnClose, btnRotateCW, btnRotateCCW, btnGrayscale, btnUndo;
        TrackBar opacityTrackBar;
        ColorDialog colorDialog1;
        Stack<Bitmap> undoStack; // Added for undo functionality

        public Pildivaatur(int w, int h)
        {
            InitializeComponent();
            this.Width = w;
            this.Height = h;
            this.Text = "Pildivaatur";
            this.BackColor = Color.White;

            colorDialog1 = new ColorDialog();
            undoStack = new Stack<Bitmap>(); // Initialize undo stack

            pictureBox = new PictureBox();
            pictureBox.Dock = DockStyle.Fill;
            pictureBox.SizeMode = PictureBoxSizeMode.Zoom;
            this.Controls.Add(pictureBox);

            FlowLayoutPanel flp = new FlowLayoutPanel();
            flp.Dock = DockStyle.Bottom;
            this.Controls.Add(flp);

            btnOpen = new Button();
            btnOpen.Text = "Avatud pilt";
            btnOpen.Click += BtnOpen_Click;
            flp.Controls.Add(btnOpen);

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

            btnRotateCW = new Button();
            btnRotateCW.Text = "Pööra 90° CW";
            btnRotateCW.Click += RotateClockwiseButton_Click;
            flp.Controls.Add(btnRotateCW);

            btnRotateCCW = new Button();
            btnRotateCCW.Text = "Pööra 90° CCW";
            btnRotateCCW.Click += RotateCounterClockwiseButton_Click;
            flp.Controls.Add(btnRotateCCW);

            btnGrayscale = new Button();
            btnGrayscale.Text = "Halltoon";
            btnGrayscale.Click += GrayscaleButton_Click;
            flp.Controls.Add(btnGrayscale);

            btnUndo = new Button();
            btnUndo.Text = "Tühista";
            btnUndo.Click += UndoButton_Click;
            flp.Controls.Add(btnUndo);

            chb1 = new CheckBox();
            chb1.Text = "Stretch Image";
            chb1.CheckedChanged += CheckBox1_CheckedChanged;
            flp.Controls.Add(chb1);

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
                openFileDialog.InitialDirectory = "c:\\";
                openFileDialog.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp;*.gif";
                openFileDialog.Title = "Valige pildi fail";

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    SaveCurrentImage(); // Save current image before opening a new one
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
            pictureBox.Image = null;
            undoStack.Clear(); // Clear the undo stack
        }

        private void BackgroundButton_Click(object sender, EventArgs e)
        {
            if (colorDialog1.ShowDialog() == DialogResult.OK)
                pictureBox.BackColor = colorDialog1.Color;
        }

        private void CloseButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void CheckBox1_CheckedChanged(object sender, EventArgs e)
        {
            pictureBox.SizeMode = chb1.Checked ? PictureBoxSizeMode.StretchImage : PictureBoxSizeMode.Normal;
        }

        private void RotateClockwiseButton_Click(object sender, EventArgs e)
        {
            if (pictureBox.Image != null)
            {
                SaveCurrentImage(); // Save the current image before rotating
                pictureBox.Image.RotateFlip(RotateFlipType.Rotate90FlipNone);
                pictureBox.Refresh();
            }
        }

        private void RotateCounterClockwiseButton_Click(object sender, EventArgs e)
        {
            if (pictureBox.Image != null)
            {
                SaveCurrentImage(); // Save the current image before rotating
                pictureBox.Image.RotateFlip(RotateFlipType.Rotate270FlipNone);
                pictureBox.Refresh();
            }
        }

        private void GrayscaleButton_Click(object sender, EventArgs e)
        {
            if (pictureBox.Image != null)
            {
                SaveCurrentImage(); // Save the current image before applying grayscale
                pictureBox.Image = ApplyGrayscale(new Bitmap(pictureBox.Image));
            }
        }

        private void UndoButton_Click(object sender, EventArgs e)
        {
            if (undoStack.Count > 0)
            {
                pictureBox.Image = undoStack.Pop(); // Pop the last image from the stack
                pictureBox.Refresh();
            }
            else
            {
                MessageBox.Show("Ei ole rohkem tühistamist.");
            }
        }

        private void SaveCurrentImage()
        {
            if (pictureBox.Image != null)
            {
                undoStack.Push(new Bitmap(pictureBox.Image)); // Push the current image to the stack
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
                matrix.Matrix33 = opacity;
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
