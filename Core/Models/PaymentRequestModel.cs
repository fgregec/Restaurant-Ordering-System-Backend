using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models
{
    public class PaymentRequestModel
    {
        public int Amount { get; set; }
        public string Token { get; set; }
    }
}
