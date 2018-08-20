using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SiteClicker
{
    public partial class postgerador : Form
    {
        class Post
        {
            public string tx1, tx2;
            public int int1, int2, int3;
        }
        Post px;
        public postgerador()
        {
            
            InitializeComponent();
            if (File.Exists("oldpost.txt"))
            {
                StreamReader sr = new StreamReader("oldpost.txt");
                px = JsonConvert.DeserializeObject<Post>(sr.ReadToEnd());
                sr.Close();

                textBox2.Text= px.tx1;
                textBox1.Text= px.tx2;
                numericUpDown1.Value=(decimal) px.int1;
                numericUpDown2.Value=(decimal) px.int2;
                numericUpDown3.Value= (decimal)px.int3;
            }
            else px = new Post();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(textBox1.Text) && !string.IsNullOrEmpty(textBox2.Text) && numericUpDown1.Value != 0 && numericUpDown2.Value != 0 && numericUpDown3.Value != 0)
            {

                string ftext = string.Format(@"MY COUNTRY: {0}
COUNTRIES I DONT ACCEPT: All are welcome
HOW MANY COMMENTS: Max
MY WEBSITE: {1}
MY RULES: {2}/{3}/{4}
SIGN-UP / DOWNLOAD: No
WAITING TIME TO CLICK FOR ME: 24 HR
MY IP: dynamic",textBox2.Text,textBox1.Text,numericUpDown1.Value,numericUpDown2.Value,numericUpDown3.Value);
                Thread thread = new Thread(() => Clipboard.SetText(ftext));
                thread.SetApartmentState(ApartmentState.STA); //Set the thread to STA
                thread.Start();
                thread.Join();
                px.tx1 = textBox2.Text;
                px.tx2 = textBox1.Text;
                px.int1 = (int)numericUpDown1.Value;
                px.int2 = (int)numericUpDown2.Value;
                px.int3 = (int)numericUpDown3.Value;

                MessageBox.Show("Copiado com sucesso");
                string serialized = JsonConvert.SerializeObject(px);
                if (File.Exists("oldpost.txt")) File.Delete("oldpost.txt");
                StreamWriter sw = new StreamWriter("oldpost.txt", false, Encoding.UTF8);
                sw.Write(serialized);
                sw.Close();
            }
        }
    }
}

