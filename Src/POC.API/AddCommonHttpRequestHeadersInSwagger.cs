namespace POC.API
{
    using Swashbuckle.AspNetCore.Swagger;
    using Swashbuckle.AspNetCore.SwaggerGen;

    /// <summary>
    /// Add header values for all the APIs for generated Swagger out put file
    /// </summary>
    public class AddCommonHttpRequestHeadersInSwagger : IOperationFilter
    {
        public void Apply(Operation operation, OperationFilterContext context)
        {

        ////You can also put in controller
        //[FromHeader(Name = "header-name-xxx")]
        //public string HeaderNameXxx { get; set; }

        operation.Parameters.Add(new NonBodyParameter
            {
                Name = "caller-timestamp",
                In = "header",
                Type = "string",
                //Type = "string($date-time)",
                Required = false,
                Description = "The timestamp when caller submits the request.",
                Format = "date-time"
            });

            operation.Parameters.Add(new NonBodyParameter
            {
                Name = "api-key",
                In = "header",
                Type = "string",
                Required = true,
                Description = "api key"
            });


  

        }
    }
}