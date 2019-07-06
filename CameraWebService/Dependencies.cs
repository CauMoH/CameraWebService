using CameraService.Core.DAL;
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
            Bind<IAuthProvider>().To<FormAuthProvider>();
            Bind<IUnitOfWork>().To<UnitOfWork>();
        }
    }
}