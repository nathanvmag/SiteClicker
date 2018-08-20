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
    public partial class editar : Form
    {
        List<Form1.adsense> adslist;
        public editar(List<Form1.adsense> ads)
        {
            InitializeComponent();
            adslist = ads;
            foreach (Form1.adsense ad in ads)
            {
                comboBox1.Items.Add(ad.website[0]);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Invoke(new Action(() => this.Close()));

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(textBox1.Text) && !string.IsNullOrEmpty(textBox2.Text) && !string.IsNullOrEmpty(textBox3.Text) &&
               !string.IsNullOrEmpty(textBox4.Text) && !string.IsNullOrEmpty(textBox5.Text) && !string.IsNullOrEmpty(textBox6.Text) && comboBox1.SelectedItem != null
             )
            {
                foreach (Form1.adsense ad in adslist)
                {
                    if (ad.website[0] == comboBox1.SelectedItem.ToString())

                    {
                        List<string> websites = new List<string>();
                        List<string> titles = new List<string>();
                        websites.Add(textBox1.Text);
                        websites.Add(textBox4.Text);
                        websites.Add(textBox6.Text);
                        titles.Add(textBox2.Text);
                        titles.Add(textBox3.Text);
                        titles.Add(textBox5.Text);
                        if (!string.IsNullOrEmpty(textBox8.Text) && !string.IsNullOrEmpty(textBox7.Text))
                        {
                            websites.Add(textBox8.Text);
                            titles.Add(textBox7.Text);
                        }
                        if (!string.IsNullOrEmpty(textBox9.Text) && !string.IsNullOrEmpty(textBox10.Text))
                        {
                            websites.Add(textBox10.Text);
                            titles.Add(textBox9.Text);
                        }
                        ad.title = titles.ToArray();
                        ad.website = websites.ToArray();

                        Form1.myads = adslist;
                        MessageBox.Show("Editado com sucesso");

                        this.Invoke(new Action(() => this.Close()));
                        break;
                    }
                }

            }
            else MessageBox.Show("Preencha pelo menos 3 sites e Selecione a ad para editar");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Form1.adsense toremove = null;
            foreach (Form1.adsense ad in adslist)
            {
                if (ad.website[0] == comboBox1.SelectedItem.ToString())

                {
                    toremove = ad;
                    break;
                }
            }
            if (toremove != null)
            {
                adslist.Remove(toremove);
                Form1.myads = adslist;
                if (File.Exists(toremove.imagepath)) File.Delete(toremove.imagepath);
                MessageBox.Show("Removido com sucesso");
                button3_Click(sender, e);
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
           
            foreach (Form1.adsense ad in adslist)
            {
                if (ad.website[0] == comboBox1.SelectedItem.ToString())
                {
                    if (ad.website.Length>=3)
                    {
                        textBox1.Text = ad.website[0];
                        textBox4.Text = ad.website[1];
                        textBox6.Text = ad.website[2];
                        textBox2.Text = ad.title[0];
                        textBox3.Text = ad.title[1];
                        textBox5.Text = ad.title[2];
                    }
                    if (ad.website.Length>= 4)
                    {
                        textBox8.Text = ad.website[3];
                        textBox7.Text = ad.title[3];

                    }
                    if (ad.website.Length >= 5)
                    {
                        textBox10.Text = ad.website[4];
                        textBox9.Text = ad.title[4];

                    }
                    break;
                }
            }
        }
    }
}

