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
namespace SiteClicker
{
    public partial class Form1 : Form
    {
        public List<string> Browsers;
        bool ctrlPressed;
        bool winput = false, gotInput = false;
        Keys toPrint = Keys.PrintScreen;
        public List<Process> Brprocess;

        List<Work> myworks;
        public class Work
        {
            public string name="", website, postFB,navegador;
            public DateTime when;
            bool working;
           public Work(string nam,string wbsite,string pfb,string nav,bool wk)
            {
                name = nam;
                website=wbsite;
                postFB = pfb;
                navegador = nav;
                when = DateTime.Now;
                working = true;
            }
            
        }
        public Form1()
        {
            InitializeComponent();
            Browsers = GetBrowsers();
            List<string> browsernames;
            browsernames = new List<string>();
            UserActivityHook actHook = new UserActivityHook(); // crate an instance with global hooks
            button2.Text = toPrint.ToString();   // hang on events
            actHook.OnMouseActivity += new MouseEventHandler(MouseMoved);
            actHook.KeyDown += new KeyEventHandler(MyKeyDown);
            actHook.KeyPress += new KeyPressEventHandler(MyKeyPress);
            actHook.KeyUp += new KeyEventHandler(MyKeyUp);
            Brprocess = new List<Process>();
            if (File.Exists("myworks.txt"))
            {
                StreamReader sr = new StreamReader("myworks.txt");
                myworks = JsonConvert.DeserializeObject<List<Work>>(sr.ReadToEnd());
                sr.Close();
            }
            else myworks = new List<Work>();

            foreach (string s in Browsers)
            {
                browsernames.Add(s.Split('\\')[s.Split('\\').Length - 1].Replace("\"", ""));
            }
            Navegadores.Items.AddRange(browsernames.ToArray());
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
            if (!string.IsNullOrWhiteSpace(Link.Text) && !string.IsNullOrWhiteSpace(Dono.Text) && Navegadores.SelectedItem != null)
            {
                myworks.Add(new Work(Dono.Text,Link.Text,textBox3.Text, Navegadores.SelectedItem.ToString().Replace(".exe","") ,true) { });
                string serialized = JsonConvert.SerializeObject(myworks);
                if (File.Exists("myworks.txt")) File.Delete("myworks.txt");
                StreamWriter sw = new StreamWriter("myworks.txt", false, Encoding.UTF8);
                sw.Write(serialized);
                sw.Close();

                Brprocess.Add(Process.Start(Browsers[Navegadores.SelectedIndex], Link.Text));
                this.WindowState = FormWindowState.Minimized;

            }
            else Alert("Você não preencheu todos os itens corretamente");
        }
        void Alert(string text)
        {
            MessageBox.Show(text, "Clicker");
        }
        public void MouseMoved(object sender, MouseEventArgs e)
        {
            if (e.Clicks > 0) LogWrite("MouseButton 	- " + e.Button.ToString());
        }

        public async void MyKeyDown(object sender, KeyEventArgs e)
        {

            if (toPrint == e.KeyCode && checkBox1.Checked && Brprocess != null)
            {

                foreach (Process p in Brprocess)
                {
                    Console.WriteLine(p.MainModule.FileName + "   " + GetactiveProcess().MainModule.FileName);
                    if (p.Id == GetactiveProcess().Id)
                    {
                        Console.WriteLine("Tem processo igual");
                    }
                }
                SendKeys.Send("^{h}");
                await Task.Delay(8000);
                //Create a new bitmap.
                var bmpScreenshot = new Bitmap(Screen.PrimaryScreen.Bounds.Width,
                                               Screen.PrimaryScreen.Bounds.Height,
                                               PixelFormat.Format32bppArgb);

                // Create a graphics object from the bitmap.
                var gfxScreenshot = Graphics.FromImage(bmpScreenshot);

                // Take the screenshot from the upper left corner to the right bottom corner.
                gfxScreenshot.CopyFromScreen(Screen.PrimaryScreen.Bounds.X,
                                            Screen.PrimaryScreen.Bounds.Y,
                                            0,
                                            0,
                                            Screen.PrimaryScreen.Bounds.Size,
                                            CopyPixelOperation.SourceCopy);

                // Save the screenshot to the specified path that the user has chosen.
                if (File.Exists("Screenshot.png")) File.Delete("Screenshot.png");
                bmpScreenshot.Save("Screenshot.png", ImageFormat.Png);
                this.WindowState = FormWindowState.Maximized;
                this.Focus();
                DialogResult result = MessageBox.Show("Print tirado com sucesso, deseja vê-lo na pasta ?", "Clicker", MessageBoxButtons.YesNo);
                if (result == DialogResult.Yes)
                {
                    Process.Start("explorer.exe", " /select, " + "Screenshot.png");

                }


            }
            LogWrite("KeyDown 	- " + e.KeyData.ToString() + "   modi" + e.Alt);
            if (winput)
            {
                toPrint = e.KeyCode;
                button2.Text = e.KeyData.ToString();
                gotInput = true;
                label5.Text = "Botao para print";
                winput = false;
            }
        }

        public void MyKeyPress(object sender, KeyPressEventArgs e)
        {
            //LogWrite("KeyPress 	- " + e.KeyChar);
            if (Brprocess != null)
            {
                Console.WriteLine(GetActiveWindowTitle() +"  "+ GetActiveWindowTitle().ToLower().Split('-')[GetActiveWindowTitle().ToLower().Split('-').Length-1].Contains("chrome"));
                foreach (Work w in myworks) {
                    if (GetActiveWindowTitle().ToLower().Split('-')[GetActiveWindowTitle().ToLower().Split('-').Length - 1].Contains(w.navegador))
                        Console.WriteLine("Mandou bem bb " + w.name + " " + w.navegador);
                }
            }

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
            Historico h = new Historico(myworks);
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
        public Process GetactiveProcess()
        {
            IntPtr hWnd = GetForegroundWindow();
            uint procId = 0;
            GetWindowThreadProcessId(hWnd, out procId);
            var proc = Process.GetProcessById((int)procId);
            return proc;


        }
    }
}
