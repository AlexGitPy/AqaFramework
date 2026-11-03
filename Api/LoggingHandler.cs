using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using Allure.Net.Commons;
using AqaFramework.Utils;

namespace AqaFramework.Api;

/// <summary>
/// Перехватывает каждый HTTP-запрос: оборачивает его в Allure-шаг, пишет полный
/// запрос и ответ (заголовки + тело) в консоль и прикладывает их к шагу вложениями.
/// </summary>
public class LoggingHandler() : DelegatingHandler(new HttpClientHandler())
{
    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken) =>
        Step.RunAsync($"{request.Method} {request.RequestUri}", async () =>
        {
            var requestBody = request.Content is null ? "" : await request.Content.ReadAsStringAsync(cancellationToken);
            var requestText = Format($"{request.Method} {request.RequestUri}", request.Headers, request.Content, requestBody);
            Log.Write($"Запрос:\n{requestText}");
            Attach("Request", requestText);

            var response = await base.SendAsync(request, cancellationToken);

            // LoadIntoBufferAsync — тело можно прочитать ещё раз уже в тесте
            await response.Content.LoadIntoBufferAsync();
            var responseBody = await response.Content.ReadAsStringAsync(cancellationToken);
            var responseText = Format($"{(int)response.StatusCode} {response.StatusCode}", response.Headers, response.Content, responseBody);
            Log.Write($"Ответ:\n{responseText}");
            Attach("Response", responseText);

            return response;
        });

    private static string Format(string firstLine, IEnumerable<KeyValuePair<string, IEnumerable<string>>> headers,
        HttpContent? content, string body)
    {
        var sb = new StringBuilder(firstLine).AppendLine();
        var allHeaders = content is null ? headers : headers.Concat(content.Headers);
        foreach (var (name, values) in allHeaders)
            sb.AppendLine($"{name}: {string.Join(", ", values)}");

        if (body.Length > 0)
            sb.AppendLine().AppendLine(PrettyJson(body));

        return sb.ToString();
    }

    private static string PrettyJson(string body)
    {
        try
        {
            using var doc = JsonDocument.Parse(body);
            return JsonSerializer.Serialize(doc.RootElement, new JsonSerializerOptions
            {
                WriteIndented = true,
                Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping // кириллица без \u-экранирования
            });
        }
        catch (JsonException)
        {
            return body;
        }
    }

    private static void Attach(string name, string text) =>
        AllureApi.AddAttachment(name, "text/plain", Encoding.UTF8.GetBytes(text), ".txt");
}
