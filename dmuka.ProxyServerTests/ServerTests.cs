using dmuka.ProxyServer;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace dmuka.ProxyServerTests
{
    [TestClass]
    public class ServerTests
    {
        [TestMethod]
        public void ProxyTest()
        {
            bool testCompleted = false;
            DateTime testStartDate = DateTime.Now;
            var testHaveError = false;
            var testErrorMessage = "";

            var testThread = new Thread(() =>
            {
                try
                {
                    var processCompleted = false;

                    Random rndm = new Random();
                    byte[] originalData = new byte[(1 * 1024/*KB*/ * 1024/*MB*/) + 100000];
                    for (int i = 0; i < originalData.Length; i++)
                        originalData[i] = (byte)rndm.Next(0, 256);

                    TcpListener listener = new TcpListener(IPAddress.Any, 33445);
                    listener.Start();

                    new Thread(() =>
                    {
                        var client = listener.AcceptTcpClient();
                        var stream = client.GetStream();

                        while (processCompleted == false)
                        {
                            if (stream.DataAvailable == true)
                            {
                                int bufferIndex = 0;

                                byte[] buffer = new byte[2048]; // read in chunks of 2KB
                                int bytesRead;
                                while ((bytesRead = stream.Read(buffer, 0, buffer.Length)) > 0)
                                {
                                    for (int i = 0; i < bytesRead; i++)
                                    {
                                        if (originalData[bufferIndex] != buffer[i])
                                        {
                                            testHaveError = true;
                                            testErrorMessage = "Read wrong data from proxy server!";
                                        }

                                        bufferIndex++;
                                        if (bufferIndex == originalData.Length)
                                        {
                                            processCompleted = true;
                                            break;
                                        }
                                    }
                                }
                            }
                        }
                    }).Start();

                    Server server = new Server("127.0.0.1", 33445, 22334);

                    new Thread(() =>
                    {
                        try
                        {
                            server.Start();
                        }
                        catch
                        {
                            testHaveError = true;
                            testErrorMessage = "Server start exception!";

                            processCompleted = true;
                        }
                    }).Start();

                    var clientForConnectToProxy = new TcpClient();
                    clientForConnectToProxy.Connect("127.0.0.1", 22334);
                    var clientForConnectToProxyStream = clientForConnectToProxy.GetStream();
                    clientForConnectToProxyStream.Write(originalData, 0, originalData.Length / 2);
                    clientForConnectToProxyStream.Flush();

                    Thread.Sleep(500);
                    clientForConnectToProxyStream.Write(originalData, originalData.Length / 2, originalData.Length / 2);
                    clientForConnectToProxyStream.Flush();

                    while (processCompleted == false)
                        Thread.Sleep(1);

                    clientForConnectToProxy.Close();

                    Thread.Sleep(1000);

                    if (server.ClientCount != 0)
                    {
                        testHaveError = true;
                        testErrorMessage = "Connection not closed!";
                    }
                }
                catch
                {
                    testHaveError = true;
                    testErrorMessage = "Any exception!";
                }

                testCompleted = true;
            });
            testThread.Start();

            while (testCompleted == false)
            {
                var diffTestDate = DateTime.Now.AddTicks(testStartDate.Ticks * -1);
                if (diffTestDate.Second > 10)
                {
                    Assert.Fail(message: "Timeout!");
                }
            }

            if (testHaveError)
                Assert.Fail(testErrorMessage);
        }
    }
}
