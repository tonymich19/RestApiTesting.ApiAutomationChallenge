using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using RestApiTesting.ApiAutomationChallenge.Helpers;
using RestApiTesting.ApiAutomationChallenge.Models;
using RestApiTesting.ApiAutomationChallenge.Services;
using TechTalk.SpecFlow;

namespace RestApiTesting.ApiAutomationChallenge.StepDefinitions
{
    [Binding]
    [Scope(Tag = "PostsSteps")]
    public sealed class PostsSteps : Steps
    {
        private readonly IJsonPlaceHolderService m_jsonPlaceHolderService;

        public PostsSteps(IJsonPlaceHolderService jsonPlaceHolderService)
        {
            m_jsonPlaceHolderService = jsonPlaceHolderService;
        }

        public HttpClient Client { get => (HttpClient)ScenarioContext[nameof(Client)]; set => ScenarioContext[nameof(Client)] = value; }

        public HttpResponseMessage Response { get => (HttpResponseMessage)ScenarioContext[nameof(Response)]; set => ScenarioContext[nameof(Response)] = value; }

        public PostModel Post { get => (PostModel)ScenarioContext[nameof(Post)]; set => ScenarioContext[nameof(Post)] = value; }

        [When(@"a non-authenticated user sends a request to get a post")]
        public async Task WhenANon_AuthenticatedUserSendsARequestToGetAPost()
        {
            Post = ModelGeneratorHelper.GetPost();
            Response = await m_jsonPlaceHolderService.PostsControllerProxy.GetPostsAsync("1");
        }

        [When(@"a non-authenticated user sends a request to delete a post")]
        public async Task WhenANon_AuthenticatedUserSendsARequestToDeleteAPost()
        {
            Post = ModelGeneratorHelper.GetEmptyPost();
            Response = await m_jsonPlaceHolderService.PostsControllerProxy.DeletePostsAsync("1");
        }

        [When(@"a non-authenticated user sends a request to create a post")]
        public async Task WhenANon_AuthenticatedUserSendsARequestToCreateAPost()
        {
            Post = ModelGeneratorHelper.GeneratePost();
            Response = await m_jsonPlaceHolderService.PostsControllerProxy.CreatePostsAsync(Post);
        }

        [When(@"a non-authenticated user sends a request to update a post")]
        public async Task WhenANon_AuthenticatedUserSendsARequestToUpdateAPost()
        {
            Post = ModelGeneratorHelper.UpdatePost();
            Response = await m_jsonPlaceHolderService.PostsControllerProxy.UpdatePostsAsync("1", Post);
        }

        [When(@"a non-authenticated user sends a request to patch a post")]
        public async Task WhenANon_AuthenticatedUserSendsARequestToPatchAPost()
        {
            Post = ModelGeneratorHelper.PatchedPost();
            PatchPostModel patchModel = ModelGeneratorHelper.PatchPost();
            Response = await m_jsonPlaceHolderService.PostsControllerProxy.PatchPostsAsync("1", patchModel);
        }

        [When(@"a non-authenticated user sends a request to patch a nonexistent post")]
        public async Task WhenANon_AuthenticatedUserSendsARequestToPatchANonexistentPost()
        {
            Post = ModelGeneratorHelper.PatchedNonExistentPost();
            PatchPostModel patchModel = ModelGeneratorHelper.PatchPost();
            Response = await m_jsonPlaceHolderService.PostsControllerProxy.PatchPostsAsync("101", patchModel);
        }

        [When(@"a non-authenticated user sends a request to update a nonexistent post")]
        public async Task WhenANon_AuthenticatedUserSendsARequestToUpdateANonexistentPost()
        {
            Post = ModelGeneratorHelper.UpdatePost();
            Response = await m_jsonPlaceHolderService.PostsControllerProxy.UpdatePostsAsync("101", Post);
        }

        [When(@"a non-authenticated user sends a request to get a nonexistent post")]
        public async Task WhenANon_AuthenticatedUserSendsARequestToGetANonexistentPost()
        {
            Post = ModelGeneratorHelper.GetPost();
            Response = await m_jsonPlaceHolderService.PostsControllerProxy.GetPostsAsync("101");
        }

        [When(@"a non-authenticated user sends a request to delete a nonexistent post")]
        public async Task WhenANon_AuthenticatedUserSendsARequestToDeleteANonexistentPost()
        {
            Post = ModelGeneratorHelper.GetEmptyPost();
            Response = await m_jsonPlaceHolderService.PostsControllerProxy.DeletePostsAsync("101");
        }
            
        [When(@"the user sends a GET request to /posts")]
        public async Task WhenTheUserSendsAGETRequestToPosts()
        {
            Response = await m_jsonPlaceHolderService.PostsControllerProxy.GetAllPostsAsync();
        }

        [Then(@"the response status code should be 200")]
        public void ThenTheResponseStatusCodeShouldBe200()
        {
            Response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Then(@"the response should contain a list of posts with valid structure")]
        public async Task ThenTheResponseShouldContainAListOfPostsWithValidStructure()
        {
            var posts = await Response.Content.ReadAsAsync<List<PostModel>>();

            posts.Should().NotBeNullOrEmpty();
            posts.Should().OnlyContain(p =>
                p.id > 0 &&
                p.userId > 0 &&
                !string.IsNullOrWhiteSpace(p.title) &&
                !string.IsNullOrWhiteSpace(p.body));
        }

        [Then(@"the service's response is ""(.*)""")]
        public void ThenTheServiceSResponseIs(HttpStatusCode httpStatusCode)
        {
            Response.StatusCode.Should().Be(httpStatusCode);
        }

        [Then(@"the service returns the (newly created|updated|specific|empty|patched) post")]
        public async Task ThenTheServiceReturnsTheNewlyCreatedPost(string option)
        {
            await ValidateResponseContent();
        }

        private async Task ValidateResponseContent()
        {
            PostModel postModel = await Response.Content.ReadAsAsync<PostModel>();
            postModel.Should().BeEquivalentTo(Post);
        }
    }
}