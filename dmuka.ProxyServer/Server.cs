using dmuka.Semaphore;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;

namespace dmuka.ProxyServer
{
    public class Server : IDisposable
    {
        #region Constructors
        public Server(string hostname, int port, int proxyServerPort, int coreCount = 4)
        {
            this.ProxyServerPort = proxyServerPort;
            this.Listener = new TcpListener(IPAddress.Any, ProxyServerPort);

            this.Hostname = hostname;
            this.Port = port;

            this.ActionQueue = new ActionQueue(coreCount);
        }
        #endregion

        #region Variables
        /// <summary>
        /// Your proxy port on your os
        /// </summary>
        public int ProxyServerPort { get; private set; }

        /// <summary>
        /// Waiting byte[] from this hostname
        /// </summary>
        public string Hostname { get; private set; }
        /// <summary>
        /// Waiting byte[] on this port
        /// </summary>
        public int Port { get; private set; }

        internal ActionQueue ActionQueue = null;
        /// <summary>
        /// Listener for read the original client
        /// </summary>
        internal TcpListener Listener { get; private set; }

        internal List<Client> Clients = new List<Client>();
        public int ClientCount
        {
            get
            {
                return this.Clients.Count;
            }
        }

        private bool _disposed = false;
        private bool _started = false;
        #endregion

        #region Methods
        /// <summary>
        /// Run proxy server as sync
        /// </summary>
        public void Start()
        {
            if (this._disposed == true)
                throw new ObjectDisposedException("Server");
            if (this._started == true)
                throw new Exception("State is open!");
            this._started = true;

            try
            {
                this.Listener.Start();
                this.ActionQueue.Start();

                while (_disposed == false)
                {
                    Client newClient = new Client(this);
                    newClient.Start();

                    lock (this.Clients)
                    {
                        this.Clients.Add(newClient);
                    }
                }
            }
            catch (Exception ex)
            {
                if (this._disposed == false)
                    throw ex;
            }
        }

        public void Dispose()
        {
            if (this._disposed == true)
                throw new ObjectDisposedException("Server");

            this._disposed = true;

            this.ActionQueue.Dispose();

            lock (Clients)
            {
                var clientsAsArray = this.Clients.ToArray();
                foreach (var client in clientsAsArray)
                    client.Dispose();
            }

            try
            { this.Listener.Stop(); }
            catch { }
        }
        #endregion
    }
}
