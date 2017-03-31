using SoaProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace SoaProject.Controllers
{
    public class AdminController : ApiController
    {
        article007DataContext dc = new article007DataContext("Server=tcp:article007.database.windows.net,1433;Initial Catalog=article007;Persist Security Info=False;User ID=article007;Password=article_007;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");

        //RetriveAuthor     Admin 
        [Route("Retrive/User/{uname}")]
        public AuthorMaster GetAuthor(string mail)
        {
            //Fetch Author Info
            AuthorMaster author = (from x in dc.GetTable<AuthorMaster>()
                                   where x.mail == mail
                                   select x).SingleOrDefault();

            return author;
        }
    }
}
