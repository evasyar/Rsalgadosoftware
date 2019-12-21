using LiteDB;
using Rdr2ModManager.Helper;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Rdr2ModManager.Data
{
    public class modFileCrud : IDisposable
    {
        public List<modFile> Get()
        {
            var retval = new List<modFile>();
            try
            {
                using (var db = new LiteDatabase(@"Rdr2ModsDB"))
                {
                    LogFactory log = new LogFactory();
                    log.infoLog("Getting all mod file sources");
                    var modSources = db.GetCollection<modFile>("modfiles");
                    retval = modSources.FindAll().ToList();
                    log.infoLog("Get completed");
                }
            }
            catch (Exception ex)
            {
                using (LogFactory log = new LogFactory())
                {
                    log.errLog(ex.Message);
                }
            }
            return retval;
        }

        public void Post(modFile _mod)
        {
            try
            {
                using (var db = new LiteDatabase(@"Rdr2ModsDB"))
                {
                    LogFactory log = new LogFactory();
                    log.infoLog(string.Format("Posting a new mod source file {0}", _mod.FileName));
                    var mods = db.GetCollection<modFile>("modfiles");
                    var modfile = mods.Find(row => row.Source.ToLower().Equals(_mod.Source.ToLower()));
                    if (modfile != null)
                        mods.Delete(_mod.Id);
                    _mod.creationDate = DateTime.Now;
                    _mod.modifiedDate = DateTime.Now;
                    _mod.modifiedBy = UserHelper.GetWinUser();
                    mods.Insert(_mod);
                    log.infoLog(string.Format("Posting completed for new mod source file {0}", _mod.FileName));
                }
            }
            catch (Exception ex)
            {
                using (LogFactory log = new LogFactory())
                {
                    log.errLog(ex.Message);
                }
            }
        }

        public void Del(modFile _mod)
        {
            try
            {
                using (var db = new LiteDatabase(@"Rdr2ModsDB"))
                {
                    LogFactory log = new LogFactory();
                    log.infoLog(string.Format("Deleting mod source file {0}", _mod.FileName));
                    var mods = db.GetCollection<modFile>("modfiles");
                    var modfile = mods.Find(row => row.Id == _mod.Id);
                    if (modfile != null)
                        mods.Delete(_mod.Id);
                    log.infoLog(string.Format("Mod source file {0} deleted", _mod.FileName));
                }
            }
            catch (Exception ex)
            {
                using (LogFactory log = new LogFactory())
                {
                    log.errLog(ex.Message);
                }
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
        // ~modFileCrud()
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
