using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rdr2ModManager.Data
{
    public class auditEntry : IauditEntry
    {
        public DateTime creationDate { get; set; }
        public DateTime modifiedDate { get; set; }
        public string modifiedBy { get; set; }
    }
}
