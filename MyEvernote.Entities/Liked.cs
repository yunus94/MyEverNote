using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyEvernote.Entities
{
   public class Liked
    {
        //Aralarında çoktan çoğa bir ilişki vardır.
        public virtual Note Note { get; set; }
        public virtual EvernoteUser LikedUser { get; set; }

    }
}
