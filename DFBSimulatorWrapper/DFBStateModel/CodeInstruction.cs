using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DFB_v1_40.Asm;

namespace DFBSimulatorWrapper.DFBStateModel
{
	/// <summary>
	/// Represents an instruction for a cycle
	/// </summary>
	public class CodeInstruction
	{
		/// <summary>
		/// Represents a single instruction
		/// </summary>
		public readonly CyControlWord Instruction;

		/// <summary>
		/// Represents the program line text for the instruction
		/// </summary>
		public string InstructionText;

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="instruction">Instruction</param>
		public CodeInstruction(CyControlWord instruction)
		{
			Instruction = instruction;
			InstructionText = CodeStoreModel.ProgramLineList[instruction.Line - 1];
		}
	}
}
