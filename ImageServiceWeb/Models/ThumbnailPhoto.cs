﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ImageServiceWeb.Models
{
    public class ThumbnailPhoto
    {
        static int count = 0;

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

        public ThumbnailPhoto(string name, string year, string month, string path)
        {
            this.Name = name;
            this.Year = year;
            this.Month = month;
            this.Path = path;
            count++;
            ID = count;
        }

        public void copy(ThumbnailPhoto thumbnailPhoto)
        {
            this.Name = thumbnailPhoto.Name;
            this.Year = thumbnailPhoto.Year;
            this.Month = thumbnailPhoto.Month;
            this.Path = thumbnailPhoto.Path;
        }
    }
}