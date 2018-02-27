using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CyDesigner.Extensions.Common;
using CyDesigner.Extensions.Gde;

namespace DFBSimulatorWrapper
{
	public class CyInstQuery : CyDesigner.Extensions.Gde.ICyInstQuery_v1
	{
		public string ComponentName => "";

		public string SymbolFileName => "";

		public string InstanceName => "";

		public string InstanceIdPath => "";

		public string CurrentDevice => "";

		public bool IsPreviewCanvas => false;

		public string DefaultToolTipText => "";

		public int ParamCount => 0;

		public object CustomData => null;

		public ICyDesignQuery_v1 DesignQuery => null;

		public ICyDesignEntryPrefs_v1 Preferences => null;

		public ICyDeviceQuery_v1 DeviceQuery => null;

		public ICyVoltageQuery_v1 VoltageQuery => null;

		public bool IsInSystemBuilder => false;

		public bool ContainsParam(string paramName)
		{
			return false;
		}

		public CyCompDevParam GetCommittedParam(string paramName)
		{
			return null;
		}

		public IEnumerable<string> GetParamNames()
		{
			throw new NotImplementedException();
		}

		public IEnumerable<string> GetPossibleEnumValues(string paramName)
		{
			throw new NotImplementedException();
		}

		public IEnumerable<string> GetPossibleEnumValuesFromType(string enumType)
		{
			throw new NotImplementedException();
		}

		public bool IsBoolType(string paramName)
		{
			throw new NotImplementedException();
		}

		public bool IsEnumType(string paramName)
		{
			throw new NotImplementedException();
		}

		public string ResolveEnumDisplayToId(string paramName, string displayName)
		{
			throw new NotImplementedException();
		}

		public string ResolveEnumIdToDisplay(string paramName, string idName)
		{
			throw new NotImplementedException();
		}

		public CyCustErr ResolveEnumParamToDisplay(CyCompDevParam param, out string displayName)
		{
			throw new NotImplementedException();
		}
	}
}
