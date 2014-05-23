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
   public class OperatorDAL
    {
       //根据Id获得数据
        public Operator GetById(Guid id)
        {
            DataTable table = SqlHelper.ExecuteDataTable("select * from T_Operator where Id=@Id and IsDeleted=0",
                new SqlParameter("@Id", id));
            if (table.Rows.Count <= 0)
            {
                return null;
            }
            else if (table.Rows.Count > 1)
            {
                throw new Exception("存在Id重复用户！");
            }
            else
            {
                return ToOperator(table.Rows[0]);
            }
        }
        //通过用户名查询
        public Operator GetByUserName(string userName)
        {
            DataTable table = SqlHelper.ExecuteDataTable(@"select * from T_Operator where UserName=@UserName and IsDeleted=0",
                 new SqlParameter("@UserName", userName));
            if (table.Rows.Count <= 0)
            {
                return null;
            }
            else if (table.Rows.Count > 1)
            {
                throw new Exception("用户名重复了！");
            }
            else
            {
                return ToOperator(table.Rows[0]);
            }
        }
       //根据Id删除数据用户
        public int DeleteById(Guid id)
        {
            //软删除
            return SqlHelper.ExecuteNonQuery("Update T_Operator Set IsDeleted=1 where Id=@Id",
             new SqlParameter("@Id", id));
        }
       //恢复已删除用户
        public int ReuseOperator(Guid id)
        {
            return SqlHelper.ExecuteNonQuery("Update T_Operator Set IsDeleted=0 where Id=@Id",
             new SqlParameter("@Id", id));
        }
       //显示所有用户信息
        public Operator[] ListAll()
        {
            DataTable dt = SqlHelper.ExecuteDataTable("select * from T_Operator where IsDeleted=0");
            Operator[] operators = new Operator[dt.Rows.Count];
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                operators[i] = ToOperator(dt.Rows[i]);
            }
            return operators;
        }
        //执行新增（插入）数据,或判断密码是否正确
        public int Insert(Operator operators)
        {
               return SqlHelper.ExecuteNonQuery(@"insert into T_Operator(
                Id,UserName,Password,IsDeleted,IsLocked) values(newid(),@UserName,@Password,0,0)",
                new SqlParameter("@UserName", operators.UserName),
             new SqlParameter("@Password", operators.Password));
        }
       //赋值给Operator属性值
        private Operator ToOperator(DataRow row)
        {
            Operator operators = new Operator();
            operators.Id = (Guid)row["Id"];
            operators.UserName = (string)row["UserName"];
            operators.Password = (string)row["Password"];
            operators.IsDeleted = (bool)row["IsDeleted"];
            operators.IsLocked = (bool)row["IsLocked"];
            return operators;
        }
        //修改用户信息，更改用户密码，不改用户名
        public int Update(Guid id, string pwd)
        {
         int i=SqlHelper.ExecuteNonQuery("update T_Operator set Password=@Password where Id=@Id",
                new SqlParameter("@Password", pwd),
                new SqlParameter("@Id", id));
         return i;
        }
       //修改用户信息，并且修改密码
        public int Update(Guid id, string userName,string pwd)
        {
           return SqlHelper.ExecuteNonQuery("update T_Operator set UserName=@UserName,Password=@Password where Id=@Id",
               new SqlParameter("@UserName", userName),
               new SqlParameter("@Password", pwd),
               new SqlParameter("@Id", id));
        }

    }
}
