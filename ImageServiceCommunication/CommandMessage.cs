using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Web.Script.Serialization;

namespace ImageServiceCommunication
{
    public class CommandMessage
    {
        private int m_commandID;                                    // The command ID
        private string[] m_commandArgs;                             // The command args

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="commandId">The command Id</param>
        /// <param name="commandArgs">The command Args</param>
        public CommandMessage(int commandId, string[] commandArgs)
        {
            this.m_commandID = commandId;
            this.m_commandArgs = commandArgs;
        }

        /// <summary>
        /// Get and Set of command ID
        /// </summary>
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

        /// <summary>
        /// Get and Set of command Args
        /// </summary>
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

        /// <summary>
        /// serialize the object
        /// </summary>
        /// <returns>The Serialize object</returns>
        public string ToJSON()
        {
            return JsonConvert.SerializeObject(this);
        }

        /// <summary>
        /// Deserialize The Object
        /// </summary>
        /// <param name="str">The string message</param>
        /// <returns>The Deserialize Object</returns>
        public static CommandMessage ParseJSon(string str)
        {
            return JsonConvert.DeserializeObject<CommandMessage>(str);
        }
    }
}
