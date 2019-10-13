using MyEvernote.Common;
using MyEvernote.DataAccessLayer;
using MyEvernote.DataAccessLayer.Abstract;
using MyEvernote.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MyEvernote.DataAccessLayer.EntityFramework
{
   public class Repository<T> :RepositoryBase, IRepository<T> where T:class //farklı türden verileri önlemek için Ör:int
    {
        private DbSet<T> _objectSet;

        public Repository() //performans açısından dbset i bu class da yaratıp , aşağıdaki classlarda kullandım.
        {
            _objectSet = context.Set<T>();  // db set: o an hangi class çağrılıyorsa o class ı alır. Ör Category,Note...
        }
        public List<T> List()
        {
           return /*db.Set<T>()*/ _objectSet.ToList();
        }
        public IQueryable<T> ListQueryable()
        {
            return _objectSet.AsQueryable<T>();
        }
        public List<T> List(Expression<Func<T,bool>> where)//istenilen bir kritere göre listeleme yapılıyor.
        {
            return _objectSet.Where(where).ToList();
        }

        //Kullanıcı veya admin order by ekleyebilir. İlk 3 kaydı getir diyebilir . Bu şekilde kullanmak istenirse aşağıdaki kod kullanılabilir.
        //public IQueryable<T> List(Expression<Func<T, bool>> where)
        //{
        //    return _objectSet.Where(where);
        //}


        public int Insert(T obj)
        {
            /*db.Set<T>()*/ _objectSet.Add(obj);
            if (obj is MyEntityBase)
            {
                MyEntityBase o = obj as MyEntityBase;
                DateTime now = DateTime.Now;
                o.CreatedOn = now;
                o.ModifiedOn = now;
                o.ModifiedUsername = App.Common.GetCurrentUsername();
            }
            return Save();
        }
        public int Update(T obj)
        {
            if (obj is MyEntityBase)
            {
                MyEntityBase o = obj as MyEntityBase;
                o.ModifiedOn =DateTime.Now;
                o.ModifiedUsername = App.Common.GetCurrentUsername();
            }
            return Save();
        }
        public int Delete(T obj)
        {
            //if (obj is MyEntityBase)
            //{
            //    MyEntityBase o = obj as MyEntityBase;
            //    o.ModifiedOn = DateTime.Now;
            //    o.ModifiedUsername = App.Common.GetUsername();
            //}
            _objectSet.Remove(obj);
            return Save();
        }
        public int Save()
        {
            return context.SaveChanges();
        }
        public T Find(Expression<Func<T,bool>> where) //tek tip kayıt döner . Verilen koşula uygun veriyi döner.
        {
            return _objectSet.FirstOrDefault(where);
        }
    }
}
