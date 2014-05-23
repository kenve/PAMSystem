using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PAMSystem.Model
{
    public class Account
    {
        public Guid Id { get; set; }
        public Guid OperatorId { get; set; }
        public Guid Item { get; set; }
        public double Money { get; set; }
        public Guid CostType { get; set; }
        public DateTime Date { get; set; }
        //  public DateTime? Date { get; set; }
        public string Remarks { get; set; }
    }
}
