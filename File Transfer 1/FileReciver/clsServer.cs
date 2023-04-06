namespace FileReciver
{
    using System;
    using System.Net;
    using System.Net.Sockets;

    internal class Server
    {
        private Socket s;
        private int port;
        private bool isRunning;

        internal Server(int port)
        {
            this.port = port;
        }

        internal bool StartServer()
        {
            bool res = false;

            try
            {
                StopServer();
                this.s = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                this.s.Bind(new IPEndPoint(IPAddress.Parse("0.0.0.0"),this.port));
                this.s.Listen(0);

                this.s.BeginAccept(new AsyncCallback(OnAccept), null);

            }
            catch
            {
                StopServer();
            }


            return res;
        }

        internal bool StopServer()
        {
            bool res = false;

            try
            {
                if (!(this.s is null))
                {
                    this.s.Close();
                    this.s.Dispose();
                }

                this.s = null;
                this.isRunning = false;
            }
            catch
            {

            }
            return res;
        }

        private void OnAccept(IAsyncResult ar)
        {
            try
            {
                Socket c = this.s.EndAccept(ar);
                new RequestProcess(c);
                this.s.BeginAccept(new AsyncCallback(OnAccept), null); ;

            }
            catch
            {
                StopServer();
            }
        }

        internal bool IsRunning
        {
            get
            {
                return this.isRunning;
            }

        }
    }
}
