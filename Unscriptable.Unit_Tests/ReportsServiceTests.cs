using Microsoft.AspNetCore.Http;
using Moq;
using System.Data;
using System.Security.Claims;
using Unscriptable.Application.DTOs.Dapper.Filters;
using Unscriptable.Domain.Enums;
using Unscriptable.Infrastructure.Services;
using Xunit;

public class ReportsServiceTests
{
    private readonly Mock<IDbConnection> _mockDb;
    private readonly Mock<IHttpContextAccessor> _mockAccessor;
    private readonly ReportsService _service;

    public ReportsServiceTests()
    {
        _mockDb = new Mock<IDbConnection>();
        _mockAccessor = new Mock<IHttpContextAccessor>();

        // Создаем сервис с моками
        _service = new ReportsService(_mockDb.Object, _mockAccessor.Object);
    }

    [Fact]
    public async Task GetStudentPassRateSummary_StudentAccessesOtherStudent_ThrowsUnauthorized()
    {
        // 1. Arrange (Настройка)
        var studentIdInRequest = 10; // Студент хочет посмотреть отчет с ID 10
        var studentIdInToken = "11"; // А в его токене зашит ID 11

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, "5"), // UserId
            new Claim(ClaimTypes.Role, "Student"),
            new Claim("StudentId", studentIdInToken)
        };

        var identity = new ClaimsIdentity(claims, "TestAuth");
        var user = new ClaimsPrincipal(identity);

        // Настраиваем имитацию HttpContext
        var mockContext = new DefaultHttpContext { User = user };
        _mockAccessor.Setup(a => a.HttpContext).Returns(mockContext);

        // Создаем фильтр, который передается в метод
        var filter = new StudentPassRateSummaryFilter { StudentId = studentIdInRequest };

        // 2. Act & Assert (Действие и Проверка)
        // Вызываем именно GetStudentPassRateSummaryAsync
        await Assert.ThrowsAsync<UnauthorizedAccessException>(() =>
            _service.GetStudentPassRateSummaryAsync(filter));
    }

    [Fact]
    public async Task GetStudentRating_TeacherAccesses_ShouldPassAuthorization()
    {
        // 1. Arrange
        var claims = new List<Claim>
    {
        new Claim(ClaimTypes.NameIdentifier, "1"),
        new Claim(ClaimTypes.Role, UserRole.Manager.ToString())
    };
        var identity = new ClaimsIdentity(claims, "TestAuth");
        var user = new ClaimsPrincipal(identity);

        _mockAccessor.Setup(a => a.HttpContext).Returns(new DefaultHttpContext { User = user });

        var filter = new StudentRatingFilter {};

        // 2. Act & Assert
        var exception = await Record.ExceptionAsync(() => _service.GetStudentRatingAsync(filter));

        // Если exception — NullReference, значит CheckStaffAccess пройден (успех для Unit-теста логики)
        // Если exception — UnauthorizedAccess, значит логика ролей сломана.
        Assert.True(exception is null || exception is NullReferenceException);
    }
}