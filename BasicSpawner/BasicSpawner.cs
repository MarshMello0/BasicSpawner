using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using System.Linq;
using System.IO;

[ModTitle("BasicSpawner")]
[ModDescription("A mod which allows you to spawn items in where ever you are")]
[ModAuthor(". Marsh.Mello .")]
[ModIconUrl("https://gamepedia.cursecdn.com/minecraft_gamepedia/6/6c/Spawner.png")]
[ModWallpaperUrl("https://gamepedia.cursecdn.com/minecraft_gamepedia/6/6c/Spawner.png")]
[ModVersion("1.1.0")]
[RaftVersion("Update 9 (3556813)")]
public class BasicSpawner : Mod
{
    private List<Item_Base> items = new List<Item_Base>();

    private void Start()
    {
        GenerateList();
        RegisterCommands();
        RConsole.Log("BasicSpawner loaded!");
    }

    private void RegisterCommands()
    {
        RConsole.registerCommand(typeof(BasicSpawner), "Spawns the item", "spawn", SpawnItem);
        RConsole.registerCommand(typeof(BasicSpawner), "Opens your default browser to the list of items", "itemList", OpenBrowerList);
        RConsole.registerCommand(typeof(BasicSpawner), "List all the items into the console", "itemListConsole", PrintItems);
        RConsole.registerCommand(typeof(BasicSpawner), "Exports the list of items to a .txt file", "exportItemList", ExportList);
    }

    private void GenerateList()
    {
        items = ItemManager.GetAllItems();
    }

    public void SpawnItem()
    {
        Item_Base currentItem = null;
        string[] lastCommand = RConsole.lastCommands.LastOrDefault<string>().Split(' ');
        if (lastCommand.Length == 1)
        {
            RConsole.Log("Please include an item name after eg \"spawn Paddle\"");
        }
        else
        {
            string itemName = lastCommand[1];
            int amount = 1;
            if (lastCommand.Length > 2)
            {
                string itemAmount = lastCommand[2];
                int result;
                if (int.TryParse(itemAmount, out result))
                {
                    amount = result;
                }
            }

            for (int i = 0; i < items.Count; i++)
            {
                if (items[i].UniqueName == itemName)
                {
                    PlayerInventory pi = ComponentManager<PlayerInventory>.Value;
                    try
                    {
                        pi.AddItem(items[i].UniqueName, amount);
                        RConsole.Log("Have given player " + amount + " " + items[i].UniqueName);
                    }
                    catch (Exception e)
                    {
                        RConsole.LogError(e.ToString());
                    }
                    return;
                }
            }

            RConsole.Log("Couldn't find the item called " + itemName);
        }
        
    }

    public void PrintItems()
    {
        string longString = "";
        foreach (Item_Base item in items)
        {
            longString += " | " + item.UniqueName;
        }

        RConsole.Log(longString);
        RConsole.Log("This is a long list, but it is recomended to view it in your browser at https://pastebin.com/raw/VdtzyM0N");
    }

    public void OpenBrowerList()
    {
        Application.OpenURL("https://raw.githubusercontent.com/MarshMello0/BasicSpawner/master/itemList.txt");
    }

    public void ExportList()
    {
        RConsole.Log("Exporting list to " + Directory.GetCurrentDirectory() + @"\itemList.txt");
        StreamWriter writer = new StreamWriter(Directory.GetCurrentDirectory() + @"\itemList.txt");

        foreach (Item_Base item in items)
        {
            writer.WriteLine("UniqueName: " + item.UniqueName + " ID: " + item.UniqueIndex + " Display Name: " + item.name);
        }

        writer.Close();
        RConsole.Log("Finished Exporting!");
    }
    public void OnModUnload()
    {
        RConsole.Log("BasicSpawner has been unloaded!");
        Destroy(gameObject);
    }
}