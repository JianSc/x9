using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Sql;
using System.Data.SqlClient;

namespace x9web.Controllers
{
    public class HandleController : Controller
    {
        //
        // GET: /Handle/

        /// <summary>
        /// 下注处理
        /// </summary>
        /// <param name="id"></param>
        /// <param name="ball"></param>
        /// <param name="beilu"></param>
        /// <param name="money"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult xiazu(int id, string[] ball, decimal[] beilu, int[] money)
        {
            var xid = id;
            var xball = ball;
            var xbeilu = beilu;
            var xmoney = money;
            return null;
        }

        /// <summary>
        /// 下注总单显示
        /// </summary>
        /// <param name="id"></param>
        /// <param name="ball"></param>
        /// <param name="beilu"></param>
        /// <param name="money"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult xiazu_view(int id, string[] name, string[] ball, string[] beilu, int[] money)
        {
            ViewBag.id = id;
            var bol = Api.WebClass.auto_pankou();
            try
            {
                if (Session["id"] == null)
                {
                    return Content("连接已断开，请重新登陆。");
                }
                if (ball == null || beilu == null || money == null || id == 0)
                {
                    return Content("数据错误");
                }
                var t1 = string.Empty;
                var t2 = string.Empty;
                var m = new List<Models.xxiazu>();
                if (Api.WebClass.f_liubie(ref t1, ref t2, id))
                {
                    ViewBag.pagename = t2;
                    for (int i = 0; i < ball.Length; i++)
                    {
                        if (money[i] == 0)
                        {
                            continue;
                        }
                        var mm = new Models.xxiazu();
                        var balltxt = Api.WebClass.XiazhuName_Edit(ball[i]);
                        mm.t1 = t1;
                        mm.t2 = t2;
                        mm.name = name[i];
                        mm.ball = ball[i];
                        mm.beilu = beilu[i];
                        mm.money = money[i];
                        mm.yongjin = Api.WebClass.f_tuisui(balltxt, t2);
                        m.Add(mm);
                    }
                    ViewBag.total = m.Count();
                    return PartialView("xiazu_view", m);
                }
                else
                {
                    return Content("下注类别不存在！");
                }
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }
        /// <summary>
        /// 下注总单设置（没有自动降赔）
        /// </summary>
        /// <param name="id"></param>
        /// <param name="ball"></param>
        /// <param name="beilu"></param>
        /// <param name="money"></param>
        /// <returns></returns>
        public ActionResult xiazu_view2(int id, string[] ball, string[] beilu, int[] money)
        {
            ViewBag.id = id;
            var bol = Api.WebClass.auto_pankou();
            try
            {
                if (Session["id"] == null)
                {
                    return Content("连接已断开，请重新登陆。");
                }
                if (ball == null || beilu == null || money == null || id == 0)
                {
                    return Content("数据错误");
                }
                var t1 = string.Empty;
                var t2 = string.Empty;
                var m = new List<Models.xxiazu>();
                if (Api.WebClass.f_liubie(ref t1, ref t2, id))
                {
                    ViewBag.pagename = t2;
                    for (int i = 0; i < ball.Length; i++)
                    {
                        if (money[i] == 0)
                        {
                            continue;
                        }
                        var mm = new Models.xxiazu();
                        var balltxt = Api.WebClass.XiazhuName_Edit(ball[i]);
                        mm.t1 = t1;
                        mm.t2 = t2;
                        mm.name = "";
                        mm.ball = ball[i];
                        mm.beilu = beilu[i];
                        mm.money = money[i];
                        mm.yongjin = Api.WebClass.f_tuisui(balltxt, t2);
                        m.Add(mm);
                    }
                    ViewBag.total = m.Count();
                    return PartialView("xiazu_view", m);
                }
                else
                {
                    return Content("下注类别不存在！");
                }
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }

        /// <summary>
        /// 下注单金额输入框
        /// </summary>
        /// <param name="id"></param>
        /// <param name="ball"></param>
        /// <param name="beilu"></param>
        /// <param name="money"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult xiazu_view_dan(int id, string name, string ball, string beilu)
        {
            try
            {
                var bol = Api.WebClass.auto_pankou();
                if (Session["id"] == null)
                {
                    return Content("连接已断开，请重新登陆。");
                }

                var mode = new x9web.Models.xxiazu();
                ViewBag.id = id;
                var t1 = string.Empty;
                var t2 = string.Empty;
                if (Api.WebClass.f_liubie(ref t1, ref t2, id))
                {
                    mode.name = name;
                    mode.ball = ball;
                    mode.beilu = beilu;
                    mode.t1 = t1;
                    mode.t2 = t2;
                    var balltxt = Api.WebClass.XiazhuName_Edit(ball);
                    mode.yongjin = Api.WebClass.f_tuisui(balltxt, t2);
                    return PartialView("xiazu_view_dan", mode);
                }
                else
                {
                    return Content("获取数据出错，请重新下注！");
                }
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }

        public ActionResult javascript_sx(string s)
        {
            if (s == string.Empty || s == "")
            {
                return Json(new { type = "err", msg = "数据传输错误！" });
            }
            try
            {
                using (Models.Sqlconnect con = new Models.Sqlconnect())
                {
                    if (s == "家")
                    {
                        var sx = "牛,马,羊,鸡,狗,猪";
                        var str = string.Empty;
                        var mode = con.xball.ToList();
                        foreach (var m in mode)
                        {
                            if (sx.IndexOf(m.shenxiao.ToString()) > -1)
                            {
                                str += m.name.Trim();
                                str += ",";
                            }

                        }
                        str = str.Substring(0, str.Length - 1);
                        return Json(new { type = "suc", msg = str });
                    }
                    else if (s == "野")
                    {
                        var sx = "鼠,虎,兔,龙,蛇,猴";
                        var str = string.Empty;
                        var mode = con.xball.ToList();
                        foreach (var m in mode)
                        {
                            if (sx.IndexOf(m.shenxiao.ToString()) > -1)
                            {
                                str += m.name.Trim();
                                str += ",";
                            }
                        }
                        str = str.Substring(0, str.Length - 1);
                        return Json(new { type = "suc", msg = str });
                    }
                    else
                    {
                        var str = string.Empty;
                        var mode = con.xball.Where(a => a.shenxiao == Convert.ToChar(s)).ToList();
                        foreach (var m in mode)
                        {
                            str += m.name.Trim();
                            str += ",";
                        }
                        str = str.Substring(0, str.Length - 1);
                        return Json(new { type = "suc", msg = str });
                    }

                }
            }
            catch (Exception ex)
            {
                return Json(new { type = "err", msg = ex.Message });
            }
        }

        [HttpPost]
        public ActionResult manage_pankou(int id, bool xbool, int qisu)
        {
            try
            {
                if (Session["xname"] == null)
                {
                    return Json(new { type = "err", msg = "已断开连接，请重新登陆！" });
                }
                switch (id)
                {
                    case 6001:
                        using (Models.Sqlconnect con = new Models.Sqlconnect())
                        {
                            var m = from q in con.xqisu
                                    where (q.id == qisu)
                                    select q;
                            if (m.Count() < 1)
                            {
                                return Json(new { type = "err", msg = "数据错误！" });
                            }
                            if ((bool)m.First().jiang)
                            {
                                return Json(new { type = "err", msg = "已开奖，不能更改盘口状态！" });
                            }
                            m.First().zema_bankou = xbool;
                            m.First().z_auto = false;
                            con.SubmitChanges();
                            return Json(new { type = "suc" });
                        }
                        break;
                    case 6002:
                        using (Models.Sqlconnect con = new Models.Sqlconnect())
                        {
                            var m = from q in con.xqisu
                                    where (q.id == qisu)
                                    select q;
                            if (m.Count() < 1)
                            {
                                return Json(new { type = "err", msg = "数据错误！" });
                            }
                            if ((bool)m.First().jiang)
                            {
                                return Json(new { type = "err", msg = "已开奖，不能更改盘口状态！" });
                            }
                            m.First().tema_bankou = xbool;
                            m.First().t_auto = false;
                            con.SubmitChanges();
                            return Json(new { type = "suc" });
                        }
                        break;
                    case 6003:
                        using (Models.Sqlconnect con = new Models.Sqlconnect())
                        {
                            var m = from q in con.xqisu
                                    where (q.id == qisu)
                                    select q;
                            if (m.Count() < 1)
                            {
                                return Json(new { type = "err", msg = "数据错误！" });
                            }
                            if ((bool)m.First().jiang)
                            {
                                return Json(new { type = "err", msg = "已开奖，不能更改盘口状态！" });
                            }
                            m.First().bankou = xbool;
                            m.First().strat = true;
                            con.SubmitChanges();
                            return Json(new { type = "suc" });
                        }
                        break;
                    case 6004:
                        using (Models.Sqlconnect con = new Models.Sqlconnect())
                        {
                            var m = from q in con.xqisu
                                    where (q.id == qisu)
                                    select q;
                            if (m.Count() < 1)
                            {
                                return Json(new { type = "err", msg = "数据错误！" });
                            }
                            if ((bool)m.First().jiang)
                            {
                                return Json(new { type = "err", msg = "已开奖，不能更改盘口状态！" });
                            }
                            if (!(bool)m.First().zema_bankou)
                            {
                                return Json(new { type = "err", msg = "正码已开盘，自动开盘设置失效！" });
                            }
                            m.First().z_auto = xbool;
                            con.SubmitChanges();
                            return Json(new { type = "suc" });
                        }
                        break;
                    case 6005:
                        using (Models.Sqlconnect con = new Models.Sqlconnect())
                        {
                            var m = from q in con.xqisu
                                    where (q.id == qisu)
                                    select q;
                            if (m.Count() < 1)
                            {
                                return Json(new { type = "err", msg = "数据错误！" });
                            }
                            if ((bool)m.First().jiang)
                            {
                                return Json(new { type = "err", msg = "已开奖，不能更改盘口状态！" });
                            }
                            if (!(bool)m.First().tema_bankou)
                            {
                                return Json(new { type = "err", msg = "特码已开盘，自动开盘设置失效！" });
                            }
                            m.First().t_auto = xbool;
                            con.SubmitChanges();
                            return Json(new { type = "suc" });
                        }
                        break;
                    case 6006:
                        try
                        {
                            using (Models.Sqlconnect con = new Models.Sqlconnect())
                            {
                                var m = con.xconfig.First();
                                m.web_switch = xbool;
                                con.SubmitChanges();
                                return Json(new { type = "suc" });
                            }
                        }
                        catch (Exception ex)
                        {
                            return Json(new { type = "err", msg = ex.Message });
                        }
                        break;
                    case 6007:
                        try
                        {
                            using (Models.Sqlconnect con = new Models.Sqlconnect())
                            {
                                var m = con.xusers.Where(a => a.id == qisu).First();
                                m.state = xbool;
                                con.SubmitChanges();
                                return Json(new { type = "suc" });
                            }
                        }
                        catch (Exception ex)
                        {
                            return Json(new { type = "err", msg = ex.Message });
                        }
                        break;
                    case 6008:
                        try
                        {
                            using (Models.Sqlconnect con = new Models.Sqlconnect())
                            {
                                var m = con.xauto_beilu.Where(a => a.id == qisu).First();
                                m.state = xbool;
                                con.SubmitChanges();
                                return Json(new { type = "suc" });
                            }
                        }
                        catch (Exception ex)
                        {
                            return Json(new { type = "err", msg = ex.Message });
                        }
                        break;
                    case 6009:
                        try
                        {
                            using (Models.Sqlconnect con = new Models.Sqlconnect())
                            {
                                var m = con.xmarquee.Where(a => a.id == qisu).First();
                                m.state = xbool;
                                con.SubmitChanges();
                                return Json(new { type = "suc" });
                            }
                        }
                        catch (Exception ex)
                        {
                            return Json(new { type = "err", msg = ex.Message });
                        }
                        break;

                    default:
                        return null;

                }
            }
            catch (Exception ex)
            {
                return Json(new { type = "err", msg = ex.Message });
            }
        }

        [HttpPost]
        public ActionResult pankou_addnew(int name, DateTime k_time, DateTime z_time, DateTime t_time)
        {
            try
            {
                if (name == 0)
                {
                    return Json(new { type = "err", msg = "数据输入错误！" });
                }
                if (k_time == null || z_time == null || t_time == null)
                {
                    return Json(new { type = "err", msg = "数据输入错误！" });
                }
                if ((z_time <= k_time) || (t_time <= k_time))
                {
                    return Json(new { type = "err", msg = "封盘时间低于开盘时间！" });
                }
                using (Models.Sqlconnect con = new Models.Sqlconnect())
                {
                    var m = con.xqisu.Where(a => a.del == false).OrderByDescending(a => a.id).ToList();
                    if (m.Count() > 0)
                    {
                        if (k_time <= m.First().z_time)
                        {
                            return Json(new { type = "err", msg = "自动开盘时间不能比上一期封盘时间早！" });
                        }

                    }
                    m = con.xqisu.Where(a => a.name == name).ToList();
                    if (m.Count() > 0)
                    {
                        return Json(new { type = "err", msg = "这个期号已经存在！" });
                    }
                    var m2 = new Models.xqisu
                    {
                        name = name,
                        zema_bankou = false,
                        tema_bankou = false,
                        jiang = false,
                        k_time = k_time,
                        z_time = z_time,
                        t_time = t_time,
                        strat = false,
                        bankou = false,
                        del = false,
                        jiesuan = false,
                        z_auto = false,
                        t_auto = false
                    };
                    con.xqisu.InsertOnSubmit(m2);
                    con.SubmitChanges();
                    return Json(new { type = "suc", msg = "盘口添加成功！" });
                }
            }
            catch (Exception ex)
            {
                return Json(new { type = "err", msg = ex.Message });
            }
        }

        public ActionResult pankou_del(int id, int xn)
        {
            try
            {
                var constr = System.Configuration.ConfigurationManager.ConnectionStrings["x9dataConnectionString"].ConnectionString;
                using (SqlConnection con = new SqlConnection(constr))
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand("DELETE FROM XQISU WHERE ID=" + id, con);
                    cmd.ExecuteNonQuery();
                    cmd = new SqlCommand("DELETE FROM XXIAZU WHERE QISU=" + xn, con);
                    cmd.ExecuteNonQuery();
                    con.Close();
                    return Json(new { type = "suc", msg = "操作成功!" });
                }
            }
            catch (Exception ex)
            {
                return Json(new { type = "err", msg = ex.Message });
            }
        }
        public ActionResult pankou_kaijiang(int id, string ball)
        {
            try
            {
                if (id == 0 || ball.Trim() == string.Empty)
                {
                    return Json(new { type = "err", msg = "错误：733" });
                }
                using (Models.Sqlconnect con = new Models.Sqlconnect())
                {
                    bool b = (bool)con.xqisu.Where(a => a.id == id).First().bankou;
                    if (b)
                    {
                        return Json(new { type = "err", msg = "总盘未封盘，不允许开奖！" });
                    }
                    var ms = con.xball.Where(a => a.shenxiao != null).ToList();
                    string[] ssx = ball.Split(',');
                    string sx = "";
                    string color = "";
                    string wx = "";
                    foreach (var s in ssx)
                    {
                        var mss = ms.Where(a => a.name.Trim() == s).First();
                        sx += mss.shenxiao;
                        sx += ",";
                        color += mss.color;
                        color += ",";
                        wx += mss.wuxin;
                        wx += ",";
                    }
                    sx = sx.Substring(0, sx.Length - 1);
                    color = color.Substring(0, color.Length - 1);
                    wx = wx.Substring(0, wx.Length - 1);
                    var m = con.xqisu.Where(a => a.id == id).First();
                    m.jiang = true;
                    m.ball = ball;
                    m.shengxiao = sx;
                    m.color = color;
                    m.wuxin = wx;
                    con.SubmitChanges();
                    return Json(new { type = "suc", msg = "操作成功！" });
                }
            }
            catch (Exception ex)
            {
                return Json(new { type = "err", msg = ex.Message });
            }
        }
        public ActionResult pankou_edit(int id, DateTime k_time, DateTime z_time, DateTime t_time)
        {
            try
            {
                if (id == 0)
                {
                    return Json(new { type = "err", msg = "错误：734" });
                }
                if (k_time == null || z_time == null || t_time == null)
                {
                    return Json(new { type = "err", msg = "数据输入错误！" });
                }
                if ((z_time <= k_time) || (t_time <= k_time))
                {
                    return Json(new { type = "err", msg = "封盘时间低于开盘时间！" });
                }
                using (Models.Sqlconnect con = new Models.Sqlconnect())
                {
                    var m = con.xqisu.Where(a => a.id == id).First();
                    m.k_time = k_time;
                    m.z_time = z_time;
                    m.t_time = t_time;
                    con.SubmitChanges();
                    return Json(new { type = "suc", msg = "操作成功！" });
                }
            }
            catch (Exception ex)
            {
                return Json(new { type = "err", msg = ex.Message });
            }
        }
        public ActionResult web_config(string name, string sx)
        {
            if (Session["xname"] == null)
            {
                return Json(new { type = "err", msg = "已断开，请重新登陆！" });
            }
            if (name == string.Empty)
            {
                return Json(new { type = "err", msg = "网站名称不能为空！" });
            }
            if (sx == string.Empty)
            {
                return Json(new { type = "err", msg = "错误：735" });
            }
            try
            {
                using (Models.Sqlconnect con = new Models.Sqlconnect())
                {
                    var m = con.xconfig.First();
                    m.webname = name;
                    con.SubmitChanges();
                    Api.WebClass.f_ball_senxiao(sx);
                    return Json(new { type = "suc", msg = "操作成功！" });
                }
            }
            catch (Exception ex)
            {
                return Json(new { type = "err", msg = ex.Message });
            }
        }
        [HttpPost]
        public ActionResult save_default_tuisui(string[] name, decimal[] yongjin, int[] min_e, int[] danzu_e, int[] max_e)
        {
            try
            {
                if (Session["xname"] == null)
                {
                    return Json(new { type = "err", msg = "已断开连接，请重新登陆！" });
                }
                using (Models.Sqlconnect con = new Models.Sqlconnect())
                {
                    for (int i = 0; i < name.Length; i++)
                    {
                        var m = con.xtuisui.Where(a => a.pid == 0).Where(a => a.name.Trim() == name[i]).First();
                        m.yongjin = yongjin[i];
                        m.min_e = min_e[i];
                        m.danzu_e = danzu_e[i];
                        m.max_e = max_e[i];
                        con.SubmitChanges();
                    }
                }
                return Json(new { type = "suc", msg = "操作成功！" });
            }
            catch (Exception ex)
            {
                return Json(new { type = "err", msg = ex.Message });
            }
        }
        [HttpPost]
        public ActionResult save_addnew_user(string name, string pwd, string xinmin, string tel, string xb, int m1, string gf, string gf_state, decimal[] yongjin, int[] min_e, int[] danzu_e, int[] max_e, string[] tname)
        {
            try
            {
                if (Session["xname"] == null)
                {
                    return Json(new { type = "err", msg = "连接已断开，请重新登陆！" });
                }
                decimal xgf = 0;
                if (gf != "")
                {
                    xgf = decimal.Parse(gf);
                }
                bool xgf_state = false;
                if (gf_state == "on")
                {
                    xgf_state = true;
                }
                if (name.Length < 2)
                {
                    return Json(new { type = "err", msg = "帐号名称字数小于2个，请输入2个以上字符！" });
                }
                using (Models.Sqlconnect con = new Models.Sqlconnect())
                {
                    var model = con.xusers.Where(a => a.name.Trim() == name).ToList();
                    if (model.Count > 0)
                    {
                        return Json(new { type = "err", msg = "帐号名称已经注册，请更换帐号名！" });
                    }
                    Models.xusers m = new Models.xusers();
                    m.name = name;
                    m.pwd = Api.WebClass.FunMD5(pwd);
                    m.xb = Convert.ToChar(xb);
                    m.tel = tel;
                    m.date = DateTime.Now.AddHours(Api.WebClass.plus_timer_hours);
                    m.m1 = m1;
                    m.gf = xgf;
                    m.gf_state = xgf_state;
                    m.xinmin = xinmin;
                    m.pid = 0;
                    m.pid2 = ",";
                    m.manage = false;
                    m.state = true;
                    con.xusers.InsertOnSubmit(m);
                    con.SubmitChanges();
                    var id = m.id;

                    Models.xtuisui mm;
                    for (int i = 0; i < tname.Length; i++)
                    {
                        mm = new Models.xtuisui();
                        mm.pid = id;
                        mm.name = tname[i];
                        mm.yongjin = yongjin[i];
                        mm.min_e = min_e[i];
                        mm.danzu_e = danzu_e[i];
                        mm.max_e = max_e[i];
                        con.xtuisui.InsertOnSubmit(mm);

                    }
                    con.SubmitChanges();
                    return Json(new { type = "suc", msg = "新会员添加成功！" });
                }

            }
            catch (Exception ex)
            {
                return Json(new { type = "err", msg = ex.Message });
            }
        }
        public ActionResult save_addnew_xuser(string name, string pwd, string xinmin, string tel, string xb, int m1, decimal[] yongjin, int[] min_e, int[] danzu_e, int[] max_e, string[] tname)
        {
            try
            {
                if (Session["id"] == null)
                {
                    return Json(new { type = "err", msg = "已断开连接，请重新登陆！" });
                }
                if (name.Length < 2)
                {
                    return Json(new { type = "err", msg = "帐号名称字数小于2个，请输入2个以上字符！" });
                }
                var ms = Api.WebClass.u_tuisui;
                for (int i = 0; i < tname.Length; i++)
                {
                    var m = ms.Where(a => a.name.Trim() == tname[i]).First();
                    if (yongjin[i] > (decimal)m.yongjin)
                    {
                        return Json(new { type = "err", msg = tname[i] + "@退水超过了您的设置上限！" });
                    }
                    if (min_e[i] > (decimal)m.min_e)
                    {
                        return Json(new { type = "err", msg = tname[i] + "@最小限额超过了您的设置上限！" });
                    }
                    if (danzu_e[i] > (decimal)m.danzu_e)
                    {
                        return Json(new { type = "err", msg = tname[i] + "@单注限额超过了您的设置上限！" });
                    }
                    if (max_e[i] > (decimal)m.max_e)
                    {
                        return Json(new { type = "err", msg = tname[i] + "@总项超过了您的设置上限！" });
                    }
                    if (m1 > Api.WebClass.User_info_m3)
                    {
                        return Json(new { type = "err", msg = "设置信用额度@超过了您的信用余额！" });
                    }
                }
                using (Models.Sqlconnect con = new Models.Sqlconnect())
                {
                    var model = con.xusers.Where(a => a.name.Trim() == name).ToList();
                    if (model.Count > 0)
                    {
                        return Json(new { type = "err", msg = "帐号名称已存在，请更换帐号名！" });
                    }
                    var um = con.xusers.Where(a => a.id == int.Parse(Session["id"].ToString())).First();
                    Models.xusers m = new Models.xusers();
                    m.name = name;
                    m.pwd = Api.WebClass.FunMD5(pwd);
                    m.xb = Convert.ToChar(xb);
                    m.tel = tel;
                    m.date = DateTime.Now.AddHours(Api.WebClass.plus_timer_hours);
                    m.m1 = m1;
                    m.gf = 0;
                    m.gf_state = false;
                    m.xinmin = xinmin;
                    m.pid = int.Parse(Session["id"].ToString());
                    if (um.pid2 == null)
                    {
                        m.pid2 = Session["id"].ToString() + ",";
                    }
                    else
                    {
                        m.pid2 = um.pid2.Trim() + Session["id"].ToString() + ",";
                    }
                    m.manage = false;
                    m.state = true;
                    con.xusers.InsertOnSubmit(m);
                    con.SubmitChanges();
                    var id = m.id;

                    Models.xtuisui mm;
                    for (int i = 0; i < tname.Length; i++)
                    {
                        mm = new Models.xtuisui();
                        mm.pid = id;
                        mm.name = tname[i];
                        mm.yongjin = yongjin[i];
                        mm.min_e = min_e[i];
                        mm.danzu_e = danzu_e[i];
                        mm.max_e = max_e[i];
                        con.xtuisui.InsertOnSubmit(mm);

                    }
                    con.SubmitChanges();
                    return Json(new { type = "suc", msg = "帐号添加成功！" });
                }

            }
            catch (Exception ex)
            {
                return Json(new { type = "err", msg = ex.Message });
            }
        }
        public ActionResult del_user(int id)
        {
            if (id == 0 || id == null)
            {
                return Json(new { type = "err", msg = "数据错误！" });
            }
            try
            {
                using (Models.Sqlconnect con = new Models.Sqlconnect())
                {
                    var mc = con.xusers.Where(a => a.pid == id).ToList();
                    if (mc.Count > 0)
                    {
                        return Json(new { type = "err", msg = "此帐户下存在子帐户不能删除！" });
                    }
                    var m = con.xusers.Where(a => a.id == id).ToList();
                    con.xusers.DeleteOnSubmit(m.First());
                    var mm = con.xtuisui.Where(a => a.pid == id).ToList();
                    foreach (var ms in mm)
                    {
                        con.xtuisui.DeleteOnSubmit(ms);
                    }
                    var muz = con.xxiazu.Where(a => a.pid == id).ToList();
                    foreach (var mz in muz)
                    {
                        con.xxiazu.DeleteOnSubmit(mz);
                    }
                    con.SubmitChanges();
                    return Json(new { type = "suc", msg = "操作成功！" });

                }
            }
            catch (Exception ex)
            {
                return Json(new { type = "err", msg = ex.Message });
            }
        }
        //public ActionResult xdel_xuser(int id)
        //{
        //    if (id == 0 || id == null)
        //    {
        //        return Json(new { type = "err", msg = "数据错误！" });
        //    }
        //    try
        //    {
        //        using (Models.Sqlconnect con = new Models.Sqlconnect())
        //        {
        //            var m = con.xusers.Where(a => a.id == id).Where(a => a.pid == int.Parse(Session["id"].ToString())).ToList();
        //            if (m.Count > 0)
        //            {
        //                con.xusers.DeleteOnSubmit(m.First());
        //                var mm = con.xtuisui.Where(a => a.pid == id).ToList();
        //                if (mm.Count > 0)
        //                {
        //                    foreach (var ms in mm)
        //                    {
        //                        con.xtuisui.DeleteOnSubmit(ms);
        //                    }
        //                }
        //                var mzu = con.xxiazu.Where(a => a.pid == id).ToList();
        //                foreach (var mz in mzu)
        //                {
        //                    con.xxiazu.DeleteOnSubmit(mz);
        //                }
        //                con.SubmitChanges();
        //                return Json(new { type = "suc", msg = "操作成功！" });
        //            }
        //            else
        //            {
        //                return null;
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return Json(new { type = "err", msg = ex.Message });
        //    }
        //}
        public ActionResult edit_user(int userid, string pwd, string edit_state, string xinmin, string tel, string xb, int m1, int[] tid, decimal[] yongjin, int[] min_e, int[] danzu_e, int[] max_e)
        {
            if (userid == 0 || userid == null)
            {
                return Json(new { type = "err", msg = "数据错误！" });
            }
            try
            {
                var xstate = false;
                if (edit_state == "on")
                {
                    xstate = true;
                }
                using (Models.Sqlconnect con = new Models.Sqlconnect())
                {
                    var m = con.xusers.Where(a => a.id == userid).First();
                    if (pwd != "")
                    {
                        m.pwd = Api.WebClass.FunMD5(pwd);
                    }
                    m.state = xstate;
                    m.xinmin = xinmin;
                    m.tel = tel;
                    m.xb = Convert.ToChar(xb);
                    m.m1 = m1;

                    for (int i = 0; i < tid.Length; i++)
                    {
                        var mm = con.xtuisui.Where(a => a.id == tid[i]).First();
                        mm.yongjin = yongjin[i];
                        mm.min_e = min_e[i];
                        mm.danzu_e = danzu_e[i];
                        mm.max_e = max_e[i];
                    }
                    con.SubmitChanges();
                    return Json(new { type = "suc", msg = "操作成功！" });
                }
            }
            catch (Exception ex)
            {
                return Json(new { type = "err", msg = ex.Message });
            }
        }
        public ActionResult xedit_xuser(int userid, string pwd, string xinmin, string tel, string xb, int m1, string[] tname, int[] tid, decimal[] yongjin, int[] min_e, int[] danzu_e, int[] max_e)
        {
            if (userid == 0 || userid == null)
            {
                return Json(new { type = "err", msg = "数据错误！" });
            }
            try
            {
                if (Session["id"] == null)
                {
                    return Json(new { type = "err", msg = "已断开连接，请重新登陆！" });
                }
                var ms = Api.WebClass.u_tuisui;
                for (int i = 0; i < tid.Length; i++)
                {
                    var m = ms.Where(a => a.name.Trim() == tname[i]).First();
                    if (yongjin[i] > (decimal)m.yongjin)
                    {
                        return Json(new { type = "err", msg = m.name.Trim() + "@退水超过了您的设置上限！" });
                    }
                    if (min_e[i] > (decimal)m.min_e)
                    {
                        return Json(new { type = "err", msg = m.name.Trim() + "@最小限额超过了您的设置上限！" });
                    }
                    if (danzu_e[i] > (decimal)m.danzu_e)
                    {
                        return Json(new { type = "err", msg = m.name.Trim() + "@单注限额超过了您的设置上限！" });
                    }
                    if (max_e[i] > (decimal)m.max_e)
                    {
                        return Json(new { type = "err", msg = m.name.Trim() + "@总项超过了您的设置上限！" });
                    }
                    if (m1 > Api.WebClass.User_info_m3)
                    {
                        return Json(new { type = "err", msg = "设置信用额度@超过了您的信用余额！" });
                    }
                }
                using (Models.Sqlconnect con = new Models.Sqlconnect())
                {
                    var m = con.xusers.Where(a => a.id == userid).First();
                    if (pwd != "")
                    {
                        m.pwd = Api.WebClass.FunMD5(pwd);
                    }
                    m.xinmin = xinmin;
                    m.tel = tel;
                    m.xb = Convert.ToChar(xb);
                    m.m1 = m1;

                    for (int i = 0; i < tid.Length; i++)
                    {
                        var mm = con.xtuisui.Where(a => a.id == tid[i]).First();
                        mm.yongjin = yongjin[i];
                        mm.min_e = min_e[i];
                        mm.danzu_e = danzu_e[i];
                        mm.max_e = max_e[i];
                    }
                    con.SubmitChanges();
                    return Json(new { type = "suc", msg = "操作成功！" });
                }
            }
            catch (Exception ex)
            {
                return Json(new { type = "err", msg = ex.Message });
            }
        }
        public ActionResult edit_user2(int userid, string gf, string pwd, string edit_state, string xinmin, string tel, string xb, int m1, int[] tid, decimal[] yongjin, int[] min_e, int[] danzu_e, int[] max_e)
        {
            if (userid == 0 || userid == null)
            {
                return Json(new { type = "err", msg = "数据错误！" });
            }
            try
            {
                var xstate = false;
                if (edit_state == "on")
                {
                    xstate = true;
                }
                decimal xgf = 0;
                if (gf != "")
                {
                    xgf = decimal.Parse(gf);
                }
                using (Models.Sqlconnect con = new Models.Sqlconnect())
                {
                    var m = con.xusers.Where(a => a.id == userid).First();
                    if (pwd != "")
                    {
                        m.pwd = Api.WebClass.FunMD5(pwd);
                    }
                    m.state = xstate;
                    m.xinmin = xinmin;
                    m.tel = tel;
                    m.xb = Convert.ToChar(xb);
                    m.m1 = m1;
                    m.gf = xgf;

                    for (int i = 0; i < tid.Length; i++)
                    {
                        var mm = con.xtuisui.Where(a => a.id == tid[i]).First();
                        mm.yongjin = yongjin[i];
                        mm.min_e = min_e[i];
                        mm.danzu_e = danzu_e[i];
                        mm.max_e = max_e[i];
                    }
                    con.SubmitChanges();
                    return Json(new { type = "suc", msg = "操作成功！" });
                }
            }
            catch (Exception ex)
            {
                return Json(new { type = "err", msg = ex.Message });
            }
        }
        public ActionResult edit_beilu(string panname, string[] ball, decimal[] beilu, decimal[] beilu2)
        {
            if (Session["xname"] == null)
            {
                return Json(new { type = "err", msg = "已断开连接！" });
            }
            if (panname == "")
            {
                return Json(new { type = "err", msg = "数据错误！" });
            }
            if (ball == null || beilu == null)
            {
                return Json(new { type = "err", msg = "数据错误" });
            }
            try
            {
                using (Models.Sqlconnect con = new Models.Sqlconnect())
                {
                    switch (panname)
                    {
                        #region 特码
                        case "特A":
                            for (int i = 0; i < ball.Length; i++)
                            {
                                var m = con.xball.Where(a => a.name.Trim() == ball[i]).First();
                                m.tma = beilu[i];
                                con.SubmitChanges();
                            }
                            return Json(new { type = "suc", msg = "操作成功！" });
                        case "特B":
                            for (int i = 0; i < ball.Length; i++)
                            {
                                var m = con.xball.Where(a => a.name.Trim() == ball[i]).First();
                                m.tmb = beilu[i];
                                con.SubmitChanges();
                            }
                            return Json(new { type = "suc", msg = "操作成功！" });
                        #endregion
                        #region 正码
                        case "正A":
                            for (int i = 0; i < ball.Length; i++)
                            {
                                var m = con.xball.Where(a => a.name.Trim() == ball[i]).First();
                                m.zma = beilu[i];
                                con.SubmitChanges();
                            }
                            return Json(new { type = "suc", msg = "操作成功！" });
                        case "正B":
                            for (int i = 0; i < ball.Length; i++)
                            {
                                var m = con.xball.Where(a => a.name.Trim() == ball[i]).First();
                                m.zmb = beilu[i];
                                con.SubmitChanges();
                            }
                            return Json(new { type = "suc", msg = "操作成功！" });
                        #endregion
                        #region 正码特
                        case "正1特":
                            var mo = from q in con.xball
                                     where (q.color != null || q.type == 813)
                                     select q;
                            for (int i = 0; i < ball.Length; i++)
                            {
                                var m = mo.Where(a => a.name.Trim() == ball[i]).First();
                                m.zt1 = beilu[i];
                            }
                            con.SubmitChanges();
                            return Json(new { type = "suc", msg = "操作成功！" });
                        case "正2特":
                            mo = from q in con.xball
                                 where (q.color != null || q.type == 813)
                                 select q;
                            for (int i = 0; i < ball.Length; i++)
                            {
                                var m = mo.Where(a => a.name.Trim() == ball[i]).First();
                                m.zt2 = beilu[i];
                            }
                            con.SubmitChanges();
                            return Json(new { type = "suc", msg = "操作成功！" });
                        case "正3特":
                            mo = from q in con.xball
                                 where (q.color != null || q.type == 813)
                                 select q;
                            for (int i = 0; i < ball.Length; i++)
                            {
                                var m = mo.Where(a => a.name.Trim() == ball[i]).First();
                                m.zt3 = beilu[i];
                            }
                            con.SubmitChanges();
                            return Json(new { type = "suc", msg = "操作成功！" });
                        case "正4特":
                            mo = from q in con.xball
                                 where (q.color != null || q.type == 813)
                                 select q;
                            for (int i = 0; i < ball.Length; i++)
                            {
                                var m = mo.Where(a => a.name.Trim() == ball[i]).First();
                                m.zt4 = beilu[i];
                            }
                            con.SubmitChanges();
                            return Json(new { type = "suc", msg = "操作成功！" });
                        case "正5特":
                            mo = from q in con.xball
                                 where (q.color != null || q.type == 813)
                                 select q;
                            for (int i = 0; i < ball.Length; i++)
                            {
                                var m = mo.Where(a => a.name.Trim() == ball[i]).First();
                                m.zt5 = beilu[i];
                            }
                            con.SubmitChanges();
                            return Json(new { type = "suc", msg = "操作成功！" });
                        case "正6特":
                            mo = from q in con.xball
                                 where (q.color != null || q.type == 813)
                                 select q;
                            for (int i = 0; i < ball.Length; i++)
                            {
                                var m = mo.Where(a => a.name.Trim() == ball[i]).First();
                                m.zt6 = beilu[i];
                            }
                            con.SubmitChanges();
                            return Json(new { type = "suc", msg = "操作成功！" });
                        #endregion
                        #region 连码
                        case "二全中":
                            for (int i = 0; i < ball.Length; i++)
                            {
                                var m = con.xball.Where(a => a.name.Trim() == ball[i]).First();
                                m.s2qz = beilu[i];
                                con.SubmitChanges();
                            }
                            return Json(new { type = "suc", msg = "操作成功！" });
                        case "二中特":
                            for (int i = 0; i < ball.Length; i++)
                            {
                                var m = con.xball.Where(a => a.name.Trim() == ball[i]).First();
                                m.s2zt = beilu[i];
                                m.s2zt2 = beilu2[i];
                                con.SubmitChanges();
                            }
                            return Json(new { type = "suc", msg = "操作成功！" });
                        case "特串":
                            for (int i = 0; i < ball.Length; i++)
                            {
                                var m = con.xball.Where(a => a.name.Trim() == ball[i]).First();
                                m.stc = beilu[i];
                                con.SubmitChanges();
                            }
                            return Json(new { type = "suc", msg = "操作成功！" });
                        case "三全中":
                            for (int i = 0; i < ball.Length; i++)
                            {
                                var m = con.xball.Where(a => a.name.Trim() == ball[i]).First();
                                m.s3qz = beilu[i];
                                con.SubmitChanges();
                            }
                            return Json(new { type = "suc", msg = "操作成功！" });
                        case "三中二":
                            for (int i = 0; i < ball.Length; i++)
                            {
                                var m = con.xball.Where(a => a.name.Trim() == ball[i]).First();
                                m.s3z2 = beilu[i];
                                m.s3z22 = beilu2[i];
                                con.SubmitChanges();
                            }
                            return Json(new { type = "suc", msg = "操作成功！" });
                        #endregion
                        #region 多选不中
                        case "五不中":
                            for (int i = 0; i < ball.Length; i++)
                            {
                                var m = con.xball.Where(a => a.name.Trim() == ball[i]).First();
                                m.db5 = beilu[i];
                                con.SubmitChanges();
                            }
                            return Json(new { type = "suc", msg = "操作成功！" });
                        case "六不中":
                            for (int i = 0; i < ball.Length; i++)
                            {
                                var m = con.xball.Where(a => a.name.Trim() == ball[i]).First();
                                m.db6 = beilu[i];
                                con.SubmitChanges();
                            }
                            return Json(new { type = "suc", msg = "操作成功！" });
                        case "七不中":
                            for (int i = 0; i < ball.Length; i++)
                            {
                                var m = con.xball.Where(a => a.name.Trim() == ball[i]).First();
                                m.db7 = beilu[i];
                                con.SubmitChanges();
                            }
                            return Json(new { type = "suc", msg = "操作成功！" });
                        case "八不中":
                            for (int i = 0; i < ball.Length; i++)
                            {
                                var m = con.xball.Where(a => a.name.Trim() == ball[i]).First();
                                m.db8 = beilu[i];
                                con.SubmitChanges();
                            }
                            return Json(new { type = "suc", msg = "操作成功！" });
                        case "九不中":
                            for (int i = 0; i < ball.Length; i++)
                            {
                                var m = con.xball.Where(a => a.name.Trim() == ball[i]).First();
                                m.db9 = beilu[i];
                                con.SubmitChanges();
                            }
                            return Json(new { type = "suc", msg = "操作成功！" });
                        case "十不中":
                            for (int i = 0; i < ball.Length; i++)
                            {
                                var m = con.xball.Where(a => a.name.Trim() == ball[i]).First();
                                m.db10 = beilu[i];
                                con.SubmitChanges();
                            }
                            return Json(new { type = "suc", msg = "操作成功！" });
                        case "十二不中":
                            for (int i = 0; i < ball.Length; i++)
                            {
                                var m = con.xball.Where(a => a.name.Trim() == ball[i]).First();
                                m.db12 = beilu[i];
                                con.SubmitChanges();
                            }
                            return Json(new { type = "suc", msg = "操作成功！" });
                        case "十五不中":
                            for (int i = 0; i < ball.Length; i++)
                            {
                                var m = con.xball.Where(a => a.name.Trim() == ball[i]).First();
                                m.db15 = beilu[i];
                                con.SubmitChanges();
                            }
                            return Json(new { type = "suc", msg = "操作成功！" });
                        #endregion
                        #region 多选中一
                        case "五中一":
                            for (int i = 0; i < ball.Length; i++)
                            {
                                var m = con.xball.Where(a => a.name.Trim() == ball[i]).First();
                                m.dz5 = beilu[i];
                                con.SubmitChanges();
                            }
                            return Json(new { type = "suc", msg = "操作成功！" });
                        case "六中一":
                            for (int i = 0; i < ball.Length; i++)
                            {
                                var m = con.xball.Where(a => a.name.Trim() == ball[i]).First();
                                m.dz6 = beilu[i];
                                con.SubmitChanges();
                            }
                            return Json(new { type = "suc", msg = "操作成功！" });
                        case "七中一":
                            for (int i = 0; i < ball.Length; i++)
                            {
                                var m = con.xball.Where(a => a.name.Trim() == ball[i]).First();
                                m.dz7 = beilu[i];
                                con.SubmitChanges();
                            }
                            return Json(new { type = "suc", msg = "操作成功！" });
                        case "八中一":
                            for (int i = 0; i < ball.Length; i++)
                            {
                                var m = con.xball.Where(a => a.name.Trim() == ball[i]).First();
                                m.dz8 = beilu[i];
                                con.SubmitChanges();
                            }
                            return Json(new { type = "suc", msg = "操作成功！" });
                        case "九中一":
                            for (int i = 0; i < ball.Length; i++)
                            {
                                var m = con.xball.Where(a => a.name.Trim() == ball[i]).First();
                                m.dz9 = beilu[i];
                                con.SubmitChanges();
                            }
                            return Json(new { type = "suc", msg = "操作成功！" });
                        case "十中一":
                            for (int i = 0; i < ball.Length; i++)
                            {
                                var m = con.xball.Where(a => a.name.Trim() == ball[i]).First();
                                m.dz10 = beilu[i];
                                con.SubmitChanges();
                            }
                            return Json(new { type = "suc", msg = "操作成功！" });
                        #endregion
                        #region 特码项目
                        case "特肖":
                            for (int i = 0; i < ball.Length; i++)
                            {
                                var m = con.xball.Where(a => a.name.Trim() == ball[i]).First();
                                m.tiex = beilu[i];
                                con.SubmitChanges();
                            }
                            return Json(new { type = "suc", msg = "操作成功！" });
                        case "半波":
                            for (int i = 0; i < ball.Length; i++)
                            {
                                var m = con.xball.Where(a => a.name.Trim() == ball[i]).First();
                                m.babo = beilu[i];
                                con.SubmitChanges();
                            }
                            return Json(new { type = "suc", msg = "操作成功！" });
                        case "五行":
                            for (int i = 0; i < ball.Length; i++)
                            {
                                var m = con.xball.Where(a => a.name.Trim() == ball[i]).First();
                                m.n5xin = beilu[i];
                                con.SubmitChanges();
                            }
                            return Json(new { type = "suc", msg = "操作成功！" });
                        #endregion
                        #region 生肖项目
                        case "一肖":
                            for (int i = 0; i < ball.Length; i++)
                            {
                                var m = con.xball.Where(a => a.name.Trim() == ball[i]).First();
                                m.yix = beilu[i];
                                con.SubmitChanges();
                            }
                            return Json(new { type = "suc", msg = "操作成功！" });
                        case "一肖不中":
                            for (int i = 0; i < ball.Length; i++)
                            {
                                var m = con.xball.Where(a => a.name.Trim() == ball[i]).First();
                                m.yixb = beilu[i];
                                con.SubmitChanges();
                            }
                            return Json(new { type = "suc", msg = "操作成功！" });
                        case "六肖":
                            for (int i = 0; i < ball.Length; i++)
                            {
                                var m = con.xball.Where(a => a.name.Trim() == ball[i]).First();
                                m.liux = beilu[i];
                                con.SubmitChanges();
                            }
                            return Json(new { type = "suc", msg = "操作成功！" });
                        case "六肖不中":
                            for (int i = 0; i < ball.Length; i++)
                            {
                                var m = con.xball.Where(a => a.name.Trim() == ball[i]).First();
                                m.liuxb = beilu[i];
                                con.SubmitChanges();
                            }
                            return Json(new { type = "suc", msg = "操作成功！" });
                        case "生肖量":
                            for (int i = 0; i < ball.Length; i++)
                            {
                                var m = con.xball.Where(a => a.name.Trim() == ball[i]).First();
                                m.nsxl = beilu[i];
                                con.SubmitChanges();
                            }
                            return Json(new { type = "suc", msg = "操作成功！" });
                        #endregion
                        #region 总数项目
                        case "尾数":
                            for (int i = 0; i < ball.Length; i++)
                            {
                                var m = con.xball.Where(a => a.name.Trim() == ball[i]).First();
                                m.ws = beilu[i];
                                con.SubmitChanges();
                            }
                            return Json(new { type = "suc", msg = "操作成功！" });
                        case "尾数不中":
                            for (int i = 0; i < ball.Length; i++)
                            {
                                var m = con.xball.Where(a => a.name.Trim() == ball[i]).First();
                                m.wsb = beilu[i];
                                con.SubmitChanges();
                            }
                            return Json(new { type = "suc", msg = "操作成功！" });
                        case "尾数量":
                            for (int i = 0; i < ball.Length; i++)
                            {
                                var m = con.xball.Where(a => a.name.Trim() == ball[i]).First();
                                m.nwsl = beilu[i];
                                con.SubmitChanges();
                            }
                            return Json(new { type = "suc", msg = "操作成功！" });
                        #endregion
                        #region 生肖连
                        case "二肖中":
                            for (int i = 0; i < ball.Length; i++)
                            {
                                var m = con.xball.Where(a => a.name.Trim() == ball[i]).First();
                                m.x2 = beilu[i];
                                con.SubmitChanges();
                            }
                            return Json(new { type = "suc", msg = "操作成功！" });
                        case "三肖中":
                            for (int i = 0; i < ball.Length; i++)
                            {
                                var m = con.xball.Where(a => a.name.Trim() == ball[i]).First();
                                m.x3 = beilu[i];
                                con.SubmitChanges();
                            }
                            return Json(new { type = "suc", msg = "操作成功！" });
                        case "四肖中":
                            for (int i = 0; i < ball.Length; i++)
                            {
                                var m = con.xball.Where(a => a.name.Trim() == ball[i]).First();
                                m.x4 = beilu[i];
                                con.SubmitChanges();
                            }
                            return Json(new { type = "suc", msg = "操作成功！" });
                        case "五肖中":
                            for (int i = 0; i < ball.Length; i++)
                            {
                                var m = con.xball.Where(a => a.name.Trim() == ball[i]).First();
                                m.x5 = beilu[i];
                                con.SubmitChanges();
                            }
                            return Json(new { type = "suc", msg = "操作成功！" });
                        case "二肖不中":
                            for (int i = 0; i < ball.Length; i++)
                            {
                                var m = con.xball.Where(a => a.name.Trim() == ball[i]).First();
                                m.x2b = beilu[i];
                                con.SubmitChanges();
                            }
                            return Json(new { type = "suc", msg = "操作成功！" });
                        case "三肖不中":
                            for (int i = 0; i < ball.Length; i++)
                            {
                                var m = con.xball.Where(a => a.name.Trim() == ball[i]).First();
                                m.x3b = beilu[i];
                                con.SubmitChanges();
                            }
                            return Json(new { type = "suc", msg = "操作成功！" });
                        case "四肖不中":
                            for (int i = 0; i < ball.Length; i++)
                            {
                                var m = con.xball.Where(a => a.name.Trim() == ball[i]).First();
                                m.x4b = beilu[i];
                                con.SubmitChanges();
                            }
                            return Json(new { type = "suc", msg = "操作成功！" });
                        case "五肖不中":
                            for (int i = 0; i < ball.Length; i++)
                            {
                                var m = con.xball.Where(a => a.name.Trim() == ball[i]).First();
                                m.x5b = beilu[i];
                                con.SubmitChanges();
                            }
                            return Json(new { type = "suc", msg = "操作成功！" });
                        #endregion
                        #region 尾数连
                        case "二尾中":
                            for (int i = 0; i < ball.Length; i++)
                            {
                                string theball = ball[i].Replace("尾", "");
                                var m = con.xball.Where(a => a.name.Trim() == theball).First();
                                m.w2 = beilu[i];
                                con.SubmitChanges();
                            }
                            return Json(new { type = "suc", msg = "操作成功！" });
                        case "三尾中":
                            for (int i = 0; i < ball.Length; i++)
                            {
                                string theball = ball[i].Replace("尾", "");
                                var m = con.xball.Where(a => a.name.Trim() == theball).First();
                                m.w3 = beilu[i];
                                con.SubmitChanges();
                            }
                            return Json(new { type = "suc", msg = "操作成功！" });
                        case "四尾中":
                            for (int i = 0; i < ball.Length; i++)
                            {
                                string theball = ball[i].Replace("尾", "");
                                var m = con.xball.Where(a => a.name.Trim() == theball).First();
                                m.w4 = beilu[i];
                                con.SubmitChanges();
                            }
                            return Json(new { type = "suc", msg = "操作成功！" });
                        case "五尾中":
                            for (int i = 0; i < ball.Length; i++)
                            {
                                string theball = ball[i].Replace("尾", "");
                                var m = con.xball.Where(a => a.name.Trim() == theball).First();
                                m.w5 = beilu[i];
                                con.SubmitChanges();
                            }
                            return Json(new { type = "suc", msg = "操作成功！" });
                        case "二尾不中":
                            for (int i = 0; i < ball.Length; i++)
                            {
                                string theball = ball[i].Replace("尾", "");
                                var m = con.xball.Where(a => a.name.Trim() == theball).First();
                                m.w2b = beilu[i];
                                con.SubmitChanges();
                            }
                            return Json(new { type = "suc", msg = "操作成功！" });
                        case "三尾不中":
                            for (int i = 0; i < ball.Length; i++)
                            {
                                string theball = ball[i].Replace("尾", "");
                                var m = con.xball.Where(a => a.name.Trim() == theball).First();
                                m.w3b = beilu[i];
                                con.SubmitChanges();
                            }
                            return Json(new { type = "suc", msg = "操作成功！" });
                        case "四尾不中":
                            for (int i = 0; i < ball.Length; i++)
                            {
                                string theball = ball[i].Replace("尾", "");
                                var m = con.xball.Where(a => a.name.Trim() == theball).First();
                                m.w4b = beilu[i];
                                con.SubmitChanges();
                            }
                            return Json(new { type = "suc", msg = "操作成功！" });
                        case "五尾不中":
                            for (int i = 0; i < ball.Length; i++)
                            {
                                string theball = ball[i].Replace("尾", "");
                                var m = con.xball.Where(a => a.name.Trim() == theball).First();
                                m.w5b = beilu[i];
                                con.SubmitChanges();
                            }
                            return Json(new { type = "suc", msg = "操作成功！" });
                        #endregion
                        #region 特平中
                        case "一粒任中":
                            for (int i = 0; i < ball.Length; i++)
                            {
                                var m = con.xball.Where(a => a.name.Trim() == ball[i]).First();
                                m.rz1 = beilu[i];
                                con.SubmitChanges();
                            }
                            return Json(new { type = "suc", msg = "操作成功！" });
                        case "二粒任中":
                            for (int i = 0; i < ball.Length; i++)
                            {
                                var m = con.xball.Where(a => a.name.Trim() == ball[i]).First();
                                m.rz2 = beilu[i];
                                con.SubmitChanges();
                            }
                            return Json(new { type = "suc", msg = "操作成功！" });
                        case "三粒任中":
                            for (int i = 0; i < ball.Length; i++)
                            {
                                var m = con.xball.Where(a => a.name.Trim() == ball[i]).First();
                                m.rz3 = beilu[i];
                                con.SubmitChanges();
                            }
                            return Json(new { type = "suc", msg = "操作成功！" });
                        case "四粒任中":
                            for (int i = 0; i < ball.Length; i++)
                            {
                                var m = con.xball.Where(a => a.name.Trim() == ball[i]).First();
                                m.rz4 = beilu[i];
                                con.SubmitChanges();
                            }
                            return Json(new { type = "suc", msg = "操作成功！" });
                        case "五粒任中":
                            for (int i = 0; i < ball.Length; i++)
                            {
                                var m = con.xball.Where(a => a.name.Trim() == ball[i]).First();
                                m.rz5 = beilu[i];
                                con.SubmitChanges();
                            }
                            return Json(new { type = "suc", msg = "操作成功！" });
                        #endregion

                        default:
                            return Json(new { type = "err", msg = "错误：779" });
                    }
                }
            }
            catch (Exception ex)
            {
                return Json(new { type = "err", msg = ex.Message });
            }
        }
        public ActionResult edit_beilu_default(string panname, string[] ball, decimal[] beilu, decimal[] beilu2)
        {
            if (Session["xname"] == null)
            {
                return Json(new { type = "err", msg = "已断开连接！" });
            }
            if (panname == "")
            {
                return Json(new { type = "err", msg = "数据错误！" });
            }
            if (ball == null || beilu == null)
            {
                return Json(new { type = "err", msg = "数据错误" });
            }
            try
            {
                using (Models.Sqlconnect con = new Models.Sqlconnect())
                {
                    switch (panname)
                    {
                        #region 特码
                        case "特A":
                            for (int i = 0; i < ball.Length; i++)
                            {
                                var m = con.xball_copy.Where(a => a.name.Trim() == ball[i]).First();
                                m.tma = beilu[i];
                                con.SubmitChanges();
                            }
                            return Json(new { type = "suc", msg = "操作成功！" });
                        case "特B":
                            for (int i = 0; i < ball.Length; i++)
                            {
                                var m = con.xball_copy.Where(a => a.name.Trim() == ball[i]).First();
                                m.tmb = beilu[i];
                                con.SubmitChanges();
                            }
                            return Json(new { type = "suc", msg = "操作成功！" });
                        #endregion
                        #region 正码
                        case "正A":
                            for (int i = 0; i < ball.Length; i++)
                            {
                                var m = con.xball_copy.Where(a => a.name.Trim() == ball[i]).First();
                                m.zma = beilu[i];
                                con.SubmitChanges();
                            }
                            return Json(new { type = "suc", msg = "操作成功！" });
                        case "正B":
                            for (int i = 0; i < ball.Length; i++)
                            {
                                var m = con.xball_copy.Where(a => a.name.Trim() == ball[i]).First();
                                m.zmb = beilu[i];
                                con.SubmitChanges();
                            }
                            return Json(new { type = "suc", msg = "操作成功！" });
                        #endregion
                        #region 正码特
                        case "正1特":
                            var mo = from q in con.xball_copy
                                     where (q.color != null || q.type == 813)
                                     select q;
                            for (int i = 0; i < ball.Length; i++)
                            {
                                var m = mo.Where(a => a.name.Trim() == ball[i]).First();
                                m.zt1 = beilu[i];
                            }
                            con.SubmitChanges();
                            return Json(new { type = "suc", msg = "操作成功！" });
                        case "正2特":
                            mo = from q in con.xball_copy
                                 where (q.color != null || q.type == 813)
                                 select q;
                            for (int i = 0; i < ball.Length; i++)
                            {
                                var m = mo.Where(a => a.name.Trim() == ball[i]).First();
                                m.zt2 = beilu[i];
                            }
                            con.SubmitChanges();
                            return Json(new { type = "suc", msg = "操作成功！" });
                        case "正3特":
                            mo = from q in con.xball_copy
                                 where (q.color != null || q.type == 813)
                                 select q;
                            for (int i = 0; i < ball.Length; i++)
                            {
                                var m = mo.Where(a => a.name.Trim() == ball[i]).First();
                                m.zt3 = beilu[i];
                            }
                            con.SubmitChanges();
                            return Json(new { type = "suc", msg = "操作成功！" });
                        case "正4特":
                            mo = from q in con.xball_copy
                                 where (q.color != null || q.type == 813)
                                 select q;
                            for (int i = 0; i < ball.Length; i++)
                            {
                                var m = mo.Where(a => a.name.Trim() == ball[i]).First();
                                m.zt4 = beilu[i];
                            }
                            con.SubmitChanges();
                            return Json(new { type = "suc", msg = "操作成功！" });
                        case "正5特":
                            mo = from q in con.xball_copy
                                 where (q.color != null || q.type == 813)
                                 select q;
                            for (int i = 0; i < ball.Length; i++)
                            {
                                var m = mo.Where(a => a.name.Trim() == ball[i]).First();
                                m.zt5 = beilu[i];
                            }
                            con.SubmitChanges();
                            return Json(new { type = "suc", msg = "操作成功！" });
                        case "正6特":
                            mo = from q in con.xball_copy
                                 where (q.color != null || q.type == 813)
                                 select q;
                            for (int i = 0; i < ball.Length; i++)
                            {
                                var m = mo.Where(a => a.name.Trim() == ball[i]).First();
                                m.zt6 = beilu[i];
                            }
                            con.SubmitChanges();
                            return Json(new { type = "suc", msg = "操作成功！" });
                        #endregion
                        #region 连码
                        case "二全中":
                            for (int i = 0; i < ball.Length; i++)
                            {
                                var m = con.xball_copy.Where(a => a.name.Trim() == ball[i]).First();
                                m.s2qz = beilu[i];
                                con.SubmitChanges();
                            }
                            return Json(new { type = "suc", msg = "操作成功！" });
                        case "二中特":
                            for (int i = 0; i < ball.Length; i++)
                            {
                                var m = con.xball_copy.Where(a => a.name.Trim() == ball[i]).First();
                                m.s2zt = beilu[i];
                                m.s2zt2 = beilu2[i];
                                con.SubmitChanges();
                            }
                            return Json(new { type = "suc", msg = "操作成功！" });
                        case "特串":
                            for (int i = 0; i < ball.Length; i++)
                            {
                                var m = con.xball_copy.Where(a => a.name.Trim() == ball[i]).First();
                                m.stc = beilu[i];
                                con.SubmitChanges();
                            }
                            return Json(new { type = "suc", msg = "操作成功！" });
                        case "三全中":
                            for (int i = 0; i < ball.Length; i++)
                            {
                                var m = con.xball_copy.Where(a => a.name.Trim() == ball[i]).First();
                                m.s3qz = beilu[i];
                                con.SubmitChanges();
                            }
                            return Json(new { type = "suc", msg = "操作成功！" });
                        case "三中二":
                            for (int i = 0; i < ball.Length; i++)
                            {
                                var m = con.xball_copy.Where(a => a.name.Trim() == ball[i]).First();
                                m.s3z2 = beilu[i];
                                m.s3z22 = beilu2[i];
                                con.SubmitChanges();
                            }
                            return Json(new { type = "suc", msg = "操作成功！" });
                        #endregion
                        #region 多选不中
                        case "五不中":
                            for (int i = 0; i < ball.Length; i++)
                            {
                                var m = con.xball_copy.Where(a => a.name.Trim() == ball[i]).First();
                                m.db5 = beilu[i];
                                con.SubmitChanges();
                            }
                            return Json(new { type = "suc", msg = "操作成功！" });
                        case "六不中":
                            for (int i = 0; i < ball.Length; i++)
                            {
                                var m = con.xball_copy.Where(a => a.name.Trim() == ball[i]).First();
                                m.db6 = beilu[i];
                                con.SubmitChanges();
                            }
                            return Json(new { type = "suc", msg = "操作成功！" });
                        case "七不中":
                            for (int i = 0; i < ball.Length; i++)
                            {
                                var m = con.xball_copy.Where(a => a.name.Trim() == ball[i]).First();
                                m.db7 = beilu[i];
                                con.SubmitChanges();
                            }
                            return Json(new { type = "suc", msg = "操作成功！" });
                        case "八不中":
                            for (int i = 0; i < ball.Length; i++)
                            {
                                var m = con.xball_copy.Where(a => a.name.Trim() == ball[i]).First();
                                m.db8 = beilu[i];
                                con.SubmitChanges();
                            }
                            return Json(new { type = "suc", msg = "操作成功！" });
                        case "九不中":
                            for (int i = 0; i < ball.Length; i++)
                            {
                                var m = con.xball_copy.Where(a => a.name.Trim() == ball[i]).First();
                                m.db9 = beilu[i];
                                con.SubmitChanges();
                            }
                            return Json(new { type = "suc", msg = "操作成功！" });
                        case "十不中":
                            for (int i = 0; i < ball.Length; i++)
                            {
                                var m = con.xball_copy.Where(a => a.name.Trim() == ball[i]).First();
                                m.db10 = beilu[i];
                                con.SubmitChanges();
                            }
                            return Json(new { type = "suc", msg = "操作成功！" });
                        case "十二不中":
                            for (int i = 0; i < ball.Length; i++)
                            {
                                var m = con.xball_copy.Where(a => a.name.Trim() == ball[i]).First();
                                m.db12 = beilu[i];
                                con.SubmitChanges();
                            }
                            return Json(new { type = "suc", msg = "操作成功！" });
                        case "十五不中":
                            for (int i = 0; i < ball.Length; i++)
                            {
                                var m = con.xball_copy.Where(a => a.name.Trim() == ball[i]).First();
                                m.db15 = beilu[i];
                                con.SubmitChanges();
                            }
                            return Json(new { type = "suc", msg = "操作成功！" });
                        #endregion
                        #region 多选中一
                        case "五中一":
                            for (int i = 0; i < ball.Length; i++)
                            {
                                var m = con.xball_copy.Where(a => a.name.Trim() == ball[i]).First();
                                m.dz5 = beilu[i];
                                con.SubmitChanges();
                            }
                            return Json(new { type = "suc", msg = "操作成功！" });
                        case "六中一":
                            for (int i = 0; i < ball.Length; i++)
                            {
                                var m = con.xball_copy.Where(a => a.name.Trim() == ball[i]).First();
                                m.dz6 = beilu[i];
                                con.SubmitChanges();
                            }
                            return Json(new { type = "suc", msg = "操作成功！" });
                        case "七中一":
                            for (int i = 0; i < ball.Length; i++)
                            {
                                var m = con.xball_copy.Where(a => a.name.Trim() == ball[i]).First();
                                m.dz7 = beilu[i];
                                con.SubmitChanges();
                            }
                            return Json(new { type = "suc", msg = "操作成功！" });
                        case "八中一":
                            for (int i = 0; i < ball.Length; i++)
                            {
                                var m = con.xball_copy.Where(a => a.name.Trim() == ball[i]).First();
                                m.dz8 = beilu[i];
                                con.SubmitChanges();
                            }
                            return Json(new { type = "suc", msg = "操作成功！" });
                        case "九中一":
                            for (int i = 0; i < ball.Length; i++)
                            {
                                var m = con.xball_copy.Where(a => a.name.Trim() == ball[i]).First();
                                m.dz9 = beilu[i];
                                con.SubmitChanges();
                            }
                            return Json(new { type = "suc", msg = "操作成功！" });
                        case "十中一":
                            for (int i = 0; i < ball.Length; i++)
                            {
                                var m = con.xball_copy.Where(a => a.name.Trim() == ball[i]).First();
                                m.dz10 = beilu[i];
                                con.SubmitChanges();
                            }
                            return Json(new { type = "suc", msg = "操作成功！" });
                        #endregion
                        #region 特码项目
                        case "特肖":
                            for (int i = 0; i < ball.Length; i++)
                            {
                                var m = con.xball_copy.Where(a => a.name.Trim() == ball[i]).First();
                                m.tiex = beilu[i];
                                con.SubmitChanges();
                            }
                            return Json(new { type = "suc", msg = "操作成功！" });
                        case "半波":
                            for (int i = 0; i < ball.Length; i++)
                            {
                                var m = con.xball_copy.Where(a => a.name.Trim() == ball[i]).First();
                                m.babo = beilu[i];
                                con.SubmitChanges();
                            }
                            return Json(new { type = "suc", msg = "操作成功！" });
                        case "五行":
                            for (int i = 0; i < ball.Length; i++)
                            {
                                var m = con.xball_copy.Where(a => a.name.Trim() == ball[i]).First();
                                m.n5xin = beilu[i];
                                con.SubmitChanges();
                            }
                            return Json(new { type = "suc", msg = "操作成功！" });
                        #endregion
                        #region 生肖项目
                        case "一肖":
                            for (int i = 0; i < ball.Length; i++)
                            {
                                var m = con.xball_copy.Where(a => a.name.Trim() == ball[i]).First();
                                m.yix = beilu[i];
                                con.SubmitChanges();
                            }
                            return Json(new { type = "suc", msg = "操作成功！" });
                        case "一肖不中":
                            for (int i = 0; i < ball.Length; i++)
                            {
                                var m = con.xball_copy.Where(a => a.name.Trim() == ball[i]).First();
                                m.yixb = beilu[i];
                                con.SubmitChanges();
                            }
                            return Json(new { type = "suc", msg = "操作成功！" });
                        case "六肖":
                            for (int i = 0; i < ball.Length; i++)
                            {
                                var m = con.xball_copy.Where(a => a.name.Trim() == ball[i]).First();
                                m.liux = beilu[i];
                                con.SubmitChanges();
                            }
                            return Json(new { type = "suc", msg = "操作成功！" });
                        case "六肖不中":
                            for (int i = 0; i < ball.Length; i++)
                            {
                                var m = con.xball_copy.Where(a => a.name.Trim() == ball[i]).First();
                                m.liuxb = beilu[i];
                                con.SubmitChanges();
                            }
                            return Json(new { type = "suc", msg = "操作成功！" });
                        case "生肖量":
                            for (int i = 0; i < ball.Length; i++)
                            {
                                var m = con.xball_copy.Where(a => a.name.Trim() == ball[i]).First();
                                m.nsxl = beilu[i];
                                con.SubmitChanges();
                            }
                            return Json(new { type = "suc", msg = "操作成功！" });
                        #endregion
                        #region 总数项目
                        case "尾数":
                            for (int i = 0; i < ball.Length; i++)
                            {
                                var m = con.xball_copy.Where(a => a.name.Trim() == ball[i]).First();
                                m.ws = beilu[i];
                                con.SubmitChanges();
                            }
                            return Json(new { type = "suc", msg = "操作成功！" });
                        case "尾数不中":
                            for (int i = 0; i < ball.Length; i++)
                            {
                                var m = con.xball_copy.Where(a => a.name.Trim() == ball[i]).First();
                                m.wsb = beilu[i];
                                con.SubmitChanges();
                            }
                            return Json(new { type = "suc", msg = "操作成功！" });
                        case "尾数量":
                            for (int i = 0; i < ball.Length; i++)
                            {
                                var m = con.xball_copy.Where(a => a.name.Trim() == ball[i]).First();
                                m.nwsl = beilu[i];
                                con.SubmitChanges();
                            }
                            return Json(new { type = "suc", msg = "操作成功！" });
                        #endregion
                        #region 生肖连
                        case "二肖中":
                            for (int i = 0; i < ball.Length; i++)
                            {
                                var m = con.xball_copy.Where(a => a.name.Trim() == ball[i]).First();
                                m.x2 = beilu[i];
                                con.SubmitChanges();
                            }
                            return Json(new { type = "suc", msg = "操作成功！" });
                        case "三肖中":
                            for (int i = 0; i < ball.Length; i++)
                            {
                                var m = con.xball_copy.Where(a => a.name.Trim() == ball[i]).First();
                                m.x3 = beilu[i];
                                con.SubmitChanges();
                            }
                            return Json(new { type = "suc", msg = "操作成功！" });
                        case "四肖中":
                            for (int i = 0; i < ball.Length; i++)
                            {
                                var m = con.xball_copy.Where(a => a.name.Trim() == ball[i]).First();
                                m.x4 = beilu[i];
                                con.SubmitChanges();
                            }
                            return Json(new { type = "suc", msg = "操作成功！" });
                        case "五肖中":
                            for (int i = 0; i < ball.Length; i++)
                            {
                                var m = con.xball_copy.Where(a => a.name.Trim() == ball[i]).First();
                                m.x5 = beilu[i];
                                con.SubmitChanges();
                            }
                            return Json(new { type = "suc", msg = "操作成功！" });
                        case "二肖不中":
                            for (int i = 0; i < ball.Length; i++)
                            {
                                var m = con.xball_copy.Where(a => a.name.Trim() == ball[i]).First();
                                m.x2b = beilu[i];
                                con.SubmitChanges();
                            }
                            return Json(new { type = "suc", msg = "操作成功！" });
                        case "三肖不中":
                            for (int i = 0; i < ball.Length; i++)
                            {
                                var m = con.xball_copy.Where(a => a.name.Trim() == ball[i]).First();
                                m.x3b = beilu[i];
                                con.SubmitChanges();
                            }
                            return Json(new { type = "suc", msg = "操作成功！" });
                        case "四肖不中":
                            for (int i = 0; i < ball.Length; i++)
                            {
                                var m = con.xball_copy.Where(a => a.name.Trim() == ball[i]).First();
                                m.x4b = beilu[i];
                                con.SubmitChanges();
                            }
                            return Json(new { type = "suc", msg = "操作成功！" });
                        case "五肖不中":
                            for (int i = 0; i < ball.Length; i++)
                            {
                                var m = con.xball_copy.Where(a => a.name.Trim() == ball[i]).First();
                                m.x5b = beilu[i];
                                con.SubmitChanges();
                            }
                            return Json(new { type = "suc", msg = "操作成功！" });
                        #endregion
                        #region 尾数连
                        case "二尾中":
                            for (int i = 0; i < ball.Length; i++)
                            {
                                string theball = ball[i].Replace("尾", "");
                                var m = con.xball_copy.Where(a => a.name.Trim() == theball).First();
                                m.w2 = beilu[i];
                                con.SubmitChanges();
                            }
                            return Json(new { type = "suc", msg = "操作成功！" });
                        case "三尾中":
                            for (int i = 0; i < ball.Length; i++)
                            {
                                string theball = ball[i].Replace("尾", "");
                                var m = con.xball_copy.Where(a => a.name.Trim() == theball).First();
                                m.w3 = beilu[i];
                                con.SubmitChanges();
                            }
                            return Json(new { type = "suc", msg = "操作成功！" });
                        case "四尾中":
                            for (int i = 0; i < ball.Length; i++)
                            {
                                string theball = ball[i].Replace("尾", "");
                                var m = con.xball_copy.Where(a => a.name.Trim() == theball).First();
                                m.w4 = beilu[i];
                                con.SubmitChanges();
                            }
                            return Json(new { type = "suc", msg = "操作成功！" });
                        case "五尾中":
                            for (int i = 0; i < ball.Length; i++)
                            {
                                string theball = ball[i].Replace("尾", "");
                                var m = con.xball_copy.Where(a => a.name.Trim() == theball).First();
                                m.w5 = beilu[i];
                                con.SubmitChanges();
                            }
                            return Json(new { type = "suc", msg = "操作成功！" });
                        case "二尾不中":
                            for (int i = 0; i < ball.Length; i++)
                            {
                                string theball = ball[i].Replace("尾", "");
                                var m = con.xball_copy.Where(a => a.name.Trim() == theball).First();
                                m.w2b = beilu[i];
                                con.SubmitChanges();
                            }
                            return Json(new { type = "suc", msg = "操作成功！" });
                        case "三尾不中":
                            for (int i = 0; i < ball.Length; i++)
                            {
                                string theball = ball[i].Replace("尾", "");
                                var m = con.xball_copy.Where(a => a.name.Trim() == theball).First();
                                m.w3b = beilu[i];
                                con.SubmitChanges();
                            }
                            return Json(new { type = "suc", msg = "操作成功！" });
                        case "四尾不中":
                            for (int i = 0; i < ball.Length; i++)
                            {
                                string theball = ball[i].Replace("尾", "");
                                var m = con.xball_copy.Where(a => a.name.Trim() == theball).First();
                                m.w4b = beilu[i];
                                con.SubmitChanges();
                            }
                            return Json(new { type = "suc", msg = "操作成功！" });
                        case "五尾不中":
                            for (int i = 0; i < ball.Length; i++)
                            {
                                string theball = ball[i].Replace("尾", "");
                                var m = con.xball_copy.Where(a => a.name.Trim() == theball).First();
                                m.w5b = beilu[i];
                                con.SubmitChanges();
                            }
                            return Json(new { type = "suc", msg = "操作成功！" });
                        #endregion
                        #region 特平中
                        case "一粒任中":
                            for (int i = 0; i < ball.Length; i++)
                            {
                                var m = con.xball_copy.Where(a => a.name.Trim() == ball[i]).First();
                                m.rz1 = beilu[i];
                                con.SubmitChanges();
                            }
                            return Json(new { type = "suc", msg = "操作成功！" });
                        case "二粒任中":
                            for (int i = 0; i < ball.Length; i++)
                            {
                                var m = con.xball_copy.Where(a => a.name.Trim() == ball[i]).First();
                                m.rz2 = beilu[i];
                                con.SubmitChanges();
                            }
                            return Json(new { type = "suc", msg = "操作成功！" });
                        case "三粒任中":
                            for (int i = 0; i < ball.Length; i++)
                            {
                                var m = con.xball_copy.Where(a => a.name.Trim() == ball[i]).First();
                                m.rz3 = beilu[i];
                                con.SubmitChanges();
                            }
                            return Json(new { type = "suc", msg = "操作成功！" });
                        case "四粒任中":
                            for (int i = 0; i < ball.Length; i++)
                            {
                                var m = con.xball_copy.Where(a => a.name.Trim() == ball[i]).First();
                                m.rz4 = beilu[i];
                                con.SubmitChanges();
                            }
                            return Json(new { type = "suc", msg = "操作成功！" });
                        case "五粒任中":
                            for (int i = 0; i < ball.Length; i++)
                            {
                                var m = con.xball_copy.Where(a => a.name.Trim() == ball[i]).First();
                                m.rz5 = beilu[i];
                                con.SubmitChanges();
                            }
                            return Json(new { type = "suc", msg = "操作成功！" });
                        #endregion

                        default:
                            return Json(new { type = "err", msg = "错误：779" });
                    }
                }
            }
            catch (Exception ex)
            {
                return Json(new { type = "err", msg = ex.Message });
            }
        }
        public ActionResult edit_auto_beilu(int[] id, int[] max_money, decimal[] auto_beilu)
        {
            try
            {
                using (Models.Sqlconnect con = new Models.Sqlconnect())
                {
                    for (int i = 0; i < id.Length; i++)
                    {
                        var m = con.xauto_beilu.Where(a => a.id == id[i]).First();
                        m.max_money = max_money[i];
                        m.auto_beilu = auto_beilu[i];
                    }
                    con.SubmitChanges();
                    return Json(new { type = "suc", msg = "操作成功！" });
                }
            }
            catch (Exception ex)
            {
                return Json(new { type = "err", msg = ex.Message });
            }
        }
        public ActionResult save_add_marquee(string text)
        {
            try
            {
                if (text == "")
                {
                    return Json(new { type = "err", msg = "请输入公告内容！" });
                }
                using (Models.Sqlconnect con = new Models.Sqlconnect())
                {
                    var m = new Models.xmarquee();
                    m.text = text;
                    m.state = true;
                    con.xmarquee.InsertOnSubmit(m);
                    con.SubmitChanges();
                    return Json(new { type = "suc", msg = "公告添加成功！" });
                }
            }
            catch (Exception ex)
            {
                return Json(new { type = "err", msg = ex.Message });
            }
        }
        public ActionResult del_news(int id)
        {
            try
            {
                using (Models.Sqlconnect con = new Models.Sqlconnect())
                {
                    var m = con.xmarquee.Where(a => a.id == id).First();
                    con.xmarquee.DeleteOnSubmit(m);
                    con.SubmitChanges();
                    return Json(new { type = "suc", msg = "操作成功！" });
                }
            }
            catch (Exception ex)
            {
                return Json(new { type = "err", msg = ex.Message });
            }
        }
        public ActionResult save_new_xiazu(string[] t1, string[] t2, string[] ball, string[] beilu, decimal[] yongjing, decimal[] money, string[] name)
        {
            string xid = DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString() + DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString() + DateTime.Now.Second.ToString() + DateTime.Now.Millisecond.ToString();
            DateTime xnow = DateTime.Now.AddHours(Api.WebClass.plus_timer_hours);
            int qisu = Api.WebClass.xconfig_qisu;
            try
            {
                if (!Api.WebClass.pankou_zon)
                {
                    return Json(new { type = "err", msg = "还未开盘，请开盘后再下注！" });
                }
                if (t1[0] == "特码")
                {
                    if (!Api.WebClass.pankou_tem)
                    {
                        return Json(new { type = "err", msg = "特码已封盘，请开盘后再下注！" });
                    }
                }
                else
                {
                    if (!Api.WebClass.pankou_zem)
                    {
                        return Json(new { type = "err", msg = "正码已封盘，请开盘后再下注！" });
                    }
                }
                decimal moneys = 0;
                foreach (var d in money)
                {
                    moneys += d;
                }
                for (var i = 0; i < ball.Length; i++)
                {
                    bool b = Api.WebClass.f_xiazhu_money_d(ball[i], t1[i], t2[i], money[i]);
                    if (!b)
                    {
                        return Json(new { type = "err", msg = "单项下注超过限额，终止下注！" });
                    }
                }
                bool bb = Api.WebClass.f_xiazhu_money_z(t1[0], t2[0], moneys);
                if (!bb)
                {
                    return Json(new { type = "err", msg = "下注超过总额限制，终止下注！" });
                }
                if (Api.WebClass.User_info_m3 < moneys)
                {
                    return Json(new { type = "err", msg = "您的信用余额不足，终止下注！" });
                }
                using (Models.Sqlconnect con = new Models.Sqlconnect())
                {
                    for (var i = 0; i < ball.Length; i++)
                    {
                        var m = new Models.xxiazu();
                        m.timer = xnow;
                        m.t1 = t1[i];
                        m.t2 = t2[i];
                        m.beilu = beilu[i];
                        m.yongjin = yongjing[i];
                        m.money = money[i];
                        m.qisu = qisu;
                        m.ball = ball[i];
                        m.pid = int.Parse(Session["id"].ToString());
                        m.xid = xid;
                        m.name = name[i];
                        m.yongjine2 = 0;
                        m.pname = Session["name"].ToString();
                        m.z = false;
                        m.other = "";
                        con.xxiazu.InsertOnSubmit(m);
                    }
                    con.SubmitChanges();
                    return Json(new { type = "suc", msg = "流水单号：" + xid.ToString() + "\r\n下注成功！如需查看详细下注信息，请前往投注明细查看。" });
                }

            }
            catch (Exception ex)
            {
                return Json(new { type = "err", msg = ex.Message });
            }


        }
        public ActionResult del_zhudan(int id)
        {
            try
            {
                using (Models.Sqlconnect con = new Models.Sqlconnect())
                {
                    var m = con.xxiazu.Where(a => a.id == id).First();
                    if (m.qisu != Api.WebClass.xconfig_qisu)
                    {
                        return Json(new { type = "err", msg = "非当前运行期数此功能无效！" });
                    }
                    con.xxiazu.DeleteOnSubmit(m);
                    con.SubmitChanges();
                    return Json(new { type = "suc", msg = "操作成功！" });
                }
            }
            catch (Exception ex)
            {
                return Json(new { type = "err", msg = ex.Message });
            }
        }
        public ActionResult edit_zhudan(int id, decimal money, decimal yongjin, string beilu, string ball)
        {
            try
            {
                using (Models.Sqlconnect con = new Models.Sqlconnect())
                {
                    var m = con.xxiazu.Where(a => a.id == id).First();
                    if (m.qisu != Api.WebClass.xconfig_qisu)
                    {
                        return Json(new { type = "err", msg = "非当前运行期数此功能无效！" });
                    }
                    m.money = money;
                    m.yongjin = yongjin;
                    m.beilu = beilu;
                    m.ball = ball;
                    con.SubmitChanges();
                    return Json(new { type = "suc", msg = "操作成功！" });
                }
            }
            catch (Exception ex)
            {
                return Json(new { type = "err", msg = ex.Message });
            }
        }
        public ActionResult cplook_edit_beilu(int id, decimal beilu, string xname)
        {
            try
            {
                if (Session["xname"] == null)
                {
                    return Json(new { type = "err", msg = "已断开连接！" });
                }
                using (Models.Sqlconnect con = new Models.Sqlconnect())
                {
                    var m = new Models.xball();
                    switch (xname)
                    {
                        case "特码":
                            m = con.xball.Where(a => a.id == id).First();
                            decimal bl = m.tma.Value;
                            decimal pulsbl = 0;
                            if (beilu > bl)
                            {
                                pulsbl = beilu - bl;
                                m.tma = beilu;
                                m.tmb += pulsbl;
                            }
                            if (beilu < bl)
                            {
                                pulsbl = bl - beilu;
                                m.tma = beilu;
                                decimal bbei = (decimal)m.tmb - pulsbl;
                                if (bbei < 0)
                                {
                                    m.tmb = 1;
                                }
                                else
                                {
                                    m.tmb = bbei;
                                }
                            }
                            con.SubmitChanges();
                            return Json(new { type = "suc", msg = "修改成功！" });
                        case "特A":
                            m = con.xball.Where(a => a.id == id).First();
                            m.tma = beilu;
                            con.SubmitChanges();
                            return Json(new { type = "suc", msg = "修改成功！" });
                        case "特B":
                            m = con.xball.Where(a => a.id == id).First();
                            m.tmb = beilu;
                            con.SubmitChanges();
                            return Json(new { type = "suc", msg = "修改成功！" });
                        case "正1特":
                            m = con.xball.Where(a => a.id == id).First();
                            m.zt1 = beilu;
                            con.SubmitChanges();
                            return Json(new { type = "suc", msg = "修改成功！" });
                        case "正2特":
                            m = con.xball.Where(a => a.id == id).First();
                            m.zt2 = beilu;
                            con.SubmitChanges();
                            return Json(new { type = "suc", msg = "修改成功！" });
                        case "正3特":
                            m = con.xball.Where(a => a.id == id).First();
                            m.zt3 = beilu;
                            con.SubmitChanges();
                            return Json(new { type = "suc", msg = "修改成功！" });
                        case "正4特":
                            m = con.xball.Where(a => a.id == id).First();
                            m.zt4 = beilu;
                            con.SubmitChanges();
                            return Json(new { type = "suc", msg = "修改成功！" });
                        case "正5特":
                            m = con.xball.Where(a => a.id == id).First();
                            m.zt5 = beilu;
                            con.SubmitChanges();
                            return Json(new { type = "suc", msg = "修改成功！" });
                        case "正6特":
                            m = con.xball.Where(a => a.id == id).First();
                            m.zt6 = beilu;
                            con.SubmitChanges();
                            return Json(new { type = "suc", msg = "修改成功！" });
                        case "正A":
                            m = con.xball.Where(a => a.id == id).First();
                            m.zma = beilu;
                            con.SubmitChanges();
                            return Json(new { type = "suc", msg = "修改成功！" });
                        case "正B":
                            m = con.xball.Where(a => a.id == id).First();
                            m.zmb = beilu;
                            con.SubmitChanges();
                            return Json(new { type = "suc", msg = "修改成功！" });
                        case "一肖":
                            m = con.xball.Where(a => a.id == id).First();
                            m.yix = beilu;
                            con.SubmitChanges();
                            return Json(new { type = "suc", msg = "修改成功！" });
                        case "一肖不中":
                            m = con.xball.Where(a => a.id == id).First();
                            m.yixb = beilu;
                            con.SubmitChanges();
                            return Json(new { type = "suc", msg = "修改成功！" });
                        case "尾数":
                            m = con.xball.Where(a => a.id == id).First();
                            m.ws = beilu;
                            con.SubmitChanges();
                            return Json(new { type = "suc", msg = "修改成功！" });
                        case "尾数不中":
                            m = con.xball.Where(a => a.id == id).First();
                            m.wsb = beilu;
                            con.SubmitChanges();
                            return Json(new { type = "suc", msg = "修改成功！" });
                        case "特肖":
                            m = con.xball.Where(a => a.id == id).First();
                            m.tiex = beilu;
                            con.SubmitChanges();
                            return Json(new { type = "suc", msg = "修改成功！" });

                        default:
                            return Json(new { type = "err", msg = "此项没有赔率修改功能！" });
                    }
                }
            }
            catch (Exception ex)
            {
                return Json(new { type = "err", msg = ex.Message });
            }
        }
        public ActionResult jies(int id, int qisu)
        {
            try
            {
                using (Models.Sqlconnect con = new Models.Sqlconnect())
                {
                    var mq = con.xqisu.Where(a => a.id == id).First();
                    if (!(bool)mq.jiang)
                    {
                        return Json(new { type = "err", msg = "未开奖，不能结算！" });
                    }
                    var ms = con.xxiazu.Where(a => a.qisu == qisu).ToList();
                    var ums = con.xusers.Where(a => a.manage == false).ToList();
                    var utuisui = con.xtuisui.ToList();
                    foreach (var m in ms)
                    {
                        var umss = ums.Where(a => a.id == m.pid).First();
                        decimal xbeilu = 0;
                        #region test
                        //if (umss.pid != 0)
                        //{
                        //    if (xjies(m, mq, ref xbeilu) == 1)
                        //    {
                        //        m.yongjine = m.money * m.yongjin;
                        //        m.moneye = (m.money * xbeilu) - m.money;
                        //        m.z = true;
                        //    }
                        //    else if (xjies(m, mq, ref xbeilu) == 2)
                        //    {
                        //        m.yongjine = 0;
                        //        m.moneye = 0;
                        //        m.z = false;
                        //    }
                        //    else
                        //    {
                        //        m.yongjine = m.money * m.yongjin;
                        //        m.moneye = -(m.money);
                        //        m.z = false;
                        //    }
                        //}
                        //else
                        //{
                        //    if (xjies(m, mq, ref xbeilu) == 1)
                        //    {
                        //        m.yongjine = m.money * m.yongjin;
                        //        m.moneye = (m.money * xbeilu) - m.money + m.yongjine;
                        //        m.z = true;
                        //    }
                        //    else if (xjies(m, mq, ref xbeilu) == 2)
                        //    {
                        //        m.yongjine = 0;
                        //        m.moneye = 0;
                        //        m.z = false;
                        //    }
                        //    else
                        //    {
                        //        m.yongjine = m.money * m.yongjin;
                        //        m.moneye = -(m.money) + m.yongjine;
                        //        m.z = false;
                        //    }
                        //}
                        #endregion

                        if (xjies(m, mq, ref xbeilu) == 1)
                        {
                            m.yongjine = m.money * m.yongjin;
                            //m.moneye = (m.money * xbeilu) - m.money + m.yongjine;
                            m.moneye = (m.money * xbeilu) - m.money;
                            m.z = true;
                        }
                        else if (xjies(m, mq, ref xbeilu) == 2)
                        {
                            m.yongjine = 0;
                            m.moneye = 0;
                            m.z = false;
                        }
                        else
                        {
                            m.yongjine = m.money * m.yongjin;
                            //m.moneye = -(m.money) + m.yongjine;
                            m.moneye = -(m.money);
                            m.z = false;
                        }

                        decimal pantuisui = ptoatuisui(m, (int)umss.pid, utuisui);
                        m.yongjine2 = m.money * pantuisui;
                    }
                    mq.jiesuan = true;
                    con.SubmitChanges();

                    return Json(new { type = "suc", msg = "第 " + qisu + " 期，结算完毕！" });
                }
            }
            catch (Exception ex)
            {
                return Json(new { type = "err", msg = ex.Message });
            }
        }
        private decimal ptoatuisui(Models.xxiazu m, int userpid, List<Models.xtuisui> ms)
        {
            decimal tuisui1 = (decimal)m.yongjin;
            decimal tuisui2 = 0;
            var tuisui_model = ms.Where(a => a.pid == userpid).ToList();
            if (tuisui_model.Count < 1)
            {
                tuisui_model = ms.Where(a => a.pid == 0).ToList();
            }
            var balltxt = m.ball.Trim();
            var test = "红波,绿波,蓝波";
            if (test.IndexOf(balltxt) > -1)
            {
                balltxt = "波色";
            }
            test = "特单,特双,特大,特小,合单,合双,特尾大,特尾小,总单,总双,总大,总小,单,双,大,小";
            if (test.IndexOf(balltxt) > -1)
            {
                balltxt = "两面";
            }
            if (balltxt == "波色")
            {
                tuisui2 = (decimal)tuisui_model.Where(a => a.name.Trim() == balltxt).First().yongjin;
            }
            else if (balltxt == "两面")
            {
                tuisui2 = (decimal)tuisui_model.Where(a => a.name.Trim() == balltxt).First().yongjin;
            }
            else
            {
                var tms = from q in tuisui_model
                          where (q.name.Trim() == m.t1.Trim() || q.name.Trim() == m.t2.Trim())
                          select q;
                tuisui2 = (decimal)tms.First().yongjin;
            }
            tuisui2 = tuisui2 - tuisui1;
            if (tuisui2 < 0)
            {
                tuisui2 = 0;
            }
            return tuisui2;
        }
        private int xjies(Models.xxiazu m, Models.xqisu q, ref decimal beilu)
        {
            ///开奖号码字符串数组
            string[] ball_str = q.ball.Trim().Split(',');
            ///开奖号码颜色
            string[] ball_color = q.color.Trim().Split(',');
            ///开奖号码数字数组
            int[] ball_int = ball_str.Select(o => int.Parse(o)).ToArray();
            ///开奖号码生肖数组
            string[] ball_sx = q.shengxiao.Trim().Split(',');
            ///开奖号码五行数组
            string[] ball_wuxin = q.wuxin.Trim().Split(',');
            ///开奖号码正码段
            string ball_zema = ball_str[0].ToString() + "," + ball_str[1].ToString() + "," + ball_str[2].ToString() + "," + ball_str[3].ToString() + "," + ball_str[4].ToString() + "," + ball_str[5].ToString();

            #region 多选不中
            if (m.t1.Trim() == "多选不中")
            {
                string[] xdb = m.ball.Trim().Split(',');
                var xbeilu = m.beilu.Trim().Split(',').ToArray().Select(o => decimal.Parse(o)).OrderBy(o => o).First();
                var xdbi = 0;
                foreach (var s in xdb)
                {
                    if (q.ball.Trim().IndexOf(s) > -1)
                    {
                        xdbi++;
                    }
                }
                if (xdbi == 0)
                {
                    beilu = xbeilu;
                    return 1;
                }
            }
            #endregion
            #region 多选中一
            if (m.t1.Trim() == "多选中一")
            {
                string[] xdb = m.ball.Trim().Split(',');
                var xbeilu = m.beilu.Trim().Split(',').ToArray().Select(o => decimal.Parse(o)).OrderBy(o => o).First();
                var xdbi = 0;
                foreach (var s in xdb)
                {
                    if (q.ball.Trim().IndexOf(s) > -1)
                    {
                        xdbi++;
                    }
                }
                if (xdbi == 1)
                {
                    beilu = xbeilu;
                    return 1;
                }
            }
            #endregion

            switch (m.t2.Trim())
            {
                // 0 不中
                // 1 中
                // 2 和
                #region 特A
                case "特A":
                    if (m.ball.Trim() == ball_str[6])
                    {
                        beilu = decimal.Parse(m.beilu.Trim().ToString());
                        return 1;
                    }
                    if (m.ball.Trim() == "特双")
                    {
                        if (ball_int[6] == 49)
                        {
                            return 2;
                        }
                        if (ball_int[6] % 2 == 0)
                        {
                            beilu = decimal.Parse(m.beilu.Trim().ToString());
                            return 1;
                        }
                    }
                    if (m.ball.Trim() == "特单")
                    {
                        if (ball_int[6] == 49)
                        {
                            return 2;
                        }
                        if (ball_int[6] % 2 != 0)
                        {
                            beilu = decimal.Parse(m.beilu.Trim().ToString());
                            return 1;
                        }
                    }
                    if (m.ball.Trim() == "特大")
                    {
                        if (ball_int[6] == 49)
                        {
                            return 2;
                        }
                        if (ball_int[6] > 24)
                        {
                            beilu = decimal.Parse(m.beilu.Trim().ToString());
                            return 1;
                        }
                    }
                    if (m.ball.Trim() == "特小")
                    {
                        if (ball_int[6] == 49)
                        {
                            return 2;
                        }
                        if (ball_int[6] < 25)
                        {
                            beilu = decimal.Parse(m.beilu.Trim().ToString());
                            return 1;
                        }
                    }
                    if (m.ball.Trim() == "合单")
                    {
                        if (ball_int[6] == 49)
                        {
                            return 2;
                        }
                        int[] arr = ball_str[6].ToArray().Select(o => int.Parse(o.ToString())).ToArray();
                        int harr = arr[0] + arr[1];
                        if (harr % 2 != 0)
                        {
                            beilu = decimal.Parse(m.beilu.Trim().ToString());
                            return 1;
                        }
                    }
                    if (m.ball.Trim() == "合双")
                    {
                        if (ball_int[6] == 49)
                        {
                            return 2;
                        }
                        int[] arr = ball_str[6].ToArray().Select(o => int.Parse(o.ToString())).ToArray();
                        int harr = arr[0] + arr[1];
                        if (harr % 2 == 0)
                        {
                            beilu = decimal.Parse(m.beilu.Trim().ToString());
                            return 1;
                        }
                    }
                    if (m.ball.Trim() == "红波")
                    {
                        if (ball_color[6] == "R")
                        {
                            beilu = decimal.Parse(m.beilu.Trim().ToString());
                            return 1;
                        }
                    }
                    if (m.ball.Trim() == "蓝波")
                    {
                        if (ball_color[6] == "B")
                        {
                            beilu = decimal.Parse(m.beilu.Trim().ToString());
                            return 1;
                        }
                    }
                    if (m.ball.Trim() == "绿波")
                    {
                        if (ball_color[6] == "G")
                        {
                            beilu = decimal.Parse(m.beilu.Trim().ToString());
                            return 1;
                        }
                    }
                    if (m.ball.Trim() == "特尾大")
                    {
                        int[] arr = ball_str[6].ToArray().Select(o => int.Parse(o.ToString())).ToArray();
                        if (arr[1] > 4)
                        {
                            beilu = decimal.Parse(m.beilu.Trim().ToString());
                            return 1;
                        }
                    }
                    if (m.ball.Trim() == "特尾小")
                    {
                        int[] arr = ball_str[6].ToArray().Select(o => int.Parse(o.ToString())).ToArray();
                        if (arr[1] < 5)
                        {
                            beilu = decimal.Parse(m.beilu.Trim().ToString());
                            return 1;
                        }
                    }
                    break;
                #endregion
                #region 特B
                case "特B":
                    if (m.ball.Trim() == ball_str[6])
                    {
                        beilu = decimal.Parse(m.beilu.Trim().ToString());
                        return 1;
                    }
                    if (m.ball.Trim() == "特双")
                    {
                        if (ball_int[6] == 49)
                        {
                            return 2;
                        }
                        if (ball_int[6] % 2 == 0)
                        {
                            beilu = decimal.Parse(m.beilu.Trim().ToString());
                            return 1;
                        }
                    }
                    if (m.ball.Trim() == "特单")
                    {
                        if (ball_int[6] == 49)
                        {
                            return 2;
                        }
                        if (ball_int[6] % 2 != 0)
                        {
                            beilu = decimal.Parse(m.beilu.Trim().ToString());
                            return 1;
                        }
                    }
                    if (m.ball.Trim() == "特大")
                    {
                        if (ball_int[6] == 49)
                        {
                            return 2;
                        }
                        if (ball_int[6] > 24)
                        {
                            beilu = decimal.Parse(m.beilu.Trim().ToString());
                            return 1;
                        }
                    }
                    if (m.ball.Trim() == "特小")
                    {
                        if (ball_int[6] == 49)
                        {
                            return 2;
                        }
                        if (ball_int[6] < 25)
                        {
                            beilu = decimal.Parse(m.beilu.Trim().ToString());
                            return 1;
                        }
                    }
                    if (m.ball.Trim() == "合单")
                    {
                        if (ball_int[6] == 49)
                        {
                            return 2;
                        }
                        int[] arr = ball_str[6].ToArray().Select(o => int.Parse(o.ToString())).ToArray();
                        int harr = arr[0] + arr[1];
                        if (harr % 2 != 0)
                        {
                            beilu = decimal.Parse(m.beilu.Trim().ToString());
                            return 1;
                        }
                    }
                    if (m.ball.Trim() == "合双")
                    {
                        if (ball_int[6] == 49)
                        {
                            return 2;
                        }
                        int[] arr = ball_str[6].ToArray().Select(o => int.Parse(o.ToString())).ToArray();
                        int harr = arr[0] + arr[1];
                        if (harr % 2 == 0)
                        {
                            beilu = decimal.Parse(m.beilu.Trim().ToString());
                            return 1;
                        }
                    }
                    if (m.ball.Trim() == "红波")
                    {
                        if (ball_color[6] == "R")
                        {
                            beilu = decimal.Parse(m.beilu.Trim().ToString());
                            return 1;
                        }
                    }
                    if (m.ball.Trim() == "蓝波")
                    {
                        if (ball_color[6] == "B")
                        {
                            beilu = decimal.Parse(m.beilu.Trim().ToString());
                            return 1;
                        }
                    }
                    if (m.ball.Trim() == "绿波")
                    {
                        if (ball_color[6] == "G")
                        {
                            beilu = decimal.Parse(m.beilu.Trim().ToString());
                            return 1;
                        }
                    }
                    if (m.ball.Trim() == "特尾大")
                    {
                        int[] arr = ball_str[6].ToArray().Select(o => int.Parse(o.ToString())).ToArray();
                        if (arr[1] > 4)
                        {
                            beilu = decimal.Parse(m.beilu.Trim().ToString());
                            return 1;
                        }
                    }
                    if (m.ball.Trim() == "特尾小")
                    {
                        int[] arr = ball_str[6].ToArray().Select(o => int.Parse(o.ToString())).ToArray();
                        if (arr[1] < 5)
                        {
                            beilu = decimal.Parse(m.beilu.Trim().ToString());
                            return 1;
                        }
                    }
                    break;
                #endregion
                #region 正A
                case "正A":
                    string xball = q.ball.Trim().Substring(0, q.ball.Trim().Length - 2);
                    if (xball.IndexOf(m.ball.Trim()) >= 0)
                    {
                        beilu = decimal.Parse(m.beilu.Trim().ToString());
                        return 1;
                    }
                    if (m.ball.Trim() == "总单")
                    {
                        int xxball = 0;
                        for (int i = 0; i < ball_int.Length - 1; i++)
                        {
                            xxball += ball_int[i];
                        }
                        if (xxball % 2 != 0)
                        {
                            beilu = decimal.Parse(m.beilu.Trim().ToString());
                            return 1;
                        }
                    }
                    if (m.ball.Trim() == "总双")
                    {
                        int xxball = 0;
                        for (int i = 0; i < ball_int.Length - 1; i++)
                        {
                            xxball += ball_int[i];
                        }
                        if (xxball % 2 == 0)
                        {
                            beilu = decimal.Parse(m.beilu.Trim().ToString());
                            return 1;
                        }
                    }
                    if (m.ball.Trim() == "总大")
                    {
                        int xxball = 0;
                        for (int i = 0; i < ball_int.Length - 1; i++)
                        {
                            xxball += ball_int[i];
                        }
                        if (xxball > 174)
                        {
                            beilu = decimal.Parse(m.beilu.Trim().ToString());
                            return 1;
                        }
                    }
                    if (m.ball.Trim() == "总小")
                    {
                        int xxball = 0;
                        for (int i = 0; i < ball_int.Length - 1; i++)
                        {
                            xxball += ball_int[i];
                        }
                        if (xxball < 175)
                        {
                            beilu = decimal.Parse(m.beilu.Trim().ToString());
                            return 1;
                        }
                    }
                    break;
                #endregion
                #region 正B
                case "正B":
                    xball = q.ball.Trim().Substring(0, q.ball.Trim().Length - 2);
                    if (xball.IndexOf(m.ball.Trim()) >= 0)
                    {
                        beilu = decimal.Parse(m.beilu.Trim().ToString());
                        return 1;
                    }
                    if (m.ball.Trim() == "总单")
                    {
                        int xxball = 0;
                        for (int i = 0; i < ball_int.Length - 1; i++)
                        {
                            xxball += ball_int[i];
                        }
                        if (xxball % 2 != 0)
                        {
                            beilu = decimal.Parse(m.beilu.Trim().ToString());
                            return 1;
                        }
                    }
                    if (m.ball.Trim() == "总双")
                    {
                        int xxball = 0;
                        for (int i = 0; i < ball_int.Length - 1; i++)
                        {
                            xxball += ball_int[i];
                        }
                        if (xxball % 2 == 0)
                        {
                            beilu = decimal.Parse(m.beilu.Trim().ToString());
                            return 1;
                        }
                    }
                    if (m.ball.Trim() == "总大")
                    {
                        int xxball = 0;
                        for (int i = 0; i < ball_int.Length - 1; i++)
                        {
                            xxball += ball_int[i];
                        }
                        if (xxball > 174)
                        {
                            beilu = decimal.Parse(m.beilu.Trim().ToString());
                            return 1;
                        }
                    }
                    if (m.ball.Trim() == "总小")
                    {
                        int xxball = 0;
                        for (int i = 0; i < ball_int.Length - 1; i++)
                        {
                            xxball += ball_int[i];
                        }
                        if (xxball < 175)
                        {
                            beilu = decimal.Parse(m.beilu.Trim().ToString());
                            return 1;
                        }
                    }
                    break;
                #endregion
                #region 正1特
                case "正1特":
                    int ztball = ball_int[0];
                    string ztballs = ball_str[0];
                    string ztcolor = ball_color[0];
                    if (m.ball.Trim() == ball_str[0])
                    {
                        beilu = decimal.Parse(m.beilu.Trim().ToString());
                        return 1;
                    }
                    if (m.ball.Trim() == "单")
                    {
                        if (ztball == 49)
                        {
                            return 2;
                        }
                        if (ztball % 2 != 0)
                        {
                            beilu = decimal.Parse(m.beilu.Trim().ToString());
                            return 1;
                        }
                    }
                    if (m.ball.Trim() == "双")
                    {
                        if (ztball == 49)
                        {
                            return 2;
                        }
                        if (ztball % 2 == 0)
                        {
                            beilu = decimal.Parse(m.beilu.Trim().ToString());
                            return 1;
                        }
                    }
                    if (m.ball.Trim() == "大")
                    {
                        if (ztball == 49)
                        {
                            return 2;
                        }
                        if (ztball > 24)
                        {
                            beilu = decimal.Parse(m.beilu.Trim().ToString());
                            return 1;
                        }
                    }
                    if (m.ball.Trim() == "小")
                    {
                        if (ztball == 49)
                        {
                            return 2;
                        }
                        if (ztball < 25)
                        {
                            beilu = decimal.Parse(m.beilu.Trim().ToString());
                            return 1;
                        }
                    }
                    if (m.ball.Trim() == "合单")
                    {
                        if (ztball == 49)
                        {
                            return 2;
                        }
                        int[] ztballarr = ztballs.Select(o => int.Parse(o.ToString())).ToArray();
                        int x_ztball = ztballarr[0] + ztballarr[1];
                        if (x_ztball % 2 != 0)
                        {
                            beilu = decimal.Parse(m.beilu.Trim().ToString());
                            return 1;
                        }
                    }
                    if (m.ball.Trim() == "合双")
                    {
                        if (ztball == 49)
                        {
                            return 2;
                        }
                        int[] ztballarr = ztballs.Select(o => int.Parse(o.ToString())).ToArray();
                        int x_ztball = ztballarr[0] + ztballarr[1];
                        if (x_ztball % 2 == 0)
                        {
                            beilu = decimal.Parse(m.beilu.Trim().ToString());
                            return 1;
                        }
                    }
                    if (m.ball.Trim() == "红波")
                    {
                        if (ztcolor == "R")
                        {
                            beilu = decimal.Parse(m.beilu.Trim().ToString());
                            return 1;
                        }
                    }
                    if (m.ball.Trim() == "蓝波")
                    {
                        if (ztcolor == "B")
                        {
                            beilu = decimal.Parse(m.beilu.Trim().ToString());
                            return 1;
                        }
                    }
                    if (m.ball.Trim() == "绿波")
                    {
                        if (ztcolor == "G")
                        {
                            beilu = decimal.Parse(m.beilu.Trim().ToString());
                            return 1;
                        }
                    }
                    break;
                #endregion
                #region 正2特
                case "正2特":
                    ztball = ball_int[1];
                    ztballs = ball_str[1];
                    ztcolor = ball_color[1];
                    if (m.ball.Trim() == ball_str[1])
                    {
                        beilu = decimal.Parse(m.beilu.Trim().ToString());
                        return 1;
                    }
                    if (m.ball.Trim() == "单")
                    {
                        if (ztball == 49)
                        {
                            return 2;
                        }
                        if (ztball % 2 != 0)
                        {
                            beilu = decimal.Parse(m.beilu.Trim().ToString());
                            return 1;
                        }
                    }
                    if (m.ball.Trim() == "双")
                    {
                        if (ztball == 49)
                        {
                            return 2;
                        }
                        if (ztball % 2 == 0)
                        {
                            beilu = decimal.Parse(m.beilu.Trim().ToString());
                            return 1;
                        }
                    }
                    if (m.ball.Trim() == "大")
                    {
                        if (ztball == 49)
                        {
                            return 2;
                        }
                        if (ztball > 24)
                        {
                            beilu = decimal.Parse(m.beilu.Trim().ToString());
                            return 1;
                        }
                    }
                    if (m.ball.Trim() == "小")
                    {
                        if (ztball == 49)
                        {
                            return 2;
                        }
                        if (ztball < 25)
                        {
                            beilu = decimal.Parse(m.beilu.Trim().ToString());
                            return 1;
                        }
                    }
                    if (m.ball.Trim() == "合单")
                    {
                        if (ztball == 49)
                        {
                            return 2;
                        }
                        int[] ztballarr = ztballs.Select(o => int.Parse(o.ToString())).ToArray();
                        int x_ztball = ztballarr[0] + ztballarr[1];
                        if (x_ztball % 2 != 0)
                        {
                            beilu = decimal.Parse(m.beilu.Trim().ToString());
                            return 1;
                        }
                    }
                    if (m.ball.Trim() == "合双")
                    {
                        if (ztball == 49)
                        {
                            return 2;
                        }
                        int[] ztballarr = ztballs.Select(o => int.Parse(o.ToString())).ToArray();
                        int x_ztball = ztballarr[0] + ztballarr[1];
                        if (x_ztball % 2 == 0)
                        {
                            beilu = decimal.Parse(m.beilu.Trim().ToString());
                            return 1;
                        }
                    }
                    if (m.ball.Trim() == "红波")
                    {
                        if (ztcolor == "R")
                        {
                            beilu = decimal.Parse(m.beilu.Trim().ToString());
                            return 1;
                        }
                    }
                    if (m.ball.Trim() == "蓝波")
                    {
                        if (ztcolor == "B")
                        {
                            beilu = decimal.Parse(m.beilu.Trim().ToString());
                            return 1;
                        }
                    }
                    if (m.ball.Trim() == "绿波")
                    {
                        if (ztcolor == "G")
                        {
                            beilu = decimal.Parse(m.beilu.Trim().ToString());
                            return 1;
                        }
                    }
                    break;
                #endregion
                #region 正3特
                case "正3特":
                    ztball = ball_int[2];
                    ztballs = ball_str[2];
                    ztcolor = ball_color[2];
                    if (m.ball.Trim() == ball_str[2])
                    {
                        beilu = decimal.Parse(m.beilu.Trim().ToString());
                        return 1;
                    }
                    if (m.ball.Trim() == "单")
                    {
                        if (ztball == 49)
                        {
                            return 2;
                        }
                        if (ztball % 2 != 0)
                        {
                            beilu = decimal.Parse(m.beilu.Trim().ToString());
                            return 1;
                        }
                    }
                    if (m.ball.Trim() == "双")
                    {
                        if (ztball == 49)
                        {
                            return 2;
                        }
                        if (ztball % 2 == 0)
                        {
                            beilu = decimal.Parse(m.beilu.Trim().ToString());
                            return 1;
                        }
                    }
                    if (m.ball.Trim() == "大")
                    {
                        if (ztball == 49)
                        {
                            return 2;
                        }
                        if (ztball > 24)
                        {
                            beilu = decimal.Parse(m.beilu.Trim().ToString());
                            return 1;
                        }
                    }
                    if (m.ball.Trim() == "小")
                    {
                        if (ztball == 49)
                        {
                            return 2;
                        }
                        if (ztball < 25)
                        {
                            beilu = decimal.Parse(m.beilu.Trim().ToString());
                            return 1;
                        }
                    }
                    if (m.ball.Trim() == "合单")
                    {
                        if (ztball == 49)
                        {
                            return 2;
                        }
                        int[] ztballarr = ztballs.Select(o => int.Parse(o.ToString())).ToArray();
                        int x_ztball = ztballarr[0] + ztballarr[1];
                        if (x_ztball % 2 != 0)
                        {
                            beilu = decimal.Parse(m.beilu.Trim().ToString());
                            return 1;
                        }
                    }
                    if (m.ball.Trim() == "合双")
                    {
                        if (ztball == 49)
                        {
                            return 2;
                        }
                        int[] ztballarr = ztballs.Select(o => int.Parse(o.ToString())).ToArray();
                        int x_ztball = ztballarr[0] + ztballarr[1];
                        if (x_ztball % 2 == 0)
                        {
                            beilu = decimal.Parse(m.beilu.Trim().ToString());
                            return 1;
                        }
                    }
                    if (m.ball.Trim() == "红波")
                    {
                        if (ztcolor == "R")
                        {
                            beilu = decimal.Parse(m.beilu.Trim().ToString());
                            return 1;
                        }
                    }
                    if (m.ball.Trim() == "蓝波")
                    {
                        if (ztcolor == "B")
                        {
                            beilu = decimal.Parse(m.beilu.Trim().ToString());
                            return 1;
                        }
                    }
                    if (m.ball.Trim() == "绿波")
                    {
                        if (ztcolor == "G")
                        {
                            beilu = decimal.Parse(m.beilu.Trim().ToString());
                            return 1;
                        }
                    }
                    break;

                #endregion
                #region 正4特
                case "正4特":
                    ztball = ball_int[3];
                    ztballs = ball_str[3];
                    ztcolor = ball_color[3];
                    if (m.ball.Trim() == ball_str[3])
                    {
                        beilu = decimal.Parse(m.beilu.Trim().ToString());
                        return 1;
                    }
                    if (m.ball.Trim() == "单")
                    {
                        if (ztball == 49)
                        {
                            return 2;
                        }
                        if (ztball % 2 != 0)
                        {
                            beilu = decimal.Parse(m.beilu.Trim().ToString());
                            return 1;
                        }
                    }
                    if (m.ball.Trim() == "双")
                    {
                        if (ztball == 49)
                        {
                            return 2;
                        }
                        if (ztball % 2 == 0)
                        {
                            beilu = decimal.Parse(m.beilu.Trim().ToString());
                            return 1;
                        }
                    }
                    if (m.ball.Trim() == "大")
                    {
                        if (ztball == 49)
                        {
                            return 2;
                        }
                        if (ztball > 24)
                        {
                            beilu = decimal.Parse(m.beilu.Trim().ToString());
                            return 1;
                        }
                    }
                    if (m.ball.Trim() == "小")
                    {
                        if (ztball == 49)
                        {
                            return 2;
                        }
                        if (ztball < 25)
                        {
                            beilu = decimal.Parse(m.beilu.Trim().ToString());
                            return 1;
                        }
                    }
                    if (m.ball.Trim() == "合单")
                    {
                        if (ztball == 49)
                        {
                            return 2;
                        }
                        int[] ztballarr = ztballs.Select(o => int.Parse(o.ToString())).ToArray();
                        int x_ztball = ztballarr[0] + ztballarr[1];
                        if (x_ztball % 2 != 0)
                        {
                            beilu = decimal.Parse(m.beilu.Trim().ToString());
                            return 1;
                        }
                    }
                    if (m.ball.Trim() == "合双")
                    {
                        if (ztball == 49)
                        {
                            return 2;
                        }
                        int[] ztballarr = ztballs.Select(o => int.Parse(o.ToString())).ToArray();
                        int x_ztball = ztballarr[0] + ztballarr[1];
                        if (x_ztball % 2 == 0)
                        {
                            beilu = decimal.Parse(m.beilu.Trim().ToString());
                            return 1;
                        }
                    }
                    if (m.ball.Trim() == "红波")
                    {
                        if (ztcolor == "R")
                        {
                            beilu = decimal.Parse(m.beilu.Trim().ToString());
                            return 1;
                        }
                    }
                    if (m.ball.Trim() == "蓝波")
                    {
                        if (ztcolor == "B")
                        {
                            beilu = decimal.Parse(m.beilu.Trim().ToString());
                            return 1;
                        }
                    }
                    if (m.ball.Trim() == "绿波")
                    {
                        if (ztcolor == "G")
                        {
                            beilu = decimal.Parse(m.beilu.Trim().ToString());
                            return 1;
                        }
                    }
                    break;
                #endregion
                #region 正5特
                case "正5特":
                    ztball = ball_int[4];
                    ztballs = ball_str[4];
                    ztcolor = ball_color[4];
                    if (m.ball.Trim() == ball_str[4])
                    {
                        beilu = decimal.Parse(m.beilu.Trim().ToString());
                        return 1;
                    }
                    if (m.ball.Trim() == "单")
                    {
                        if (ztball == 49)
                        {
                            return 2;
                        }
                        if (ztball % 2 != 0)
                        {
                            beilu = decimal.Parse(m.beilu.Trim().ToString());
                            return 1;
                        }
                    }
                    if (m.ball.Trim() == "双")
                    {
                        if (ztball == 49)
                        {
                            return 2;
                        }
                        if (ztball % 2 == 0)
                        {
                            beilu = decimal.Parse(m.beilu.Trim().ToString());
                            return 1;
                        }
                    }
                    if (m.ball.Trim() == "大")
                    {
                        if (ztball == 49)
                        {
                            return 2;
                        }
                        if (ztball > 24)
                        {
                            beilu = decimal.Parse(m.beilu.Trim().ToString());
                            return 1;
                        }
                    }
                    if (m.ball.Trim() == "小")
                    {
                        if (ztball == 49)
                        {
                            return 2;
                        }
                        if (ztball < 25)
                        {
                            beilu = decimal.Parse(m.beilu.Trim().ToString());
                            return 1;
                        }
                    }
                    if (m.ball.Trim() == "合单")
                    {
                        if (ztball == 49)
                        {
                            return 2;
                        }
                        int[] ztballarr = ztballs.Select(o => int.Parse(o.ToString())).ToArray();
                        int x_ztball = ztballarr[0] + ztballarr[1];
                        if (x_ztball % 2 != 0)
                        {
                            beilu = decimal.Parse(m.beilu.Trim().ToString());
                            return 1;
                        }
                    }
                    if (m.ball.Trim() == "合双")
                    {
                        if (ztball == 49)
                        {
                            return 2;
                        }
                        int[] ztballarr = ztballs.Select(o => int.Parse(o.ToString())).ToArray();
                        int x_ztball = ztballarr[0] + ztballarr[1];
                        if (x_ztball % 2 == 0)
                        {
                            beilu = decimal.Parse(m.beilu.Trim().ToString());
                            return 1;
                        }
                    }
                    if (m.ball.Trim() == "红波")
                    {
                        if (ztcolor == "R")
                        {
                            beilu = decimal.Parse(m.beilu.Trim().ToString());
                            return 1;
                        }
                    }
                    if (m.ball.Trim() == "蓝波")
                    {
                        if (ztcolor == "B")
                        {
                            beilu = decimal.Parse(m.beilu.Trim().ToString());
                            return 1;
                        }
                    }
                    if (m.ball.Trim() == "绿波")
                    {
                        if (ztcolor == "G")
                        {
                            beilu = decimal.Parse(m.beilu.Trim().ToString());
                            return 1;
                        }
                    }
                    break;
                #endregion
                #region 正6特
                case "正6特":
                    ztball = ball_int[5];
                    ztballs = ball_str[5];
                    ztcolor = ball_color[5];
                    if (m.ball.Trim() == ball_str[5])
                    {
                        beilu = decimal.Parse(m.beilu.Trim().ToString());
                        return 1;
                    }
                    if (m.ball.Trim() == "单")
                    {
                        if (ztball == 49)
                        {
                            return 2;
                        }
                        if (ztball % 2 != 0)
                        {
                            beilu = decimal.Parse(m.beilu.Trim().ToString());
                            return 1;
                        }
                    }
                    if (m.ball.Trim() == "双")
                    {
                        if (ztball == 49)
                        {
                            return 2;
                        }
                        if (ztball % 2 == 0)
                        {
                            beilu = decimal.Parse(m.beilu.Trim().ToString());
                            return 1;
                        }
                    }
                    if (m.ball.Trim() == "大")
                    {
                        if (ztball == 49)
                        {
                            return 2;
                        }
                        if (ztball > 24)
                        {
                            beilu = decimal.Parse(m.beilu.Trim().ToString());
                            return 1;
                        }
                    }
                    if (m.ball.Trim() == "小")
                    {
                        if (ztball == 49)
                        {
                            return 2;
                        }
                        if (ztball < 25)
                        {
                            beilu = decimal.Parse(m.beilu.Trim().ToString());
                            return 1;
                        }
                    }
                    if (m.ball.Trim() == "合单")
                    {
                        if (ztball == 49)
                        {
                            return 2;
                        }
                        int[] ztballarr = ztballs.Select(o => int.Parse(o.ToString())).ToArray();
                        int x_ztball = ztballarr[0] + ztballarr[1];
                        if (x_ztball % 2 != 0)
                        {
                            beilu = decimal.Parse(m.beilu.Trim().ToString());
                            return 1;
                        }
                    }
                    if (m.ball.Trim() == "合双")
                    {
                        if (ztball == 49)
                        {
                            return 2;
                        }
                        int[] ztballarr = ztballs.Select(o => int.Parse(o.ToString())).ToArray();
                        int x_ztball = ztballarr[0] + ztballarr[1];
                        if (x_ztball % 2 == 0)
                        {
                            beilu = decimal.Parse(m.beilu.Trim().ToString());
                            return 1;
                        }
                    }
                    if (m.ball.Trim() == "红波")
                    {
                        if (ztcolor == "R")
                        {
                            beilu = decimal.Parse(m.beilu.Trim().ToString());
                            return 1;
                        }
                    }
                    if (m.ball.Trim() == "蓝波")
                    {
                        if (ztcolor == "B")
                        {
                            beilu = decimal.Parse(m.beilu.Trim().ToString());
                            return 1;
                        }
                    }
                    if (m.ball.Trim() == "绿波")
                    {
                        if (ztcolor == "G")
                        {
                            beilu = decimal.Parse(m.beilu.Trim().ToString());
                            return 1;
                        }
                    }
                    break;
                #endregion
                #region 连码
                case "二中特":
                    if (m.ball.Trim().IndexOf(ball_str[6]) > -1)
                    {
                        if (m.ball.Trim().IndexOf(ball_str[1]) > -1 || m.ball.Trim().IndexOf(ball_str[2]) > -1 || m.ball.Trim().IndexOf(ball_str[3]) > -1 || m.ball.Trim().IndexOf(ball_str[4]) > -1 || m.ball.Trim().IndexOf(ball_str[5]) > -1)
                        {
                            string s2beilu = m.beilu.Trim().Split('/')[0];

                            beilu = decimal.Parse(s2beilu);
                            m.other = "@中特";
                            return 1;
                        }
                    }
                    else
                    {
                        string[] s2ball = m.ball.Trim().Split(',');
                        if (q.ball.Trim().IndexOf(s2ball[0]) > -1 && q.ball.Trim().IndexOf(s2ball[1]) > -1)
                        {
                            string s2beilu = m.beilu.Trim().Split('/')[1];
                            beilu = decimal.Parse(s2beilu);
                            m.other = "@中二";
                            return 1;
                        }
                    }
                    break;
                case "二全中":
                    string[] s2qball = m.ball.Trim().Split(',');
                    if (ball_zema.IndexOf(s2qball[0]) > -1 && ball_zema.IndexOf(s2qball[1]) > -1)
                    {
                        beilu = decimal.Parse(m.beilu.Trim().ToString());
                        return 1;
                    }
                    break;
                case "特串":
                    string[] stcball = m.ball.Trim().Split(',');
                    if (m.ball.Trim().IndexOf(ball_str[6]) > -1)
                    {
                        if (ball_zema.IndexOf(stcball[0]) > -1 || ball_zema.IndexOf(stcball[1]) > -1)
                        {
                            beilu = decimal.Parse(m.beilu.Trim().ToString());
                            return 1;
                        }
                    }
                    break;
                case "三中二":
                    string[] s32ball = m.ball.Trim().Split(',');
                    if (ball_zema.IndexOf(s32ball[0]) > -1 && ball_zema.IndexOf(s32ball[1]) > -1 && ball_zema.IndexOf(s32ball[2]) > -1)
                    {
                        beilu = decimal.Parse(m.beilu.Trim().Split('/')[1].ToString());
                        m.other = "@中三";
                        return 1;
                    }
                    var ii = 0;
                    foreach (var s in s32ball)
                    {
                        if (ball_zema.IndexOf(s) > -1)
                        {
                            ii++;
                        }
                    }
                    if (ii > 1)
                    {
                        beilu = decimal.Parse(m.beilu.Trim().Split('/')[0].ToString());
                        m.other = "@中二";
                        return 1;
                    }
                    break;
                case "三全中":
                    string[] s3qball = m.ball.Trim().Split(',');
                    if (ball_zema.IndexOf(s3qball[0]) > -1 && ball_zema.IndexOf(s3qball[1]) > -1 && ball_zema.IndexOf(s3qball[2]) > -1)
                    {
                        beilu = decimal.Parse(m.beilu.Trim().ToString());
                        return 1;
                    }
                    break;
                #endregion
                #region 特码项目
                case "特肖":
                    if (m.name.Trim() == ball_sx[6])
                    {
                        beilu = decimal.Parse(m.beilu.Trim().ToString());
                        return 1;
                    }
                    break;
                case "半波":
                    if (ball_int[6] == 49)
                    {
                        return 2;
                    }
                    if (m.ball.Trim().IndexOf(ball_str[6]) > -1)
                    {
                        beilu = decimal.Parse(m.beilu.Trim().ToString());
                        return 1;
                    }
                    break;
                case "五行":
                    if (m.ball.Trim().IndexOf(ball_str[6]) > -1)
                    {
                        beilu = decimal.Parse(m.beilu.Trim().ToString());
                        return 1;
                    }
                    break;
                #endregion
                #region 生肖项目
                case "一肖":
                    int yix_sx = ball_sx.Where(o => o == m.name.Trim()).Count();
                    if (yix_sx > 0)
                    {
                        beilu = decimal.Parse(m.beilu.Trim().ToString());
                        return 1;
                    }
                    break;
                case "一肖不中":
                    yix_sx = ball_sx.Where(o => o == m.name.Trim()).Count();
                    if (yix_sx < 1)
                    {
                        beilu = decimal.Parse(m.beilu.Trim().ToString());
                        return 1;
                    }
                    break;
                case "六肖":
                    if (ball_int[6] == 49)
                    {
                        return 2;
                    }
                    if (m.ball.Trim().IndexOf(ball_sx[6]) > -1)
                    {
                        beilu = decimal.Parse(m.beilu.Trim().ToString());
                        return 1;
                    }
                    break;
                case "六肖不中":
                    if (ball_int[6] == 49)
                    {
                        return 2;
                    }
                    if (m.ball.Trim().IndexOf(ball_sx[6]) < 0)
                    {
                        beilu = decimal.Parse(m.beilu.Trim().ToString());
                        return 1;
                    }
                    break;
                case "生肖量":
                    var the_sxl = ball_sx.GroupBy(o => o).ToArray().Count();
                    if (int.Parse(m.name.Trim()) == the_sxl)
                    {
                        beilu = decimal.Parse(m.beilu.Trim().ToString());
                        return 1;
                    }
                    break;
                #endregion
                #region 总数项目
                case "尾数":
                    int weisk = 0;
                    foreach (string s in ball_str)
                    {
                        if (m.ball.Trim().IndexOf(s) > -1)
                        {
                            weisk++;
                        }
                    }
                    if (weisk > 0)
                    {
                        beilu = decimal.Parse(m.beilu.Trim().ToString());
                        return 1;
                    }
                    break;
                case "尾数不中":
                    weisk = 0;
                    foreach (string s in ball_str)
                    {
                        if (m.ball.Trim().IndexOf(s) > -1)
                        {
                            weisk++;
                        }
                    }
                    if (weisk < 1)
                    {
                        beilu = decimal.Parse(m.beilu.Trim().ToString());
                        return 1;
                    }
                    break;
                case "尾数量":
                    var the_wsl = ball_str.Select(o => o.Substring(1, 1)).GroupBy(o => o).ToArray().Count();
                    if (int.Parse(m.name.Trim()) == the_wsl)
                    {
                        beilu = decimal.Parse(m.beilu.Trim().ToString());
                        return 1;
                    }
                    break;
                #endregion
                #region 生肖连
                case "二肖中":
                    decimal s2xzbeilu = m.beilu.Trim().Split(',').ToArray().Select(o => decimal.Parse(o)).OrderBy(o => o).First();
                    int s2xzint = 0;
                    string[] s2xz = m.ball.Trim().Split(',');
                    foreach (var s in s2xz)
                    {
                        if (q.shengxiao.Trim().IndexOf(s) > -1)
                        {
                            s2xzint++;
                        }
                    }
                    if (s2xzint == 2)
                    {
                        beilu = s2xzbeilu;
                        return 1;
                    }
                    break;
                case "三肖中":
                    s2xzbeilu = m.beilu.Trim().Split(',').ToArray().Select(o => decimal.Parse(o)).OrderBy(o => o).First();
                    s2xzint = 0;
                    s2xz = m.ball.Trim().Split(',');
                    foreach (var s in s2xz)
                    {
                        if (q.shengxiao.Trim().IndexOf(s) > -1)
                        {
                            s2xzint++;
                        }
                    }
                    if (s2xzint == 3)
                    {
                        beilu = s2xzbeilu;
                        return 1;
                    }
                    break;
                case "四肖中":
                    s2xzbeilu = m.beilu.Trim().Split(',').ToArray().Select(o => decimal.Parse(o)).OrderBy(o => o).First();
                    s2xzint = 0;
                    s2xz = m.ball.Trim().Split(',');
                    foreach (var s in s2xz)
                    {
                        if (q.shengxiao.Trim().IndexOf(s) > -1)
                        {
                            s2xzint++;
                        }
                    }
                    if (s2xzint == 4)
                    {
                        beilu = s2xzbeilu;
                        return 1;
                    }
                    break;
                case "五肖中":
                    s2xzbeilu = m.beilu.Trim().Split(',').ToArray().Select(o => decimal.Parse(o)).OrderBy(o => o).First();
                    s2xzint = 0;
                    s2xz = m.ball.Trim().Split(',');
                    foreach (var s in s2xz)
                    {
                        if (q.shengxiao.Trim().IndexOf(s) > -1)
                        {
                            s2xzint++;
                        }
                    }
                    if (s2xzint == 5)
                    {
                        beilu = s2xzbeilu;
                        return 1;
                    }
                    break;
                case "二肖不中":
                    s2xzbeilu = m.beilu.Trim().Split(',').ToArray().Select(o => decimal.Parse(o)).OrderBy(o => o).First();
                    s2xzint = 0;
                    s2xz = m.ball.Trim().Split(',');
                    foreach (var s in s2xz)
                    {
                        if (q.shengxiao.Trim().IndexOf(s) > -1)
                        {
                            s2xzint++;
                        }
                    }
                    if (s2xzint == 0)
                    {
                        beilu = s2xzbeilu;
                        return 1;
                    }
                    break;
                case "三肖不中":
                    s2xzbeilu = m.beilu.Trim().Split(',').ToArray().Select(o => decimal.Parse(o)).OrderBy(o => o).First();
                    s2xzint = 0;
                    s2xz = m.ball.Trim().Split(',');
                    foreach (var s in s2xz)
                    {
                        if (q.shengxiao.Trim().IndexOf(s) > -1)
                        {
                            s2xzint++;
                        }
                    }
                    if (s2xzint == 0)
                    {
                        beilu = s2xzbeilu;
                        return 1;
                    }
                    break;
                case "四肖不中":
                    s2xzbeilu = m.beilu.Trim().Split(',').ToArray().Select(o => decimal.Parse(o)).OrderBy(o => o).First();
                    s2xzint = 0;
                    s2xz = m.ball.Trim().Split(',');
                    foreach (var s in s2xz)
                    {
                        if (q.shengxiao.Trim().IndexOf(s) > -1)
                        {
                            s2xzint++;
                        }
                    }
                    if (s2xzint == 0)
                    {
                        beilu = s2xzbeilu;
                        return 1;
                    }
                    break;
                case "五肖不中":
                    s2xzbeilu = m.beilu.Trim().Split(',').ToArray().Select(o => decimal.Parse(o)).OrderBy(o => o).First();
                    s2xzint = 0;
                    s2xz = m.ball.Trim().Split(',');
                    foreach (var s in s2xz)
                    {
                        if (q.shengxiao.Trim().IndexOf(s) > -1)
                        {
                            s2xzint++;
                        }
                    }
                    if (s2xzint == 0)
                    {
                        beilu = s2xzbeilu;
                        return 1;
                    }
                    break;
                #endregion
                #region 尾数连
                case "二尾中":
                    var s2wzbeilu = m.beilu.Trim().Split(',').ToArray().Select(o => decimal.Parse(o)).OrderBy(o => o).First();
                    var s2wzint = 0;
                    var s2wz = m.ball.Trim().Split(',');
                    var s2wpanball = ball_str.Select(o => o.Substring(1, 1)).ToArray();
                    var s2wpanballs = "";
                    foreach (var i in s2wpanball)
                    {
                        s2wpanballs += i.ToString();
                        s2wpanballs += ",";
                    }
                    if (s2wpanballs.Length > 0)
                    {
                        s2wpanballs = s2wpanballs.Substring(0, s2wpanballs.Length - 1);
                    }
                    foreach (var s in s2wz)
                    {
                        if (s2wpanballs.IndexOf(s) > -1)
                        {
                            s2wzint++;
                        }
                    }
                    if (s2wzint == 2)
                    {
                        beilu = s2wzbeilu;
                        return 1;
                    }
                    break;
                case "三尾中":
                    s2wzbeilu = m.beilu.Trim().Split(',').ToArray().Select(o => decimal.Parse(o)).OrderBy(o => o).First();
                    s2wzint = 0;
                    s2wz = m.ball.Trim().Split(',');
                    s2wpanball = ball_str.Select(o => o.Substring(1, 1)).ToArray();
                    s2wpanballs = "";
                    foreach (var i in s2wpanball)
                    {
                        s2wpanballs += i.ToString();
                        s2wpanballs += ",";
                    }
                    if (s2wpanballs.Length > 0)
                    {
                        s2wpanballs = s2wpanballs.Substring(0, s2wpanballs.Length - 1);
                    }
                    foreach (var s in s2wz)
                    {
                        if (s2wpanballs.IndexOf(s) > -1)
                        {
                            s2wzint++;
                        }
                    }
                    if (s2wzint == 3)
                    {
                        beilu = s2wzbeilu;
                        return 1;
                    }
                    break;
                case "四尾中":
                    s2wzbeilu = m.beilu.Trim().Split(',').ToArray().Select(o => decimal.Parse(o)).OrderBy(o => o).First();
                    s2wzint = 0;
                    s2wz = m.ball.Trim().Split(',');
                    s2wpanball = ball_str.Select(o => o.Substring(1, 1)).ToArray();
                    s2wpanballs = "";
                    foreach (var i in s2wpanball)
                    {
                        s2wpanballs += i.ToString();
                        s2wpanballs += ",";
                    }
                    if (s2wpanballs.Length > 0)
                    {
                        s2wpanballs = s2wpanballs.Substring(0, s2wpanballs.Length - 1);
                    }
                    foreach (var s in s2wz)
                    {
                        if (s2wpanballs.IndexOf(s) > -1)
                        {
                            s2wzint++;
                        }
                    }
                    if (s2wzint == 4)
                    {
                        beilu = s2wzbeilu;
                        return 1;
                    }
                    break;
                case "五尾中":
                    s2wzbeilu = m.beilu.Trim().Split(',').ToArray().Select(o => decimal.Parse(o)).OrderBy(o => o).First();
                    s2wzint = 0;
                    s2wz = m.ball.Trim().Split(',');
                    s2wpanball = ball_str.Select(o => o.Substring(1, 1)).ToArray();
                    s2wpanballs = "";
                    foreach (var i in s2wpanball)
                    {
                        s2wpanballs += i.ToString();
                        s2wpanballs += ",";
                    }
                    if (s2wpanballs.Length > 0)
                    {
                        s2wpanballs = s2wpanballs.Substring(0, s2wpanballs.Length - 1);
                    }
                    foreach (var s in s2wz)
                    {
                        if (s2wpanballs.IndexOf(s) > -1)
                        {
                            s2wzint++;
                        }
                    }
                    if (s2wzint == 5)
                    {
                        beilu = s2wzbeilu;
                        return 1;
                    }
                    break;
                case "二尾不中":
                    s2wzbeilu = m.beilu.Trim().Split(',').ToArray().Select(o => decimal.Parse(o)).OrderBy(o => o).First();
                    s2wzint = 0;
                    s2wz = m.ball.Trim().Split(',');
                    s2wpanball = ball_str.Select(o => o.Substring(1, 1)).ToArray();
                    s2wpanballs = "";
                    foreach (var i in s2wpanball)
                    {
                        s2wpanballs += i.ToString();
                        s2wpanballs += ",";
                    }
                    if (s2wpanballs.Length > 0)
                    {
                        s2wpanballs = s2wpanballs.Substring(0, s2wpanballs.Length - 1);
                    }
                    foreach (var s in s2wz)
                    {
                        if (s2wpanballs.IndexOf(s) > -1)
                        {
                            s2wzint++;
                        }
                    }
                    if (s2wzint == 0)
                    {
                        beilu = s2wzbeilu;
                        return 1;
                    }
                    break;
                case "三尾不中":
                    s2wzbeilu = m.beilu.Trim().Split(',').ToArray().Select(o => decimal.Parse(o)).OrderBy(o => o).First();
                    s2wzint = 0;
                    s2wz = m.ball.Trim().Split(',');
                    s2wpanball = ball_str.Select(o => o.Substring(1, 1)).ToArray();
                    s2wpanballs = "";
                    foreach (var i in s2wpanball)
                    {
                        s2wpanballs += i.ToString();
                        s2wpanballs += ",";
                    }
                    if (s2wpanballs.Length > 0)
                    {
                        s2wpanballs = s2wpanballs.Substring(0, s2wpanballs.Length - 1);
                    }
                    foreach (var s in s2wz)
                    {
                        if (s2wpanballs.IndexOf(s) > -1)
                        {
                            s2wzint++;
                        }
                    }
                    if (s2wzint == 0)
                    {
                        beilu = s2wzbeilu;
                        return 1;
                    }
                    break;
                case "四尾不中":
                    s2wzbeilu = m.beilu.Trim().Split(',').ToArray().Select(o => decimal.Parse(o)).OrderBy(o => o).First();
                    s2wzint = 0;
                    s2wz = m.ball.Trim().Split(',');
                    s2wpanball = ball_str.Select(o => o.Substring(1, 1)).ToArray();
                    s2wpanballs = "";
                    foreach (var i in s2wpanball)
                    {
                        s2wpanballs += i.ToString();
                        s2wpanballs += ",";
                    }
                    if (s2wpanballs.Length > 0)
                    {
                        s2wpanballs = s2wpanballs.Substring(0, s2wpanballs.Length - 1);
                    }
                    foreach (var s in s2wz)
                    {
                        if (s2wpanballs.IndexOf(s) > -1)
                        {
                            s2wzint++;
                        }
                    }
                    if (s2wzint == 0)
                    {
                        beilu = s2wzbeilu;
                        return 1;
                    }
                    break;
                case "五尾不中":
                    s2wzbeilu = m.beilu.Trim().Split(',').ToArray().Select(o => decimal.Parse(o)).OrderBy(o => o).First();
                    s2wzint = 0;
                    s2wz = m.ball.Trim().Split(',');
                    s2wpanball = ball_str.Select(o => o.Substring(1, 1)).ToArray();
                    s2wpanballs = "";
                    foreach (var i in s2wpanball)
                    {
                        s2wpanballs += i.ToString();
                        s2wpanballs += ",";
                    }
                    if (s2wpanballs.Length > 0)
                    {
                        s2wpanballs = s2wpanballs.Substring(0, s2wpanballs.Length - 1);
                    }
                    foreach (var s in s2wz)
                    {
                        if (s2wpanballs.IndexOf(s) > -1)
                        {
                            s2wzint++;
                        }
                    }
                    if (s2wzint == 0)
                    {
                        beilu = s2wzbeilu;
                        return 1;
                    }
                    break;
                #endregion
                #region 特平中
                case "一粒任中":
                    var xbeilu = m.beilu.Trim().Split(',').ToArray().Select(o => decimal.Parse(o)).OrderBy(o => o).First();
                    var tpzint = 0;
                    if (q.ball.Trim().IndexOf(m.ball.Trim()) > -1)
                    {
                        beilu = xbeilu;
                        return 1;
                    }
                    break;
                case "二粒任中":
                    xbeilu = m.beilu.Trim().Split(',').ToArray().Select(o => decimal.Parse(o)).OrderBy(o => o).First();
                    tpzint = 0;
                    string[] tepzball = m.ball.Trim().Split(',');
                    foreach (var i in tepzball)
                    {
                        if (q.ball.Trim().IndexOf(i) > -1)
                        {
                            tpzint++;
                        }
                    }
                    if (tpzint > 0)
                    {
                        beilu = xbeilu;
                        return 1;
                    }
                    break;
                case "三粒任中":
                    xbeilu = m.beilu.Trim().Split(',').ToArray().Select(o => decimal.Parse(o)).OrderBy(o => o).First();
                    tpzint = 0;
                    tepzball = m.ball.Trim().Split(',');
                    foreach (var i in tepzball)
                    {
                        if (q.ball.Trim().IndexOf(i) > -1)
                        {
                            tpzint++;
                        }
                    }
                    if (tpzint > 0)
                    {
                        beilu = xbeilu;
                        return 1;
                    }
                    break;
                case "四粒任中":
                    xbeilu = m.beilu.Trim().Split(',').ToArray().Select(o => decimal.Parse(o)).OrderBy(o => o).First();
                    tpzint = 0;
                    tepzball = m.ball.Trim().Split(',');
                    foreach (var i in tepzball)
                    {
                        if (q.ball.Trim().IndexOf(i) > -1)
                        {
                            tpzint++;
                        }
                    }
                    if (tpzint > 0)
                    {
                        beilu = xbeilu;
                        return 1;
                    }
                    break;
                case "五粒任中":
                    xbeilu = m.beilu.Trim().Split(',').ToArray().Select(o => decimal.Parse(o)).OrderBy(o => o).First();
                    tpzint = 0;
                    tepzball = m.ball.Trim().Split(',');
                    foreach (var i in tepzball)
                    {
                        if (q.ball.Trim().IndexOf(i) > -1)
                        {
                            tpzint++;
                        }
                    }
                    if (tpzint > 0)
                    {
                        beilu = xbeilu;
                        return 1;
                    }
                    break;
                #endregion

                default:
                    return 0;
            }

            return 0;
        }
        public ActionResult reload_beilu()
        {
            try
            {
                using (Models.Sqlconnect con = new Models.Sqlconnect())
                {
                    var m1 = con.xball_copy.ToList();
                    var m2 = con.xball.ToList();
                    foreach (var m in m1)
                    {
                        #region 恢复默认赔率
                        var mm = m2.Where(a => a.id == m.id).First();
                        mm.tma = m.tma;
                        mm.tmb = m.tmb;
                        mm.zma = m.zma;
                        mm.zmb = m.zmb;
                        mm.zt1 = m.zt1;
                        mm.zt2 = m.zt2;
                        mm.zt3 = m.zt3;
                        mm.zt4 = m.zt4;
                        mm.zt5 = m.zt5;
                        mm.zt6 = m.zt6;
                        mm.s2qz = m.s2qz;
                        mm.s2zt = m.s2zt;
                        mm.s2zt2 = m.s2zt2;
                        mm.stc = m.stc;
                        mm.s3qz = m.s3qz;
                        mm.s3z2 = m.s3z2;
                        mm.s3z22 = m.s3z22;
                        mm.tiex = m.tiex;
                        mm.babo = m.babo;
                        mm.yix = m.yix;
                        mm.yixb = m.yixb;
                        mm.liux = m.liux;
                        mm.liuxb = m.liuxb;
                        mm.ws = m.ws;
                        mm.wsb = m.wsb;
                        mm.db5 = m.db5;
                        mm.db6 = m.db6;
                        mm.db7 = m.db7;
                        mm.db8 = m.db8;
                        mm.db9 = m.db9;
                        mm.db10 = m.db10;
                        mm.db12 = m.db12;
                        mm.db15 = m.db15;
                        mm.x2 = m.x2;
                        mm.x2b = m.x2b;
                        mm.x3 = m.x3;
                        mm.x3b = m.x3b;
                        mm.x4 = m.x4;
                        mm.x4b = m.x4b;
                        mm.x5 = m.x5;
                        mm.x5b = m.x5b;
                        mm.w2 = m.w2;
                        mm.w2b = m.w2b;
                        mm.w3 = m.w3;
                        mm.w3b = m.w3b;
                        mm.w4 = m.w4;
                        mm.w4b = m.w4b;
                        mm.w5 = m.w5;
                        mm.w5b = m.w5b;
                        mm.dz5 = m.dz5;
                        mm.dz6 = m.dz6;
                        mm.dz7 = m.dz7;
                        mm.dz8 = m.dz8;
                        mm.dz9 = m.dz9;
                        mm.dz10 = m.dz10;
                        mm.rz1 = m.rz1;
                        mm.rz2 = m.rz2;
                        mm.rz3 = m.rz3;
                        mm.rz4 = m.rz4;
                        mm.rz5 = m.rz5;
                        mm.n5xin = m.n5xin;
                        mm.nsxl = m.nsxl;
                        mm.nwsl = m.nwsl;
                        #endregion
                    }
                    con.SubmitChanges();
                    return Json(new { type = "suc", msg = "恢复默认赔率完成！" });
                }
            }
            catch (Exception ex)
            {
                return Json(new { type = "err", msg = ex.Message });
            }
        }
        public ActionResult del_arrtime_zhudan(string arrtime)
        {
            if (Session["xname"] == null)
            {
                return Json(new { type = "err", msg = "已断开连接！" });
            }
            string t_arrtime = arrtime.Replace(">>>", ">");
            string[] arrtm = t_arrtime.Split('>');
            string s1 = arrtm[0].Trim() + " 00:00:00";
            string s2 = arrtm[1].Trim() + " 23:59:59";
            //try
            //{
            string constr = System.Configuration.ConfigurationManager.ConnectionStrings["x9ConnectionString"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constr))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("DELETE FROM XXIAZU WHERE [TIMER] BETWEEN '" + s1 + "' AND '" + s2 + "'", con);
                cmd.ExecuteNonQuery();
                con.Close();
                return Json(new { type = "suc", msg = "操作成功！" });
            }
            //}
            //catch (Exception ex)
            //{
            //    return Json(new { type = "err", msg = ex.Message });
            //}
        }
        public ActionResult edit_wuxi(string ball, string wuxi)
        {
            if (Session["xname"] == null)
            {
                return Json(new { type = "err", msg = "已断开连接！" });
            }

            string[] xball = ball.Split(',');
            string[] xwuxi = wuxi.Split(',');
            try
            {
                using (Models.Sqlconnect con = new Models.Sqlconnect())
                {
                    for (var i = 0; i < xball.Length; i++)
                    {
                        var ms = con.xball.Where(a => a.name.Trim() == xball[i]).ToList();
                        if (ms.Count > 0)
                        {
                            var m = ms.First();
                            m.wuxin = Convert.ToChar(xwuxi[i]);
                        }
                    }
                    con.SubmitChanges();
                    return Json(new { type = "suc", msg = "五行设置修改成功！" });
                }
            }
            catch (Exception ex)
            {
                return Json(new { type = "err", msg = ex.Message });
            }
        }
    }
}
