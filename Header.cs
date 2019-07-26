using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Net;

namespace Assets.Script
{
    class Header
    {
        private MemoryStream ms;
        private BinaryReader br;
        private BinaryWriter bw;

        public Header(byte[] buffer = null)
        {
            if (buffer == null)
                ms = new MemoryStream();
            else
                ms = new MemoryStream(buffer);

            br = new BinaryReader(ms);
            bw = new BinaryWriter(ms);
        }

        public void Close()
        {
            ms.Close();
            br.Close();
            bw.Close();
        }

        public long ReadInt64()
        {
            return IPAddress.HostToNetworkOrder(br.ReadInt64());
        }

        public int ReadInt32()
        {
            return IPAddress.HostToNetworkOrder(br.ReadInt32());
        }

        public int ReadInt16()
        {
            return IPAddress.HostToNetworkOrder(br.ReadInt16());
        }

        public byte ReadByte()
        {
            return br.ReadByte();
        }

        public string ReadString8()
        {
            return System.Text.Encoding.UTF8.GetString
                   (
                        br.ReadBytes(ReadByte())
                   );
        }

        public string ReadString16()
        {
            return System.Text.Encoding.UTF8.GetString
                   (
                        br.ReadBytes(ReadInt16())
                   );
        }

        public long Seek(long offset)
        {
            return ms.Seek(offset, SeekOrigin.Begin);
        }

        // -------------------------------------------------------------------------------  

        public void WriteByte(byte value)
        {
            bw.Write(value);
        }


        public void WriteInt16(short value)
        {
            bw.Write
            (
                BitConverter.GetBytes
                (
                    IPAddress.HostToNetworkOrder(value)
                )
            );
        }

        public void WriteInt32(int value)
        {
            bw.Write
            (
                BitConverter.GetBytes
                (
                    IPAddress.HostToNetworkOrder(value)
                )
            );
        }

        public void WriteInt64(long value)
        {
            bw.Write
            (
                BitConverter.GetBytes
                (
                    IPAddress.HostToNetworkOrder(value)
                )
            );
        }

        public void WriteString8(string value)
        {
            WriteByte
            (
                (byte)value.Length
            );


            bw.Write
            (
                System.Text.Encoding.UTF8.GetBytes(value)
            );
        }

        public void WriteString16(string value)
        {
            WriteInt16
            (
                (short)value.Length
            );


            bw.Write
            (
                System.Text.Encoding.UTF8.GetBytes(value)
            );
        }

        public byte[] GetBuffer()
        {
            return ms.ToArray();
        }

        public int GetLength()
        {
            return (int)ms.Length;
        }
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
