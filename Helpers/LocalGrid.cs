using Sandbox.ModAPI.Ingame;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VRage.Game.ModAPI.Ingame;

namespace IngameScript.Helpers
{
	public static class LocalGrid
	{
		public static List<IMyInventory> GetAllLocalInventories()
		{
			List<IMyTerminalBlock> allBlocks = new List<IMyTerminalBlock>();
			List<IMyInventory> inventories = new List<IMyInventory>();
			GridTerminalSystem.GetBlocks(allBlocks); // Populate list with all blocks on the grid

			foreach (IMyTerminalBlock block in allBlocks)
			{
				if (IsLocal(block, GridTerminalSystem))
				{
					if (block is IMyCargoContainer || block is IMyShipConnector || block is IMyAssembler)
					{
						IMyInventory inventory = block.GetInventory(0); //Get block inventory
						List<MyInventoryItem> invList = new List<MyInventoryItem>();
						inventory.GetItems(invList, null);
						if (block is IMyAssembler)
						{
							//Since this is an assembler, get its output inventory
							inventory = block.GetInventory(1);

						}
						if (invList.Count > 0)
						{
							inventories.Add(inventory);
						}
					}
				}
			}

			return inventories;
		}

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
