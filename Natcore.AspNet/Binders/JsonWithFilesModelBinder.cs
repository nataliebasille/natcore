using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text.Json;
using System.Threading.Tasks;

namespace Natcore.AspNet.Binders
{
	public class JsonWithFilesModelBinder : IModelBinder
	{
		private readonly IOptions<JsonOptions> _jsonOptions;
		private readonly FormFileModelBinder _formFileModelBinder;

		public JsonWithFilesModelBinder(IOptions<JsonOptions> jsonOptions, ILoggerFactory loggerFactory)
		{
			_jsonOptions = jsonOptions;
			_formFileModelBinder = new FormFileModelBinder(loggerFactory);
		}

		public async Task BindModelAsync(ModelBindingContext bindingContext)
		{
			if (bindingContext == null)
				throw new ArgumentNullException(nameof(bindingContext));

			object model = null;
			ValueProviderResult result = bindingContext.ValueProvider.GetValue(bindingContext.FieldName);

			if (result != ValueProviderResult.None)
				model = Convert(bindingContext.ModelType, result.FirstValue);

			if (!bindingContext.ModelType.IsPrimitive && bindingContext.ModelType != typeof(string))
			{
				foreach (var property in bindingContext.ModelMetadata.Properties)
				{
					object propertyValue = property.PropertyGetter(model);

					if (propertyValue == null)
					{
						var fieldName = property.BinderModelName ?? property.PropertyName;
						var modelName = fieldName;

						ModelBindingResult propertyResult;
						using (bindingContext.EnterNestedScope(property, fieldName, modelName, propertyValue))
						{
							if (property.ModelType == typeof(IFormFile))
							{
								await _formFileModelBinder.BindModelAsync(bindingContext);
								propertyResult = bindingContext.Result;
							}
							else
							{
								await BindModelAsync(bindingContext);
								propertyResult = bindingContext.Result;
							}
						}

						if (propertyResult.IsModelSet)
						{
							if (model == null)
								model = Activator.CreateInstance(bindingContext.ModelType);

							property.PropertySetter(model, propertyResult.Model);
						}
					}
				}
			}

			bindingContext.Result = model != null ? ModelBindingResult.Success(model) : ModelBindingResult.Failed();
		}

		private object Convert(Type modelType, string value)
		{
			object convertedValue;

			if (modelType == typeof(string))
				convertedValue = value;
			else if (modelType.IsClass || typeof(IEnumerable<object>).IsAssignableFrom(modelType))
				convertedValue = JsonSerializer.Deserialize(value, modelType, _jsonOptions.Value.JsonSerializerOptions);
			else
				convertedValue = TypeDescriptor.GetConverter(modelType)?.ConvertFromString(value);

			return convertedValue;
		}
	}
}
