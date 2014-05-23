using PAMSystem.DAL;
using PAMSystem.Model;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PAMSystem.BLL
{

    public class AccountBLL
    {
        public int AddNew(Account model)
        {
            return new AccountDAL().Insert(model);
        }
        //根据Id删除
        public int DeleteById(Guid id)
        {
            return new AccountDAL().DeleteById(id);
        }
        //更新
        public bool Update(Account model)
        {
            return new AccountDAL().Update(model);
        }

        public Account GetById(Guid id)
        {
            return new AccountDAL().GetById(id);
        }
        //根据用户名
        public Account[] GetByOperatorId(Guid operatorid)
        {
            return new AccountDAL().GetByOperatorId(operatorid);
        }
        // 根据用户名和收支
        public Account[] GetByOperatorIdAndCostType(Guid operatorid, Guid costType)
        {
            return new AccountDAL().GetByOperatorIdAndCostType(operatorid, costType);
        }
        //查询搜索
        public Account[] Search(string sql,SqlParameter [] parameters)
        {
           return new AccountDAL().Search(sql, parameters);
        }
    }
}

