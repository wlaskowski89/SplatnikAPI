﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Splatnik.Contracts.V1.Responses;
using System.Linq;
using System.Threading.Tasks;

namespace Splatnik.API.Filters
{
	public class ValidationFilter : IAsyncActionFilter
	{
		public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
		{
			if (!context.ModelState.IsValid)
			{
				var errorsInModelStae = context.ModelState
					.Where(x => x.Value.Errors.Count > 0)
					.ToDictionary(kvp => kvp.Key, kvp => kvp.Value.Errors.Select(x => x.ErrorMessage)).ToArray();

				var errorResponse = new ErrorResponse();

				foreach (var error in errorsInModelStae)
				{
					foreach (var subError in error.Value)
					{
						var errorModel = new ErrorModel
						{
							FieldName = error.Key,
							Message = subError
						};

						errorResponse.Errors.Add(errorModel);
					}
				}

				context.Result = new BadRequestObjectResult(errorResponse);
				return;
			}

			await next();
		}
	}
}
