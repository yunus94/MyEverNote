using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using MyEvernote.Entities;

namespace MyEvernote.DataAccessLayer.EntityFramework
{
  public class MyInitializer:CreateDatabaseIfNotExists<DatabaseContext>
    {
        protected override void Seed(DatabaseContext context)
        {
            //adding admin user
            EvernoteUser admin = new EvernoteUser()
            {
                Name="Yunus",
                Surname="ÖNAL",
                Email="yunusonal94@gmail.com",
                ActivateGuid=Guid.NewGuid(),
                IsActive=true,
                IsAdmin=true,
                Username="yunusonal",
                Password="123456",
                CreatedOn=DateTime.Now,
                ModifiedOn=DateTime.Now.AddMinutes(5),
                ModifiedUsername="yunusonal"
            };
            //adding standart user
            EvernoteUser standarUser = new EvernoteUser()
            {
                Name = "Ahmet",
                Surname = "AK",
                Email = "ahmetak@gmail.com",
                ActivateGuid = Guid.NewGuid(),
                IsActive = true,
                IsAdmin = false,
                Username = "ahmetak",
                Password = "123456",
                CreatedOn = DateTime.Now.AddHours(1),
                ModifiedOn = DateTime.Now.AddMinutes(65),
                ModifiedUsername = "ahmetak"
            };
            context.EvernoteUsers.Add(admin);
            context.EvernoteUsers.Add(standarUser);

            for (int i = 0; i < 8; i++)
            {
                EvernoteUser user = new EvernoteUser()
                {
                    Name = FakeData.NameData.GetFirstName(),
                    Surname = FakeData.NameData.GetSurname(),
                    Email = FakeData.NetworkData.GetEmail(),
                    ActivateGuid = Guid.NewGuid(),
                    IsActive = true,
                    IsAdmin = false,
                    Username = $"user{i}",  //string formating
                    Password = "123",
                    CreatedOn = FakeData.DateTimeData.GetDatetime(DateTime.Now.AddYears(-1), DateTime.Now),
                    ModifiedOn = FakeData.DateTimeData.GetDatetime(DateTime.Now.AddYears(-1), DateTime.Now),
                    ModifiedUsername = $"user{i}"
                };
                context.EvernoteUsers.Add(user);
            }

            context.SaveChanges();

            //user list for using
            List<EvernoteUser> userList = context.EvernoteUsers.ToList();//her seferinden sql den select çekmemesi için eklendi.

            //adding fake categories
            for (int i = 0; i < 10; i++)
            {
                Category category = new Category()
                {
                    Title = FakeData.PlaceData.GetStreetName(),
                    Description = FakeData.PlaceData.GetAddress(),
                    CreatedOn = DateTime.Now,
                    ModifiedOn = DateTime.Now,
                    ModifiedUsername = "yunusonal"
                };
                context.Categories.Add(category);

                //adding fake notes
                for (int k = 0; k < FakeData.NumberData.GetNumber(5,9); k++)
                {
                    EvernoteUser owner = userList[FakeData.NumberData.GetNumber(0, userList.Count - 1)];
                    Note note = new Note()
                    {
                        Title = FakeData.TextData.GetAlphabetical(FakeData.NumberData.GetNumber(5, 25)),
                        Text = FakeData.TextData.GetSentences(FakeData.NumberData.GetNumber(1, 3)),
                        //Category = category,  ,//aşağıda zaten category classına eklendiği için gerek duyulmadı
                        IsDraft = false,
                        LikeCount = FakeData.NumberData.GetNumber(1, 9),
                        Owner = owner,
                        CreatedOn = FakeData.DateTimeData.GetDatetime(DateTime.Now.AddYears(-1), DateTime.Now),
                        ModifiedOn = FakeData.DateTimeData.GetDatetime(DateTime.Now.AddYears(-1), DateTime.Now),
                        ModifiedUsername = owner.Username
                    };
                    category.Notes.Add(note);

                    //adding fake commets
                    for (int j = 0; j < FakeData.NumberData.GetNumber(3,5); j++)
                    {
                        EvernoteUser comment_owner = userList[FakeData.NumberData.GetNumber(0, userList.Count - 1)];
                        Comment comment = new Comment()
                        {
                            Text = FakeData.TextData.GetSentence(),
                            //Note=note,
                            Owner= comment_owner,
                            CreatedOn = FakeData.DateTimeData.GetDatetime(DateTime.Now.AddYears(-1), DateTime.Now),
                            ModifiedOn = FakeData.DateTimeData.GetDatetime(DateTime.Now.AddYears(-1), DateTime.Now),
                            ModifiedUsername = comment_owner.Username
                        };
                        note.Comments.Add(comment);
                    }
                    //adding fake likes
                    for (int m = 0; m < note.LikeCount; m++)
                    {
                        Liked liked = new Liked()
                        {
                            LikedUser =userList[m]
                        };
                        note.Likes.Add(liked);
                    }
                }
            }
            context.SaveChanges();
        }
    }
}
