using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ImageServiceWeb.Models
{
    public class PhotosModel
    {
        [Display(Name = "photos")]
        public List<ThumbnailPhoto> photos = new List<ThumbnailPhoto>();
    }
}