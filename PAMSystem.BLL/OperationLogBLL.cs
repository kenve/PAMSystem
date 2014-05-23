using PAMSystem.DAL;
using PAMSystem.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PAMSystem.BLL
{
   public  class OperationLogBLL
    {
       public bool Insert(Guid opId,string message)
       {
          return new OperationLogDAL().Insert(opId, message);
       }
       public OperationLog[] Search(Guid operatorId)
       {
           return new OperationLogDAL().Search(operatorId);
       }
    }
}
