using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FiscalMachines.CZ.DTO.Data
{
    public class TaxItem
    {
        public TaxItem(decimal net, decimal tax)
        {
            Net = net;
            Tax = tax;
        }

        public decimal Net { get; }

        public decimal Tax { get; }
    }
}
