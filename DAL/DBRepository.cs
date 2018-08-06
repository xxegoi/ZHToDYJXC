using System.Collections.Generic;
using System.Linq;


namespace DAL
{
    public class DBRepository : IYzkRepository, IYzkEntry1Repository, IGXHBRepository, ICPInHeaderRepository
    {
        EMTDBContainer emt;
        JXCDBContainer jxc;

        public DBRepository()
        {
            emt = new EMTDBContainer();
            jxc = new JXCDBContainer();
        }

        public int Delete(t_CPInBillHeader entity)
        {
            jxc.t_CPInBillHeader.Attach(entity);
            jxc.Entry(entity).State = System.Data.Entity.EntityState.Deleted;
            return jxc.SaveChanges();
        }

        public t_DY_YZK GetYzk(string fgh)
        {
            var yzk= emt.t_DY_YZK.First(p => p.FGH == fgh);
            return yzk;
        }

        public int Insert(List<t_DY_YZKEntry1> entryList)
        {
            entryList.ForEach(p =>
            {
                emt.t_DY_YZKEntry1.Add(p);
            });

            return emt.SaveChanges();
        }

        public int Insert(t_DYJXC_GXHB entity)
        {
            jxc.t_DYJXC_GXHB.Add(entity);
            return jxc.SaveChanges();
        }

        public int Insert(t_CPInBillHeader entity)
        {
            jxc.t_CPInBillHeader.Add(entity);
            return jxc.SaveChanges();
        }

        public int Update(t_DY_YZK entity)
        {
            emt.t_DY_YZK.Attach(entity);
            emt.Entry(entity).State = System.Data.Entity.EntityState.Modified;
            return emt.SaveChanges();
        }
    }
}
