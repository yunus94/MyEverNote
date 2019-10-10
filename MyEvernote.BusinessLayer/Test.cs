using MyEvernote.DataAccessLayer.EntityFramework;
using MyEvernote.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyEvernote.BusinessLayer
{
    public class Test
    {
        private Repository<EvernoteUser> repo_user = new Repository<EvernoteUser>();
        private Repository<Category> repo_category = new Repository<Category>();
        private Repository<Comment> repo_comment = new Repository<Comment>();
        private Repository<Note> repo_note = new Repository<Note>();

        public Test()
       {
            //DataAccessLayer.DatabaseContext db = new DataAccessLayer.DatabaseContext();
            //db.Categories.ToList();

            List<Category> categories = repo_category.List();
            //List<Category> categories_filtered= repo_category.List(x=>x.Id>5);
        }
        public void InsertTest()
        {
            int result = repo_user.Insert(new EvernoteUser()
            {
                Name =    "aaa",
                Surname = "bbb",
                Email =   "aabb@gmail.com",
                ActivateGuid = Guid.NewGuid(),
                IsActive = true,
                IsAdmin = false,
                Username = "asdad",  
                Password = "123",
                CreatedOn = DateTime.Now,
                ModifiedOn = DateTime.Now.AddMinutes(5),
                ModifiedUsername = "abab"
            });
        }
        public void UpdateTest()
        {
            EvernoteUser user = repo_user.Find(x => x.Username == "asdad");
            if (user!=null)
            {
                user.Username = "xxx";
               int result= repo_user.Update(user);
            }
        }
        public void DeleteTest()
        {
            EvernoteUser user=repo_user.Find(x => x.Username == "xxx");
            if (user!=null)
            {
               int result= repo_user.Delete(user);
            }
        }
        public void CommentTest()
        {
            EvernoteUser user = repo_user.Find(x => x.Id == 1);
            Note note = repo_note.Find(x => x.Id == 3);

            Comment comment = new Comment()
            {
                Text = "Bu bir test denemesi",
                CreatedOn = DateTime.Now,
                ModifiedOn = DateTime.Now,
                ModifiedUsername = "yyunus",
                Note = null,
                Owner = null
            };
            repo_comment.Insert(comment);
        }

    }
}


