using System.Net.Mail;

namespace eshop_productapi.Business.Helpers
{
    public static class CommonHelper
    {
        public static bool IsValidEmail(string email)
        {
            try
            {
                var addr = new MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }
    }
}