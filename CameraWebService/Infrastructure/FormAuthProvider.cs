using System.Web.Security;
using CameraWebService.Infrastructure.Interfaces;

namespace CameraWebService.Infrastructure
{
    public class FormAuthProvider : IAuthProvider
    {
        public bool Authenticate(string username, string password)
        {
           var result = FormsAuthentication.Authenticate(username, password);
            if (result)
                FormsAuthentication.SetAuthCookie(username, false);
            return result;
        }
    }
}