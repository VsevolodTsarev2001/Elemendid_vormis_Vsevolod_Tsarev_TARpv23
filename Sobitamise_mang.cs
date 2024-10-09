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
        public Sobitamise_mang(int w, int h)
        {
            this.Width = w;
            this.Height = h;
            this.Text = "Sobitamise Mang";
            this.BackColor = Color.White; // Начальный цвет фона
        }
    }
}
