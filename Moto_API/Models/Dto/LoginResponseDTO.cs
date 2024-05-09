using System.Net;

namespace Moto_API.Models.Dto
{
    public class LoginResponseDTO
    {
        public UserDTO User { get; set; }
        public string Token { get; set; }
        public HttpStatusCode StatusCode { get; set; }
    }
}
