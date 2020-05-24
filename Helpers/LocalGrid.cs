using Sandbox.ModAPI.Ingame;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IngameScript.Helpers
{
	public static class LocalGrid
	{
		// Returns true if a block is on the same grid as the running script's Programmable Block
		public static bool IsLocal(IMyTerminalBlock b, IMyGridTerminalSystem gts)
		{
			return (b.CubeGrid == getThisCPU(gts).CubeGrid);
		}

		// Returns the Programmble Block which is currently executing this script.
		public static IMyTerminalBlock getThisCPU(IMyGridTerminalSystem gts)
		{
			// Get a list of all the IMyProgrammableBlock whose bool property IsRunning is true
			List<IMyTerminalBlock> blocks = new List<IMyTerminalBlock>();
			gts.GetBlocksOfType<IMyProgrammableBlock>(blocks, delegate (IMyTerminalBlock b) {
				return (((IMyProgrammableBlock)b).IsRunning);
			}
			);

			// If there is only one programmable block which has IsRunning == true,
			// then it has to be the one running this script
			if (blocks.Count == 1) return (blocks[0]);

			// Otherwise, return null. Remember to check for this when using the method!
			// Edit: throw exception instead as per LordDevious' advice
			throw new Exception("Cannot find running IMyProgrammableBlock. Blocks found: " + blocks.Count.ToString());
			// return null; 
		}
	}
}
