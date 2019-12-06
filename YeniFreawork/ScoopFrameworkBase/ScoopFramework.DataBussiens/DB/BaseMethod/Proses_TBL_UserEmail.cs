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
    /// Represents a TBL_UserEmail.
    /// NOTE: This class is generated from a T4 template - you should not modify it manually.
    /// </summary>
    partial class ScoopManagement
    {
        /// <summary>
        ///  TBL_UserEmail Tablodan Tüm Dataları çeker  (gridin IDataTablesRequest gönderildiğinde şartlı veri çeker)
        /// </summary>
        /// <param name="requestModel"></param>
        /// <returns></returns>
        public List<TBL_UserEmail> GETTBL_UserEmail(IDataTablesRequest requestModel = null, DbTransaction tran = null)
        {
            using (var db = GetDB<TBL_UserEmail>(tran))
            {
                return db.Table().OrderBy(x => x.created).DataTablesFiltre(requestModel).RunToList();
            }
        }

        /// <summary>
        /// TBL_UserEmail Tablodan Şarta göre veri çeker ve parametre olarak Linq Where Expression alır
        /// </summary>
        /// <param name="where"></param>
        /// <param name="requestModel"></param>
        /// <returns></returns>
        public List<TBL_UserEmail> GETTBL_UserEmailByWhere(Expression<Func<TBL_UserEmail, bool>> @where, IDataTablesRequest requestModel = null, DbTransaction tran = null)
        {
            using (var db = GetDB<TBL_UserEmail>(tran))
            {
                return db.Table().Where(where).OrderBy(x => x.created).DataTablesFiltre(requestModel).RunToList();
            }
        }

        /// <summary>
        /// TBL_UserEmail Tablosundaki verilerin sayısını verir
        /// </summary>
        /// <param name="requestModel"></param>
        /// <returns></returns>
        public int GETTBL_UserEmailCount(IDataTablesRequest requestModel = null, DbTransaction tran = null)
        {
            using (var db = GetDB<TBL_UserEmail>(tran))
            {
                return db.Table().DataTablesFiltre(requestModel).Count().RunCount();
            }
        }

        /// <summary>
        /// TBL_UserEmail Tablodan Şarta göre veri çeker ve parametre olarak Linq Where Expression alır
        /// </summary>
        /// <param name="where"></param>
        /// <param name="requestModel"></param>
        /// <returns></returns>
        public int GETTBL_UserEmailCount(Expression<Func<TBL_UserEmail, bool>> @where, IDataTablesRequest requestModel = null, DbTransaction tran = null)
        {
            using (var db = GetDB<TBL_UserEmail>(tran))
            {
                return db.Table().DataTablesFiltre(requestModel).Where(@where).Count().RunCount();
            }
        }

        /// <summary>
        /// TBL_UserEmail Tablodan Şarta göre veri çeker ve parametre olarak Linq Where Expression alır
        /// </summary>
        /// <param name="where"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public TBL_UserEmail GETTBL_UserEmailById(Guid id, DbTransaction tran = null)
        {
            using (var db = GetDB<TBL_UserEmail>(tran))
            {
                return db.Table().Where(x => x.id == id).RunFirstOrDefault();
            }
        }

        /// <summary>
        /// TBL_UserEmail Tablodan  Şarta Göre En Son Eklenen Veriyi Çeker
        /// </summary>
        /// <param name="solid"></param>
        /// <returns></returns>
        public TBL_UserEmail GETTBL_UserEmailByLastData(Expression<Func<TBL_UserEmail, object>> solid, DbTransaction tran = null)
        {
            using (var db = GetDB<TBL_UserEmail>(tran))
            {
                return db.Table().OrderByDesc(solid).RunFirstOrDefault();
            }
        }

        /// <summary>
        /// TBL_UserEmail Tablosundan  Verileri Küçükten büyüğe doğru sırları parametre olarak linq order by Expression alır
        /// </summary>
        /// <param name="solid"></param>
        /// <returns></returns>
        public List<TBL_UserEmail> GETTBL_UserEmailByOrderByDesc(Expression<Func<TBL_UserEmail, object>> solid, DbTransaction tran = null)
        {
            using (var db = GetDB<TBL_UserEmail>(tran))
            {
                return db.Table().OrderByDesc(solid).RunToList();
            }
        }

        /// <summary>
        /// TBL_UserEmail Tablosundan Verileri büyükten küçüğe doğru sırları parametre olarak linq order by Expression alır
        /// </summary>
        /// <param name="solid"></param>
        /// <returns></returns>
        public List<TBL_UserEmail> GETTBL_UserEmailByOrderBy(Expression<Func<TBL_UserEmail, object>> solid, DbTransaction tran = null)
        {
            using (var db = GetDB<TBL_UserEmail>(tran))
            {
                return db.Table().OrderBy(solid).RunToList();
            }
        }

        /// <summary>
        /// TBL_UserEmail tabloya kayıt atar
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public ReturnValue InsertTBL_UserEmail(TBL_UserEmail entity, DbTransaction tran = null)
        {
            using (var db = GetDB<TBL_UserEmail>(tran))
            {
                return db.Table().Insert(entity);
            }
        }

        /// <summary>
        /// TBL_UserEmail tabloya birden fazla kayıt atar
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public ReturnValue BulkInsertTBL_UserEmail(List<TBL_UserEmail> entity, DbTransaction tran = null)
        {
            using (var db = GetDB<TBL_UserEmail>(tran))
            {
                return db.Table().BulkInsert(entity);
            }
        }

        /// <summary>
        /// TBL_UserEmail primary key göre güncelleme işlemi yapar
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public ReturnValue UpdateTBL_UserEmail(TBL_UserEmail entity, DbTransaction tran = null)
        {
            using (var db = GetDB<TBL_UserEmail>(tran))
            {
                return db.Table().Update(entity);
            }
        }

        /// <summary>
        /// TBL_UserEmail tabloya update yapar bizim belirlediğimiz şarta göre
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        public ReturnValue UpdateTBL_UserEmail(TBL_UserEmail entity, Expression<Func<TBL_UserEmail, object>> @where, DbTransaction tran = null)
        {
            using (var db = GetDB<TBL_UserEmail>(tran))
            {
                return db.Table().Update(entity, @where);
            }
        }

        /// <summary>
        /// TBL_UserEmail birden fazla update yapar
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public ReturnValue BulkUpdateTBL_UserEmail(List<TBL_UserEmail> entity, DbTransaction tran = null)
        {
            using (var db = GetDB<TBL_UserEmail>(tran))
            {
                return db.Table().BulkUpdate(entity);
            }
        }

        /// <summary>
        /// TBL_UserEmail birden fazla kayıt atar şarta göre 
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        public ReturnValue BulkUpdateTBL_UserEmail(List<TBL_UserEmail> entity, Expression<Func<TBL_UserEmail, object>> @where, DbTransaction tran = null)
        {
            using (var db = GetDB<TBL_UserEmail>(tran))
            {
                return db.Table().BulkUpdate(entity, @where);
            }
        }

        /// <summary>
        /// TBL_UserEmail tabloya daki kayıtı siler primary key'e göre
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public ReturnValue DeleteTBL_UserEmail(TBL_UserEmail entity, DbTransaction tran = null)
        {
            using (var db = GetDB<TBL_UserEmail>(tran))
            {
                return db.Table().Delete(entity);
            }
        }

        /// <summary>
        /// TBL_UserEmail tabloya daki kayıt siler şarta göre
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        public ReturnValue DeleteTBL_UserEmail(TBL_UserEmail entity, Expression<Func<TBL_UserEmail, object>> @where, DbTransaction tran = null)
        {
            using (var db = GetDB<TBL_UserEmail>(tran))
            {
                return db.Table().Delete(entity, @where);
            }
        }

        /// <summary>
        /// TBL_UserEmail tabloya daki kayıtları siler primary keye göre
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public ReturnValue BulkDeleteTBL_UserEmail(List<TBL_UserEmail> entity, DbTransaction tran = null)
        {
            using (var db = GetDB<TBL_UserEmail>(tran))
            {
                return db.Table().BulkDelete(entity);
            }
        }

        /// <summary>
        /// TBL_UserEmail tabloya daki kayıtları siler şarta göre
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        public ReturnValue BulkDeleteTBL_UserEmail(List<TBL_UserEmail> entity, Expression<Func<TBL_UserEmail, object>> @where, DbTransaction tran = null)
        {
            using (var db = GetDB<TBL_UserEmail>(tran))
            {
                return db.Table().BulkDelete(entity, @where);
            }
        }
    }
}  
