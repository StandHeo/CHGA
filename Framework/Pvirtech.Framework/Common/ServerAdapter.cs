using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pvirtech.Framework
{
    public class ServerAdapter
    {

        public ServerAdapter()
        { }

        public string ServerName
        { get; set; }

        public string Port
        { get; set; }


        public void Init(string serverip, string port)
        {
            ServerName = serverip;
            Port = port;

        }


        public virtual void Start()
        {}

        public virtual void Stop()
        { }
        
    }

}
