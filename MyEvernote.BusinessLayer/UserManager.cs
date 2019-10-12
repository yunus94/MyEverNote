using MyEvernote.DataAccessLayer.EntityFramework;
using MyEvernote.Entities;
using MyEvernote.Entities.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyEvernote.BusinessLayer
{
    public class UserManager
    {
        private Repository<EvernoteUser> repo_user = new Repository<EvernoteUser>();
        public BusinessLayerResult<EvernoteUser> RegisterUser(RegisterViewModel data)
        {
            EvernoteUser user = repo_user.Find(x => x.Username == data.Username || x.Email == data.Email);
            BusinessLayerResult<EvernoteUser> res = new BusinessLayerResult<EvernoteUser>();
            if (user != null)
            {
                if (user.Username == data.Username)
                {
                    res.Errors.Add("Kullanıcı adı var.");
                }
                if (user.Email == data.Email)
                {
                    res.Errors.Add("E-posta adresi var");
                }
            }
            else
            {
                int dbResult = repo_user.Insert(new EvernoteUser()
                {
                    Username = data.Username,
                    Email = data.Email,
                    Password = data.Password,
                    ActivateGuid = Guid.NewGuid(),
                    IsActive = false,
                    IsAdmin = false

                });
                if (dbResult > 0)
                {
                    res.Result = repo_user.Find(x => x.Email == data.Email && x.Username == data.Username);

                    //layerResult.Result.ActivateGuid
                }
            }
            return res;
        }
        public BusinessLayerResult<EvernoteUser> LoginUser(LoginViewModel data)
        {
            BusinessLayerResult<EvernoteUser> res = new BusinessLayerResult<EvernoteUser>();
            res.Result = repo_user.Find(x => x.Username == data.Username && x.Password == data.Password);

            if (res.Result!=null)
            {
                if (!res.Result.IsActive)
                {
                    res.Errors.Add("Kullanıcı aktif olmamıştır. Lütfen e-posta adresinizi kontrol ediniz.");
                }
            }
            else
            {
                res.Errors.Add("Kullanıcı adı veya şifre uyuşmuyor.");
            }
            return res;
        }
    }
}
