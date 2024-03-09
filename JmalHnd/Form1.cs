using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Xml;
using JmalHnd.Util.JMAXML;

namespace JmalHnd
{
    public partial class Form1 : Form
    {

        [DllImport("kernel32.dll")]
        public static extern bool AllocConsole();

        public Form1()
        {
            InitializeComponent();
            AllocConsole();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //Console.WriteLine(DateTime.Now.ToString("ss.ffff"));
            Gettimer.Interval = 3000 - DateTime.Now.Millisecond;
            Gettimer.Enabled = true;
            return;

            XmlDocument xml = new();
            xml.Load("D:\\Ichihai1415\\data\\jma\\xml\\https___www.data.jma.go.jp_developer_xml_feed_eqvol.xml");
            Feed.FeedParse(xml);
        }

        public static HttpClient client = new();

        private void Gettimer_Tick(object sender, EventArgs e)
        {
            GetFeed().ConfigureAwait(false);
        }

        public async Task GetFeed()
        {
            while (DateTime.Now.Microsecond > 900)
                await Task.Delay(50);//.ConfigureAwait(false)‚·‚é‚ÆŽ~‚Ü‚é
            //Console.WriteLine(DateTime.Now.ToString("ss.ffff"));
            Gettimer.Interval = 1000 - DateTime.Now.Millisecond;

            DateTime now = DateTime.Now;
            string url;
            switch (now.Second)
            {
                case 0:
                    url = "https://www.data.jma.go.jp/developer/xml/feed/regular.xml";
                    break;
                case 15:
                    url = "https://www.data.jma.go.jp/developer/xml/feed/extra.xml";
                    break;
                case 30:
                    url = "https://www.data.jma.go.jp/developer/xml/feed/eqvol.xml";
                    break;
                case 45:
                    url = "https://www.data.jma.go.jp/developer/xml/feed/other.xml";
                    break;
                default:
                    return;
            }

            XmlDocument xml = new();
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine($"{DateTime.Now:HH:mm:ss}  Get {url}");
            xml.LoadXml(await client.GetStringAsync(url).ConfigureAwait(false));
            Feed.FeedParse(xml);
        }
    }
}