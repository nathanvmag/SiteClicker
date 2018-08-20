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
    
    public partial class AddPerson : Form
    {
        List<GoodAds> list;
        int q;
        string[] paths = new string[2] { "goodads.txt", "badads.txt" };
        public AddPerson(List<GoodAds> l,int i)
        {
            InitializeComponent();
            list = l;
            q = i;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(Link.Text) && !string.IsNullOrWhiteSpace(Dono.Text))
            {
                GoodAds ga = new GoodAds(Dono.Text, Link.Text, (int)numericUpDown2.Value, (int)numericUpDown3.Value)
                {
                    fblink = textBox3.Text
                };
                list.Add(ga);
                string serialized = JsonConvert.SerializeObject(list);
                if (File.Exists(paths[q])) File.Delete(paths[q]);
                StreamWriter sw = new StreamWriter(paths[q], false, Encoding.UTF8);
                sw.Write(serialized);
                sw.Close();
                this.Close();

            }
            else MessageBox.Show("Preencha todos os campos");

        }
    }
    public class GoodAds
    {
        public string nome, fblink, bloglink;
        public int qntsabas, totaltime;
        public DateTime when;
        public GoodAds(string no, string blog, int qa, int ta)
        {
            nome = no;
            bloglink = blog;
            qntsabas = qa;
            totaltime = ta;
        }
    }
}
