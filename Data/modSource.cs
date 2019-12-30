using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rdr2ModManager.Data
{
    public class modSource : auditEntry
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Root { get; set; }
        public string Version { get; set; }
        public DateTime ReleaseDate { get; set; }
        public string TargetId { get; set; }
    }
}
