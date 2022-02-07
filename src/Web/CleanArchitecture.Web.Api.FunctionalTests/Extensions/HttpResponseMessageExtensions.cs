using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace CleanArchitecture.Web.Api.FunctionalTests.Extensions
{
    public static class HttpResponseMessageExtensions
    {
        public static async Task<TModel> ResolveAsync<TModel>(this HttpResponseMessage msg)
        {
            var stringResponse = await msg.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<TModel>(stringResponse);
        }
    }
}
