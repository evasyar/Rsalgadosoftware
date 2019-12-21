using System.Collections.Generic;

namespace Rdr2ModManager.Data
{
    public interface IappLogCrud
    {
        void Del();
        void Dispose();
        List<appLog> Get();
        void Post(appLog _appLog);
    }
}