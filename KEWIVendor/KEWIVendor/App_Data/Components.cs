using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Web;
using KEWIVendor.NAVWS;

namespace KEWIVendor
{
	public class Components
	{
		public static Procurement ObjNav
		{
			get
			{
				var ws = new Procurement();
				try
				{
					var credentials = new NetworkCredential(ConfigurationManager.AppSettings["W_USER"], ConfigurationManager.AppSettings["W_PWD"]);
					ws.Credentials = credentials;
                    ws.PreAuthenticate = true;
                }
				catch(Exception ex)
                {
					ex.Data.Clear();
                }
                return ws;
			}
		}
	}
}