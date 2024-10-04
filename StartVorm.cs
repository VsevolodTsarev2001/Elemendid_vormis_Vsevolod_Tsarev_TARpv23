namespace Elemendid_vormis_Vsevolod_Tsarev_TARpv23
{
    public partial class StartVorm : Form
    {
        List<string> elemendid = new List<string> { "Nupp", "Silt", "Pilt", "Märkeruut", "Raadionupp", "Tekstikast" };
        List<string> rbtn_list = new List<string> { "Üks", "Kaks", "Kolm"};
        TreeView tree;
        Button btn;
        Label lbl;
        PictureBox pbox;
        CheckBox chk1, chk2;
        RadioButton rbtn, rbtn1, rbtn2, rbtn3;
        TextBox txt;
        int tt = 0;
        int t = 0;

        public StartVorm()
        {
            this.Height = 500;
            this.Width = 700;
            this.Text = "Vorm elementidega";
            this.BackColor = Color.White; // Начальный цвет фона

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

            //nupp - button - Кнопка
            btn = new Button();
            btn.Text = "Vajuta siia";
            btn.Height = 50;
            btn.Width = 70;
            btn.Location = new Point(150, 50);
            btn.Click += Btn_Click;
            btn.DoubleClick += Btn_DoubleClick; // Событие двойного клика

            //silt - label - Метка
            lbl = new Label();
            lbl.Text = "Aknade elemendid c# abil";
            lbl.Font = new Font("Arial", 30, FontStyle.Underline);
            lbl.Size = new Size(500, 50);
            lbl.Location = new Point(150, 0);
            lbl.MouseHover += Lbl_MouseHover;
            lbl.MouseLeave += Lbl_MouseLeave;

            // PictureBox
            pbox = new PictureBox();
            pbox.Size = new Size(60, 80);
            pbox.Location = new Point(150, btn.Height + lbl.Height + 5);
            pbox.SizeMode = PictureBoxSizeMode.Zoom;
            pbox.Image = Image.FromFile(@"..\..\..\Mona_Lisa.PNG");
            pbox.DoubleClick += Pbox_DoubleClick;


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
        }

        private void Tree_AfterSelect(object sender, TreeViewEventArgs e)
        {
            this.BackColor = e.Node.Text switch
            {
                "Nupp" => Color.LightBlue,
                "Silt" => Color.LightGreen,
                "Pilt" => Color.LightCoral,
                "Märkeruut" => Color.LightGoldenrodYellow,
                _ => this.BackColor
            };

            if (e.Node.Text == "Nupp")
            {
                Controls.Add(btn);
            }
            else if (e.Node.Text == "Silt")
            {
                Controls.Add(lbl);
            }
            else if (e.Node.Text == "Pilt")
            {
                Controls.Add(pbox);
            }
            else if (e.Node.Text == "Märkeruut")
            {
                chk1 = new CheckBox();
                chk1.Checked = false;
                chk1.Text = e.Node.Text;
                chk1.Size = new Size(chk1.Text.Length * 10, chk1.Size.Height);
                chk1.Location = new Point(150, btn.Height + lbl.Height + pbox.Height + 10);
                chk1.CheckedChanged += Chk_CheckedChanged;

                chk2 = new CheckBox();
                chk2.Checked = false;
                chk2.BackgroundImage = Image.FromFile(@"..\..\..\mona_lisa6.jpg");
                chk2.BackgroundImageLayout = ImageLayout.Zoom;
                chk2.Size = new Size(100, 100);
                chk2.Location = new Point(150, btn.Height + lbl.Height + pbox.Height + chk1.Height + 15);
                chk2.CheckedChanged += Chk_CheckedChanged;

                Controls.Add(chk1);
                Controls.Add(chk2);
            }
            else if (e.Node.Text == "Raadionupp")
            {
                rbtn1 = new RadioButton();
                rbtn1.Checked = false;
                rbtn1.Text = "Must teema";
                rbtn1.Location = new Point(150, 420);
                rbtn1.CheckedChanged += new EventHandler (Rbtn_CheckedChanged);

                rbtn2 = new RadioButton();
                rbtn2.Checked = false;
                rbtn2.Text = "Valge teema";
                rbtn2.Location = new Point(150, 440);
                rbtn2.CheckedChanged += new EventHandler (Rbtn_CheckedChanged);

                rbtn3 = new RadioButton();
                rbtn3.Checked = false;
                rbtn3.Text = "Roheline teema";
                rbtn3.Location = new Point(150, 460);
                rbtn3.CheckedChanged += new EventHandler (Rbtn_CheckedChanged);

                this.Controls.Add(rbtn1);
                this.Controls.Add(rbtn2);
                this.Controls.Add(rbtn3);
                /*
                //2. variant
                int x = 20;
                for (int i = 0; i < rbtn_list.Count; i++)
                {
                    rbtn = new RadioButton();
                    rbtn.Checked = false;
                    rbtn.Text = rbtn_list[i];
                    rbtn.Height = x;
                    x = x + 20;
                    rbtn.Location = new Point(150, btn.Height + lbl.Height + pbox.Height + chk1.Height + chk2.Height + rbtn.Height);
                    rbtn.CheckedChanged += new EventHandler(Btn_CheckedChanged);

                    Controls.Add(rbtn);
                }
                */
            }
            else if (e.Node.Text == "Tekstikast")
            {
                txt=new TextBox();
                txt.Location = new Point(150+btn.Width+5, btn.Height);
                txt.Font = new Font("Arial", 30);
                txt.Width = 200;
                txt.TextChanged += Txt_TextChanged;
                Controls.Add(txt);
            }
        }

        private void Txt_TextChanged(object? sender, EventArgs e)
        {
            lbl.Text = txt.Text;
        }

        private void Btn_CheckedChanged(object? sender, EventArgs e)
        {
            RadioButton rb = (RadioButton)sender;
            lbl.Text = rb.Text;
        }

        private void Rbtn_CheckedChanged(object? sender, EventArgs e)
        {
            if (rbtn1.Checked)
            {
                this.BackColor = Color.Black;
                this.ForeColor = Color.White;
                rbtn3.ForeColor = Color.White;
                rbtn2.ForeColor = Color.White;
                rbtn1.ForeColor = Color.White;
                
            }
            else if (rbtn2.Checked)
            {
                this.BackColor = Color.Black;
                this.ForeColor = Color.White;
                rbtn3.ForeColor = Color.Black;
                rbtn2.ForeColor = Color.Black;
                rbtn1.ForeColor = Color.Black;
            }
            else if (rbtn2.Checked)
            {
                this.BackColor = Color.Black;
                this.ForeColor = Color.White;
                rbtn3.ForeColor = Color.Green;
                rbtn2.ForeColor = Color.Green;
                rbtn1.ForeColor = Color.Green;
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
