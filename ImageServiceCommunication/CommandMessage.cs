using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace ImageServiceCommunication
{
    public class CommandMessage
    {
        private int m_commandID;
        private string[] m_commandArgs;

        public CommandMessage(int commandId, string[] commandArgs)
        {
            this.m_commandID = commandId;
            this.m_commandArgs = commandArgs;
        }

        public int CommandID
        {
            get
            {
                return this.m_commandID;
            }
            set
            {
                this.m_commandID = value;
            }
        }

        public string[] CommandArgs
        {
            get
            {
                return this.m_commandArgs;
            }
            set
            {
                this.m_commandArgs = value;
            }
        }

        public string ToJSON()
        {
            JObject cmdobj = new JObject();
            cmdobj["CommandID"] = this.m_commandID;
            JArray args = new JArray(this.m_commandArgs);
            cmdobj["CommandArgs"] = args;
            return cmdobj.ToString();
        }

        public static CommandMessage ParseJSon(string str)
        {
            JObject cmdObj = JObject.Parse(str);
            JArray arr = (JArray)cmdObj["CommandArgs"];
            string[] args = arr.Select(c => (string)c).ToArray();
            return new CommandMessage((int)cmdObj["CommandID"], args);
        }
    }
}
