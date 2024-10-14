using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace Elemendid_vormis_Vsevolod_Tsarev_TARpv23
{
    public partial class StartVorm : Form
    {
        List<string> elemendid = new List<string> { "Nupp", "Silt", "Pilt", "Märkeruut", "Raadionupp", "Tekstikast", "Loetelu", "Tabel", "Dialoogiaknad" };
        List<string> rbtn_list = new List<string> { "Üks", "Kaks", "Kolm" };
        TreeView tree;
        Button btn;
        Label lbl;
        PictureBox pbox;
        CheckBox chk1, chk2;
        RadioButton rbtn1, rbtn2, rbtn3, rbtn4, rbtn5, rbtn6;
        TextBox txt;
        ListBox lb;
        DataSet ds;
        DataGridView dg;
        int tt = 0;
        int t = 0;

        public StartVorm()
        {
            this.Height = 500;
            this.Width = 700;
            this.Text = "Vorm elementidega";
            this.BackColor = Color.White;

            tree = new TreeView();
            tree.Dock = DockStyle.Left;
            tree.AfterSelect += Tree_AfterSelect;
            TreeNode tn = new TreeNode("Elemendid:");
            foreach (var element in elemendid)
            {
                tn.Nodes.Add(new TreeNode(element));
            }

            tree.Nodes.Add(tn);
            this.Controls.Add(tree);

            btn = new Button();
            btn.Text = "Vajuta siia";
            btn.Height = 50;
            btn.Width = 70;
            btn.Location = new Point(150, 50);
            btn.Click += Btn_Click;
            btn.DoubleClick += Btn_DoubleClick;

            lbl = new Label();
            lbl.Text = "Aknade elemendid c# abil";
            lbl.Font = new Font("Arial", 30, FontStyle.Underline);
            lbl.Size = new Size(500, 50);
            lbl.Location = new Point(150, 0);
            lbl.MouseHover += Lbl_MouseHover;
            lbl.MouseLeave += Lbl_MouseLeave;

            pbox = new PictureBox();
            pbox.Size = new Size(60, 80);
            pbox.Location = new Point(150, btn.Height + lbl.Height + 5);
            pbox.SizeMode = PictureBoxSizeMode.Zoom;
            pbox.Image = Image.FromFile(@"..\..\..\Mona_Lisa.PNG");
            pbox.DoubleClick += Pbox_DoubleClick;

            this.Controls.Add(btn);
            this.Controls.Add(lbl);
            this.Controls.Add(pbox);
        }

        private void Btn_DoubleClick(object sender, EventArgs e)
        {
            btn.Text = btn.Text == "Vajuta siia" ? "Sa vajutasid!" : "Vajuta siia";
        }

        private void Pbox_DoubleClick(object sender, EventArgs e)
        {
            string[] pildid = { "Mona_Lisa.PNG", "mona_lisa2.jpg", "mona_lisa3.jpg", "mona_lisa4.jpg", "mona_lisa5.jpg" };
            string fail = pildid[tt];
            pbox.Image = Image.FromFile(@"..\..\..\" + fail);
            tt++;
            if (tt == 5) { tt = 0; }
        }

        private void Lbl_MouseLeave(object sender, EventArgs e)
        {
            lbl.Font = new Font("Arial", 35, FontStyle.Underline);
            lbl.ForeColor = Color.Wheat;
        }

        private void Lbl_MouseHover(object sender, EventArgs e)
        {
            lbl.Font = new Font("Arial", 28, FontStyle.Bold);
            lbl.ForeColor = Color.Violet;
        }

        private void Btn_Click(object sender, EventArgs e)
        {
            t++;
            btn.BackColor = t % 2 == 0 ? Color.Red : Color.Blue;
            Pildivaatur teineVorm = new Pildivaatur(200, 200);
            teineVorm.Show();
        }

        private void Tree_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (e.Node.Text == "Nupp") Controls.Add(btn);
            else if (e.Node.Text == "Silt") Controls.Add(lbl);
            else if (e.Node.Text == "Pilt") Controls.Add(pbox);
            else if (e.Node.Text == "Märkeruut") CreateCheckBoxes(e.Node);
            else if (e.Node.Text == "Raadionupp") CreateRadioButtons();
            else if (e.Node.Text == "Tekstikast") CreateTextBox();
            else if (e.Node.Text == "Loetelu") CreateListBox();
            else if (e.Node.Text == "Tabel") CreateDataGridView();
            else if (e.Node.Text == "Dialoogiaknad") ShowDialog();
        }

        private void CreateCheckBoxes(TreeNode eNode)
        {
            chk1 = new CheckBox { Checked = false, Text = eNode.Text, Size = new Size(100, 20), Location = new Point(150, btn.Height + lbl.Height + pbox.Height + 10) };
            chk1.CheckedChanged += Chk_CheckedChanged;

            chk2 = new CheckBox { Checked = false, BackgroundImage = Image.FromFile(@"..\..\..\mona_lisa6.jpg"), BackgroundImageLayout = ImageLayout.Zoom, Size = new Size(100, 100), Location = new Point(150, btn.Height + lbl.Height + pbox.Height + chk1.Height + 15) };
            chk2.CheckedChanged += Chk_CheckedChanged;

            Controls.Add(chk1);
            Controls.Add(chk2);
        }

        private void CreateRadioButtons()
        {
            rbtn1 = new RadioButton { Text = "Must teema", Location = new Point(150, 380) };
            rbtn1.CheckedChanged += Rbtn_CheckedChanged;

            rbtn2 = new RadioButton { Text = "Valge teema", Location = new Point(150, 400) };
            rbtn2.CheckedChanged += Rbtn_CheckedChanged;

            rbtn3 = new RadioButton { Text = "Roheline teema", Location = new Point(150, 420) };
            rbtn3.CheckedChanged += Rbtn_CheckedChanged;

            rbtn4 = new RadioButton { Text = "pildivaatur", Location = new Point(550, 380) };

            rbtn4.Click += Pildivaatur;

            rbtn5 = new RadioButton { Text = "matemaatika viktoriin", Location = new Point(550, 400) };
            rbtn5.Click += matemaatika_viktoriin;

            rbtn6 = new RadioButton { Text = "sobitamise mäng", Location = new Point(550, 420) };
            rbtn6.Click += sobitamise_mang;

            Controls.AddRange(new Control[] { rbtn1, rbtn2, rbtn3, rbtn4, rbtn5, rbtn6 });
        }

        private void CreateTextBox()
        {
            txt = new TextBox { Location = new Point(150 + btn.Width + 5, btn.Height), Font = new Font("Arial", 30), Width = 200 };
            txt.TextChanged += Txt_TextChanged;
            Controls.Add(txt);
        }

        private void CreateListBox()
        {
            lb = new ListBox();
            foreach (string item in rbtn_list)
            {
                lb.Items.Add(item);
            }
            lb.Height = 50;
            lb.Location = new Point(160 + btn.Width + txt.Width, btn.Height);
            lb.SelectedIndexChanged += Lb_SelectedIndexChanged;
            Controls.Add(lb);
        }

        private void CreateDataGridView()
        {
            ds = new DataSet("XML fail");
            ds.ReadXml(@"..\..\..\menu.xml");
            dg = new DataGridView { Location = new Point(160 + chk1.Width, lbl.Height + 10), DataSource = ds, DataMember = "food" };
            dg.RowHeaderMouseClick += Dg_RowHeaderMouseClick;
            Controls.Add(dg);
        }

        private void ShowDialog()
        {
            MessageBox.Show("Dialoog", "See on lihtne aken");
            var vastus = MessageBox.Show("Sisestame andmed", "Kas tahad InputBoxi kasutada?", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (vastus == DialogResult.Yes)
            {
                string text = Interaction.InputBox("Sisesta midagi siia", "andmete sisestamine");
                MessageBox.Show("Oli sisestatud" + text);
            }
        }

        private void sobitamise_mang(object sender, EventArgs e)
        {
            Sobitamise_mang teineVorm = new Sobitamise_mang(200, 200);
            teineVorm.Show();
        }

        private void matemaatika_viktoriin(object sender, EventArgs e)
        {
            Matemaatika_viktoriin kolmasVorm = new Matemaatika_viktoriin(200, 200);
            kolmasVorm.Show();
        }

        private void Pildivaatur(object sender, EventArgs e)
        {
            int w = 700;
            int h = 500;

            Pildivaatur neljasVorm = new Pildivaatur(w, h);
            neljasVorm.Show();
        }

        private void Dg_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            txt.Text = dg.Rows[e.RowIndex].Cells[0].Value.ToString() + " hind " + dg.Rows[e.RowIndex].Cells[1].Value.ToString();
        }

        private void Lb_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (lb.SelectedIndex)
            {
                case 0: tree.BackColor = Color.MistyRose; break;
                case 1: tree.BackColor = Color.SlateGray; break;
                case 2: tree.BackColor = Color.Crimson; break;
            }
        }

        private void Txt_TextChanged(object sender, EventArgs e)
        {
            lbl.Text = txt.Text;
        }

        private void Rbtn_CheckedChanged(object sender, EventArgs e)
        {
            if (rbtn1.Checked)
            {
                this.BackColor = Color.Black;
                this.ForeColor = Color.White;
            }
            else if (rbtn2.Checked)
            {
                this.BackColor = Color.White;
                this.ForeColor = Color.Black;
            }
            else if (rbtn3.Checked)
            {
                this.BackColor = Color.Green;
                this.ForeColor = Color.Blue;
            }
        }

        private void Chk_CheckedChanged(object sender, EventArgs e)
        {
            if (chk1.Checked && chk2.Checked)
            {
                lbl.BorderStyle = BorderStyle.Fixed3D;
                pbox.BorderStyle = BorderStyle.Fixed3D;
            }
            else if (chk1.Checked)
            {
                lbl.BorderStyle = BorderStyle.Fixed3D;
                pbox.BorderStyle = BorderStyle.None;
            }
            else if (chk2.Checked)
            {
                pbox.BorderStyle = BorderStyle.Fixed3D;
                lbl.BorderStyle = BorderStyle.None;
            }
            else
            {
                lbl.BorderStyle = BorderStyle.None;
                pbox.BorderStyle = BorderStyle.None;
            }
        }
    }
}
