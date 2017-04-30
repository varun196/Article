using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SoaProject.Models
{
    public class ArticleReturn
    {
          public int  Id { get; set; }
          public int author_id { get; set; }
          public string author_fname { get; set; }
          public string author_lname { get; set; }
          public string text { get; set; }
          public string author_uname { get; set; }
          public string title { get; set; }
          public  DateTime? uploaded_date { get; set; }     //Cause Null possible
          public string url { get; set; }
    }
}