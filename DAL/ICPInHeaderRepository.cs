

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    interface ICPInHeaderRepository:IRepository<t_CPInBillHeader>
    {
        int Insert(t_CPInBillHeader entity);

        int Delete(t_CPInBillHeader entity);
    }
}
