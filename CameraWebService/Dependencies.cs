using CameraService.Core.CameraStreamService;
using CameraService.Core.DAL;
using CameraService.Core.Logger;
using CameraWebService.DAL.EntityFramework;
using CameraWebService.Infrastructure;
using CameraWebService.Infrastructure.Interfaces;
using Ninject.Modules;

namespace CameraWebService
{
    public class Dependencies : NinjectModule
    {
        public override void Load()
        {
            Bind<ILogger>().To<Logger>();
            Bind<IAuthProvider>().To<FormAuthProvider>();
            Bind<IUnitOfWork>().To<UnitOfWork>();
            Bind<ICameraStreamSaver>().To<CameraStreamSaver>().InSingletonScope();
        }
    }
}