// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FileStreamAccess.cs" company="">
//   
// </copyright>
// <summary>
//   The file stream access.
// </summary>
// --------------------------------------------------------------------------------------------------------------------


using System;
using ScoopFramework.FileStream.Interface;

namespace ScoopFramework.FileStream.File
{
    /// <summary>
    /// The file stream access.
    /// </summary>
    public class FileStreamAccess : IFilesStream
    {
        /// <summary>
        /// The file stream delete.
        /// </summary>
        /// <param name="path">
        /// The path.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public object FileStreamDelete(string path)
        {
            try
            {
                var mapPath = System.Web.HttpContext.Current.Server.MapPath(path);
                if (System.IO.File.Exists(mapPath))
                {
                    System.IO.File.Delete(mapPath);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("Dosya silme işlemi yapılamadı. HATA KODU : {0}", ex.Message));
            }
            return "Dosya silme işlemi başarı ile tamamlanmıştır.";
        }

        public bool FileStreamSearch(string path)
        {
            try
            {
                var mapPath = System.Web.HttpContext.Current.Server.MapPath(path);
                if (System.IO.File.Exists(mapPath))
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return false;
        }

    }
}
