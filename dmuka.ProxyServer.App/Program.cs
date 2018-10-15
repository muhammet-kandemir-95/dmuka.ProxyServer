using System;

namespace dmuka.ProxyServer.App
{
    class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                try
                {
                    Console.Write("Type host name = ");
                    string hostName = Console.ReadLine();

                    Console.Write("Type host port = ");
                    int port = Convert.ToInt32(Console.ReadLine());

                    Console.Write("Type proxy port = ");
                    int proxyPort = Convert.ToInt32(Console.ReadLine());

                    Server server = new Server(hostName, port, proxyPort, coreCount: 100);
                    server.Start();
                }
                catch (Exception)
                {
                    Console.WriteLine("Server crached!");
                }
            }
        }
    }
}
