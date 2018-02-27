using AutoMapper;
using DFB_v1_40;
using DFB_v1_40.Asm;
using DFBSimulatorWrapper;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace DFBSimulatorWrapper.DFBStateModel
{
	/// <summary>
	/// The code store represents the instructions and states that have been stored in the DFB from the VLIW assembly
	/// </summary>
	public class CodeStoreModel
	{
		#region Private Members

		private static List<CyState> stateTable;
		private static List<CyControlWord> instructionAList;
		private static List<CyControlWord> instructionBList;
		private static List<CyControlWord> instructionList;
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private static List<string> programLineList;

		#endregion
		#region Public Members

		//public class CycleInstrInfo
		//{
		//	/// <summary>
		//	/// Cycle number
		//	/// </summary>
		//	public int Cycle { get; set; }
		//	/// <summary>
		//	/// State (label)
		//	/// </summary>
		//	public CyState State { get; set; }
		//	/// <summary>
		//	/// First instruction before this label block (represents values incoming to label block, e.g. ACU address)
		//	/// </summary>
		//	public CyControlWord InstructionPriorToLabel { get; set; }
		//	/// <summary>
		//	/// Last instruction in label
		//	/// </summary>
		//	public CyControlWord InstructionLast { get; set; }
		//	/// <summary>
		//	/// Text of last instruction in block
		//	/// </summary>
		//	public string InstructionLastText { get; set; }
		//}

		/// <summary>
		/// List of unique states
		/// </summary>
		/// <returns></returns>
		public static List<CyState> InstructionStateList
		{
			get
			{
				return stateTable;
			}
		}

		/// <summary>
		/// List of unique instructions for control store bank A
		/// If code is not optimized, code store A and B are identical
		/// </summary>
		public static List<CyControlWord> InstructionAList
		{
			get
			{

				return instructionAList;
			}
		}

		/// <summary>
		/// List of unique instructions for control store bank B
		/// If code is not optimized, code store A and B are identical
		/// </summary>
		public static List<CyControlWord> InstructionBList
		{
			get
			{
				return instructionBList;
			}
		}

		/// <summary>
		/// Returns a consolidated list of instructions
		/// </summary>
		public static List<CyControlWord> InstructionList
		{
			get
			{
				return instructionList;
			}
		}

		/// <summary>
		/// Listing of all asm program lines
		/// </summary>
		public static List<string> ProgramLineList
		{
			get
			{
				return programLineList;
			}
		}

		#endregion
		#region Constructor

		/// <summary>
		/// Create a new instance of this device
		/// </summary>
		/// <param name="dfbSimulatorWrapper"></param>
		public CodeStoreModel(Wrapper dfbSimulatorWrapper)
		{
			string[] programLines = dfbSimulatorWrapper.Parameters.Code.Replace("\n", "").Split(new char[] { '\r' }, StringSplitOptions.None);
			programLineList = programLines.ToList();

			var states = (Dictionary<int, DFB_v1_40.Asm.CyState>)PrivateValueAccessor.GetPrivateFieldValue(typeof(CyDfbAsm), "m_stateTable", Wrapper.DfbAsm);
			stateTable = states.OrderBy(x => x.Value.Line).Select(x => x.Value).ToList();

			instructionList = new List<CyControlWord>();

			var compactor = (CyDFBOptimizer)Wrapper.GetCyDfbAsmPrivateFieldCurr("m_compactor");
			if (compactor != null)
			{
				instructionAList = Wrapper.DfbAsm.CommandsA.OrderBy(x => x.Line).ToList();
				instructionBList = Wrapper.DfbAsm.CommandsB.OrderBy(x => x.Line).ToList();
				instructionList.AddRange(Wrapper.DfbAsm.CommandsA);
				instructionList.AddRange(Wrapper.DfbAsm.CommandsB);
			}
			else
			{
				instructionAList = Wrapper.DfbAsm.Commands.OrderBy(x => x.Line).ToList();
				instructionBList = Wrapper.DfbAsm.Commands.OrderBy(x => x.Line).ToList();
				instructionList.AddRange(Wrapper.DfbAsm.Commands);
			}

			instructionList = instructionList.OrderBy(x => x.Line).ToList();
		}

		#endregion
		#region Methods

		///// <summary>
		///// Returns code info for the given cycle
		///// </summary>
		///// <param name="cycle"></param>
		///// <returns></returns>
		//public static CycleInstrInfo CycleCodeInfo(int cycle)
		//{
		//	var cycleInfo = new CycleInstrInfo();
		//	cycleInfo.Cycle = cycle;
		//	cycleInfo.InstructionLast = Instruction(cycle);
		//	cycleInfo.InstructionLastText = ProgramLine(cycle);

		//	// Find label by looking for first state upward from this code line
		//	cycleInfo.State = InstructionStateList.Where(x => x.Line < cycleInfo.InstructionLast.Line).OrderByDescending(x => x.Line).First();

		//	if (cycle > 0)
		//	{
		//		Instruction(cycle - 1);
		//	}

		//	return cycleInfo;
		//}

		/// <summary>
		/// State for the cycle
		/// </summary>
		/// <param name="cycle"></param>
		/// <returns></returns>
		public static CyState State(int cycle)
		{
			var currLine = Instruction(cycle).Line;
			return InstructionStateList
				.OrderByDescending(x => x.Line)
				.Where(x => x.Line < currLine)
				.FirstOrDefault();
		}

		/// <summary>
		/// Retrieves the simulator object for a given cycle back from the current cycle
		/// </summary>
		/// <param name="cycle">Zero-based program cycle number</param>
		/// <returns></returns>
		public static CyControlWord Instruction(int cycle)
		{
			// Note: m_commandIndex in the simulator object is the next instruction
			// so retrieve the prior simulator
			var simulation = DFBState.GetCySimulator(cycle - 1);

			//int instrIndex;

			if(simulation == null)
			{
				return instructionList[0];
			}
			else if (simulation.RamSel.Equals("A"))
			{
				if(simulation.m_commandIndexA > instructionAList.Count - 1)
				{
					return instructionAList[0];
				}
				else
				{
					return instructionAList[simulation.m_commandIndexA];
				}
			}
			else if (simulation.RamSel.Equals("B"))
			{
				if (simulation.m_commandIndexB > instructionBList.Count - 1)
				{
					return instructionBList[0];
				}
				else
				{
					return instructionBList[simulation.m_commandIndexB];
				}
			}
			else
			{
				return null;
			}
			
			/*
			if (simulation == null || simulation.RamSel.Equals("A"))
			{
				instrIndex = simulation == null ? 0 : simulation.m_commandIndexA;
			}
			else
			{
				instrIndex = simulation.m_commandIndexB;
			}

			if (instrIndex >= 0 && instrIndex < instructionList.Count)
			{
				return instructionList[instrIndex];
			}
			else
			{
				//return new CyControlWord(instrIndex);
				return null;
			}
			*/
		}

		/// <summary>
		/// Line number of code that was just executed (1-based)
		/// </summary>
		/// <param name="cycle">Zero-based program cycle number</param>
		/// <returns></returns>
		public static int ProgramLineNbr(int cycle)
		{
			var instr = Instruction(cycle);

			if (instr != null)
			{
				return instr.Line;
			}
			else
			{
				return 1;
			}
		}

		/// <summary>
		/// Line of code that was just executed
		/// </summary>
		/// <param name="cycle">Zero-based program cycle number</param>
		/// <returns></returns>
		private static string ProgramLine(int cycle)
		{
			return programLineList[ProgramLineNbr(cycle) - 1];
		}

		#endregion
	}
}
