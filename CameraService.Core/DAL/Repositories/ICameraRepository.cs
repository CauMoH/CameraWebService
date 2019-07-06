using System.Collections.Generic;
using CameraService.Core.Entities;

namespace CameraService.Core.DAL.Repositories
{
    public interface ICameraRepository
    {
        List<Camera> GetAllCameras();
        Camera GetCameraById(int cameraId);
    }
}
