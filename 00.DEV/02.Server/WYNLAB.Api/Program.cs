using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using WYNLAB.Api.Data;
using WYNLAB.Api.Repositories;
using WYNLAB.Api.Repositories.SM;
using WYNLAB.Api.Services;

var builder = WebApplication.CreateBuilder(args);

// ---- DI 등록 ----
builder.Services.AddSingleton<IDapperContext, DapperContext>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserManageRepository, UserManageRepository>();
builder.Services.AddScoped<IUserGroupManageRepository, UserGroupManageRepository>();
builder.Services.AddScoped<IMenuManageRepository, MenuManageRepository>();
builder.Services.AddScoped<IMenuAuthAssignRepository, MenuAuthAssignRepository>();
builder.Services.AddScoped<IMinorCodeManageRepository, MinorCodeManageRepository>();
builder.Services.AddScoped<IShortcutRepository, ShortcutRepository>();

// 화면별 Repository를 만들지 않고 프로시저를 그대로 실행하는 범용 통로(GENERIC_DATA_API.md 참고).
// 특정 화면 소속이 아니라서 Repositories\SM\ 같은 모듈 폴더가 아니라 루트에 둔다.
builder.Services.AddScoped<IGenericDataRepository, GenericDataRepository>();
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

    // 컨트롤러 액션에서 처리 안 된 예외는 기본적으로 빈 본문 500만 돌아가서, 클라이언트
    // (WinForms, ApiClient.cs)는 "500 Internal Server Error"라는 것 말고는 원인을 알 방법이
    // 없었다(실제로 여러 번 겪음 - frmUserAuth 개발 중). 개발 환경에서만 실제 예외 타입/메시지를
    // JSON으로 내려줘서 ApiClient가 그 본문을 그대로 화면 오류 메시지에 포함시키게 한다.
    // 스택트레이스까지 노출하는 위험이 있어서 이 블록 밖(운영)에는 절대 넣지 않는다.
    app.UseExceptionHandler(errorApp => errorApp.Run(async context =>
    {
        var feature = context.Features.Get<Microsoft.AspNetCore.Diagnostics.IExceptionHandlerFeature>();
        var ex = feature?.Error;
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = StatusCodes.Status500InternalServerError;
        await context.Response.WriteAsJsonAsync(new
        {
            success = false,
            message = ex == null ? "알 수 없는 오류" : $"{ex.GetType().Name}: {ex.Message}",
            stackTrace = ex?.ToString()
        });
    }));
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
