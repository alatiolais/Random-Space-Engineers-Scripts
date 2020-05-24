using IngameScript.Helpers;
using Sandbox.ModAPI.Ingame;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VRage.Game.ModAPI.Ingame;

namespace IngameScript.Organization
{
    public class DumpToOneInventory : MyGridProgram
	{
		
		//*************************TRASHMAN v0.4********************
		void Main()
		{
			string thisShipsName = "*Teroc-Stoned*";

			IMyCargoContainer containerB = GridTerminalSystem.GetBlockWithName("MAIN STORAGE " + thisShipsName) as IMyCargoContainer;
			// Blocks with an inventory can be cast as inventory owners
			// You can then use "GetInventory(inventory_number)" on them
			if (containerB == null)
			{ return; }

			IMyInventory inventoryB = containerB.GetInventory(0);

			List<IMyInventory> allInventories = new List<IMyInventory>();

			//Then all the inventories on the grid that might contain the items we want
			allInventories = GetAllShipInventories();

			//Echo($@"{allContainers.Count}");
			// Let's find out how many items we need to move
			foreach (IMyInventory inventory in allInventories)
			{
				List<MyInventoryItem> items = new List<MyInventoryItem>();
				inventory.GetItems(items, null);
				//Echo($@" COUNT: {items.Count}");
				for (int i = items.Count - 1; i >= 0; i--)
				{
					if (!inventoryB.IsFull)
					{
						// check jcuber's api doc for details on the TransferItemTo function
						inventory.TransferItemTo(inventoryB, i, null, true, null);
					}
					else
					{
						// inventoryB is full, let's just break out of the for loop
						break;
					}
				}
			}
			Echo($@"Moved {allInventories.Count()} inventories to {containerB.CustomName}");
		}

		List<IMyInventory> GetAllShipInventories()
		{
			List<IMyTerminalBlock> allBlocks = new List<IMyTerminalBlock>();
			List<IMyInventory> inventories = new List<IMyInventory>();
			GridTerminalSystem.GetBlocks(allBlocks); // Populate list with all blocks on the grid

			foreach (IMyTerminalBlock block in allBlocks)
			{
				if (LocalGrid.IsLocal(block, GridTerminalSystem))
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
	}
}
