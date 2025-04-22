using RestApiTesting.ApiAutomationChallenge.Helpers;

namespace RestApiTesting.ApiAutomationChallenge.Services.Impl
{
    public class JsonPlaceHolderService: IJsonPlaceHolderService
    {
        public PostsControllerProxy PostsControllerProxy { get; set; }

        public JsonPlaceHolderService()
        {
            PostsControllerProxy = new PostsControllerProxy(ConfigurationHelper.TestApiUrl);
        }
    }
}
