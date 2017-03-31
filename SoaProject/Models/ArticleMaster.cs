using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace SoaProject.Models
{
    //[Serializable]
    //[DataContract(IsReference = true)]
    public partial class ArticleMaster
    {
        public void copy(ArticleMaster q)
        {
            this.Id = q.Id;
            this.author_id = q.author_id;
            this.text = q.text;
            this.title = q.title;
            this.uploaded_date = q.uploaded_date;
            this.url = q.url;
          }
    }
    //[Serializable]
    //[DataContract(IsReference =true)]
    public partial class AuthorMaster
    {}
}