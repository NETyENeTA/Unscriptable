using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Unscriptable.API.Controllers;
using Unscriptable.Application.DTOs.Dapper;
using Unscriptable.Application.DTOs.Filters;
using Unscriptable.Application.Interfaces;

namespace Unscriptable.Unit_Tests;

public class ReportsControllerTests
{
    private readonly Mock<IReportService> _mockService;
    private readonly ReportsController _controller;

    public ReportsControllerTests()
    {
        _mockService = new Mock<IReportService>();
        _controller = new ReportsController(_mockService.Object);
    }

    [Fact]
    public async Task GetStudentSummary_ShouldReturnForbid_WhenStudentTriesToAccessOtherStudentData()
    {
        // Arrange
        int realStudentId = 10;
        int targetStudentId = 20; // Студент хочет посмотреть чужой ID

        // Имитируем авторизованного пользователя с ролью "Student" и Id = 10
        var claims = new List<Claim>
        {
            new("id", realStudentId.ToString()),
            new(ClaimTypes.Role, "Student")
        };
        var identity = new ClaimsIdentity(claims, "TestAuth");
        var user = new ClaimsPrincipal(identity);

        _controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext { User = user }
        };

        // Act
        var result = await _controller.GetStudentSummary(new StudentPassRateSummaryFilter { StudentId = targetStudentId });

        // Assert
        result.Should().BeOfType<ForbidResult>(); // Должен вернуть 403 Forbidden
        _mockService.Verify(s => s.GetStudentPassRateSummaryAsync(It.IsAny<StudentPassRateSummaryFilter>()), Times.Never);
    }

    [Fact]
    public async Task GetStudentSummary_ShouldAllowAccess_WhenUserIsManager()
    {
        // Arrange
        var claims = new List<Claim>
        {
            new("id", "999"),
            new(ClaimTypes.Role, "Manager") // Пользователь - менеджер
        };
        var user = new ClaimsPrincipal(new ClaimsIdentity(claims, "TestAuth"));

        _controller.ControllerContext = new ControllerContext { HttpContext = new DefaultHttpContext { User = user } };

        _mockService.Setup(s => s.GetStudentPassRateSummaryAsync(It.IsAny<StudentPassRateSummaryFilter>()))
                    .ReturnsAsync(new StudentPassRateSummaryItem());

        // Act
        var result = await _controller.GetStudentSummary(new StudentPassRateSummaryFilter { StudentId = 10 });

        // Assert
        result.Should().BeOfType<OkObjectResult>(); // Менеджеру можно всё
    }
}
