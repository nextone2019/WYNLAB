using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using WYNLAB.Api.Authorization;
using WYNLAB.Api.Data;
using WYNLAB.Api.Repositories;
using WYNLAB.Api.Repositories.Framework;
using WYNLAB.Api.Repositories.SM;
using WYNLAB.Api.Services;
using WYNLAB.Api.Services.Integrations;

var builder = WebApplication.CreateBuilder(args);

// ---- 로깅(2026-09-15) ----
// 지금까지 운영 서버는 예외가 나도 어디에도 기록이 안 남았다(개발환경에서만 응답 바디에
// 예외 내용을 실어줬을 뿐, 그마저도 파일로는 안 남았음) - w3wp.exe 프로세스가 재시작되면
// 그 순간의 증거가 그냥 사라졌다. ELK/Grafana 같은 별도 인프라를 새로 들이기엔 이 규모
// (서버 한 대)에 안 맞아서, 그냥 파일로 남기는 가장 단순한 방법을 쓴다 - 하루 단위로
// 파일이 나뉘고(logs\wynlab-20260915.log 형태), 오래된 파일은 자동으로 지워진다(31일 보관).
// AppContext.BaseDirectory 기준이라 배포 위치(D:\WYNLAB_SVC\Api\ 등)와 무관하게 그 폴더
// 바로 밑의 logs\에 항상 쌓인다.
builder.Host.UseSerilog((context, services, config) => config
    .MinimumLevel.Information()
    .MinimumLevel.Override("Microsoft.AspNetCore", Serilog.Events.LogEventLevel.Warning)
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .WriteTo.File(
        Path.Combine(AppContext.BaseDirectory, "logs", "wynlab-.log"),
        rollingInterval: RollingInterval.Day,
        retainedFileCountLimit: 31,
        outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff} [{Level:u3}] {Message:lj}{NewLine}{Exception}"));

// ---- DI 등록 ----
builder.Services.AddScoped<IErrorAlertService, ErrorAlertService>();
builder.Services.AddSingleton<IDapperContext, DapperContext>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserManageRepository, UserManageRepository>();
builder.Services.AddScoped<IUserGroupManageRepository, UserGroupManageRepository>();
builder.Services.AddScoped<IMenuManageRepository, MenuManageRepository>();
builder.Services.AddScoped<IMenuAuthAssignRepository, MenuAuthAssignRepository>();
builder.Services.AddScoped<IMinorCodeManageRepository, MinorCodeManageRepository>();
builder.Services.AddScoped<IShortcutRepository, ShortcutRepository>();
builder.Services.AddScoped<IGridLayoutRepository, GridLayoutRepository>();
builder.Services.AddScoped<IFavoriteMenuRepository, FavoriteMenuRepository>();
builder.Services.AddScoped<IPwdResetRepository, PwdResetRepository>();
builder.Services.AddScoped<IFileRepository, FileRepository>();
builder.Services.AddScoped<ISiteConfigRepository, SiteConfigRepository>();
builder.Services.AddSingleton<IFileStorageService, FileStorageService>();

// SMTP는 회사마다 값이 다르므로 appsettings/user-secrets(개발)나 web.config
// environmentVariables(운영, Deploy-Local.ps1이 DB연결문자열을 넣는 자리와 동일)에서만
// 온다 - SmtpSettings.cs 설명 참고.
builder.Services.Configure<SmtpSettings>(builder.Configuration.GetSection("Smtp"));
builder.Services.AddScoped<IEmailService, EmailService>();

// 화면별 Repository를 만들지 않고 프로시저를 그대로 실행하는 범용 통로(GENERIC_DATA_API.md 참고).
// 특정 화면 소속이 아니라서 Repositories\SM\ 같은 모듈 폴더가 아니라 루트에 둔다.
builder.Services.AddScoped<IGenericDataRepository, GenericDataRepository>();
builder.Services.AddScoped<IPopupLookupRepository, PopupLookupRepository>();
builder.Services.AddScoped<IScreenBuilderRepository, ScreenBuilderRepository>();
builder.Services.AddScoped<IProcBuilderRepository, ProcBuilderRepository>();

// 외부 API 연동 프레임워크(2026-09-15) - 정의(TSMAPIDEF)는 ApiIntegrationRepository/Runner가
// 공통으로 다루고, 연동마다 다른 fetch/parse/저장 로직만 IExternalApiIntegration 구현체로 추가한다.
// 새 연동을 붙일 때는 구현체 클래스 + 아래 AddScoped 한 줄만 추가하면 됨(컨트롤러/화면은 공용).
builder.Services.AddHttpClient();
builder.Services.AddScoped<IApiIntegrationRepository, ApiIntegrationRepository>();
builder.Services.AddScoped<IApiIntegrationRunner, ApiIntegrationRunner>();
builder.Services.AddScoped<IExternalApiIntegration, EximFxRateIntegration>();
builder.Services.AddScoped<ILookupRepository, LookupRepository>();
builder.Services.AddScoped<IMenuRepository, MenuRepository>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IJwtTokenService, JwtTokenService>();
builder.Services.AddScoped<IMenuPermissionService, MenuPermissionService>();

// MustChangePasswordFilter를 전역으로 걸어서, 비밀번호를 먼저 바꿔야 하는 계정의 토큰으로는
// change-password 말고 어떤 API도 못 부르게 한다(클라이언트 쪽 다이얼로그 우회 방지 - 자세한
// 이유는 그 필터 클래스 설명 참고).
builder.Services.AddControllers(options => options.Filters.Add<MustChangePasswordFilter>());
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

// 모든 요청을 한 줄씩 남긴다(메서드/경로/상태코드/걸린시간) - "그날 몇 시쯔음 이 API가
// 느려졌다/에러가 났다"를 나중에 로그 파일만 보고 되짚어볼 수 있게 한다.
app.UseSerilogRequestLogging();

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

// 컨트롤러 액션에서 처리 안 된 예외는 기본적으로 빈 본문 500만 돌아가서, 클라이언트(WinForms,
// ApiClient.cs)는 "500 Internal Server Error"라는 것 말고는 원인을 알 방법이 없었다(실제로
// 여러 번 겪음 - frmUserAuth 개발 중). 그래서 두 가지를 한다:
//   1) 환경과 무관하게 항상 로그 파일에 전체 스택트레이스를 남기고, 관리자에게 이메일로도
//      알린다(2026-09-15 전까지는 이 부분 자체가 아예 없어서 운영에서 뭐가 잘못돼도 아무도
//      몰랐다).
//   2) 클라이언트에게 돌려주는 응답 내용은 예전 그대로 유지한다 - 개발환경은 실제 예외
//      타입/메시지/스택트레이스까지 그대로(디버깅용), 운영은 스택트레이스를 뺀 일반 메시지만
//      (그대로 노출하면 보안 위험이라 여기는 절대 안 바꾼다).
app.UseExceptionHandler(errorApp => errorApp.Run(async context =>
{
    var feature = context.Features.Get<Microsoft.AspNetCore.Diagnostics.IExceptionHandlerFeature>();
    var ex = feature?.Error;

    if (ex != null)
    {
        Log.Error(ex, "처리되지 않은 예외: {Path}", context.Request.Path);
        var alertService = context.RequestServices.GetRequiredService<IErrorAlertService>();
        await alertService.NotifyAsync(ex, context.Request.Path);
    }

    context.Response.ContentType = "application/json";
    context.Response.StatusCode = StatusCodes.Status500InternalServerError;

    if (app.Environment.IsDevelopment())
    {
        await context.Response.WriteAsJsonAsync(new
        {
            success = false,
            message = ex == null ? "알 수 없는 오류" : $"{ex.GetType().Name}: {ex.Message}",
            stackTrace = ex?.ToString()
        });
    }
    else
    {
        await context.Response.WriteAsJsonAsync(new
        {
            success = false,
            message = "처리 중 오류가 발생했습니다. 잠시 후 다시 시도해주세요."
        });
    }
}));

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

// 배포 스크립트(Deploy-Local.ps1)가 배포 직후 "정말 떴는지"를 확인하는 용도 + 그냥 사람이
// 브라우저로 열어봐도 되는 간단한 헬스체크. DB 연결까지 확인하지는 않는다(API 프로세스
// 자체가 응답하는지만 - DB까지 확인하려면 매 헬스체크마다 쿼리를 날리게 되어 배보다 배꼽이
// 커진다) - 인증도 없다(모니터링 도구가 토큰 없이도 두드릴 수 있어야 하므로).
app.MapGet("/health", () => Results.Ok(new
{
    status = "ok",
    timeUtc = DateTime.UtcNow,
    environment = app.Environment.EnvironmentName
}));

app.Run();
