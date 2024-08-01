
using SGoncharovFileSharingService.Repository.FileRepository;
using SGoncharovFileSharingService.Services.FileServices;

namespace SGoncharovFileSharingService;

public class AutoDeletingService : BackgroundService, IDisposable
{
    private readonly IWebHostEnvironment _webHostEnvironment;

    private readonly IServiceScopeFactory _serviceScopeFactory;

    private readonly ILogger _logger;

    public AutoDeletingService(IWebHostEnvironment webHostEnvironment, IServiceScopeFactory scopeFactory,ILogger logger)
    {
        _webHostEnvironment = webHostEnvironment;
        _serviceScopeFactory = scopeFactory;
        _logger = logger;
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        try
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                DeleteSharingFiles(stoppingToken);
                Task.Delay(TimeSpan.FromDays(1), stoppingToken);
            }
        }
        catch (System.Exception ex)
        {
            _logger.LogInformation(ex.Message);
        }

        return Task.CompletedTask;
    }

    public async void DeleteSharingFiles(object? state)
    {
        using (var scopedRepository = _serviceScopeFactory.CreateScope())
        {
            var fileRepository = scopedRepository.ServiceProvider.GetRequiredService<IFileRepository>();
            var sharedFilesDirectory = Directory.EnumerateFiles(_webHostEnvironment.WebRootPath);

            foreach (var sharedFile in sharedFilesDirectory)
            {
                if (File.GetCreationTimeUtc(sharedFile).AddDays(1) > DateTime.UtcNow)
                {
                    continue;
                }
                File.Delete(sharedFile);
                await fileRepository.DeleteFileInfoByPathAsync(sharedFile);
            }
        }
    }
}
