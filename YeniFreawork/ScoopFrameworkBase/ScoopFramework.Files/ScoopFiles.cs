using ScoopFramework.Files.Model;
using ScoopFramework.Helper;
using ScoopFramework.Model;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Configuration;

namespace ScoopFramework.Files
{
    public static class ScoopFiles
    {
        public static List<string> GetFoldersName(string path)
        {
            return Directory.GetDirectories(path).Select(d => new DirectoryInfo(d).Name).ToList();
        }
        public static ReturnFiles FileImport(string folderPath, string filePath, HttpPostedFileBase file)
        {

            try
            {
                if (string.IsNullOrEmpty(folderPath))
                {
                    return new ReturnFiles() { Success = false, FileName = folderPath, Message = "Hata:Böyle Bir Klasör Bulunamadı" };
                }
                if (string.IsNullOrEmpty(filePath))
                {
                    return new ReturnFiles() { Success = false, FileName = filePath, Message = "Hata:Böyle Bir Dosya Yolu Bulunamadı" };
                }


                if (!Directory.Exists(filePath))
                {
                    Directory.CreateDirectory(filePath);
                    file.SaveAs(Path.Combine(HttpContext.Current.Request.MapPath(folderPath + filePath),
                        Path.GetFileName(DateTime.Now.ToString("s") + "_" + file.FileName)));

                }

            }
            catch (System.Exception ex)
            {
                return new ReturnFiles() { Success = false, FileName = filePath, Message = ex.Message };
            }

            return new ReturnFiles() { Success = true, FileName = filePath, Message = "Dosya aktarım işlemi başarılı." };
        }
        public static ReturnFiles FileDelete(string folderPath, string filePath, string fileName)
        {

            try
            {
                if (string.IsNullOrEmpty(folderPath))
                {

                    return new ReturnFiles() { Success = false, FileName = folderPath, Message = "Hata:Böyle Bir Klasör Bulunamadı" };

                }

                if (string.IsNullOrEmpty(filePath))
                {

                    return new ReturnFiles() { Success = false, FileName = filePath, Message = "Hata:Böyle Bir Dosya Yolu Bulunamadı" };

                }


                if (!Directory.Exists(filePath))
                {

                    var dFiled = Path.Combine(HttpContext.Current.Request.MapPath(folderPath + filePath), fileName);
                    Directory.Delete(dFiled, true);

                }

            }
            catch (System.Exception ex)
            {

                return new ReturnFiles() { Success = false, FileName = filePath, Message = ex.Message };

            }

            return new ReturnFiles() { Success = true, FileName = filePath, Message = "Dosya silme işlemi başarılı." };

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="file"></param>
        /// <param name="filePath">Dosyanın Kayıt Edileceği yer</param>
        /// <param name="bigWidth"></param>
        /// <param name="bigHeight"></param>
        /// <param name="smallWidth"></param>
        /// <param name="smallHeight"></param>
        /// <param name="fileName">fileName göndermezseniz dosyanın adı (DateTime.Now.ToString("s") + "_" + file.FileName) nu şekilde olacaktır</param>
        /// <param name="customFileName"></param>
        /// <returns></returns>
        public static ReturnFiles FileImagePathImports(HttpPostedFileBase file, string filePath, int bigMaxSideSize = 730, int smallMaxSideSize = 150, string fileName = "", bool customFileName = false)
        {
            try
            {
                if (file == null)
                    return new ReturnFiles() { Success = false, FileName = fileName, Message = "Dosya aktarım işlemi başarısız Sorun:(file parametresi boş gönderilmiştir)." };

                var bigP = filePath + "/Big";
                var smallP = filePath + "/small";

                if (string.IsNullOrEmpty(fileName))
                {
                    fileName = DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss") + "_" + file.FileName;
                }
                else
                {
                    if (customFileName == false)
                    {
                        fileName = fileName + "_" + file.FileName;
                    }

                    var fileSuccess = Path.Combine(HttpContext.Current.Server.MapPath(bigP), fileName);

                    if (Directory.Exists(fileSuccess))
                    {
                        fileName = DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss") + "_" + fileName + "_" + file.FileName;
                    }
                }

                if (!Directory.Exists(HttpContext.Current.Server.MapPath(filePath)))
                {
                    Directory.CreateDirectory($"{HttpContext.Current.Server.MapPath(filePath)}");
                }
                if (!Directory.Exists(HttpContext.Current.Server.MapPath(bigP)))
                {
                    Directory.CreateDirectory($"{HttpContext.Current.Server.MapPath(bigP)}");
                }
                if (!Directory.Exists(HttpContext.Current.Server.MapPath(smallP)))
                {
                    Directory.CreateDirectory($"{HttpContext.Current.Server.MapPath(smallP)}");
                }

                if (filePath == null)
                    return new ReturnFiles() { Success = false, FileName = fileName, Message = "Dosya aktarım işlemi başarısız Sorun:(filePath parametresi boş gönderilmiştir)." };

                var pathBig = Path.Combine(HttpContext.Current.Server.MapPath(bigP));
                var pathSmall = Path.Combine(HttpContext.Current.Server.MapPath(smallP));

                var bigSuccess = ResizeAndSave(pathBig, fileName, file.InputStream, bigMaxSideSize, false);
                var smallSuccess = ResizeAndSave(pathSmall, fileName, file.InputStream, smallMaxSideSize, false);

                return new ReturnFiles() { Success = (Convert.ToBoolean(bigSuccess.Keys.FirstOrDefault()) && Convert.ToBoolean(smallSuccess.Keys.FirstOrDefault())), FileName = fileName, BigPath = pathBig, SmallPath = pathSmall, Message = "Dosya aktarım işlemi başarılı." };
            }
            catch (System.Exception ex)
            {
                return new ReturnFiles() { Success = true, FileName = fileName, Message = $"HATA:{ex.Message}" };
            }
        }
        private static Dictionary<bool, string> ResizeAndSave(string savePath, string fileName, Stream imageBuffer, int maxSideSize, bool makeItSquare)
        {
            try
            {
                int newWidth;
                int newHeight;
                Image image = Image.FromStream(imageBuffer);
                int oldWidth = image.Width;
                int oldHeight = image.Height;
                Bitmap newImage;
                if (makeItSquare)
                {
                    int smallerSide = oldWidth >= oldHeight ? oldHeight : oldWidth;
                    double coeficient = maxSideSize / (double)smallerSide;
                    newWidth = Convert.ToInt32(coeficient * oldWidth);
                    newHeight = Convert.ToInt32(coeficient * oldHeight);
                    Bitmap tempImage = new Bitmap(image, newWidth, newHeight);
                    int cropX = (newWidth - maxSideSize) / 2;
                    int cropY = (newHeight - maxSideSize) / 2;
                    newImage = new Bitmap(maxSideSize, maxSideSize);
                    Graphics tempGraphic = Graphics.FromImage(newImage);
                    tempGraphic.SmoothingMode = SmoothingMode.AntiAlias;
                    tempGraphic.InterpolationMode = InterpolationMode.HighQualityBicubic;
                    tempGraphic.PixelOffsetMode = PixelOffsetMode.HighQuality;
                    tempGraphic.DrawImage(tempImage, new Rectangle(0, 0, maxSideSize, maxSideSize), cropX, cropY, maxSideSize, maxSideSize, GraphicsUnit.Pixel);
                }
                else
                {
                    int maxSide = oldWidth >= oldHeight ? oldWidth : oldHeight;

                    if (maxSide > maxSideSize)
                    {
                        double coeficient = maxSideSize / (double)maxSide;
                        newWidth = Convert.ToInt32(coeficient * oldWidth);
                        newHeight = Convert.ToInt32(coeficient * oldHeight);
                    }
                    else
                    {
                        newWidth = oldWidth;
                        newHeight = oldHeight;
                    }
                    newImage = new Bitmap(image, newWidth, newHeight);
                }
                newImage.Save(savePath + "\\" + fileName);
                image.Dispose();
                newImage.Dispose();
                return new Dictionary<bool, string>() { { true, "İşlem Başarılı" } };
            }
            catch (System.Exception ex)
            {
                return new Dictionary<bool, string>() { { false, ex.Message } };
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="filePath">Silmek istediğin dosyanın yolu</param>
        /// <param name="recursive">Silmek istediğin klasörün içindeki verileride silmek istiyormusunuz?</param>
        /// <returns></returns>
        public static ReturnFiles FileImageDelete(string filePath, string fileName, bool recursive = true)
        {
            try
            {
                var bigP = HttpContext.Current.Server.MapPath(String.Format("{0}/Big/{1}", filePath, fileName));
                var smallP = HttpContext.Current.Server.MapPath(String.Format("{0}/small/{1}", filePath, fileName));

                if (filePath == null)
                    return new ReturnFiles() { Success = false, Message = "Dosya silme işlemi başarısız Sorun:(filePath parametresi boş gönderilmiştir)." };

                Directory.Delete(bigP, true);
                Directory.Delete(smallP, true);

                return new ReturnFiles() { Success = true, Message = "Dosya silme işlemi başarılı." };
            }
            catch (System.Exception ex)
            {
                return new ReturnFiles() { Success = true, Message = $"HATA:{ex.Message}" };
            }
        }
        public static ReturnValue GetDBScript(string tables = null)
        {
            var Tables = tables == null ? null : tables.Split(',');
            var dbs = new DBScripter(null, WebConfigurationManager.AppSettings["DBConnection"]);
            return dbs.GenerateTableScript(Tables);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="files"></param>
        /// <param name="filePath"></param>
        /// <param name="size">new Size(100, 100)</param>
        /// <param name="categoriFile"></param>
        /// <returns></returns>
        public static List<string> ImagesFilesCreate(IEnumerable<HttpPostedFileBase> files, string filePath, Size? size, string title = "", string categoriFile = null, bool watermark = false, string watermarkText = "ScoopFramework")
        {
            List<string> liste = new List<string>();

            try
            {
                if (!Directory.Exists(filePath))
                    Directory.CreateDirectory(filePath);

                if (!string.IsNullOrEmpty(categoriFile))
                {
                    categoriFile = categoriFile.ToSeoUrl();
                    if (!Directory.Exists(filePath + "/" + categoriFile))
                        Directory.CreateDirectory(filePath + "/" + categoriFile);
                    else
                        filePath = filePath + "/" + categoriFile;
                }

                foreach (var file in files)
                {
                    if (file.ContentLength == 0) continue;
                    var extension = Path.GetExtension(file.FileName);
                    var fileName = Path.GetFileName(file.FileName);
                    if (extension == null) continue;
                    extension = extension.ToLower();
                    if (fileName == null) continue;

                    var tarih = DateTime.Now.ToString("yyyy-MM-dd-HH", new CultureInfo("tr")).Replace(" ", "").Replace(",", "").Replace(":", "").Replace(".", "").Replace("/", "");

                    if (!string.IsNullOrEmpty(title))
                    {
                        fileName = tarih + "_" + title.ToSeoUrl() + "_" + fileName.Replace(extension, "").ToSeoUrl().Replace(" ", "").Replace("--", "-").Replace("__", "-").ToLower();
                    }
                    else
                    {
                        fileName = tarih + "_" + fileName.Replace(extension, "").ToSeoUrl().Replace(" ", "").Replace("--", "-").Replace("__", "-").ToLower();
                    }
                    liste.Add($"{fileName}{extension}");
                    using (var img = Image.FromStream(file.InputStream))
                    {
                        if (size == null)
                            size = img.Size;
                        else
                            fileName = $"{size.Value.Width}X{size.Value.Height}_{fileName}";


                        SaveToFolder(img, size.Value, $"{filePath}/{fileName}{extension}", watermark, watermarkText);
                    }
                }
            }
            catch
            {
            }

            return liste;
        }
        public static bool ImagesCreate(string openFile, string saveFile, string fileName, Size? size, string categoriFile = null, bool watermark = false, string watermarkText = "ScoopFramework")
        {
            try
            {
                if (!Directory.Exists(saveFile))
                    Directory.CreateDirectory(saveFile);

                if (!string.IsNullOrEmpty(categoriFile))
                {
                    categoriFile = categoriFile.ToSeoUrl();
                    if (!Directory.Exists(saveFile + "/" + categoriFile))
                        Directory.CreateDirectory(saveFile + "/" + categoriFile);

                    saveFile = saveFile + "/" + categoriFile;
                }

                var file = string.Format("{0}{1}", openFile, fileName);

                using (var img = Image.FromFile(file))
                {

                    string extension = Path.GetExtension(fileName);
                    fileName = fileName.Replace(extension, "");

                    if (size == null)
                        size = img.Size;
                    else
                        fileName = $"{size.Value.Width}X{size.Value.Height}_{fileName}";

                    SaveToFolder(img, size.Value, $"{saveFile}/{fileName}{extension}", watermark, watermarkText);
                }
            }
            catch
            {
                return false;
            }

            return true;
        }
        private static void SaveToFolder(Image img, Size newSize, string pathToSave, bool watermark = false, string watermarkText = "ScoopFramework")
        {
            var imgSize = NewImageSize(img.Size, newSize);

            using (Font font = new Font("Arial", 22, FontStyle.Regular))
            using (SolidBrush brush = new SolidBrush(Color.FromArgb(128, 255, 255, 255)))
            using (var newImg = new Bitmap(img, imgSize.Width, imgSize.Height))
            {
                if (watermark)
                {
                    Graphics graphics = Graphics.FromImage(newImg);

                    graphics.SmoothingMode = SmoothingMode.AntiAlias;


                    SizeF measuredSize = graphics.MeasureString(watermarkText, font);

                    graphics.DrawString(watermarkText, font, brush, newImg.Width - measuredSize.Width, newImg.Height - measuredSize.Height);

                    graphics.Dispose();
                }

                newImg.Save(pathToSave, img.RawFormat);
            }
        }
        public static Size NewImageSize(Size imageSize, Size newSize)
        {
            Size finalSize;
            if (imageSize.Height > newSize.Height || imageSize.Width > newSize.Width)
            {
                double tempval;
                if (imageSize.Height > imageSize.Width)
                {
                    tempval = newSize.Height / (imageSize.Height * 1.0);
                }
                else
                {
                    tempval = newSize.Width / (imageSize.Width * 1.0);
                }

                finalSize = new Size((int)(tempval * imageSize.Width), (int)(tempval * imageSize.Height));

            }
            else
            {
                finalSize = imageSize;
            }

            return finalSize;
        }

    }

}


