using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.ConfigureSwaggerGen(options => {
	
	//this is what enables swagger file upload
	options.OperationFilter<FileUploadOperation>();
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();


//this is what enables swagger file upload
public class FileUploadOperation : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
		if (operation.Parameters == null)
		{
			return;
		}

		// REF: https://github.com/domaindrivendev/Swashbuckle.AspNetCore/issues/412
		// REF: https://github.com/domaindrivendev/Swashbuckle.AspNetCore/pull/413
		foreach (var parameter in operation.Parameters)
		{
			var description = context.ApiDescription.ParameterDescriptions.First(p => p.Name == parameter.Name);
			var routeInfo = description.RouteInfo;

			if (string.IsNullOrEmpty(parameter.Name))
			{
				parameter.Name = description.ModelMetadata?.Name;
			}

			if (parameter.Description == null)
			{
				parameter.Description = description.ModelMetadata?.Description;
			}

			if (routeInfo == null)
			{
				continue;
			}

			parameter.Required |= !routeInfo.IsOptional;
		}

		// Overwrite description for shared response code
		//operation.Responses["400"].Description = "Invalid query parameter(s). Read the response description";
		//operation.Responses["401"].Description = "Authorization has been denied for this request";
	}
}
