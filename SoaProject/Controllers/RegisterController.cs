using System;
using System.Linq;
using System.Web.Http;
using SoaProject.Models;

namespace SoaProject.Controllers
{
    public class RegisterController : ApiController
    {
        article007DataContext dc = new article007DataContext("Server=tcp:article007.database.windows.net,1433;Initial Catalog=article007;Persist Security Info=False;User ID=article007;Password=article_007;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");
        [Route("Register")]
        [HttpPost]
        public bool register([FromBody]RegisterMaster a)
        {
            try
            {
                  bool b = checkForMail(a.mail);
                  if (b == true)
                  {
                    AuthorMaster au = new AuthorMaster()
                    {
                        mail = a.mail,
                        uname = a.uname,
                        fname = a.fname,
                        pass = a.pass,
                        lname = a.lname
                    };
                      dc.AuthorMasters.InsertOnSubmit(au);
                      dc.SubmitChanges();
                      return true;
                  }
                  else
                      return false;
              }
            catch (Exception e)
            {
                throw e;
            }
        }
        bool checkForMail(string mail)
        {
            try
            {
                var a = (from b in dc.GetTable<AuthorMaster>()
                        where b.mail == mail
                        select b).ToList();
                if (a.Count != 0)
                    return false;
                else
                    return true;
            }
            catch(Exception e)
            {
                throw e;
            }
        }
    }
}
