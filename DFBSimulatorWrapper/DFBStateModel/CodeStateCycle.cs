using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DFBSimulatorWrapper.DFBStateModel
{
	/// <summary>
	/// Represents the state (label) and instruction at this code cycle
	/// </summary>
	public class CodeStateCycle
	{
		/// <summary>
		/// Cycle number
		/// </summary>
		public readonly int Cycle;

		/// <summary>
		/// Label of current instruction
		/// </summary>
		public readonly string Label;

		/// <summary>
		/// Current instruction at this cycle
		/// </summary>
		public readonly CodeInstruction Instruction;

		/// <summary>
		/// Indicates if this is the jump instruction (last line) in the label
		/// </summary>
		public readonly bool IsJumpInstruction;
		
		/// <summary>
		/// State (label) at this cycle
		/// </summary>
		public readonly CodeState CodeState;

		/// <summary>
		/// Returns a label common to all calls of this state
		/// </summary>
		public readonly string GroupName;

		/// <summary>
		/// Returns a label unique to this state and cycle
		/// </summary>
		public readonly string UniqueName;

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="cycle">Cycle number</param>
		public CodeStateCycle(int cycle)
		{
			var instr = CodeStoreModel.Instruction(cycle);
			var state = CodeStoreModel.State(cycle);
			if(instr == null || state == null) { return; }

			Cycle = cycle;
			Label = state.Label;
			Instruction = new CodeInstruction(instr);
			IsJumpInstruction = Convert.ToBoolean(Instruction.Instruction.Eob);
			CodeState = new CodeState(state);
			GroupName = Label;
			UniqueName = string.Format("{0}_{1}", GroupName, Cycle);
		}
	}
}
