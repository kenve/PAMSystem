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
    public class IdNameDAL
    {
        //根据Category获得IdName的数组
        public IdName[] GetByCategory(string category)
        {
            DataTable table = SqlHelper.ExecuteDataTable("select Id,Name from T_IdName where Category=@Category",
                 new SqlParameter("@Category", category));
            IdName[] items = new IdName[table.Rows.Count];
            for (int i = 0; i < table.Rows.Count; i++)
            {
                DataRow row = table.Rows[i];
                IdName idname = new IdName();
                idname.Id = (Guid)row["Id"];
                idname.Name = (string)row["Name"];
                items[i] = idname;
            }
            return items;
        }
        public IdName  GetById(Guid id)
        {
            
            DataTable table = SqlHelper.ExecuteDataTable("select * from T_IdName where Id=@Id",
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
                return ToIdName(table.Rows[0]);
            }
        }

        private IdName ToIdName(DataRow row)
        {
            IdName idname= new IdName();
            idname.Id = (Guid)row["Id"];
            idname.Name = (string)row["Name"];
            idname.Category = (string)row["Category"];
            return idname;
        }
        //显示所有
        public IdName[] ListAll()
        {
            DataTable table = SqlHelper.ExecuteDataTable("select * from T_IdName");
            IdName[] idname = new IdName[table.Rows.Count];
            for (int i = 0; i < table.Rows.Count; i++)
            {
                idname[i] = ToIdName(table.Rows[i]);
            }
            return idname;
        }
        //通过Name查找
        public IdName GetByName(string name)
        {
            DataTable table = SqlHelper.ExecuteDataTable("select * from T_IdName where Name=@Name",
                new SqlParameter("@Name", name));
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
                return ToIdName(table.Rows[0]);
            }
        }
    }
}
