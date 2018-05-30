using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Hosting;
using ImageServiceCommunication;

namespace ImageServiceWeb.Models
{
    public class ImageWebModel
    {
        static string path = HostingEnvironment.MapPath("~/App_Data/studentsData.txt");
        static string[] lines = System.IO.File.ReadAllLines(@path);

        [Display(Name = "students")]
        public List<Student> students = new List<Student>()
        {
            new Student { FirstName = lines[0], LastName = lines[1], ID = lines[2] },
            new Student { FirstName = lines[3], LastName = lines[4], ID = lines[5] }
        };

        [Display(Name = "numOfImages")]
        public int numOfImages = 10;

        [Display(Name = "ifServiceConected")]
        public string ifServiceConected = checkIfServerConnected();

        public static string checkIfServerConnected()
        {
            if (ClientSingleton.Instance.IsConnected) { return "service connected"; }
            return "service is not connected";
        }
    }
}