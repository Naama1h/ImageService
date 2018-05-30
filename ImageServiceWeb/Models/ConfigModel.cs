using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ImageServiceWeb.Models
{
    public class ConfigModel
    {
        [Display(Name = "handlers")]
        public List<string> handlers = new List<string>() {};

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "outputDir")]
        public string outputDir { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "sourceName")]
        public string sourceName { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "logName")]
        public string logName { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "thumbnailSize")]
        public string thumbnailSize { get; set; }
    }
}