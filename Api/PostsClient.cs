using System.Net.Http.Json;
using AqaFramework.Api.Models;
using AqaFramework.Config;

namespace AqaFramework.Api;

public class PostsClient
{
    // HttpClient создаётся один раз на весь прогон — так рекомендует Microsoft.
    // LoggingHandler логирует каждый запрос и ответ и делает из него Allure-шаг.
    private static readonly HttpClient Http = new(new LoggingHandler())
    {
        BaseAddress = new Uri(Settings.Current.ApiBaseUrl)
    };

    public Task<HttpResponseMessage> GetPostAsync(int id) =>
        Http.GetAsync($"/posts/{id}");

    public Task<HttpResponseMessage> CreatePostAsync(Post post) =>
        Http.PostAsJsonAsync("/posts", post);
}
