using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using WYNLAB.Api.Data;
using WYNLAB.Api.Repositories;
using WYNLAB.Api.Services;

var builder = WebApplication.CreateBuilder(args);

// ---- DI 등록 ----
builder.Services.AddSingleton<IDapperContext, DapperContext>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserManageRepository, UserManageRepository>();
builder.Services.AddScoped<IUserGroupManageRepository, UserGroupManageRepository>();
builder.Services.AddScoped<IMenuManageRepository, MenuManageRepository>();
builder.Services.AddScoped<IMenuRepository, MenuRepository>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IJwtTokenService, JwtTokenService>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// ---- JWT 인증 설정 ----
var jwtSection = builder.Configuration.GetSection("Jwt");
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtSection["Issuer"],
            ValidAudience = jwtSection["Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSection["SecretKey"]!))
        };
    });

builder.Services.AddAuthorization();

var app = builder.Build();

// 로그인(AuthController)은 [AllowAnonymous] 대상이며, 이후 추가되는 업무 API 컨트롤러는
// 기본적으로 인증 필요 - 각 컨트롤러/액션에 [Authorize] 부여 예정
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
