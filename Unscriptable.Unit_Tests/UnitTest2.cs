using FluentAssertions;
using Moq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unscriptable.Application.DTOs.Filters;
using Unscriptable.Infrastructure.Services;

namespace Unscriptable.Unit_Tests;

public class UnitTest2
{

    private readonly Mock<IDbConnection> _mockDb;
    private readonly ReportService _service;

    public UnitTest2()
    {
        // Создаём Мокки объект данных (обвёртка)
        _mockDb = new Mock<IDbConnection>();

        // Передаем мок подключения в конструктор сервиса
        _service = new ReportService(_mockDb.Object);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(-100)]
    public async Task AllMethods_ShouldThrowArgumentException_WhenIdsAreInvalid(int invalidId)
    {
        // Тестируем сразу несколько методов на валидацию ID

        // 1. Тест для StudentPassRate
        var actRate = async () => await _service.GetStudentPassRateAsync(new StudentPassRateFilter { StudentId = invalidId });
        await actRate.Should().ThrowAsync<ArgumentException>().WithParameterName("StudentId");

        // 2. Тест для GroupTrend
        var actTrend = async () => await _service.GetGroupTrendAsync(new GroupTrendFilter { GroupId = invalidId });
        await actTrend.Should().ThrowAsync<ArgumentException>().WithParameterName("GroupId");
    }

}
