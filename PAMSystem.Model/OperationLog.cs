using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PAMSystem.Model
{
     public class OperationLog
    {
        public System.Guid Id { get; set; }
        public System.Guid OperatorId { get; set; }
        public System.DateTime OperatingDate { get; set; }  //日期
        public System.String ActionDesc { get; set; } //操作描述
    }
}
