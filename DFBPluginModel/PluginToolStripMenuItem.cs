using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DFBProject;
using DFBSimulatorWrapper;

namespace DFBPluginModel
{
	public class PluginToolStripMenuItem : ToolStripMenuItem
	{
		IPlugin plugInOwner;
		IPlugin PluginOwner => plugInOwner;
		
		public PluginToolStripMenuItem(IPlugin plugInOwner)
		{
			this.plugInOwner = plugInOwner;
		}
	}
}
