using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unscriptable.Infrastructure.Reports;

public static class ReportQueries
{
    // 1. Активность по дням недели (на основе попыток/attempts)
    public const string DayOfWeekActivity = @"
    WITH days AS (
        -- Генерируем числа от 0 до 6 (0 - воскресенье в Postgres)
        SELECT generate_series(0, 6) AS day_num
    )
    SELECT 
        d.day_num AS DayOfWeek,
        -- Используем функцию to_char для получения названий на русском
        -- 01.01.2023 — это воскресенье, используем его как точку отсчета
        to_char(make_date(2023, 1, 1) + d.day_num, 'TMdy') AS DayName,
        COUNT(a.id) AS TestsCompleted,
        COUNT(DISTINCT a.studentid) AS UniqueStudents
    FROM days d
    LEFT JOIN attempts a ON EXTRACT(DOW FROM a.startedat) = d.day_num
        -- Фильтруем по датам
        AND a.startedat >= @DateFrom 
        AND a.startedat <= @DateTo
        -- Связываем студента с группой через промежуточную таблицу student_groups
        AND (@GroupId IS NULL OR EXISTS (
            SELECT 1 FROM student_groups sg 
            WHERE sg.studentsid = a.studentid AND sg.groupsid = @GroupId
        ))
    GROUP BY d.day_num
    ORDER BY d.day_num";

    // 2. Лидеры и отстающие (средний балл по группе)
    public const string LeadersAndLaggards = @"
    WITH StudentStats AS (
        -- Собираем средний балл для каждого студента в разрезе его группы
        SELECT 
            g.id AS GroupId,
            g.name AS GroupName,
            d.name AS DirectionName,
            c.name AS CourseName,
            u.firstname || ' ' || u.lastname AS StudentName,
            ROUND(AVG(COALESCE(a.score, 0)), 2) AS AvgScore
        FROM groups g
        JOIN directions d ON g.directionid = d.id
        JOIN courses c ON g.courseid = c.id
        JOIN student_groups sg ON sg.groupsid = g.id
        JOIN students s ON s.id = sg.studentsid
        JOIN users u ON u.id = s.userid
        LEFT JOIN attempts a ON a.studentid = s.id
        WHERE (@DirectionId IS NULL OR g.directionid = @DirectionId)
          AND (@CourseId IS NULL OR g.courseid = @CourseId)
        GROUP BY g.id, g.name, d.name, c.name, u.id, u.firstname, u.lastname
    ),
    RankedStudents AS (
        -- Определяем лидера (Rank 1 по убыванию) и отстающего (Rank 1 по возрастанию)
        SELECT *,
            ROW_NUMBER() OVER(PARTITION BY GroupId ORDER BY AvgScore DESC, StudentName ASC) as LeaderRank,
            ROW_NUMBER() OVER(PARTITION BY GroupId ORDER BY AvgScore ASC, StudentName DESC) as LaggardRank
        FROM StudentStats
    )
    SELECT 
        l.GroupId,
        l.GroupName,
        l.DirectionName AS Direction,
        l.CourseName AS Course,
        l.StudentName AS LeaderName,
        l.AvgScore AS LeaderScore,
        lag.StudentName AS LaggardName,
        lag.AvgScore AS LaggardScore
    FROM RankedStudents l
    JOIN RankedStudents lag ON l.GroupId = lag.GroupId
    WHERE l.LeaderRank = 1 AND lag.LaggardRank = 1";

    // 3. Динамика среднего балла группы (Тренды)
    public const string GroupTrend = @"
        SELECT 
            g.id AS GroupId, 
            g.name AS GroupName,
            DATE_TRUNC('month', a.startedat)::date AS Month,
            TO_CHAR(a.startedat, 'TMMon YYYY') AS MonthLabel,
            ROUND(AVG(a.score), 2) AS AverageScore,
            COUNT(a.id) AS AttemptsCount
        FROM groups g
        JOIN student_groups sg ON g.id = sg.groupsid
        JOIN attempts a ON a.studentid = sg.studentsid
        WHERE (@GroupIds IS NULL OR g.id = ANY(@GroupIds))
        GROUP BY g.id, g.name, Month, MonthLabel
        ORDER BY Month DESC";

    // 4. Накопленные баллы студента по месяцам
    public const string StudentMonthlyProgress = @"
    SELECT 
        m.month AS Month,
        m.monthlabel AS MonthLabel,
        m.score AS Score,
        SUM(m.score) OVER (ORDER BY m.month) AS CumulativeScore
    FROM (
        SELECT 
            DATE_TRUNC('month', a.startedat)::date AS month,
            TO_CHAR(a.startedat, 'TMMon YYYY') AS monthlabel,
            SUM(COALESCE(a.score, 0)) AS score
        FROM attempts a
        WHERE a.studentid = @StudentId
        GROUP BY month, monthlabel
    ) m
    ORDER BY m.month DESC";

    // 5. Процент прохождения тестов (Pass Rate)
    public const string StudentPassRate = @"
        SELECT 
            s.id AS StudentId, 
            u.firstname || ' ' || u.lastname AS FullName, 
            g.name AS GroupName,
            COUNT(tg.testsid) AS TestsAvailable, 
            COUNT(tr.id) AS TestsPassed,
            CASE WHEN COUNT(tg.testsid) = 0 THEN 0 
                 ELSE (ROUND(COUNT(tr.id)::NUMERIC / COUNT(tg.testsid), 2) * 100)::INTEGER 
            END AS PassRate
        FROM students s
        JOIN users u ON s.userid = u.id
        JOIN student_groups sg ON sg.studentsid = s.id
        JOIN groups g ON g.id = sg.groupsid
        JOIN test_groups tg ON tg.groupsid = g.id
        LEFT JOIN testresults tr ON tr.testid = tg.testsid 
            AND tr.studentid = s.id 
            AND tr.passed = True
        WHERE (@GroupId IS NULL OR g.id = @GroupId)
        GROUP BY s.id, u.firstname, u.lastname, g.name";

    // 6. Сводка по конкретному студенту
    public const string StudentPassRateSummary = @"
    SELECT 
        s.id AS StudentId,
        u.firstname || ' ' || u.lastname AS FullName,
        -- Считаем уникальные попытки
        (SELECT COUNT(id) FROM attempts WHERE studentid = s.id) AS TestsAttempted,
        -- Считаем только уникальные успешно пройденные тесты
        (SELECT COUNT(DISTINCT testid) FROM testresults WHERE studentid = s.id AND passed = True) AS TestsPassed,
        -- PassRate на основе назначенных группе тестов
        CASE 
            WHEN (SELECT COUNT(testsid) FROM test_groups WHERE groupsid = sg.groupsid) = 0 THEN 0
            ELSE (
                (SELECT COUNT(DISTINCT testid) FROM testresults WHERE studentid = s.id AND passed = True)::NUMERIC / 
                (SELECT COUNT(testsid) FROM test_groups WHERE groupsid = sg.groupsid) * 100
            )::INTEGER
        END AS PassRate,
        -- Чистая сумма баллов по всем попыткам студента без дублей от джоинов
        COALESCE((SELECT SUM(score) FROM attempts WHERE studentid = s.id), 0) AS TotalScore,
        -- Средний балл
        COALESCE(ROUND(AVG(a.score), 2), 0) AS AverageScore
    FROM students s
    JOIN users u ON s.userid = u.id
    JOIN student_groups sg ON sg.studentsid = s.id
    LEFT JOIN attempts a ON a.studentid = s.id
    WHERE s.id = @StudentId
    GROUP BY s.id, u.firstname, u.lastname, sg.groupsid";

    // 7. Общий рейтинг студентов
    public const string StudentRating = @"
        SELECT 
            DENSE_RANK() OVER(ORDER BY SUM(COALESCE(a.score, 0)) DESC) AS Rank,
            u.firstname || ' ' || u.lastname AS FullName,
            c.name AS Course,
            g.name AS Group,
            d.name AS Direction,
            SUM(COALESCE(a.score, 0)) AS TotalScore
        FROM students s
        JOIN users u ON s.userid = u.id
        JOIN student_groups sg ON sg.studentsid = s.id
        JOIN groups g ON g.id = sg.groupsid
        JOIN courses c ON c.id = g.courseid
        JOIN directions d ON d.id = g.directionid
        LEFT JOIN attempts a ON a.studentid = s.id
        WHERE (@DirectionId IS NULL OR g.directionid = @DirectionId)
            AND (@CourseId IS NULL OR g.courseid = @CourseId)
            AND (@GroupId IS NULL OR g.id = @GroupId)
        GROUP BY s.id, u.firstname, u.lastname, c.name, g.name, d.name
        ORDER BY TotalScore DESC
        --LIMIT @TopCount";

    // 8. Все результаты тестов студента
    public const string StudentTestResults = @"
    SELECT 
        t.id AS TestId,
        t.title AS TestTitle,
        MAX(COALESCE(a.score, 0)) AS BestScore,
        COALESCE(t.passingscore, 100) AS MaxPossibleScore,
        BOOL_OR(COALESCE(tr.passed, false)) AS Passed,
        MAX(a.submittedat) AS CompletedAt,
        COUNT(a.id) AS AttemptsCount
    FROM tests t
    JOIN test_groups tg ON tg.testsid = t.id
    JOIN student_groups sg ON sg.groupsid = tg.groupsid
    LEFT JOIN attempts a ON a.testid = t.id AND a.studentid = sg.studentsid
    LEFT JOIN testresults tr ON tr.testid = t.id AND tr.studentid = sg.studentsid AND tr.passed = true
    WHERE sg.studentsid = @StudentId
    GROUP BY t.id, t.title, t.passingscore
    ORDER BY t.title";
}