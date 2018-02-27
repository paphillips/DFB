using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DFBSimulatorWrapper.GraphViz
{
	public static class AppendNameValueExtensions
	{
		public static void AppendNameValue(this escString instance, StringBuilder sb, string variableName)
		{
			if(instance == null || sb == null || string.IsNullOrEmpty(variableName)) { return; }

			AppendNameValueHtml(sb, variableName, instance.Value);
		}
		public static void AppendNameValue(this double? instance, StringBuilder sb, string variableName)
		{
			if (instance == null || sb == null || string.IsNullOrEmpty(variableName)) { return; }

			AppendNameValueStd(sb, variableName, instance.Value);
		}
		public static void AppendNameValue(this KnownColor? instance, StringBuilder sb, string variableName)
		{
			if (instance == null || sb == null || string.IsNullOrEmpty(variableName)) { return; }

			AppendNameValueStd(sb, variableName, Enum.GetName(typeof(KnownColor), instance));
		}
		public static void AppendNameValue(this string instance, StringBuilder sb, string variableName)
		{
			if (instance == null || sb == null || string.IsNullOrEmpty(variableName)) { return; }

			AppendNameValueStd(sb, variableName, instance);
		}
		public static void AppendNameValue(this bool? instance, StringBuilder sb, string variableName)
		{
			if (instance == null || sb == null || string.IsNullOrEmpty(variableName)) { return; }

			AppendNameValueStd(sb, variableName, instance);
		}
		public static void AppendNameValue(this int? instance, StringBuilder sb, string variableName)
		{
			if (instance == null || sb == null || string.IsNullOrEmpty(variableName)) { return; }

			AppendNameValueStd(sb, variableName, instance);

		}
		public static void AppendNameValue(this Point instance, StringBuilder sb, string variableName)
		{
			if (instance == null || sb == null || string.IsNullOrEmpty(variableName)) { return; }

			AppendNameValueStd(sb, variableName, instance);
		}
		public static void AppendNameValue(this Rect instance, StringBuilder sb, string variableName)
		{
			if (instance == null || sb == null || string.IsNullOrEmpty(variableName)) { return; }

			AppendNameValueStd(sb, variableName, instance);
		}
		public static void AppendNameValue(this Shape? instance, StringBuilder sb, string variableName)
		{
			if (instance == null || sb == null || string.IsNullOrEmpty(variableName)) { return; }

			AppendNameValueStd(sb, variableName, instance);
		}
		public static void AppendNameValue(this Style.Node? instance, StringBuilder sb, string variableName)
		{
			if (instance == null || sb == null || string.IsNullOrEmpty(variableName)) { return; }

			AppendNameValueStd(sb, variableName, instance);
		}
		public static void AppendNameValue(this Style.Edge? instance, StringBuilder sb, string variableName)
		{
			if (instance == null || sb == null || string.IsNullOrEmpty(variableName)) { return; }

			AppendNameValueStd(sb, variableName, instance);
		}
		public static void AppendNameValue(this Style.Cluster? instance, StringBuilder sb, string variableName)
		{
			if (instance == null || sb == null || string.IsNullOrEmpty(variableName)) { return; }

			AppendNameValueStd(sb, variableName, instance);
		}
		public static void AppendNameValue(this PointList instance, StringBuilder sb, string variableName)
		{
			if (instance == null || sb == null || string.IsNullOrEmpty(variableName)) { return; }

			AppendNameValueStd(sb, variableName, instance);
		}
		public static void AppendNameValue(this lblString instance, StringBuilder sb, string variableName)
		{
			if (instance == null || sb == null || string.IsNullOrEmpty(variableName)) { return; }

			AppendNameValueHtml(sb, variableName, instance.Value);
		}
		public static void AppendNameValue(this layerRange instance, StringBuilder sb, string variableName)
		{
			if (instance == null || sb == null || string.IsNullOrEmpty(variableName)) { return; }

			AppendNameValueStd(sb, variableName, instance);
		}
		public static void AppendNameValue(this OutputMode? instance, StringBuilder sb, string variableName)
		{
			if (instance == null || sb == null || string.IsNullOrEmpty(variableName)) { return; }

			AppendNameValueStd(sb, variableName, instance);
		}
		public static void AppendNameValue(this PackMode? instance, StringBuilder sb, string variableName)
		{
			if (instance == null || sb == null || string.IsNullOrEmpty(variableName)) { return; }

			AppendNameValueStd(sb, variableName, instance);
		}
		public static void AppendNameValue(this Pagedir? instance, StringBuilder sb, string variableName)
		{
			if (instance == null || sb == null || string.IsNullOrEmpty(variableName)) { return; }

			AppendNameValueStd(sb, variableName, instance);
		}
		public static void AppendNameValue(this QuadType? instance, StringBuilder sb, string variableName)
		{
			if (instance == null || sb == null || string.IsNullOrEmpty(variableName)) { return; }

			AppendNameValueStd(sb, variableName, instance);
		}
		public static void AppendNameValue(this Rankdir? instance, StringBuilder sb, string variableName)
		{
			if (instance == null || sb == null || string.IsNullOrEmpty(variableName)) { return; }

			AppendNameValueStd(sb, variableName, instance);
		}
		public static void AppendNameValue(this Adddouble instance, StringBuilder sb, string variableName)
		{
			if (instance == null || sb == null || string.IsNullOrEmpty(variableName)) { return; }

			AppendNameValueStd(sb, variableName, instance);
		}
		public static void AppendNameValue(this SmoothType? instance, StringBuilder sb, string variableName)
		{
			if (instance == null || sb == null || string.IsNullOrEmpty(variableName)) { return; }

			AppendNameValueStd(sb, variableName, instance);
		}
		public static void AppendNameValue(this Splines? instance, StringBuilder sb, string variableName)
		{
			if (instance == null || sb == null || string.IsNullOrEmpty(variableName)) { return; }

			AppendNameValueStd(sb, variableName, instance);
		}
		public static void AppendNameValue(this StartType instance, StringBuilder sb, string variableName)
		{
			if (instance == null || sb == null || string.IsNullOrEmpty(variableName)) { return; }

			AppendNameValueStd(sb, variableName, instance);
		}
		public static void AppendNameValue(this Style instance, StringBuilder sb, string variableName)
		{
			if (instance == null || sb == null || string.IsNullOrEmpty(variableName)) { return; }

			AppendNameValueStd(sb, variableName, instance);
		}
		public static void AppendNameValue(this ViewPort instance, StringBuilder sb, string variableName)
		{
			if (instance == null || sb == null || string.IsNullOrEmpty(variableName)) { return; }

			AppendNameValueStd(sb, variableName, instance);
		}
		public static void AppendNameValue(this ClusterMode? instance, StringBuilder sb, string variableName)
		{
			if (instance == null || sb == null || string.IsNullOrEmpty(variableName)) { return; }

			AppendNameValueStd(sb, variableName, instance);
		}
		public static void AppendNameValue(this ArrowType? instance, StringBuilder sb, string variableName)
		{
			if (instance == null || sb == null || string.IsNullOrEmpty(variableName)) { return; }

			AppendNameValueStd(sb, variableName, instance);
		}
		public static void AppendNameValue(this DirType? instance, StringBuilder sb, string variableName)
		{
			if (instance == null || sb == null || string.IsNullOrEmpty(variableName)) { return; }

			AppendNameValueStd(sb, variableName, instance);
		}
		public static void AppendNameValue(this PortPos? instance, StringBuilder sb, string variableName)
		{
			if (instance == null || sb == null || string.IsNullOrEmpty(variableName)) { return; }

			AppendNameValueStd(sb, variableName, instance);
		}
		
		#region Shared Handlers

		/// <summary>
		/// Standard handling
		/// </summary>
		/// <param name="sb"></param>
		/// <param name="name"></param>
		/// <param name="value"></param>
		private static void AppendNameValueStd(StringBuilder sb, string name, object value)
		{
			sb.AppendFormat(" {0} = \"{1}\"", name, value);
		}

		public static void AppendNameValueHtml(StringBuilder sb, string variableName, string value)
		{
			if (value.StartsWith("<"))
			{
				// HTML label
				sb.AppendFormat(" {0} = <{1}>", variableName, value);
			}
			else
			{
				// Plain label
				sb.AppendFormat(" {0} = \"{1}\"", variableName, value);
			}
		}
		#endregion
	}
}
