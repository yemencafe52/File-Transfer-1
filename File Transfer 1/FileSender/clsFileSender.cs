namespace FileSender
{
    using System;
    using System.IO;
    using System.Net.Sockets;
    internal class FileSender
    {
        private string ip;
        private int port;
        private string filePath;
        private bool isBusy;

        internal FileSender(
         string ip,
         int port,
         string filePath    
            )
        {
            this.ip = ip;
            this.port = port;
            this.filePath = filePath;
        }

        internal bool Send(IProgress<float> p)
        //internal bool Send()
        {
            bool res = false;

            this.isBusy = true;

            try
            {
                //Task.Run(() =>
                //{
                    System.IO.FileInfo fi = new FileInfo(this.filePath);
                    MyFileInfo myFileInfo = new MyFileInfo(fi.Length, fi.Name);

                    Socket s = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                    s.Connect(this.ip, this.port);

                    s.Send(myFileInfo.ToBytes());
                    byte[] buffer = new byte[] { 0 };
                    int len = s.Receive(buffer);

                if (len == 1)
                {
                    if (buffer[0] == 254)
                    {
                        FileStream fs = new FileStream(this.filePath, FileMode.Open);

                        int bufferSize = 1024 * 1024 * 8;
                        buffer = new byte[bufferSize];

                        while (fs.Length > fs.Position)
                        {
                            if (!(fs.Length - fs.Position > bufferSize))
                            {
                                bufferSize = (int)(fs.Length - fs.Position);
                                buffer = new byte[bufferSize];
                            }

                           
                            fs.Read(buffer, 0, buffer.Length);
                            s.Send(buffer);

                            p.Report((((float)fs.Position / ((float)fs.Length)) * ((float)100)));
                        }


                        s.Close();
                        fs.Close();

                        res = true;
                    } 
                }
              
              //  });

            }
            catch
            { }

            this.isBusy = false;
            return res;

        }

        internal bool IsBusy
        {
            get
            {
                return this.isBusy;
            }
        }

    }
}
