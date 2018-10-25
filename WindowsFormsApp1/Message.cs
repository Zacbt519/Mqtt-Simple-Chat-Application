using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp1
{
    public class Message
    {
        public string UserName { get; set; }
        public DateTime TimeSent = DateTime.Now.ToLocalTime();
        public string UserMessage { get; set; }

    }
}
