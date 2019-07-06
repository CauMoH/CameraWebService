using System;
using System.Collections.Generic;
using System.Linq;
using CameraService.Core.DAL.Repositories;
using CameraService.Core.Entities;

namespace CameraWebService.DAL.EntityFramework.Repositories
{
    public class CameraRepository : ICameraRepository
    {
        private readonly CameraServerDataModel _dataContext;

        public CameraRepository(CameraServerDataModel dataContext)
        {
            _dataContext = dataContext ?? throw new ArgumentNullException();
        }

        public List<Camera> GetAllCameras()
        {
            return _dataContext.Cameras.ToList();
        }

        public Camera GetCameraById(int cameraId)
        {
            return (from p in _dataContext.Cameras where p.Id == cameraId select p).FirstOrDefault();
        }
    }
}
