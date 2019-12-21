using System;

namespace Rdr2ModManager.Data
{
    public class appLog : auditEntry
    {
        public int Id { get; set; }
        public string LogType { get; set; }
        public string Log { get; set; }
    }
}
