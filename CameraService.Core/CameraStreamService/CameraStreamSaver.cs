using System.Collections.Generic;
using System.Linq;
using CameraService.Core.DAL;
using CameraService.Core.Logger;

namespace CameraService.Core.CameraStreamService
{
    public class CameraStreamSaver : ICameraStreamSaver
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger _logger;
        private readonly List<CameraStream> _cameraStreams = new List<CameraStream>();

        public CameraStreamSaver(IUnitOfWork unitOfWork, ILogger logger)
        {
            _logger = logger;

            _unitOfWork = unitOfWork;
            StartCamerasRecording();
        }

        private void StartCamerasRecording()
        {
            _logger.WriteInformation("Start cameras streaming...");

            var cameras = _unitOfWork.CameraRepository.GetAllCameras();
            foreach (var camera in cameras)
            {
                _cameraStreams.Add(new CameraStream(camera));
            }
        }

        public CameraStream GetCameraCaptureStream(int cameraId)
        {
            return _cameraStreams.FirstOrDefault(stream => stream.Id == cameraId);
        }
    }
}
