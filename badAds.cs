using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SiteClicker
{
    public partial class badAds : Form
    {
        List<GoodAds> goodAds;

        public badAds()
        {
            InitializeComponent();
            if (File.Exists("badads.txt"))
            {
                StreamReader sr = new StreamReader("badads.txt");
                goodAds = JsonConvert.DeserializeObject<List<GoodAds>>(sr.ReadToEnd());
                sr.Close();
            }
            else goodAds = new List<GoodAds>();

            foreach (GoodAds ga in goodAds)
            {
                comboBox1.Items.Add(ga.nome);
                dataGridView1.Rows.Add(ga.nome, ga.bloglink, ga.fblink, ga.qntsabas, ga.totaltime );
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            AddPerson ad = new AddPerson(goodAds,1);
            ad.ShowDialog();
            dataGridView1.Rows.Clear();
            comboBox1.Items.Clear();
            foreach (GoodAds ga in goodAds)
            {
                comboBox1.Items.Add(ga.nome);
                dataGridView1.Rows.Add(ga.nome, ga.bloglink, ga.fblink, ga.qntsabas );
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
                        dataGridView1.Rows.Add(ga.nome, ga.bloglink, ga.fblink, ga.qntsabas, ga.totaltime);
                    }
                    string serialized = JsonConvert.SerializeObject(goodAds);
                    if (File.Exists("badads.xt")) File.Delete("badads.txt");
                    StreamWriter sw = new StreamWriter("badads.txt", false, Encoding.UTF8);
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

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex] is DataGridViewLinkCell)
            {
                try
                {
                    System.Diagnostics.Process.Start(dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString());
                }
                catch
                {
                    MessageBox.Show("Não foi possível abrir o site selecionado");
                }
            }
        }
    }
}
