using System;
using System.Net.Http;
using System.Web.Http;

namespace mongo_todo
{
	public static class ApiControllerExtension
	{
		public static string GetLinkHeader<T>(
			this ApiController controller,
			HttpRequestMessage request,
			HttpResponseMessage response,
			string id,
			string relativeLocation)
		{
			return GetLinkHeader<T>(
								    controller, request, response, id, relativeLocation, string.Empty);
		}

		public static string GetLinkHeader<T>(
			this ApiController controller,
			HttpRequestMessage request,
			HttpResponseMessage response,
			string relativeLocation)
		{
			return GetLinkHeader<T>(
								    controller,
									request,
									response,
									string.Empty,
									relativeLocation,
									string.Empty);
		}

		public static string GetLinkHeader<T>(
			this ApiController controller,
			HttpRequestMessage request,
			HttpResponseMessage response,
			string id,
			string relativeLocation,
			string description)
		{
			string currentUri = controller.Request.RequestUri.AbsoluteUri;
			if (!string.IsNullOrEmpty(id))
				currentUri = string.Concat(currentUri, "/", id);

			var uri = new Uri(string.Concat(currentUri, relativeLocation));
			string rel = typeof(T).Name;

			string type = string.Empty;
			if (response != null
				&& response.Content != null
				&& response.Content.Headers != null
				&& response.Content.Headers.ContentType != null)
				type = response.Content.Headers.ContentType.MediaType;

			string location = string.Format("<{0}>", uri);
			if (!string.IsNullOrWhiteSpace(rel))
				location = string.Concat(location, "; rel=", rel);

			if (!string.IsNullOrWhiteSpace(type))
				location = string.Concat(location, "; type=\"", type, "\"");

			if (!string.IsNullOrWhiteSpace(description))
				location = string.Concat(location, "; title=\"", description, "\"");

			return location;
		}
	}
}