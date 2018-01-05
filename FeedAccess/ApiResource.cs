using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace FeedAccess
{
    public class ApiResource
    {
        public Config Config { get; set; }
        public virtual string ApiRoot { get { return Config.ApiUrl; } }
        public virtual string Username { get { return Config.ApiUsername; } }
        public virtual string Password { get { return Config.ApiPassword; } }

        public ApiResource(Config config)
        {
            Config = config;
        }

        public async Task<TResult> GetAsync<TResult>(string routeTemplate, params object[] queryParams)
        {
            var route = CreateRoute(routeTemplate, queryParams);
            using (var client = CreateClient())
            {
                var response = await client.GetAsync(route);
                if (!response.IsSuccessStatusCode)
                    throw new InvalidOperationException(response.ToString());

                return await ReadResponse<TResult>(response);
            }
        }

        protected virtual HttpClient CreateClient()
        {
            var client = new HttpClient();
            client.BaseAddress = new Uri(ApiRoot);
            client.DefaultRequestHeaders.Accept.Clear();
            client.Timeout = TimeSpan.FromMinutes(5);
            AddHeaders(client.DefaultRequestHeaders);

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(ASCIIEncoding.ASCII.GetBytes(
                            string.Format("{0}:{1}", Username, Password))));

            return client;
        }

        protected virtual string CreateRoute(string routeTemplate, params object[] queryParams)
        {
            queryParams = queryParams
                .Select(qp => Uri.EscapeUriString((qp ?? string.Empty).ToString()))
                .ToArray();

            return string.Format(routeTemplate, queryParams);
        }

        protected virtual void AddHeaders(HttpRequestHeaders headers)
        {
            headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        /// <summary>
        /// Gets the standard <see cref="MediaTypeFormatter"/>s in use by the API.
        /// </summary>
        /// <returns></returns>
        /// <remarks>Legacy implementation forced inheritance - BAD!</remarks>
        protected virtual IEnumerable<MediaTypeFormatter> GetFormatters()
        {
            var jsonFormatter = new JsonMediaTypeFormatter();
            return new MediaTypeFormatter[] { jsonFormatter };
        }

        /// <summary>
        /// Reads an HttpResponseMessage asynchronously. If the response could not be read, an ApiException is thrown.
        /// </summary>
        /// <typeparam name="TResult">The Type of data contained in response.</typeparam>
        /// <param name="response">The response of an API request.</param>
        /// <returns>The content of the response asynchronously.</returns>
        protected virtual async Task<TResult> ReadResponse<TResult>(HttpResponseMessage response)
        {
            return await response.Content.ReadAsAsync<TResult>(GetFormatters());
        }
    }
}