namespace SGoncharovFileSharingService.Options
{
    public class AutoDeletingServiceOptions
    {
        public const string ConfigurationSectionName = "AutoDeletingServiceOptions";
        public int DaysInterval { get; set; }
    }
}
