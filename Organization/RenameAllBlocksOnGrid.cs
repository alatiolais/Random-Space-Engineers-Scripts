using IngameScript.Helpers;
using Sandbox.ModAPI.Ingame;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IngameScript.Organization
{
    public class RenameAllBlocksOnGrid : MyGridProgram
    {
		//*************************RENAME SCRIPT*************************
		void Main()
		{
			List<IMyTerminalBlock> allBlocks = new List<IMyTerminalBlock>();
			string oldName = "Teroc Stoned";//Use this variable while renaming
			string inputString = "*Teroc-Stoned*";

			GridTerminalSystem.GetBlocks(allBlocks); //Populate allBlocks list with list of all blocks on grid

			addShipNameToTerminalBlocks(inputString, allBlocks);
			//replaceShipNameToTerminalBlocks(inputString, oldName, allBlocks);
		}

		void addShipNameToTerminalBlocks(string shipName, List<IMyTerminalBlock> Blocks)
		{
			foreach (IMyTerminalBlock block in Blocks)
			{
				if (!block.CustomName.Contains(shipName) && LocalGrid.IsLocal(block, GridTerminalSystem))
				{
					block.CustomName = $@"{block.CustomName} {shipName}";
				}
			}

			Echo($@"Added {shipName} to each terminal block name on grid");
		}

		void replaceShipNameToTerminalBlocks(string newName, string oldName, List<IMyTerminalBlock> Blocks)
		{
			foreach (IMyTerminalBlock block in Blocks)
			{
				if (block.CustomName.Contains(oldName))
				{
					string _newName = block.CustomName.Replace(oldName, newName);
					block.CustomName = _newName;
				}
			}

			Echo($@"Changed {oldName} to {newName} on each terminal block name on grid");
		}
	}
}
