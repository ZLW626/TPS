using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using UnityEngine;
using System.Collections;
namespace Assets.Script.Common
{
    public class UniParam
    {
        public char type; //'s': str; 'i': int; 'c': short
        public string strVal;
        public int intVal;
        public short shortVal;
    }

    public class MsgCSBase
    {
        //private ArrayList paramters = new ArrayList();
        private List<UniParam> parameters = new List<UniParam>();
        protected MemoryStream memoryStream;
        protected BinaryWriter binaryWriter;
        protected short sid_cid;

        public MsgCSBase()
        {
            memoryStream = new MemoryStream();
            binaryWriter = new BinaryWriter(memoryStream);
        }
        protected void appendParamStr(string param)
        {
            UniParam uniParam = new UniParam();
            uniParam.type = 's';
            uniParam.strVal = param;

            parameters.Add(uniParam);
        }
 
        protected void appendParamInt(int param)
        {
            UniParam uniParam = new UniParam();
            uniParam.type = 'i';
            uniParam.intVal = param;
            parameters.Add(uniParam);
        }

        public byte[] Marshal()
        {
            int len = parameters.Count;
            for(int i = 0;i < len;++i)
            {
                switch(parameters[i].type)
                {
                    case 's':
                        binaryWriter.Write(Encoding.UTF8.GetBytes(parameters[i].strVal));
                        break;
                    case 'i':
                        binaryWriter.Write(parameters[i].intVal);
                        break;
                    default:
                        break;
                }
                if(i == 0)
                {
                    binaryWriter.Write(sid_cid);
                }

            }
            binaryWriter.Close();
            return memoryStream.ToArray();
        }
    }
    
    public class MsgCSLogin : MsgCSBase
    {
        public MsgCSLogin(string name, string password)
        {
            sid_cid = Conf.MSG_CS_LOGIN;
            int lenLenFlag = sizeof(int) * 2;
            int datalen = Conf.NET_HEAD_LENGTH_SIZE + Conf.NET_SID_CID_LENGTH_SIZE + name.Length + password.Length + lenLenFlag;

            appendParamInt(datalen);
            appendParamInt(name.Length);
            appendParamStr(name);
            appendParamInt(password.Length);
            appendParamStr(password);
        }

        //public byte[] Marshal()
        //{
        //    //byte[] bytes;
        //    //using (MemoryStream memoryStream = new MemoryStream())
        //    //MemoryStream memoryStream = new MemoryStream();
        //    //{
        //    //using (BinaryWriter binaryWriter = new BinaryWriter(memoryStream))
        //    //BinaryWriter binaryWriter = new BinaryWriter(memoryStream);
        //    //{
        //    //int datalen = Conf.NET_HEAD_LENGTH_SIZE + Conf.NET_SID_CID_LENGTH_SIZE + name.Length + password.Length;
        //    int lenLenflag = sizeof(int) * 2;
        //    int datalen = Conf.NET_HEAD_LENGTH_SIZE + Conf.NET_SID_CID_LENGTH_SIZE + name.Length + password.Length + lenLenflag;
        //    Debug.Log("datalen " + datalen);
        //    Debug.Log("lenLenflag " + lenLenflag);
        //    Debug.Log("name.Length " + name.Length);
        //    Debug.Log("password.Length " + password.Length);
        //    binaryWriter.Write(datalen);
        //    binaryWriter.Write(Conf.MSG_CS_LOGIN);
        //    binaryWriter.Write(name.Length);
        //    binaryWriter.Write(Encoding.UTF8.GetBytes(name));
        //    binaryWriter.Write(password.Length);
        //    binaryWriter.Write(Encoding.UTF8.GetBytes(password));
        //    binaryWriter.Close();
        //    Debug.Log("memoryStream.ToArray().Length " + memoryStream.ToArray().Length);
        //    //bytes = (byte[])memoryStream.ToArray().Clone();
        //    return memoryStream.ToArray();
        //    //}
        //    //}

        //    //return bytes;
        //}
    }

    public class MsgCSRegister:MsgCSBase
    {
        public MsgCSRegister(string name, string password)
        {
            sid_cid = Conf.MSG_CS_REGISTER;
            int lenLenFlag = sizeof(int) * 2;
            int datalen = Conf.NET_HEAD_LENGTH_SIZE + Conf.NET_SID_CID_LENGTH_SIZE + name.Length + password.Length + lenLenFlag;

            appendParamInt(datalen);
            appendParamInt(name.Length);
            appendParamStr(name);
            appendParamInt(password.Length);
            appendParamStr(password);
        }

        //public byte[] Marshal()
        //{
        //    int lenLenflag = size
        //}
    }

    public class MsgSCBase
    {
        public int sid;
        public int cid;
        protected MemoryStream memoryStream;
        protected BinaryReader binaryReader;

        public MsgSCBase()
        {
            //memoryStream = new MemoryStream();
            //binaryReader = new BinaryReader(memoryStream);
        }
    }

    public class MsgSCConfirm : MsgSCBase
    {
        public int confirm;

        public MsgSCConfirm()
        {
            confirm = -1;
        }

        public MsgSCConfirm Unmarshal(byte[] bytes)
        {
            memoryStream = new MemoryStream(bytes);
            binaryReader = new BinaryReader(memoryStream);
            //int dataLen = binaryReader.ReadInt32();
            short sid_cid = binaryReader.ReadInt16();
            confirm = binaryReader.ReadInt32();
            return this;
        }
    }

    public class UnifromUnmarshal
    {
        //protected MemoryStream memoryStream;
        //protected BinaryReader binaryReader;

        public MsgSCBase Unmarshal(byte[] bytes)
        {
            MsgSCBase msgSCBase = null;
            //memoryStream = new MemoryStream(bytes);
            //binaryReader = new BinaryReader(memoryStream);
            ///int dataLen = binaryReader.ReadInt32();
            //short sid_cid = binaryReader.ReadInt16();
            short sid_cid = ConvertBytesToInt16(bytes);
            switch(sid_cid)
            {
                case 0x2001:
                    msgSCBase = new MsgSCConfirm().Unmarshal(bytes);
                    msgSCBase.sid = sid_cid >> 8;
                    msgSCBase.cid = sid_cid & 0x00FF;
                    break;
                default:
                    return null;
            }
            return msgSCBase;
        }

        private short ConvertBytesToInt16(byte[] bytes)
        {
            return (short)(bytes[0] & 0xff | ((bytes[1] & 0xff) << 8));
        }
    }
}
