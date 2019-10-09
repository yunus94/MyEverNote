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
        public Test()
       {
            //DataAccessLayer.DatabaseContext db = new DataAccessLayer.DatabaseContext();
            //db.Categories.ToList();

            
            List<Category> categories= repo_category.List();
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
               int result= repo_user.Save();
            }
        }

    }
}


