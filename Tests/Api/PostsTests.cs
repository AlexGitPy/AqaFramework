using System.Net;
using System.Net.Http.Json;
using Allure.Net.Commons.Attributes;
using Allure.NUnit;
using AqaFramework.Api;
using AqaFramework.Api.Models;
using AqaFramework.Utils;

namespace AqaFramework.Tests.Api;

[TestFixture]
[AllureNUnit]
[Category("API")]
[AllureFeature("Posts API")]
[Parallelizable(ParallelScope.All)]
public class PostsTests
{
    private readonly PostsClient _client = new();

    [Test]
    public async Task GetPost_ById_ReturnsPost()
    {
        var response = await _client.GetPostAsync(1);

        Step.Run("Проверить статус 200 OK", () =>
            response.StatusCode.Should().Be(HttpStatusCode.OK));

        var post = await response.Content.ReadFromJsonAsync<Post>();
        Step.Run("Проверить, что вернулся пост с id = 1 и заголовком", () =>
        {
            post.Should().NotBeNull();
            post!.Id.Should().Be(1);
            post.Title.Should().NotBeNullOrWhiteSpace();
        });
    }

    [Test]
    public async Task GetPost_NonExistent_Returns404()
    {
        var response = await _client.GetPostAsync(99999);

        Step.Run("Проверить статус 404 Not Found", () =>
            response.StatusCode.Should().Be(HttpStatusCode.NotFound));
    }

    [Test]
    public async Task CreatePost_ReturnsCreatedPost()
    {
        var newPost = new Post { UserId = 1, Title = "AQA test", Body = "Created from C#" };

        var response = await _client.CreatePostAsync(newPost);

        Step.Run("Проверить статус 201 Created", () =>
            response.StatusCode.Should().Be(HttpStatusCode.Created));

        var created = await response.Content.ReadFromJsonAsync<Post>();
        Step.Run("Проверить, что ответ совпадает с отправленным постом и получил id", () =>
        {
            created.Should().BeEquivalentTo(newPost, options => options.Excluding(p => p.Id));
            created!.Id.Should().BePositive();
        });
    }
}
