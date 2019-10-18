using System;
using System.Collections.Generic;
using System.Text;

namespace Models
{
    public class MQTTMessage_Recv
    {
        public string src { get; set; }
        public string dst { get; set; }
        public string msg { get; set; }
        //public string state { get; set; }
        //public Enum state
        //{
        //    refresh = 0,
        //    play = 1,
        //    playing = 3

        //};

    }
}
