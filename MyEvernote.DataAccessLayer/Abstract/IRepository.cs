﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MyEvernote.DataAccessLayer.Abstract
{
   public interface IRepository<T>
    {
         List<T> List();
        IQueryable<T> ListQueryable();
         List<T> List(Expression<Func<T, bool>> where);//istenilen bir kritere göre listeleme yapılıyor.

         int Insert(T obj);

         int Update(T obj);

         int Delete(T obj);

         int Save();

         T Find(Expression<Func<T, bool>> where); //tek tip kayıt döner . Verilen koşula uygun veriyi döner.
       
    }
}
