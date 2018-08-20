using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Text;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SiteClicker
{
    public partial class chist: Form
    {
        
        Font f1, f2;
        List<his> lhis;
        bool started;
        Form1.Work fwork;
        int resizer= 35000;
        string[] weekdays=new string[7] { "Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday" };
        string[] mL = new string[12] { "January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December" };

        public chist(List<history> historys,Form1.Work work,Form1.adsense ad= null)
        {
            InitializeComponent();
            this.WindowState = FormWindowState.Maximized;
            SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            Console.WriteLine("meuuu dia "+(int)DateTime.Now.DayOfWeek);
            string daytx = "Today - " + weekdays[(int)DateTime.Now.DayOfWeek] + ", " + mL[(int)DateTime.Now.Month - 1] + " " + DateTime.Now.Day + ", " + DateTime.Now.Year;
            label1.Text = daytx;
            PrivateFontCollection pfc = new PrivateFontCollection();
            pfc.AddFontFile("robotob.ttf");
            PrivateFontCollection pf2 = new PrivateFontCollection();
            pf2.AddFontFile("roboto.ttf");
            f1 = new Font(pfc.Families[0], 13, FontStyle.Regular);
            f2= new Font(pf2.Families[0], 13, FontStyle.Regular);
            label1.Parent = pictureBox1;
            label1.Location = new Point((int)((231f / 818f) * this.Size.Width), (int)((52f / 497f) * this.Size.Height));
            label1.Font = new Font(f1.FontFamily, this.Size.Width * this.Size.Height / 25409);
            fwork = work;
            label1.AutoSize = true;
            Console.WriteLine(label2.Parent.Name + "   "+Controls.Count);
            pictureBox1.Visible = true;
            pictureBox1.SendToBack();
            lhis = new List<his>();
            int ct = 1;
            int nGCD = GetGreatestCommonDivisor(Screen.PrimaryScreen.Bounds.Height, Screen.PrimaryScreen.Bounds.Width);
            string str = string.Format("{0}:{1}", Screen.PrimaryScreen.Bounds.Height / nGCD, Screen.PrimaryScreen.Bounds.Width / nGCD);
            //MessageBox.Show(str);
            resizer = 35000;
            if(Screen.PrimaryScreen.Bounds.Height / nGCD ==9 && Screen.PrimaryScreen.Bounds.Width / nGCD==16 )
            {
                resizer = 51428;
            }
            if (ad!=null)
            {
                DateTime dt = DateTime.Now;
                
                for(int i=ad.title.Length-1;i>=0;i--)
                {
                    string hora = ((dt.Hour + 11) % 12 + 1).ToString();
                    string min = dt.Minute > 9 ? dt.Minute.ToString() : "0" + dt.Minute;
                    string final = hora + ":" + min + " " + (dt.Hour <= 12 ? "AM" : "PM");
                    string site = ad.website[i];
                    try
                    {
                        site = new Uri(site).Host;
                    }
                    catch
                    {

                    }
                    string tt = ad.title[i];
                    if (tt.Length > 80)
                        tt = tt.Substring(0, 79) + "...";
                    lhis.Add(new his(final, tt, site,new Bitmap( ad.imagepath), this, label1, pictureBox1, ct, fontresize(resizer)));
                    ct++;



                    dt = dt.AddMinutes((DateTime.Now-historys[0].time.AddMinutes(-7)).TotalMinutes/ad.title.Length *-1 );
                    
                }
            }
            

            foreach (history h in historys)
            {
                h.time = h.time.AddMinutes(-7);
                string hora = ((h.time.Hour+11)%12 +1).ToString() ;
                string min = h.time.Minute > 9 ? h.time.Minute.ToString() : "0" + h.time.Minute;
                string final = hora + ":" + min + " " +( h.time.Hour <= 12 ? "AM" : "PM");
                string site = h.adress;
                site = new Uri(site).Host;
                string tt = h.title;
                if (tt.Length > 80)
                    tt = tt.Substring(0, 79) + "...";
                lhis.Add(new his(final,  tt, site, h.favicon, this, label1, pictureBox1, ct,fontresize(resizer)));
                ct++;
            }
            started = true;

        }
        static int GetGreatestCommonDivisor(int a, int b)
        {
            return b == 0 ? a : GetGreatestCommonDivisor(b, a % b);
        }
        [STAThread]

        private void button1_Click(object sender, EventArgs e)
        {
            button1.Visible = false;
            string oldweb = fwork.website;
            fwork.website = fwork.website.Replace("https://", "").Replace("www.", "").Replace("http://", "");
            fwork.website = fwork.website.Replace('.', '-');
            fwork.website = fwork.website.Remove(fwork.website.Length - 1);

            string photpath = "";
            photpath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, photpath);

            Console.WriteLine(photpath);
            if (!Directory.Exists(photpath + "prints/")) Directory.CreateDirectory(photpath + "prints/");
            photpath += "prints/Dia " + DateTime.Now.Day + " Mes " + DateTime.Now.Month + "/";
            photpath = photpath.Replace('/', '\u005c');


            Console.WriteLine(photpath + "   122");
            if (!Directory.Exists(photpath)) Directory.CreateDirectory(photpath);
            photpath += "nome " + fwork.name + "  site " + fwork.website + "/";
            photpath = photpath.Replace('/', '\u005c');

            if (!Directory.Exists(photpath)) Directory.CreateDirectory(photpath);
            if (File.Exists(photpath + "print.png")) File.Delete(photpath + "print.png");
            photpath += "print.png";
            Bitmap bmp = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            pictureBox1.DrawToBitmap(bmp, new Rectangle(0, 0, bmp.Width, bmp.Height));
            bmp.Save(photpath,ImageFormat.Png);

            DialogResult result = MessageBox.Show("Print tirado com sucesso, deseja vê-lo na pasta ?", "Clicker", MessageBoxButtons.YesNo);
            if (result == DialogResult.Yes)
            {
                Process.Start("explorer.exe", " /select, " + photpath);

            }

            button1.Visible = true;
            DialogResult resultt2 = MessageBox.Show("Deseja abrir o gerador de text para o post do facebook ?", "Clicker", MessageBoxButtons.YesNo);
            if (resultt2 == DialogResult.Yes)
            {

                postgerador pg = new postgerador();
                pg.ShowDialog();

            }
            if (!string.IsNullOrEmpty(fwork.postFB))
            {
                DialogResult resultt = MessageBox.Show("Deseja abrir a publicação do facebook do grupo de cliques ?", "Clicker", MessageBoxButtons.YesNo);
                if (resultt == DialogResult.Yes)
                {
                    try
                    {
                        Uri uri = new Uri(fwork.postFB);
                        Process.Start(uri.ToString());
                    }
                    catch
                    {
                        Process.Start("https://" + fwork.postFB.Replace("https://", ""));
                    }                 

                }
            }
            int q;
            string[] paths = new string[2] { "goodads.txt", "badads.txt" };
            DialogResult resultt3 = MessageBox.Show("Esta foi uma ad boa ou ruim ? (sim - boa , não - ruim, cancelar - fechar)", "Clicker", MessageBoxButtons.YesNoCancel,MessageBoxIcon.Question);
            if (resultt3 == DialogResult.Yes)

            {
                q = 0;
                List<GoodAds> list;
                if (File.Exists(paths[q]))
                {
                    StreamReader sr = new StreamReader(paths[q]);
                    list = JsonConvert.DeserializeObject<List<GoodAds>>(sr.ReadToEnd());
                    sr.Close();
                }
                else list = new List<GoodAds>();
                int control = 0;
                foreach( GoodAds ad in list )
                {
                    if (ad.nome==fwork.name &&ad.bloglink== oldweb)
                    {
                        ad.when = DateTime.Now;
                        control = 1;
                    }
                }
                if (control==0)
                list.Add(new GoodAds(fwork.name, oldweb, fwork.qads, fwork.qtime) { when = DateTime.Now });

                string serialized = JsonConvert.SerializeObject(list);
                if (File.Exists(paths[q])) File.Delete(paths[q]);
                StreamWriter sw = new StreamWriter(paths[q], false, Encoding.UTF8);
                sw.Write(serialized);
                sw.Close();

            }
            if (resultt3== DialogResult.No)
            {
                q = 1;
                List<GoodAds> list;
                if (File.Exists(paths[q]))
                {
                    StreamReader sr = new StreamReader(paths[q]);
                    list = JsonConvert.DeserializeObject<List<GoodAds>>(sr.ReadToEnd());
                    sr.Close();
                }
                else list = new List<GoodAds>();
                int control = 0;
                foreach (GoodAds ad in list)
                {
                    if (ad.nome == fwork.name && ad.bloglink == fwork.website)
                    {
                        ad.when = DateTime.Now;
                        control = 1;
                    }
                }
                if (control == 0)
                    list.Add(new GoodAds(fwork.name, fwork.website, fwork.qads, fwork.qtime) { when = DateTime.Now });
                string serialized = JsonConvert.SerializeObject(list);
                if (File.Exists(paths[q])) File.Delete(paths[q]);
                StreamWriter sw = new StreamWriter(paths[q], false, Encoding.UTF8);
                sw.Write(serialized);
                sw.Close();
            }




        }

        private void pictureBox1_StyleChanged(object sender, EventArgs e)
        {
            {
                try
                {
                    pictureBox2.Visible = false;
                    label2.Visible = false;
                    pictureBox3.Visible = false;
                    label3.Visible = false;
                    label4.Visible = false;
                    pictureBox4.Visible = false;
                    if (started)
                    {
                        label1.Location = new Point((int)((231f / 818f) * this.Size.Width), (int)((52f / 497f) * this.Size.Height));
                        int fontsize = fontresize(resizer) > 3 ? fontresize(resizer) : 3;
                        Console.WriteLine(fontsize);
                        try
                        {
                            label1.Font = new Font(f1.FontFamily, fontsize, FontStyle.Regular);
                        }
                        catch { };
                        foreach (his h in lhis)
                        {
                            h.update(label1, this, fontresize(resizer));
                        }
                    }
                }
                catch { }
            }


            }
        int fontresize(int t)
        {
            return 10;
        }


    }
    



    class his
    {
        public PictureBox box, file, pts;
        public Label hora, title, site;
        int i;
        public his (string h,string t,string st, Bitmap bp,chist c,Label label1,PictureBox pictureBox1 ,int count,int resizer)
        {
            i = count;
            box = new PictureBox();
            box.Image = Properties.Resources.mark;
            box.SizeMode = PictureBoxSizeMode.StretchImage;
            box.BringToFront();
            box.Size = new Size(label1.Size.Height, label1.Size.Height);
            box.Location = new Point(label1.Location.X, label1.Location.Y + (int)((label1.Size.Height * 2.5f) * i*1f));

            pictureBox1.Controls.Add(box);
            hora = new Label();
            hora.Text = h;
            hora.AutoSize = true;
            hora.ForeColor = ColorTranslator.FromHtml("#646464");
            hora.BackColor = Color.White;
            hora.Font = new Font(hora.Font.FontFamily, fontresize(resizer,c) * 0.8f);
            hora.Location = new Point(box.Location.X + (box.Size.Width * 2), box.Location.Y + (box.Size.Height / 2 - hora.Size.Height / 2));
            pictureBox1.Controls.Add(hora);
            file = new PictureBox();
            file.Image = bp;
            file.BackColor = Color.White;
            file.SizeMode = PictureBoxSizeMode.CenterImage;
            file.BringToFront();
            file.Size = new Size((int)(box.Size.Width ), box.Size.Height);
            file.Location = new Point(label1.Location.X + (hora.Width + (box.Width) * 5), box.Location.Y);
            pictureBox1.Controls.Add(file);

            title = new Label();
            title.Text = t ;
            title.ForeColor = ColorTranslator.FromHtml("#212121");
            title.BackColor = Color.White;
            title.Font = new Font(hora.Font.FontFamily, fontresize(resizer,c) * 0.8f);
            title.Location = new Point(file.Location.X + box.Size.Width, box.Location.Y + (box.Size.Height / 2 - title.Size.Height / 2));
            title.AutoSize = true;
            pictureBox1.Controls.Add(title);
            site = new Label();
            site.Text = st;
            site.ForeColor = ColorTranslator.FromHtml("#646464");
            site.BackColor = Color.White;
            site.AutoSize = true;
            site.Font = new Font(hora.Font.FontFamily, fontresize(resizer,c) * 0.8f);
            site.Location = new Point(title.Location.X + title.Size.Width + box.Size.Width, box.Location.Y + (box.Size.Height / 2 - site.Size.Height / 2));
            pictureBox1.Controls.Add(site);

            pts = new PictureBox();
            pts.Image = Properties.Resources.ops;
            pts.SizeMode = PictureBoxSizeMode.StretchImage;
            pts.Location = new Point((int)((704f / 800f) * c.Size.Width), box.Location.Y);
            pts.Size = new Size((int)(box.Height / 2.7142f), box.Height);
            pts.BringToFront();
            pictureBox1.Controls.Add(pts);
        }
        
        public void update(Label label1 ,chist c,int resizer)
        {
            box.Size = new Size(label1.Size.Height, label1.Size.Height);
            box.Location = new Point(label1.Location.X, label1.Location.Y + (int)(label1.Size.Height * 2.5f * i));
            hora.Font = new Font(hora.Font.FontFamily, fontresize(resizer,c) * 0.8f);
            hora.Location = new Point(box.Location.X + (box.Size.Width * 2), box.Location.Y + (box.Size.Height / 2 - hora.Size.Height / 2));
            file.Size = new Size((int)(box.Size.Width ), box.Size.Height);
            file.Location = new Point(label1.Location.X + (hora.Width + (box.Width) * 5), box.Location.Y);
            title.Font = new Font(hora.Font.FontFamily, fontresize(resizer,c) * 0.8f);
            title.Location = new Point(file.Location.X + box.Size.Width, box.Location.Y + (box.Size.Height / 2 - title.Size.Height / 2));
            site.Font = new Font(hora.Font.FontFamily, fontresize(resizer,c) * 0.8f);
            site.Location = new Point(title.Location.X + title.Size.Width + box.Size.Width, box.Location.Y + (box.Size.Height / 2 - site.Size.Height / 2));
            pts.Location = new Point((int)((704f / 800f) * c.Size.Width), box.Location.Y);
            pts.Size = new Size((int)(box.Height / 2.7142f), box.Height);
        }
        float fontresize(int t,chist c)
        {
            return 10;
        }
    }
    
}
