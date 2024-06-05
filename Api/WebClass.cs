using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace x9web.Api
{
    public class WebClass
    {
        /// <summary>
        /// MD5加密类
        /// </summary>
        /// <param name="inStr"></param>
        /// <returns></returns>
        public static string FunMD5(string inStr)
        {
            //return System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(Str , "MD5");
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            byte[] InBytes = Encoding.GetEncoding("GB2312").GetBytes(inStr);
            byte[] OutBytes = md5.ComputeHash(InBytes);
            string OutString = "";
            for (int i = 0; i < OutBytes.Length; i++)
            {
                OutString += OutBytes[i].ToString("x2");
            }

            return OutString.ToUpper();
        }
        /// <summary>
        /// 服务器时间（小时）修正
        /// </summary>
        public static int plus_timer_hours = 0;
        /// <summary>
        /// 服务器时间（分钟）修正
        /// </summary>
        public static int plus_time_minutes = 0;

        /// <summary>
        /// 获取ip
        /// </summary>
        /// <returns></returns>
        public static string f_Getip()
        {
            string userIP = string.Empty;

            try
            {
                string ip = string.Empty;
                if (!string.IsNullOrEmpty(System.Web.HttpContext.Current.Request.ServerVariables["HTTP_VIA"]))
                    ip = Convert.ToString(System.Web.HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"]);
                if (string.IsNullOrEmpty(ip))
                    ip = Convert.ToString(System.Web.HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"]);
                return ip;
            }
            catch
            {
                return userIP;
            }
        }
        /// <summary>
        /// 下单类别函数
        /// </summary>
        /// <param name="t1">取回第一级类别</param>
        /// <param name="t2">取回第二级类别</param>
        /// <param name="id">传入id</param>
        /// <returns></returns>
        public static bool f_liubie(ref string t1, ref string t2, int id)
        {
            try
            {
                using (Models.Sqlconnect con = new Models.Sqlconnect())
                {
                    var m = con.xliubie.Where(a => a.id == id).ToList();
                    if (m.Count > 0)
                    {
                        t1 = m.First().t1.Trim();
                        t2 = m.First().t2.Trim();
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            catch
            {
                return false;
            }
        }
        /// <summary>
        /// 返回用户退水率
        /// </summary>
        /// <param name="xname">下注名称</param>
        /// <param name="t2">第二类别</param>
        /// <returns></returns>
        public static decimal f_tuisui(string xname, string t2)
        {
            try
            {
                var m = new List<Models.xtuisui>();
                using (Models.Sqlconnect con = new Models.Sqlconnect())
                {
                    m = con.xtuisui.Where(a => a.pid == u_userid).ToList();
                    if (m.Count < 1)
                    {
                        m = con.xtuisui.Where(a => a.pid == 0).ToList();
                    }

                    var f = from q in m
                            where (q.name.Trim() == xname)
                            select q;

                    if (f.Count() < 1)
                    {
                        f = from q in m
                            where (q.name.Trim() == t2)
                            select q;
                    }

                    if (f.Count() < 1)
                    {
                        return 0;
                    }
                    else
                    {
                        return (decimal)f.First().yongjin;
                    }
                }
            }
            catch
            {
                return 0;
            }
        }
        /// <summary>
        /// 获取倍率
        /// </summary>
        /// <param name="name">传入下注项目名称</param>
        /// <returns></returns>
        public static decimal f_beilu(string name)
        {
            return 0;
        }
        /// <summary>
        /// 特殊下注名称修改
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static string XiazhuName_Edit(string name)
        {
            var balltxt = name;
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

            return balltxt;
        }
        /// <summary>
        /// 返回下注额度单注限制
        /// </summary>
        /// <param name="name"></param>
        /// <param name="t2"></param>
        /// <param name="money"></param>
        /// <returns></returns>
        public static bool f_xiazhu_money_d(string name, string t1, string t2, decimal money)
        {
            try
            {
                var balltxt = XiazhuName_Edit(name);
                var model = u_tuisui;
                var m = from q in model
                        where (q.name.Trim() == balltxt)
                        select q;
                if (m.Count() < 1)
                {
                    m = from q in model
                        where (q.name.Trim() == t2)
                        select q;
                }
                if (m.Count() < 1)
                {
                    m = from q in model
                        where (q.name.Trim() == t1)
                        select q;
                }
                if (m.Count() < 1)
                {
                    return false;
                }
                if (m.First().danzu_e < money || m.First().min_e > money)
                {
                    return false;
                }
                else
                {
                    return true;
                }

            }
            catch
            {
                return false;
            }
        }
        /// <summary>
        /// 返回下注总额度限制
        /// </summary>
        /// <param name="name"></param>
        /// <param name="t2"></param>
        /// <param name="money"></param>
        /// <returns></returns>
        public static bool f_xiazhu_money_z(string t1, string t2, decimal money)
        {
            try
            {
                var model = u_tuisui;
                var m = from q in model
                        where (q.name.Trim() == t2)
                        select q;
                if (m.Count() < 1)
                {
                    m = from q in model
                        where (q.name.Trim() == t1)
                        select q;
                }
                if (m.Count() < 1)
                {
                    return false;
                }
                if (m.First().max_e < money)
                {
                    return false;
                }
                else
                {
                    return true;
                }

            }
            catch
            {
                return false;
            }
        }
        /// <summary>
        /// 更改色球生肖属性
        /// </summary>
        /// <param name="s">生肖</param>
        public static bool f_ball_senxiao(string s)
        {
            var se = xconfig_12sx.Replace(",", "");
            var j = se.IndexOf(s);
            var sx = xconfig_12sx.Split(',');
            try
            {
                using (Models.Sqlconnect con = new Models.Sqlconnect())
                {
                    var mode = con.xball.Where(a => a.color != null).ToList();
                    foreach (var m in mode)
                    {
                        m.shenxiao = Convert.ToChar(sx[j]);
                        j++;
                        if (j == 12)
                        {
                            j = 0;
                        }

                    }
                    con.SubmitChanges();
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }
        /// <summary>
        /// 根据ID返回上级帐号名称
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static string f_shangji_name(int id)
        {
            try
            {
                if (id == 0)
                {
                    return "直属";
                }
                using (Models.Sqlconnect con = new Models.Sqlconnect())
                {
                    var m = con.xusers.Where(a => a.id == id).First();
                    return m.name.Trim();
                }
            }
            catch
            {
                return "Error";
            }
        }
        /// <summary>
        /// 根据ID返回帐号名称
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static string f_idtoname(int id)
        {
            try
            {
                using (Models.Sqlconnect con = new Models.Sqlconnect())
                {
                    var m = con.xusers.Where(a => a.id == id).First();
                    return m.name.Trim();
                }
            }
            catch
            {
                return "";
            }
        }
        /// <summary>
        /// 返回会员下线数量
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static int f_xuser_xiaxian(int id)
        {
            try
            {
                using (Models.Sqlconnect con = new Models.Sqlconnect())
                {
                    var m = con.xusers.Where(a => a.pid == id).ToList();
                    return m.Count();
                }
            }
            catch
            {
                return 0;
            }
        }
        /// <summary>
        /// 股东占成推算
        /// </summary>
        /// <param name="id"></param>
        /// <param name="money"></param>
        /// <returns></returns>
        public static decimal f_money_gdzc(int id, decimal money)
        {
            try
            {
                using (Models.Sqlconnect con = new Models.Sqlconnect())
                {
                    var m = con.xusers.Where(a => a.id == id).First();
                    if (!(bool)m.gf_state)
                    {
                        return money;
                    }
                    else
                    {
                        decimal the_money = (decimal)m.gf * money;
                        return decimal.Round(the_money, 2);
                    }
                }
            }
            catch
            {
                return 0;
            }
        }
        /// <summary>
        /// 公司占成推算
        /// </summary>
        /// <param name="id"></param>
        /// <param name="money"></param>
        /// <returns></returns>
        public static decimal f_money_gszc(int id, decimal money)
        {
            try
            {
                using (Models.Sqlconnect con = new Models.Sqlconnect())
                {
                    var m = con.xusers.Where(a => a.id == id).First();
                    if (!(bool)m.gf_state)
                    {
                        return 0;
                    }
                    else
                    {
                        decimal the_money = (1 - (decimal)m.gf) * money;
                        return decimal.Round(the_money, 2);
                    }
                }
            }
            catch
            {
                return 0;
            }
        }
        /// <summary>
        /// 返回会员状态（分销还是股东）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static string f_user_gd(int id)
        {
            try
            {
                using (Models.Sqlconnect con = new Models.Sqlconnect())
                {
                    var m = con.xusers.Where(a => a.id == id).First();
                    if (!(bool)m.gf_state)
                    {
                        return "分销会员";
                    }
                    else
                    {
                        return "股东会员";
                    }
                }
            }
            catch
            {
                return "分销会员";
            }
        }
        /// <summary>
        /// 返回当期下注总额
        /// </summary>
        public static decimal f_pankou_money(int qisu)
        {
            try
            {
                using (Models.Sqlconnect con = new Models.Sqlconnect())
                {
                    var m = con.xxiazu.Where(a => a.qisu == qisu).Sum(a => a.money);
                    return decimal.Round((decimal)m, 0);
                }
            }
            catch
            {
                return 0;
            }
        }
        /// <summary>
        /// 返回前台期数信息
        /// </summary>
        /// <param name="qisu"></param>
        /// <returns></returns>
        public static Models.xqisu f_get_xqisu_kjiang(int qisu)
        {
            try
            {
                using (Models.Sqlconnect con = new Models.Sqlconnect())
                {
                    var m = con.xqisu.Where(a => a.name == qisu).First();
                    return m;
                }
            }
            catch
            {
                return null;
            }
        }
        /// <summary>
        /// 自动开封盘
        /// </summary>
        /// <returns></returns>
        public static bool auto_pankou()
        {
            try
            {
                var theTimer = DateTime.Now.AddHours(plus_timer_hours);
                using (Models.Sqlconnect con = new Models.Sqlconnect())
                {
                    var msk = con.xqisu.Where(a => a.strat == false).ToList();
                    foreach (var m in msk)
                    {
                        var Timerdiff = (DateTime)m.k_time;
                        if (Timerdiff < theTimer)
                        {
                            m.strat = true;
                            m.bankou = true;
                            m.tema_bankou = true;
                            m.zema_bankou = true;
                            m.t_auto = true;
                            m.z_auto = true;
                        }
                    }
                    con.SubmitChanges();
                    var msf = con.xqisu.Where(a => a.bankou == true).ToList();
                    foreach (var m in msf)
                    {
                        var Timerdiff1 = (DateTime)m.t_time;
                        var Timerdiff2 = (DateTime)m.z_time;
                        if (Timerdiff1 < theTimer)
                        {
                            m.bankou = false;
                            if ((bool)m.t_auto)
                            {
                                m.tema_bankou = false;
                                m.t_auto = false;
                            }
                        }
                        if (Timerdiff2 < theTimer)
                        {
                            if ((bool)m.z_auto)
                            {
                                m.zema_bankou = false;
                                m.z_auto = false;
                            }
                        }
                        con.SubmitChanges();
                    }

                    return true;
                }
            }
            catch
            {
                return false;
            }
        }
        /// <summary>
        /// 返回自动降赔后的xball表
        /// </summary>
        /// <param name="name">类别名称(t2)</param>
        /// <param name="mxball">传入xball表</param>
        /// <returns></returns>
        public static List<Models.xball> auto_beilu(string name, List<Models.xball> mxball)
        {
            try
            {
                using (Models.Sqlconnect con = new Models.Sqlconnect())
                {
                    var ma = con.xauto_beilu.Where(a => a.state == true).Where(a => a.on == true).Where(a => a.auto_beilu > 0).Where(a => a.name.Trim() == name.Trim()).ToList();
                    if (ma.Count < 1)
                    {
                        return mxball;
                    }
                    var mz = con.xxiazu.Where(a => a.qisu == xconfig_qisu).Where(a => a.t2.Trim() == name.Trim()).ToList();
                    foreach (var m in mxball)
                    {
                        decimal moneys = (decimal)mz.Where(a => a.name.Trim() == m.name.Trim()).Sum(a => a.money);
                        if (moneys > ma.First().max_money)
                        {
                            m.tma = m.tma - ma.First().auto_beilu;
                            if ((decimal)m.tma < 0)
                            {
                                m.tma = 0;
                            }
                        }
                        //mxball.Where(a => a.name.Trim() == m.name.Trim()).First().tma = m.tma;
                    }
                    return mxball;
                }
            }
            catch
            {
                return mxball;
            }
        }
        /// <summary>
        /// 用户总信用额度
        /// </summary>.
        public static decimal User_info_m1
        {
            get
            {
                try
                {
                    using (Models.Sqlconnect con = new Models.Sqlconnect())
                    {
                        var mm = con.xusers.Where(a => a.id == u_userid).ToList();
                        if (mm.Count > 0)
                        {
                            return (decimal)mm.First().m1;
                        }
                        else
                        {
                            return 0;
                        }
                    }
                }
                catch
                {
                    return 0;
                }
            }
        }
        /// <summary>
        /// 已下注额度
        /// </summary>
        public static decimal User_info_m2
        {
            get
            {
                try
                {
                    decimal money = 0;
                    using (Models.Sqlconnect con = new Models.Sqlconnect())
                    {
                        var m = con.xusers.Where(a => a.pid == u_userid).Sum(a => a.m1);
                        if (m == null)
                        {
                            m = 0;
                        }
                        money = money + (decimal)m;
                        var mm = con.xxiazu.Where(a => a.pid == u_userid).Where(a => a.qisu == xconfig_qisu).Sum(a => a.money);
                        if (mm == null)
                        {
                            mm = 0;
                        }
                        money = money + (decimal)mm;

                        return decimal.Round(money, 0);
                    }
                }
                catch
                {
                    return User_info_m1;
                }
            }
        }
        /// <summary>
        /// 信用余额
        /// </summary>
        public static decimal User_info_m3
        {
            get
            {
                decimal t = User_info_m1 - User_info_m2;
                return decimal.Round(t, 0);

            }
        }
        /// <summary>
        /// 返回期数
        /// </summary>
        public static int xconfig_qisu
        {
            get
            {
                try
                {
                    using (Models.Sqlconnect con = new Models.Sqlconnect())
                    {
                        var m = con.xqisu.Where(a => a.strat == true).Where(a => a.del == false).OrderByDescending(a => a.id).ToList();
                        if (m.Count > 0)
                        {
                            return (int)m.First().name;
                        }
                        else
                        {
                            return 0;
                        }
                    }
                }
                catch
                {
                    return 0;
                }
            }
        }
        /// <summary>
        /// 返回12生肖顺序
        /// </summary>
        public static string xconfig_12sx
        {
            get
            {
                try
                {
                    using (Models.Sqlconnect con = new Models.Sqlconnect())
                    {
                        var m = con.xconfig.ToList();
                        if (m.Count > 0)
                        {
                            return m.First().s12sx.Trim();
                        }
                        else
                        {
                            return null;
                        }
                    }
                }
                catch
                {
                    return null;
                }
            }
        }
        /// <summary>
        /// 返回开奖号码
        /// </summary>
        public static string xconfig_kaijiang
        {
            get
            {
                return string.Empty;
            }
        }
        /// <summary>
        /// 获取 session["id"] 用户ID
        /// </summary>
        private static int u_userid
        {
            get
            {
                try { return int.Parse(HttpContext.Current.Session["id"].ToString()); }
                catch { return 0; }
            }
        }
        /// <summary>
        /// 获取最新期号
        /// </summary>
        public static int u_qisu
        {
            get
            {
                string s1 = DateTime.Now.Year.ToString();
                s1 += "001";
                int id = int.Parse(s1);
                try
                {
                    using (Models.Sqlconnect con = new Models.Sqlconnect())
                    {
                        var m = from q in con.xqisu
                                orderby q.id descending
                                select q;
                        if (m.Count() > 0)
                        {
                            int xid = (int)m.First().name;
                            if (id <= xid)
                            {
                                id = xid;
                                id++;
                            }
                        }
                        return id;
                    }
                }
                catch
                {
                    return 0;
                }
            }
        }
        /// <summary>
        /// 返回用户菜单栏
        /// </summary>
        public static List<Models.xmenus> u_menu
        {
            get
            {
                try
                {
                    using (Models.Sqlconnect con = new Models.Sqlconnect())
                    {
                        var m = con.xmenus.Where(a => a.type == 1).OrderBy(a => a.id).ToList();
                        return m;
                    }
                }
                catch
                {
                    return null;
                }
            }
        }
        /// <summary>
        /// 返回退水资料
        /// </summary>
        public static List<Models.xtuisui> u_tuisui
        {
            get
            {
                try
                {
                    using (Models.Sqlconnect con = new Models.Sqlconnect())
                    {
                        var m = con.xtuisui.Where(a => a.pid == u_userid).ToList();
                        if (m.Count > 0)
                        {
                            return m;
                        }
                        else
                        {
                            m = con.xtuisui.Where(a => a.pid == 0).ToList();
                            return m;
                        }

                    }
                }
                catch
                {
                    return null;
                }
            }
        }
        /// <summary>
        /// 返回网站开关状态
        /// </summary>
        public static bool web_switch
        {
            get
            {
                try
                {
                    using (Models.Sqlconnect con = new Models.Sqlconnect())
                    {
                        var m = con.xconfig.First();
                        return (bool)m.web_switch;
                    }
                }
                catch
                {
                    return false;
                }
            }
        }
        /// <summary>
        /// 返回网站名称
        /// </summary>
        public static string web_name
        {
            get
            {
                try
                {
                    using (Models.Sqlconnect con = new Models.Sqlconnect())
                    {
                        var m = con.xconfig.First();
                        return m.webname;
                    }
                }
                catch
                {
                    return string.Empty;
                }
            }
        }
        /// <summary>
        /// 返回轮播数据
        /// </summary>
        public static List<Models.xmarquee> web_marquee
        {
            get
            {
                try
                {
                    using (Models.Sqlconnect con = new Models.Sqlconnect())
                    {
                        var m = con.xmarquee.Where(a => a.state == true).OrderByDescending(a => a.id).ToList();
                        return m;
                    }
                }
                catch
                {
                    return null;
                }
            }
        }
        /// <summary>
        /// 返回总盘状态
        /// </summary>
        public static bool pankou_zon
        {
            get
            {
                try
                {
                    using (Models.Sqlconnect con = new Models.Sqlconnect())
                    {
                        var m = con.xqisu.Where(a => a.name == xconfig_qisu).First();
                        return (bool)m.bankou;
                    }
                }
                catch
                {
                    return false;
                }
            }
        }
        /// <summary>
        /// 返回正码盘状态
        /// </summary>
        public static bool pankou_zem
        {
            get
            {
                try
                {
                    using (Models.Sqlconnect con = new Models.Sqlconnect())
                    {
                        var m = con.xqisu.Where(a => a.name == xconfig_qisu).First();
                        return (bool)m.zema_bankou;
                    }
                }
                catch
                {
                    return false;
                }
            }
        }
        /// <summary>
        /// 返回特码盘状态
        /// </summary>
        public static bool pankou_tem
        {
            get
            {
                try
                {
                    using (Models.Sqlconnect con = new Models.Sqlconnect())
                    {
                        var m = con.xqisu.Where(a => a.name == xconfig_qisu).First();
                        return (bool)m.tema_bankou;
                    }
                }
                catch
                {
                    return false;
                }
            }
        }

        /// <summary>
        /// 返回退水总注数
        /// </summary>
        /// <param name="ms"></param>
        /// <param name="mxz"></param>
        /// <returns></returns>
        public static int userts_zs(List<Models.xusers> ms, List<Models.xxiazu> mxz)
        {
            var zss = 0;
            foreach (var m in ms)
            {
                var zs = mxz.Where(a => a.pid == m.id).Count();
                zss += zs;
            }
            return zss;
        }

        /// <summary>
        /// 返回退水下注总金额
        /// </summary>
        /// <param name="ms"></param>
        /// <param name="mxz"></param>
        /// <returns></returns>
        public static decimal userts_money(List<Models.xusers> ms, List<Models.xxiazu> mxz)
        {
            decimal money = 0;
            foreach (var m in ms)
            {
                var mn = mxz.Where(a => a.pid == m.id).Sum(a => a.money);
                money += (decimal)mn;
            }
            return money;
        }

        /// <summary>
        /// 返回退水下注盈亏总额
        /// </summary>
        /// <param name="ms"></param>
        /// <param name="mxz"></param>
        /// <returns></returns>
        public static decimal userts_moneye(List<Models.xusers> ms, List<Models.xxiazu> mxz)
        {
            decimal moneye = 0;
            foreach (var m in ms)
            {
                var mn = mxz.Where(a => a.pid == m.id).Sum(a => a.moneye);
                moneye += (decimal)mn;
            }
            return moneye;
        }

        public static decimal userts_tsmoney(List<Models.xusers> ms, List<Models.xxiazu> mxz, List<Models.xtuisui> mts)
        {
            decimal ts = 0;
            foreach (var m in ms)
            {
                var mt = mxz.Where(a => a.pid == m.id).ToList();
                foreach (var z in mt)
                {
                    decimal tsl = (decimal)mts.Where(a => a.name.Trim() == z.t1.Trim() || a.name.Trim() == z.t2.Trim()).First().yongjin;
                    decimal tsmoney = tsl * (decimal)z.money;
                    ts += tsmoney;
                }
            }
            return ts;
        }
    }
}