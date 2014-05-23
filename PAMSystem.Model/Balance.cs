using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PAMSystem.Model
{
   public  class Balance
    {

       public Guid Id { get; set; }
       public Guid OperatorId { get; set; }
       public decimal Balances { get; set; }

    }
}
