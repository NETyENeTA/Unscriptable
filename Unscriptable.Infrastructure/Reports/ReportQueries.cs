using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unscriptable.Infrastructure.Reports;

public static class ReportQueries
{
    // 1. Лидеры и отстающие (используем связь через student_groups)
    public const string GroupLeadersAndLaggards = @"
        WITH StudentScores AS (
            SELECT g.name AS GroupName, 
                   u.firstname || ' ' || u.lastname AS FullName, 
                   AVG(a.score) AS AvgScore
            FROM groups g
            JOIN student_groups sg ON g.id = sg.groupsid
            JOIN students s ON sg.studentsid = s.id
            JOIN users u ON s.userid = u.id
            JOIN attempts a ON a.studentid = s.id
            WHERE (@GroupId IS NULL OR g.id = @GroupId)
            GROUP BY g.name, u.firstname, u.lastname
        )
        SELECT * FROM StudentScores ORDER BY AvgScore DESC;";

    // 2. Детальные результаты студента
    public const string StudentTestResults = @"
        SELECT t.id as TestId, t.title as TestTitle, MAX(a.score) as BestScore, 
               t.maxscore as MaxPossibleScore,
               (MAX(a.score) >= t.maxscore * 0.7) as Passed,
               MAX(a.submittedat) as CompletedAt,
               COUNT(a.id) as AttemptsCount
        FROM tests t
        LEFT JOIN attempts a ON a.testid = t.id AND a.studentid = @StudentId
        WHERE @StudentId IS NOT NULL
        GROUP BY t.id, t.title, t.maxscore
        ORDER BY t.title;";

    // 3. Динамика группы по месяцам
    public const string GroupTrend = @"
        SELECT DATE_TRUNC('month', a.submittedat) AS Month,
               AVG(a.score) AS AverageScore,
               COUNT(a.id) AS TestsCompleted
        FROM attempts a
        JOIN student_groups sg ON a.studentid = sg.studentsid
        WHERE sg.groupsid = @GroupId AND a.submittedat IS NOT NULL
        GROUP BY Month ORDER BY Month;";

    // 4. Общий рейтинг студентов (используем DENSE_RANK для мест)
    public const string StudentRating = @"
        SELECT DENSE_RANK() OVER (ORDER BY SUM(a.score) DESC) as Rank,
               u.firstname || ' ' || u.lastname as FullName,
               g.name as GroupName,
               SUM(a.score) as TotalScore
        FROM students s
        JOIN users u ON s.userid = u.id
        JOIN student_groups sg ON s.id = sg.studentsid
        JOIN groups g ON sg.groupsid = g.id
        JOIN attempts a ON a.studentid = s.id
        GROUP BY u.firstname, u.lastname, g.name
        ORDER BY Rank ASC;";

    // 5. Личный прогресс ученика по месяцам
    public const string StudentMonthlyProgress = @"
        SELECT DATE_TRUNC('month', a.submittedat) AS Month,
               AVG(a.score) AS AvgScore,
               MAX(a.score) AS MaxScore
        FROM attempts a
        WHERE a.studentid = @StudentId AND a.submittedat IS NOT NULL
        GROUP BY Month ORDER BY Month;";

    // 6. Статистика прохождения (сколько попыток ушло на тест)
    public const string StudentPassRate = @"
        SELECT t.title as TestTitle,
               COUNT(a.id) as AttemptsTaken,
               (CASE WHEN MAX(a.score) >= t.maxscore * 0.7 THEN 'Passed' ELSE 'Failed' END) as Status
        FROM tests t
        JOIN attempts a ON t.id = a.testid
        WHERE a.studentid = @StudentId
        GROUP BY t.id, t.title, t.maxscore;";

    // 7. Сводная карточка успеваемости
    public const string StudentPassRateSummary = @"
        SELECT u.firstname || ' ' || u.lastname AS FullName,
               COUNT(DISTINCT a.testid) AS TestsAttempted,
               COUNT(DISTINCT CASE WHEN a.score >= t.maxscore * 0.7 THEN a.testid END) AS TestsPassed,
               COALESCE(SUM(a.score), 0) AS TotalScore,
               ROUND(AVG(a.score)::NUMERIC, 2) AS AverageScore
        FROM students s
        JOIN users u ON s.userid = u.id
        LEFT JOIN attempts a ON s.id = a.studentid
        LEFT JOIN tests t ON a.testid = t.id
        WHERE s.id = @StudentId
        GROUP BY u.firstname, u.lastname;";

    // 8. Активность по дням недели
    public const string DayOfWeekActivity = @"
        SELECT EXTRACT(DOW FROM a.startedat)::INT AS DayOfWeek, 
               COUNT(a.id) AS TestsCompleted, 
               COUNT(DISTINCT s.id) AS UniqueStudents
        FROM attempts a
        JOIN students s ON a.studentid = s.id
        JOIN student_groups sg ON s.id = sg.studentsid
        WHERE a.submittedat IS NOT NULL 
          AND (@DateFrom IS NULL OR a.startedat >= @DateFrom)
          AND (@DateTo IS NULL OR a.startedat <= @DateTo)
          AND (@GroupId IS NULL OR sg.groupsid = @GroupId)
        GROUP BY DayOfWeek ORDER BY DayOfWeek;";
}