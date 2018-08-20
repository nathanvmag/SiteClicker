using CefSharp;
using CefSharp.WinForms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SiteClicker;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Drawing.Imaging;

using System.IO;
namespace SiteClicker
{

    public partial class navegador : Form,ILifeSpanHandler
    {
        public ChromiumWebBrowser chromeBrowser;
        Form1.Work fwork;
        string adress = "";
        string title = "";
        int abas, mins, tabs,openabs;
        bool clickprint = false;
        bool canuse = false;
        bool show = false;
        bool clickad = false;
        int tempcounter = -1;
        Form1.adsense ads;
        List<history> myacess;
        TextBox url;
        [DllImport("User32.dll")]
        public static extern Int32 SetForegroundWindow(int hWnd);

        public navegador(Form1.Work w,int abs,int ms,Form1.adsense ad=null)
        {
            Console.Write("ah");
            fwork = w;
            InitializeComponent();
            InitializeChromium();
            abas = abs;
            mins = ms;
            tabs = (ms * 60000) / abas + 1000;
            timer1.Interval =(int)( tabs * 2.5);
            timer1.Enabled = true;
            timer1.Tick += Timer1_Tick;
            openabs = 0;
            abas++;
            Console.WriteLine(abas + "  " + tabs);
            clickprint = false;
            myacess = new List<history>();
            canuse = false;
            show = false;
            clickad = false;
            label1.Text =("Status : Carregando navegador");
            tempcounter = -1;
            if (ad != null) ;
            ads = ad;
        }

        private void Timer1_Tick(object sender, EventArgs e)
        {
            if (openabs >= 0)
            {
                if (openabs != tempcounter) tempcounter = openabs;
                else
                {
                    RestartALL();
                    Console.WriteLine("Pelo contador");
                }
            }
        }
        void RestartALL()
        {
            if (openabs <= abas)
            {
                chromeBrowser.Load(fwork.website);
                openabs--;
            }
        }
        public void InitializeChromium()
        {           

            chromeBrowser = new ChromiumWebBrowser(fwork.website);
            chromeBrowser.AddressChanged += ChromeBrowser_AddressChanged;          
            chromeBrowser.IsBrowserInitializedChanged += ChromeBrowser_IsBrowserInitializedChanged;
            chromeBrowser.TitleChanged += ChromeBrowser_TitleChanged;
            chromeBrowser.LifeSpanHandler = this;

            this.panel1.Controls.Add(chromeBrowser);
            chromeBrowser.Dock = DockStyle.Fill;
            CefSharpSettings.LegacyJavascriptBindingEnabled = true;

            chromeBrowser.RegisterJsObject("cefCustomObject", new CefCustomObject(chromeBrowser, this));

        }
      


        private void ChromeBrowser_TitleChanged(object sender, TitleChangedEventArgs e)
        {
            title = e.Title;
        }

        private void Print_Click(object sender, EventArgs e)
        {   if (canuse)
            {
               // this.label1.Text = "Status: Histórico";

                DialogResult result = MessageBox.Show("Ao clickar em print você irá parar o trabalho do robo automático, deseja continuar ? ", "Clicker", MessageBoxButtons.YesNo);
                if (result == DialogResult.Yes)
                {
                    myacess.Reverse();
                    foreach (history h in myacess)
                    {
                        Console.WriteLine("hora {0}  do titutlo {1} do site {2}", String.Format("{0:HH:mm:ss}", h.time), h.title, h.adress);
                    }
                    clickprint = true;                    
                    if (ads!=null){ 
                        chist c = new chist(myacess, fwork, ads);
                        c.ShowDialog();
                    }
                    else
                    {
                        chist c = new chist(myacess, fwork);
                        c.ShowDialog();
                    }
                }
            }

            Console.Write("");
        }

       
        
            
            

        


        private void ChromeBrowser_IsBrowserInitializedChanged(object sender, IsBrowserInitializedChangedEventArgs e)
        {
            if (e.IsBrowserInitialized) chromeBrowser.LoadingStateChanged += ChromeBrowser_LoadingStateChanged;
        }

        private async void ChromeBrowser_LoadingStateChanged(object sender, LoadingStateChangedEventArgs e)
        {
            try
            {

                if (Cef.IsInitialized)
                {
                    if (!e.IsLoading)
                    {
                        System.Net.WebRequest request =
                        System.Net.WebRequest.Create(
                       "https://www.google.com/s2/favicons?domain=" + adress);

                        System.Net.WebResponse response = request.GetResponse();
                        System.IO.Stream responseStream =
                            response.GetResponseStream();
                        Bitmap bitmap2 = new Bitmap(responseStream);
                        myacess.Add(new history(title, adress, DateTime.Now, bitmap2));

                        if (openabs <= abas && !clickprint&&!clickad)
                        {
                            label1.change_text_from_different_thread("Status : "+(int)((openabs*1.0f/abas)*100)+"% Robo Trabalhando | "+openabs+" abas abertas de :"+abas);
                            if (!clickprint)
                            {
                            }

                            Console.WriteLine("CArrego");
                            await Task.Delay(tabs);
                            Console.WriteLine("CArrego 1xodigo"); ;
                            JavascriptResponse jsr;
                            if (!clickprint)
                            {
                                jsr = await chromeBrowser.EvaluateScriptAsync(@"var ele = document.getElementsByTagName('a')[Math.floor(Math.random() * document.getElementsByTagName('a').length - 0 ) ]; 
                var cont=0;
                    while(ele.href.indexOf('" + fwork.website + @"')==-1||ele.href.indexOf('#')!=-1||ele.href==window.location){

                    console.log(ele.href.indexOf('" + fwork.website + @"')==-1+'    '+ele.href.indexOf('#')!=-1);
                    ele = document.getElementsByTagName('a')[Math.floor(Math.random() * document.getElementsByTagName('a').length - 0)];
                                    cont++;
                                    if (cont == 250) break;
                                }



                if (ele.href.indexOf('" + fwork.website + @"') != -1)
                {
                    console.log('boa');
                    ele.target = '';
                    ele.click();
                }
                else
                {
                    var ele = document.getElementsByTagName('a')[Math.floor(Math.random() * document.getElementsByTagName('a').length - 0)];
                    ele.target = '';
                    ele.click();
                    console.log('PELO ERROR');
                }
                ");

                                int tmptimer = 0;

                                while (!jsr.Success&&tmptimer<13)
                                {
                                    
                                    jsr = await chromeBrowser.EvaluateScriptAsync(@"var ele = document.getElementsByTagName('a')[Math.floor(Math.random() * document.getElementsByTagName('a').length - 0 ) ]; 
                var cont=0;
                    while(ele.href.indexOf('" + fwork.website + @"')==-1||ele.href.indexOf('#')!=-1||ele.href==window.location){

                    console.log(ele.href.indexOf('" + fwork.website + @"')==-1+'    '+ele.href.indexOf('#')!=-1);
                    ele = document.getElementsByTagName('a')[Math.floor(Math.random() * document.getElementsByTagName('a').length - 0)];
                                    cont++;
                                    if (cont == 250) break;
                                }



                if (ele.href.indexOf('" + fwork.website + @"') != -1)
                {
                    console.log('boa');
                    ele.target = '';
                    ele.click();
                }
                else
                {
                    var ele = document.getElementsByTagName('a')[Math.floor(Math.random() * document.getElementsByTagName('a').length - 0)];
                    ele.target = '';
                    ele.click();
                    console.log('PELO ERROR');
                }
                ");
                                    Console.WriteLine("Esta na tab " + openabs); ;
                                    Console.WriteLine(jsr.Result);
                                    tmptimer++;
                                }
                                if (jsr.Success)
                                openabs++;

                            }
                        }
                        else if (!clickprint&&!show&&!clickad)
                        {
                            label1.change_text_from_different_thread("Status : Robo terminou a tarefa");
                            MessageBox.Show("Terminou a tarefa do site " + fwork.website, "Clicker");
                            show = true;

                        }
                       
                    }
                }
            }
            catch { }
        }

        private void ChromeBrowser_AddressChanged(object sender, AddressChangedEventArgs e)
        {
            if (Cef.IsInitialized)
            {
                textBox1.change_text_from_different_thread(e.Address);
                adress = e.Address;
                canuse = true;
                Console.WriteLine(adress);
                if (!adress.Contains(fwork.website) && openabs <= abas && !clickprint && !clickad)
                {
                    chromeBrowser.ExecuteScriptAsync("window.location='" + fwork.website + "'");
                }
             
               
            }
        }

        public bool OnBeforePopup(IWebBrowser browserControl, IBrowser browser, IFrame frame, string targetUrl, string targetFrameName, WindowOpenDisposition targetDisposition, bool userGesture, IPopupFeatures popupFeatures, IWindowInfo windowInfo, IBrowserSettings browserSettings, ref bool noJavascriptAccess, out IWebBrowser newBrowser)
        {
            // Preserve new windows to be opened and load all popup urls in the same browser view
            clickad = true;
            Console.WriteLine(targetUrl);
            chromeBrowser.Load(targetUrl);
            //
            newBrowser = new ChromiumWebBrowser(targetUrl);
            return true;

        }

        private void button2_Click(object sender, EventArgs e)
        {
           
        }

        private void button3_Click(object sender, EventArgs e)
        {
            
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button3_Click_1(object sender, EventArgs e)
        {
            if (((IWebBrowser)chromeBrowser).CanGoBack)
                chromeBrowser.Back();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (((IWebBrowser)chromeBrowser).CanGoForward)
                chromeBrowser.Forward();
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            chromeBrowser.Reload();
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (chromeBrowser != null) chromeBrowser.Load(textBox1.Text);
            }
        }

        public void OnAfterCreated(IWebBrowser browserControl, IBrowser browser)
        {
        }

        public bool DoClose(IWebBrowser browserControl, IBrowser browser)
        {
            return false;
        }

        public void OnBeforeClose(IWebBrowser browserControl, IBrowser browser)
        {
           
        }
    }
    class CefCustomObject
    {
        // Declare a local instance of chromium and the main form in order to execute things from here in the main thread
        private static ChromiumWebBrowser _instanceBrowser = null;
        // The form class needs to be changed according to yours
        private static navegador _instanceMainForm = null;


        public CefCustomObject(ChromiumWebBrowser originalBrowser, navegador mainForm)
        {
            _instanceBrowser = originalBrowser;
            _instanceMainForm = mainForm;
        }

        public void showDevTools()
        {
            _instanceBrowser.ShowDevTools();
        }

        public void opencmd()
        {
            ProcessStartInfo start = new ProcessStartInfo("cmd.exe", "/c pause");
            Process.Start(start);
        }
    }

    public class history
    {
        public history(string tt, string ad, DateTime when,Bitmap ic)
        {
            favicon = ic;
            title = tt;
            adress = ad;
            time = when;
        }
        public string title, adress;
        public DateTime time;
        public Bitmap favicon;
    }

    public static class ExtensionMethods
    {
        public static void change_text_from_different_thread(this TextBox item, string text)
        {
            item.Invoke(new EventHandler(delegate
            {
                item.Text = text;
            }));

        }
        public static void change_text_from_different_thread(this Label item, string text)
        {
            item.Invoke(new EventHandler(delegate
            {
                item.Text = text;
            }));

        }
    }
}
