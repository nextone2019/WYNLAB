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
builder.Services.AddScoped<IMenuAuthAssignRepository, MenuAuthAssignRepository>();
builder.Services.AddScoped<ICodeManageRepository, CodeManageRepository>();
builder.Services.AddScoped<IMenuRepository, MenuRepository>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IJwtTokenService, JwtTokenService>();
builder.Services.AddScoped<IMenuPermissionService, MenuPermissionService>();

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
else
{
    // HSTS: 운영에선 한번 https로 접속에 성공하면 브라우저/클라이언트가 이후 요청을
    // 자동으로 https로만 보내도록 강제한다. 지금 운영 API가 실제로는 http로 서비스되고
    // 있어서(appsettings.Prod.json ApiBaseUrl 참고 - 서버에 TLS 인증서가 아직 없음) 당장은
    // 효과가 없지만, 인증서를 설치하고 클라이언트 ApiBaseUrl을 https로 바꾸는 순간부터
    // 바로 적용되도록 미리 켜둔다.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
