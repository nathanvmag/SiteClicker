using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SiteClicker
{
    public partial class Adsense : Form
    {
        bool loadImage = false;
        string newfilepath;
        public Adsense()
        {
            InitializeComponent();
            loadImage = false;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(textBox1.Text) && !string.IsNullOrEmpty(textBox2.Text) && !string.IsNullOrEmpty(textBox3.Text) &&
                !string.IsNullOrEmpty(textBox4.Text) && !string.IsNullOrEmpty(textBox5.Text) && !string.IsNullOrEmpty(textBox6.Text) && loadImage&&
                !string.IsNullOrEmpty(newfilepath))
             {
                List<string> websites = new List<string>();
                List<string> titles = new List<string>();
                websites.Add(textBox1.Text);
                websites.Add(textBox4.Text);
                websites.Add(textBox6.Text);
                titles.Add(textBox2.Text);
                titles.Add(textBox3.Text);
                titles.Add(textBox5.Text);
                if (!string.IsNullOrEmpty(textBox8.Text) && !string.IsNullOrEmpty(textBox7.Text)) {
                    websites.Add(textBox8.Text);
                    titles.Add(textBox7.Text);
                }
                if (!string.IsNullOrEmpty(textBox9.Text) && !string.IsNullOrEmpty(textBox10.Text))
                {
                    websites.Add(textBox10.Text);
                    titles.Add(textBox9.Text);
                }
                Form1.adsense ad = new Form1.adsense(websites.ToArray(), titles.ToArray(), newfilepath);
                Form1.addAd(ad);
                MessageBox.Show("Cadastrado com sucesso");
                button3_Click(sender, e);
            }
            else MessageBox.Show("Preencha pelo menos 3 sites e cadastre a imagem");
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Invoke(new Action(() => this.Close()));
        }

        private void button1_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog dlg = new OpenFileDialog())
            {
                dlg.Title = "Open Image";
                dlg.Filter = "Image Files (*.bmp;*.jpg;*.jpeg,*.png)|*.BMP;*.JPG;*.JPEG;*.PNG";

                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    if (!Directory.Exists("adsimage/")) Directory.CreateDirectory("adsimage/");
                    newfilepath = "adsimage/" + RandomString(5)  +Path.GetExtension(dlg.SafeFileName);
                    while (File.Exists(newfilepath))
                    {
                        newfilepath = "adsimage/" + RandomString(5)  + Path.GetExtension(dlg.SafeFileName);
                    }
                    File.Copy(dlg.FileName, newfilepath, true);
                    

                    pictureBox1.Image = new Bitmap(newfilepath);
                    loadImage = true;
                    // Add the new control to its parent's controls collection
                }
            }

            string RandomString(int length)
            {
                Random random = new Random();
                const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
                return new string(Enumerable.Repeat(chars, length)
                  .Select(s => s[random.Next(s.Length)]).ToArray());

            }


        }
    }
}