
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
        [HttpGet]
        [Route("Upload")]
        public string GetUploadArticle([FromUri] ArticleMaster newArticle)
        {
            //Create hash , return as url
            using (MD5 md5Hash = MD5.Create())
            {
                newArticle.url = GetMd5Hash(md5Hash, (newArticle.text + newArticle.title + newArticle.author_id.ToString()));

                //Upload to db
               
                try
                {
                    dc.ArticleMasters.InsertOnSubmit(newArticle);
                    dc.SubmitChanges();
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
        
        [Route("Retrive/Article/{url}")]
        public ArticleReturn GetArticle(string url)
        {
            ArticleMaster articles = (from x in dc.GetTable<ArticleMaster>()
                           where x.url == url
                           select x).SingleOrDefault();

          
           AuthorMaster authorIs = (from auth in dc.GetTable<AuthorMaster>()
                                    where auth.Id == articles.author_id
                                    select auth).SingleOrDefault();     

            ArticleReturn ar = new ArticleReturn() {  Id = articles.Id, author_id = articles.author_id,
                                                      author_fname = authorIs.fname, author_lname = authorIs.lname,
                                                      title = articles.title, uploaded_date = articles.uploaded_date,
                                                      url = articles.url, author_uname = authorIs.uname,
                                                      text = articles.text};

            return ar;
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
                AuthorMaster authorIs = (from auth in dc.GetTable<AuthorMaster>()
                               where auth.Id == x.author_id
                               select auth).SingleOrDefault();
                lar.Add(new ArticleReturn() { Id = x.Id, author_id = x.author_id,  title = x.title, uploaded_date = x.uploaded_date, url = x.url });
            }
            return lar;
        
        }
        [Route("GetAllArticles")]
        public Object GetAllArticles()
        {
            var articles = (from a in dc.GetTable<ArticleMaster>()
                            select new { a.Id, a.author_id, a.uploaded_date, a.title, a.text, a.url });
            return articles;
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
