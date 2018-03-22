using DFBSimulatorWrapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DFBProject;
using System.Configuration;

namespace DFBPluginModel
{
    public interface IPlugin
	{
		/// <summary>
		/// Name of plugin
		/// </summary>
		string Name { get; }

		/// <summary>
		/// Application context
		/// </summary>
		PluginContext PluginContext { get; }

		/// <summary>
		/// Menu to be added to parent form by plugin
		/// </summary>
		ToolStripMenuItem Menu { get; }

		/// <summary>
		/// Called by application when the context changes
		/// </summary>
		/// <param name="pluginContext"></param>
		void ContextChanged(PluginContext pluginContext);

		/// <summary>
		/// Called before simulation runs
		/// </summary>
		void PreSimulate();

		/// <summary>
		/// Called after simulation is complete
		/// </summary>
		void PostSimulate();
	}
}
