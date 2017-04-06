
using System.Web.Http;
using SoaProject.Models;
using System.Security.Cryptography;
using System.Text;
using System.Linq;
using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace SoaProject.Controllers
{
    public class ArticleController : ApiController
    {
        article007DataContext dc = new article007DataContext("Server=tcp:article007.database.windows.net,1433;Initial Catalog=article007;Persist Security Info=False;User ID=article007;Password=article_007;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");
        
        //UploadArticle and return md5
        [HttpPost]
        [Route("Upload")]
        public string PostArticle([FromBody] ArticleMaster newArticle)
        {
            //Create hash , return as url
            using (MD5 md5Hash = MD5.Create())
            {
                newArticle.url = GetMd5Hash(md5Hash, (newArticle.text + newArticle.title + newArticle.author_id.ToString()));

                //Upload to db
                dc.ArticleMasters.InsertOnSubmit(newArticle);
                dc.SubmitChanges();
                try
                {
                    newArticle = (from x in dc.GetTable<ArticleMaster>()
                                  where x == newArticle
                                  select x).Single();
                }
                catch(Exception e)
                {
                    return newArticle.url;              //Same author , title and body {text} then consider duplicate
                }
                newArticle.url = GetMd5Hash(md5Hash, newArticle.Id.ToString());

                dc.SubmitChanges();
            }
            return newArticle.url;
        }
        
        [Route("GetAllArticles")]
        public Object GetAllArticles()
        {
            var articles = (from a in dc.GetTable<ArticleMaster>()
                            select new { a.Id , a.author_id , a.uploaded_date , a.title ,a.text ,a.url});
            return articles;
        }
        //Retrive Article
        [Route("Retrive/Article/{url}")]
        public ArticleMaster GetArticle(string url)
        {
            List<ArticleMaster> li = new List<ArticleMaster>();
            //fetch article via url
            ArticleMaster q = fetchAM(url);
            ArticleMaster am = new ArticleMaster();
            am.copy(q);
            return am;
        }
        //Linq Query 
        private ArticleMaster fetchAM(string url)
        {
            ArticleMaster q = (from x in dc.GetTable<ArticleMaster>()
                               where x.url == url
                               select x).SingleOrDefault();
            if (q == null)
            {
                throw new Exception();
            }
            return q;
        }
        
       // Get all articles uploaded by author
        [Route("Retrive/ArticlesBy/{id}")]
        public IEnumerable<ArticleReturn> getAllArticlesBy(int id)
        {
            var articles = from x in dc.GetTable<ArticleMaster>()
                           where x.author_id == id
                           select x;

            List<ArticleReturn> lar = new List<ArticleReturn>();
            
            foreach (var x in articles)
            {
                lar.Add(new ArticleReturn() { Id = x.Id, author_id = x.author_id, title = x.title, uploaded_date = x.uploaded_date, url = x.url });
            }
            return lar;
        
        }

        //The Md5 method
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
