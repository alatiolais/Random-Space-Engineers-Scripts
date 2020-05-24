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
    public class Sorter : MyGridProgram
    {
		//*************************ORGANIZER/SORTER SCRIPT*************************
		void Main()
		{
			string thisShipsName = "*Teroc-Stoned*";
			List<IMyCargoContainer> customContainers = new List<IMyCargoContainer>();
			List<IMyInventory> allInventories = new List<IMyInventory>();
			// Let's find the cargo containers we want to move to/from
			// We'll be trying to sort all items into containers that have the relivent tags for the items we
			// will be moving to them. For example if the block has .CustomData that contains the tag 'Steel Plate', it 
			// will have all steel plate in the grid moved to it.

			//First we grab all the containers with CustomData populated
			customContainers = GetCargoContainersWithCustomData(thisShipsName);
			//Then all the inventories on the grid that might contain the items we want
			allInventories = GetAllShipInventories(thisShipsName);


			foreach (IMyCargoContainer customCargo in customContainers)
			{
				IMyInventory customCargoInv = customCargo.GetInventory(0);
				string customCargoData = customCargo.CustomData;

				// Let's find out how many items we need to move
				foreach (IMyInventory inventory in allInventories)
				{
					List<MyInventoryItem> items = new List<MyInventoryItem>();

					inventory.GetItems(items);
					//Echo($@" COUNT: {items.Count}");
					foreach(MyInventoryItem i in items)
					{
						Echo($@"ItemId: {i.ItemId.ToString()}");
						Echo($@"Type: {i.Type.ToString()}");
					}
					
					//for (int i = items.Count - 1; i >= 0; i--)
					//{
					//	Echo($@"CustomItem: {items[i].Content.SubtypeId.ToString()}");
					//	if (customCargoData.Contains(items[i].Content.SubtypeId.ToString()))
					//	{

					//		// Check and make sure customCargoInv isn't full
					//		if (!customCargoInv.IsFull)
					//		{
					//			// check jcuber's api doc for details on the TransferItemTo function
					//			inventory.TransferItemTo(customCargoInv, i, null, true, null);
					//		}
					//		else
					//		{
					//			// inventoryB is full, let's just break out of the for loop
					//			break;
					//		}
					//	}
					//}
				}
			}
			Echo($@"Items sorted to corresponding containers");
		}

		List<IMyInventory> GetAllShipInventories(string shipName)
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
						if (block is IMyAssembler)
						{
							inventory = block.GetInventory(1); //Since this is an assembler, get its output inventory
						}
						if (inventory.ItemCount > 0)
						{
							inventories.Add(inventory);
						}
					}
				}
			}

			return inventories;
		}

		List<IMyCargoContainer> GetCargoContainersWithCustomData(string shipName)
		{

			List<IMyCargoContainer> cargoContainers = new List<IMyCargoContainer>();
			List<IMyCargoContainer> customContainers = new List<IMyCargoContainer>();
			//List<IMyTerminalBlock> allBlocks = new List<IMyTerminalBlock>(); 

			GridTerminalSystem.GetBlocksOfType<IMyCargoContainer>(cargoContainers); // Where allBlocks is a list type.  

			foreach (IMyCargoContainer container in cargoContainers)
			{
				if (container.CustomName.Contains(shipName))
				{
					if (container.CustomData != "")
					{
						customContainers.Add(container);
					}
				}
			}

			return customContainers;
		}
	}
}
