using LiteDB;
using Rdr2ModManager.Helper;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Rdr2ModManager.Data
{
    public class appLogCrud : IDisposable, IappLogCrud
    {

        public List<appLog> Get()
        {
            var retval = new List<appLog>();
            try
            {
                using (var db = new LiteDatabase(@"Rdr2ModsDB"))
                {
                    var appLogs = db.GetCollection<appLog>("logs");
                    retval = appLogs.FindAll().ToList();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return retval;
        }

        public void Del()
        {
            try
            {
                using (var db = new LiteDatabase(@"Rdr2ModsDB"))
                {
                    var appLogs = db.GetCollection<appLog>("logs");
                    foreach (var item in appLogs.FindAll().ToList())
                    {
                        appLogs.Delete(item.Id);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public void Post(appLog _appLog)
        {
            try
            {
                using (var db = new LiteDatabase(@"Rdr2ModsDB"))
                {
                    Console.WriteLine(String.Format("Writing {0} log on {1}", _appLog.LogType, DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss tt")));
                    var mods = db.GetCollection<appLog>("logs");
                    _appLog.creationDate = DateTime.Now;
                    _appLog.modifiedDate = DateTime.Now;
                    _appLog.modifiedBy = UserHelper.GetWinUser();
                    mods.Insert(_appLog);
                    Console.WriteLine(String.Format("Completed {0} log", _appLog.LogType));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
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
        // ~appLogCrud()
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
