using Hangfire.Dashboard;
using eshop_productapi.Business.Helpers;

namespace eshop_productapi.API.Filters
{
    public class HangfireAuthorizationFilter : IDashboardAuthorizationFilter
    {
        public bool Authorize(DashboardContext context)
        {
            return new AppSettings().EnableHangfire;
        }
    }
}