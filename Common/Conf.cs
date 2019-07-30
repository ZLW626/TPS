using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Script.Common
{
    public class Conf
    {
        public static short MSG_CS_LOGIN = 0x1001;
        public static short MSG_CS_REGISTER = 0x1002;
        public static short MSG_SC_LOGIN_CONFIRM = 0x1003;

        public static short MSG_SC_CONFIRM = 0x2001;

        public static short MSG_SC_ENEMY_INITIALIZE = 0x3002;
        public static short MSG_CS_ASK_FOR_ENEMY = 0x3001;

        public static int NET_HEAD_LENGTH_SIZE = 4;
        public static int NET_SID_CID_LENGTH_SIZE = 2;
    }
}
