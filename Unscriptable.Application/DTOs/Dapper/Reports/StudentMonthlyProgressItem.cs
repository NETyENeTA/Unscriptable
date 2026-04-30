using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unscriptable.Application.DTOs.Dapper.Reports;

public class StudentMonthlyProgressItem
{
    public DateTime Month { get; set; }      // 2024-03-01
    public string MonthLabel { get; set; } = string.Empty; // "Мар 2024"
    public int Score { get; set; }           // Баллы за месяц
    public int CumulativeScore { get; set; } // Накопленные баллы
}

