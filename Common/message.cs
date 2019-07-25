using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using UnityEngine;
namespace Assets.Script.Common
{
    public class MsgCSLogin
    {
        private int name;
        private int password;

        public MsgCSLogin(int name, int password)
        {
            this.name = name;
            this.password = password;
        }

        public byte[] Marshal()
        {
            //byte[] bytes;
            //using (MemoryStream memoryStream = new MemoryStream())
            MemoryStream memoryStream = new MemoryStream();
            //{
                //using (BinaryWriter binaryWriter = new BinaryWriter(memoryStream))
                BinaryWriter binaryWriter = new BinaryWriter(memoryStream);
                //{
                    //int datalen = Conf.NET_HEAD_LENGTH_SIZE + Conf.NET_SID_CID_LENGTH_SIZE + name.Length + password.Length;
                    int datalen = Conf.NET_HEAD_LENGTH_SIZE + Conf.NET_SID_CID_LENGTH_SIZE + 8;
                    binaryWriter.Write(datalen);
                    binaryWriter.Write(Conf.MSG_CS_LOGIN);
                    binaryWriter.Write(name);
                    binaryWriter.Write(password);
                    binaryWriter.Close();
                    //bytes = (byte[])memoryStream.ToArray().Clone();
                    return memoryStream.ToArray();
                //}
            //}
            
            //return bytes;
        }
    }
}
