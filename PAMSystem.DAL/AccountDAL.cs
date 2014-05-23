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
    public class AccountDAL
    {
        //根据Id获得数据
        public Account GetById(Guid id)
        {
            DataTable table = SqlHelper.ExecuteDataTable(@"select * from T_Account where Id=@Id and IsDeleted=0",
                new SqlParameter("@Id", id));
            if (table.Rows.Count <= 0)
            {
                return null;
            }
            else if (table.Rows.Count > 1)
            {
                throw new Exception("有重复！");
            }
            else
            {
                return ToAccount(table.Rows[0]);
            }
        }
        //根据用户名的GUi获取数据
        public Account[] GetByOperatorId(Guid operatorId)
        {
            DataTable table = SqlHelper.ExecuteDataTable(@"select * from T_Account where OperatorId=@OperatorId and IsDeleted=0",
                new SqlParameter("@OperatorId", operatorId));
            if (table.Rows.Count <= 0)
            {
                return null;
            }
            else
            {
                Account[] account = new Account[table.Rows.Count];
                for (int i = 0; i < table.Rows.Count; i++)
                {
                    account[i] = ToAccount(table.Rows[i]);
                }
                return account;
            }
        }
        //根据Id删除数据(软删除)
        public int DeleteById(Guid id)
        {
            //软删除
            return SqlHelper.ExecuteNonQuery("Update T_Account Set IsDeleted=1 where Id=@Id",
             new SqlParameter("@Id", id));
        }
        //修改编辑
        public bool Update(Account account)
        {
            string sql = "UPDATE T_Account SET OperatorId=@OperatorId,Item=@Item,Money=@Money,CostType=@CostType,Date=@Date,Remarks=@Remarks WHERE Id=@Id";
            int rows = SqlHelper.ExecuteNonQuery(sql
                , new SqlParameter("@Id", account.Id)
                , new SqlParameter("@OperatorId", account.OperatorId)
                , new SqlParameter("@Item", account.Item)
                , new SqlParameter("@Money", account.Money)
                , new SqlParameter("@CostType", account.CostType)
                , new SqlParameter("@Date", account.Date)
                , new SqlParameter("@Remarks", account.Remarks)
            );
            return rows > 0;
        }
        //执行新增（插入）数据,或判断密码是否正确
        public int Insert(Account account)
        {
            return SqlHelper.ExecuteNonQuery(@"insert into T_Account (Id,OperatorId,Item,Money,CostType,Date,Remarks,IsDeleted) 
   values(newid(),@OperatorId,@Item,@Money,@CostType,@Date,@Remarks,0)",
                new SqlParameter("@OperatorId", account.OperatorId),
                new SqlParameter("@Item", account.Item),
                new SqlParameter("@Money", account.Money),
                new SqlParameter("@CostType", account.CostType),
                new SqlParameter("@Date", account.Date),
                new SqlParameter("@Remarks", account.Remarks));
        }
        //赋值给Account属性值
        private Account ToAccount(DataRow row)
        {
            Account account = new Account();
            account.Id = (Guid)row["Id"];
            account.OperatorId = (Guid)row["OperatorId"];
            account.Item = (Guid)row["Item"];
            account.Money = (double)row["Money"];
            account.CostType = (Guid)row["CostType"];
            account.Date = (DateTime)row["Date"];
            account.Remarks = (string)row["Remarks"];
            return account;
        }
        //通过用户Id和收支查询信息
        public Account[] GetByOperatorIdAndCostType(Guid operatorId, Guid costtype)
        {
            DataTable table = SqlHelper.ExecuteDataTable(@"select * from T_Account where OperatorId=@OperatorId and CostType=@CostType and IsDeleted=0",
                new SqlParameter("@OperatorId", operatorId),
                new SqlParameter("@CostType", costtype));
            if (table.Rows.Count <= 0)
            {
                return null;
            }
            else
            {
                Account[] account = new Account[table.Rows.Count];
                for (int i = 0; i < table.Rows.Count; i++)
                {
                    account[i] = ToAccount(table.Rows[i]);
                }
                return account;
            }
        }
        //搜索帐本信息
        public Account[] Search(string sql, SqlParameter[] parameters)
        {
            DataTable table = SqlHelper.ExecuteDataTable(sql, parameters);
            Account[] acc = new Account[table.Rows.Count];
            for (int i = 0; i < table.Rows.Count; i++)
            {
                acc[i] = ToAccount(table.Rows[i]);
            }
            return acc;
        }
    }
}
