using System;
using System.Net.Http;
using System.Net.Mime;
using System.Threading.Tasks;
using System.Transactions;
using Microsoft.AspNetCore.TestHost;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Morcatko.AspNetCore.JsonMergePatch;
using Newtonsoft.Json;

//Note: Install-Package Microsoft.AspNetCore.TestHost -Version 2.1.1
//install nuget (tick include prerelease) Morcatko.AspNetCore.JsonMergePatch  

namespace POC.API.Tests.IntegrationTestsV2
{
    /// <summary>
    /// Base API Test class.
    /// </summary>
    public abstract class BaseApiTest
    {
        /// <summary>
        /// Gets or sets the TestContext.
        /// </summary>
        public TestContext TestContext { get; set; }

        /// <summary>
        /// Gets or sts the TestServer.
        /// </summary>
        protected static TestServer TestServer { get; set; }

        /// <summary>
        /// Invoke Get on the URL.
        /// </summary>
        /// <param name="path">The URL.</param>
        /// <param name="onBeforeSend">Actions to pefrform on request before sending.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public virtual async Task<HttpResponseMessage> Get(string path, Action<RequestBuilder> onBeforeSend = null)
        {
            var httpRequest = this.CreateRequest(path);
            if (onBeforeSend != null)
            {
                onBeforeSend.Invoke(httpRequest);
            }

            var httpResponse = await httpRequest.GetAsync().ConfigureAwait(false);
            return await httpResponse.CheckServerError(this.TestContext);
        }

        /// <summary>
        /// Invoke Get on the URL.
        /// </summary>
        /// <param name="path">The URL.</param>
        /// <param name="onBeforeSend">Actions to pefrform on request before sending.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public virtual async Task<HttpResponseMessage> GetAll(string path, Action<RequestBuilder> onBeforeSend = null)
        {
            var httpRequest = this.CreateRequest(path);

            if (onBeforeSend != null)
            {
                onBeforeSend.Invoke(httpRequest);
            }

            var httpResponse = await httpRequest.GetAsync().ConfigureAwait(false);
            return await httpResponse.CheckServerError(this.TestContext);
        }

        /// <summary>
        /// Invoke post on the URL.
        /// </summary>
        /// <param name="path">the URL.</param>
        /// <param name="body">Object to post.</param>
        /// <param name="onBeforeSend">Actions to pefrform on request before sending.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public virtual async Task<HttpResponseMessage> Post(string path, object body, Action<RequestBuilder> onBeforeSend = null)
        {
            var httpRequest = this.CreateRequest(path, body);
            if (onBeforeSend != null)
            {
                onBeforeSend.Invoke(httpRequest);
            }

            // Check if we are in a transaction to avoid "transaction aborted" errors
            var inTransaction = Transaction.Current != null;
            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                var httpResponse = await httpRequest.PostAsync().ConfigureAwait(false);

                if (inTransaction)
                {
                    scope.Complete();
                }

                return await httpResponse.CheckServerError(this.TestContext);
            }
        }

        /// <summary>
        /// Invoke patch on the URL.
        /// </summary>
        /// <param name="path">the URL.</param>
        /// <param name="body">Object to post.</param>
        /// <param name="onBeforeSend">Actions to pefrform on request before sending.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public virtual async Task<HttpResponseMessage> Patch(string path, object body, Action<RequestBuilder> onBeforeSend = null)
        {
            var httpRequest = this.CreatePatchRequest(path, body);

            if (onBeforeSend != null)
            {
                onBeforeSend.Invoke(httpRequest);
            }

            // Check if we are in a transaction to avoid "transaction aborted" errors
            var inTransaction = Transaction.Current != null;
            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                var httpResponse = await httpRequest.SendAsync("PATCH").ConfigureAwait(false);

                if (inTransaction)
                {
                    scope.Complete();
                }

                return await httpResponse.CheckServerError(this.TestContext);
            }
        }

        /// <summary>
        /// Invoke delete on the URL.
        /// </summary>
        /// <param name="path">url</param>
        /// <param name="onBeforeSend">Actions to pefrform on request before sending.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public virtual async Task<HttpResponseMessage> Delete(string path, Action<RequestBuilder> onBeforeSend = null)
        {
            var httpRequest = this.CreateRequest(path);

            if (onBeforeSend != null)
            {
                onBeforeSend.Invoke(httpRequest);
            }

            // Check if we are in a transaction to avoid "transaction aborted" errors
            var inTransaction = Transaction.Current != null;
            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                var httpResponse = await httpRequest.SendAsync("DELETE").ConfigureAwait(false);
                if (inTransaction)
                {
                    scope.Complete();
                }

                return await httpResponse.CheckServerError(this.TestContext);
            }
        }

        /// <summary>
        /// Create the http request for 'application/merge-patch+json' with SetDefaultHeaders();
        /// </summary>
        /// <param name="path">The path.</param>
        /// <param name="body">The body.</param>
        /// <returns>The Http Request.</returns>
        protected virtual RequestBuilder CreatePatchRequest(string path, object body)
        {
            var httpRequest = TestServer.CreateRequest(path);

            var content = new StringContent(JsonConvert.SerializeObject(body), System.Text.Encoding.UTF8, JsonMergePatchDocument.ContentType);
            httpRequest.And(r => r.Content = content);
            this.SetDefaultHeaders(httpRequest);

            return httpRequest;
        }

        /// <summary>
        /// Create the http request with SetDefaultHeaders();
        /// </summary>
        /// <param name="path">The path.</param>
        /// <param name="body">The body.</param>
        /// <returns>The Http Request.</returns>
        protected virtual RequestBuilder CreateRequest(string path, object body = null)
        {
            var httpRequest = TestServer.CreateRequest(path);

            if (body == null)
            {
                httpRequest.AddHeader("Accept", MediaTypeNames.Application.Json);
            }
            else
            {
                var content = new StringContent(JsonConvert.SerializeObject(body), System.Text.Encoding.UTF8, MediaTypeNames.Application.Json);
                httpRequest.And(r => r.Content = content);
            }

            this.SetDefaultHeaders(httpRequest);

            return httpRequest;
        }

        /// <summary>
        /// Set the headers for the created request.
        /// </summary>
        /// <param name="httpRequest">The http request.</param>
        protected virtual void SetDefaultHeaders(RequestBuilder httpRequest)
        {
            httpRequest.AddHeader(ApiParametersConstant.ApiToken, "123");
        }
    }
}
