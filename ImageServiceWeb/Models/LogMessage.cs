using ImageServiceCommunication.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ImageServiceWeb.Models
{
    public class LogMessage
    {
        public LogMessage(MessageTypeEnum Type1, string Message1)
        {
            if (Type1 == MessageTypeEnum.FAIL)
            {
                Type = "FAIL";
            } else if (Type1 == MessageTypeEnum.INFO)
            {
                Type = "INFO";
            } else
            {
                Type = "WARNING";
            }
            this.Message = Message1;
        }
        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Type")]
        public string Type { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Message")]
        public string Message { get; set; }
    }
}