using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Webdiyer.WebControls.Mvc;

namespace x9web.Controllers
{
    public class ManageController : Controller
    {
        //
        // GET: /Manage/

        public ActionResult Index()
        {
            Session["xname"] = null;
            Session["xid"] = null;
            return View();
        }
        [HttpPost]
        public ActionResult Index(string name, string pwd, string code)
        {
            //name = "admin";
            //pwd = "2";
            //code = "1";

            if (name == string.Empty || pwd == string.Empty || code == string.Empty)
            {
                return Json(new { type = "err", msg = "帐号、密码、验证码不能为空！" });
            }
            if (code.ToUpper() != Session["code"].ToString())
            {
                return Json(new { type = "err", msg = "验证码不正确！" });
            }
            try
            {
                using (Models.Sqlconnect con = new Models.Sqlconnect())
                {
                    var m = con.xusers.Where(a => a.name == name).Where(a => a.pwd == Api.WebClass.FunMD5(pwd)).ToList();
                    if (m.Count < 1)
                    {
                        return Json(new { type = "err", msg = "帐号或密码错误！" });
                    }
                    else
                    {
                        Session["xname"] = name;
                        Session["xid"] = m.First().id;
                        Session["tsid"] = 0;
                        return Json(new { type = "suc" });
                    }

                }
            }
            catch (Exception ex)
            {
                return Json(new { type = "err", msg = ex.Message });
            }
        }
        public ActionResult Main(int id = 1)
        {
            if (Session["xname"] == null)
            {
                return Content("未登陆，或已断开连接，请重新登陆！<a href='/manage/index'>返回</a>");
            }
            try
            {
                using (Models.Sqlconnect con = new Models.Sqlconnect())
                {
                    var m = con.xqisu.Where(a => a.del == false).OrderByDescending(a => a.id).ToPagedList(id, 15);
                    return View(m);
                }
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }
        public ActionResult addnew_pankou()
        {
            if (Session["xname"] == null)
            {
                return Content("已断开连接，请重新登陆！");
            }
            return View();
        }
        public ActionResult kaijiang(int id)
        {
            if (Session["xname"] == null)
            {
                if (Session["xname"] == null)
                {
                    return Content("未登陆，或已断开连接，请重新登陆！");
                }
            }
            try
            {
                ViewBag.id = id;
                ViewBag.ball = "";
                using (Models.Sqlconnect con = new Models.Sqlconnect())
                {
                    var m = con.xqisu.Where(a => a.id == id).First();
                    if (m.ball != null)
                    {
                        ViewBag.ball = m.ball.Trim();
                    }
                }
                return View();
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }

        }
        public ActionResult edit_pankou(int id)
        {
            if (Session["xname"] == null)
            {
                if (Session["xname"] == null)
                {
                    return Content("未登陆，或已断开连接，请重新登陆！");
                }
            }
            try
            {
                using (Models.Sqlconnect con = new Models.Sqlconnect())
                {
                    var m = con.xqisu.Where(a => a.id == id).First();
                    if (m.jiang == true)
                    {
                        return Content("已开奖不能再进行修改！");
                    }
                    return View(m);
                }
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }
        public ActionResult xconfig()
        {
            if (Session["xname"] == null)
            {
                return Content("未登陆，或已断开连接，请重新登陆！<a href='/manage/index'>返回</a>");
            }
            try
            {
                using (Models.Sqlconnect con = new Models.Sqlconnect())
                {
                    var m = con.xconfig.First();
                    var mm = con.xball.Where(a => a.shenxiao != null).First();
                    var mu = con.xball.Where(a => a.color != null).ToList();
                    ViewBag.xball_model = mu;
                    ViewBag.sx = mm.shenxiao.ToString();
                    return View(m);
                }
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }
        public ActionResult member(int id = 1)
        {
            if (Session["xname"] == null)
            {
                return Content("未登陆，或已断开连接，请重新登陆！<a href='/manage/index'>返回</a>");
            }
            try
            {
                using (Models.Sqlconnect con = new Models.Sqlconnect())
                {
                    var m = con.xusers.Where(a => a.manage == false).Where(a => a.gf_state == false).ToPagedList(id, 10);
                    return View(m);
                }
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }
        public ActionResult member_d(int id = 1)
        {
            if (Session["xname"] == null)
            {
                return Content("未登陆，或已断开连接，请重新登陆！<a href='/manage/index'>返回</a>");
            }
            try
            {
                using (Models.Sqlconnect con = new Models.Sqlconnect())
                {
                    var m = con.xusers.Where(a => a.manage == false).Where(a => a.gf_state == true).ToPagedList(id, 10);
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
            if (Session["xname"] == null)
            {
                return Content("未登陆，或已断开连接，请重新登陆！<a href='/manage/index'>返回</a>");
            }
            try
            {
                using (Models.Sqlconnect con = new Models.Sqlconnect())
                {
                    var m = con.xtuisui.Where(a => a.pid == 0).OrderBy(a => a.xx).ToList();
                    return View(m);
                }
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }
        public ActionResult edit_xuser(int id)
        {
            if (id == 0 || id == null)
            {
                return Content("数据错误");
            }
            try
            {
                using (Models.Sqlconnect con = new Models.Sqlconnect())
                {
                    var mm = con.xusers.Where(a => a.id == id).Where(a => a.manage == false).Where(a => a.gf_state == false).First();
                    ViewBag.xuser = mm;
                    var m = con.xtuisui.Where(a => a.pid == id).ToList();
                    return View(m);
                }
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }
        public ActionResult edit_xuser2(int id)
        {
            if (Session["xname"] == null)
            {
                return Content("未登陆，或已断开连接，请重新登陆！<a href='/manage/index'>返回</a>");
            }
            if (id == 0 || id == null)
            {
                return Content("数据错误");
            }
            try
            {
                using (Models.Sqlconnect con = new Models.Sqlconnect())
                {
                    var mm = con.xusers.Where(a => a.id == id).Where(a => a.manage == false).Where(a => a.gf_state == true).First();
                    ViewBag.xuser = mm;
                    var m = con.xtuisui.Where(a => a.pid == id).ToList();
                    return View(m);
                }
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }
        public ActionResult tuisui()
        {
            if (Session["xname"] == null)
            {
                return Content("未登陆，或已断开连接，请重新登陆！<a href='/manage/index'>返回</a>");
            }
            try
            {
                using (Models.Sqlconnect con = new Models.Sqlconnect())
                {
                    var m = con.xtuisui.Where(a => a.pid == 0).OrderBy(a => a.xx).ToList();
                    return View(m);
                }
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }
        public ActionResult beilu(int id = 1)
        {
            try
            {
                if (Session["xname"] == null)
                {
                    return Content("未登陆，或已断开连接，请重新登陆！<a href='/manage/index'>返回</a>");
                }
                using (Models.Sqlconnect con = new Models.Sqlconnect())
                {
                    var m = con.xliubie.GroupBy(a => a.t1).ToList();
                    ViewBag.menu_z = m;
                    return View();
                }
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }
        public ActionResult beilu_default(int id = 1)
        {
            try
            {
                if (Session["xname"] == null)
                {
                    return Content("未登陆，或已断开连接，请重新登陆！<a href='/manage/index'>返回</a>");
                }
                using (Models.Sqlconnect con = new Models.Sqlconnect())
                {
                    var m = con.xliubie.GroupBy(a => a.t1).ToList();
                    ViewBag.menu_z = m;
                    return View();
                }
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }
        public ActionResult auto_beilu()
        {
            try
            {
                if (Session["xname"] == null)
                {
                    return Content("未登陆，或已断开连接，请重新登陆！<a href='/manage/index'>返回</a>");
                }
                using (Models.Sqlconnect con = new Models.Sqlconnect())
                {
                    var m = con.xauto_beilu.Where(a => a.on == true).ToList();
                    return View(m);
                }
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }
        public ActionResult marquee()
        {
            try
            {
                if (Session["xname"] == null)
                {
                    return Content("未登陆，或已断开连接，请重新登陆！<a href='/manage/index'>返回</a>");
                }
                using (Models.Sqlconnect con = new Models.Sqlconnect())
                {
                    var m = con.xmarquee.OrderByDescending(a => a.id).ToList();
                    return View(m);
                }
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }
        public ActionResult zhudan(int id = 0)
        {
            int qisu = 0;
            if (id == 0)
            {
                qisu = Api.WebClass.xconfig_qisu;
            }
            else
            {
                qisu = id;
            }
            try
            {
                if (Session["xname"] == null)
                {
                    return Content("未登陆，或已断开连接，请重新登陆！<a href='/manage/index'>返回</a>");
                }
                using (Models.Sqlconnect con = new Models.Sqlconnect())
                {
                    //var m = con.xxiazu.Where(a => a.qisu == qisu).ToList();
                    var mm = con.xqisu.Where(a => a.strat == true).OrderByDescending(a => a.id).ToList();
                    //var mmm = con.xusers.Where(a => a.manage == false).ToList();
                    //ViewBag.user_model = mmm;
                    ViewBag.qisu_model = mm;
                    ViewBag.qisu = qisu;
                    return View();
                }
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }
        public ActionResult zhudan2()
        {
            try
            {
                if (Session["xname"] == null)
                {
                    return Content("未登陆，或已断开连接，请重新登陆！<a href='/manage/index'>返回</a>");
                }
                using (Models.Sqlconnect con = new Models.Sqlconnect())
                {
                    var m = con.xxiazu.Where(a => a.qisu == Api.WebClass.xconfig_qisu).ToList();
                    var mm = con.xqisu.Where(a => a.strat == true).OrderByDescending(a => a.id).ToList();
                    ViewBag.qisu_model = mm;
                    return View(m);
                }
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }
        public ActionResult manage_day_form()
        {
            try
            {
                if (Session["xname"] == null)
                {
                    return Content("未登陆，或已断开连接，请重新登陆！<a href='/manage/index'>返回</a>");
                }
                using (Models.Sqlconnect con = new Models.Sqlconnect())
                {
                    var mm = con.xqisu.Where(a => a.strat == true).OrderByDescending(a => a.id).ToList();
                    ViewBag.qisu_model = mm;
                    return View();
                }
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }
        public ActionResult manage_caplook()
        {
            if (Session["xname"] == null)
            {
                return Content("未登陆，或已断开连接，请重新登陆！<a href='/manage/index'>返回</a>");
            }
            try
            {
                var models = new Models.um_man_cplook();
                using (Models.Sqlconnect con = new Models.Sqlconnect())
                {
                    models.xball = (from q in con.xball
                                    where (q.color != null || q.type == 811)
                                    select q).ToList();
                    models.xiazu = con.xxiazu.Where(a => a.qisu == Api.WebClass.xconfig_qisu).ToList();
                    models.xqisu = con.xqisu.OrderByDescending(a => a.id).Take(20).ToList();
                    return View(models);
                }
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }
        public ActionResult manage_caplook_partial(int qisu, string xm)
        {
            try
            {
                if (Session["xname"] == null)
                {
                    return Content("未登陆，或已断开连接，请重新登陆！");
                }
                var models = new Models.um_man_cplook();
                using (Models.Sqlconnect con = new Models.Sqlconnect())
                {
                    models.xiazu = con.xxiazu.Where(a => a.qisu == qisu).ToList();
                    models.xqisu = con.xqisu.OrderByDescending(a => a.id).Take(20).ToList();
                    models.qisu = qisu;
                    models.xm_name = xm;
                    var ms = con.xball.ToList();
                    IEnumerable<x9web.Models.xball> mss;
                    IEnumerable<x9web.Models.xball> txballs;
                    switch (xm)
                    {
                        #region 特码
                        case "特码":
                            //mss = from q in ms
                            //      where (q.color != null || q.type == 811)
                            //      select new Models.xball
                            //      {
                            //          name = q.name.Trim(),
                            //          color = q.color,
                            //          id = q.id,
                            //          type = q.type,
                            //          tma = q.tma
                            //      };
                            mss = from q in ms
                                  where (q.color != null || q.type == 811)
                                  select q;
                            models.xball = mss.ToList();
                            return PartialView("~/views/partial/manage_caplook_partial.cshtml", models);
                        case "特A":
                            //mss = from q in ms
                            //      where (q.color != null || q.type == 811)
                            //      select new Models.xball
                            //      {
                            //          name = q.name.Trim(),
                            //          color = q.color,
                            //          id = q.id,
                            //          type = q.type,
                            //          tma = q.tma
                            //      };
                            mss = from q in ms
                                  where (q.color != null || q.type == 811)
                                  select q;
                            models.xball = mss.ToList();
                            return PartialView("~/views/partial/manage_caplook_partial.cshtml", models);
                        case "特B":
                            //mss = from q in ms
                            //      where (q.color != null || q.type == 811)
                            //      select new Models.xball
                            //      {
                            //          name = q.name.Trim(),
                            //          color = q.color,
                            //          id = q.id,
                            //          type = q.type,
                            //          tma = q.tmb
                            //      };
                            mss = from q in ms
                                  where (q.color != null || q.type == 811)
                                  select q;
                            models.xball = mss.ToList();
                            return PartialView("~/views/partial/manage_caplook_partial.cshtml", models);
                        #endregion
                        #region 正码特
                        case "正1特":
                            mss = from q in ms
                                  where (q.color != null || q.type == 813)
                                  select new Models.xball
                                  {
                                      name = q.name.Trim(),
                                      color = q.color,
                                      id = q.id,
                                      type = q.type,
                                      tma = q.zt1
                                  };
                            models.xball = mss.ToList();
                            return PartialView("~/views/partial/manage_caplook_partial.cshtml", models);
                        case "正2特":
                            mss = from q in ms
                                  where (q.color != null || q.type == 813)
                                  select new Models.xball
                                  {
                                      name = q.name.Trim(),
                                      color = q.color,
                                      id = q.id,
                                      type = q.type,
                                      tma = q.zt2
                                  };
                            models.xball = mss.ToList();
                            return PartialView("~/views/partial/manage_caplook_partial.cshtml", models);
                        case "正3特":
                            mss = from q in ms
                                  where (q.color != null || q.type == 813)
                                  select new Models.xball
                                  {
                                      name = q.name.Trim(),
                                      color = q.color,
                                      id = q.id,
                                      type = q.type,
                                      tma = q.zt3
                                  };
                            models.xball = mss.ToList();
                            return PartialView("~/views/partial/manage_caplook_partial.cshtml", models);
                        case "正4特":
                            mss = from q in ms
                                  where (q.color != null || q.type == 813)
                                  select new Models.xball
                                  {
                                      name = q.name.Trim(),
                                      color = q.color,
                                      id = q.id,
                                      type = q.type,
                                      tma = q.zt4
                                  };
                            models.xball = mss.ToList();
                            return PartialView("~/views/partial/manage_caplook_partial.cshtml", models);
                        case "正5特":
                            mss = from q in ms
                                  where (q.color != null || q.type == 813)
                                  select new Models.xball
                                  {
                                      name = q.name.Trim(),
                                      color = q.color,
                                      id = q.id,
                                      type = q.type,
                                      tma = q.zt5
                                  };
                            models.xball = mss.ToList();
                            return PartialView("~/views/partial/manage_caplook_partial.cshtml", models);
                        case "正6特":
                            mss = from q in ms
                                  where (q.color != null || q.type == 813)
                                  select new Models.xball
                                  {
                                      name = q.name.Trim(),
                                      color = q.color,
                                      id = q.id,
                                      type = q.type,
                                      tma = q.zt6
                                  };
                            models.xball = mss.ToList();
                            return PartialView("~/views/partial/manage_caplook_partial.cshtml", models);
                        #endregion
                        #region 正码
                        case "正码":
                            mss = from q in ms
                                  where (q.color != null || q.type == 812)
                                  select new Models.xball
                                  {
                                      name = q.name.Trim(),
                                      color = q.color,
                                      id = q.id,
                                      type = q.type,
                                      tma = q.zma
                                  };
                            models.xball = mss.ToList();
                            return PartialView("~/views/partial/manage_caplook_partial.cshtml", models);
                        case "正A":
                            mss = from q in ms
                                  where (q.color != null || q.type == 812)
                                  select new Models.xball
                                  {
                                      name = q.name.Trim(),
                                      color = q.color,
                                      id = q.id,
                                      type = q.type,
                                      tma = q.zma
                                  };
                            models.xball = mss.ToList();
                            return PartialView("~/views/partial/manage_caplook_partial.cshtml", models);
                        case "正B":
                            mss = from q in ms
                                  where (q.color != null || q.type == 812)
                                  select new Models.xball
                                  {
                                      name = q.name.Trim(),
                                      color = q.color,
                                      id = q.id,
                                      type = q.type,
                                      tma = q.zma
                                  };
                            models.xball = mss.ToList();
                            return PartialView("~/views/partial/manage_caplook_partial.cshtml", models);
                        #endregion
                        #region 特码项目
                        case "特肖":
                            mss = from q in ms
                                  where (q.type == 814)
                                  select new Models.xball
                                  {
                                      name = q.name.Trim(),
                                      color = q.color,
                                      id = q.id,
                                      type = q.type,
                                      tma = q.tiex
                                  };
                            txballs = from q in ms
                                                 where (q.color != null)
                                                 select q;
                            ViewBag.xmb = txballs;
                            ViewData["xxxname"] = "特肖";
                            models.xball = mss.ToList();
                            return PartialView("~/views/partial/manage_caplook_partial3.cshtml", models);
                        case "半波":
                            mss = from q in ms
                                  where (q.type == 815)
                                  select new Models.xball
                                  {
                                      name = q.name.Trim(),
                                      color = q.color,
                                      id = q.id,
                                      type = q.type,
                                      tma = q.babo
                                  };
                            models.xball = mss.ToList();
                            return PartialView("~/views/partial/manage_caplook_partial3.cshtml", models);
                        case "五行":
                            mss = from q in ms
                                  where (q.type == 817)
                                  select new Models.xball
                                  {
                                      name = q.name.Trim(),
                                      color = q.color,
                                      id = q.id,
                                      type = q.type,
                                      tma = q.n5xin
                                  };
                            models.xball = mss.ToList();
                            return PartialView("~/views/partial/manage_caplook_partial2.cshtml", models);
                        #endregion
                        #region 连码
                        case "二全中":
                            mss = from q in ms
                                  where (q.type == null)
                                  select new Models.xball
                                  {
                                      name = q.name.Trim(),
                                      color = q.color,
                                      id = q.id,
                                      type = q.type,
                                      tma = q.s2qz
                                  };
                            models.xball = mss.ToList();
                            return PartialView("~/views/partial/manage_caplook_partial2.cshtml", models);
                        case "二中特":
                            mss = from q in ms
                                  where (q.type == null)
                                  select new Models.xball
                                  {
                                      name = q.name.Trim(),
                                      color = q.color,
                                      id = q.id,
                                      type = q.type,
                                      tma = q.s2zt
                                  };
                            models.xball = mss.ToList();
                            return PartialView("~/views/partial/manage_caplook_partial2.cshtml", models);
                        case "特串":
                            mss = from q in ms
                                  where (q.type == null)
                                  select new Models.xball
                                  {
                                      name = q.name.Trim(),
                                      color = q.color,
                                      id = q.id,
                                      type = q.type,
                                      tma = q.stc
                                  };
                            models.xball = mss.ToList();
                            return PartialView("~/views/partial/manage_caplook_partial2.cshtml", models);
                        case "三中二":
                            mss = from q in ms
                                  where (q.type == null)
                                  select new Models.xball
                                  {
                                      name = q.name.Trim(),
                                      color = q.color,
                                      id = q.id,
                                      type = q.type,
                                      tma = q.s3z2
                                  };
                            models.xball = mss.ToList();
                            return PartialView("~/views/partial/manage_caplook_partial2.cshtml", models);
                        case "三全中":
                            mss = from q in ms
                                  where (q.type == null)
                                  select new Models.xball
                                  {
                                      name = q.name.Trim(),
                                      color = q.color,
                                      id = q.id,
                                      type = q.type,
                                      tma = q.s3qz
                                  };
                            models.xball = mss.ToList();
                            return PartialView("~/views/partial/manage_caplook_partial2.cshtml", models);
                        #endregion
                        #region 生肖连
                        case "二肖中":
                            mss = from q in ms
                                  where (q.type == 814)
                                  select new Models.xball
                                  {
                                      name = q.name.Trim(),
                                      color = q.color,
                                      id = q.id,
                                      type = q.type,
                                      tma = q.x2
                                  };
                            models.xball = mss.ToList();
                            return PartialView("~/views/partial/manage_caplook_partial2.cshtml", models);
                        case "三肖中":
                            mss = from q in ms
                                  where (q.type == 814)
                                  select new Models.xball
                                  {
                                      name = q.name.Trim(),
                                      color = q.color,
                                      id = q.id,
                                      type = q.type,
                                      tma = q.x3
                                  };
                            models.xball = mss.ToList();
                            return PartialView("~/views/partial/manage_caplook_partial2.cshtml", models);
                        case "四肖中":
                            mss = from q in ms
                                  where (q.type == 814)
                                  select new Models.xball
                                  {
                                      name = q.name.Trim(),
                                      color = q.color,
                                      id = q.id,
                                      type = q.type,
                                      tma = q.x4
                                  };
                            models.xball = mss.ToList();
                            return PartialView("~/views/partial/manage_caplook_partial2.cshtml", models);
                        case "五肖中":
                            mss = from q in ms
                                  where (q.type == 814)
                                  select new Models.xball
                                  {
                                      name = q.name.Trim(),
                                      color = q.color,
                                      id = q.id,
                                      type = q.type,
                                      tma = q.x5
                                  };
                            models.xball = mss.ToList();
                            return PartialView("~/views/partial/manage_caplook_partial2.cshtml", models);
                        case "二肖不中":
                            mss = from q in ms
                                  where (q.type == 814)
                                  select new Models.xball
                                  {
                                      name = q.name.Trim(),
                                      color = q.color,
                                      id = q.id,
                                      type = q.type,
                                      tma = q.x2b
                                  };
                            models.xball = mss.ToList();
                            return PartialView("~/views/partial/manage_caplook_partial2.cshtml", models);
                        case "三肖不中":
                            mss = from q in ms
                                  where (q.type == 814)
                                  select new Models.xball
                                  {
                                      name = q.name.Trim(),
                                      color = q.color,
                                      id = q.id,
                                      type = q.type,
                                      tma = q.x3b
                                  };
                            models.xball = mss.ToList();
                            return PartialView("~/views/partial/manage_caplook_partial2.cshtml", models);
                        case "四肖不中":
                            mss = from q in ms
                                  where (q.type == 814)
                                  select new Models.xball
                                  {
                                      name = q.name.Trim(),
                                      color = q.color,
                                      id = q.id,
                                      type = q.type,
                                      tma = q.x4b
                                  };
                            models.xball = mss.ToList();
                            return PartialView("~/views/partial/manage_caplook_partial2.cshtml", models);
                        case "五肖不中":
                            mss = from q in ms
                                  where (q.type == 814)
                                  select new Models.xball
                                  {
                                      name = q.name.Trim(),
                                      color = q.color,
                                      id = q.id,
                                      type = q.type,
                                      tma = q.x5b
                                  };
                            models.xball = mss.ToList();
                            return PartialView("~/views/partial/manage_caplook_partial2.cshtml", models);
                        #endregion
                        #region 尾数连
                        case "二尾中":
                            mss = from q in ms
                                  where (q.type == 816)
                                  select new Models.xball
                                  {
                                      name = q.name.Trim(),
                                      color = q.color,
                                      id = q.id,
                                      type = q.type,
                                      tma = q.w2
                                  };
                            models.xball = mss.ToList();
                            return PartialView("~/views/partial/manage_caplook_partial2.cshtml", models);
                        case "三尾中":
                            mss = from q in ms
                                  where (q.type == 816)
                                  select new Models.xball
                                  {
                                      name = q.name.Trim(),
                                      color = q.color,
                                      id = q.id,
                                      type = q.type,
                                      tma = q.w3
                                  };
                            models.xball = mss.ToList();
                            return PartialView("~/views/partial/manage_caplook_partial2.cshtml", models);
                        case "四尾中":
                            mss = from q in ms
                                  where (q.type == 816)
                                  select new Models.xball
                                  {
                                      name = q.name.Trim(),
                                      color = q.color,
                                      id = q.id,
                                      type = q.type,
                                      tma = q.w4
                                  };
                            models.xball = mss.ToList();
                            return PartialView("~/views/partial/manage_caplook_partial2.cshtml", models);
                        case "五尾中":
                            mss = from q in ms
                                  where (q.type == 816)
                                  select new Models.xball
                                  {
                                      name = q.name.Trim(),
                                      color = q.color,
                                      id = q.id,
                                      type = q.type,
                                      tma = q.w5
                                  };
                            models.xball = mss.ToList();
                            return PartialView("~/views/partial/manage_caplook_partial2.cshtml", models);
                        case "二尾不中":
                            mss = from q in ms
                                  where (q.type == 816)
                                  select new Models.xball
                                  {
                                      name = q.name.Trim(),
                                      color = q.color,
                                      id = q.id,
                                      type = q.type,
                                      tma = q.w2b
                                  };
                            models.xball = mss.ToList();
                            return PartialView("~/views/partial/manage_caplook_partial2.cshtml", models);
                        case "三尾不中":
                            mss = from q in ms
                                  where (q.type == 816)
                                  select new Models.xball
                                  {
                                      name = q.name.Trim(),
                                      color = q.color,
                                      id = q.id,
                                      type = q.type,
                                      tma = q.w3b
                                  };
                            models.xball = mss.ToList();
                            return PartialView("~/views/partial/manage_caplook_partial2.cshtml", models);
                        case "四尾不中":
                            mss = from q in ms
                                  where (q.type == 816)
                                  select new Models.xball
                                  {
                                      name = q.name.Trim(),
                                      color = q.color,
                                      id = q.id,
                                      type = q.type,
                                      tma = q.w4b
                                  };
                            models.xball = mss.ToList();
                            return PartialView("~/views/partial/manage_caplook_partial2.cshtml", models);
                        case "五尾不中":
                            mss = from q in ms
                                  where (q.type == 816)
                                  select new Models.xball
                                  {
                                      name = q.name.Trim(),
                                      color = q.color,
                                      id = q.id,
                                      type = q.type,
                                      tma = q.w5b
                                  };
                            models.xball = mss.ToList();
                            return PartialView("~/views/partial/manage_caplook_partial2.cshtml", models);
                        #endregion
                        #region 生肖项目
                        case "一肖":
                            mss = from q in ms
                                  where (q.type == 814)
                                  select new Models.xball
                                  {
                                      name = q.name.Trim(),
                                      color = q.color,
                                      id = q.id,
                                      type = q.type,
                                      tma = q.yix
                                  };
                            txballs = from q in ms
                                                 where (q.color != null)
                                                 select q;
                            ViewBag.xmb = txballs;
                            ViewData["xxxname"] = "一肖";
                            models.xball = mss.ToList();
                            return PartialView("~/views/partial/manage_caplook_partial3.cshtml", models);
                        case "一肖不中":
                            mss = from q in ms
                                  where (q.type == 814)
                                  select new Models.xball
                                  {
                                      name = q.name.Trim(),
                                      color = q.color,
                                      id = q.id,
                                      type = q.type,
                                      tma = q.yixb
                                  };
                            models.xball = mss.ToList();
                            return PartialView("~/views/partial/manage_caplook_partial3.cshtml", models);
                        case "六肖":
                            mss = from q in ms
                                  where (q.type == 814)
                                  select new Models.xball
                                  {
                                      name = q.name.Trim(),
                                      color = q.color,
                                      id = q.id,
                                      type = q.type,
                                      tma = q.liux
                                  };
                            models.xball = mss.ToList();
                            return PartialView("~/views/partial/manage_caplook_partial2.cshtml", models);
                        case "六肖不中":
                            mss = from q in ms
                                  where (q.type == 814)
                                  select new Models.xball
                                  {
                                      name = q.name.Trim(),
                                      color = q.color,
                                      id = q.id,
                                      type = q.type,
                                      tma = q.liuxb
                                  };
                            models.xball = mss.ToList();
                            return PartialView("~/views/partial/manage_caplook_partial2.cshtml", models);
                        case "生肖量":
                            mss = from q in ms
                                  where (q.type == 818)
                                  select new Models.xball
                                  {
                                      name = q.name.Trim(),
                                      color = q.color,
                                      id = q.id,
                                      type = q.type,
                                      tma = q.nsxl
                                  };
                            models.xball = mss.ToList();
                            return PartialView("~/views/partial/manage_caplook_partial2.cshtml", models);
                        #endregion
                        #region 总数项目
                        case "尾数":
                            mss = from q in ms
                                  where (q.type == 816)
                                  select new Models.xball
                                  {
                                      name = q.name.Trim(),
                                      color = q.color,
                                      id = q.id,
                                      type = q.type,
                                      tma = q.ws
                                  };
                            models.xball = mss.ToList();
                            return PartialView("~/views/partial/manage_caplook_partial3.cshtml", models);
                        case "尾数不中":
                            mss = from q in ms
                                  where (q.type == 816)
                                  select new Models.xball
                                  {
                                      name = q.name.Trim(),
                                      color = q.color,
                                      id = q.id,
                                      type = q.type,
                                      tma = q.wsb
                                  };
                            models.xball = mss.ToList();
                            return PartialView("~/views/partial/manage_caplook_partial3.cshtml", models);
                        case "尾数量":
                            mss = from q in ms
                                  where (q.type == 819)
                                  select new Models.xball
                                  {
                                      name = q.name.Trim(),
                                      color = q.color,
                                      id = q.id,
                                      type = q.type,
                                      tma = q.nwsl
                                  };
                            models.xball = mss.ToList();
                            return PartialView("~/views/partial/manage_caplook_partial2.cshtml", models);
                        #endregion
                        #region 多选不中
                        case "五不中":
                            mss = from q in ms
                                  where (q.type == null)
                                  select new Models.xball
                                  {
                                      name = q.name.Trim(),
                                      color = q.color,
                                      id = q.id,
                                      type = q.type,
                                      tma = q.db5
                                  };
                            models.xball = mss.ToList();
                            return PartialView("~/views/partial/manage_caplook_partial2.cshtml", models);
                        case "六不中":
                            mss = from q in ms
                                  where (q.type == null)
                                  select new Models.xball
                                  {
                                      name = q.name.Trim(),
                                      color = q.color,
                                      id = q.id,
                                      type = q.type,
                                      tma = q.db6
                                  };
                            models.xball = mss.ToList();
                            return PartialView("~/views/partial/manage_caplook_partial2.cshtml", models);
                        case "七不中":
                            mss = from q in ms
                                  where (q.type == null)
                                  select new Models.xball
                                  {
                                      name = q.name.Trim(),
                                      color = q.color,
                                      id = q.id,
                                      type = q.type,
                                      tma = q.db7
                                  };
                            models.xball = mss.ToList();
                            return PartialView("~/views/partial/manage_caplook_partial2.cshtml", models);
                        case "八不中":
                            mss = from q in ms
                                  where (q.type == null)
                                  select new Models.xball
                                  {
                                      name = q.name.Trim(),
                                      color = q.color,
                                      id = q.id,
                                      type = q.type,
                                      tma = q.db8
                                  };
                            models.xball = mss.ToList();
                            return PartialView("~/views/partial/manage_caplook_partial2.cshtml", models);
                        case "九不中":
                            mss = from q in ms
                                  where (q.type == null)
                                  select new Models.xball
                                  {
                                      name = q.name.Trim(),
                                      color = q.color,
                                      id = q.id,
                                      type = q.type,
                                      tma = q.db9
                                  };
                            models.xball = mss.ToList();
                            return PartialView("~/views/partial/manage_caplook_partial2.cshtml", models);
                        case "十不中":
                            mss = from q in ms
                                  where (q.type == null)
                                  select new Models.xball
                                  {
                                      name = q.name.Trim(),
                                      color = q.color,
                                      id = q.id,
                                      type = q.type,
                                      tma = q.db10
                                  };
                            models.xball = mss.ToList();
                            return PartialView("~/views/partial/manage_caplook_partial2.cshtml", models);
                        case "十二不中":
                            mss = from q in ms
                                  where (q.type == null)
                                  select new Models.xball
                                  {
                                      name = q.name.Trim(),
                                      color = q.color,
                                      id = q.id,
                                      type = q.type,
                                      tma = q.db12
                                  };
                            models.xball = mss.ToList();
                            return PartialView("~/views/partial/manage_caplook_partial2.cshtml", models);
                        case "十五不中":
                            mss = from q in ms
                                  where (q.type == null)
                                  select new Models.xball
                                  {
                                      name = q.name.Trim(),
                                      color = q.color,
                                      id = q.id,
                                      type = q.type,
                                      tma = q.db15
                                  };
                            models.xball = mss.ToList();
                            return PartialView("~/views/partial/manage_caplook_partial2.cshtml", models);
                        #endregion
                        #region 多选中一
                        case "五中一":
                            mss = from q in ms
                                  where (q.type == null)
                                  select new Models.xball
                                  {
                                      name = q.name.Trim(),
                                      color = q.color,
                                      id = q.id,
                                      type = q.type,
                                      tma = q.dz5
                                  };
                            models.xball = mss.ToList();
                            return PartialView("~/views/partial/manage_caplook_partial2.cshtml", models);
                        case "六中一":
                            mss = from q in ms
                                  where (q.type == null)
                                  select new Models.xball
                                  {
                                      name = q.name.Trim(),
                                      color = q.color,
                                      id = q.id,
                                      type = q.type,
                                      tma = q.dz6
                                  };
                            models.xball = mss.ToList();
                            return PartialView("~/views/partial/manage_caplook_partial2.cshtml", models);
                        case "七中一":
                            mss = from q in ms
                                  where (q.type == null)
                                  select new Models.xball
                                  {
                                      name = q.name.Trim(),
                                      color = q.color,
                                      id = q.id,
                                      type = q.type,
                                      tma = q.dz7
                                  };
                            models.xball = mss.ToList();
                            return PartialView("~/views/partial/manage_caplook_partial2.cshtml", models);
                        case "八中一":
                            mss = from q in ms
                                  where (q.type == null)
                                  select new Models.xball
                                  {
                                      name = q.name.Trim(),
                                      color = q.color,
                                      id = q.id,
                                      type = q.type,
                                      tma = q.dz8
                                  };
                            models.xball = mss.ToList();
                            return PartialView("~/views/partial/manage_caplook_partial2.cshtml", models);
                        case "九中一":
                            mss = from q in ms
                                  where (q.type == null)
                                  select new Models.xball
                                  {
                                      name = q.name.Trim(),
                                      color = q.color,
                                      id = q.id,
                                      type = q.type,
                                      tma = q.dz9
                                  };
                            models.xball = mss.ToList();
                            return PartialView("~/views/partial/manage_caplook_partial2.cshtml", models);
                        case "十中一":
                            mss = from q in ms
                                  where (q.type == null)
                                  select new Models.xball
                                  {
                                      name = q.name.Trim(),
                                      color = q.color,
                                      id = q.id,
                                      type = q.type,
                                      tma = q.dz10
                                  };
                            models.xball = mss.ToList();
                            return PartialView("~/views/partial/manage_caplook_partial2.cshtml", models);
                        #endregion
                        #region 特平中
                        case "一粒任中":
                            mss = from q in ms
                                  where (q.type == null)
                                  select new Models.xball
                                  {
                                      name = q.name.Trim(),
                                      color = q.color,
                                      id = q.id,
                                      type = q.type,
                                      tma = q.rz1
                                  };
                            models.xball = mss.ToList();
                            return PartialView("~/views/partial/manage_caplook_partial2.cshtml", models);
                        case "二粒任中":
                            mss = from q in ms
                                  where (q.type == null)
                                  select new Models.xball
                                  {
                                      name = q.name.Trim(),
                                      color = q.color,
                                      id = q.id,
                                      type = q.type,
                                      tma = q.rz2
                                  };
                            models.xball = mss.ToList();
                            return PartialView("~/views/partial/manage_caplook_partial2.cshtml", models);
                        case "三粒任中":
                            mss = from q in ms
                                  where (q.type == null)
                                  select new Models.xball
                                  {
                                      name = q.name.Trim(),
                                      color = q.color,
                                      id = q.id,
                                      type = q.type,
                                      tma = q.rz3
                                  };
                            models.xball = mss.ToList();
                            return PartialView("~/views/partial/manage_caplook_partial2.cshtml", models);
                        case "四粒任中":
                            mss = from q in ms
                                  where (q.type == null)
                                  select new Models.xball
                                  {
                                      name = q.name.Trim(),
                                      color = q.color,
                                      id = q.id,
                                      type = q.type,
                                      tma = q.rz4
                                  };
                            models.xball = mss.ToList();
                            return PartialView("~/views/partial/manage_caplook_partial2.cshtml", models);
                        case "五粒任中":
                            mss = from q in ms
                                  where (q.type == null)
                                  select new Models.xball
                                  {
                                      name = q.name.Trim(),
                                      color = q.color,
                                      id = q.id,
                                      type = q.type,
                                      tma = q.rz5
                                  };
                            models.xball = mss.ToList();
                            return PartialView("~/views/partial/manage_caplook_partial2.cshtml", models);
                        #endregion

                        default:
                            return Content("没有此项监控数据！");

                    }
                }
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }

        }
        public ActionResult mpwd_edit()
        {
            if (Session["xname"] == null)
            {
                return Content("已断开连接，请重新登陆！");
            }
            return View();
        }
        [HttpPost]
        public ActionResult pwdedit(string p1, string p2, string p3)
        {
            if (Session["xname"] == null)
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
                    var m = con.xusers.Where(a => a.id == int.Parse(Session["xid"].ToString())).Where(a => a.pwd == Api.WebClass.FunMD5(p1)).ToList();
                    if (m.Count < 1)
                    {
                        return Json(new { type = "err", msg = "输入的原密码错！" });
                    }
                    else
                    {
                        var f = from q in con.xusers
                                where (q.id == int.Parse(Session["xid"].ToString()))
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
        public ActionResult user_tuisui()
        {
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

        public ActionResult user_tuisui2(string time, int pid)
        {
            if (time == string.Empty)
            {
                return null;
            }
            try
            {
                using (Models.Sqlconnect con = new Models.Sqlconnect())
                {
                    var tims = time.Replace(">>>", ">");
                    var tarr = tims.Split('>');
                    var t1 = tarr[0] + " 00:00:00";
                    var t2 = tarr[1] + " 23:59:59";

                    var m = con.xusers.Where(a => a.manage == false).ToList();
                    var q = con.xqisu.ToList();
                    var x = con.xxiazu.Where(a => a.timer >= DateTime.Parse(t1)).Where(a => a.timer <= DateTime.Parse(t2)).ToList();
                    var s = con.xtuisui.ToList();

                    ViewBag.qisu_model = q;
                    ViewBag.xiazu_model = x;
                    ViewBag.tuisui_model = s;
                    ViewBag.retime = time;
                    ViewBag.type = 2;
                    ViewBag.pid = pid;
                    return View("user_tuisui", m);
                }

            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }

        }

        public ActionResult man_look_sum(string type)
        {
            List<Models.xqisu> m;
            if (Session["xname"] == null)
            {
                return Content("连接已断开，请重新登陆！");
            }
            if (string.IsNullOrEmpty(type))
            {
                return null;
            }

            try
            {
                using (Models.Sqlconnect con = new Models.Sqlconnect())
                {
                    m = con.xqisu.Where(a => a.jiang == true).OrderByDescending(a => a.id).ToList();
                }
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }

            switch (type)
            {
                case "dx":
                    return View("~/views/manage/t1.cshtml", m);
                case "ws":
                    return View("~/views/manage/t2.cshtml", m);
                case "yx":
                    string sx = string.Empty;
                    using (Models.Sqlconnect con = new Models.Sqlconnect())
                    {
                        var mx = con.xconfig.ToList().First();
                        ViewBag.sx = mx.s12sx.Trim();
                    }
                    return View("~/views/manage/t3.cshtml", m);
                default:
                    return null;
            }

        }

        public ActionResult yujin()
        {
            if (Session["xname"] == null)
            {
                return Content("未登陆，或已断开连接，请重新登陆！<a href='/manage/index'>返回</a>");
            }
            using (Models.Sqlconnect con = new Models.Sqlconnect())
            {
                var m = con.xqisu.ToList().OrderByDescending(a => a.id);
                return View(m);
            }
        }
    }
}
