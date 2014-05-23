using PAMSystem.DAL;
using PAMSystem.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PAMSystem.BLL
{
   public class IdNameBLL
    { 
        public  IdName[] GetByCategory(string category)
        {
           return new IdNameDAL().GetByCategory(category);
        }
       //根据Id查找
       public IdName  GetById(Guid id)
       {
           return new IdNameDAL().GetById(id);
       }
       //查询所有
       public IdName[] ListAll()
       {
           return new IdNameDAL().ListAll();
       }
       public IdName GetByName(string name)
       {
           return new IdNameDAL().GetByName(name);
       }
    }
}
