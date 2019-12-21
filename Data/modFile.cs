namespace Rdr2ModManager.Data
{
    public class modFile : auditEntry
    {
        public int Id { get; set; }
        public string ModId { get; set; }
        public string Source { get; set; }
        public string FileName { get; set; }
        public string DestOneLevel { get; set; }
    }
}
