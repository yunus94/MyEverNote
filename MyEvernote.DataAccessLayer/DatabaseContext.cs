using MyEvernote.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyEvernote.DataAccessLayer
{
   public class DatabaseContext:DbContext
    {
        public DbSet<EvernoteUser> EvernoteUsers { get; set; } //evernoteUser ı kullanmak için referans olarak entities katmanı eklendi.
        public DbSet<Note> Notes { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Liked> Likes { get; set; }

    }
}
