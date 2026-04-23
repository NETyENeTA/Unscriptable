using Moq;
using System.Data;
using Unscriptable.Application.DTOs.Dapper;
using Unscriptable.Application.DTOs.Filters;
using Unscriptable.Infrastructure.Services;
using Moq.Dapper;

using System.Data;
using Moq;
using Xunit;
using FluentAssertions;
using Dapper;

namespace Unscriptable.Unit_Tests;


public class UnitTest1
{

    private readonly Mock<IDbConnection> _mockDb;
    private readonly ReportService _service;

    public UnitTest1()
    {
        // Создаём Мокки объект данных (обвёртка)
        _mockDb = new Mock<IDbConnection>();

        // Передаем мок подключения в конструктор сервиса
        _service = new ReportService(_mockDb.Object);
    }


    [Fact]
    public async Task GetStudentTestResultsAsync_ShouldReturnItems_WhenDataExists()
    {
        // Arrange
        var filter = new StudentTestResultsFilter { StudentId = 1 };
        var expectedData = new[]
        {
        new StudentTestResultsItem { TestTitle = "C# Basics", BestScore = 85, Passed = true },
        new StudentTestResultsItem { TestTitle = "SQL Advanced", BestScore = 40, Passed = false }
    };

        // Настраиваем Moq.Dapper на ожидание конкретного SQL и возвращаем данные
        _mockDb.SetupDapperAsync(c => c.QueryAsync<StudentTestResultsItem>(
            It.IsAny<string>(),
            It.IsAny<object>(),
            null, null, null))
            .ReturnsAsync(expectedData);

        // Act
        var result = await _service.GetStudentTestResultsAsync(filter);

        // Assert
        result.Should().HaveCount(2);
        result.First().TestTitle.Should().Be("C# Basics");
        result.Last().Passed.Should().BeFalse();
    }


    [Fact]
    public async Task GetStudentSummary_ShouldThrowException_WhenIdIsZero()
    {
        // Arrange
        var filter = new StudentPassRateSummaryFilter { StudentId = 0 };

        // Act
        // Мы сохраняем действие в переменную, не вызывая его сразу через await
        Func<Task> act = async () => await _service.GetStudentPassRateSummaryAsync(filter);

        // Assert
        // Теперь FluentAssertions перехватит именно ArgumentException из нашего Guard Clause
        await act.Should().ThrowAsync<ArgumentException>()
            .WithParameterName("StudentId") // Проверяем, что ругается на нужный параметр
            .WithMessage("*greater than zero*"); // Проверяем текст ошибки
    }


    [Fact]
    public async Task GetDayOfWeekActivity_ShouldReturnEmpty_WhenNoAttempts()
    {
        // Arrange
        _mockDb.SetupDapperAsync(c => c.QueryAsync<DayOfWeekActivityItem>(
            It.IsAny<string>(), It.IsAny<object>(), null, null, null))
            .ReturnsAsync(Enumerable.Empty<DayOfWeekActivityItem>());

        // Act
        var result = await _service.GetDayOfWeekActivityAsync(new DayOfWeekActivityFilter());

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEmpty();
    }
}