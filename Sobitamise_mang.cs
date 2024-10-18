using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace Elemendid_vormis_Vsevolod_Tsarev_TARpv23
{
    public partial class Sobitamise_mang : Form
    {
        enum Raskusaste { Lihtne, Keskmine, Raskem }
        private Raskusaste valitudRaskusaste = Raskusaste.Lihtne;
        List<int> numbrid = new List<int>();
        System.Drawing.Image esimeneValik;
        System.Drawing.Image teineValik;
        int katsed;
        List<PictureBox> pildid = new List<PictureBox>();
        PictureBox picA;
        PictureBox picB;
        Label lblOlek;
        Label lblAegJäänud;
        System.Windows.Forms.Timer MänguKell;
        int koguaeg;
        int allahindlusaeg;
        bool mängLõppenud = false;

        // Images list to store your actual images instead of file paths
        List<Image> pildiKogum = new List<Image>();

        public Sobitamise_mang(int w, int h)
        {
            InitializeComponent();
            this.ClientSize = new Size(w, h);
            MänguKell = new System.Windows.Forms.Timer();
            MänguKell.Interval = 1000;
            MänguKell.Tick += KellEvent;

            lblOlek = new Label
            {
                Location = new Point(20, 200),
                Size = new Size(200, 30)
            };
            lblAegJäänud = new Label
            {
                Location = new Point(20, 230),
                Size = new Size(200, 30)
            };
            this.Controls.Add(lblOlek);
            this.Controls.Add(lblAegJäänud);

            AlustaRaskusastmeValijat();
            LaePildid();  // Загрузка изображений
        }

        private void AlustaRaskusastmeValijat()
        {
            ComboBox raskusastmeValija = new ComboBox
            {
                Location = new Point(20, 170),
                Size = new Size(120, 30)
            };
            raskusastmeValija.Items.AddRange(Enum.GetNames(typeof(Raskusaste)));
            raskusastmeValija.SelectedIndex = 0; // По умолчанию Lihtne
            raskusastmeValija.SelectedIndexChanged += RaskusastmeValija_SelectedIndexChanged;
            this.Controls.Add(raskusastmeValija);
        }

        private void RaskusastmeValija_SelectedIndexChanged(object sender, EventArgs e)
        {
            valitudRaskusaste = (Raskusaste)Enum.Parse(typeof(Raskusaste), ((ComboBox)sender).SelectedItem.ToString());
            TaastaMäng();  // Перезапуск игры при изменении сложности
        }

        private void KellEvent(object sender, EventArgs e)
        {
            allahindlusaeg--;
            lblAegJäänud.Text = "Aeg Jäänud: " + allahindlusaeg;
            if (allahindlusaeg < 1)
            {
                MängLõppenud("Aeg on läbi, kaotasite");
                foreach (PictureBox x in pildid)
                {
                    if (x.Tag != null)
                    {
                        x.Image = (Image)x.Tag; // Восстановление изображения при завершении
                    }
                }
            }
        }

        private void LaePildid()
        {
            // Загружаем картинки сразу в список изображений
            pildiKogum = new List<Image>
            {
                Image.FromFile(@"..\..\..\css.png"),
                Image.FromFile(@"..\..\..\java.png"),
                Image.FromFile(@"..\..\..\py.png"),
                Image.FromFile(@"..\..\..\sql.png"),
                Image.FromFile(@"..\..\..\swift.png")
            };

            // Очистка существующих PictureBoxes
            pildid.Clear();
            int paarideArv = SaaPaarideArv(valitudRaskusaste);
            numbrid = Enumerable.Range(0, paarideArv).SelectMany(i => new[] { i, i }).ToList();
            Segage(numbrid);

            int vasakPos = 20;
            int üleminePos = 20;
            int read = 0;

            for (int i = 0; i < numbrid.Count; i++)
            {
                PictureBox uusPic = new PictureBox
                {
                    Height = 50,
                    Width = 50,
                    BackColor = Color.LightGray,
                    SizeMode = PictureBoxSizeMode.StretchImage
                };
                uusPic.Click += UusPic_Click;
                pildid.Add(uusPic);
                uusPic.Left = vasakPos;
                uusPic.Top = üleminePos;
                this.Controls.Add(uusPic);

                vasakPos += 60;
                read++;
                if (read == 4)
                {
                    vasakPos = 20;
                    üleminePos += 60;
                    read = 0;
                }
            }

            TaastaMäng();  // Восстановление состояния игры
        }

        private int SaaPaarideArv(Raskusaste raskusaste)
        {
            return raskusaste switch
            {
                Raskusaste.Lihtne => 5,  // 6 пар
                Raskusaste.Keskmine => 8,  // 8 пар
                Raskusaste.Raskem => 10,  // 10 пар
                _ => 5,
            };
        }

        private void Segage(List<int> nimekiri)
        {
            Random rng = new Random();
            int n = nimekiri.Count;
            while (n > 1)
            {
                int k = rng.Next(n--);
                int temp = nimekiri[n];
                nimekiri[n] = nimekiri[k];
                nimekiri[k] = temp;
            }
        }

        private void TaastaMäng()
        {
            Segage(numbrid);
            for (int i = 0; i < pildid.Count; i++)
            {
                pildid[i].Image = null;
                pildid[i].Tag = pildiKogum[numbrid[i]];  // Связываем картинку с PictureBox через Tag
            }

            katsed = 0;
            lblOlek.Text = "Mismatched: " + katsed + " korda.";
            lblAegJäänud.Text = "Aeg Jäänud: " + koguaeg;
            mängLõppenud = false;
            allahindlusaeg = koguaeg;
            koguaeg = 60;  // Сброс таймера на 60 секунд
            MänguKell.Start();
        }

        private void UusPic_Click(object sender, EventArgs e)
        {
            if (mängLõppenud)
                return;

            if (esimeneValik == null)
            {
                picA = sender as PictureBox;
                if (picA.Tag != null && picA.Image == null)
                {
                    picA.Image = (Image)picA.Tag;  // Загружаем изображение из Tag
                    esimeneValik = (Image)picA.Tag;
                }
            }
            else if (teineValik == null)
            {
                picB = sender as PictureBox;
                if (picB.Tag != null && picB.Image == null)
                {
                    picB.Image = (Image)picB.Tag;  // Загружаем изображение из Tag
                    teineValik = (Image)picB.Tag;
                }
            }
            else
            {
                KontrolliPilti(picA, picB);
            }
        }

        private void KontrolliPilti(PictureBox A, PictureBox B)
        {
            if (esimeneValik == teineValik)
            {
                A.Tag = null;
                B.Tag = null;
            }
            else
            {
                katsed++;
                lblOlek.Text = "Mismatched " + katsed + " korda.";
            }
            esimeneValik = null;
            teineValik = null;

            foreach (PictureBox pics in pildid.ToList())
            {
                if (pics.Tag != null)
                {
                    pics.Image = null;
                }
            }

            if (pildid.All(o => o.Tag == null))
            {   
                MängLõppenud("Suurepärane töö, võitsite!!!!");
            }
        }

        private void MängLõppenud(string msg)
        {
            MänguKell.Stop();
            mängLõppenud = true;
            MessageBox.Show(msg + " Kliki Taasta, et uuesti mängida.");
        }
    }
}
