using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SiteClicker
{
    public partial class Godads : Form
    {
        Form1 me;
        List<GoodAds> goodAds;
        public Godads(Form1 f)
        {
            InitializeComponent();
            me = f;
            if (File.Exists("goodads.txt"))
            {
                StreamReader sr = new StreamReader("goodads.txt");
                goodAds = JsonConvert.DeserializeObject<List<GoodAds>>(sr.ReadToEnd());
                sr.Close();
            }
            else goodAds = new List<GoodAds>();

            foreach (GoodAds ga in goodAds)
            {
                comboBox1.Items.Add(ga.nome);

                dataGridView1.Rows.Add(ga.nome, ga.bloglink,string.IsNullOrEmpty( ga.fblink)?" ":ga.fblink, ga.qntsabas, ga.totaltime,ga.when==null?"": String.Format("{0:d/M/yyyy}", ga.when), "Clicar");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            AddPerson ad = new AddPerson(goodAds,0);
            ad.ShowDialog();
            dataGridView1.Rows.Clear();
            comboBox1.Items.Clear();
            foreach (GoodAds ga in goodAds)
            {
                comboBox1.Items.Add(ga.nome);
                dataGridView1.Rows.Add(ga.nome, ga.bloglink, string.IsNullOrEmpty(ga.fblink) ? " " : ga.fblink, ga.qntsabas, ga.totaltime, ga.when == null ? "" : String.Format("{0:d/M/yyyy}", ga.when), "Clicar");
                
            }

        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedItem != null)
            {
                DialogResult result = MessageBox.Show("Deseja remover usuário " + comboBox1.SelectedItem.ToString() + " ?", "Clicker", MessageBoxButtons.YesNo);
                if (result == DialogResult.Yes)
                {
                    
                    goodAds.RemoveAt(comboBox1.SelectedIndex);
                    dataGridView1.Rows.Clear();
                    comboBox1.Items.Clear();
                    foreach (GoodAds ga in goodAds)
                    {
                        comboBox1.Items.Add(ga.nome);
                        dataGridView1.Rows.Add(ga.nome, ga.bloglink, string.IsNullOrEmpty(ga.fblink) ? " " : ga.fblink, ga.qntsabas, ga.totaltime, ga.when == null ? "" : String.Format("{0:d/M/yyyy}", ga.when));
                    }
                    string serialized = JsonConvert.SerializeObject(goodAds);
                    if (File.Exists("goodads.txt")) File.Delete("goodads.txt");
                    StreamWriter sw = new StreamWriter("goodads.txt", false, Encoding.UTF8);
                    sw.Write(serialized);
                    sw.Close();

                }
            }

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            string searchValue = textBox1.Text;
            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            try
            {
                bool valueResult = false;
                foreach (DataGridViewRow row in dataGridView1.Rows)
                {
                    for (int i = 0; i < row.Cells.Count; i++)
                    {
                        if (row.Cells[i].Value != null && row.Cells[i].Value.ToString().Equals(searchValue))
                        {
                           for (int z = 0; z < dataGridView1.Rows.Count; z++)
                            {
                                dataGridView1.Rows[z].Selected = false;
                            }
                            int rowIndex = row.Index;
                            dataGridView1.Rows[rowIndex].Selected = true;
                            valueResult = true;
                            break;
                        }
                    }

                }
                if (!valueResult)
                {
                    
                }
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message);
            }

        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (
                  dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex] is DataGridViewButtonCell)
                {
                    me.putiten(dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString(), dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString(),
                        dataGridView1.Rows[e.RowIndex].Cells[2].Value.ToString(),
                        int.Parse(dataGridView1.Rows[e.RowIndex].Cells[3].Value.ToString()),
                        int.Parse(dataGridView1.Rows[e.RowIndex].Cells[4].Value.ToString()));
                    foreach (GoodAds ga in goodAds)
                    {
                        if (ga.nome == dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString() && ga.bloglink == dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString() && ga.fblink == dataGridView1.Rows[e.RowIndex].Cells[2].Value.ToString())
                        {
                            ga.when = DateTime.Now;
                        }
                    }
                    string serialized = JsonConvert.SerializeObject(goodAds);
                    if (File.Exists("goodads.txt")) File.Delete("goodads.txt");
                    StreamWriter sw = new StreamWriter("goodads.txt", false, Encoding.UTF8);
                    sw.Write(serialized);
                    sw.Close();
                    this.Close();
                }
            }
            catch
            {

            }
            try
            {
                if (dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex] is DataGridViewLinkCell)
                {
                    try
                    {
                        Process.Start(dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString());
                    }
                    catch
                    {
                        MessageBox.Show("Não foi possível abrir o site selecionado");
                    }
                }
            }
            catch
            {

            }


        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
