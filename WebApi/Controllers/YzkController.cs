using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Newtonsoft.Json.Linq;
using WebApi.Models;
using WebApi.Common;

namespace WebApi.Controllers
{
    [Log]
    public class YzkController : ApiController
    {
        EMTDBContainer db = new EMTDBContainer();
        JXCDBContainer jxc = new JXCDBContainer();

        [HttpPost]
        public Result InsertEntry(JObject obj)
        {
            try
            {

                Resolver res = new Resolver(obj);

                if (JObject.Parse(res.DataStr).GetValue("fgh") == null)
                {
                    throw new Exception("fgh缸号不能为空");
                }

                var fgh = JObject.Parse(res.DataStr).GetValue("fgh").ToString();

                var yzk = GetFID(fgh);

                var xm = db.t_DY_YZKEntry1.Where(p => p.FID == yzk.FID).ToList();

                foreach (var p in xm.AsParallel())
                {
                    db.t_DY_YZKEntry1.Attach(p);
                    db.Entry(p).State = System.Data.Entity.EntityState.Deleted;
                }

                var mx = JObject.Parse(res.DataStr).GetValue("mx");

                if (mx != null)
                {
                    List<YzkEntryViewModel> mxList = GetList(mx);

                    foreach (var p in mxList.AsParallel())
                    {
                        db.t_DY_YZKEntry1.Add(new t_DY_YZKEntry1()
                        {
                            FID = yzk.FID,
                            FPB = 0,
                            FXM = p.FXM,
                            FPZ = p.FPZ,
                            FIndex = mxList.IndexOf(p)
                        });
                    }

                    yzk.FQty = mxList.Count;
                    yzk.FWeight = mxList.Sum(p => p.FPZ);

                    db.t_DY_YZK.Attach(yzk);
                    db.Entry(yzk).State = System.Data.Entity.EntityState.Modified;

                    db.SaveChanges();
                }
                else
                {
                    throw new Exception("mx明细不能为空");
                }

                return new SucessResult(obj.ToString());
            }
            catch (Exception ex)
            {
                return new FailResult(obj.ToString(), ex.ToString());
            }
        }

        [HttpPost]
        public Result DeleteXM(JObject obj)
        {
            try
            {
                Resolver res = new Resolver(obj);

                if (JObject.Parse(res.DataStr).GetValue("fgh") == null)
                {
                    throw new Exception("fgh缸号不能为空");
                }

                var fgh = JObject.Parse(res.DataStr).GetValue("fgh").ToString();

                var yzk = GetFID(fgh);

                var xm = db.t_DY_YZKEntry1.Where(p => p.FID == yzk.FID).ToList();

                foreach (var p in xm.AsParallel())
                {
                    db.t_DY_YZKEntry1.Attach(p);
                    db.Entry(p).State = System.Data.Entity.EntityState.Deleted;
                }

                db.SaveChanges();

                return new SucessResult(res.DataStr);

            }
            catch (Exception ex)
            {
                return new FailResult(obj.ToString(), ex.ToString());
            }
        }

        [HttpPost]
        public Result CPInReport(JObject obj)
        {
            try
            {
                Resolver res = new Resolver(obj);

                if (res.Data.GetValue("fgh") == null)
                {
                    throw new Exception("fgh：缸号不能为空");
                }
                if (res.Data.GetValue("qty") == null)
                {
                    throw new Exception("qty：成品入库条数不能为空");
                }

                var fgh = res.Data.GetValue("fgh").ToString();
                var qty = int.Parse(res.Data.GetValue("qty").ToString());
                var yzk = GetFID(fgh);

                var index = GetIndex(fgh, 1025);

                if (jxc.t_DYJXC_GXHB.Count(p => p.FGH == fgh &&p.FProcedureID==1025&& p.FIndex == index&&p.FOperType=="S") > 0)
                {
                    var entity = jxc.t_DYJXC_GXHB.SingleOrDefault(p => p.FGH == fgh && p.FIndex == index&&p.FProcedureID==1025);

                    entity.FNum = qty;
                    entity.FRecDate = DateTime.Now;

                    jxc.t_DYJXC_GXHB.Attach(entity);
                    jxc.Entry(entity).State = System.Data.Entity.EntityState.Modified;
                }
                else
                {
                    t_DYJXC_GXHB gxhb = new t_DYJXC_GXHB()
                    {
                        FGH = fgh,
                        FItemID = yzk.FItemid,
                        FJT = 0,
                        FRecDate = DateTime.Now,
                        FProcedureID = 1025,
                        FOperType = "S",
                        FNum = qty,
                        FIndex = index,
                        FTime = DateTime.Now,
                        FOperMan = "admin",
                        FMemo = "正恒汇报"
                    };

                    jxc.t_DYJXC_GXHB.Add(gxhb);
                }

                jxc.SaveChanges();

                return new SucessResult(obj.ToString());

            }
            catch (Exception ex)
            {
                return new FailResult(obj.ToString(), ex.ToString());
            }
        }

        [HttpPost]
        public Result PBReport(JObject obj)
        {
            try
            {
                Resolver res = new Resolver(obj);

                if (res.Data.GetValue("fgh") == null)
                {
                    throw new Exception("缸号不能为空");
                }
                if (res.Data.GetValue("qty") == null)
                {
                    throw new Exception("配布条数不能为空");
                }

                var fgh = res.Data.GetValue("fgh").ToString();
                var qty = int.Parse(res.Data.GetValue("qty").ToString());
                var yzk = GetFID(fgh);

                var index = GetIndex(fgh, 1000);

                if (jxc.t_DYJXC_GXHB.Count(p => p.FGH == fgh&&p.FProcedureID==1000 && p.FIndex == index && p.FOperType == "S") > 0)
                {
                    var entity = jxc.t_DYJXC_GXHB.Last(p => p.FGH == fgh && p.FIndex == index&&p.FProcedureID==1000);

                    entity.FNum = qty;
                    entity.FRecDate = DateTime.Now;

                    jxc.t_DYJXC_GXHB.Attach(entity);
                    jxc.Entry(entity).State = System.Data.Entity.EntityState.Modified;
                }

                t_DYJXC_GXHB gxhb = new t_DYJXC_GXHB()
                {
                    FGH = fgh,
                    FItemID = yzk.FItemid,
                    FJT = 0,
                    FRecDate = DateTime.Now,
                    FProcedureID = 1000,
                    FOperType = "S",
                    FNum = qty,
                    FIndex = index,
                    FTime = DateTime.Now,
                    FOperMan = "admin",
                    FMemo = "正恒汇报"
                };

                jxc.t_DYJXC_GXHB.Add(gxhb);

                jxc.SaveChanges();

                return new SucessResult(obj.ToString());

            }
            catch (Exception ex)
            {
                return new FailResult(obj.ToString(), ex.ToString());
            }
        }

        private List<YzkEntryViewModel> GetList(JToken obj)
        {
            List<YzkEntryViewModel> result = new List<YzkEntryViewModel>();

            foreach (var m in obj.ToList())
            {
                var jObj = JObject.Parse(m.ToString());
                result.Add(JObjectToEntity.Convert<YzkEntryViewModel>(jObj));
            }

            return result;
        }

        private t_DY_YZK GetFID(string fgh)
        {
            var yzk = db.t_DY_YZK.SingleOrDefault(p => p.FGH == fgh);
            if (yzk == null)
            {
                throw new Exception("缸号不存在");
            }

            return yzk;
        }

        private int GetIndex(string fgh,int workproce)
        {
            var index = (from yzk in db.t_DY_YZK
                        join gylc in db.t_DY_GYLC on yzk.FGYLC equals gylc.FBillNo into g1
                        from a in g1.DefaultIfEmpty()
                        join gylce in db.t_DY_GYLCEntry on a.FID equals gylce.FID into g2
                        from b in g2.DefaultIfEmpty()
                        where yzk.FGH == fgh && b.FWorkProcedure == workproce
                        select b.FIndex).FirstOrDefault();

            if (index > 0)
            {
                return index;
            }

            return -1;
        }
        /// <summary>
        /// 锁定运转卡，运转卡不能反审
        /// </summary>
        /// <param name="obj">JSON格式{"fgh":"180801001"},fgh:缸号</param>
        /// <returns></returns>
        [HttpPost]
        public object YZKLock(JObject obj)
        {
            try
            {
                Resolver res = new Resolver(obj);
                if (res.Data.GetValue("fgh") == null)
                {
                    throw new Exception("fgh：缸号不能为空");
                }

                var fgh = res.Data.GetValue("fgh").ToString();

                if (jxc.t_FGHCPIn.Count(p => p.FGH == fgh)== 0)
                {
                    jxc.t_FGHCPIn.Add(new t_FGHCPIn() { FGH = fgh });
                    db.SaveChanges();
                }

                return new SucessResult(obj.ToString());

            }
            catch(Exception ex)
            {
                return new FailResult(obj.ToString(), ex.ToString());
            }
        }
        /// <summary>
        /// 解锁运转卡，运转卡能反审
        /// </summary>
        /// <param name="obj">JSON格式{"fgh":"180801001"},fgh:缸号</param>
        /// <returns></returns>
        [HttpPost]
        public object YZKUnLock(JObject obj)
        {
            try
            {
                Resolver res = new Resolver(obj);
                if (res.Data.GetValue("fgh") == null)
                {
                    throw new Exception("fgh：缸号不能为空");
                }

                var fgh = res.Data.GetValue("fgh").ToString();

                var entity = jxc.t_FGHCPIn.Where(p => p.FGH == fgh).ToList();

                entity.ForEach(p =>
                {
                    jxc.t_FGHCPIn.Attach(p);
                    jxc.Entry(p).State = System.Data.Entity.EntityState.Deleted;
                });

                db.SaveChanges();

                return new SucessResult(obj.ToString());

            }
            catch (Exception ex)
            {
                return new FailResult(obj.ToString(), ex.ToString());
            }
        }
        /// <summary>
        /// 删除工序汇报
        /// </summary>
        /// <param name="obj">JSON格式{"fgh":"180801002","gx":"1025"},fgh:缸号,gx:工序代码，配布1000，入仓1025</param>
        /// <returns></returns>
        [HttpPost]
        public object DeleteGXHB(JObject obj)
        {
            try
            {
                var res = new Resolver(obj);
                if (res.Data.GetValue("fgh") == null)
                {
                    throw new Exception("fgh：缸号不能为空");
                }

                if (res.Data.GetValue("gx") == null)
                {
                    throw new Exception("gx：工序代码不能为空");
                }
                else
                {
                    var gx = res.Data.GetValue("gx").ToString();

                    if (gx != "1000" && gx != "1025")
                    {
                        throw new Exception("工序代码gx只能是配布：1000或入仓：1025");
                    }
                }

                var fgh = res.Data.GetValue("fgh").ToString();
                var gxid = Convert.ToInt32(res.Data.GetValue("gx").ToString());
                var index = GetIndex(fgh, Convert.ToInt32(gxid));

                var entity = jxc.t_DYJXC_GXHB.Where(p => p.FGH == fgh && p.FIndex == index && p.FProcedureID == gxid).ToList();
                entity.ForEach(p =>
                {
                    jxc.t_DYJXC_GXHB.Attach(p);
                    jxc.Entry(p).State = System.Data.Entity.EntityState.Deleted;
                });

                jxc.SaveChanges();

                return new SucessResult(obj.ToString());

            }
            catch (Exception ex)
            {
                return new FailResult(obj.ToString(), ex.ToString());
            }
        }
    }
}
