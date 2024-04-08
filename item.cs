using System;
using System.Collections.Generic;
using System.IO;

class Item
{
    public string Name { get; set; }
    public int Price { get; set; }

    // Constructor for the item
    public Item(string name, int price)
    {
        Name = name;
        Price = price;
    }
}

class Transaction
{
    public Item[] Items { get; set; }  // Array of items in the transaction
    public int AmountPaid { get; set; }  // Amount paid in the transaction

    public Transaction(Item[] items, int amountPaid)
    {
        Items = items;  // Set the items
        AmountPaid = amountPaid;  // Set the amount paid
    }
}

class TillTransaction
{
    // Define a dictionary for the till
    static Dictionary<string, int> till = new Dictionary<string, int>()
    {
        {"R50", 5},
        {"R20", 5},
        {"R10", 6},
        {"R5", 12},
        {"R2", 10},
        {"R1", 10}
    };

    // Define a method to calculate change
    static int[] CalculateChange(int transactionTotal, int amountPaid)
    {
        int change = amountPaid - transactionTotal;
        int[] changeBreakdown = new int[6];
        int[] denominations = { 50, 20, 10, 5, 2, 1 };

        // Loop through the denominations
        for (int i = 0; i < denominations.Length; i++)
        {
            int count = change / denominations[i];
            change -= count * denominations[i];
            changeBreakdown[i] = count;
        }

        return changeBreakdown;
    }

    // Define a method to process the transaction
    static void ProcessTransaction(Transaction transaction)
    {
        int tillTotal = 500;

        // Loop through the till
        foreach (var kvp in till)
        {
            tillTotal += int.Parse(kvp.Key.Substring(1)) * kvp.Value;
        }

        int transactionTotal = 0;

        // Loop through the items in the transaction
        foreach (var item in transaction.Items)
        {
            transactionTotal += item.Price;
        }

        // Calculate the change
        int[] change = CalculateChange(transactionTotal, transaction.AmountPaid);

        // Print the details of the transaction
        Console.WriteLine($"Till Start: R{tillTotal}, Transaction Total: R{transactionTotal}, Paid: R{transaction.AmountPaid}, Change Total: R{transaction.AmountPaid - transactionTotal}, Change Breakdown: R{string.Join("-", change)}");
    }

    static void Main(string[] args)
    {
        // Read the lines from the input file
        string[] lines = File.ReadAllLines("input.txt");

        // Loop through the lines
        foreach (var line in lines)
        {
            // Split the line into transaction details
            string[] transactionDetails = line.Split(";");

            // Initialize a list of item objects
            List<Item> itemObjects = new List<Item>();

            // Loop through the items in the transaction
            foreach (var itemString in transactionDetails[0].Split(","))
            {
                // Check if the item string contains a range of prices
                if (itemString.Contains("-"))
                {
                    string[] itemParts = itemString.Split("-");
                    foreach (var part in itemParts)
                    {
                        // Split the item details
                        string[] itemDetails = part.Trim().Split(" ");

                        // Ensure there are two parts (name and price)
                        if (itemDetails.Length == 2)
                        {
                            // Add the item to the list of item objects
                            itemObjects.Add(new Item(itemDetails[0], int.Parse(itemDetails[1].Substring(1))));
                        }
                    }
                }
                else
                {
                    // Split the item details
                    string[] itemDetails = itemString.Trim().Split(" ");

                    // Ensure there are two parts (name and price)
                    if (itemDetails.Length == 2)
                    {
                        // Add the item to the list of item objects
                        itemObjects.Add(new Item(itemDetails[0], int.Parse(itemDetails[1].Substring(1))));
                    }
                }
            }

            // Parse the amount paid
            int amountPaid = int.Parse(transactionDetails[1].Split("-")[0].Substring(1));

            // Process the transaction
            ProcessTransaction(new Transaction(itemObjects.ToArray(), amountPaid));
        }

        int tillEndTotal = 500;

        // Loop through the till
        foreach (var kvp in till)
        {
            tillEndTotal += int.Parse(kvp.Key.Substring(1)) * kvp.Value;
        }

        // Print the total at the end of the till
        Console.WriteLine($"Till End: R{tillEndTotal}");
    }
}
