using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyEvernote.Entities
{
    public class Note:MyEntityBase
    {
        public string Title { get; set; }
        public string Text { get; set; }
        public bool IsDraft { get; set; } //kullanıcı notunu hemen paylaşmak istemezse taslak olarak kaydedebilir.
        public int LikeCount { get; set; }
        public int CategoryId { get; set; }//Sql içerisinde category için tekrar select atmasın diye tek sorguda categoryId yi alıyoruz. 

        public virtual EvernoteUser Owner { get; set; } //bir notun tek bir kullanıcısı olmalı.
        public virtual List<Comment> Comments { get; set; }
        public virtual Category Category { get; set; }
        public virtual List<Liked> Likes { get; set; }//çoklu ilişki


    }
}
