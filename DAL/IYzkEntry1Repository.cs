using DAL;
using System.Collections.Generic;

namespace DAL
{
    interface IYzkEntry1Repository: IRepository<t_DY_YZKEntry1>
    {
        int Insert(List<t_DY_YZKEntry1> entryList);
    }
}
