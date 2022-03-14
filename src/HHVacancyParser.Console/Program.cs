using HHVacancyParser.Core.Reports;
using HHVacancyParser.Core.Extensions;
using HHVacancyParser.Core.Models.Requests;
using HHVacancyParser.Core.Web;
using HHVacancyParser.Console.Extensions;
using HHVacancyParser.Console;

var currentPage = 0;
var client = new HeadHunterClient();

client.ParsedPage += args =>
{
    global::System.Console.WriteLine($"Проанализировано страниц: {++currentPage}/{args.TraversalRequest!.Pages.Length}");
};

Console.WriteLine("Введите название вакансии, которую хотите проанализировать...");

var vacancyName = Console.ReadLine();

if (string.IsNullOrWhiteSpace(vacancyName))
{
    Console.WriteLine("Вы ввели пустую строку, попробуйте снова.");

    return;
}

var request = new HeadHunterPagesTraversalRequest
{
    Request = new HeadHunterRequest
    {
        SearchText = vacancyName,
    }
};

Console.WriteLine("Укажите сколько страниц нужно проанализировать...");

var numberOfPagesString = Console.ReadLine();

if (!string.IsNullOrWhiteSpace(numberOfPagesString) && int.TryParse(numberOfPagesString, out var pages))
{
    pages = Math.Abs(pages);

    if (pages > 10)
        pages = 10;

    request.Pages = pages.GenerateSequence();
}
else
{
    Console.WriteLine("Введены пустые данные, будет проанализирована первая страница.");

    request.Pages = new int[] { 1 };
}

Console.WriteLine($"Получаем вакансии с '{HeadHunterEndpoints.Domain}'...");

var response = await client.GoThroughPagesAsync(request);

if (response.Responses.Count == 0)
{
    Console.WriteLine("Анализ вернул пустые результаты.");

    return;
}

var vacancies = response.Responses
    .Where(x => x.Vacancies != null && x.Vacancies.Length > 0)
    .SelectMany(x => x.Vacancies!)
    .ToArray();

if (vacancies.Length == 0)
{
    Console.WriteLine("Результаты не вернули вакансий.");

    return;
}

Console.WriteLine("Получено вакансий: " + vacancies.Length);
Console.WriteLine("Исключить повторяющиеся вакансии? [y/n]");

var iWantUniqueVacancies = Console.ReadLine();

if (!string.IsNullOrWhiteSpace(iWantUniqueVacancies) && iWantUniqueVacancies.Equals("y", StringComparison.InvariantCultureIgnoreCase))
{
    var beforeCount = vacancies.Length;

    vacancies = vacancies.UniqueVacancies().ToArray();

    var afterCount = vacancies.Length;

    Console.WriteLine("Удалено дубликантов: " + (beforeCount - afterCount));
}

var report = vacancies.CreateVacanciesReport();

report.SearchText = vacancyName;

Console.WriteLine("Сохранить отчет? [y/n]");

var iWantSaveReport = Console.ReadLine();

if (!string.IsNullOrWhiteSpace(iWantSaveReport) && iWantSaveReport.Equals("y", StringComparison.InvariantCultureIgnoreCase))
{
    ReportStorage.SaveReport(report);

    Console.WriteLine("Отчет успешно сохранен.");
}

report.PrintReport();

Console.WriteLine();
Console.WriteLine("Программа успешно завершила свою работу.");
Console.ReadKey(true);