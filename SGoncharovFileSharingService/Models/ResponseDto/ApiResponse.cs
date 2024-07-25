namespace SGoncharovFileSharingService.Models.ResponseDto
{
    public class ApiResponse<T>
    {
        public T? Data { get; set; }
        public string? ErrorDetails { get; set; }
        public int StatusCode { get; set; }
    }
}
