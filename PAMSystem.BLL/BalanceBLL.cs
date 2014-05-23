using PAMSystem.DAL;
using PAMSystem.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PAMSystem.BLL
{
   public  class BalanceBLL
    {
       //添加新账户余额
       public void AddNew(Guid operatorId)
       {
           new BalanceDAL().AddNew(operatorId);
       }
       //更新
       public bool Update(Guid operatorId, decimal balances)
       {
         return new BalanceDAL().Update(operatorId,balances);
       }
       //根据Id获得账户余额
       public Balance GetBalance(Guid operatorId)
       {
           return new BalanceDAL().GetByOperatorId(operatorId);
       }
       //删除账户余额
       public bool DeleteByOperatorId(Guid operatorId)
       {
           return new BalanceDAL().DeleteByOperatorId(operatorId);
       }
       //删除账户余额
       public bool ReuseByOperatorId(Guid operatorId)
       {
           return new BalanceDAL().ReuseByOperatorId(operatorId);
       }
    }
}
