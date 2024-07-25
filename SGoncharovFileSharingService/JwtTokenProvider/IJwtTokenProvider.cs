namespace SGoncharovFileSharingService.JwtTokenProvider
{
    public interface IJwtTokenProvider
    {
        string GetJwtToken(Guid id, string name);
    }
}
