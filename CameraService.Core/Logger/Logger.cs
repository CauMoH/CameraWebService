using System;
using Microsoft.Practices.EnterpriseLibrary.Logging;

namespace CameraService.Core.Logger
{
    public class Logger : ILogger
    {
        private static readonly LogWriter LogWriter = new LogWriterFactory().Create();

        private const string CategoryError = "Error";
        private const string CategoryInformation = "Information";

        public void WriteError(Exception ex, bool isShow = false)
        {
            WriteError(ex.ToString(), isShow);
        }

        public void WriteError(string error, Exception ex, bool isShow = false)
        {
            WriteError($"{error}{Environment.NewLine}{ex}", isShow);
        }

        public void WriteError(string error, bool isShow = false)
        {
            LogWriter.Write(error, CategoryError, 0, 0, System.Diagnostics.TraceEventType.Error);
        }

        public void WriteInformation(string text)
        {
            LogWriter.Write(text, CategoryInformation, 0, 0, System.Diagnostics.TraceEventType.Information);
        }
    }
}