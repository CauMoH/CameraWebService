using CameraService.Core.CameraStreamService;
using CameraService.Core.Entities;

namespace CameraWebService.Models
{
    public class CameraViewModel
    {
        public Camera Camera { get; }

        public CameraStream CameraStream { get; }

        public CameraViewModel(Camera camera, CameraStream cameraStream)
        {
            Camera = camera;
            CameraStream = cameraStream;
        }
    }
}