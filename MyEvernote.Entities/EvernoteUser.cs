using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyEvernote.Entities
{
    [Table("EvernoteUsers")]
    public class EvernoteUser : MyEntityBase
    {
        [DisplayName("İsim"),
            StringLength(25, ErrorMessage = "{0} alanı max {1} karakter olmalıdır.")]
        public string Name { get; set; }
        [DisplayName("Soyisim"),
            StringLength(25, ErrorMessage = "{0} alanı max {1} karakter olmalıdır.")]
        public string Surname { get; set; }
        [DisplayName("Kullanıcı Adı"),
            Required(ErrorMessage = "{0} alanı gereklidir."),
            StringLength(25, ErrorMessage = "{0} alanı max {1} karakter olmalıdır.")]
        public string Username { get; set; }
        [DisplayName("E-Posta"),
            Required(ErrorMessage = "{0} alanı gereklidir."),
            StringLength(70, ErrorMessage = "{0} alanı max {1} karakter olmalıdır.")]
        public string Email { get; set; }
        [DisplayName("Şifre"),
            Required(ErrorMessage = "{0} alanı gereklidir."),
            StringLength(25, ErrorMessage = "{0} alanı max {1} karakter olmalıdır.")]
        public string Password { get; set; }
        [StringLength(30)]                   //images/user_12.jpg
        public string ProfileImageFilename { get; set; }
        public bool IsActive { get; set; }
        [Required]
        public Guid ActivateGuid { get; set; } //kullanıcının id sini karmaşıklaştırmak için kullanıldı(myevernote.com/user/activate/asda-adad)
        [Required]
        public bool IsAdmin { get; set; } //kullanıcı admin mi?


        public virtual List<Note> Notes { get; set; }//kullanıcının birden çok notu olabilir.
        public virtual List<Comment> Comments { get; set; }
        public virtual List<Liked> Likes { get; set; }
    }
}
