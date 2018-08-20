using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;
using System.Management;
using Newtonsoft.Json;
using CefSharp;
using System.Threading;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using FoxLearn.License;

namespace SiteClicker
{
    public partial class Form1 : Form
    {

        public List<string> Browsers;
        bool winput = false, gotInput = false;
        Keys toPrint = Keys.PrintScreen;
        public List<Process> Brprocess;
        public static List<adsense> myads;
        [DllImport("User32.dll")]
        public static extern Int32 SetForegroundWindow(int hWnd);
        List<GoodAds> goodAds;
        List<GoodAds> badAds;
        List<Work> myworks;
        public class Work
        {
            public string name = "", website, postFB, navegador, photpath;
            public DateTime when;
            public bool working;
            public int qads=0, qtime=0;
            public Work(string nam, string wbsite, string pfb, string nav, bool wk)
            {
                name = nam;
                website = wbsite;
                postFB = pfb;
                navegador = nav;
                when = DateTime.Now;
                working = true;
                photpath = "";
            }

        }
        public class adsense
        {
            public string imagepath;
            public string[] website, title;
            public adsense(string[] web, string[] titles, string url)
            {
                imagepath = url;
                website = web;
                title = titles;
            }
        }
        public Form1()
        {
            InitializeComponent();
           
            initializeCEF();
           


            Browsers = GetBrowsers();
            List<string> browsernames;
            browsernames = new List<string>();
            UserActivityHook actHook = new UserActivityHook(); // crate an instance with global hooks
            if (File.Exists("key.txt"))
            {
                StreamReader sr = new StreamReader("key.txt");
                toPrint = JsonConvert.DeserializeObject<Keys>(sr.ReadToEnd());
                sr.Close();
            }
            else
            {
                toPrint = Keys.PrintScreen;
                StreamWriter rw = new StreamWriter("key.txt");
                rw.Write(JsonConvert.SerializeObject(toPrint));
                rw.Close();
            }
            button2.Text = toPrint.ToString();   // hang on events
            actHook.OnMouseActivity += new MouseEventHandler(MouseMoved);
            actHook.KeyDown += new KeyEventHandler(MyKeyDown);
            actHook.KeyPress += new KeyPressEventHandler(MyKeyPress);
            actHook.KeyUp += new KeyEventHandler(MyKeyUp);
            Brprocess = new List<Process>();
            if (File.Exists("myads.txt"))
            {
                StreamReader sr = new StreamReader("myads.txt");
                myads = JsonConvert.DeserializeObject<List<adsense>>(sr.ReadToEnd());
                sr.Close();
            }
            else myads = new List<adsense>();
           
            List<string> adtitles = new List<string>();
            foreach (adsense a in myads) adtitles.Add(a.website[0]);
            if (File.Exists("myworks.txt"))
            {
                StreamReader sr = new StreamReader("myworks.txt");
                myworks = JsonConvert.DeserializeObject<List<Work>>(sr.ReadToEnd());
                sr.Close();
            }
            else myworks = new List<Work>();
            foreach (Work w in myworks) w.working = false;
            foreach (string s in Browsers)
            {
                browsernames.Add(s.Split('\\')[s.Split('\\').Length - 1].Replace("\"", ""));
            }
            Navegadores.Items.AddRange(browsernames.ToArray());
            comboBox1.Items.AddRange(adtitles.ToArray());

            if (File.Exists("badads.txt"))
            {
                StreamReader sr = new StreamReader("badads.txt");
                badAds = JsonConvert.DeserializeObject<List<GoodAds>>(sr.ReadToEnd());
                sr.Close();
            }
            else badAds = new List<GoodAds>();
            if (File.Exists("goodads.txt"))
            {
                StreamReader sr = new StreamReader("goodads.txt");
                goodAds = JsonConvert.DeserializeObject<List<GoodAds>>(sr.ReadToEnd());
                sr.Close();
            }
            else goodAds = new List<GoodAds>();            

            }
        void initializeCEF()
        {
            try
            {
                CefSettings settings = new CefSettings();
                settings.RemoteDebuggingPort = 8088;

                // Initialize cef with the provided settings

                Cef.Initialize(settings);
                Cef.EnableHighDPISupport();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        List<string> GetBrowsers()
        {
            List<string> gb = new List<string>();
            using (RegistryKey hklm = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry32))
            {
                RegistryKey webClientsRootKey = hklm.OpenSubKey(@"SOFTWARE\Clients\StartMenuInternet");
                if (webClientsRootKey != null)
                    foreach (var subKeyName in webClientsRootKey.GetSubKeyNames())
                        if (webClientsRootKey.OpenSubKey(subKeyName) != null)
                            if (webClientsRootKey.OpenSubKey(subKeyName).OpenSubKey("shell") != null)
                                if (webClientsRootKey.OpenSubKey(subKeyName).OpenSubKey("shell").OpenSubKey("open") != null)
                                    if (webClientsRootKey.OpenSubKey(subKeyName).OpenSubKey("shell").OpenSubKey("open").OpenSubKey("command") != null)
                                    {
                                        string commandLineUri = (string)webClientsRootKey.OpenSubKey(subKeyName).OpenSubKey("shell").OpenSubKey("open").OpenSubKey("command").GetValue(null);
                                        //your turn
                                        gb.Add(commandLineUri);
                                    }
            }
            return gb;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            bool dowork = true;
            Work todelete = null;
            foreach (Work w in myworks)
            {
                if (Link.Text == w.website && Dono.Text == w.name)
                {                    
                    
                    DateTime dt1 = w.when;
                    DateTime dt2 = DateTime.Now;

                    TimeSpan diff = (dt2 - dt1).Duration();

                    if (diff.TotalHours <= 48)
                    {

                        DialogResult result = MessageBox.Show("Há menos de 2 dias que você ja acessou esse site pelo programa, deseja continuar ?", "Clicker", MessageBoxButtons.YesNo);
                        if (result == DialogResult.No)
                        {
                            dowork = false;
                            break;
                        }
                        else
                        {
                            todelete = w;
                            break;

                        }
                    }
                }
            }
            foreach (GoodAds ga in badAds)
            {
                if (Link.Text == ga.bloglink && ga.nome == Dono.Text)
                {
                    DialogResult result = MessageBox.Show("Esta ad está na lista dos Bad ads, deseja continuar ?", "Clicker", MessageBoxButtons.YesNo);
                    if (result == DialogResult.No)
                    {
                        dowork = false;
                        break;
                    }
                   
                }
            }
                if (todelete != null)
                myworks.Remove(todelete);
            if (manu.Checked && !string.IsNullOrWhiteSpace(Link.Text) && !string.IsNullOrWhiteSpace(Dono.Text) && Navegadores.SelectedItem != null && dowork)
            {
                foreach (Work w in myworks)
                {
                    if (w.navegador == Navegadores.SelectedIndex.ToString() && w.working)
                        w.working = false;
                }
                myworks.Add(new Work(Dono.Text, Link.Text, textBox3.Text, Navegadores.SelectedItem.ToString().Replace(".exe", ""), true) { qads = (int)numericUpDown2.Value, qtime = (int)numericUpDown3.Value });
                string serialized = JsonConvert.SerializeObject(myworks);
                if (File.Exists("myworks.txt")) File.Delete("myworks.txt");
                StreamWriter sw = new StreamWriter("myworks.txt", false, Encoding.UTF8);
                sw.Write(serialized);
                sw.Close();

                Brprocess.Add(Process.Start(Browsers[Navegadores.SelectedIndex], Link.Text));
                this.WindowState = FormWindowState.Minimized;

            }
            else if (dowork && manu.Checked) Alert("Você não preencheu todos os itens corretamente");

            else if (dowork && auto.Checked && !string.IsNullOrWhiteSpace(Link.Text) && !string.IsNullOrWhiteSpace(Dono.Text) && numericUpDown2.Value != 0 && numericUpDown3.Value != 0) {
                Work mwork = new Work(Dono.Text, Link.Text, textBox3.Text, "cefsharp", true) { qads = (int)numericUpDown2.Value, qtime = (int)numericUpDown3.Value };
                myworks.Add(mwork);
                string serialized = JsonConvert.SerializeObject(myworks);
                if (File.Exists("myworks.txt")) File.Delete("myworks.txt");
                StreamWriter sw = new StreamWriter("myworks.txt", false, Encoding.UTF8);
                sw.Write(serialized);
                sw.Close();
                new Thread(() =>
                {
                    navegador nv = new navegador(mwork, (int)numericUpDown2.Value, (int)numericUpDown3.Value);
                    nv.Size = new Size(Convert.ToInt32(Screen.PrimaryScreen.Bounds.Size.Width * 0.7f), Convert.ToInt32(Screen.PrimaryScreen.Bounds.Size.Height * 0.7f));
                    nv.StartPosition = FormStartPosition.CenterScreen;
                    nv.ShowDialog();
                }).Start();
            }
            else if (dowork && auto.Checked) Alert("Você não preencheu todos os itens corretamente");

            else if (dowork && checkBox2.Checked && !string.IsNullOrWhiteSpace(Link.Text) && !string.IsNullOrWhiteSpace(Dono.Text) && numericUpDown2.Value != 0 && numericUpDown3.Value != 0&&comboBox1.SelectedItem!=null)
            {
                Work mwork = new Work(Dono.Text, Link.Text, textBox3.Text, "cefsharp", true) { qads = (int)numericUpDown2.Value, qtime = (int)numericUpDown3.Value };
                myworks.Add(mwork);
                string serialized = JsonConvert.SerializeObject(myworks);
                if (File.Exists("myworks.txt")) File.Delete("myworks.txt");
                StreamWriter sw = new StreamWriter("myworks.txt", false, Encoding.UTF8);
                sw.Write(serialized);
                sw.Close();
                int c = comboBox1.SelectedIndex;
                new Thread(() =>
                {
                    navegador nv = new navegador(mwork, (int)numericUpDown2.Value, (int)numericUpDown3.Value, myads[c]);
                    nv.Size = new Size(Convert.ToInt32(Screen.PrimaryScreen.Bounds.Size.Width * 0.7f), Convert.ToInt32(Screen.PrimaryScreen.Bounds.Size.Height * 0.7f));
                    nv.StartPosition = FormStartPosition.CenterScreen;
                    nv.ShowDialog();
                }).Start();
            }
            else if (dowork && checkBox2.Checked) Alert("Você não preencheu todos os itens corretamente");

        }
        void Alert(string text)
        {
            MessageBox.Show(text, "Clicker");
        }
        public void MouseMoved(object sender, MouseEventArgs e)
        {
            if (e.Clicks > 0) LogWrite("MouseButton 	- " + e.Button.ToString());
        }
        public static void addAd (adsense ad){
                myads.Add(ad);
                string serialized = JsonConvert.SerializeObject(myads);
                if (File.Exists("myads.txt")) File.Delete("myads.txt");
                StreamWriter sw = new StreamWriter("myads.txt", false, Encoding.UTF8);
                sw.Write(serialized);
                sw.Close();
                
        }
            

            
        public async void MyKeyDown(object sender, KeyEventArgs e)
        {
            Work toprint= null;
            if (winput)
            {
                toPrint = e.KeyCode;
                StreamWriter rw = new StreamWriter("key.txt");
                rw.Write(JsonConvert.SerializeObject(toPrint));
                rw.Close();
                button2.Text = e.KeyData.ToString();
                gotInput = true;
                label5.Text = "Botao para print";
                winput = false;
            }
            if (toPrint == e.KeyCode && checkBox1.Checked && Brprocess != null)
            {

                //LogWrite("KeyPress 	- " + e.KeyChar);
                if (Brprocess != null)
                {
                    Console.WriteLine(GetActiveWindowTitle() + "  " + GetActiveWindowTitle().ToLower().Split('-')[GetActiveWindowTitle().ToLower().Split('-').Length - 1].Contains("chrome"));
                    foreach (Work w in myworks)
                    {
                        if (GetActiveWindowTitle().ToLower().Split('-')[GetActiveWindowTitle().ToLower().Split('-').Length - 1].Contains(w.navegador) && w.working)
                        {
                            toprint = w;
                        }
                    }
                }
                if (toprint == null) toprint = myworks[myworks.Count - 1];
                if (toprint != null)
                {
                    SendKeys.Send("^{h}");
                    await Task.Delay(8000);
                    string photpath = "";
                    toprint.website = toprint.website.Replace("https://", "").Replace("www.", "").Replace("http://", "");
                    toprint.website = toprint.website.Replace('.', '-');
                    toprint.website = toprint.website.Remove(toprint.website.Length - 1);
                    photpath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, photpath);
                    if (!Directory.Exists(photpath + "prints/")) Directory.CreateDirectory(photpath + "prints/");

                    // Save the screenshot to the specified path that the user has chosen.
                    photpath += "prints/Dia " + DateTime.Now.Day + " Mes " + DateTime.Now.Month + "/";
                    photpath = photpath.Replace('/', '\u005c');
                    Console.WriteLine(photpath);
                    if (!Directory.Exists(photpath)) Directory.CreateDirectory(photpath);
                    if (toprint != null)
                    {
                        if (!Directory.Exists(photpath + toprint.name + "/")) Directory.CreateDirectory(photpath + toprint.name + "/");
                        photpath += toprint.name + "/" + toprint.website + ".jpeg";
                    }
                    else photpath += "Hora " + DateTime.Now.Hour + ":" + DateTime.Now.Minute + " .jpeg";
                    photpath = photpath.Replace('/', '\u005c');
                    Console.WriteLine(photpath);
                    if (File.Exists(photpath)) File.Delete(photpath);

                    /*ScreenCapture sc = new ScreenCapture();
                     Image img = sc.CaptureScreen();


                    img.Save(photpath, ImageFormat.Jpeg);*/
                    photpath = photpath.Replace('/', '\u005c');
                    try
                    {
                        System.Drawing.Bitmap printscreen = new System.Drawing.Bitmap(System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width, System.Windows.Forms.Screen.PrimaryScreen.Bounds.Height);
                        System.Drawing.Graphics graphics = System.Drawing.Graphics.FromImage(printscreen as System.Drawing.Image);
                        graphics.CopyFromScreen(0, 0, 0, 0, printscreen.Size);

                        byte[] byteArray = new byte[0];
                        using (MemoryStream stream = new MemoryStream())
                        {
                            printscreen.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
                            stream.Close();

                            byteArray = stream.ToArray();
                        }

                        File.WriteAllBytes(photpath, byteArray);

                    }
                    catch 
                    {
                        //Tratamento de erros
                    }
                
            Console.WriteLine(photpath);
                foreach (Work w in myworks) if (w == toprint)
                    { 
                        w.photpath = photpath;
                    }
                //  bmpScreenshot.Save(photpath, ImageFormat.Jpeg);
                
                SetForegroundWindow(Handle.ToInt32());

                DialogResult result = MessageBox.Show("Print tirado com sucesso, deseja vê-lo na pasta ?", "Clicker", MessageBoxButtons.YesNo);
                if (result == DialogResult.Yes)
                {
                    Process.Start("explorer.exe", " /select, " + photpath);

                }
                    DialogResult resultt2 = MessageBox.Show("Deseja abrir o gerador de text para o post do facebook ?", "Clicker", MessageBoxButtons.YesNo);
                    if (resultt2 == DialogResult.Yes)
                    {
                        postgerador pg = new postgerador();
                        pg.ShowDialog();

                    }
                    if (!string.IsNullOrEmpty(toprint.postFB))
                    {
                        DialogResult resultt = MessageBox.Show("Deseja abrir a publicação do facebook do grupo de cliques ?", "Clicker", MessageBoxButtons.YesNo);
                        if (resultt == DialogResult.Yes)
                        {
                            try
                            {
                                Uri uri = new Uri(toprint.postFB);
                                Process.Start(uri.ToString());
                            }
                            catch
                            {
                                Process.Start("https://" + toprint.postFB.Replace("https://", ""));
                            }

                        }
                    }
                   

                }
            LogWrite("KeyDown 	- " + e.KeyData.ToString() + "   modi" + e.Alt);
           
            }
        }

        public void MyKeyPress(object sender, KeyPressEventArgs e)
        {
          

        }

        public void MyKeyUp(object sender, KeyEventArgs e)
        {
            LogWrite("KeyUp 		- " + e.KeyData.ToString());
        }

        private void LogWrite(string txt)
        {
            // Console.WriteLine(txt );
        }

        private void button2_Click(object sender, EventArgs e)
        {
            label5.Text = "Digite a nova tecla para o print";
            button2.Text = "---";
            
            WaitForInput();

        }
        private void WaitForInput()
        {
            Task.Factory.StartNew(() =>
            {
                while (!gotInput)
                {
                    winput = true;
                    System.Threading.Thread.Sleep(1);
                }
                gotInput = false;
            });
        }
        [DllImport("user32.dll")]
        public static extern IntPtr GetForegroundWindow();
        [DllImport("user32.dll", SetLastError = true)]
        public static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint lpdwProcessId);

        private void button3_Click(object sender, EventArgs e)
        {
            Historico h = new Historico(myworks,goodAds,badAds);
            h.StartPosition = FormStartPosition.CenterScreen;
            h.Visible = true;
        }

        public string GetActiveWindowTitle()
        {
            IntPtr hWnd = GetForegroundWindow();
            uint procId = 0;
            GetWindowThreadProcessId(hWnd, out procId);
            var proc = Process.GetProcessById((int)procId);
            return proc.MainWindowTitle;


        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            try
            {
                foreach (Work w in myworks)
                {
                    w.working = false;
                }
                if (myworks.Count > 0)
                {
                    string serialized = JsonConvert.SerializeObject(myworks);
                    if (File.Exists("myworks.txt")) File.Delete("myworks.txt");
                    StreamWriter sw = new StreamWriter("myworks.txt", false, Encoding.UTF8);
                    sw.Write(serialized);
                    sw.Close();
                }
            }
            catch( Exception ex)
            {
                Alert(ex.ToString());
            }

        }
        int control = 0;
        private void manu_CheckedChanged(object sender, EventArgs e)
        {
            if (control == 1||control==2)
            {
                auto.Checked = false;
                label4.Visible = true;
                Navegadores.Visible = true;
                label6.Visible = false;
                label7.Visible = false;
                numericUpDown2.Visible = false;
                numericUpDown3.Visible = false;
                checkBox2.Checked = false;
                cads.Visible = false;
                comboBox1.Visible = false;
                label9.Visible = false;
                control = 0;
                button4.Visible = false;

            }
        }

        private void auto_CheckedChanged(object sender, EventArgs e)
        {
            if (control == 0||control==2)
            {
                manu.Checked = false;
                label4.Visible = false;
                Navegadores.Visible = false;
                label6.Visible = true;
                label7.Visible = true;
                checkBox2.Checked = false;
                numericUpDown2.Visible = true;
                numericUpDown3.Visible = true;
                control = 1;
                cads.Visible = false;
                comboBox1.Visible = false;
                label9.Visible = false;
                button4.Visible = false;


            }
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://"+linkLabel1.Text);

        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (control==1||control==0)
            {
                manu.Checked = false;
                label4.Visible = false;
                auto.Checked = false;
                Navegadores.Visible = false;
                label6.Visible = true;
                label7.Visible = true;
                numericUpDown2.Visible = true;
                numericUpDown3.Visible = true;
                control = 2;
                cads.Visible = true;
                comboBox1.Visible = true;
                label9.Visible = true;
                button4.Visible = true;
            }
        }

        private void cads_Click(object sender, EventArgs e)
        {
            Adsense ad = new Adsense();
            ad.StartPosition = FormStartPosition.CenterScreen;
            ad.ShowDialog();
            comboBox1.Items.Clear();
            List<string> adtitles = new List<string>();
            foreach (adsense a in myads) adtitles.Add(a.website[0]);
            comboBox1.Items.AddRange(adtitles.ToArray());
        }

        private void button4_Click(object sender, EventArgs e)
        {
            editar ed = new editar(myads);
            ed.ShowDialog();
            comboBox1.Items.Clear();
            List<string> adtitles = new List<string>();
            foreach (adsense a in myads) adtitles.Add(a.website[0]);
            comboBox1.Items.AddRange(adtitles.ToArray());
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Godads ga = new Godads(this);
            ga.StartPosition = FormStartPosition.CenterScreen;
            ga.Visible = true;
        }

        private void button6_Click(object sender, EventArgs e)
        {
            badAds ga = new badAds();
            ga.StartPosition = FormStartPosition.CenterScreen;
            ga.Visible = true;
            ga.FormClosed += delegate
             {
                 if (File.Exists("badads.txt"))
                 {
                     StreamReader sr = new StreamReader("badads.txt");
                     badAds = JsonConvert.DeserializeObject<List<GoodAds>>(sr.ReadToEnd());
                     sr.Close();
                 }
                 foreach (GoodAds b in badAds)
                 {
                     Console.WriteLine(b.nome + " " + b.bloglink + " ");
                 }
             };

        }

        public Process GetactiveProcess()
        {
            IntPtr hWnd = GetForegroundWindow();
            uint procId = 0;
            GetWindowThreadProcessId(hWnd, out procId);
            var proc = Process.GetProcessById((int)procId);
            return proc;


        }
        public void putiten(string nome,string link,string fbpost,int a,int b)
        {
            Dono.Text = nome;
            Link.Text = link;
            textBox3.Text = fbpost;
            numericUpDown2.Value = a;
            numericUpDown3.Value = b;
        }
       
    }
}
