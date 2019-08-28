using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Net;
using Accord.Video.FFMPEG;
using AForge.Video;
using CameraService.Core.Entities;
using CameraService.Core.Logger;
using Newtonsoft.Json;

namespace CameraService.Core.CameraStreamService
{
    public class CameraStream
    {
        private readonly MJPEGStream _stream = new MJPEGStream();
        private readonly Camera _camera;
        private readonly ILogger _logger;
        private readonly VideoFileWriter _videoFileWriter = new VideoFileWriter();
        
        private long _frames;
        private const int FrameRate = 8;
        private const long MaxVideoFrames = FrameRate * 60 * 5; //5 минут видео

        public string FilePath { get; }
        public int Id => _camera.Id;

        public string GetFilePath => @"C:\Videos\" +
                                     _camera.Name + "_" +
                                     DateTime.Now.ToString("MM_dd_yyyy HH_mm_ss") +
                                     ".avi";


        public CameraStream(Camera camera, ILogger logger)
        {
            _logger = logger;

            _camera = camera;
            
            FilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Content", _camera.Name + ".jpeg");
            
            _videoFileWriter.Open(GetFilePath, 1024, 768, FrameRate, VideoCodec.MPEG4);
            
            var source = "http://" + camera.IpAddress + "/";
            _stream.Source = source;
            _stream.NewFrame += StreamOnNewFrame;
            _stream.Start();
        }

        public void SetPtzState(string command)
        {
            try
            {
                var ip = _camera.IpAddress.Substring(0, _camera.IpAddress.IndexOf(":", StringComparison.Ordinal));
                var url = "http://" + ip;

                var data = "ptz?params=" + command;

                var request = (HttpWebRequest)WebRequest.Create(url + "/" + data);
                request.Method = "POST";
                request.ContentType = "application/json";


                var webResponse = request.GetResponse();
                using (var webStream = webResponse.GetResponseStream() ?? Stream.Null)
                using (var responseReader = new StreamReader(webStream))
                {
                    var response = responseReader.ReadToEnd();
                    dynamic result = JsonConvert.DeserializeObject(response);
                    var resut = result.return_value;
                }
            }
            catch (Exception exception)
            {
                _logger.WriteError(exception);
            }
        }

        public void ResetRecording()
        {
            OpenRecording();
        }

        private void OpenRecording()
        {
            _frames = 0;
            _videoFileWriter.Close();
            _videoFileWriter.Open(GetFilePath, 1024, 768, FrameRate, VideoCodec.MPEG4);
        }

        #region Event Handlers

        private void StreamOnNewFrame(object sender, NewFrameEventArgs e)
        {
            e.Frame.RotateFlip(RotateFlipType.Rotate180FlipNone);
            e.Frame.Save(FilePath, ImageFormat.Jpeg);
            
            _videoFileWriter.WriteVideoFrame(e.Frame);

            _frames++;

            if (_frames > MaxVideoFrames)
            {
                OpenRecording();
            }
        }

        #endregion
    }
}
