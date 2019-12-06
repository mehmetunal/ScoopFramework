using ScoopFramework.DataBussiens;
using ScoopFramework.Files;
using ScoopFramework.Model;
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using System.Drawing;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KodTest.Controllers
{
    /*Yapılacaklar     
     LikeFirst,
     LikeIn,
     LikeLast,
     Between,
     GetDataByIndex
         
    */
    public class HomeController : Controller
    {
        //public Size NewImageSize(Size imageSize, Size newSize)
        //{
        //    Size finalSize;
        //    double tempval;
        //    if (imageSize.Height > newSize.Height || imageSize.Width > newSize.Width)
        //    {
        //        if (imageSize.Height > imageSize.Width)
        //            tempval = newSize.Height / (imageSize.Height * 1.0);
        //        else
        //            tempval = newSize.Width / (imageSize.Width * 1.0);

        //        finalSize = new Size((int)(tempval * imageSize.Width), (int)(tempval * imageSize.Height));
        //    }
        //    else
        //        finalSize = imageSize; // image is already small size

        //    return finalSize;
        //}
        //private void SaveToFolder(Image img, string fileName, string extension, Size newSize, string pathToSave)
        //{
        //    // Get new resolution
        //    Size imgSize = NewImageSize(img.Size, newSize);
        //    using (System.Drawing.Image newImg = new Bitmap(img, imgSize.Width, imgSize.Height))
        //    {
        //        newImg.Save(Server.MapPath(pathToSave), img.RawFormat);
        //    }
        //}
        //[HttpPost]
        //public ActionResult Create( IEnumerable<HttpPostedFileBase> files)
        //{
        //    foreach (var file in files)
        //    {
        //        if (file.ContentLength == 0) continue;

        //        var fileName = Guid.NewGuid().ToString();
        //        var extension = System.IO.Path.GetExtension(file.FileName).ToLower();

        //        using (var img = System.Drawing.Image.FromStream(file.InputStream))
        //        {
        //           // model.ThumbPath = String.Format("/GalleryImages/thumbs/{0}{1}", fileName, extension);
        //           // model.ImagePath = String.Format("/GalleryImages/{0}{1}", fileName, extension);

        //            // Save thumbnail size image, 100 x 100
        //            SaveToFolder(img, fileName, extension, new Size(100, 100), model.ThumbPath);

        //            // Save large size image, 800 x 800
        //            SaveToFolder(img, fileName, extension, new Size(600, 600), model.ImagePath);
        //        }

        //        // Save record to database

        //    }

        //    return RedirectPermanent("/home");
        //}
        // GET: Home
        public ActionResult Index()
        {
            #region Kod
            /*SELECT column FROM table LIMIT {someLimit} OFFSET {someOffset};*/

            //var asd = new ScoopMySql<famous_people>().Where(x => x.id == 18449).RunToList();
            //var asd = new ScoopMySql<famous_people>().Paggin(10, 20).RunToList();
            //var asd = new ScoopMySql<famous_people>().Paggin(1, 100).OrderByDesc(x => x.id).RunToList();
            //var a = new ScoopMySql<famous_people>().ExecuteReader("SELECT * FROM VW_famous_people WHERE id IN", new MySqlParameter[0]).ToList();

            //FeedBack feedBack = new FeedBack();
            //var asd = feedBack.Success("başarılı", false, Url.Action("Index", "Home"));

            //var guids =
            //    new[]
            //    {
            //        new Guid("85F2339A-3063-452D-A86B-76190175780C"), new Guid("E4D1A900-7C16-4178-A6FE-9DC89A0FCEEF"),
            //        new Guid("6BA12E31-6624-4937-9424-BA12474E5C31"), new Guid("5DCE82FE-2DB5-498A-B550-C0BB541D264C")
            //    }
            //        .ToList();

            //var id = new ScoopLinq<Users>().ExecuteReader("SELECT * FROM Users WHERE id IN (" +
            //                                     string.Format("'{0}'", string.Join("','", guids)) + ")");

            //var s = new ScoopLinq<Users>().FromSubCube(x => x.IsActive).TranslateToScoop();
            //var b = new ScoopLinq<Users>().FromSubCube(x => x.IsActive).RunToList();

            //var xxxxx = new ScoopLinq<Users>().OrderByDesc(x => x.id).Paggin(0, 10).RunFirstOrDefault();


            //var temalar = ScoopXML.GetProjectThemes(Server.MapPath("~/App_Data/Theme.xml"));

            //var path = string.Format(@"{0}", Server.MapPath("~/Themes"));

            //var dirs = Directory.GetDirectories(path, "DefaultClean*").FirstOrDefault();


            //foreach (XmlNode node in temalar)
            //{
            //    //Fetch the Node values and assign it to Model.
            //    customers.Add(new CustomerModel
            //    {
            //        CustomerId = int.Parse(node["Id"].InnerText),
            //        Name = node["Name"].InnerText,
            //        Country = node["Country"].InnerText
            //    });
            //}

            //foreach (var item in users)
            //{
            //    item.UserName = "1";
            //}


            //var rrr = new ScoopLinq<Users>().BulkDelete(users, x => x.UserName);

            //new ScoopLinq<Users>().BulkUpdate

            //var bulkUpdate = new ScoopLinq<Users>().BulkUpdate(users);



            //   var user =
            //   new ScoopLinq<Users>().Where(x => x.id == new Guid("85F2339A-3063-452D-A86B-76190175780C"))
            //       .RunToList()
            //       .FirstOrDefault();
            //   //user.id=Guid.NewGuid();
            //   user.UserName = "mehmet3";
            //   var rs = new ScoopLinq<Users>().Update(user);






            //   user =
            //new ScoopLinq<Users>().Where(x => x.id == new Guid("85F2339A-3063-452D-A86B-76190175780C"))
            //    .RunToList()
            //    .FirstOrDefault();
            //   user.id = Guid.NewGuid();
            //   user.UserName = "mehmet4";
            //   rs = new ScoopLinq<Users>().Insert(user);



            //var icerik = new VW_TBL_Icerik();

            //var users = new List<Users>
            //{
            //    new Users()
            //    {
            //        id = new Guid("85F2339A-3063-452D-A86B-76190175780C"),
            //        UserEmail = "unal.m1",
            //        Order = 1,
            //        Password = "123453",created = DateTime.Now,
            //        UserName = "mehmet1"
            //    },
            //    new Users()
            //    {
            //        id = new Guid("E4D1A900-7C16-4178-A6FE-9DC89A0FCEEF"),
            //        UserEmail = "unal.m2",
            //        Order = 2,
            //        Password = "123452",
            //        UserName = "mehmet2",
            //        created = DateTime.Now,
            //    },
            //    new Users()
            //    {
            //        id = new Guid("6BA12E31-6624-4937-9424-BA12474E5C31"),
            //        UserEmail = "unal.m3",
            //        Order = 3,
            //        Password = "123450",
            //        UserName = "mehmet3",
            //        created = DateTime.Now,
            //    },
            //    new Users()
            //    {
            //        IsActive = true,
            //        id = new Guid("5DCE82FE-2DB5-498A-B550-C0BB541D264C"),
            //        UserEmail = "unal.m4",
            //        Order = 4,
            //        Password = "1234510",
            //        UserName = "mehmet4",
            //        created = DateTime.Now,
            //    }
            //};
            //var rsDB = new ScoopLinq<Users>().BulkInsert(users);

            //var a = "";
            ////var aaa = new ScoopLinq<VW_TBL_Icerik>().Select().RunToList();
            //new Logger().LogProvider.Error("WebPiranha.BeginRequest", "HATA");
            //Logger.TraceLogProvider.Error("WebPiranha.BeginRequest", "Unhandled exception");


            //var user = new Users()
            //{
            //    IsActive = true,
            //    UserEmail = "unal.m1",
            //    Order = 1,
            //    Password = "123456",
            //    UserName = "mehmet1",
            //    IsDelete = false
            //};


            //var model = new ScoopLinq<Users>().Where(x => x.IsActive == true).RunToList();
            //var model = new ScoopLinq<Users>().BulkUpdate(users);
            //var model = new ScoopLinq<Users>().Filtre(user).RunToList();

            //user.id = new Guid("E825BBF6-C6F2-4FD7-A1FE-0EE638D33A1C");


            //var id = new Guid("E825BBF6-C6F2-4FD7-A1FE-0EE638D33A1C");
            //var deleteWhere3 = new ScoopLinq<Users>().Delete(icerik, x => x.KategoriId == user.id);


            // var tip = 0;
            //  id = Guid.NewGuid();
            // //var asd= new ScoopLinq<VW_TBL_Icerik>().RunToList().ToList();
            // var asd = new ScoopLinq<VW_TBL_Icerik>()
            //     .Where(x => x.SayfaTipi == tip && x.KategoriId == new Guid("E825BBF6-C6F2-4FD7-A1FE-0EE638D33A1C") && x.durumu == Convert.ToBoolean(1) || x.KategoriId == id && x.SayfaTipi != 10)
            //         .RunToList()
            //         .ToList();


            // new ScoopLinq<VW_TBL_Icerik>()
            //  .Where(x => x.SayfaTipi == tip && x.KategoriId == new Guid("E825BBF6-C6F2-4FD7-A1FE-0EE638D33A1C") && x.durumu == Convert.ToBoolean(1) || x.KategoriId == id && x.SayfaTipi != 10)
            //      .RunToList()
            //      .ToList();
            // new ScoopLinq<VW_TBL_Icerik>()
            //  .Where(x => x.SayfaTipi == tip && x.KategoriId == new Guid("E825BBF6-C6F2-4FD7-A1FE-0EE638D33A1C") && x.durumu == Convert.ToBoolean(1) || x.KategoriId == id && x.SayfaTipi != 10)
            //      .RunToList()
            //      .ToList();
            // new ScoopLinq<VW_TBL_Icerik>()
            //  .Where(x => x.SayfaTipi == tip && x.KategoriId == new Guid("E825BBF6-C6F2-4FD7-A1FE-0EE638D33A1C") && x.durumu == Convert.ToBoolean(1) || x.KategoriId == id && x.SayfaTipi != 10)
            //      .RunToList()
            //      .ToList();
            // new ScoopLinq<VW_TBL_Icerik>()
            //.Where(x => x.SayfaTipi == tip && x.KategoriId == new Guid("E825BBF6-C6F2-4FD7-A1FE-0EE638D33A1C") && x.durumu == Convert.ToBoolean(1) || x.KategoriId == id && x.SayfaTipi != 10)
            //    .RunToList()
            //    .ToList();
            // new ScoopLinq<VW_TBL_Icerik>()
            //.Where(x => x.SayfaTipi == tip && x.KategoriId == new Guid("E825BBF6-C6F2-4FD7-A1FE-0EE638D33A1C") && x.durumu == Convert.ToBoolean(1) || x.KategoriId == id && x.SayfaTipi != 10)
            //    .RunToList()
            //    .ToList();
            // new ScoopLinq<VW_TBL_Icerik>()
            //.Where(x => x.SayfaTipi == tip && x.KategoriId == new Guid("E825BBF6-C6F2-4FD7-A1FE-0EE638D33A1C") && x.durumu == Convert.ToBoolean(1) || x.KategoriId == id && x.SayfaTipi != 10)
            //    .RunToList()
            //    .ToList();
            // new ScoopLinq<VW_TBL_Icerik>()
            //.Where(x => x.SayfaTipi == tip && x.KategoriId == new Guid("E825BBF6-C6F2-4FD7-A1FE-0EE638D33A1C") && x.durumu == Convert.ToBoolean(1) || x.KategoriId == id && x.SayfaTipi != 10)
            //    .RunToList()
            //    .ToList();
            // new ScoopLinq<VW_TBL_Icerik>()
            //.Where(x => x.SayfaTipi == tip && x.KategoriId == new Guid("E825BBF6-C6F2-4FD7-A1FE-0EE638D33A1C") && x.durumu == Convert.ToBoolean(1) || x.KategoriId == id && x.SayfaTipi != 10)
            //    .RunToList()
            //    .ToList();
            // new ScoopLinq<VW_TBL_Icerik>()
            //.Where(x => x.SayfaTipi == tip && x.KategoriId == new Guid("E825BBF6-C6F2-4FD7-A1FE-0EE638D33A1C") && x.durumu == Convert.ToBoolean(1) || x.KategoriId == id && x.SayfaTipi != 10)
            //    .RunToList()
            //    .ToList();
            //var id = new Guid("b8ae6bc9-8739-4a2f-be17-a49fd432d42a");


            //var userss = new Users();
            //userss.Order = 10;


            //#region SCOOP SELECT
            ////var users =
            ////        new ScoopLinq<Users>(DBHelper<Users>.ProviderConnection())
            ////            .Select(x => new Users() { UserEmail = x.UserEmail, IsActive = x.IsActive })
            ////            .Where(x => x.id == id && x.UserEmail == "demo" && x.IsActive == true && x.Order == userss.Order)
            ////            .OrderByDesc(o => o.id)
            ////            .Paggin(0, 10)
            ////            .RunToList();

            ////new ScoopLinq<Users>().Select().Where(x=>Regex.IsMatch(x.UserEmail,"\\Z")).RunToList();

            ////var xSelect = new ScoopLinq<Users>().Select().RunToList();
            //#endregion

            //#region SCOOP PERCOLATE
            //// var asd = new ScoopLinq<Users>().Percolate<Users>();
            //#endregion

            //var user = new Users()
            //{
            //    id = Guid.NewGuid(),
            //    IsActive = true,
            //    Password = "asdasd",
            //    UserName = "asdasdasd",
            //    IsDelete = false,
            //    UserEmail = "resdfsdf",
            //    created = DateTime.Now,
            //    Order = 3
            //};

            //#region SCOOP INSERT
            //var dbRes = new ScoopLinq<Users>().Insert(user);
            //#endregion

            //#region SCOOP DELETE
            ////var deleteWhere = new ScoopLinq<Users>().Delete(user, x => x.IsActive == Convert.ToBoolean(1));
            ////var deleteWhere2 = new ScoopLinq<Users>().Delete(user, x => x.id == new Guid("E825BBF6-C6F2-4FD7-A1FE-0EE638D33A1C"));
            ////var deletePk = new ScoopLinq<Users>().Delete(user);

            ////Hatalı oluyor valuesi gelmiyor x => x.id nin  => GetValueWhere Methodu Kontrol Edilecek
            ////var deleteWhere3 = new ScoopLinq<Users>().Delete(user, x => x.id);

            //#endregion

            //#region SCOOP UPDATE
            ////var updateWhere = new ScoopLinq<Users>().Update(user, list => list.IsActive == true);
            ////var updatePk = new ScoopLinq<Users>().Update(user);
            //#endregion 
            #endregion


            var db = new ScoopManagement();
            var trans = db.BeginTransaction();

            var user = new TBL_User { id = Guid.NewGuid(), Name = "Mehmet" };

            var rs = db.InsertTBL_User(user, trans);
            if (rs.Success)
            {
                var userEmail = new TBL_UserEmail { id = Guid.NewGuid(), UserId = user.id, Email = "a@gmail.com" };
                var rs2 = db.InsertTBL_UserEmail(userEmail, trans);
                trans.Commit();
            }

            return View();
        }

        //public ActionResult test(Guid id)
        //{

        //    var user = new Users() { id = id };

        //    var users = new ScoopLinq<Users>()
        //        .Where(x =>
        //            x.created == new DateTime(2017, 03, 29, 09, 02, 16, 793) ||
        //            x.id == user.id ||
        //            x.IsActive == false ||
        //            x.id == new Guid("b8ae6bc9-8739-4a2f-be17-a49fd432d42a") ||
        //            x.id.Equals(new Guid("b8ae6bc9-8739-4a2f-be17-a49fd432d42a")) ||
        //           !string.IsNullOrEmpty(x.UserEmail) ||
        //           x.IsActive != null ||
        //           x.IsActive == null
        //        ).RunToList();



        //    return View();
        //}
        //public List<Users> GetUser()
        //{
        //    var cacheKey = "User";
        //    List<Users> currencyData = _repository.Cache.Get(cacheKey) as List<Users>;
        //    if (currencyData == null)
        //    {
        //        //DB den veri burda çekilecek
        //        currencyData = new ScoopLinq<Users>().Where(x => x.id == new Guid("85F2339A-3063-452D-A86B-76190175780C")).RunToList().ToList();
        //        if (currencyData.Any())
        //        {
        //            var cachedTimeOut = System.Web.Configuration.WebConfigurationManager.AppSettings["CachedTimeOut"];
        //            var time = !string.IsNullOrEmpty(cachedTimeOut) ? int.Parse(cachedTimeOut) : 30;
        //            _repository.Cache.Set(cacheKey, currencyData, time);
        //        }
        //    }
        //    return currencyData;
        //}    

        public ActionResult DatabaseBackup()
        {
            var feedBack = new FeedBack();
            var fileRS = ScoopFiles.GetDBScript(null);

            if (fileRS.Success)
            {
                feedBack.Success("Veri Tabanı Yedekleme İşlemi Başarılı", true);
                return base.File(fileRS.Data.ToString(), ".zip");
            }

            feedBack.Warning("Veri Tabanı Yedekleme İşlemi Başarısız", true);
            return Redirect(Url.Action("Index", "Home"));
        }


        public ActionResult Sart(Guid id)
        {
            var userss = new Users();
            #region SCOOP SELECT
            //var users =
            //        new ScoopLinq<Users>(DBHelper<Users>.MysqlProviderConnection())
            //            .Select(x => new Users() { UserEmail = x.UserEmail, IsActive = x.IsActive })
            //            .Where(x => x.id == id && x.UserEmail == "demo" && x.IsActive == true)
            //            .OrderByDesc(o => o.id)
            //            .Paggin(0, 10)
            //            .RunToList();
            //new ScoopLinq<Users>().Select().Where(x=>Regex.IsMatch(x.UserEmail,"\\Z")).RunToList();
            //var xSelect = new ScoopLinq<Users>().Select().RunToList();
            //#endregion
            //#region SCOOP PERCOLATE
            //var asd = new ScoopLinq<Users>().Percolate<Users>();
            //#endregion
            //var user = new Users()
            //{
            //    id = Guid.NewGuid(),
            //    IsActive = true,
            //    Password = "asdasd",
            //    UserName = "asdasdasd",
            //    IsDelete = false,
            //    UserEmail = "resdfsdf",
            //    created = DateTime.Now,
            //};
            //#region SCOOP INSERT
            //var dbRes = new ScoopLinq<Users>().Insert(user);
            //#endregion
            //#region SCOOP DELETE
            //var deleteWhere = new ScoopLinq<Users>().Delete(user, x => x.IsActive == Convert.ToBoolean(1));
            //var deleteWhere2 = new ScoopLinq<Users>().Delete(user, x => x.id == new Guid("E825BBF6-C6F2-4FD7-A1FE-0EE638D33A1C"));
            //var deletePk = new ScoopLinq<Users>().Delete(user);
            ////Hatalı oluyor valuesi gelmiyor x => x.id nin  => GetValueWhere Methodu Kontrol Edilecek
            //var deleteWhere3 = new ScoopLinq<Users>().Delete(user, x => x.id);
            //#endregion
            //#region SCOOP UPDATE
            //var updateWhere = new ScoopLinq<Users>().Update(user, list => list.IsActive == true);
            //var updatePk = new ScoopLinq<Users>().Update(user);
            #endregion
            return View();
        }

        //public ActionResult MyActionResult([ModelBinder(typeof(DataTablesBinder))] IDataTablesRequest requestModel)
        //{

        //    var model = new ScoopLinq<Users>().RunToList().ToList();


        //    // do your stuff...
        //    var paged = model.Skip(requestModel.Start).Take(requestModel.Length);
        //    var myFilteredData = paged;
        //    var myOriginalDataSet = model;
        //    return View(new DataTablesResponse(requestModel.Draw, paged, myFilteredData.Count(), myOriginalDataSet.Count()));
        //}

        //// Or if you'd like to return a JsonResult, try this:
        //public JsonResult DataSource([ModelBinder(typeof(DataTablesBinder))] IDataTablesRequest requestModel)
        //{
        //    var paged = new ScoopLinq<Users>().OrderBy(o => o.created).Where(x => x.IsActive == true).DataTablesFiltre(requestModel).RunToList().ToList();
        //    var myFilteredData = paged.Count();
        //    var myOriginalDataSet = new ScoopLinq<Users>().Where(x => x.IsActive == true).DataTablesFiltre(requestModel).Count().RunCount();
        //    return Json(new DataTablesResponse(requestModel.Draw, paged, myFilteredData, myOriginalDataSet), JsonRequestBehavior.AllowGet);
        //}

        public ActionResult Upload()
        {

            //var data = new ScoopLinq<VW_TBL_Icerik>().RunToList().ToList();

            //foreach (var item in data)
            //{
            //    var a = ScoopFiles.ImagesCreate(Server.MapPath("~/upload/img/big/"), Server.MapPath("~/upload/img/"), item.Resim, new Size(398, 240), item.KategoriAdi);
            //    var b = ScoopFiles.ImagesCreate(Server.MapPath("~/upload/img/big/"), Server.MapPath("~/upload/img/"), item.Resim, new Size(300, 169), item.KategoriAdi);
            //    var c = ScoopFiles.ImagesCreate(Server.MapPath("~/upload/img/big/"), Server.MapPath("~/upload/img/"), item.Resim, new Size(80, 80), item.KategoriAdi);
            //    var d = ScoopFiles.ImagesCreate(Server.MapPath("~/upload/img/big/"), Server.MapPath("~/upload/img/"), item.Resim, null, item.KategoriAdi);
            //}



            var file = Request.Files[0];



            if (file != null && file.ContentLength > 0)
            {
                var list = new List<HttpPostedFileBase> { file }.ToArray();
                var a = ScoopFiles.ImagesFilesCreate(list, Server.MapPath("~/upload"), new Size(398, 240), "saglikli olmak", null, true, "mehmet-unal.com.tr");
            }


            return View("Bitti");
        }
    }
    public class Users
    {
        public Guid id { get; set; }
        [Required, DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime created { get; set; }
        public DateTime? changed { get; set; }
        public string UserName { get; set; }
        public string UserEmail { get; set; }
        public string Password { get; set; }
        public bool IsActive { get; set; }
        public bool IsDelete { get; set; }
        public int Order { get; set; }
    }
}