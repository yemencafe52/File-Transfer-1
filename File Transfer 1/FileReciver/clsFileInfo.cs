using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileReciver
{
    internal class FileInfo
    {
        private long size;
        private string fileName;
        internal FileInfo(long size,string fileName)
        {
            this.size = size;
            this.fileName = fileName;
        }

        internal long Size
        {
            get
            { 
                return this.size;
            }
        }

        internal string Name
        {
            get
            {
                return this.fileName;
            }
        }

        internal byte[] ToBytes()
        {
            byte[] fileNameBytes = Encoding.UTF8.GetBytes(this.fileName);
            byte[] res = new byte[8+fileName.Length];

            unsafe
            {
                fixed (void* p0 = &this.size)
                {
                    for(int i=0;i<8;i++)
                    {
                        res[i] = *(((byte*)p0) + i);
                    }
                }
            }

            for(int i=0;i<fileNameBytes.Count();i++)
            {
                res[i + 8] = fileNameBytes[i];
            }

            return res;
        }

        internal FileInfo(byte[] ar)
        {
            unsafe
            {
                fixed(void* p0 = &ar[0])
                {
                    this.size = (*(long*)p0);
                }
            }

            byte[] ar2 = new byte[ar.Length - 8];

            for(int i=8;i<ar.Length;i++)
            {
                ar2[i - 8] = ar[i];
            }

            this.fileName = Encoding.UTF8.GetString(ar2);
        }
    }

}
