using Sandbox.ModAPI.Ingame;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VRage.Game.ModAPI.Ingame;

namespace IngameScript.Organization
{
    public class Transfer : MyGridProgram
    {
		//*************************TRANSFOR TO/FROM*************************
		void Main()
		{
			TransferToCargoContainer();
		}
		
        void TransferToCargoContainer()
        {
			List<IMyCargoContainer> customCargoContainers = GetCargoContainersWithCustomData();

			IMyCargoContainer containerB = GridTerminalSystem.GetBlockWithName("MAIN STORAGE ") as IMyCargoContainer;

			List<IMyCargoContainer> toCargo = new List<IMyCargoContainer>();
			List<IMyCargoContainer> fromCargo = new List<IMyCargoContainer>();

			foreach (IMyCargoContainer customCargo in customCargoContainers)
			{
				//IMyInventory customCargoInv = customCargo.GetInventory(0);
				string customCargoData = customCargo.CustomData;

				if (customCargoData.Contains("*TO*"))
				{
					toCargo.Add(customCargo);
				}
				else if (customCargoData.Contains("*FROM*"))
				{
					fromCargo.Add(customCargo);
				}
			}

			foreach(IMyCargoContainer cc in fromCargo)
			{
				string fromAddress = cc.CustomData.Replace("*FROM* ", "");
				foreach(IMyCargoContainer cct in toCargo)
				{
					string toAddress = cct.CustomData.Replace("*TO* ", "");
					if(fromAddress == toAddress)
					{//to and from addresses match so begin transfer
						IMyInventory fromInventory = cc.GetInventory(0);
						IMyInventory toInventory = cct.GetInventory(0);

						//get items from the 'to' inventory
						List<MyInventoryItem> items = new List<MyInventoryItem>();
						toInventory.GetItems(items, null);
						int counter = 0;
						for (int i = items.Count - 1; i >= 0; i--)
						{
							if (!fromInventory.IsFull)
							{	//transfer items to the destination
								toInventory.TransferItemTo(fromInventory, i, null, true, null);
								counter++;
							}
							else
							{
								// fromInventory is full, let's just break out of the for loop
								break;
							}
						}

						Echo($@"{counter} items moved from {cct.DisplayNameText} to {cc.DisplayNameText}");
					}
				}
			}
		}

		List<IMyCargoContainer> GetCargoContainersWithCustomData()
        {
			List<IMyCargoContainer> cargoContainers = new List<IMyCargoContainer>();
			List<IMyCargoContainer> customContainers = new List<IMyCargoContainer>();
			//List<IMyTerminalBlock> allBlocks = new List<IMyTerminalBlock>(); 

			GridTerminalSystem.GetBlocksOfType<IMyCargoContainer>(cargoContainers); // Where allBlocks is a list type.  

			foreach (IMyCargoContainer container in cargoContainers)
			{
				if (container.CustomData != "")
				{
					customContainers.Add(container);
				}
			}

			return customContainers;
		}
    }
}
