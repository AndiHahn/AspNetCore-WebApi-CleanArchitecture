using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace CleanArchitecture.FunctionalTests.Helper
{
    public static class ApiResultHelper
    {
        public static async Task<TModel> GetModelFromResponseAsync<TModel>(HttpResponseMessage msg)
        {
            var stringResponse = await msg.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<TModel>(stringResponse);
        }
    }
}
