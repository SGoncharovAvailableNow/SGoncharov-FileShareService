
using Microsoft.Extensions.Options;
using SGoncharovFileSharingService.Options;
using SGoncharovFileSharingService.Repository.FileRepository;
using SGoncharovFileSharingService.Services.FileServices;

namespace SGoncharovFileSharingService;

public class AutoDeletingService : BackgroundService, IDisposable
{
    private readonly IWebHostEnvironment _webHostEnvironment;

    private readonly IServiceScopeFactory _serviceScopeFactory;

    private readonly ILogger _logger;

    private readonly IOptions<AutoDeletingServiceOptions> _options;

    public AutoDeletingService(IWebHostEnvironment webHostEnvironment, IServiceScopeFactory scopeFactory,ILogger logger, IOptions<AutoDeletingServiceOptions> options)
    {
        _webHostEnvironment = webHostEnvironment;
        _serviceScopeFactory = scopeFactory;
        _logger = logger;
        _options = options;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        try
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                DeleteSharingFiles(stoppingToken, stoppingToken);
                await Task.Delay(TimeSpan.FromDays(_options.Value.DaysInterval), stoppingToken);
            }
        }
        catch (System.Exception ex)
        {
            _logger.LogError(ex,ex.Message);
        }
    }

    public async void DeleteSharingFiles(object? state, CancellationToken cancellationToken)
    {
        using (var scopedRepository = _serviceScopeFactory.CreateScope())
        {
            var fileRepository = scopedRepository.ServiceProvider.GetRequiredService<IFileRepository>();
            var sharedFilesDirectory = Directory.EnumerateFiles(_webHostEnvironment.WebRootPath);

            foreach (var sharedFile in sharedFilesDirectory)
            {
                if (File.GetCreationTimeUtc(sharedFile).AddDays(_options.Value.DaysInterval) > DateTime.UtcNow)
                {
                    continue;
                }
                File.Delete(sharedFile);
                await fileRepository.DeleteFileInfoByPathAsync(sharedFile, cancellationToken);
            }
        }
    }
}
