using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ImageServiceWeb.Models
{
    public class Student
    {
        static int count = 0;
        public Student()
        {
            count++;
            num = count;
        }
        public void copy(Student student)
        {
            FirstName = student.FirstName;
            LastName = student.LastName;
            ID = student.ID;
        }
        [Required]
        [Display(Name = "num")]
        public int num { get; set; }
        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "FirstName")]
        public string FirstName { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "LastName")]
        public string LastName { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "ID")]
        public string ID { get; set; }
    }
}