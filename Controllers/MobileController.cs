using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace x9web.Controllers
{
    public class MobileController : Controller
    {
        //
        // GET: /Mobile/

        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Index(string name, string pwd, string xcode)
        {
            //name = "cuayi";
            //pwd = "1";
            //xcode = "2";
            //-------------------------------------------------------------
            if (!Api.WebClass.web_switch)
            {
                return Json(new { type = "err", msg = "网站维护中，请稍候再访问！" });
            }
            if (name == string.Empty)
            {
                return Json(new { type = "err", msg = "會員名稱不能爲空！" });
            }
            if (pwd == string.Empty)
            {
                return Json(new { type = "err", msg = "密碼不能爲空！" });
            }
            if (xcode == string.Empty)
            {
                return Json(new { type = "err", msg = "驗證碼不能爲空！" });
            }
            if (xcode.ToUpper() != Session["code"].ToString())
            {
                return Json(new { type = "err", msg = "驗證碼輸入錯誤" });
            }

            try
            {
                using (Models.Sqlconnect con = new Models.Sqlconnect())
                {
                    var m = con.xusers.Where(a => a.name == name).Where(a => a.pwd == Api.WebClass.FunMD5(pwd)).Where(a => a.manage == false).Where(a => a.state == true).ToList();

                    if (m.Count() > 0)
                    {
                        Session["name"] = name;
                        Session["id"] = m.First().id;
                        Session["gf"] = m.First().gf_state;

                        return Json(new { type = "suc" });
                    }
                    else
                    {
                        return Json(new { type = "err", msg = "用戶名或密碼錯誤，請重新輸入！" });
                    }
                }
            }
            catch (Exception ex)
            {
                return Json(new { type = "err", msg = ex.Message });
            }
        }
        public ActionResult Agreement()
        {
            return View();
        }
    }
}
