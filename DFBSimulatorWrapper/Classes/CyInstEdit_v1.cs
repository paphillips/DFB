using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CyDesigner.Extensions.Common;
using CyDesigner.Extensions.Gde;

namespace DFBSimulatorWrapper
{
	public class CyInstEdit_v1 : CyDesigner.Extensions.Gde.ICyInstEdit_v1
	{
		private Dictionary<string, CyCompDevParam> parameters = new Dictionary<string, CyCompDevParam>();

		public string ComponentName => "DFB";

		public string SymbolFileName => "";

		public string InstanceName => "Instance";

		public string InstanceIdPath => "";

		public string CurrentDevice => "";

		public bool IsPreviewCanvas => false;

		public string DefaultToolTipText => "Tooltip";

		public int ParamCount => parameters.Count;

		public object CustomData => null;

		public ICyDesignQuery_v1 DesignQuery => null;

		public ICyDesignEntryPrefs_v1 Preferences => null;

		public ICyDeviceQuery_v1 DeviceQuery => null;

		public ICyVoltageQuery_v1 VoltageQuery => null;

		public bool IsInSystemBuilder => true;

		public bool CommitParamExprs()
		{
			return true;
		}

		public bool ContainsParam(string paramName)
		{
			return parameters.ContainsKey(paramName);
		}

		public string CreateDesignPersistantPath(string canonicalPath)
		{
			throw new NotImplementedException();
		}

		public ICyParamEditor CreateParamEditor(ICyParamEditingControl controlToDisplay)
		{
			throw new NotImplementedException();
		}

		public ICyTabbedParamEditor CreateTabbedParamEditor()
		{
			throw new NotImplementedException();
		}

		public CyCompDevParam GetCommittedParam(string paramName)
		{
			return parameters[paramName];
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

		public void NotifyWhenDesignUpdates(CyDesignUpdated_v1 fnc)
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

		public CyCustErr SetCustomData(object o)
		{
			throw new NotImplementedException();
		}

		public bool SetParamExpr(string paramName, string expr)
		{
			if (parameters.ContainsKey(paramName))
			{
				parameters[paramName] = GenerateCyCompDevParam(paramName, expr);
			}
			else
			{
				parameters.Add(paramName, GenerateCyCompDevParam(paramName, expr));
			}
			return true;
		}

		private CyCompDevParam GenerateCyCompDevParam(string paramName, string expr)
		{
			var name = paramName;
			var exprType = "";
			var value = expr;
			var typeName = "";
			var defaultExpr = "";
			var tabName = "";
			var categoryName = "";
			var description = "";
			bool isVisible = false;
			bool isReadOnly = false;
			bool isHardware = false;
			bool isFormal = false;
			bool isDisplayEvaluated = false;
			int errorCount = 0;
			object valueObject = expr;
			IEnumerable<string> errors = null;

			return new CyCompDevParam(name, expr, exprType, value, typeName, defaultExpr, tabName, categoryName, description, isVisible, isReadOnly, isHardware, isFormal, isDisplayEvaluated, errorCount, valueObject, errors);
		}
	}
}
