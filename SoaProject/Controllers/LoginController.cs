using System;
using System.Linq;
using System.Web.Http;
using SoaProject.Models;
using System.Text;
using System.Security.Cryptography;
using System.Diagnostics;

namespace SoaProject.Controllers
{
    public class LoginController : ApiController
    {
        article007DataContext dc = new article007DataContext("Server=tcp:article007.database.windows.net,1433;Initial Catalog=article007;Persist Security Info=False;User ID=article007;Password=article_007;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");
/*
        [Route("Login")]
        [HttpPost]
        public int login([FromBody]LoginMaster l)
        {
            string value1;
            try
            {
                var usr = (from b in dc.GetTable<AuthorMaster>()
                         where b.mail == l.mail
                         select b).SingleOrDefault();
                
                var nce = (from a in dc.GetTable<RanNum>()
                             where a.mail == l.mail
                             select a).SingleOrDefault();
                using (MD5 md5Hash = MD5.Create())
                {
                    value1 = GetMd5Hash(md5Hash, usr.pass + nce.nonce.ToString());
                   
                }
                if (value1 == l.pass)
                    {
                        return usr.Id;
                    }
                    else
                    {
                        return -1;
                    }
            }
            catch (Exception e)
            {
               throw e;
            }
        }
  */      [Route("Article/Login")]
        [HttpGet]
        public int Getloginonly([FromUri]LoginMaster l)
        {

            try
            {
                var usr = (from b in dc.GetTable<AuthorMaster>()
                           where b.mail == l.mail
                           where b.pass == l.pass
                           select b).SingleOrDefault();

                if (usr != null)
                {
                    return usr.Id;
                }
                else
                {
                    return -1;
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        /*       [Route("GetNonce")]
               [HttpGet]
               public int GetNonce(string remail)
               {
                   try
                   {
                       var q = (from a in dc.GetTable<AuthorMaster>()
                                where a.mail == remail
                                select a).SingleOrDefault();
                       if(q != null)
                       {
                           Random rnd = new Random();
                           int num = rnd.Next(1000, 1000000);
                           RanNum rn = new RanNum()
                           {
                               mail = remail,
                               nonce = num
                           };
                           dc.RanNums.InsertOnSubmit(rn);
                           dc.SubmitChanges();
                           return num;    
                       }
                       return -1;
                   }
                   catch(Exception e)
                   {
                       throw e;
                   }
               }
          */
        static string GetMd5Hash(MD5 md5Hash, string input)
        {

            // Convert the input string to a byte array and compute the hash.
            byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));

            // Create a new Stringbuilder to collect the bytes
            // and create a string.
            StringBuilder sBuilder = new StringBuilder();

            // Loop through each byte of the hashed data 
            // and format each one as a hexadecimal string.
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }

            // Return the hexadecimal string.
            return sBuilder.ToString();
        }
    }
}