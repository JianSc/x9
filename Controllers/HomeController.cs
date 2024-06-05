using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Webdiyer.WebControls.Mvc;

namespace x9web.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/

        public ActionResult Index()
        {
            Session["name"] = null;
            if (IsMobileDevice())
            {
                Response.Redirect("/mobile/index");
                return null;
            }
            else
            {
                return View();
            }

        }

        public bool IsMobileDevice()
        {
            string[] mobileAgents = { "iphone", "android", "phone", "mobile", "wap", "netfront", "java", "opera mobi", "opera mini", "ucweb", "windows ce", "symbian", "series", "webos", "sony", "blackberry", "dopod", "nokia", "samsung", "palmsource", "xda", "pieplus", "meizu", "midp", "cldc", "motorola", "foma", "docomo", "up.browser", "up.link", "blazer", "helio", "hosin", "huawei", "novarra", "coolpad", "webos", "techfaith", "palmsource", "alcatel", "amoi", "ktouch", "nexian", "ericsson", "philips", "sagem", "wellcom", "bunjalloo", "maui", "smartphone", "iemobile", "spice", "bird", "zte-", "longcos", "pantech", "gionee", "portalmmm", "jig browser", "hiptop", "benq", "haier", "^lct", "320x320", "240x320", "176x220", "w3c ", "acs-", "alav", "alca", "amoi", "audi", "avan", "benq", "bird", "blac", "blaz", "brew", "cell", "cldc", "cmd-", "dang", "doco", "eric", "hipt", "inno", "ipaq", "java", "jigs", "kddi", "keji", "leno", "lg-c", "lg-d", "lg-g", "lge-", "maui", "maxo", "midp", "mits", "mmef", "mobi", "mot-", "moto", "mwbp", "nec-", "newt", "noki", "oper", "palm", "pana", "pant", "phil", "play", "port", "prox", "qwap", "sage", "sams", "sany", "sch-", "sec-", "send", "seri", "sgh-", "shar", "sie-", "siem", "smal", "smar", "sony", "sph-", "symb", "t-mo", "teli", "tim-", "tosh", "tsm-", "upg1", "upsi", "vk-v", "voda", "wap-", "wapa", "wapi", "wapp", "wapr", "webc", "winw", "winw", "xda", "xda-", "Googlebot-Mobile" };
            bool isMoblie = false;
            if (Request.UserAgent.ToString().ToLower() != null)
            {
                for (int i = 0; i < mobileAgents.Length; i++)
                {
                    if (Request.UserAgent.ToString().ToLower().IndexOf(mobileAgents[i]) >= 0)
                    {
                        isMoblie = true;
                        break;
                    }
                }
            }
            if (isMoblie)
            {
                return true;
            }
            else
            {
                return false;
            }
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
                        Session["name"] = name.ToLower();
                        Session["id"] = m.First().id;
                        //Session["gf"] = m.First().gf_state;
                        Session["gf"] = false;
                        Session["tsid"] = m.First().id;

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
            if (Session["name"] == null)
            {
                return RedirectToAction("index", "home");
            }
            return View();
        }

        public ActionResult exit()
        {
            Session["name"] = null;
            return RedirectToAction("index", "home");
        }

        public ActionResult main(string id = "temaA")
        {
            if (Session["name"] == null)
            {
                return RedirectToAction("index", "home");
            }
            if (!Api.WebClass.pankou_zon)
            {
                return View("pankou_zon");
            }
            if (id == "temaA" || id == "temaB")
            {
                if (!Api.WebClass.pankou_tem)
                {
                    return View("pankou_tem");
                }
            }
            else
            {
                if (!Api.WebClass.pankou_zem)
                {
                    return View("pankou_zem");
                }
            }
            try
            {
                using (Models.Sqlconnect con = new Models.Sqlconnect())
                {
                    var m = con.xball.ToList();
                    IEnumerable<x9web.Models.xball> ms;
                    #region 封盘赔率修改为0
                    if (id != "temaA" && id != "temaB" && id != "temaBK" && id != "temaAK")
                    {
                        if (!Api.WebClass.pankou_zon || !Api.WebClass.pankou_zem)
                        {
                            foreach (var mc in m)
                            {
                                mc.zma = 0;
                                mc.zmb = 0;
                                mc.zt1 = 0;
                                mc.zt2 = 0;
                                mc.zt3 = 0;
                                mc.zt4 = 0;
                                mc.zt5 = 0;
                                mc.zt6 = 0;
                                mc.s2qz = 0;
                                mc.s2zt = 0;
                                mc.stc = 0;
                                mc.s3qz = 0;
                                mc.s3z2 = 0;
                                mc.tiex = 0;
                                mc.babo = 0;
                                mc.yix = 0;
                                mc.yixb = 0;
                                mc.liux = 0;
                                mc.liuxb = 0;
                                mc.ws = 0;
                                mc.wsb = 0;
                                mc.db5 = 0;
                                mc.db6 = 0;
                                mc.db7 = 0;
                                mc.db8 = 0;
                                mc.db9 = 0;
                                mc.db10 = 0;
                                mc.x2 = 0;
                                mc.x2b = 0;
                                mc.x3 = 0;
                                mc.x3b = 0;
                                mc.x4 = 0;
                                mc.x4b = 0;
                                mc.x5 = 0;
                                mc.x5b = 0;
                                mc.w2 = 0;
                                mc.w2b = 0;
                                mc.w3 = 0;
                                mc.w3b = 0;
                                mc.w4 = 0;
                                mc.w4b = 0;
                                mc.dz5 = 0;
                                mc.dz6 = 0;
                                mc.dz7 = 0;
                                mc.dz8 = 0;
                                mc.dz9 = 0;
                                mc.dz10 = 0;
                                mc.rz1 = 0;
                                mc.rz2 = 0;
                                mc.rz3 = 0;
                                mc.rz4 = 0;
                                mc.rz5 = 0;
                                mc.s2zt2 = 0;
                                mc.s3z22 = 0;
                            }
                        }
                    }
                    else
                    {
                        if (!Api.WebClass.pankou_zon || !Api.WebClass.pankou_tem)
                        {
                            foreach (var mc in m)
                            {
                                mc.tma = 0;
                                mc.tmb = 0;
                            }
                        }
                    }
                    #endregion

                    switch (id)
                    {
                        #region 特码
                        case "temaA":
                            ViewBag.menuname = "特碼";
                            ViewBag.script_id = 8011;
                            ViewBag.page_name = "特A";
                            ViewBag.page_menu_active = "A盘";
                            ViewBag.page_menu = "A盘,B盘";
                            ViewBag.page_menu_url = "/home/main/temaA,/home/main/temaB";
                            ViewBag.btn_kuaisu = "/home/main/temaAK";
                            ms = from q in m
                                 where (q.type == 811 || q.type == null)
                                 select new x9web.Models.xball
                                 {
                                     name = q.name.Trim(),
                                     color = q.color,
                                     tma = q.tma
                                 };
                            return View("teze_ma", ms);
                        case "temaAK":
                            ViewBag.menuname = "特碼";
                            ViewBag.script_id = 8011;
                            ViewBag.page_name = "特A";
                            ViewBag.page_menu_active = "A盘";
                            ViewBag.page_menu = "A盘,B盘";
                            ViewBag.page_menu_url = "/home/main/temaA,/home/main/temaB";
                            ViewBag.btn_kuaisu = "/home/main/temaA";
                            ms = from q in m
                                 where (q.type == null)
                                 select new x9web.Models.xball
                                 {
                                     name = q.name.Trim(),
                                     color = q.color,
                                     tma = q.tma
                                 };
                            return View("teze_ma_kuasu", ms);
                        case "temaB":
                            ViewBag.menuname = "特碼";
                            ViewBag.script_id = 8012;
                            ViewBag.page_name = "特B";
                            ViewBag.page_menu_active = "B盘";
                            ViewBag.page_menu = "A盘,B盘";
                            ViewBag.page_menu_url = "/home/main/temaA,/home/main/temaB";
                            ViewBag.btn_kuaisu = "/home/main/temaBK";
                            ms = from q in m
                                 where (q.type == 811 || q.type == null)
                                 select new x9web.Models.xball
                                 {
                                     name = q.name.Trim(),
                                     color = q.color,
                                     tma = q.tmb
                                 };
                            return View("teze_ma", ms);
                        case "temaBK":
                            ViewBag.menuname = "特碼";
                            ViewBag.script_id = 8012;
                            ViewBag.page_name = "特B";
                            ViewBag.page_menu_active = "B盘";
                            ViewBag.page_menu = "A盘,B盘";
                            ViewBag.page_menu_url = "/home/main/temaA,/home/main/temaB";
                            ViewBag.btn_kuaisu = "/home/main/temaB";
                            ms = from q in m
                                 where (q.type == null)
                                 select new x9web.Models.xball
                                 {
                                     name = q.name.Trim(),
                                     color = q.color,
                                     tma = q.tmb
                                 };
                            return View("teze_ma_kuasu", ms);
                        #endregion
                        #region 正码
                        case "zemaA":
                            ViewBag.menuname = "正碼";
                            ViewBag.script_id = 8013;
                            ViewBag.page_name = "正A";
                            ViewBag.page_menu_active = "A盘";
                            ViewBag.page_menu = "A盘,B盘";
                            ViewBag.page_menu_url = "/home/main/zemaA,/home/main/zemaB";
                            ViewBag.btn_kuaisu = "/home/main/zemaAK";
                            ms = from q in m
                                 where (q.type == 812 || q.type == null)
                                 select new x9web.Models.xball
                                 {
                                     name = q.name.Trim(),
                                     color = q.color,
                                     tma = q.zma
                                 };
                            return View("teze_ma", ms);
                        case "zemaAK":
                            ViewBag.menuname = "正碼";
                            ViewBag.script_id = 8013;
                            ViewBag.page_name = "正A";
                            ViewBag.page_menu_active = "A盘";
                            ViewBag.page_menu = "A盘,B盘";
                            ViewBag.page_menu_url = "/home/main/zemaA,/home/main/zemaB";
                            ViewBag.btn_kuaisu = "/home/main/zemaA";
                            ms = from q in m
                                 where (q.type == null)
                                 select new x9web.Models.xball
                                 {
                                     name = q.name.Trim(),
                                     color = q.color,
                                     tma = q.zma
                                 };
                            return View("teze_ma_kuasu", ms);
                        case "zemaB":
                            ViewBag.menuname = "正碼";
                            ViewBag.script_id = 8014;
                            ViewBag.page_name = "正B";
                            ViewBag.page_menu_active = "B盘";
                            ViewBag.page_menu = "A盘,B盘";
                            ViewBag.page_menu_url = "/home/main/zemaA,/home/main/zemaB";
                            ViewBag.btn_kuaisu = "/home/main/zemaBK";
                            ms = from q in m
                                 where (q.type == 812 || q.type == null)
                                 select new x9web.Models.xball
                                 {
                                     name = q.name.Trim(),
                                     color = q.color,
                                     tma = q.zmb
                                 };
                            return View("teze_ma", ms);
                        case "zemaBK":
                            ViewBag.menuname = "正碼";
                            ViewBag.script_id = 8014;
                            ViewBag.page_name = "正B";
                            ViewBag.page_menu_active = "B盘";
                            ViewBag.page_menu = "A盘,B盘";
                            ViewBag.page_menu_url = "/home/main/zemaA,/home/main/zemaB";
                            ViewBag.btn_kuaisu = "/home/main/zemaB";
                            ms = from q in m
                                 where (q.type == null)
                                 select new x9web.Models.xball
                                 {
                                     name = q.name.Trim(),
                                     color = q.color,
                                     tma = q.zmb
                                 };
                            return View("teze_ma_kuasu", ms);
                        #endregion
                        #region 正码特
                        case "zhen1":
                            ViewBag.menuname = "正碼特";
                            ViewBag.script_id = 8015;
                            ViewBag.page_name = "正1特";
                            ViewBag.page_menu_active = "正1特";
                            ViewBag.page_menu = "正1特,正2特,正3特,正4特,正5特,正6特";
                            ViewBag.page_menu_url = "/home/main/zhen1,/home/main/zhen2,/home/main/zhen3,/home/main/zhen4,/home/main/zhen5,/home/main/zhen6";
                            ViewBag.btn_kuaisu = "/home/main/zhen1K";
                            ms = from q in m
                                 where (q.type == 813 || q.type == null)
                                 select new x9web.Models.xball
                                 {
                                     name = q.name.Trim(),
                                     color = q.color,
                                     tma = q.zt1
                                 };
                            return View("zema_t", ms);
                        case "zhen1K":
                            ViewBag.menuname = "正碼特";
                            ViewBag.script_id = 8015;
                            ViewBag.page_name = "正1特";
                            ViewBag.page_menu_active = "正1特";
                            ViewBag.page_menu = "正1特,正2特,正3特,正4特,正5特,正6特";
                            ViewBag.page_menu_url = "/home/main/zhen1,/home/main/zhen2,/home/main/zhen3,/home/main/zhen4,/home/main/zhen5,/home/main/zhen6";
                            ViewBag.btn_kuaisu = "/home/main/zhen1";
                            ms = from q in m
                                 where (q.type == null)
                                 select new x9web.Models.xball
                                 {
                                     name = q.name.Trim(),
                                     color = q.color,
                                     tma = q.zt1
                                 };
                            return View("zema_t_kuasu", ms);
                        case "zhen2":
                            ViewBag.menuname = "正碼特";
                            ViewBag.script_id = 8016;
                            ViewBag.page_name = "正2特";
                            ViewBag.page_menu_active = "正2特";
                            ViewBag.page_menu = "正1特,正2特,正3特,正4特,正5特,正6特";
                            ViewBag.page_menu_url = "/home/main/zhen1,/home/main/zhen2,/home/main/zhen3,/home/main/zhen4,/home/main/zhen5,/home/main/zhen6";
                            ViewBag.btn_kuaisu = "/home/main/zhen2K";
                            ms = from q in m
                                 where (q.type == 813 || q.type == null)
                                 select new x9web.Models.xball
                                 {
                                     name = q.name.Trim(),
                                     color = q.color,
                                     tma = q.zt2
                                 };
                            return View("zema_t", ms);
                        case "zhen2K":
                            ViewBag.menuname = "正碼特";
                            ViewBag.script_id = 8016;
                            ViewBag.page_name = "正2特";
                            ViewBag.page_menu_active = "正2特";
                            ViewBag.page_menu = "正1特,正2特,正3特,正4特,正5特,正6特";
                            ViewBag.page_menu_url = "/home/main/zhen1,/home/main/zhen2,/home/main/zhen3,/home/main/zhen4,/home/main/zhen5,/home/main/zhen6";
                            ViewBag.btn_kuaisu = "/home/main/zhen2";
                            ms = from q in m
                                 where (q.type == null)
                                 select new x9web.Models.xball
                                 {
                                     name = q.name.Trim(),
                                     color = q.color,
                                     tma = q.zt2
                                 };
                            return View("zema_t_kuasu", ms);
                        case "zhen3":
                            ViewBag.menuname = "正碼特";
                            ViewBag.script_id = 8017;
                            ViewBag.page_name = "正3特";
                            ViewBag.page_menu_active = "正3特";
                            ViewBag.page_menu = "正1特,正2特,正3特,正4特,正5特,正6特";
                            ViewBag.page_menu_url = "/home/main/zhen1,/home/main/zhen2,/home/main/zhen3,/home/main/zhen4,/home/main/zhen5,/home/main/zhen6";
                            ViewBag.btn_kuaisu = "/home/main/zhen3K";
                            ms = from q in m
                                 where (q.type == 813 || q.type == null)
                                 select new x9web.Models.xball
                                 {
                                     name = q.name.Trim(),
                                     color = q.color,
                                     tma = q.zt3
                                 };
                            return View("zema_t", ms);
                        case "zhen3K":
                            ViewBag.menuname = "正碼特";
                            ViewBag.script_id = 8017;
                            ViewBag.page_name = "正3特";
                            ViewBag.page_menu_active = "正3特";
                            ViewBag.page_menu = "正1特,正2特,正3特,正4特,正5特,正6特";
                            ViewBag.page_menu_url = "/home/main/zhen1,/home/main/zhen2,/home/main/zhen3,/home/main/zhen4,/home/main/zhen5,/home/main/zhen6";
                            ViewBag.btn_kuaisu = "/home/main/zhen3";
                            ms = from q in m
                                 where (q.type == null)
                                 select new x9web.Models.xball
                                 {
                                     name = q.name.Trim(),
                                     color = q.color,
                                     tma = q.zt3
                                 };
                            return View("zema_t_kuasu", ms);
                        case "zhen4":
                            ViewBag.menuname = "正碼特";
                            ViewBag.script_id = 8018;
                            ViewBag.page_name = "正4特";
                            ViewBag.page_menu_active = "正4特";
                            ViewBag.page_menu = "正1特,正2特,正3特,正4特,正5特,正6特";
                            ViewBag.page_menu_url = "/home/main/zhen1,/home/main/zhen2,/home/main/zhen3,/home/main/zhen4,/home/main/zhen5,/home/main/zhen6";
                            ViewBag.btn_kuaisu = "/home/main/zhen4K";
                            ms = from q in m
                                 where (q.type == 813 || q.type == null)
                                 select new x9web.Models.xball
                                 {
                                     name = q.name.Trim(),
                                     color = q.color,
                                     tma = q.zt4
                                 };
                            return View("zema_t", ms);
                        case "zhen4K":
                            ViewBag.menuname = "正碼特";
                            ViewBag.script_id = 8018;
                            ViewBag.page_name = "正4特";
                            ViewBag.page_menu_active = "正4特";
                            ViewBag.page_menu = "正1特,正2特,正3特,正4特,正5特,正6特";
                            ViewBag.page_menu_url = "/home/main/zhen1,/home/main/zhen2,/home/main/zhen3,/home/main/zhen4,/home/main/zhen5,/home/main/zhen6";
                            ViewBag.btn_kuaisu = "/home/main/zhen4";
                            ms = from q in m
                                 where (q.type == null)
                                 select new x9web.Models.xball
                                 {
                                     name = q.name.Trim(),
                                     color = q.color,
                                     tma = q.zt4
                                 };
                            return View("zema_t_kuasu", ms);
                        case "zhen5":
                            ViewBag.menuname = "正碼特";
                            ViewBag.script_id = 8019;
                            ViewBag.page_name = "正5特";
                            ViewBag.page_menu_active = "正5特";
                            ViewBag.page_menu = "正1特,正2特,正3特,正4特,正5特,正6特";
                            ViewBag.page_menu_url = "/home/main/zhen1,/home/main/zhen2,/home/main/zhen3,/home/main/zhen4,/home/main/zhen5,/home/main/zhen6";
                            ViewBag.btn_kuaisu = "/home/main/zhen5K";
                            ms = from q in m
                                 where (q.type == 813 || q.type == null)
                                 select new x9web.Models.xball
                                 {
                                     name = q.name.Trim(),
                                     color = q.color,
                                     tma = q.zt5
                                 };
                            return View("zema_t", ms);
                        case "zhen5K":
                            ViewBag.menuname = "正碼特";
                            ViewBag.script_id = 8019;
                            ViewBag.page_name = "正5特";
                            ViewBag.page_menu_active = "正5特";
                            ViewBag.page_menu = "正1特,正2特,正3特,正4特,正5特,正6特";
                            ViewBag.page_menu_url = "/home/main/zhen1,/home/main/zhen2,/home/main/zhen3,/home/main/zhen4,/home/main/zhen5,/home/main/zhen6";
                            ViewBag.btn_kuaisu = "/home/main/zhen5";
                            ms = from q in m
                                 where (q.type == null)
                                 select new x9web.Models.xball
                                 {
                                     name = q.name.Trim(),
                                     color = q.color,
                                     tma = q.zt5
                                 };
                            return View("zema_t_kuasu", ms);
                        case "zhen6":
                            ViewBag.menuname = "正碼特";
                            ViewBag.script_id = 8020;
                            ViewBag.page_name = "正6特";
                            ViewBag.page_menu_active = "正6特";
                            ViewBag.page_menu = "正1特,正2特,正3特,正4特,正5特,正6特";
                            ViewBag.page_menu_url = "/home/main/zhen1,/home/main/zhen2,/home/main/zhen3,/home/main/zhen4,/home/main/zhen5,/home/main/zhen6";
                            ViewBag.btn_kuaisu = "/home/main/zhen6K";
                            ms = from q in m
                                 where (q.type == 813 || q.type == null)
                                 select new x9web.Models.xball
                                 {
                                     name = q.name.Trim(),
                                     color = q.color,
                                     tma = q.zt6
                                 };
                            return View("zema_t", ms);
                        case "zhen6K":
                            ViewBag.menuname = "正碼特";
                            ViewBag.script_id = 8020;
                            ViewBag.page_name = "正6特";
                            ViewBag.page_menu_active = "正6特";
                            ViewBag.page_menu = "正1特,正2特,正3特,正4特,正5特,正6特";
                            ViewBag.page_menu_url = "/home/main/zhen1,/home/main/zhen2,/home/main/zhen3,/home/main/zhen4,/home/main/zhen5,/home/main/zhen6";
                            ViewBag.btn_kuaisu = "/home/main/zhen6";
                            ms = from q in m
                                 where (q.type == null)
                                 select new x9web.Models.xball
                                 {
                                     name = q.name.Trim(),
                                     color = q.color,
                                     tma = q.zt6
                                 };
                            return View("zema_t_kuasu", ms);
                        #endregion
                        #region 连码
                        case "lianmaf":
                            ViewBag.script_id = 8021;
                            ViewBag.menuname = "連碼";
                            ViewBag.page_name = "二全中";
                            ViewBag.page_menu_active = "二全中";
                            ViewBag.page_menu_active2 = "复式";
                            ViewBag.page_menu2 = "复式,拖头,生肖对碰,尾数对碰,生尾对碰,任意对碰";
                            ViewBag.page_menu2_url = "/home/main/lianmaf,/home/main/lianmat,/home/main/sxdp,/home/main/wsdp,/home/main/swdp,/home/main/rydp";
                            ViewBag.page_menu = "二全中,二中特,特串,三中二,三全中";
                            ViewBag.page_menu_url = "/home/main/lianmaf,/home/main/lianmaf2,/home/main/lianmaftc,/home/main/lianmaf3z2,/home/main/lianmaf3z ";
                            ms = from q in m
                                 where (q.type == null)
                                 select new x9web.Models.xball
                                 {
                                     name = q.name.Trim(),
                                     color = q.color,
                                     tma = q.s2qz
                                 };
                            return View("lianma-f", ms);
                        case "lianmaf2":
                            ViewBag.script_id = 8022;
                            ViewBag.menuname = "連碼";
                            ViewBag.page_name = "二中特";
                            ViewBag.page_menu_active = "二中特";
                            ViewBag.page_menu_active2 = "复式";
                            ViewBag.page_menu2 = "复式,拖头,生肖对碰,尾数对碰,生尾对碰,任意对碰";
                            ViewBag.page_menu2_url = "/home/main/lianmaf2,/home/main/lianmat2,/home/main/sxdp2,/home/main/wsdp2,/home/main/swdp2,/home/main/rydp2";
                            ViewBag.page_menu = "二全中,二中特,特串,三中二,三全中";
                            ViewBag.page_menu_url = "/home/main/lianmaf,/home/main/lianmaf2,/home/main/lianmaftc,/home/main/lianmaf3z2,/home/main/lianmaf3z ";
                            ms = from q in m
                                 where (q.type == null)
                                 select new x9web.Models.xball
                                 {
                                     name = q.name.Trim(),
                                     color = q.color,
                                     tma = q.s2zt,
                                     tmb = q.s2zt2
                                 };
                            return View("lianma-f3", ms);
                        case "lianmat":
                            ViewBag.script_id = 8021;
                            ViewBag.menuname = "連碼";
                            ViewBag.page_name = "二全中";
                            ViewBag.page_menu_active = "二全中";
                            ViewBag.page_menu_active2 = "拖头";
                            ViewBag.page_menu2 = "复式,拖头,生肖对碰,尾数对碰,生尾对碰,任意对碰";
                            ViewBag.page_menu2_url = "/home/main/lianmaf,/home/main/lianmat,/home/main/sxdp,/home/main/wsdp,/home/main/swdp,/home/main/rydp";
                            ViewBag.page_menu = "二全中,二中特,特串,三中二,三全中";
                            ViewBag.page_menu_url = "/home/main/lianmat,/home/main/lianmat2,/home/main/lianmattc,/home/main/lianmat3z2,/home/main/lianmat3z ";
                            ms = from q in m
                                 where (q.type == null)
                                 select new x9web.Models.xball
                                 {
                                     name = q.name.Trim(),
                                     color = q.color,
                                     tma = q.s2qz
                                 };
                            return View("lianma-f", ms);
                        case "lianmat2":
                            ViewBag.script_id = 8022;
                            ViewBag.menuname = "連碼";
                            ViewBag.page_name = "二中特";
                            ViewBag.page_menu_active = "二中特";
                            ViewBag.page_menu_active2 = "拖头";
                            ViewBag.page_menu2 = "复式,拖头,生肖对碰,尾数对碰,生尾对碰,任意对碰";
                            ViewBag.page_menu2_url = "/home/main/lianmaf2,/home/main/lianmat2,/home/main/sxdp2,/home/main/wsdp2,/home/main/swdp2,/home/main/rydp2";
                            ViewBag.page_menu = "二全中,二中特,特串,三中二,三全中";
                            ViewBag.page_menu_url = "/home/main/lianmat,/home/main/lianmat2,/home/main/lianmattc,/home/main/lianmat3z2,/home/main/lianmat3z ";
                            ms = from q in m
                                 where (q.type == null)
                                 select new x9web.Models.xball
                                 {
                                     name = q.name.Trim(),
                                     color = q.color,
                                     tma = q.s2zt,
                                     tmb = q.s2zt2
                                 };
                            return View("lianma-f3", ms);
                        case "lianmaftc":
                            ViewBag.script_id = 8023;
                            ViewBag.menuname = "連碼";
                            ViewBag.page_name = "特串";
                            ViewBag.page_menu_active = "特串";
                            ViewBag.page_menu_active2 = "复式";
                            ViewBag.page_menu2 = "复式,拖头,生肖对碰,尾数对碰,生尾对碰,任意对碰";
                            ViewBag.page_menu2_url = "/home/main/lianmaftc,/home/main/lianmattc,/home/main/sxdp3,/home/main/wsdp3,/home/main/swdp3,/home/main/rydp3";
                            ViewBag.page_menu = "二全中,二中特,特串,三中二,三全中";
                            ViewBag.page_menu_url = "/home/main/lianmaf,/home/main/lianmaf2,/home/main/lianmaftc,/home/main/lianmaf3z2,/home/main/lianmaf3z ";
                            ms = from q in m
                                 where (q.type == null)
                                 select new x9web.Models.xball
                                 {
                                     name = q.name.Trim(),
                                     color = q.color,
                                     tma = q.stc
                                 };
                            return View("lianma-f", ms);
                        case "lianmattc":
                            ViewBag.script_id = 8023;
                            ViewBag.menuname = "連碼";
                            ViewBag.page_name = "特串";
                            ViewBag.page_menu_active = "特串";
                            ViewBag.page_menu_active2 = "拖头";
                            ViewBag.page_menu2 = "复式,拖头,生肖对碰,尾数对碰,生尾对碰,任意对碰";
                            ViewBag.page_menu2_url = "/home/main/lianmaftc,/home/main/lianmattc,/home/main/sxdp3,/home/main/wsdp3,/home/main/swdp3,/home/main/rydp3";
                            ViewBag.page_menu = "二全中,二中特,特串,三中二,三全中";
                            ViewBag.page_menu_url = "/home/main/lianmat,/home/main/lianmat2,/home/main/lianmattc,/home/main/lianmat3z2,/home/main/lianmat3z ";
                            ms = from q in m
                                 where (q.type == null)
                                 select new x9web.Models.xball
                                 {
                                     name = q.name.Trim(),
                                     color = q.color,
                                     tma = q.stc
                                 };
                            return View("lianma-f", ms);
                        case "lianmaf3z2":
                            ViewBag.script_id = 8024;
                            ViewBag.menuname = "連碼";
                            ViewBag.page_name = "三中二";
                            ViewBag.page_menu_active = "三中二";
                            ViewBag.page_menu_active2 = "复式";
                            ViewBag.page_menu2 = "复式,拖头,生肖对碰,尾数对碰,生尾对碰,任意对碰";
                            ViewBag.page_menu2_url = "/home/main/lianmaf3z2,/home/main/lianmat3z2,/home/main/sxdp,/home/main/wsdp,/home/main/swdp,/home/main/rydp";
                            ViewBag.page_menu = "二全中,二中特,特串,三中二,三全中";
                            ViewBag.page_menu_url = "/home/main/lianmaf,/home/main/lianmaf2,/home/main/lianmaftc,/home/main/lianmaf3z2,/home/main/lianmaf3z ";
                            ms = from q in m
                                 where (q.type == null)
                                 select new x9web.Models.xball
                                 {
                                     name = q.name.Trim(),
                                     color = q.color,
                                     tma = q.s3z2,
                                     tmb = q.s3z22
                                 };
                            return View("lianma-f4", ms);
                        case "lianmat3z2":
                            ViewBag.script_id = 8024;
                            ViewBag.menuname = "連碼";
                            ViewBag.page_name = "三中二";
                            ViewBag.page_menu_active = "三中二";
                            ViewBag.page_menu_active2 = "拖头";
                            ViewBag.page_menu2 = "复式,拖头,生肖对碰,尾数对碰,生尾对碰,任意对碰";
                            ViewBag.page_menu2_url = "/home/main/lianmaf3z2,/home/main/lianmat3z2,/home/main/sxdp,/home/main/wsdp,/home/main/swdp,/home/main/rydp";
                            ViewBag.page_menu = "二全中,二中特,特串,三中二,三全中";
                            ViewBag.page_menu_url = "/home/main/lianmat,/home/main/lianmat2,/home/main/lianmattc,/home/main/lianmat3z2,/home/main/lianmat3z ";
                            ms = from q in m
                                 where (q.type == null)
                                 select new x9web.Models.xball
                                 {
                                     name = q.name.Trim(),
                                     color = q.color,
                                     tma = q.s3z2,
                                     tmb = q.s3z22
                                 };
                            return View("lianma-f4", ms);
                        case "lianmaf3z":
                            ViewBag.script_id = 8025;
                            ViewBag.menuname = "連碼";
                            ViewBag.page_name = "三全中";
                            ViewBag.page_menu_active = "三全中";
                            ViewBag.page_menu_active2 = "复式";
                            ViewBag.page_menu2 = "复式,拖头,生肖对碰,尾数对碰,生尾对碰,任意对碰";
                            ViewBag.page_menu2_url = "/home/main/lianmaf3z,/home/main/lianmat3z,/home/main/sxdp,/home/main/wsdp,/home/main/swdp,/home/main/rydp";
                            ViewBag.page_menu = "二全中,二中特,特串,三中二,三全中";
                            ViewBag.page_menu_url = "/home/main/lianmaf,/home/main/lianmaf2,/home/main/lianmaftc,/home/main/lianmaf3z2,/home/main/lianmaf3z ";
                            ms = from q in m
                                 where (q.type == null)
                                 select new x9web.Models.xball
                                 {
                                     name = q.name.Trim(),
                                     color = q.color,
                                     tma = q.s3qz
                                 };
                            return View("lianma-f2", ms);
                        case "lianmat3z":
                            ViewBag.script_id = 8025;
                            ViewBag.menuname = "連碼";
                            ViewBag.page_name = "三全中";
                            ViewBag.page_menu_active = "三全中";
                            ViewBag.page_menu_active2 = "拖头";
                            ViewBag.page_menu2 = "复式,拖头,生肖对碰,尾数对碰,生尾对碰,任意对碰";
                            ViewBag.page_menu2_url = "/home/main/lianmaf3z2,/home/main/lianmat3z2,/home/main/sxdp,/home/main/wsdp,/home/main/swdp,/home/main/rydp";
                            ViewBag.page_menu = "二全中,二中特,特串,三中二,三全中";
                            ViewBag.page_menu_url = "/home/main/lianmat,/home/main/lianmat2,/home/main/lianmattc,/home/main/lianmat3z2,/home/main/lianmat3z ";
                            ms = from q in m
                                 where (q.type == null)
                                 select new x9web.Models.xball
                                 {
                                     name = q.name.Trim(),
                                     color = q.color,
                                     tma = q.s3qz
                                 };
                            return View("lianma-f2", ms);
                        case "wsdp":
                            ViewBag.script_id = 8021;
                            ViewBag.menuname = "連碼";
                            ViewBag.page_name = "二全中";
                            ViewBag.page_menu_active = "二全中";
                            ViewBag.page_menu_active2 = "尾数对碰";
                            ViewBag.page_menu2 = "复式,拖头,生肖对碰,尾数对碰,生尾对碰,任意对碰";
                            ViewBag.page_menu2_url = "/home/main/lianmaf,/home/main/lianmat,/home/main/sxdp,/home/main/wsdp,/home/main/swdp,/home/main/rydp";
                            ViewBag.page_menu = "二全中,二中特,特串";
                            ViewBag.page_menu_url = "/home/main/wsdp,/home/main/wsdp2,/home/main/wsdp3";
                            ms = from q in m
                                 where (q.type == null)
                                 select new x9web.Models.xball
                                 {
                                     name = q.name.Trim(),
                                     color = q.color,
                                     tma = q.s2qz
                                 };
                            return View("lianma_wsdp", ms);
                        case "wsdp2":
                            ViewBag.script_id = 8022;
                            ViewBag.menuname = "連碼";
                            ViewBag.page_name = "二中特";
                            ViewBag.page_menu_active = "二中特";
                            ViewBag.page_menu_active2 = "尾数对碰";
                            ViewBag.page_menu2 = "复式,拖头,生肖对碰,尾数对碰,生尾对碰,任意对碰";
                            ViewBag.page_menu2_url = "/home/main/lianmaf2,/home/main/lianmat2,/home/main/sxdp2,/home/main/wsdp2,/home/main/swdp2,/home/main/rydp2";
                            ViewBag.page_menu = "二全中,二中特,特串";
                            ViewBag.page_menu_url = "/home/main/wsdp,/home/main/wsdp2,/home/main/wsdp3";
                            ms = from q in m
                                 where (q.type == null)
                                 select new x9web.Models.xball
                                 {
                                     name = q.name.Trim(),
                                     color = q.color,
                                     tma = q.s2zt,
                                     tmb = q.s2zt2
                                 };
                            return View("lianma_wsdp", ms);
                        case "wsdp3":
                            ViewBag.script_id = 8023;
                            ViewBag.menuname = "連碼";
                            ViewBag.page_name = "特串";
                            ViewBag.page_menu_active = "特串";
                            ViewBag.page_menu_active2 = "尾数对碰";
                            ViewBag.page_menu2 = "复式,拖头,生肖对碰,尾数对碰,生尾对碰,任意对碰";
                            ViewBag.page_menu2_url = "/home/main/lianmaftc,/home/main/lianmattc,/home/main/sxdp3,/home/main/wsdp3,/home/main/swdp3,/home/main/rydp3";
                            ViewBag.page_menu = "二全中,二中特,特串";
                            ViewBag.page_menu_url = "/home/main/wsdp,/home/main/wsdp2,/home/main/wsdp3";
                            ms = from q in m
                                 where (q.type == null)
                                 select new x9web.Models.xball
                                 {
                                     name = q.name.Trim(),
                                     color = q.color,
                                     tma = q.stc
                                 };
                            return View("lianma_wsdp", ms);
                        case "sxdp":
                            ViewBag.script_id = 8021;
                            ViewBag.menuname = "連碼";
                            ViewBag.page_name = "二全中";
                            ViewBag.page_menu_active = "二全中";
                            ViewBag.page_menu_active2 = "生肖对碰";
                            ViewBag.page_menu2 = "复式,拖头,生肖对碰,尾数对碰,生尾对碰,任意对碰";
                            ViewBag.page_menu2_url = "/home/main/lianmaf,/home/main/lianmat,/home/main/sxdp,/home/main/wsdp,/home/main/swdp,/home/main/rydp";
                            ViewBag.page_menu = "二全中,二中特,特串";
                            ViewBag.page_menu_url = "/home/main/sxdp,/home/main/sxdp2,/home/main/sxdp3";
                            ms = from q in m
                                 where (q.type == null)
                                 select new x9web.Models.xball
                                 {
                                     name = q.name.Trim(),
                                     color = q.color,
                                     shenxiao = q.shenxiao,
                                     tma = q.s2qz
                                 };
                            return View("lianma_sxdp", ms);
                        case "sxdp2":
                            ViewBag.script_id = 8022;
                            ViewBag.menuname = "連碼";
                            ViewBag.page_name = "二中特";
                            ViewBag.page_menu_active = "二中特";
                            ViewBag.page_menu_active2 = "生肖对碰";
                            ViewBag.page_menu2 = "复式,拖头,生肖对碰,尾数对碰,生尾对碰,任意对碰";
                            ViewBag.page_menu2_url = "/home/main/lianmaf2,/home/main/lianmat2,/home/main/sxdp2,/home/main/wsdp2,/home/main/swdp2,/home/main/rydp2";
                            ViewBag.page_menu = "二全中,二中特,特串";
                            ViewBag.page_menu_url = "/home/main/sxdp,/home/main/sxdp2,/home/main/sxdp3";
                            ms = from q in m
                                 where (q.type == null)
                                 select new x9web.Models.xball
                                 {
                                     name = q.name.Trim(),
                                     color = q.color,
                                     shenxiao = q.shenxiao,
                                     tma = q.s2zt,
                                     tmb = q.s2zt2
                                 };
                            return View("lianma_sxdp", ms);
                        case "sxdp3":
                            ViewBag.script_id = 8023;
                            ViewBag.menuname = "連碼";
                            ViewBag.page_name = "特串";
                            ViewBag.page_menu_active = "特串";
                            ViewBag.page_menu_active2 = "生肖对碰";
                            ViewBag.page_menu2 = "复式,拖头,生肖对碰,尾数对碰,生尾对碰,任意对碰";
                            ViewBag.page_menu2_url = "/home/main/lianmaftc,/home/main/lianmattc,/home/main/sxdp3,/home/main/wsdp3,/home/main/swdp3,/home/main/rydp3";
                            ViewBag.page_menu = "二全中,二中特,特串";
                            ViewBag.page_menu_url = "/home/main/sxdp,/home/main/sxdp2,/home/main/sxdp3";
                            ms = from q in m
                                 where (q.type == null)
                                 select new x9web.Models.xball
                                 {
                                     name = q.name.Trim(),
                                     color = q.color,
                                     shenxiao = q.shenxiao,
                                     tma = q.stc
                                 };
                            return View("lianma_sxdp", ms);
                        case "swdp":
                            ViewBag.script_id = 8021;
                            ViewBag.menuname = "連碼";
                            ViewBag.page_name = "二全中";
                            ViewBag.page_menu_active = "二全中";
                            ViewBag.page_menu_active2 = "生尾对碰";
                            ViewBag.page_menu2 = "复式,拖头,生肖对碰,尾数对碰,生尾对碰,任意对碰";
                            ViewBag.page_menu2_url = "/home/main/lianmaf,/home/main/lianmat,/home/main/sxdp,/home/main/wsdp,/home/main/swdp,/home/main/rydp";
                            ViewBag.page_menu = "二全中,二中特,特串";
                            ViewBag.page_menu_url = "/home/main/swdp,/home/main/swdp2,/home/main/swdp3";
                            ms = from q in m
                                 where (q.type == null)
                                 select new x9web.Models.xball
                                 {
                                     name = q.name.Trim(),
                                     color = q.color,
                                     shenxiao = q.shenxiao,
                                     tma = q.s2qz
                                 };
                            return View("lianma_swdp", ms);
                        case "swdp2":
                            ViewBag.script_id = 8022;
                            ViewBag.menuname = "連碼";
                            ViewBag.page_name = "二中特";
                            ViewBag.page_menu_active = "二中特";
                            ViewBag.page_menu_active2 = "生尾对碰";
                            ViewBag.page_menu2 = "复式,拖头,生肖对碰,尾数对碰,生尾对碰,任意对碰";
                            ViewBag.page_menu2_url = "/home/main/lianmaf2,/home/main/lianmat2,/home/main/sxdp2,/home/main/wsdp2,/home/main/swdp2,/home/main/rydp2";
                            ViewBag.page_menu = "二全中,二中特,特串";
                            ViewBag.page_menu_url = "/home/main/swdp,/home/main/swdp2,/home/main/swdp3";
                            ms = from q in m
                                 where (q.type == null)
                                 select new x9web.Models.xball
                                 {
                                     name = q.name.Trim(),
                                     color = q.color,
                                     shenxiao = q.shenxiao,
                                     tma = q.s2zt,
                                     tmb = q.s2zt2
                                 };
                            return View("lianma_swdp", ms);
                        case "swdp3":
                            ViewBag.script_id = 8023;
                            ViewBag.menuname = "連碼";
                            ViewBag.page_name = "特串";
                            ViewBag.page_menu_active = "特串";
                            ViewBag.page_menu_active2 = "生尾对碰";
                            ViewBag.page_menu2 = "复式,拖头,生肖对碰,尾数对碰,生尾对碰,任意对碰";
                            ViewBag.page_menu2_url = "/home/main/lianmaftc,/home/main/lianmattc,/home/main/sxdp3,/home/main/wsdp3,/home/main/swdp3,/home/main/rydp3";
                            ViewBag.page_menu = "二全中,二中特,特串";
                            ViewBag.page_menu_url = "/home/main/swdp,/home/main/swdp2,/home/main/swdp3";
                            ms = from q in m
                                 where (q.type == null)
                                 select new x9web.Models.xball
                                 {
                                     name = q.name.Trim(),
                                     color = q.color,
                                     shenxiao = q.shenxiao,
                                     tma = q.stc
                                 };
                            return View("lianma_swdp", ms);
                        case "rydp":
                            ViewBag.script_id = 8021;
                            ViewBag.menuname = "連碼";
                            ViewBag.page_name = "二全中";
                            ViewBag.page_menu_active = "二全中";
                            ViewBag.page_menu_active2 = "任意对碰";
                            ViewBag.page_menu2 = "复式,拖头,生肖对碰,尾数对碰,生尾对碰,任意对碰";
                            ViewBag.page_menu2_url = "/home/main/lianmaf,/home/main/lianmat,/home/main/sxdp,/home/main/wsdp,/home/main/swdp,/home/main/rydp";
                            ViewBag.page_menu = "二全中,二中特,特串";
                            ViewBag.page_menu_url = "/home/main/rydp,/home/main/rydp2,/home/main/rydp3";
                            ms = from q in m
                                 where (q.type == null)
                                 select new x9web.Models.xball
                                 {
                                     name = q.name.Trim(),
                                     color = q.color,
                                     shenxiao = q.shenxiao,
                                     tma = q.s2qz
                                 };
                            return View("lianma_rydp", ms);
                        case "rydp2":
                            ViewBag.script_id = 8022;
                            ViewBag.menuname = "連碼";
                            ViewBag.page_name = "二中特";
                            ViewBag.page_menu_active = "二中特";
                            ViewBag.page_menu_active2 = "任意对碰";
                            ViewBag.page_menu2 = "复式,拖头,生肖对碰,尾数对碰,生尾对碰,任意对碰";
                            ViewBag.page_menu2_url = "/home/main/lianmaf2,/home/main/lianmat2,/home/main/sxdp2,/home/main/wsdp2,/home/main/swdp2,/home/main/rydp2";
                            ViewBag.page_menu = "二全中,二中特,特串";
                            ViewBag.page_menu_url = "/home/main/rydp,/home/main/rydp2,/home/main/rydp3";
                            ms = from q in m
                                 where (q.type == null)
                                 select new x9web.Models.xball
                                 {
                                     name = q.name.Trim(),
                                     color = q.color,
                                     shenxiao = q.shenxiao,
                                     tma = q.s2zt,
                                     tmb = q.s2zt2
                                 };
                            return View("lianma_rydp", ms);
                        case "rydp3":
                            ViewBag.script_id = 8023;
                            ViewBag.menuname = "連碼";
                            ViewBag.page_name = "特串";
                            ViewBag.page_menu_active = "特串";
                            ViewBag.page_menu_active2 = "任意对碰";
                            ViewBag.page_menu2 = "复式,拖头,生肖对碰,尾数对碰,生尾对碰,任意对碰";
                            ViewBag.page_menu2_url = "/home/main/lianmaftc,/home/main/lianmattc,/home/main/sxdp3,/home/main/wsdp3,/home/main/swdp3,/home/main/rydp3";
                            ViewBag.page_menu = "二全中,二中特,特串";
                            ViewBag.page_menu_url = "/home/main/rydp,/home/main/rydp2,/home/main/rydp3";
                            ms = from q in m
                                 where (q.type == null)
                                 select new x9web.Models.xball
                                 {
                                     name = q.name.Trim(),
                                     color = q.color,
                                     shenxiao = q.shenxiao,
                                     tma = q.stc
                                 };
                            return View("lianma_rydp", ms);
                        #endregion
                        #region 特码项目
                        case "zoxx":
                            ViewBag.script_id = 8026;
                            ViewBag.menuname = "特碼項目";
                            ViewBag.page_name = "特肖";
                            ViewBag.page_menu_active = "特肖";
                            ViewBag.page_menu = "特肖,半波,五行";
                            ViewBag.page_menu_url = "/home/main/zoxx,/home/main/zoxx2,/home/main/n5xin";
                            ms = from q in m
                                 where (q.type == null || q.type == 814)
                                 select new x9web.Models.xball
                                 {
                                     name = q.name.Trim(),
                                     color = q.color,
                                     shenxiao = q.shenxiao,
                                     tma = q.tiex
                                 };
                            return View("zoxx", ms);
                        case "zoxx2":
                            ViewBag.script_id = 8027;
                            ViewBag.menuname = "特碼項目";
                            ViewBag.page_name = "半波";
                            ViewBag.page_menu_active = "半波";
                            ViewBag.page_menu = "特肖,半波,五行";
                            ViewBag.page_menu_url = "/home/main/zoxx,/home/main/zoxx2,/home/main/n5xin";
                            ms = from q in m
                                 where (q.type == 815)
                                 select new x9web.Models.xball
                                 {
                                     name = q.name.Trim(),
                                     color = q.color,
                                     shenxiao = q.shenxiao,
                                     tma = q.babo
                                 };
                            return View("zoxx2", ms);
                        case "n5xin":
                            ViewBag.script_id = 8069;
                            ViewBag.menuname = "特碼項目";
                            ViewBag.page_name = "五行";
                            ViewBag.page_menu_active = "五行";
                            ViewBag.page_menu = "特肖,半波,五行";
                            ViewBag.page_menu_url = "/home/main/zoxx,/home/main/zoxx2,/home/main/n5xin";
                            ms = from q in m
                                 where (q.type == 817 || q.type == null)
                                 select new x9web.Models.xball
                                 {
                                     name = q.name.Trim(),
                                     color = q.color,
                                     shenxiao = q.shenxiao,
                                     tma = q.n5xin,
                                     wuxin = q.wuxin,
                                     type = q.type
                                 };
                            return View("n5sw", ms);
                        #endregion
                        #region 生肖项目
                        case "zos":
                            ViewBag.script_id = 8028;
                            ViewBag.menuname = "生肖項目";
                            ViewBag.page_name = "一肖";
                            ViewBag.page_menu_active = "一肖";
                            ViewBag.page_menu = "一肖,一肖不中,六肖,六肖不中,生肖量";
                            ViewBag.page_menu_url = "/home/main/zos,/home/main/zos2,/home/main/ros,/home/main/ros2,/home/main/nsxl";
                            ms = from q in m
                                 where (q.type == null || q.type == 814)
                                 select new x9web.Models.xball
                                 {
                                     name = q.name.Trim(),
                                     color = q.color,
                                     shenxiao = q.shenxiao,
                                     tma = q.yix
                                 };
                            return View("zoxx", ms);
                        case "zos2":
                            ViewBag.script_id = 8029;
                            ViewBag.menuname = "生肖項目";
                            ViewBag.page_name = "一肖不中";
                            ViewBag.page_menu_active = "一肖不中";
                            ViewBag.page_menu = "一肖,一肖不中,六肖,六肖不中,生肖量";
                            ViewBag.page_menu_url = "/home/main/zos,/home/main/zos2,/home/main/ros,/home/main/ros2,/home/main/nsxl";
                            ms = from q in m
                                 where (q.type == null || q.type == 814)
                                 select new x9web.Models.xball
                                 {
                                     name = q.name.Trim(),
                                     color = q.color,
                                     shenxiao = q.shenxiao,
                                     tma = q.yixb
                                 };
                            return View("zoxx", ms);
                        case "ros":
                            ViewBag.script_id = 8030;
                            ViewBag.menuname = "生肖項目";
                            ViewBag.page_name = "六肖";
                            ViewBag.page_menu_active = "六肖";
                            ViewBag.page_menu = "一肖,一肖不中,六肖,六肖不中,生肖量";
                            ViewBag.page_menu_url = "/home/main/zos,/home/main/zos2,/home/main/ros,/home/main/ros2,/home/main/nsxl";
                            ms = from q in m
                                 where ((q.type == null || q.type == 814) && q.name.Trim() != "49")
                                 select new x9web.Models.xball
                                 {
                                     name = q.name.Trim(),
                                     color = q.color,
                                     shenxiao = q.shenxiao,
                                     type = q.type,
                                     tma = q.liux
                                 };
                            return View("zoxxl", ms);
                        case "ros2":
                            ViewBag.script_id = 8031;
                            ViewBag.menuname = "生肖項目";
                            ViewBag.page_name = "六肖不中";
                            ViewBag.page_menu_active = "六肖不中";
                            ViewBag.page_menu = "一肖,一肖不中,六肖,六肖不中,生肖量";
                            ViewBag.page_menu_url = "/home/main/zos,/home/main/zos2,/home/main/ros,/home/main/ros2,/home/main/nsxl";
                            ms = from q in m
                                 where ((q.type == null || q.type == 814) && q.name.Trim() != "49")
                                 select new x9web.Models.xball
                                 {
                                     name = q.name.Trim(),
                                     color = q.color,
                                     shenxiao = q.shenxiao,
                                     type = q.type,
                                     tma = q.liuxb
                                 };
                            return View("zoxxl", ms);
                        case "nsxl":
                            ViewBag.script_id = 8070;
                            ViewBag.menuname = "生肖項目";
                            ViewBag.page_name = "生肖量";
                            ViewBag.page_menu_active = "生肖量";
                            ViewBag.page_menu = "一肖,一肖不中,六肖,六肖不中,生肖量";
                            ViewBag.page_menu_url = "/home/main/zos,/home/main/zos2,/home/main/ros,/home/main/ros2,/home/main/nsxl";
                            ms = from q in m
                                 where (q.type == 818)
                                 select new x9web.Models.xball
                                 {
                                     name = q.name.Trim(),
                                     color = q.color,
                                     shenxiao = q.shenxiao,
                                     type = q.type,
                                     tma = q.nsxl
                                 };
                            return View("n6sw", ms);
                        #endregion
                        #region 总数项目
                        case "zoss":
                            ViewBag.script_id = 8032;
                            ViewBag.menuname = "總數項目";
                            ViewBag.page_name = "尾数";
                            ViewBag.page_menu_active = "尾数";
                            ViewBag.page_menu = "尾数,尾数不中,尾数量";
                            ViewBag.page_menu_url = "/home/main/zoss,/home/main/zoss2,/home/main/nwsl";
                            ms = from q in m
                                 where (q.type == null || q.type == 816)
                                 select new x9web.Models.xball
                                 {
                                     name = q.name.Trim(),
                                     color = q.color,
                                     shenxiao = q.shenxiao,
                                     type = q.type,
                                     tma = q.ws
                                 };
                            return View("zoss", ms);
                        case "zoss2":
                            ViewBag.script_id = 8033;
                            ViewBag.menuname = "總數項目";
                            ViewBag.page_name = "尾数不中";
                            ViewBag.page_menu_active = "尾数不中";
                            ViewBag.page_menu = "尾数,尾数不中,尾数量";
                            ViewBag.page_menu_url = "/home/main/zoss,/home/main/zoss2,/home/main/nwsl";
                            ms = from q in m
                                 where (q.type == null || q.type == 816)
                                 select new x9web.Models.xball
                                 {
                                     name = q.name.Trim(),
                                     color = q.color,
                                     shenxiao = q.shenxiao,
                                     type = q.type,
                                     tma = q.wsb
                                 };
                            return View("zoss", ms);
                        case "nwsl":
                            ViewBag.script_id = 8071;
                            ViewBag.menuname = "總數項目";
                            ViewBag.page_name = "尾数量";
                            ViewBag.page_menu_active = "尾数量";
                            ViewBag.page_menu = "尾数,尾数不中,尾数量";
                            ViewBag.page_menu_url = "/home/main/zoss,/home/main/zoss2,/home/main/nwsl";
                            ms = from q in m
                                 where (q.type == 819)
                                 select new x9web.Models.xball
                                 {
                                     name = q.name.Trim(),
                                     color = q.color,
                                     shenxiao = q.shenxiao,
                                     type = q.type,
                                     tma = q.nwsl
                                 };
                            return View("n6sw", ms);
                        #endregion
                        #region 多选不中
                        case "duoxbz":
                            ViewBag.script_id = 8034;
                            ViewBag.script_count = 5;
                            ViewBag.menuname = "多選不中";
                            ViewBag.page_name = "五不中";
                            ViewBag.page_menu_active = "五不中";
                            ViewBag.page_menu = "五不中,六不中,七不中,八不中,九不中,十不中,十二不中,十五不中";
                            ViewBag.page_menu_url = "/home/main/duoxbz,/home/main/duoxbz6,/home/main/duoxbz7,/home/main/duoxbz8,/home/main/duoxbz9,/home/main/duoxbz10,/home/main/duoxbz12,/home/main/duoxbz15";
                            ms = from q in m
                                 where (q.type == null)
                                 select new x9web.Models.xball
                                 {
                                     name = q.name.Trim(),
                                     color = q.color,
                                     shenxiao = q.shenxiao,
                                     type = q.type,
                                     tma = q.db5
                                 };
                            return View("duoxbz", ms);
                        case "duoxbz6":
                            ViewBag.script_id = 8035;
                            ViewBag.script_count = 6;
                            ViewBag.menuname = "多選不中";
                            ViewBag.page_name = "六不中";
                            ViewBag.page_menu_active = "六不中";
                            ViewBag.page_menu = "五不中,六不中,七不中,八不中,九不中,十不中,十二不中,十五不中";
                            ViewBag.page_menu_url = "/home/main/duoxbz,/home/main/duoxbz6,/home/main/duoxbz7,/home/main/duoxbz8,/home/main/duoxbz9,/home/main/duoxbz10,/home/main/duoxbz12,/home/main/duoxbz15";
                            ms = from q in m
                                 where (q.type == null)
                                 select new x9web.Models.xball
                                 {
                                     name = q.name.Trim(),
                                     color = q.color,
                                     shenxiao = q.shenxiao,
                                     type = q.type,
                                     tma = q.db6
                                 };
                            return View("duoxbz", ms);
                        case "duoxbz7":
                            ViewBag.script_id = 8036;
                            ViewBag.script_count = 7;
                            ViewBag.menuname = "多選不中";
                            ViewBag.page_name = "七不中";
                            ViewBag.page_menu_active = "七不中";
                            ViewBag.page_menu = "五不中,六不中,七不中,八不中,九不中,十不中,十二不中,十五不中";
                            ViewBag.page_menu_url = "/home/main/duoxbz,/home/main/duoxbz6,/home/main/duoxbz7,/home/main/duoxbz8,/home/main/duoxbz9,/home/main/duoxbz10,/home/main/duoxbz12,/home/main/duoxbz15";
                            ms = from q in m
                                 where (q.type == null)
                                 select new x9web.Models.xball
                                 {
                                     name = q.name.Trim(),
                                     color = q.color,
                                     shenxiao = q.shenxiao,
                                     type = q.type,
                                     tma = q.db7
                                 };
                            return View("duoxbz", ms);
                        case "duoxbz8":
                            ViewBag.script_id = 8037;
                            ViewBag.script_count = 8;
                            ViewBag.menuname = "多選不中";
                            ViewBag.page_name = "八不中";
                            ViewBag.page_menu_active = "八不中";
                            ViewBag.page_menu = "五不中,六不中,七不中,八不中,九不中,十不中,十二不中,十五不中";
                            ViewBag.page_menu_url = "/home/main/duoxbz,/home/main/duoxbz6,/home/main/duoxbz7,/home/main/duoxbz8,/home/main/duoxbz9,/home/main/duoxbz10,/home/main/duoxbz12,/home/main/duoxbz15";
                            ms = from q in m
                                 where (q.type == null)
                                 select new x9web.Models.xball
                                 {
                                     name = q.name.Trim(),
                                     color = q.color,
                                     shenxiao = q.shenxiao,
                                     type = q.type,
                                     tma = q.db8
                                 };
                            return View("duoxbz", ms);
                        case "duoxbz9":
                            ViewBag.script_id = 8038;
                            ViewBag.script_count = 9;
                            ViewBag.menuname = "多選不中";
                            ViewBag.page_name = "九不中";
                            ViewBag.page_menu_active = "九不中";
                            ViewBag.page_menu = "五不中,六不中,七不中,八不中,九不中,十不中,十二不中,十五不中";
                            ViewBag.page_menu_url = "/home/main/duoxbz,/home/main/duoxbz6,/home/main/duoxbz7,/home/main/duoxbz8,/home/main/duoxbz9,/home/main/duoxbz10,/home/main/duoxbz12,/home/main/duoxbz15";
                            ms = from q in m
                                 where (q.type == null)
                                 select new x9web.Models.xball
                                 {
                                     name = q.name.Trim(),
                                     color = q.color,
                                     shenxiao = q.shenxiao,
                                     type = q.type,
                                     tma = q.db9
                                 };
                            return View("duoxbz", ms);
                        case "duoxbz10":
                            ViewBag.script_id = 8039;
                            ViewBag.script_count = 10;
                            ViewBag.menuname = "多選不中";
                            ViewBag.page_name = "十不中";
                            ViewBag.page_menu_active = "十不中";
                            ViewBag.page_menu = "五不中,六不中,七不中,八不中,九不中,十不中,十二不中,十五不中";
                            ViewBag.page_menu_url = "/home/main/duoxbz,/home/main/duoxbz6,/home/main/duoxbz7,/home/main/duoxbz8,/home/main/duoxbz9,/home/main/duoxbz10,/home/main/duoxbz12,/home/main/duoxbz15";
                            ms = from q in m
                                 where (q.type == null)
                                 select new x9web.Models.xball
                                 {
                                     name = q.name.Trim(),
                                     color = q.color,
                                     shenxiao = q.shenxiao,
                                     type = q.type,
                                     tma = q.db10
                                 };
                            return View("duoxbz", ms);
                        case "duoxbz12":
                            ViewBag.script_id = 8067;
                            ViewBag.script_count = 12;
                            ViewBag.menuname = "多選不中";
                            ViewBag.page_name = "十二不中";
                            ViewBag.page_menu_active = "十二不中";
                            ViewBag.page_menu = "五不中,六不中,七不中,八不中,九不中,十不中,十二不中,十五不中";
                            ViewBag.page_menu_url = "/home/main/duoxbz,/home/main/duoxbz6,/home/main/duoxbz7,/home/main/duoxbz8,/home/main/duoxbz9,/home/main/duoxbz10,/home/main/duoxbz12,/home/main/duoxbz15";
                            ms = from q in m
                                 where (q.type == null)
                                 select new x9web.Models.xball
                                 {
                                     name = q.name.Trim(),
                                     color = q.color,
                                     shenxiao = q.shenxiao,
                                     type = q.type,
                                     tma = q.db12
                                 };
                            return View("duoxbz", ms);
                        case "duoxbz15":
                            ViewBag.script_id = 8068;
                            ViewBag.script_count = 15;
                            ViewBag.menuname = "多選不中";
                            ViewBag.page_name = "十五不中";
                            ViewBag.page_menu_active = "十五不中";
                            ViewBag.page_menu = "五不中,六不中,七不中,八不中,九不中,十不中,十二不中,十五不中";
                            ViewBag.page_menu_url = "/home/main/duoxbz,/home/main/duoxbz6,/home/main/duoxbz7,/home/main/duoxbz8,/home/main/duoxbz9,/home/main/duoxbz10,/home/main/duoxbz12,/home/main/duoxbz15";
                            ms = from q in m
                                 where (q.type == null)
                                 select new x9web.Models.xball
                                 {
                                     name = q.name.Trim(),
                                     color = q.color,
                                     shenxiao = q.shenxiao,
                                     type = q.type,
                                     tma = q.db15
                                 };
                            return View("duoxbz", ms);
                        #endregion
                        #region 生肖连
                        case "senxl":
                            ViewBag.script_id = 8040;
                            ViewBag.script_count = 2;
                            ViewBag.menuname = "生肖連";
                            ViewBag.page_name = "二肖中";
                            ViewBag.page_menu_active = "二肖中";
                            ViewBag.page_menu = "二肖中,三肖中,四肖中,五肖中,二肖不中,三肖不中,四肖不中,五肖不中";
                            ViewBag.page_menu_url = "/home/main/senxl,/home/main/senxl3,/home/main/senxl4,/home/main/senxl5,/home/main/senxlb,/home/main/senxlb3,/home/main/senxlb4,/home/main/senxlb5";
                            ms = from q in m
                                 where (q.type == null || q.type == 814)
                                 select new x9web.Models.xball
                                 {
                                     name = q.name.Trim(),
                                     color = q.color,
                                     shenxiao = q.shenxiao,
                                     type = q.type,
                                     tma = q.x2
                                 };
                            return View("senxl", ms);
                        case "senxl3":
                            ViewBag.script_id = 8041;
                            ViewBag.menuname = "生肖連";
                            ViewBag.script_count = 3;
                            ViewBag.page_name = "三肖中";
                            ViewBag.page_menu_active = "三肖中";
                            ViewBag.page_menu = "二肖中,三肖中,四肖中,五肖中,二肖不中,三肖不中,四肖不中,五肖不中";
                            ViewBag.page_menu_url = "/home/main/senxl,/home/main/senxl3,/home/main/senxl4,/home/main/senxl5,/home/main/senxlb,/home/main/senxlb3,/home/main/senxlb4,/home/main/senxlb5";
                            ms = from q in m
                                 where (q.type == null || q.type == 814)
                                 select new x9web.Models.xball
                                 {
                                     name = q.name.Trim(),
                                     color = q.color,
                                     shenxiao = q.shenxiao,
                                     type = q.type,
                                     tma = q.x3
                                 };
                            return View("senxl", ms);
                        case "senxl4":
                            ViewBag.script_id = 8042;
                            ViewBag.script_count = 4;
                            ViewBag.menuname = "生肖連";
                            ViewBag.page_name = "四肖中";
                            ViewBag.page_menu_active = "四肖中";
                            ViewBag.page_menu = "二肖中,三肖中,四肖中,五肖中,二肖不中,三肖不中,四肖不中,五肖不中";
                            ViewBag.page_menu_url = "/home/main/senxl,/home/main/senxl3,/home/main/senxl4,/home/main/senxl5,/home/main/senxlb,/home/main/senxlb3,/home/main/senxlb4,/home/main/senxlb5";
                            ms = from q in m
                                 where (q.type == null || q.type == 814)
                                 select new x9web.Models.xball
                                 {
                                     name = q.name.Trim(),
                                     color = q.color,
                                     shenxiao = q.shenxiao,
                                     type = q.type,
                                     tma = q.x4
                                 };
                            return View("senxl", ms);
                        case "senxl5":
                            ViewBag.script_id = 8043;
                            ViewBag.script_count = 5;
                            ViewBag.menuname = "生肖連";
                            ViewBag.page_name = "五肖中";
                            ViewBag.page_menu_active = "五肖中";
                            ViewBag.page_menu = "二肖中,三肖中,四肖中,五肖中,二肖不中,三肖不中,四肖不中,五肖不中";
                            ViewBag.page_menu_url = "/home/main/senxl,/home/main/senxl3,/home/main/senxl4,/home/main/senxl5,/home/main/senxlb,/home/main/senxlb3,/home/main/senxlb4,/home/main/senxlb5";
                            ms = from q in m
                                 where (q.type == null || q.type == 814)
                                 select new x9web.Models.xball
                                 {
                                     name = q.name.Trim(),
                                     color = q.color,
                                     shenxiao = q.shenxiao,
                                     type = q.type,
                                     tma = q.x5
                                 };
                            return View("senxl", ms);
                        case "senxlb":
                            ViewBag.script_id = 8044;
                            ViewBag.script_count = 2;
                            ViewBag.menuname = "生肖連";
                            ViewBag.page_name = "二肖不中";
                            ViewBag.page_menu_active = "二肖不中";
                            ViewBag.page_menu = "二肖中,三肖中,四肖中,五肖中,二肖不中,三肖不中,四肖不中,五肖不中";
                            ViewBag.page_menu_url = "/home/main/senxl,/home/main/senxl3,/home/main/senxl4,/home/main/senxl5,/home/main/senxlb,/home/main/senxlb3,/home/main/senxlb4,/home/main/senxlb5";
                            ms = from q in m
                                 where (q.type == null || q.type == 814)
                                 select new x9web.Models.xball
                                 {
                                     name = q.name.Trim(),
                                     color = q.color,
                                     shenxiao = q.shenxiao,
                                     type = q.type,
                                     tma = q.x2b
                                 };
                            return View("senxl", ms);
                        case "senxlb3":
                            ViewBag.script_id = 8045;
                            ViewBag.script_count = 3;
                            ViewBag.menuname = "生肖連";
                            ViewBag.page_name = "三肖不中";
                            ViewBag.page_menu_active = "三肖不中";
                            ViewBag.page_menu = "二肖中,三肖中,四肖中,五肖中,二肖不中,三肖不中,四肖不中,五肖不中";
                            ViewBag.page_menu_url = "/home/main/senxl,/home/main/senxl3,/home/main/senxl4,/home/main/senxl5,/home/main/senxlb,/home/main/senxlb3,/home/main/senxlb4,/home/main/senxlb5";
                            ms = from q in m
                                 where (q.type == null || q.type == 814)
                                 select new x9web.Models.xball
                                 {
                                     name = q.name.Trim(),
                                     color = q.color,
                                     shenxiao = q.shenxiao,
                                     type = q.type,
                                     tma = q.x3b
                                 };
                            return View("senxl", ms);
                        case "senxlb4":
                            ViewBag.script_id = 8046;
                            ViewBag.script_count = 4;
                            ViewBag.menuname = "生肖連";
                            ViewBag.page_name = "四肖不中";
                            ViewBag.page_menu_active = "四肖不中";
                            ViewBag.page_menu = "二肖中,三肖中,四肖中,五肖中,二肖不中,三肖不中,四肖不中,五肖不中";
                            ViewBag.page_menu_url = "/home/main/senxl,/home/main/senxl3,/home/main/senxl4,/home/main/senxl5,/home/main/senxlb,/home/main/senxlb3,/home/main/senxlb4,/home/main/senxlb5";
                            ms = from q in m
                                 where (q.type == null || q.type == 814)
                                 select new x9web.Models.xball
                                 {
                                     name = q.name.Trim(),
                                     color = q.color,
                                     shenxiao = q.shenxiao,
                                     type = q.type,
                                     tma = q.x4b
                                 };
                            return View("senxl", ms);
                        case "senxlb5":
                            ViewBag.script_id = 8047;
                            ViewBag.script_count = 5;
                            ViewBag.menuname = "生肖連";
                            ViewBag.page_name = "五肖不中";
                            ViewBag.page_menu_active = "五肖不中";
                            ViewBag.page_menu = "二肖中,三肖中,四肖中,五肖中,二肖不中,三肖不中,四肖不中,五肖不中";
                            ViewBag.page_menu_url = "/home/main/senxl,/home/main/senxl3,/home/main/senxl4,/home/main/senxl5,/home/main/senxlb,/home/main/senxlb3,/home/main/senxlb4,/home/main/senxlb5";
                            ms = from q in m
                                 where (q.type == null || q.type == 814)
                                 select new x9web.Models.xball
                                 {
                                     name = q.name.Trim(),
                                     color = q.color,
                                     shenxiao = q.shenxiao,
                                     type = q.type,
                                     tma = q.x5b
                                 };
                            return View("senxl", ms);
                        #endregion
                        #region 尾数连
                        case "wensl":
                            ViewBag.script_id = 8048;
                            ViewBag.script_count = 2;
                            ViewBag.menuname = "尾數連";
                            ViewBag.page_name = "二尾中";
                            ViewBag.page_menu_active = "二尾中";
                            ViewBag.page_menu = "二尾中,三尾中,四尾中,五尾中,二尾不中,三尾不中,四尾不中,五尾不中";
                            ViewBag.page_menu_url = "/home/main/wensl,/home/main/wensl3,/home/main/wensl4,/home/main/wensl5,/home/main/wenslb,/home/main/wensl3b,/home/main/wensl4b,/home/main/wensl5b";
                            ms = from q in m
                                 where (q.type == null || q.type == 816)
                                 select new x9web.Models.xball
                                 {
                                     name = q.name.Trim(),
                                     color = q.color,
                                     shenxiao = q.shenxiao,
                                     type = q.type,
                                     tma = q.w2
                                 };
                            return View("wensl", ms);
                        case "wensl3":
                            ViewBag.script_id = 8049;
                            ViewBag.script_count = 3;
                            ViewBag.menuname = "尾數連";
                            ViewBag.page_name = "三尾中";
                            ViewBag.page_menu_active = "三尾中";
                            ViewBag.page_menu = "二尾中,三尾中,四尾中,五尾中,二尾不中,三尾不中,四尾不中,五尾不中";
                            ViewBag.page_menu_url = "/home/main/wensl,/home/main/wensl3,/home/main/wensl4,/home/main/wensl5,/home/main/wenslb,/home/main/wensl3b,/home/main/wensl4b,/home/main/wensl5b";
                            ms = from q in m
                                 where (q.type == null || q.type == 816)
                                 select new x9web.Models.xball
                                 {
                                     name = q.name.Trim(),
                                     color = q.color,
                                     shenxiao = q.shenxiao,
                                     type = q.type,
                                     tma = q.w3
                                 };
                            return View("wensl", ms);
                        case "wensl4":
                            ViewBag.script_id = 8050;
                            ViewBag.script_count = 4;
                            ViewBag.menuname = "尾數連";
                            ViewBag.page_name = "四尾中";
                            ViewBag.page_menu_active = "四尾中";
                            ViewBag.page_menu = "二尾中,三尾中,四尾中,五尾中,二尾不中,三尾不中,四尾不中,五尾不中";
                            ViewBag.page_menu_url = "/home/main/wensl,/home/main/wensl3,/home/main/wensl4,/home/main/wensl5,/home/main/wenslb,/home/main/wensl3b,/home/main/wensl4b,/home/main/wensl5b";
                            ms = from q in m
                                 where (q.type == null || q.type == 816)
                                 select new x9web.Models.xball
                                 {
                                     name = q.name.Trim(),
                                     color = q.color,
                                     shenxiao = q.shenxiao,
                                     type = q.type,
                                     tma = q.w4
                                 };
                            return View("wensl", ms);
                        case "wensl5":
                            ViewBag.script_id = 8065;
                            ViewBag.script_count = 5;
                            ViewBag.menuname = "尾數連";
                            ViewBag.page_name = "五尾中";
                            ViewBag.page_menu_active = "五尾中";
                            ViewBag.page_menu = "二尾中,三尾中,四尾中,五尾中,二尾不中,三尾不中,四尾不中,五尾不中";
                            ViewBag.page_menu_url = "/home/main/wensl,/home/main/wensl3,/home/main/wensl4,/home/main/wensl5,/home/main/wenslb,/home/main/wensl3b,/home/main/wensl4b,/home/main/wensl5b";
                            ms = from q in m
                                 where (q.type == null || q.type == 816)
                                 select new x9web.Models.xball
                                 {
                                     name = q.name.Trim(),
                                     color = q.color,
                                     shenxiao = q.shenxiao,
                                     type = q.type,
                                     tma = q.w5
                                 };
                            return View("wensl", ms);
                        case "wenslb":
                            ViewBag.script_id = 8051;
                            ViewBag.script_count = 2;
                            ViewBag.menuname = "尾數連";
                            ViewBag.page_name = "二尾不中";
                            ViewBag.page_menu_active = "二尾不中";
                            ViewBag.page_menu = "二尾中,三尾中,四尾中,五尾中,二尾不中,三尾不中,四尾不中,五尾不中";
                            ViewBag.page_menu_url = "/home/main/wensl,/home/main/wensl3,/home/main/wensl4,/home/main/wensl5,/home/main/wenslb,/home/main/wensl3b,/home/main/wensl4b,/home/main/wensl5b";
                            ms = from q in m
                                 where (q.type == null || q.type == 816)
                                 select new x9web.Models.xball
                                 {
                                     name = q.name.Trim(),
                                     color = q.color,
                                     shenxiao = q.shenxiao,
                                     type = q.type,
                                     tma = q.w2b
                                 };
                            return View("wensl", ms);
                        case "wensl3b":
                            ViewBag.script_id = 8052;
                            ViewBag.script_count = 3;
                            ViewBag.menuname = "尾數連";
                            ViewBag.page_name = "三尾不中";
                            ViewBag.page_menu_active = "三尾不中";
                            ViewBag.page_menu = "二尾中,三尾中,四尾中,五尾中,二尾不中,三尾不中,四尾不中,五尾不中";
                            ViewBag.page_menu_url = "/home/main/wensl,/home/main/wensl3,/home/main/wensl4,/home/main/wensl5,/home/main/wenslb,/home/main/wensl3b,/home/main/wensl4b,/home/main/wensl5b";
                            ms = from q in m
                                 where (q.type == null || q.type == 816)
                                 select new x9web.Models.xball
                                 {
                                     name = q.name.Trim(),
                                     color = q.color,
                                     shenxiao = q.shenxiao,
                                     type = q.type,
                                     tma = q.w3b
                                 };
                            return View("wensl", ms);
                        case "wensl4b":
                            ViewBag.script_id = 8053;
                            ViewBag.script_count = 4;
                            ViewBag.menuname = "尾數連";
                            ViewBag.page_name = "四尾不中";
                            ViewBag.page_menu_active = "四尾不中";
                            ViewBag.page_menu = "二尾中,三尾中,四尾中,五尾中,二尾不中,三尾不中,四尾不中,五尾不中";
                            ViewBag.page_menu_url = "/home/main/wensl,/home/main/wensl3,/home/main/wensl4,/home/main/wensl5,/home/main/wenslb,/home/main/wensl3b,/home/main/wensl4b,/home/main/wensl5b";
                            ms = from q in m
                                 where (q.type == null || q.type == 816)
                                 select new x9web.Models.xball
                                 {
                                     name = q.name.Trim(),
                                     color = q.color,
                                     shenxiao = q.shenxiao,
                                     type = q.type,
                                     tma = q.w4b
                                 };
                            return View("wensl", ms);
                        case "wensl5b":
                            ViewBag.script_id = 8066;
                            ViewBag.script_count = 5;
                            ViewBag.menuname = "尾數連";
                            ViewBag.page_name = "五尾不中";
                            ViewBag.page_menu_active = "五尾不中";
                            ViewBag.page_menu = "二尾中,三尾中,四尾中,五尾中,二尾不中,三尾不中,四尾不中,五尾不中";
                            ViewBag.page_menu_url = "/home/main/wensl,/home/main/wensl3,/home/main/wensl4,/home/main/wensl5,/home/main/wenslb,/home/main/wensl3b,/home/main/wensl4b,/home/main/wensl5b";
                            ms = from q in m
                                 where (q.type == null || q.type == 816)
                                 select new x9web.Models.xball
                                 {
                                     name = q.name.Trim(),
                                     color = q.color,
                                     shenxiao = q.shenxiao,
                                     type = q.type,
                                     tma = q.w5b
                                 };
                            return View("wensl", ms);
                        #endregion
                        #region 多选中一
                        case "duoxzy":
                            ViewBag.script_id = 8054;
                            ViewBag.script_count = 5;
                            ViewBag.menuname = "多選中一";
                            ViewBag.page_name = "五中一";
                            ViewBag.page_menu_active = "五中一";
                            ViewBag.page_menu = "五中一,六中一,七中一,八中一,九中一,十中一";
                            ViewBag.page_menu_url = "/home/main/duoxzy,/home/main/duoxzy6,/home/main/duoxzy7,/home/main/duoxzy8,/home/main/duoxzy9,/home/main/duoxzy10";
                            ms = from q in m
                                 where (q.type == null)
                                 select new x9web.Models.xball
                                 {
                                     name = q.name.Trim(),
                                     color = q.color,
                                     shenxiao = q.shenxiao,
                                     type = q.type,
                                     tma = q.dz5
                                 };
                            return View("duoxzy", ms);
                        case "duoxzy6":
                            ViewBag.script_id = 8055;
                            ViewBag.script_count = 6;
                            ViewBag.menuname = "多選中一";
                            ViewBag.page_name = "六中一";
                            ViewBag.page_menu_active = "六中一";
                            ViewBag.page_menu = "五中一,六中一,七中一,八中一,九中一,十中一";
                            ViewBag.page_menu_url = "/home/main/duoxzy,/home/main/duoxzy6,/home/main/duoxzy7,/home/main/duoxzy8,/home/main/duoxzy9,/home/main/duoxzy10";
                            ms = from q in m
                                 where (q.type == null)
                                 select new x9web.Models.xball
                                 {
                                     name = q.name.Trim(),
                                     color = q.color,
                                     shenxiao = q.shenxiao,
                                     type = q.type,
                                     tma = q.dz6
                                 };
                            return View("duoxzy", ms);
                        case "duoxzy7":
                            ViewBag.script_id = 8056;
                            ViewBag.script_count = 7;
                            ViewBag.menuname = "多選中一";
                            ViewBag.page_name = "七中一";
                            ViewBag.page_menu_active = "七中一";
                            ViewBag.page_menu = "五中一,六中一,七中一,八中一,九中一,十中一";
                            ViewBag.page_menu_url = "/home/main/duoxzy,/home/main/duoxzy6,/home/main/duoxzy7,/home/main/duoxzy8,/home/main/duoxzy9,/home/main/duoxzy10";
                            ms = from q in m
                                 where (q.type == null)
                                 select new x9web.Models.xball
                                 {
                                     name = q.name.Trim(),
                                     color = q.color,
                                     shenxiao = q.shenxiao,
                                     type = q.type,
                                     tma = q.dz7
                                 };
                            return View("duoxzy", ms);
                        case "duoxzy8":
                            ViewBag.script_id = 8057;
                            ViewBag.script_count = 8;
                            ViewBag.menuname = "多選中一";
                            ViewBag.page_name = "八中一";
                            ViewBag.page_menu_active = "八中一";
                            ViewBag.page_menu = "五中一,六中一,七中一,八中一,九中一,十中一";
                            ViewBag.page_menu_url = "/home/main/duoxzy,/home/main/duoxzy6,/home/main/duoxzy7,/home/main/duoxzy8,/home/main/duoxzy9,/home/main/duoxzy10";
                            ms = from q in m
                                 where (q.type == null)
                                 select new x9web.Models.xball
                                 {
                                     name = q.name.Trim(),
                                     color = q.color,
                                     shenxiao = q.shenxiao,
                                     type = q.type,
                                     tma = q.dz8
                                 };
                            return View("duoxzy", ms);
                        case "duoxzy9":
                            ViewBag.script_id = 8058;
                            ViewBag.script_count = 9;
                            ViewBag.menuname = "多選中一";
                            ViewBag.page_name = "九中一";
                            ViewBag.page_menu_active = "九中一";
                            ViewBag.page_menu = "五中一,六中一,七中一,八中一,九中一,十中一";
                            ViewBag.page_menu_url = "/home/main/duoxzy,/home/main/duoxzy6,/home/main/duoxzy7,/home/main/duoxzy8,/home/main/duoxzy9,/home/main/duoxzy10";
                            ms = from q in m
                                 where (q.type == null)
                                 select new x9web.Models.xball
                                 {
                                     name = q.name.Trim(),
                                     color = q.color,
                                     shenxiao = q.shenxiao,
                                     type = q.type,
                                     tma = q.dz9
                                 };
                            return View("duoxzy", ms);
                        case "duoxzy10":
                            ViewBag.script_id = 8059;
                            ViewBag.script_count = 10;
                            ViewBag.menuname = "多選中一";
                            ViewBag.page_name = "十中一";
                            ViewBag.page_menu_active = "十中一";
                            ViewBag.page_menu = "五中一,六中一,七中一,八中一,九中一,十中一";
                            ViewBag.page_menu_url = "/home/main/duoxzy,/home/main/duoxzy6,/home/main/duoxzy7,/home/main/duoxzy8,/home/main/duoxzy9,/home/main/duoxzy10";
                            ms = from q in m
                                 where (q.type == null)
                                 select new x9web.Models.xball
                                 {
                                     name = q.name.Trim(),
                                     color = q.color,
                                     shenxiao = q.shenxiao,
                                     type = q.type,
                                     tma = q.dz10
                                 };
                            return View("duoxzy", ms);
                        #endregion
                        #region 特平中
                        case "tepz":
                            ViewBag.script_id = 8060;
                            ViewBag.script_count = 1;
                            ViewBag.menuname = "特平中";
                            ViewBag.page_name = "一粒任中";
                            ViewBag.page_menu_active = "一粒任中";
                            ViewBag.page_menu = "一粒任中,二粒任中,三粒任中,四粒任中,五粒任中";
                            ViewBag.page_menu_url = "/home/main/tepz,/home/main/tepz2,/home/main/tepz3,/home/main/tepz4,/home/main/tepz5";
                            ms = from q in m
                                 where (q.type == null)
                                 select new x9web.Models.xball
                                 {
                                     name = q.name.Trim(),
                                     color = q.color,
                                     shenxiao = q.shenxiao,
                                     type = q.type,
                                     tma = q.rz1
                                 };
                            return View("tepz", ms);
                        case "tepz2":
                            ViewBag.script_id = 8061;
                            ViewBag.script_count = 2;
                            ViewBag.menuname = "特平中";
                            ViewBag.page_name = "二粒任中";
                            ViewBag.page_menu_active = "二粒任中";
                            ViewBag.page_menu = "一粒任中,二粒任中,三粒任中,四粒任中,五粒任中";
                            ViewBag.page_menu_url = "/home/main/tepz,/home/main/tepz2,/home/main/tepz3,/home/main/tepz4,/home/main/tepz5";
                            ms = from q in m
                                 where (q.type == null)
                                 select new x9web.Models.xball
                                 {
                                     name = q.name.Trim(),
                                     color = q.color,
                                     shenxiao = q.shenxiao,
                                     type = q.type,
                                     tma = q.rz2
                                 };
                            return View("tepz", ms);
                        case "tepz3":
                            ViewBag.script_id = 8062;
                            ViewBag.script_count = 3;
                            ViewBag.menuname = "特平中";
                            ViewBag.page_name = "三粒任中";
                            ViewBag.page_menu_active = "三粒任中";
                            ViewBag.page_menu = "一粒任中,二粒任中,三粒任中,四粒任中,五粒任中";
                            ViewBag.page_menu_url = "/home/main/tepz,/home/main/tepz2,/home/main/tepz3,/home/main/tepz4,/home/main/tepz5";
                            ms = from q in m
                                 where (q.type == null)
                                 select new x9web.Models.xball
                                 {
                                     name = q.name.Trim(),
                                     color = q.color,
                                     shenxiao = q.shenxiao,
                                     type = q.type,
                                     tma = q.rz3
                                 };
                            return View("tepz", ms);
                        case "tepz4":
                            ViewBag.script_id = 8063;
                            ViewBag.script_count = 4;
                            ViewBag.menuname = "特平中";
                            ViewBag.page_name = "四粒任中";
                            ViewBag.page_menu_active = "四粒任中";
                            ViewBag.page_menu = "一粒任中,二粒任中,三粒任中,四粒任中,五粒任中";
                            ViewBag.page_menu_url = "/home/main/tepz,/home/main/tepz2,/home/main/tepz3,/home/main/tepz4,/home/main/tepz5";
                            ms = from q in m
                                 where (q.type == null)
                                 select new x9web.Models.xball
                                 {
                                     name = q.name.Trim(),
                                     color = q.color,
                                     shenxiao = q.shenxiao,
                                     type = q.type,
                                     tma = q.rz4
                                 };
                            return View("tepz", ms);
                        case "tepz5":
                            ViewBag.script_id = 8064;
                            ViewBag.script_count = 5;
                            ViewBag.menuname = "特平中";
                            ViewBag.page_name = "五粒任中";
                            ViewBag.page_menu_active = "五粒任中";
                            ViewBag.page_menu = "一粒任中,二粒任中,三粒任中,四粒任中,五粒任中";
                            ViewBag.page_menu_url = "/home/main/tepz,/home/main/tepz2,/home/main/tepz3,/home/main/tepz4,/home/main/tepz5";
                            ms = from q in m
                                 where (q.type == null)
                                 select new x9web.Models.xball
                                 {
                                     name = q.name.Trim(),
                                     color = q.color,
                                     shenxiao = q.shenxiao,
                                     type = q.type,
                                     tma = q.rz5
                                 };
                            return View("tepz", ms);
                        #endregion

                        default:
                            ViewBag.menuname = "特碼";
                            ViewBag.script_id = 8011;
                            ViewBag.page_name = "特A";
                            ViewBag.page_menu_active = "A盘";
                            ViewBag.page_menu = "A盘,B盘";
                            ViewBag.page_menu_url = "/home/main/temaA,/home/main/temaB";
                            ViewBag.btn_kuais = "/home/main/temaAK";
                            ms = from q in m
                                 where (q.type == 811 || q.type == null)
                                 select new x9web.Models.xball
                                 {
                                     name = q.name.Trim(),
                                     color = q.color,
                                     tma = q.tma
                                 };
                            return View("teze_ma", ms);
                    }
                }
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }

        }

        public ActionResult pwdedit()
        {
            return View();
        }

        [HttpPost]
        public ActionResult pwdedit(string p1, string p2, string p3)
        {
            if (Session["name"] == null)
            {
                return Content("连接已断开，请重新登陆！");
            }
            if (p1 == string.Empty || p1 == "" || p1 == null)
            {
                return Json(new { type = "err", msg = "原密码不能为空！" });
            }
            if (p2 == string.Empty || p2 == "" || p2 == null)
            {
                return Json(new { type = "err", msg = "新密码不能为空！" });
            }
            if (p3 == string.Empty || p3 == "" || p3 == null)
            {
                return Json(new { type = "err", msg = "新密码不能为空！" });
            }
            if (p2 != p3)
            {
                return Json(new { type = "err", msg = "两次输入的密码不一致！" });
            }
            try
            {
                using (Models.Sqlconnect con = new Models.Sqlconnect())
                {
                    var m = con.xusers.Where(a => a.id == int.Parse(Session["id"].ToString())).Where(a => a.pwd == Api.WebClass.FunMD5(p1)).ToList();
                    if (m.Count < 1)
                    {
                        return Json(new { type = "err", msg = "输入的原密码错！" });
                    }
                    else
                    {
                        var f = from q in con.xusers
                                where (q.id == int.Parse(Session["id"].ToString()))
                                select q;
                        f.First().pwd = Api.WebClass.FunMD5(p2);
                        con.SubmitChanges();
                        return Json(new { type = "suc", msg = "密码修改成功！" });
                    }
                }
            }
            catch (Exception ex)
            {
                return Json(new { type = "err", msg = ex.Message });
            }

        }
        public ActionResult ur_touzu_list(int qisu, int id = 1)
        {
            try
            {
                if (Session["name"] == null)
                {
                    return Content("连接已断开，请重新登陆！<a href='/home/index'>返回</a>");
                }
                using (Models.Sqlconnect con = new Models.Sqlconnect())
                {
                    int uid = int.Parse(Session["id"].ToString());
                    var m = con.xxiazu.Where(a => a.qisu == qisu).Where(a => a.pid == uid).ToList();
                    var ms = from q in con.xxiazu
                             group q by new { q.qisu } into g
                             select new
                             {
                                 qisu = g.Key.qisu
                             };
                    var xqisu = "";
                    foreach (var mq in ms.ToList().OrderByDescending(a => a.qisu))
                    {
                        xqisu += mq.qisu.ToString();
                        xqisu += ",";
                    }
                    if (xqisu.Length > 0)
                    {
                        xqisu = xqisu.Substring(0, xqisu.Length - 1);
                    }
                    ViewBag.xqisu = xqisu;
                    ViewBag.qisu = qisu;
                    ViewBag.id = id;
                    return View(m);
                }
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }
        public ActionResult ur_touzu_list2(string arrtime)
        {
            try
            {
                if (Session["name"] == null)
                {
                    return Content("连接已断开，请重新登陆！<a href='/home/index'>返回</a>");
                }
                string t_arrtime = arrtime.Replace(">>>", ">");
                string[] arrtm = t_arrtime.Split('>');
                string s1 = arrtm[0].Trim() + " 00:00:00";
                string s2 = arrtm[1].Trim() + " 23:59:59";
                using (Models.Sqlconnect con = new Models.Sqlconnect())
                {
                    var m = con.xxiazu.Where(a => a.timer >= DateTime.Parse(s1)).Where(a => a.timer <= DateTime.Parse(s2)).Where(a => a.pid == int.Parse(Session["id"].ToString())).ToList();
                    return PartialView(m);
                }
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }
        public ActionResult ur_qisu_list()
        {
            try
            {
                using (Models.Sqlconnect con = new Models.Sqlconnect())
                {
                    var m = con.xxiazu.Where(a => a.pid == int.Parse(Session["id"].ToString())).ToList();
                    return View(m);
                }
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }
        public ActionResult utuisui()
        {
            return View();
        }
        public ActionResult help()
        {
            return View();
        }
        public ActionResult umember(int id = 1)
        {
            try
            {
                using (Models.Sqlconnect con = new Models.Sqlconnect())
                {
                    var m = con.xusers.Where(a => a.pid == int.Parse(Session["id"].ToString())).ToPagedList(id, 20);
                    return View(m);
                }
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }
        public ActionResult addnew_xuser()
        {
            return View();
        }
        public ActionResult xedit_xuser(int id)
        {
            try
            {
                using (Models.Sqlconnect con = new Models.Sqlconnect())
                {
                    var m = con.xtuisui.Where(a => a.pid == id).ToList();
                    ViewBag.usm = con.xusers.Where(a => a.id == id).First();
                    return View(m);
                }
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }
        public ActionResult liskaijiang(int id = 1)
        {
            try
            {
                if (Session["name"] == null)
                {
                    return Content("连接已断开，请重新登陆！<a href='/home/index'>返回</a>");
                }
                using (Models.Sqlconnect con = new Models.Sqlconnect())
                {
                    var m = con.xqisu.Where(a => a.jiang == true).OrderByDescending(a => a.id).ToPagedList(id, 20);
                    return View(m);
                }
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }
        public ActionResult huser_tuisui()
        {
            if (Session["name"] == null || Session["tsid"] == null)
            {
                return Content("连接已断开，请重新登陆！<a href='/home/index'>返回</a>");
            }

            try
            {
                using (Models.Sqlconnect con = new Models.Sqlconnect())
                {
                    var m = con.xqisu.ToList();
                    return View(m);
                }

            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }
        public ActionResult huser_tuisui_partial(int qisu, int id)
        {
            try
            {
                using (Models.Sqlconnect con = new Models.Sqlconnect())
                {
                    var m = con.xxiazu.Where(a => a.qisu == qisu).Where(a => a.pid == id).ToList();
                    return PartialView(m);
                }
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }

        public ActionResult uUser_swith(bool xbool, int id)
        {
            if (Session["id"] == null)
            {
                return Json(new { type = "err", msg = "已断开连接，请重新登陆！" });
            }
            try
            {
                using (Models.Sqlconnect con = new Models.Sqlconnect())
                {
                    var m = con.xusers.Where(a => a.id == id).First();
                    m.state = xbool;
                    con.SubmitChanges();
                    return Json(new { type = "suc" });
                }
            }
            catch (Exception ex)
            {
                return Json(new { type = "err", msg = ex.Message });
            }
        }
    }
}
