using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Web;
using KEWIStudents.NAVWS;

namespace KEWIStudents
{
	public class Components
	{
		public static Studentportal ObjNav
		{
			get
			{
				var ws = new Studentportal();
				try
				{
					var credentials = new NetworkCredential(ConfigurationManager.AppSettings["W_USER"], ConfigurationManager.AppSettings["W_PWD"]);
					ws.Credentials = credentials;
					ws.PreAuthenticate = true;
				}
				catch(Exception ex)
				{
					throw ex;
				}
				return ws;
			}
		}
	}
}