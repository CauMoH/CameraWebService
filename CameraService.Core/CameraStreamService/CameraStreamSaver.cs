using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Timers;
using CameraService.Core.DAL;
using CameraService.Core.Logger;

namespace CameraService.Core.CameraStreamService
{
    public class CameraStreamSaver : ICameraStreamSaver
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger _logger;
        private readonly List<CameraStream> _cameraStreams = new List<CameraStream>();
        private readonly Timer _clearTimer = new Timer(60000);
        private const int MaxCountVideos = 1200;
        private const int NumberOfDeleteVideos = 36; //3 часа записей

        public CameraStreamSaver(IUnitOfWork unitOfWork, ILogger logger)
        {
            _logger = logger;

            _clearTimer.Elapsed += ClearTimerOn_Elapsed;
            _clearTimer.Start();

            _unitOfWork = unitOfWork;
            StartCamerasRecording();
        }
        
        private void StartCamerasRecording()
        {
            _logger.WriteInformation("Start cameras streaming...");

            Directory.CreateDirectory(@"C:\Videos");

            var cameras = _unitOfWork.CameraRepository.GetAllCameras();
            foreach (var camera in cameras)
            {
                _cameraStreams.Add(new CameraStream(camera, _logger));
            }
        }

        public CameraStream GetCameraCaptureStream(int cameraId)
        {
            return _cameraStreams.FirstOrDefault(stream => stream.Id == cameraId);
        }

        #region Evetn Handlers

        private void ClearTimerOn_Elapsed(object sender, ElapsedEventArgs e)
        {
            var info = new DirectoryInfo(@"C:\Videos");
            var files = info.GetFiles().OrderBy(p => p.LastWriteTime).ToArray();

            if (files.Length > MaxCountVideos)
            {
                for (var index = 0; index < NumberOfDeleteVideos; index++)
                {
                    var file = files[index];
                    try
                    {
                        File.Delete(file.FullName);
                    }
                    catch (Exception exception)
                    {
                        _logger.WriteError(exception);
                    }
                }
            }
        }

        #endregion
    }
}
