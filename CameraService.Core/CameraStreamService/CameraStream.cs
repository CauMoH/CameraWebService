using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Timers;
using AForge.Video;
using CameraService.Core.Entities;

namespace CameraService.Core.CameraStreamService
{
    public class CameraStream
    {
        private readonly MJPEGStream _stream = new MJPEGStream();
        private readonly Camera _camera;
        private readonly Timer _updateTimer = new Timer(75);
        private int _counter;
        
        public string FilePath { get; }
        public int Id => _camera.Id;
        
        public CameraStream(Camera camera)
        {
            _camera = camera;

            FilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Content", _camera.Name + ".jpeg");
            
            var source = "http://" + camera.IpAddress + "/";
            _stream.Source = source;
            _stream.NewFrame += StreamOnNewFrame;
            _stream.Start();
            
            _updateTimer.Elapsed += UpdateTimer_OnElapsed;
            _updateTimer.Start();
        }

        private void UpdateTimer_OnElapsed(object sender, ElapsedEventArgs e)
        {
            _counter++;
        }

        private void StreamOnNewFrame(object sender, NewFrameEventArgs e)
        {
            if (_counter > 2)
            {
                _counter = 0;
                e.Frame.Save(FilePath, ImageFormat.Jpeg);
            }
        }
    }
}
