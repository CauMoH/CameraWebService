using System;

namespace CameraService.Core.Logger
{
    public interface ILogger
    {
        void WriteError(Exception ex, bool isShow = false);

        void WriteError(string error, Exception ex, bool isShow = false);

        void WriteError(string error, bool isShow = false);

        void WriteInformation(string text);
    }
}