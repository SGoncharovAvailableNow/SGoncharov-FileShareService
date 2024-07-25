using DotNetEnv;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using SGoncharovFileSharingService.AutoMapper;
using SGoncharovFileSharingService.JwtTokenProvider;
using SGoncharovFileSharingService.Repository.FileRepository;
using SGoncharovFileSharingService.Repository.UserRepository;
using SGoncharovFileSharingService.Services.FileServices;
using SGoncharovFileSharingService.Services.UserServices;
using System.Text;

var builder = WebApplication.CreateBuilder(new WebApplicationOptions()
{
    WebRootPath = Env.GetString("SHARING_FILES")
});
builder.Services.AddAutoMapper(typeof(MappingProfiles));
builder.Services.AddScoped<IJwtTokenProvider,JwtTokenProvider>();
builder.Services.AddScoped<IUserServices, UserServices>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IFileServices,FileServices>();
builder.Services.AddScoped<IFileRepository, FileRepository>();


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateIssuerSigningKey = true,

        ValidIssuer = Env.GetString("JWT_ISSUER"),
        ValidAudience = Env.GetString("JWT_AUDIENCE"),
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(Env.GetString("JWT_KEY"))
        )
    };
});
var app = builder.Build();
app.UseExceptionHandler();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseDeveloperExceptionPage();
}
else 
{
    app.UseHsts();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
