using CameraService.Core.Entities;
using System.Data.Entity;

namespace CameraWebService.DAL.EntityFramework
{
    public class CameraServerDataModel : DbContext
    {
        // Your context has been configured to use a 'LicenseServerModel' connection string from your application's 
        // configuration file (App.config or Web.config). By default, this connection string targets the 
        // 'Astro.Protection.LicenseServer.Models.LicenseServerModel' database on your LocalDb instance. 
        // 
        // If you wish to target a different database and/or database provider, modify the 'LicenseServerModel' 
        // connection string in the application configuration file.
        public CameraServerDataModel()
            : base("name=CameraServerDataModel")
        {

        }

        static CameraServerDataModel()
        {
            Database.SetInitializer(new CameraServerDatabaseInitializer());
        }

        // Add a DbSet for each entity type that you want to include in your model. For more information 
        // on configuring and using a Code First model, see http://go.microsoft.com/fwlink/?LinkId=390109.

        public virtual DbSet<Camera> Cameras { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {

        }
    }

    public class CameraServerDatabaseInitializer : DropCreateDatabaseIfModelChanges<CameraServerDataModel>
    {

    }
}
