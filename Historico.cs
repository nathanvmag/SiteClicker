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
        List<GoodAds> goodads1, badads1;
        public Historico(List<Form1.Work> works, List<GoodAds> goodads, List<GoodAds> badads)
        {
            InitializeComponent();
            goodads1 = goodads;
            badads1 = badads;
            foreach (Form1.Work w in works)
            {
                dataGridView1.Rows.Add(w.name, w.website,string.IsNullOrWhiteSpace( w.postFB)?string.Empty:w.postFB, String.Format("{0:d/M/yyyy}", w.when));
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            Console.WriteLine("CLICk");
            try
            {
                if (
                  dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex] is DataGridViewTextBoxCell)
                {
                    DialogResult resultt3 = MessageBox.Show("Esta é uma ad boa ou ruim ? (sim - boa , não - ruim, cancelar - fechar)", "Clicker", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                    if (resultt3 == DialogResult.Yes)

                    {
                        AddPerson ad = new AddPerson(goodads1, 0)
                        {
                            Visible = true,
                            StartPosition = FormStartPosition.CenterScreen

                        };
                        ad.Link.Text = dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString();
                        ad.Dono.Text= dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString();
                        ad.textBox3.Text = dataGridView1.Rows[e.RowIndex].Cells[2].Value.ToString();


                    }
                    if (resultt3 == DialogResult.No)
                    {
                        AddPerson ad = new AddPerson(badads1,1)
                        {
                            Visible = true,
                            StartPosition = FormStartPosition.CenterScreen

                        };
                        ad.Link.Text = dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString();
                        ad.Dono.Text = dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString();
                        ad.textBox3.Text = dataGridView1.Rows[e.RowIndex].Cells[2].Value.ToString();
                    }
                }
            }
            catch
            {

            }
        }
    }
}
