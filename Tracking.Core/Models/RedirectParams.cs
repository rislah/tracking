namespace Tracking.Core.Models
{
    public class RedirectParams
    {
        public readonly string Hmac;
        public readonly string PropertiesBase64;

        public RedirectParams(string hmac, string propertiesBase64)
        {
            Hmac = hmac;
            PropertiesBase64 = propertiesBase64;
        }
    }
}