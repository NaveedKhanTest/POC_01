namespace System.Net.Http
{
    using System.Collections.Generic;
    using System.Net.Mime;
    using System.Threading.Tasks;
    using Newtonsoft.Json;
    /// <summary>
    /// Extension methods for <see cref="HttpResponseMessage"/>.
    /// </summary>
    public static class HttpResponseMessageExtensions
    {
        /// <summary>
        /// Convert httpResponse to ApiResponseMessage.
        /// </summary>
        /// <typeparam name="T">Type of value expected.</typeparam>
        /// <param name="httpResponseTask">the httpResponseTask.</param>
        /// <param name="dispose">True to dispose of the httpResponse.</param>
        /// <remarks>Will dispose of the httpResponse, if you want this get it before hand</remarks>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public static async Task<T> As<T>(this Task<HttpResponseMessage> httpResponseTask, bool dispose = true)
            where T : class
        {
            var httpResponse = await httpResponseTask;
            var value = (T)null;
            if (httpResponse.Content.Headers.ContentType.MediaType == MediaTypeNames.Application.Json)
            {
                var json = await httpResponse.Content.ReadAsStringAsync();
                value = JsonConvert.DeserializeObject<T>(json);
            }
            else
            {
                throw new Exception($"Expected media type of {MediaTypeNames.Application.Json} recevied {httpResponse.Content.Headers.ContentType.MediaType}");
            }

            if (dispose)
            {
                httpResponse.Dispose();
            }

            return value;
        }

        /// <summary>
        /// Convert httpResponse to ApiResponseMessage.
        /// </summary>
        /// <typeparam name="T">Type of value expected.</typeparam>
        /// <param name="httpResponseTask">the httpResponseTask.</param>
        /// <param name="dispose">True to dispose of the httpResponse.</param>
        /// <remarks>Will dispose of the httpResponse, if you want this get it before hand</remarks>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public static async Task<List<T>> AsListOf<T>(this Task<HttpResponseMessage> httpResponseTask, bool dispose = true)
            where T : class
        {
            var httpResponse = await httpResponseTask;
            var value = (List<T>)null;

            if (httpResponse.Content.Headers.ContentType.MediaType == MediaTypeNames.Application.Json)
            {
                var json = await httpResponse.Content.ReadAsStringAsync();
                value = JsonConvert.DeserializeObject<List<T>>(json);
            }
            else
            {
                throw new System.Exception($"Expected media type of {MediaTypeNames.Application.Json} recevied {httpResponse.Content.Headers.ContentType.MediaType}");
            }

            if (dispose)
            {
                httpResponse.Dispose();
            }

            return value;
        }
    }
}
