using LivrariaWEB.Infra;
using LivrariaWEB.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace LivrariaWEB.Controllers
{
	public class BaseController : Controller
	{
		public void Success(string message, bool dismissable = false)
		{
			AddAlert(AlertStyles.Success, message, dismissable);
		}

		public void Information(string message, bool dismissable = false)
		{
			AddAlert(AlertStyles.Information, message, dismissable);
		}

		public void Warning(string message, bool dismissable = false)
		{
			AddAlert(AlertStyles.Warning, message, dismissable);
		}

		public void Danger(string message, bool dismissable = false)
		{
			AddAlert(AlertStyles.Danger, message, dismissable);
		}

		private void AddAlert(string alertStyle, string message, bool dismissable)
		{
			var alerta = new Alert
			{
				AlertStyle = alertStyle,
				Message = message,
				Dismissable = dismissable
			};

			TempData.Put(Alert.TempDataKey, alerta);
		}
	}
}
