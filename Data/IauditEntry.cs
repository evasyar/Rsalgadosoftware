using System;

namespace Rdr2ModManager.Data
{
    public interface IauditEntry
    {
        DateTime creationDate { get; set; }
        string modifiedBy { get; set; }
        DateTime modifiedDate { get; set; }
    }
}