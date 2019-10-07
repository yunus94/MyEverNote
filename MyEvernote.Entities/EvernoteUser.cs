using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyEvernote.Entities
{
    public class EvernoteUser:MyEntityBase
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public bool IsActive { get; set; } 
        public Guid ActivateGuid { get; set; } //kullanıcının id sini karmaşıklaştırmak için kullanıldı(myevernote.com/user/activate/asda-adad)
        public bool IsAdmin { get; set; } //kullanıcı admin mi?
        public virtual List<Note> Notes { get; set; }//kullanıcının birden çok notu olabilir.
        public virtual List<Comment> Comments { get; set; }
        public virtual List<Liked> Likes { get; set; }
    }
}
