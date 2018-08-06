using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace BLL
{
    public class YzkBLL:IValidate<t_DY_YZK>
    {
        public t_DY_YZK Get(string fgh)
        {
            DBRepository db = new DBRepository();
            return db.GetYzk(fgh);
        }

        public int Update(JObject obj)
        {
            var entity = Validate(obj);
        }

        public t_DY_YZK Validate(JObject obj)
        {
            
        }
    }
}
