using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace x9web.Controllers
{
    public class PartialController : Controller
    {
        //
        // GET: /Partial/

        public ActionResult home_zhudan()
        {
            try
            {
                using (Models.Sqlconnect con = new Models.Sqlconnect())
                {
                    var m = con.xxiazu.Where(a => a.qisu == Api.WebClass.xconfig_qisu).Where(a => a.pid == int.Parse(Session["id"].ToString())).OrderByDescending(a => a.id).Take(15).ToList();
                    return PartialView(m);
                }

            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }

        }
        public ActionResult home_user_info()
        {
            return PartialView();
        }
        public ActionResult manage_member_min1(string name)
        {
            try
            {
                if (name == "" || name == null)
                {
                    return Content("请输入要搜索的帐号名或姓名！");
                }
                using (Models.Sqlconnect con = new Models.Sqlconnect())
                {
                    var m = (from q in con.xusers
                             where (q.gf_state == false)
                             where (q.manage == false)
                             where (q.name.StartsWith(name) || q.xinmin.StartsWith(name))
                             select q).ToList();
                    return PartialView(m);
                }
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }
        public ActionResult manage_member_min2(string name)
        {
            try
            {
                if (name == "" || name == null)
                {
                    return Content("请输入要搜索的帐号名或姓名！");
                }
                using (Models.Sqlconnect con = new Models.Sqlconnect())
                {
                    var m = (from q in con.xusers
                             where (q.gf_state == true)
                             where (q.manage == false)
                             where (q.name.StartsWith(name) || q.xinmin.StartsWith(name))
                             select q).ToList();
                    return PartialView(m);
                }
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }
        public ActionResult manage_member_min3(string name)
        {
            try
            {
                if (name == "" || name == null)
                {
                    return Content("请输入要搜索的帐号名或姓名！");
                }
                using (Models.Sqlconnect con = new Models.Sqlconnect())
                {
                    var m = (from q in con.xusers
                             where (q.gf_state == false)
                             where (q.manage == false)
                             where (q.pid == int.Parse(Session["id"].ToString()))
                             where (q.name.StartsWith(name) || q.xinmin.StartsWith(name))
                             select q).ToList();
                    return PartialView(m);
                }
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }
        public ActionResult beilu_menud(string name)
        {
            try
            {
                using (Models.Sqlconnect con = new Models.Sqlconnect())
                {
                    var m = con.xliubie.Where(a => a.t1.Trim() == name).ToList();
                    return PartialView("manage_beilu", m);
                }
            }
            catch
            {
                return null;
            }
        }
        public ActionResult manage_beilu_ball(string name)
        {
            try
            {
                if (Session["xname"] == null)
                {
                    return Content("未登陆，或已断开连接，请重新登陆！<a href='/manage/index'>返回</a>");
                }
                using (Models.Sqlconnect con = new Models.Sqlconnect())
                {
                    var ms = con.xball.ToList();
                    IEnumerable<x9web.Models.xball> m;
                    switch (name)
                    {
                        #region 特码
                        case "特A":
                            m = from q in ms
                                where (q.color != null || q.type == 811)
                                select new x9web.Models.xball
                                {
                                    name = q.name.Trim(),
                                    color = q.color,
                                    type = q.type,
                                    tma = q.tma
                                };
                            return PartialView(m);
                        case "特B":
                            m = from q in ms
                                where (q.color != null || q.type == 811)
                                select new x9web.Models.xball
                                {
                                    name = q.name.Trim(),
                                    color = q.color,
                                    type = q.type,
                                    tma = q.tmb
                                };
                            return PartialView(m);
                        #endregion
                        #region 正码
                        case "正A":
                            m = from q in ms
                                where (q.color != null || q.type == 812)
                                select new x9web.Models.xball
                                {
                                    name = q.name.Trim(),
                                    color = q.color,
                                    type = q.type,
                                    tma = q.zma
                                };
                            return PartialView(m);
                        case "正B":
                            m = from q in ms
                                where (q.color != null || q.type == 812)
                                select new x9web.Models.xball
                                {
                                    name = q.name.Trim(),
                                    color = q.color,
                                    type = q.type,
                                    tma = q.zmb
                                };
                            return PartialView(m);
                        #endregion
                        #region 正特码
                        case "正1特":
                            m = from q in ms
                                where (q.color != null || q.type == 813)
                                select new x9web.Models.xball
                                {
                                    name = q.name.Trim(),
                                    color = q.color,
                                    type = q.type,
                                    tma = q.zt1
                                };
                            return PartialView(m);
                        case "正2特":
                            m = from q in ms
                                where (q.color != null || q.type == 813)
                                select new x9web.Models.xball
                                {
                                    name = q.name.Trim(),
                                    color = q.color,
                                    type = q.type,
                                    tma = q.zt2
                                };
                            return PartialView(m);
                        case "正3特":
                            m = from q in ms
                                where (q.color != null || q.type == 813)
                                select new x9web.Models.xball
                                {
                                    name = q.name.Trim(),
                                    color = q.color,
                                    type = q.type,
                                    tma = q.zt3
                                };
                            return PartialView(m);
                        case "正4特":
                            m = from q in ms
                                where (q.color != null || q.type == 813)
                                select new x9web.Models.xball
                                {
                                    name = q.name.Trim(),
                                    color = q.color,
                                    type = q.type,
                                    tma = q.zt4
                                };
                            return PartialView(m);
                        case "正5特":
                            m = from q in ms
                                where (q.color != null || q.type == 813)
                                select new x9web.Models.xball
                                {
                                    name = q.name.Trim(),
                                    color = q.color,
                                    type = q.type,
                                    tma = q.zt5
                                };
                            return PartialView(m);
                        case "正6特":
                            m = from q in ms
                                where (q.color != null || q.type == 813)
                                select new x9web.Models.xball
                                {
                                    name = q.name.Trim(),
                                    color = q.color,
                                    type = q.type,
                                    tma = q.zt6
                                };
                            return PartialView(m);
                        #endregion
                        #region 连码
                        case "二全中":
                            m = from q in ms
                                where (q.color != null)
                                select new x9web.Models.xball
                                {
                                    name = q.name.Trim(),
                                    color = q.color,
                                    type = q.type,
                                    tma = q.s2qz
                                };
                            return PartialView(m);
                        case "二中特":
                            m = from q in ms
                                where (q.color != null)
                                select new x9web.Models.xball
                                {
                                    name = q.name.Trim(),
                                    color = q.color,
                                    type = q.type,
                                    tma = q.s2zt,
                                    tmb = q.s2zt2
                                };
                            return PartialView("manage_beilu_ball3", m);
                        case "特串":
                            m = from q in ms
                                where (q.color != null)
                                select new x9web.Models.xball
                                {
                                    name = q.name.Trim(),
                                    color = q.color,
                                    type = q.type,
                                    tma = q.stc
                                };
                            return PartialView(m);
                        case "三全中":
                            m = from q in ms
                                where (q.color != null)
                                select new x9web.Models.xball
                                {
                                    name = q.name.Trim(),
                                    color = q.color,
                                    type = q.type,
                                    tma = q.s3qz
                                };
                            return PartialView(m);
                        case "三中二":
                            m = from q in ms
                                where (q.color != null)
                                select new x9web.Models.xball
                                {
                                    name = q.name.Trim(),
                                    color = q.color,
                                    type = q.type,
                                    tma = q.s3z2,
                                    tmb = q.s3z22
                                };
                            return PartialView("manage_beilu_ball2", m);
                        #endregion
                        #region 多选不中
                        case "五不中":
                            m = from q in ms
                                where (q.color != null)
                                select new x9web.Models.xball
                                {
                                    name = q.name.Trim(),
                                    color = q.color,
                                    type = q.type,
                                    tma = q.db5
                                };
                            return PartialView(m);
                        case "六不中":
                            m = from q in ms
                                where (q.color != null)
                                select new x9web.Models.xball
                                {
                                    name = q.name.Trim(),
                                    color = q.color,
                                    type = q.type,
                                    tma = q.db6
                                };
                            return PartialView(m);
                        case "七不中":
                            m = from q in ms
                                where (q.color != null)
                                select new x9web.Models.xball
                                {
                                    name = q.name.Trim(),
                                    color = q.color,
                                    type = q.type,
                                    tma = q.db7
                                };
                            return PartialView(m);
                        case "八不中":
                            m = from q in ms
                                where (q.color != null)
                                select new x9web.Models.xball
                                {
                                    name = q.name.Trim(),
                                    color = q.color,
                                    type = q.type,
                                    tma = q.db8
                                };
                            return PartialView(m);
                        case "九不中":
                            m = from q in ms
                                where (q.color != null)
                                select new x9web.Models.xball
                                {
                                    name = q.name.Trim(),
                                    color = q.color,
                                    type = q.type,
                                    tma = q.db9
                                };
                            return PartialView(m);
                        case "十不中":
                            m = from q in ms
                                where (q.color != null)
                                select new x9web.Models.xball
                                {
                                    name = q.name.Trim(),
                                    color = q.color,
                                    type = q.type,
                                    tma = q.db10
                                };
                            return PartialView(m);
                        case "十二不中":
                            m = from q in ms
                                where (q.color != null)
                                select new x9web.Models.xball
                                {
                                    name = q.name.Trim(),
                                    color = q.color,
                                    type = q.type,
                                    tma = q.db12
                                };
                            return PartialView(m);
                        case "十五不中":
                            m = from q in ms
                                where (q.color != null)
                                select new x9web.Models.xball
                                {
                                    name = q.name.Trim(),
                                    color = q.color,
                                    type = q.type,
                                    tma = q.db15
                                };
                            return PartialView(m);
                        #endregion
                        #region 多选中一
                        case "五中一":
                            m = from q in ms
                                where (q.color != null)
                                select new x9web.Models.xball
                                {
                                    name = q.name.Trim(),
                                    color = q.color,
                                    type = q.type,
                                    tma = q.dz5
                                };
                            return PartialView(m);
                        case "六中一":
                            m = from q in ms
                                where (q.color != null)
                                select new x9web.Models.xball
                                {
                                    name = q.name.Trim(),
                                    color = q.color,
                                    type = q.type,
                                    tma = q.dz6
                                };
                            return PartialView(m);
                        case "七中一":
                            m = from q in ms
                                where (q.color != null)
                                select new x9web.Models.xball
                                {
                                    name = q.name.Trim(),
                                    color = q.color,
                                    type = q.type,
                                    tma = q.dz7
                                };
                            return PartialView(m);
                        case "八中一":
                            m = from q in ms
                                where (q.color != null)
                                select new x9web.Models.xball
                                {
                                    name = q.name.Trim(),
                                    color = q.color,
                                    type = q.type,
                                    tma = q.dz8
                                };
                            return PartialView(m);
                        case "九中一":
                            m = from q in ms
                                where (q.color != null)
                                select new x9web.Models.xball
                                {
                                    name = q.name.Trim(),
                                    color = q.color,
                                    type = q.type,
                                    tma = q.dz9
                                };
                            return PartialView(m);
                        case "十中一":
                            m = from q in ms
                                where (q.color != null)
                                select new x9web.Models.xball
                                {
                                    name = q.name.Trim(),
                                    color = q.color,
                                    type = q.type,
                                    tma = q.dz10
                                };
                            return PartialView(m);
                        #endregion
                        #region 特码项目
                        case "特肖":
                            m = from q in ms
                                where (q.type == 814)
                                select new x9web.Models.xball
                                {
                                    name = q.name.Trim(),
                                    color = q.color,
                                    type = q.type,
                                    tma = q.tiex
                                };
                            return PartialView(m);
                        case "半波":
                            m = from q in ms
                                where (q.type == 815)
                                select new x9web.Models.xball
                                {
                                    name = q.name.Trim(),
                                    color = q.color,
                                    type = q.type,
                                    tma = q.babo
                                };
                            return PartialView(m);
                        case "五行":
                            m = from q in ms
                                where (q.type == 817)
                                select new x9web.Models.xball
                                {
                                    name = q.name.Trim(),
                                    color = q.color,
                                    type = q.type,
                                    tma = q.n5xin
                                };
                            return PartialView(m);
                        #endregion
                        #region 生肖项目
                        case "一肖":
                            m = from q in ms
                                where (q.type == 814)
                                select new x9web.Models.xball
                                {
                                    name = q.name.Trim(),
                                    color = q.color,
                                    type = q.type,
                                    tma = q.yix
                                };
                            return PartialView(m);
                        case "一肖不中":
                            m = from q in ms
                                where (q.type == 814)
                                select new x9web.Models.xball
                                {
                                    name = q.name.Trim(),
                                    color = q.color,
                                    type = q.type,
                                    tma = q.yixb
                                };
                            return PartialView(m);
                        case "六肖":
                            m = from q in ms
                                where (q.type == 814)
                                select new x9web.Models.xball
                                {
                                    name = q.name.Trim(),
                                    color = q.color,
                                    type = q.type,
                                    tma = q.liux
                                };
                            return PartialView(m);
                        case "六肖不中":
                            m = from q in ms
                                where (q.type == 814)
                                select new x9web.Models.xball
                                {
                                    name = q.name.Trim(),
                                    color = q.color,
                                    type = q.type,
                                    tma = q.liuxb
                                };
                            return PartialView(m);
                        case "生肖量":
                            m = from q in ms
                                where (q.type == 818)
                                select new x9web.Models.xball
                                {
                                    name = q.name.Trim(),
                                    color = q.color,
                                    type = q.type,
                                    tma = q.nsxl
                                };
                            return PartialView(m);
                        #endregion
                        #region 总数项目
                        case "尾数":
                            m = from q in ms
                                where (q.type == 816)
                                select new x9web.Models.xball
                                {
                                    name = q.name.Trim(),
                                    color = q.color,
                                    type = q.type,
                                    tma = q.ws
                                };
                            return PartialView(m);
                        case "尾数不中":
                            m = from q in ms
                                where (q.type == 816)
                                select new x9web.Models.xball
                                {
                                    name = q.name.Trim(),
                                    color = q.color,
                                    type = q.type,
                                    tma = q.wsb
                                };
                            return PartialView(m);
                        case "尾数量":
                            m = from q in ms
                                where (q.type == 819)
                                select new x9web.Models.xball
                                {
                                    name = q.name.Trim(),
                                    color = q.color,
                                    type = q.type,
                                    tma = q.nwsl
                                };
                            return PartialView(m);
                        #endregion
                        #region 生肖连
                        case "二肖中":
                            m = from q in ms
                                where (q.type == 814)
                                select new x9web.Models.xball
                                {
                                    name = q.name.Trim(),
                                    color = q.color,
                                    type = q.type,
                                    tma = q.x2
                                };
                            return PartialView(m);
                        case "三肖中":
                            m = from q in ms
                                where (q.type == 814)
                                select new x9web.Models.xball
                                {
                                    name = q.name.Trim(),
                                    color = q.color,
                                    type = q.type,
                                    tma = q.x3
                                };
                            return PartialView(m);
                        case "四肖中":
                            m = from q in ms
                                where (q.type == 814)
                                select new x9web.Models.xball
                                {
                                    name = q.name.Trim(),
                                    color = q.color,
                                    type = q.type,
                                    tma = q.x4
                                };
                            return PartialView(m);
                        case "五肖中":
                            m = from q in ms
                                where (q.type == 814)
                                select new x9web.Models.xball
                                {
                                    name = q.name.Trim(),
                                    color = q.color,
                                    type = q.type,
                                    tma = q.x5
                                };
                            return PartialView(m);
                        case "二肖不中":
                            m = from q in ms
                                where (q.type == 814)
                                select new x9web.Models.xball
                                {
                                    name = q.name.Trim(),
                                    color = q.color,
                                    type = q.type,
                                    tma = q.x2b
                                };
                            return PartialView(m);
                        case "三肖不中":
                            m = from q in ms
                                where (q.type == 814)
                                select new x9web.Models.xball
                                {
                                    name = q.name.Trim(),
                                    color = q.color,
                                    type = q.type,
                                    tma = q.x3b
                                };
                            return PartialView(m);
                        case "四肖不中":
                            m = from q in ms
                                where (q.type == 814)
                                select new x9web.Models.xball
                                {
                                    name = q.name.Trim(),
                                    color = q.color,
                                    type = q.type,
                                    tma = q.x4b
                                };
                            return PartialView(m);
                        case "五肖不中":
                            m = from q in ms
                                where (q.type == 814)
                                select new x9web.Models.xball
                                {
                                    name = q.name.Trim(),
                                    color = q.color,
                                    type = q.type,
                                    tma = q.x5b
                                };
                            return PartialView(m);
                        #endregion
                        #region 尾数连
                        case "二尾中":
                            m = from q in ms
                                where (q.type == 816)
                                select new x9web.Models.xball
                                {
                                    name = q.name.Trim() + "尾",
                                    color = q.color,
                                    type = q.type,
                                    tma = q.w2
                                };
                            return PartialView(m);
                        case "三尾中":
                            m = from q in ms
                                where (q.type == 816)
                                select new x9web.Models.xball
                                {
                                    name = q.name.Trim() + "尾",
                                    color = q.color,
                                    type = q.type,
                                    tma = q.w3
                                };
                            return PartialView(m);
                        case "四尾中":
                            m = from q in ms
                                where (q.type == 816)
                                select new x9web.Models.xball
                                {
                                    name = q.name.Trim() + "尾",
                                    color = q.color,
                                    type = q.type,
                                    tma = q.w4
                                };
                            return PartialView(m);
                        case "五尾中":
                            m = from q in ms
                                where (q.type == 816)
                                select new x9web.Models.xball
                                {
                                    name = q.name.Trim() + "尾",
                                    color = q.color,
                                    type = q.type,
                                    tma = q.w5
                                };
                            return PartialView(m);
                        case "二尾不中":
                            m = from q in ms
                                where (q.type == 816)
                                select new x9web.Models.xball
                                {
                                    name = q.name.Trim() + "尾",
                                    color = q.color,
                                    type = q.type,
                                    tma = q.w2b
                                };
                            return PartialView(m);
                        case "三尾不中":
                            m = from q in ms
                                where (q.type == 816)
                                select new x9web.Models.xball
                                {
                                    name = q.name.Trim() + "尾",
                                    color = q.color,
                                    type = q.type,
                                    tma = q.w3b
                                };
                            return PartialView(m);
                        case "四尾不中":
                            m = from q in ms
                                where (q.type == 816)
                                select new x9web.Models.xball
                                {
                                    name = q.name.Trim() + "尾",
                                    color = q.color,
                                    type = q.type,
                                    tma = q.w4b
                                };
                            return PartialView(m);
                        case "五尾不中":
                            m = from q in ms
                                where (q.type == 816)
                                select new x9web.Models.xball
                                {
                                    name = q.name.Trim() + "尾",
                                    color = q.color,
                                    type = q.type,
                                    tma = q.w5b
                                };
                            return PartialView(m);
                        #endregion
                        #region 特平中
                        case "一粒任中":
                            m = from q in ms
                                where (q.color != null)
                                select new x9web.Models.xball
                                {
                                    name = q.name.Trim(),
                                    color = q.color,
                                    type = q.type,
                                    tma = q.rz1
                                };
                            return PartialView(m);
                        case "二粒任中":
                            m = from q in ms
                                where (q.color != null)
                                select new x9web.Models.xball
                                {
                                    name = q.name.Trim(),
                                    color = q.color,
                                    type = q.type,
                                    tma = q.rz2
                                };
                            return PartialView(m);
                        case "三粒任中":
                            m = from q in ms
                                where (q.color != null)
                                select new x9web.Models.xball
                                {
                                    name = q.name.Trim(),
                                    color = q.color,
                                    type = q.type,
                                    tma = q.rz3
                                };
                            return PartialView(m);
                        case "四粒任中":
                            m = from q in ms
                                where (q.color != null)
                                select new x9web.Models.xball
                                {
                                    name = q.name.Trim(),
                                    color = q.color,
                                    type = q.type,
                                    tma = q.rz4
                                };
                            return PartialView(m);
                        case "五粒任中":
                            m = from q in ms
                                where (q.color != null)
                                select new x9web.Models.xball
                                {
                                    name = q.name.Trim(),
                                    color = q.color,
                                    type = q.type,
                                    tma = q.rz5
                                };
                            return PartialView(m);
                        #endregion

                        default:
                            //m = from q in ms
                            //        where (q.color != null || q.type == 811)
                            //        select new x9web.Models.xball
                            //        {
                            //            name = q.name.Trim(),
                            //            color = q.color,
                            //            type = q.type,
                            //            tma = q.tma
                            //        };
                            //ViewBag.panname = "特A";
                            //return PartialView(m);
                            return null;
                    }
                }
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }
        public ActionResult manage_beilu_ball_default(string name)
        {
            try
            {
                if (Session["xname"] == null)
                {
                    return Content("未登陆，或已断开连接，请重新登陆！<a href='/manage/index'>返回</a>");
                }
                using (Models.Sqlconnect con = new Models.Sqlconnect())
                {
                    var ms = con.xball_copy.ToList();
                    IEnumerable<x9web.Models.xball> m;
                    switch (name)
                    {
                        #region 特码
                        case "特A":
                            m = from q in ms
                                where (q.color != null || q.type == 811)
                                select new x9web.Models.xball
                                {
                                    name = q.name.Trim(),
                                    color = q.color,
                                    type = q.type,
                                    tma = q.tma
                                };
                            return PartialView(m);
                        case "特B":
                            m = from q in ms
                                where (q.color != null || q.type == 811)
                                select new x9web.Models.xball
                                {
                                    name = q.name.Trim(),
                                    color = q.color,
                                    type = q.type,
                                    tma = q.tmb
                                };
                            return PartialView(m);
                        #endregion
                        #region 正码
                        case "正A":
                            m = from q in ms
                                where (q.color != null || q.type == 812)
                                select new x9web.Models.xball
                                {
                                    name = q.name.Trim(),
                                    color = q.color,
                                    type = q.type,
                                    tma = q.zma
                                };
                            return PartialView(m);
                        case "正B":
                            m = from q in ms
                                where (q.color != null || q.type == 812)
                                select new x9web.Models.xball
                                {
                                    name = q.name.Trim(),
                                    color = q.color,
                                    type = q.type,
                                    tma = q.zmb
                                };
                            return PartialView(m);
                        #endregion
                        #region 正特码
                        case "正1特":
                            m = from q in ms
                                where (q.color != null || q.type == 813)
                                select new x9web.Models.xball
                                {
                                    name = q.name.Trim(),
                                    color = q.color,
                                    type = q.type,
                                    tma = q.zt1
                                };
                            return PartialView(m);
                        case "正2特":
                            m = from q in ms
                                where (q.color != null || q.type == 813)
                                select new x9web.Models.xball
                                {
                                    name = q.name.Trim(),
                                    color = q.color,
                                    type = q.type,
                                    tma = q.zt2
                                };
                            return PartialView(m);
                        case "正3特":
                            m = from q in ms
                                where (q.color != null || q.type == 813)
                                select new x9web.Models.xball
                                {
                                    name = q.name.Trim(),
                                    color = q.color,
                                    type = q.type,
                                    tma = q.zt3
                                };
                            return PartialView(m);
                        case "正4特":
                            m = from q in ms
                                where (q.color != null || q.type == 813)
                                select new x9web.Models.xball
                                {
                                    name = q.name.Trim(),
                                    color = q.color,
                                    type = q.type,
                                    tma = q.zt4
                                };
                            return PartialView(m);
                        case "正5特":
                            m = from q in ms
                                where (q.color != null || q.type == 813)
                                select new x9web.Models.xball
                                {
                                    name = q.name.Trim(),
                                    color = q.color,
                                    type = q.type,
                                    tma = q.zt5
                                };
                            return PartialView(m);
                        case "正6特":
                            m = from q in ms
                                where (q.color != null || q.type == 813)
                                select new x9web.Models.xball
                                {
                                    name = q.name.Trim(),
                                    color = q.color,
                                    type = q.type,
                                    tma = q.zt6
                                };
                            return PartialView(m);
                        #endregion
                        #region 连码
                        case "二全中":
                            m = from q in ms
                                where (q.color != null)
                                select new x9web.Models.xball
                                {
                                    name = q.name.Trim(),
                                    color = q.color,
                                    type = q.type,
                                    tma = q.s2qz
                                };
                            return PartialView(m);
                        case "二中特":
                            m = from q in ms
                                where (q.color != null)
                                select new x9web.Models.xball
                                {
                                    name = q.name.Trim(),
                                    color = q.color,
                                    type = q.type,
                                    tma = q.s2zt,
                                    tmb = q.s2zt2
                                };
                            return PartialView("manage_beilu_ball3", m);
                        case "特串":
                            m = from q in ms
                                where (q.color != null)
                                select new x9web.Models.xball
                                {
                                    name = q.name.Trim(),
                                    color = q.color,
                                    type = q.type,
                                    tma = q.stc
                                };
                            return PartialView(m);
                        case "三全中":
                            m = from q in ms
                                where (q.color != null)
                                select new x9web.Models.xball
                                {
                                    name = q.name.Trim(),
                                    color = q.color,
                                    type = q.type,
                                    tma = q.s3qz
                                };
                            return PartialView(m);
                        case "三中二":
                            m = from q in ms
                                where (q.color != null)
                                select new x9web.Models.xball
                                {
                                    name = q.name.Trim(),
                                    color = q.color,
                                    type = q.type,
                                    tma = q.s3z2,
                                    tmb = q.s3z22
                                };
                            return PartialView("manage_beilu_ball2", m);
                        #endregion
                        #region 多选不中
                        case "五不中":
                            m = from q in ms
                                where (q.color != null)
                                select new x9web.Models.xball
                                {
                                    name = q.name.Trim(),
                                    color = q.color,
                                    type = q.type,
                                    tma = q.db5
                                };
                            return PartialView(m);
                        case "六不中":
                            m = from q in ms
                                where (q.color != null)
                                select new x9web.Models.xball
                                {
                                    name = q.name.Trim(),
                                    color = q.color,
                                    type = q.type,
                                    tma = q.db6
                                };
                            return PartialView(m);
                        case "七不中":
                            m = from q in ms
                                where (q.color != null)
                                select new x9web.Models.xball
                                {
                                    name = q.name.Trim(),
                                    color = q.color,
                                    type = q.type,
                                    tma = q.db7
                                };
                            return PartialView(m);
                        case "八不中":
                            m = from q in ms
                                where (q.color != null)
                                select new x9web.Models.xball
                                {
                                    name = q.name.Trim(),
                                    color = q.color,
                                    type = q.type,
                                    tma = q.db8
                                };
                            return PartialView(m);
                        case "九不中":
                            m = from q in ms
                                where (q.color != null)
                                select new x9web.Models.xball
                                {
                                    name = q.name.Trim(),
                                    color = q.color,
                                    type = q.type,
                                    tma = q.db9
                                };
                            return PartialView(m);
                        case "十不中":
                            m = from q in ms
                                where (q.color != null)
                                select new x9web.Models.xball
                                {
                                    name = q.name.Trim(),
                                    color = q.color,
                                    type = q.type,
                                    tma = q.db10
                                };
                            return PartialView(m);
                        case "十二不中":
                            m = from q in ms
                                where (q.color != null)
                                select new x9web.Models.xball
                                {
                                    name = q.name.Trim(),
                                    color = q.color,
                                    type = q.type,
                                    tma = q.db12
                                };
                            return PartialView(m);
                        case "十五不中":
                            m = from q in ms
                                where (q.color != null)
                                select new x9web.Models.xball
                                {
                                    name = q.name.Trim(),
                                    color = q.color,
                                    type = q.type,
                                    tma = q.db15
                                };
                            return PartialView(m);
                        #endregion
                        #region 多选中一
                        case "五中一":
                            m = from q in ms
                                where (q.color != null)
                                select new x9web.Models.xball
                                {
                                    name = q.name.Trim(),
                                    color = q.color,
                                    type = q.type,
                                    tma = q.dz5
                                };
                            return PartialView(m);
                        case "六中一":
                            m = from q in ms
                                where (q.color != null)
                                select new x9web.Models.xball
                                {
                                    name = q.name.Trim(),
                                    color = q.color,
                                    type = q.type,
                                    tma = q.dz6
                                };
                            return PartialView(m);
                        case "七中一":
                            m = from q in ms
                                where (q.color != null)
                                select new x9web.Models.xball
                                {
                                    name = q.name.Trim(),
                                    color = q.color,
                                    type = q.type,
                                    tma = q.dz7
                                };
                            return PartialView(m);
                        case "八中一":
                            m = from q in ms
                                where (q.color != null)
                                select new x9web.Models.xball
                                {
                                    name = q.name.Trim(),
                                    color = q.color,
                                    type = q.type,
                                    tma = q.dz8
                                };
                            return PartialView(m);
                        case "九中一":
                            m = from q in ms
                                where (q.color != null)
                                select new x9web.Models.xball
                                {
                                    name = q.name.Trim(),
                                    color = q.color,
                                    type = q.type,
                                    tma = q.dz9
                                };
                            return PartialView(m);
                        case "十中一":
                            m = from q in ms
                                where (q.color != null)
                                select new x9web.Models.xball
                                {
                                    name = q.name.Trim(),
                                    color = q.color,
                                    type = q.type,
                                    tma = q.dz10
                                };
                            return PartialView(m);
                        #endregion
                        #region 特码项目
                        case "特肖":
                            m = from q in ms
                                where (q.type == 814)
                                select new x9web.Models.xball
                                {
                                    name = q.name.Trim(),
                                    color = q.color,
                                    type = q.type,
                                    tma = q.tiex
                                };
                            return PartialView(m);
                        case "半波":
                            m = from q in ms
                                where (q.type == 815)
                                select new x9web.Models.xball
                                {
                                    name = q.name.Trim(),
                                    color = q.color,
                                    type = q.type,
                                    tma = q.babo
                                };
                            return PartialView(m);
                        case "五行":
                            m = from q in ms
                                where (q.type == 817)
                                select new x9web.Models.xball
                                {
                                    name = q.name.Trim(),
                                    color = q.color,
                                    type = q.type,
                                    tma = q.n5xin
                                };
                            return PartialView(m);
                        #endregion
                        #region 生肖项目
                        case "一肖":
                            m = from q in ms
                                where (q.type == 814)
                                select new x9web.Models.xball
                                {
                                    name = q.name.Trim(),
                                    color = q.color,
                                    type = q.type,
                                    tma = q.yix
                                };
                            return PartialView(m);
                        case "一肖不中":
                            m = from q in ms
                                where (q.type == 814)
                                select new x9web.Models.xball
                                {
                                    name = q.name.Trim(),
                                    color = q.color,
                                    type = q.type,
                                    tma = q.yixb
                                };
                            return PartialView(m);
                        case "六肖":
                            m = from q in ms
                                where (q.type == 814)
                                select new x9web.Models.xball
                                {
                                    name = q.name.Trim(),
                                    color = q.color,
                                    type = q.type,
                                    tma = q.liux
                                };
                            return PartialView(m);
                        case "六肖不中":
                            m = from q in ms
                                where (q.type == 814)
                                select new x9web.Models.xball
                                {
                                    name = q.name.Trim(),
                                    color = q.color,
                                    type = q.type,
                                    tma = q.liuxb
                                };
                            return PartialView(m);
                        case "生肖量":
                            m = from q in ms
                                where (q.type == 818)
                                select new x9web.Models.xball
                                {
                                    name = q.name.Trim(),
                                    color = q.color,
                                    type = q.type,
                                    tma = q.nsxl
                                };
                            return PartialView(m);
                        #endregion
                        #region 总数项目
                        case "尾数":
                            m = from q in ms
                                where (q.type == 816)
                                select new x9web.Models.xball
                                {
                                    name = q.name.Trim(),
                                    color = q.color,
                                    type = q.type,
                                    tma = q.ws
                                };
                            return PartialView(m);
                        case "尾数不中":
                            m = from q in ms
                                where (q.type == 816)
                                select new x9web.Models.xball
                                {
                                    name = q.name.Trim(),
                                    color = q.color,
                                    type = q.type,
                                    tma = q.wsb
                                };
                            return PartialView(m);
                        case "尾数量":
                            m = from q in ms
                                where (q.type == 819)
                                select new x9web.Models.xball
                                {
                                    name = q.name.Trim(),
                                    color = q.color,
                                    type = q.type,
                                    tma = q.nwsl
                                };
                            return PartialView(m);
                        #endregion
                        #region 生肖连
                        case "二肖中":
                            m = from q in ms
                                where (q.type == 814)
                                select new x9web.Models.xball
                                {
                                    name = q.name.Trim(),
                                    color = q.color,
                                    type = q.type,
                                    tma = q.x2
                                };
                            return PartialView(m);
                        case "三肖中":
                            m = from q in ms
                                where (q.type == 814)
                                select new x9web.Models.xball
                                {
                                    name = q.name.Trim(),
                                    color = q.color,
                                    type = q.type,
                                    tma = q.x3
                                };
                            return PartialView(m);
                        case "四肖中":
                            m = from q in ms
                                where (q.type == 814)
                                select new x9web.Models.xball
                                {
                                    name = q.name.Trim(),
                                    color = q.color,
                                    type = q.type,
                                    tma = q.x4
                                };
                            return PartialView(m);
                        case "五肖中":
                            m = from q in ms
                                where (q.type == 814)
                                select new x9web.Models.xball
                                {
                                    name = q.name.Trim(),
                                    color = q.color,
                                    type = q.type,
                                    tma = q.x5
                                };
                            return PartialView(m);
                        case "二肖不中":
                            m = from q in ms
                                where (q.type == 814)
                                select new x9web.Models.xball
                                {
                                    name = q.name.Trim(),
                                    color = q.color,
                                    type = q.type,
                                    tma = q.x2b
                                };
                            return PartialView(m);
                        case "三肖不中":
                            m = from q in ms
                                where (q.type == 814)
                                select new x9web.Models.xball
                                {
                                    name = q.name.Trim(),
                                    color = q.color,
                                    type = q.type,
                                    tma = q.x3b
                                };
                            return PartialView(m);
                        case "四肖不中":
                            m = from q in ms
                                where (q.type == 814)
                                select new x9web.Models.xball
                                {
                                    name = q.name.Trim(),
                                    color = q.color,
                                    type = q.type,
                                    tma = q.x4b
                                };
                            return PartialView(m);
                        case "五肖不中":
                            m = from q in ms
                                where (q.type == 814)
                                select new x9web.Models.xball
                                {
                                    name = q.name.Trim(),
                                    color = q.color,
                                    type = q.type,
                                    tma = q.x5b
                                };
                            return PartialView(m);
                        #endregion
                        #region 尾数连
                        case "二尾中":
                            m = from q in ms
                                where (q.type == 816)
                                select new x9web.Models.xball
                                {
                                    name = q.name.Trim() + "尾",
                                    color = q.color,
                                    type = q.type,
                                    tma = q.w2
                                };
                            return PartialView(m);
                        case "三尾中":
                            m = from q in ms
                                where (q.type == 816)
                                select new x9web.Models.xball
                                {
                                    name = q.name.Trim() + "尾",
                                    color = q.color,
                                    type = q.type,
                                    tma = q.w3
                                };
                            return PartialView(m);
                        case "四尾中":
                            m = from q in ms
                                where (q.type == 816)
                                select new x9web.Models.xball
                                {
                                    name = q.name.Trim() + "尾",
                                    color = q.color,
                                    type = q.type,
                                    tma = q.w4
                                };
                            return PartialView(m);
                        case "五尾中":
                            m = from q in ms
                                where (q.type == 816)
                                select new x9web.Models.xball
                                {
                                    name = q.name.Trim() + "尾",
                                    color = q.color,
                                    type = q.type,
                                    tma = q.w5
                                };
                            return PartialView(m);
                        case "二尾不中":
                            m = from q in ms
                                where (q.type == 816)
                                select new x9web.Models.xball
                                {
                                    name = q.name.Trim() + "尾",
                                    color = q.color,
                                    type = q.type,
                                    tma = q.w2b
                                };
                            return PartialView(m);
                        case "三尾不中":
                            m = from q in ms
                                where (q.type == 816)
                                select new x9web.Models.xball
                                {
                                    name = q.name.Trim() + "尾",
                                    color = q.color,
                                    type = q.type,
                                    tma = q.w3b
                                };
                            return PartialView(m);
                        case "四尾不中":
                            m = from q in ms
                                where (q.type == 816)
                                select new x9web.Models.xball
                                {
                                    name = q.name.Trim() + "尾",
                                    color = q.color,
                                    type = q.type,
                                    tma = q.w4b
                                };
                            return PartialView(m);
                        case "五尾不中":
                            m = from q in ms
                                where (q.type == 816)
                                select new x9web.Models.xball
                                {
                                    name = q.name.Trim() + "尾",
                                    color = q.color,
                                    type = q.type,
                                    tma = q.w5b
                                };
                            return PartialView(m);
                        #endregion
                        #region 特平中
                        case "一粒任中":
                            m = from q in ms
                                where (q.color != null)
                                select new x9web.Models.xball
                                {
                                    name = q.name.Trim(),
                                    color = q.color,
                                    type = q.type,
                                    tma = q.rz1
                                };
                            return PartialView(m);
                        case "二粒任中":
                            m = from q in ms
                                where (q.color != null)
                                select new x9web.Models.xball
                                {
                                    name = q.name.Trim(),
                                    color = q.color,
                                    type = q.type,
                                    tma = q.rz2
                                };
                            return PartialView(m);
                        case "三粒任中":
                            m = from q in ms
                                where (q.color != null)
                                select new x9web.Models.xball
                                {
                                    name = q.name.Trim(),
                                    color = q.color,
                                    type = q.type,
                                    tma = q.rz3
                                };
                            return PartialView(m);
                        case "四粒任中":
                            m = from q in ms
                                where (q.color != null)
                                select new x9web.Models.xball
                                {
                                    name = q.name.Trim(),
                                    color = q.color,
                                    type = q.type,
                                    tma = q.rz4
                                };
                            return PartialView(m);
                        case "五粒任中":
                            m = from q in ms
                                where (q.color != null)
                                select new x9web.Models.xball
                                {
                                    name = q.name.Trim(),
                                    color = q.color,
                                    type = q.type,
                                    tma = q.rz5
                                };
                            return PartialView(m);
                        #endregion

                        default:
                            //m = from q in ms
                            //        where (q.color != null || q.type == 811)
                            //        select new x9web.Models.xball
                            //        {
                            //            name = q.name.Trim(),
                            //            color = q.color,
                            //            type = q.type,
                            //            tma = q.tma
                            //        };
                            //ViewBag.panname = "特A";
                            //return PartialView(m);
                            return null;
                    }
                }
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }
        public ActionResult manage_zhudan_user_ajax(int qisu = 0)
        {
            if (Session["xname"] == null)
            {
                return Content("未登陆，或已断开连接！");
            }
            int xqisu = Api.WebClass.xconfig_qisu;
            if (qisu != 0)
            {
                xqisu = qisu;
            }
            try
            {
                using (Models.Sqlconnect con = new Models.Sqlconnect())
                {
                    var ms = new Models.um_man_touji();
                    ms.xxiazu = con.xxiazu.Where(a => a.qisu == xqisu).ToList();
                    ms.xusers = con.xusers.Where(a => a.manage == false).ToList();
                    return PartialView(ms);
                }
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }
        public ActionResult manage_zhudan_user_ajax2(string arrtime)
        {
            if (Session["xname"] == null)
            {
                return Content("未登陆，或已断开连接！");
            }
            //try
            //{
            string t_arrtime = arrtime.Replace(">>>", ">");
            string[] arrtm = t_arrtime.Split('>');
            string s1 = arrtm[0].Trim() + " 00:00:00";
            string s2 = arrtm[1].Trim() + " 23:59:59";
            using (Models.Sqlconnect con = new Models.Sqlconnect())
            {
                var m = con.xxiazu.Where(a => a.timer >= DateTime.Parse(s1)).Where(a => a.timer <= DateTime.Parse(s2)).ToList();
                var model = new Models.um_man_touji();
                model.xxiazu = m;
                model.xusers = con.xusers.Where(a => a.manage == false).ToList();
                return PartialView("manage_zhudan_user_ajax2_2", model);
            }
            //}
            //catch (Exception ex)
            //{
            //    return Content(ex.Message);
            //}
        }
        public ActionResult manage_zhudan_ajax(string xid, int qisu)
        {
            if (Session["xname"] == null)
            {
                return Content("未登陆，或已断开连接，请重新登陆！<a href='/manage/index'>返回</a>");
            }
            try
            {
                using (Models.Sqlconnect con = new Models.Sqlconnect())
                {
                    var ms = new List<Models.xxiazu>();
                    string[] xxid = xid.Split(',');
                    var m = con.xxiazu.Where(a => a.qisu == qisu).ToList();
                    foreach (var s in xxid)
                    {
                        var mm = m.Where(a => a.id == int.Parse(s)).First();
                        ms.Add(mm);
                    }
                    return PartialView(ms);
                }
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }
        public ActionResult manage_zhudan_ajax2(string name, int qisu)
        {
            if (Session["xname"] == null)
            {
                return Content("未登陆，或已断开连接，请重新登陆！<a href='/manage/index'>返回</a>");
            }
            try
            {
                using (Models.Sqlconnect con = new Models.Sqlconnect())
                {
                    var m = from q in con.xxiazu
                            where (q.pname == name)
                            where (q.qisu == qisu)
                            select q;
                    return PartialView(m.ToList());
                }

            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }
        public ActionResult manage_zhudan_u_ajax2(int qisu)
        {
            if (Session["xname"] == null)
            {
                return Content("未登陆，或已断开连接，请重新登陆！<a href='/manage/index'>返回</a>");
            }
            try
            {
                using (Models.Sqlconnect con = new Models.Sqlconnect())
                {
                    var m = con.xxiazu.Where(a => a.qisu == qisu).ToList();
                    return PartialView("manage_zhudan_user_ajax22", m);
                }
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }
        public ActionResult manage_day_form_partial(int qisu = 0)
        {
            try
            {
                if (qisu == 0)
                {
                    qisu = Api.WebClass.xconfig_qisu;
                }
                using (Models.Sqlconnect con = new Models.Sqlconnect())
                {
                    var m = con.xxiazu.Where(a => a.qisu == qisu).ToList();
                    return PartialView(m);
                }
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }
        public ActionResult manage_zhudan_ajax22(string xid, int qisu)
        {
            if (Session["xname"] == null)
            {
                return Content("未登陆，或已断开连接，请重新登陆！");
            }
            try
            {
                using (Models.Sqlconnect con = new Models.Sqlconnect())
                {
                    var ms = new List<Models.xxiazu>();
                    string[] xxid = xid.Split(',');
                    var m = con.xxiazu.Where(a => a.qisu == qisu).ToList();
                    foreach (var s in xxid)
                    {
                        var mm = m.Where(a => a.id == int.Parse(s)).First();
                        ms.Add(mm);
                    }
                    return View("manage_zhudan_ajax22", ms);
                }
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }

        public ActionResult manage_look_menu()
        {
            return PartialView();
        }

        public ActionResult manage_usertuisui(int id = 0, int type = 1, string time = "")
        {
            if (Session["tsid"] == null)
            {
                return Content("连接已断开，请重新登陆！");
            }
            try
            {
                var val = "";
                if (type == 1)
                {
                    if (id == 0)
                    {
                        val = Api.WebClass.xconfig_qisu.ToString();
                    }
                    else
                    {
                        val = id.ToString();
                    }
                }
                else if (type == 2)
                {
                    val = time;
                }
                using (Models.Sqlconnect con = new Models.Sqlconnect())
                {
                    var m = con.xusers.Where(a => a.manage == false).ToList();
                    List<Models.xxiazu> mxiazu = null;
                    if (type == 1)
                    {
                        mxiazu = con.xxiazu.Where(a => a.qisu == int.Parse(val)).ToList();
                    }
                    else if (type == 2)
                    {
                        var timer = time.Replace(">>>", ">");
                        var timers = timer.Split('>');
                        var t1 = DateTime.Parse(timers[0].Trim() + " 00:00:00");
                        var t2 = DateTime.Parse(timers[1].Trim() + " 23:59:59");

                        mxiazu = con.xxiazu.Where(a => a.timer > t1 && a.timer < t2).ToList();
                    }

                    var mtuisui = con.xtuisui.ToList();

                    ViewData["mxiazu"] = mxiazu;
                    ViewData["mtuisui"] = mtuisui;

                    ViewData["type"] = type;
                    ViewData["val"] = val;

                    return PartialView(m);
                }
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }

        public ActionResult ustuisuizudan(int id, int type, string val)
        {
            try
            {
                using (Models.Sqlconnect con = new Models.Sqlconnect())
                {
                    var userM = con.xusers.Where(a => a.manage == false).ToList();
                    var userM2 = userM.Where(a => a.pid2 != null && a.pid2.Trim().IndexOf(id + ",") > -1).ToList();
                    var userM3 = userM.Where(a => a.id == id).First();
                    if (userM3 != null)
                    {
                        userM2.Add(userM3);
                    }
                    List<Models.xxiazu> xzM = null;
                    if (type == 1)
                    {
                        xzM = con.xxiazu.Where(a => a.qisu == int.Parse(val)).ToList();
                    }
                    else if (type == 2)
                    {
                        var times = val.Replace(">>>", ">");
                        var timearray = times.Split('>');
                        var t1 = DateTime.Parse(timearray[0] + " 00:00:00");
                        var t2 = DateTime.Parse(timearray[1] + " 23:59:59");
                        xzM = con.xxiazu.Where(a => a.timer > t1 && a.timer < t2).ToList();
                    }
                    List<Models.xxiazu> newM = new List<Models.xxiazu>();
                    foreach (var m in userM2)
                    {
                        var temM = xzM.Where(a => a.pid == m.id).ToList();
                        foreach (var ms in temM)
                        {
                            newM.Add(ms);
                        }
                    }

                    return PartialView("~/views/partial/manage_zhudan_ajax.cshtml", newM);
                }
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }

        public ActionResult yujin(int q = 0)
        {
            if (Session["xname"] == null)
            {
                return Content("未登陆，或已断开连接，请重新登陆！<a href='/manage/index'>返回</a>");
            }

            try
            {
                using (Models.Sqlconnect con = new Models.Sqlconnect())
                {
                    int qisu = 0;
                    if (q == 0)
                    {
                        qisu = Api.WebClass.xconfig_qisu;
                    }
                    else
                    {
                        qisu = q;
                    }
                    //q = 2019060;
                    var qisudat = con.xqisu.Where(a => a.name == qisu).First();
                    var dat = con.xxiazu.Where(a => a.qisu == qisu).ToList();
                    int b = 0;
                    if (qisudat.jiesuan == true)
                    {
                        b = 1;
                    }
                    ViewData["b"] = b;
                    return PartialView(dat);
                }
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }
    }
}
