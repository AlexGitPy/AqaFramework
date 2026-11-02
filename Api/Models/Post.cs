namespace AqaFramework.Api.Models;

// System.Text.Json по умолчанию (web-настройки) сам сопоставит camelCase из JSON
public record Post
{
    public int Id { get; init; }
    public int UserId { get; init; }
    public string Title { get; init; } = "";
    public string Body { get; init; } = "";
}
