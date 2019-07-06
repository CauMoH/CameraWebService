using System;
using CameraService.Core.DAL.Repositories;

namespace CameraService.Core.DAL
{
    public interface IUnitOfWork : IDisposable
    {
        ICameraRepository CameraRepository { get; }

        void Save();
    }
}
