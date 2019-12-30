using LiteDB;
using Rdr2ModManager.Helper;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Rdr2ModManager.Data
{
    public class modSourceCrud : IDisposable
    {

        public List<modSource> Get()
        {
            var retval = new List<modSource>();
            try
            {
                using (var db = new LiteDatabase(@"Rdr2ModsDB"))
                {
                    LogFactory log = new LogFactory();
                    log.infoLog("Getting all mod sources");
                    var modSources = db.GetCollection<modSource>("mods");
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

        public List<modSource> Post(modSource _mod)
        {
            var retval = new List<modSource>();
            try
            {
                using (var db = new LiteDatabase(@"Rdr2ModsDB"))
                {
                    LogFactory log = new LogFactory();
                    log.infoLog("Posting a new mod source");
                    var mods = db.GetCollection<modSource>("mods");
                    _mod.Id = Guid.NewGuid().ToString();
                    _mod.creationDate = DateTime.Now;
                    _mod.modifiedDate = DateTime.Now;
                    _mod.modifiedBy = UserHelper.GetWinUser();
                    mods.Insert(_mod);

                    retval = Get();
                    log.infoLog("Posting completed for new mod source");
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

        public void Del(modSource _mod)
        {
            try
            {
                using (var db = new LiteDatabase(@"Rdr2ModsDB"))
                {
                    LogFactory log = new LogFactory();
                    log.infoLog(string.Format("Deleting mod source {0}", _mod.Name));
                    var mods = db.GetCollection<modSource>("mods");
                    //  first delete all relating mod files
                    using (modFileCrud mfCrud = new modFileCrud())
                    {
                        foreach (var item in mfCrud.Get().Where(mfid => mfid.ModId == _mod.Id))
                        {
                            mfCrud.Del(item);
                        }
                    }
                    mods.Delete(mid => mid.Id == _mod.Id);
                    log.infoLog(string.Format("Mod source {0} deleted", _mod.Name));
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
        // ~modSourceCrud()
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
