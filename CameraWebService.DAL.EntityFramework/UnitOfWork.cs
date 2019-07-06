using System;
using CameraService.Core.DAL;
using CameraService.Core.DAL.Repositories;
using CameraWebService.DAL.EntityFramework.Repositories;

namespace CameraWebService.DAL.EntityFramework
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly CameraServerDataModel _dataContext;

        private ICameraRepository _cameraRepository;

        public ICameraRepository CameraRepository => _cameraRepository ?? (_cameraRepository = new CameraRepository(_dataContext));

        public UnitOfWork()
        {
            _dataContext = new CameraServerDataModel();
        }

        public void Save()
        {
            _dataContext.SaveChanges();
        }

        private bool _disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _dataContext.Dispose();
                }
            }
            _disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
