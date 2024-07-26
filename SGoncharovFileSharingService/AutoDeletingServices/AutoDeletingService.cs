
using SGoncharovFileSharingService.Repository.FileRepository;
using SGoncharovFileSharingService.Services.FileServices;

namespace SGoncharovFileSharingService;

public class AutoDeletingService : BackgroundService, IDisposable
{
    private readonly IWebHostEnvironment _webHostEnvironment;
    private readonly IFileRepository _fileRepository;
    private Timer? _timer = null;

    public AutoDeletingService(IWebHostEnvironment webHostEnvironment, IFileRepository fileRepository)
    {
        _webHostEnvironment = webHostEnvironment;
        _fileRepository = fileRepository;
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _timer = new Timer(DeleteSharingFiles, null, TimeSpan.Zero, TimeSpan.FromSeconds(10));
        return Task.CompletedTask;
    }

    public async void DeleteSharingFiles(object? state)
    {
        var sharedFilesDirectory = Directory.EnumerateFiles(_webHostEnvironment.WebRootPath);
        foreach (var sharedFile in sharedFilesDirectory)
        {
            if ((File.GetCreationTimeUtc(sharedFile).AddDays(1)) < DateTime.UtcNow)
            {
                try
                {
                    File.Delete(sharedFile);
                    await _fileRepository.DeleteFileInfoByPathAsync(sharedFile);
                }
                catch (System.Exception ex)
                {
                    System.Console.WriteLine(ex.Message);
                }
            }
        }
    }

    public override void Dispose()
    {
        _timer?.Dispose();
    }
}
