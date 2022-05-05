namespace Wms.Services;

public class ApplicationHealthService
{
    private readonly ILogger<ApplicationHealthService> logger;

    public ApplicationHealthService(ILogger<ApplicationHealthService> logger)
    {
        this.logger = logger;
    }
}
