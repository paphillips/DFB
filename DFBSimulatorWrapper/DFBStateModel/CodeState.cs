using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DFB_v1_40.Asm;

namespace DFBSimulatorWrapper.DFBStateModel
{
	/// <summary>
	/// Represents a state (label) and its instructions
	/// </summary>
	public class CodeState
	{
		/// <summary>
		/// Program line number of this state label
		/// </summary>
		public int StateLineNumber
		{
			get
			{
				return State.Line;
			}
			
		}

		/// <summary>
		/// Line number of first instruction in this state (label)
		/// </summary>
		public int FirstInstructionLineNumber
		{
			get
			{
				return Instructions.First().Instruction.Line;
			}
		}

		/// <summary>
		/// Line number of last instruction in this state (label)
		/// </summary>
		public int LastInstructionLineNumber
		{
			get
			{
				return Instructions.Last().Instruction.Line;
			}
		}

		/// <summary>
		/// State (label)
		/// </summary>
		public readonly CyState State;

		/// <summary>
		/// Instructions within this state (label)
		/// </summary>
		public List<CodeInstruction> Instructions;

		/// <summary>
		/// The jump instruction for this state
		/// </summary>
		public readonly CodeInstruction JumpInstruction;

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="state">State to create the class from</param>
		public CodeState(CyState state)
		{
			State = state;

			Instructions = new List<CodeInstruction>();

			var allInstructions = CodeStoreModel.InstructionList;
			var nextState = CodeStoreModel.InstructionStateList
				.OrderBy(x => x.Line)
				.Where(x => x.Line > state.Line)
				.FirstOrDefault();

			if(nextState != null)
			{
				Instructions.AddRange(allInstructions
					.Where(x => x.Line > state.Line && x.Line < nextState.Line)
					.OrderBy(x => x.Line)
					.Select(x => new CodeInstruction(x)));
			}
			else
			{
				Instructions.AddRange(allInstructions
					.Where(x => x.Line > state.Line && x.Line < allInstructions.Last().Line + 1)
					.OrderBy(x => x.Line)
					.Select(x => new CodeInstruction(x)));
			}

			JumpInstruction = Instructions
				.Where(x => Convert.ToBoolean(x.Instruction.Eob) == true)
				.FirstOrDefault();
		}
	}
}
