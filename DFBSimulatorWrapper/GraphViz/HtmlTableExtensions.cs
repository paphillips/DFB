using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI;
using System.Web.UI.HtmlControls;

namespace DFBSimulatorWrapper.GraphViz
{
	public static class HtmlTableExtensions
	{
		/// <summary>
		/// Comverts the HtmlTable control to a GraphViz escString format
		/// </summary>
		/// <param name="instance"></param>
		/// <returns></returns>
		public static lblString ToGraphvizLabelFormat(this HtmlTable instance)
		{
			if (instance == null)  { return null; }

			var sb = new StringBuilder();
			var tw = new StringWriter(sb);
			var hw = new HtmlTextWriter(tw, "");
			instance.RenderControl(hw);

			return new lblString(sb.Replace("\r\n", "").ToString());
		}
	}
}
