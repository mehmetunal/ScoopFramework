using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace Scoop.Captcha
{
    public  class GetCaptcha:Controller
    {
        public static ImageResult Image()
        {
            var bmp = new Bitmap(200, 50);
            var g = Graphics.FromImage(bmp);
            var code = CreateCode();
            System.Web.HttpContext.Current.Session["code"] = code;
            g.FillRectangle(Brushes.White, 0, 0, 200, 50);
            g.DrawString(code, new Font("Arial", 32), Brushes.Red, new PointF(0, 0));
            return new ImageResult { Image = bmp, ImageFormat = ImageFormat.Jpeg, };
        }
        private static string CreateCode()
        {
            string code = null;
            try
            {
                var rnd = new Random();
                const string charecters = "1234567890QWERTYUIOPASDFGHJKLZXCVBNM";

                for (var i = 1; i < 7; i++)
                {
                    code = code + charecters[rnd.Next(0, charecters.Length + 1)];
                }
            }
            catch
            {
                CreateCode();
            }
            return code;
        }
      
    }
}
