using DAL;


namespace DAL
{
     interface IYzkRepository:IRepository<t_DY_YZK>
    {
        int Update(t_DY_YZK entity);

        t_DY_YZK GetYzk(string fgh);
    }
}
