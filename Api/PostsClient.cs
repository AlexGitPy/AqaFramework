using System.Net.Http.Json;
using AqaFramework.Api.Models;
using AqaFramework.Config;
using AqaFramework.Utils;

namespace AqaFramework.Api;

public class PostsClient
{
    // HttpClient создаётся один раз на весь прогон — так рекомендует Microsoft
    private static readonly HttpClient Http = new()
    {
        BaseAddress = new Uri(Settings.Current.ApiBaseUrl)
    };

    public async Task<HttpResponseMessage> GetPostAsync(int id)
    {
        Log.Step($"GET /posts/{id}");
        var response = await Http.GetAsync($"/posts/{id}");
        Log.Step($"Ответ: {(int)response.StatusCode} {response.StatusCode}");
        return response;
    }

    public async Task<HttpResponseMessage> CreatePostAsync(Post post)
    {
        Log.Step($"POST /posts: title='{post.Title}'");
        var response = await Http.PostAsJsonAsync("/posts", post);
        Log.Step($"Ответ: {(int)response.StatusCode} {response.StatusCode}");
        return response;
    }
}
