namespace FileReciver
{
    using System;
    using System.Net.Sockets;
    using System.IO;
    internal class RequestProcess
    {
        internal RequestProcess(Socket s)
        {
            try
            {
                byte[] buffer = new byte[1024 * 8];
                int len = s.Receive(buffer);

                if (len > 0)
                {
                    byte[] temp = new byte[len];
                    Array.Copy(buffer, 0, temp, 0, len);

                    FileInfo fi = new FileInfo(temp);

                    try
                    {
                        Directory.CreateDirectory(Directory.GetCurrentDirectory() + "\\Downloads");
                    }
                    catch
                    {
                        throw new Exception();
                    }

                    s.Send(new byte[] { 254 });

                    buffer = new byte[1024 * 8];

                  
                    string filePath = Directory.GetCurrentDirectory() + "\\downloads\\" + fi.Name;

                    while (s.Connected)
                    {
                        len = s.Receive(buffer);
                        if (len > 0)
                        {
                            //temp = new byte[len];
                            //Array.Copy(buffer, 0, temp, 0, len);

                            FileStream fs = new FileStream(filePath, FileMode.Append);
                            fs.Write(buffer, 0, len);
                            //fs.Write(temp, 0, len);

                            if (fs.Length >= fi.Size)
                            {
                                fs.Close();
                                break;
                            }

                            fs.Close();
                        }

                      
                    }

                    s.Close();
                }

            }
            catch
            { }
        }
    }
}
