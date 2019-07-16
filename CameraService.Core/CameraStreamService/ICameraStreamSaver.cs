namespace CameraService.Core.CameraStreamService
{
    public interface ICameraStreamSaver
    {
        CameraStream GetCameraCaptureStream(int cameraId);
    }
}
