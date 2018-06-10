using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ImageServiceWeb.Models
{
    public class ThumbnailPhoto
    {
        static int count = 0;               // number of photos

        [Required]
        [Display(Name = "ID")]
        public int ID { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Name")]
        public string Name { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Year")]
        public string Year { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Month")]
        public string Month { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Path")]
        public string Path { get; set; }

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="name">the name of the photo</param>
        /// <param name="year">the year of the photo</param>
        /// <param name="month">the month of the photo</param>
        /// <param name="path">the path of the photo</param>
        public ThumbnailPhoto(string name, string year, string month, string path)
        {
            this.Name = name;
            this.Year = year;
            this.Month = month;
            this.Path = path;
            count++;
            ID = count;
        }

        /// <summary>
        /// copy photo
        /// </summary>
        /// <param name="thumbnailPhoto">the photo</param>
        public void copy(ThumbnailPhoto thumbnailPhoto)
        {
            this.Name = thumbnailPhoto.Name;
            this.Year = thumbnailPhoto.Year;
            this.Month = thumbnailPhoto.Month;
            this.Path = thumbnailPhoto.Path;
        }
    }
}