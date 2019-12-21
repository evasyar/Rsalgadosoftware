using LiteDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rdr2ModManager.Data
{
    public class LogFactory : IDisposable
    {
        public void infoLog(string _msg)
        {
            try
            {
                using (appLogCrud log = new appLogCrud())
                {
                    log.Post(new appLog()
                    {
                        LogType = "INFO",
                        Log = _msg
                    });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

        }

        public void errLog(string _msg)
        {
            try
            {
                using (appLogCrud log = new appLogCrud())
                {
                    log.Post(new appLog()
                    {
                        LogType = "ERROR",
                        Log = _msg
                    });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public void flushLog()
        {
            try
            {
                using (appLogCrud log = new appLogCrud())
                {
                    log.Del();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public List<appLog> getLog()
        {
            List<appLog> retval = new List<appLog>();
            try
            {
                using (appLogCrud log = new appLogCrud())
                {
                    retval = log.Get().AsEnumerable().OrderByDescending(logDT => logDT.creationDate).ToList();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return retval;
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~LogFactory()
        // {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }
        #endregion
    }
}
