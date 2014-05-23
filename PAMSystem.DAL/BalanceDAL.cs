using PAMSystem.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PAMSystem.DAL
{
    public class BalanceDAL
    {
        /// <summary>
        /// 通过用户Id获得余额
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Balance GetByOperatorId(Guid operatorid)
        {
            DataTable table = SqlHelper.ExecuteDataTable(@"select * from T_Balance where OperatorId=@OperatorId and IsDeleted=0",
                new SqlParameter("@OperatorId", operatorid));
            if (table.Rows.Count <= 0)
            {
                return null;
            }
            else if (table.Rows.Count > 1)
            {
                throw new Exception("数据库出现重复！");
            }
            else
            {
                return ToBalance(table.Rows[0]);
            }
        }
        //添加新用户的余额为0
        public void AddNew(Guid operatorId)
        {
            SqlHelper.ExecuteNonQuery(
                 "insert into T_Balance(Id,OperatorId,Balance) values (newid(),@OperatorId,0)",
                 new SqlParameter("@OperatorId", operatorId));
        }
        //更新账户余额
        public bool Update(Guid operatorId,decimal balances)
        {
             int i= SqlHelper.ExecuteNonQuery(@"update T_Balance set Balance=Balance+@Balances where OperatorId=@OperatorId",
                  new SqlParameter("@Balances",balances),
                  new SqlParameter("@OperatorId", operatorId));
             if (i>0)
             {
                 return true;
             }
             else
             {
                 return false;
             }
        }
        //为属性赋值
        private Balance ToBalance(DataRow row)
        {
            Balance balance = new Balance();
            balance.Id = (Guid)row["Id"];
            balance.OperatorId = (Guid)row["OperatorId"];
            balance.Balances = (decimal)row["Balance"];
            return balance;
        }
        //删除账户余额
        public bool DeleteByOperatorId(Guid operatorId)
        {
            int i = SqlHelper.ExecuteNonQuery("Update T_Balance Set IsDeleted=1 where OperatorId=@OperatorId",
             new SqlParameter("@OperatorId", operatorId));
            if (i > 0)
            {
                return true;
            }
            else { return false; };
        }
        //恢复删除账号
        public bool ReuseByOperatorId(Guid operatorId)
        {
            int i = SqlHelper.ExecuteNonQuery("Update T_Balance Set IsDeleted=0 where OperatorId=@OperatorId",
             new SqlParameter("@OperatorId", operatorId));
            if (i > 0)
            {
                return true;
            }
            else { return false; };
        }
    }
}
