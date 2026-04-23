using System.Threading.RateLimiting;
using Microsoft.AspNetCore.RateLimiting;

namespace Web.Infrastructure;

public static class RateLimiterExtensions
{
    public static void AddCustomRateLimiter(this IServiceCollection services)
    {
        services.AddRateLimiter(options =>
        {
            options.AddFixedWindowLimiter("get", opt =>
            {
                opt.PermitLimit = 30;
                opt.Window = TimeSpan.FromSeconds(10);

                opt.QueueLimit = 5;
                opt.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
            });

            options.AddFixedWindowLimiter("post", opt =>
            {
                opt.PermitLimit = 5;
                opt.Window = TimeSpan.FromSeconds(10);

                opt.QueueLimit = 2;
                opt.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
            });

            options.AddFixedWindowLimiter("put", opt =>
            {
                opt.PermitLimit = 5;
                opt.Window = TimeSpan.FromSeconds(10);

                opt.QueueLimit = 2;
                opt.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
            });

            options.AddFixedWindowLimiter("delete", opt =>
            {
                opt.PermitLimit = 2;
                opt.Window = TimeSpan.FromSeconds(10);
            });
        });
    }
}
