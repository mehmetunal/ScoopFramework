using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Linq.Expressions;
using ScoopFramework.DataTables;
using ScoopFramework.Model;
using ScoopFramework.SqlOperactions;

namespace ScoopFramework.DataBussiens
{
    /// <summary>
    /// Represents a TBL_User.
    /// NOTE: This class is generated from a T4 template - you should not modify it manually.
    /// </summary>
    partial class ScoopManagement
    {
        /// <summary>
        ///  TBL_User Tablodan Tüm Dataları çeker  (gridin IDataTablesRequest gönderildiğinde şartlı veri çeker)
        /// </summary>
        /// <param name="requestModel"></param>
        /// <returns></returns>
        public List<TBL_User> GETTBL_User(IDataTablesRequest requestModel = null, DbTransaction tran = null)
        {
            using (var db = GetDB<TBL_User>(tran))
            {
                return db.Table().DataTablesFiltre(requestModel).RunToList();
            }
        }

        /// <summary>
        /// TBL_User Tablodan Şarta göre veri çeker ve parametre olarak Linq Where Expression alır
        /// </summary>
        /// <param name="where"></param>
        /// <param name="requestModel"></param>
        /// <returns></returns>
        public List<TBL_User> GETTBL_UserByWhere(Expression<Func<TBL_User, bool>> @where, IDataTablesRequest requestModel = null, DbTransaction tran = null)
        {
            using (var db = GetDB<TBL_User>(tran))
            {
                return db.Table().Where(where).OrderBy(x => x.created).DataTablesFiltre(requestModel).RunToList();
            }
        }

        /// <summary>
        /// TBL_User Tablosundaki verilerin sayısını verir
        /// </summary>
        /// <param name="requestModel"></param>
        /// <returns></returns>
        public int GETTBL_UserCount(IDataTablesRequest requestModel = null, DbTransaction tran = null)
        {
            using (var db = GetDB<TBL_User>(tran))
            {
                return db.Table().DataTablesFiltre(requestModel).Count().RunCount();
            }
        }

        /// <summary>
        /// TBL_User Tablodan Şarta göre veri çeker ve parametre olarak Linq Where Expression alır
        /// </summary>
        /// <param name="where"></param>
        /// <param name="requestModel"></param>
        /// <returns></returns>
        public int GETTBL_UserCount(Expression<Func<TBL_User, bool>> @where, IDataTablesRequest requestModel = null, DbTransaction tran = null)
        {
            using (var db = GetDB<TBL_User>(tran))
            {
                return db.Table().DataTablesFiltre(requestModel).Where(@where).Count().RunCount();
            }
        }

        /// <summary>
        /// TBL_User Tablodan Şarta göre veri çeker ve parametre olarak Linq Where Expression alır
        /// </summary>
        /// <param name="where"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public TBL_User GETTBL_UserById(Guid id, DbTransaction tran = null)
        {
            using (var db = GetDB<TBL_User>(tran))
            {
                return db.Table().Where(x => x.id == id).RunFirstOrDefault();
            }
        }

        /// <summary>
        /// TBL_User Tablodan  Şarta Göre En Son Eklenen Veriyi Çeker
        /// </summary>
        /// <param name="solid"></param>
        /// <returns></returns>
        public TBL_User GETTBL_UserByLastData(Expression<Func<TBL_User, object>> solid, DbTransaction tran = null)
        {
            using (var db = GetDB<TBL_User>(tran))
            {
                return db.Table().OrderByDesc(solid).RunFirstOrDefault();
            }
        }

        /// <summary>
        /// TBL_User Tablosundan  Verileri Küçükten büyüğe doğru sırları parametre olarak linq order by Expression alır
        /// </summary>
        /// <param name="solid"></param>
        /// <returns></returns>
        public List<TBL_User> GETTBL_UserByOrderByDesc(Expression<Func<TBL_User, object>> solid, DbTransaction tran = null)
        {
            using (var db = GetDB<TBL_User>(tran))
            {
                return db.Table().OrderByDesc(solid).RunToList();
            }
        }

        /// <summary>
        /// TBL_User Tablosundan Verileri büyükten küçüğe doğru sırları parametre olarak linq order by Expression alır
        /// </summary>
        /// <param name="solid"></param>
        /// <returns></returns>
        public List<TBL_User> GETTBL_UserByOrderBy(Expression<Func<TBL_User, object>> solid, DbTransaction tran = null)
        {
            using (var db = GetDB<TBL_User>(tran))
            {
                return db.Table().OrderBy(solid).RunToList();
            }
        }

        /// <summary>
        /// TBL_User tabloya kayıt atar
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public ReturnValue InsertTBL_User(TBL_User entity, DbTransaction tran = null)
        {
            using (var db = GetDB<TBL_User>(tran))
            {
                return db.Table().Insert(entity);
            }
        }

        /// <summary>
        /// TBL_User tabloya birden fazla kayıt atar
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public ReturnValue BulkInsertTBL_User(List<TBL_User> entity, DbTransaction tran = null)
        {
            using (var db = GetDB<TBL_User>(tran))
            {
                return db.Table().BulkInsert(entity);
            }
        }

        /// <summary>
        /// TBL_User primary key göre güncelleme işlemi yapar
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public ReturnValue UpdateTBL_User(TBL_User entity, DbTransaction tran = null)
        {
            using (var db = GetDB<TBL_User>(tran))
            {
                return db.Table().Update(entity);
            }
        }

        /// <summary>
        /// TBL_User tabloya update yapar bizim belirlediğimiz şarta göre
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        public ReturnValue UpdateTBL_User(TBL_User entity, Expression<Func<TBL_User, object>> @where, DbTransaction tran = null)
        {
            using (var db = GetDB<TBL_User>(tran))
            {
                return db.Table().Update(entity, @where);
            }
        }

        /// <summary>
        /// TBL_User birden fazla update yapar
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public ReturnValue BulkUpdateTBL_User(List<TBL_User> entity, DbTransaction tran = null)
        {
            using (var db = GetDB<TBL_User>(tran))
            {
                return db.Table().BulkUpdate(entity);
            }
        }

        /// <summary>
        /// TBL_User birden fazla kayıt atar şarta göre 
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        public ReturnValue BulkUpdateTBL_User(List<TBL_User> entity, Expression<Func<TBL_User, object>> @where, DbTransaction tran = null)
        {
            using (var db = GetDB<TBL_User>(tran))
            {
                return db.Table().BulkUpdate(entity, @where);
            }
        }

        /// <summary>
        /// TBL_User tabloya daki kayıtı siler primary key'e göre
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public ReturnValue DeleteTBL_User(TBL_User entity, DbTransaction tran = null)
        {
            using (var db = GetDB<TBL_User>(tran))
            {
                return db.Table().Delete(entity);
            }
        }

        /// <summary>
        /// TBL_User tabloya daki kayıt siler şarta göre
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        public ReturnValue DeleteTBL_User(TBL_User entity, Expression<Func<TBL_User, object>> @where, DbTransaction tran = null)
        {
            using (var db = GetDB<TBL_User>(tran))
            {
                return db.Table().Delete(entity, @where);
            }
        }

        /// <summary>
        /// TBL_User tabloya daki kayıtları siler primary keye göre
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public ReturnValue BulkDeleteTBL_User(List<TBL_User> entity, DbTransaction tran = null)
        {
            using (var db = GetDB<TBL_User>(tran))
            {
                return db.Table().BulkDelete(entity);
            }
        }

        /// <summary>
        /// TBL_User tabloya daki kayıtları siler şarta göre
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        public ReturnValue BulkDeleteTBL_User(List<TBL_User> entity, Expression<Func<TBL_User, object>> @where, DbTransaction tran = null)
        {
            using (var db = GetDB<TBL_User>(tran))
            {
                return db.Table().BulkDelete(entity, @where);
            }
        }
    }
}  
