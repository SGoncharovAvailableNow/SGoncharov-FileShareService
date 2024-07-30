
using SGoncharovFileSharingService.Repository.FileRepository;
using SGoncharovFileSharingService.Services.FileServices;

namespace SGoncharovFileSharingService;

public class AutoDeletingService : BackgroundService, IDisposable
{
    private readonly IWebHostEnvironment _webHostEnvironment;
    private readonly IServiceScopeFactory _serviceScopeFactory;

    public AutoDeletingService(IWebHostEnvironment webHostEnvironment, IServiceScopeFactory scopeFactory)
    {
        _webHostEnvironment = webHostEnvironment;
        _serviceScopeFactory = scopeFactory;
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        Task.Run(async () =>
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(TimeSpan.FromDays(1), stoppingToken);
                DeleteSharingFiles(stoppingToken);
            }
        });
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
                if (!(File.GetCreationTimeUtc(sharedFile).AddDays(1) < DateTime.UtcNow))
                {
                    continue;
                }
                else
                {
                    File.Delete(sharedFile);
                    await fileRepository.DeleteFileInfoByPathAsync(sharedFile);
                }
            }
        }
    }
}
