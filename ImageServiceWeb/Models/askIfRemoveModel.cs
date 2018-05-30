using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ImageServiceWeb.Models
{
    public class AskIfRemoveModel
    {
        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "handler")]
        public string handler { get; set; }
    }
}