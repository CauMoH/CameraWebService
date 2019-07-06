using System.ComponentModel.DataAnnotations;

namespace CameraService.Core.Entities
{
    public class Camera : BaseEntity
    {
        [Display(Name = "Название камеры")]
        public string Name { get; set; }

        [Display(Name = "Адрес камеры")]
        public string IpAddress { get; set; }
    }
}
