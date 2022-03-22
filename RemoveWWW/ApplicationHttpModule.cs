using System;
using System.Text.RegularExpressions;
using System.Web;

namespace RemoveWWW
	{

	public class ApplicationHttpModule : IHttpModule
	{
		
		public static string ModuleName
		{
			get
			{
				return "ApplicationHttpModule";
			}
		}
		// In the Init function, register for HttpApplication
		// events by adding your handlers.
		public void Init(HttpApplication application)
		{
			application.BeginRequest += Application_BeginRequest;
		}
		private void Application_BeginRequest(object source, EventArgs e)
		{
			// Create HttpApplication and HttpContext objects to access
			// request and response properties.
			HttpApplication application = (HttpApplication) source;
			HttpContext context = application.Context;
			string url = context.Request.Url.ToString();
			if (url.Contains("://www."))
			{
				RemoveWww(context);
			}
		}
		
		private static readonly Regex _Regex = new Regex("(http|https)://www\\.", (System.Text.RegularExpressions.RegexOptions) (RegexOptions.IgnoreCase | RegexOptions.Compiled));
		/// <summary>
		/// Removes the www subdomain from the request and redirects.
		/// </summary>
		private static void RemoveWww(HttpContext context)
		{
			string url = context.Request.Url.ToString();
			
			if (context.Request.RawUrl.Contains("404.aspx?404;"))
			{
				url = System.Convert.ToString(context.Request.RawUrl.Replace("/wpm/404.aspx?404;", string.Empty));
			}
			else
			{
				url = context.Request.Url.ToString();
			}
			if (_Regex.IsMatch(url))
			{
				url = _Regex.Replace(url, "$1://");
				PermanentRedirect(url, context);
			}
		}
		
		/// <summary>
		/// Sends permanent redirection headers (301)
		/// </summary>
		private static void PermanentRedirect(string url, HttpContext context)
		{
			if (url.EndsWith("default.aspx", StringComparison.OrdinalIgnoreCase))
			{
				url = System.Convert.ToString((url.ToLower()).Replace("default.aspx", string.Empty));
			}
			context.Response.Clear();
			context.Response.StatusCode = 301;
			context.Response.AppendHeader("location", url);
			context.Response.End();
		}
		public void Dispose()
		{
		}
	}
	
}
