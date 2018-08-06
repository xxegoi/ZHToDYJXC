using DAL;

namespace DAL
{
    interface IGXHBRepository:IRepository<t_DYJXC_GXHB>
    {
        int Insert(t_DYJXC_GXHB entity);
    }
}
