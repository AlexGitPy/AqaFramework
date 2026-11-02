using System.Net;
using System.Net.Http.Json;
using AqaFramework.Api;
using AqaFramework.Api.Models;

namespace AqaFramework.Tests.Api;

[TestFixture]
[Category("API")]
[Parallelizable(ParallelScope.All)]
public class PostsTests
{
    private readonly PostsClient _client = new();

    [Test]
    public async Task GetPost_ById_ReturnsPost()
    {
        var response = await _client.GetPostAsync(1);

        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var post = await response.Content.ReadFromJsonAsync<Post>();
        post.Should().NotBeNull();
        post!.Id.Should().Be(1);
        post.Title.Should().NotBeNullOrWhiteSpace();
    }

    [Test]
    public async Task GetPost_NonExistent_Returns404()
    {
        var response = await _client.GetPostAsync(99999);

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Test]
    public async Task CreatePost_ReturnsCreatedPost()
    {
        var newPost = new Post { UserId = 1, Title = "AQA test", Body = "Created from C#" };

        var response = await _client.CreatePostAsync(newPost);

        response.StatusCode.Should().Be(HttpStatusCode.Created);

        var created = await response.Content.ReadFromJsonAsync<Post>();
        created.Should().BeEquivalentTo(newPost, options => options.Excluding(p => p.Id));
        created!.Id.Should().BePositive();
    }
}
