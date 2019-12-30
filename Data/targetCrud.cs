using LiteDB;
using Rdr2ModManager.Helper;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Rdr2ModManager.Data
{
    public class targetCrud : IDisposable
    {
        public List<target> Post(target _target)
        {
            var retval = Get();
            try
            {
                using (var db = new LiteDatabase(@"Rdr2ModsDB"))
                {
                    var targets = db.GetCollection<target>("targets");
                    _target.Id = Guid.NewGuid().ToString();
                    _target.creationDate = DateTime.Now;
                    _target.modifiedBy = UserHelper.GetWinUser();
                    _target.modifiedDate = DateTime.Now;
                    if (targets.Exists(e => e.root == _target.root)) throw new Exception("Mod target already exist");
                    if (targets.Exists(e => e.rootName == _target.rootName)) throw new Exception("Mod target already exist");
                    targets.Insert(_target);
                    retval = Get();
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

        public void Delete(target _target)
        {
            try
            {
                using (var db = new LiteDatabase(@"Rdr2ModsDB"))
                {
                    var targets = db.GetCollection<target>("targets");
                    targets.Delete(elem => elem.Id == _target.Id);
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

        public List<target> Get()
        {
            var retval = new List<target>();
            try
            {
                using (var db = new LiteDatabase(@"Rdr2ModsDB"))
                {
                    var targets = db.GetCollection<target>("targets");
                    retval = targets.FindAll().ToList();
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
        // ~targetCrud()
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
