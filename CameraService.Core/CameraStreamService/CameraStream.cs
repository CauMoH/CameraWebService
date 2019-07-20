using System;
using System.Drawing.Imaging;
using System.IO;
using System.Net;
using System.Timers;
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
        private readonly Timer _updatePropsTimer = new Timer(1000);
        private readonly ILogger _logger;
        
        public string FilePath { get; }
        public int Id => _camera.Id;

        public bool LedState { get; private set; }
        
        public CameraStream(Camera camera, ILogger logger)
        {
            _logger = logger;

            _camera = camera;

            FilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Content", _camera.Name + ".jpeg");
            
            var source = "http://" + camera.IpAddress + "/";
            _stream.Source = source;
            _stream.NewFrame += StreamOnNewFrame;
            _stream.Start();

            _updatePropsTimer.Elapsed += UpdatePropsTimer_OnElapsed;
            _updatePropsTimer.Start();
        }

        public void SetLedState(bool state)
        {
            try
            {
                var ip = _camera.IpAddress.Substring(0, _camera.IpAddress.IndexOf(":", StringComparison.Ordinal));
                var url = "http://" + ip;

                var data = "led?params=";

                data = state ? data + "1" : data + "0";

                var request = (HttpWebRequest)WebRequest.Create(url + "/" + data);
                request.Method = "POST";
                request.ContentType = "application/json";


                var webResponse = request.GetResponse();
                using (var webStream = webResponse.GetResponseStream() ?? Stream.Null)
                using (var responseReader = new StreamReader(webStream))
                {
                    var response = responseReader.ReadToEnd();
                    dynamic result = JsonConvert.DeserializeObject(response);
                    LedState = result.return_value;
                }
            }
            catch (Exception exception)
            {
                _logger.WriteError(exception);
            }
        }

        #region Event Handlers

        private void UpdatePropsTimer_OnElapsed(object sender, ElapsedEventArgs e)
        {
            SetLedState(LedState);
        }

        private void StreamOnNewFrame(object sender, NewFrameEventArgs e)
        {
            e.Frame.Save(FilePath, ImageFormat.Jpeg);
        }

        #endregion
    }
}
