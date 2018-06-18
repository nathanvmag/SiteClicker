using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SiteClicker
{
    public partial class Historico : Form
    {
        public Historico(List<Form1.Work>works)
        {
            InitializeComponent();
            foreach (Form1.Work w in works)
            {
                dataGridView1.Rows.Add(w.name, w.website, w.postFB, String.Format("{0:d/M/yyyy}", w.when));
            }
        }
    }
}
