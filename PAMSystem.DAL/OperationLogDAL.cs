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
   public class OperationLogDAL
    {
       //添加记录
        public bool Insert(Guid operatorId, string actionDesc)
        {
          int i=  SqlHelper.ExecuteNonQuery(@"insert into T_OperationLog(
            Id,OperatorId,OperatingDate,ActionDesc)
            values(newid(),@OperatorId,getdate(),@ActionDesc)", new SqlParameter("@OperatorId", operatorId)
                 , new SqlParameter("@ActionDesc", actionDesc));
          if (i > 0)
          {
              return true;
          }
          else
          {
              return false;
          }
        }
        /// <summary>
        /// 查看操作记录
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public OperationLog[] Search(Guid operatorId)
        {
            DataTable table = SqlHelper.ExecuteDataTable(@" select * from T_OperationLog where OperatorId=@OperatorId",
              new SqlParameter("@OperatorId",operatorId));
            OperationLog[] logs = new OperationLog[table.Rows.Count];
            for (int i = 0; i < table.Rows.Count; i++)
            {
                logs[i] = ToModel(table.Rows[i]);
            }
            return logs;
        }
       
        private OperationLog ToModel(DataRow row)
        {
            OperationLog model = new OperationLog();
            model.Id = (System.Guid)row["Id"];
            model.OperatorId = (Guid)row["OperatorId"];
            model.OperatingDate = (DateTime)row["OperatingDate"];
            model.ActionDesc = (string)row["ActionDesc"];
            return model;
        }
    }
}
