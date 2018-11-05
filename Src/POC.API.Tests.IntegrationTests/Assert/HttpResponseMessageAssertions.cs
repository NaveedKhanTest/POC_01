using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace System.Net.Http 
{
    /// <summary>
    /// Extension methods for Asserting <see cref="HttpResponseMessage"/>.
    /// </summary>
    public static class HttpResponseMessageAssertions
    {
        /// <summary>
        /// Check if we had an internal server error.
        /// </summary>
        /// <param name="httpResponse">The http response.</param>
        /// <param name="testContext">The test context.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public static async Task<HttpResponseMessage> CheckServerError(this HttpResponseMessage httpResponse, TestContext testContext)
        {
            if (httpResponse.StatusCode == HttpStatusCode.InternalServerError)
            {
                var content = await httpResponse.Content.ReadAsStringAsync();
                testContext.WriteLine(content);
                throw new Exception($"Internal Server Error: {content}");
            }

            return httpResponse;
        }

        /// <summary>
        /// Assert the expected status code.
        /// </summary>
        /// <param name="httpResponseTask">The httpResponseTask.</param>
        /// <param name="httpStatusCode">The expected status code.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public static async Task<HttpResponseMessage> Expect(this Task<HttpResponseMessage> httpResponseTask, int httpStatusCode)
        {
            var httpResponse = await httpResponseTask;
            Assert.AreEqual(httpStatusCode, (int)httpResponse.StatusCode, "Incorrect status code");
            return httpResponse;
        }

        /// <summary>
        /// Assert the expected status code.
        /// </summary>
        /// <param name="httpResponseTask">The httpResponseTask.</param>
        /// <param name="httpStatusCode">The expected status code.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public static async Task<HttpResponseMessage> Expect(this Task<HttpResponseMessage> httpResponseTask, HttpStatusCode httpStatusCode)
        {
            var httpResponse = await httpResponseTask;
            Assert.AreEqual(httpStatusCode, httpResponse.StatusCode, "Incorrect status code");
            return httpResponse;
        }
    }
}
