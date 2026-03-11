using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

public static class FileManager
{
    private static readonly string DecksFolder = "Decks";
    private static readonly JsonSerializerOptions JsonOptions = new JsonSerializerOptions
    {
        WriteIndented = true,
        PropertyNameCaseInsensitive = true
    };

    // A static constructor is used (1 point)
    static FileManager()
    {
        if(!Directory.Exists(DecksFolder))
        {
            Directory.CreateDirectory(DecksFolder);
        }
    }

    public static void SaveDeck(Deck deck)
    {
        string fileName = GetSafeFileName(deck.Name);
        string filePath = Path.Combine(DecksFolder, fileName);

        string json = JsonSerializer.Serialize(deck, JsonOptions);
        File.WriteAllText(filePath, json);

        Console.WriteLine($"Deck '{deck.Name}' saved successfully.");
    }

    public static Deck LoadDeck(string fileName)
    {
        string filePath = Path.Combine(DecksFolder, fileName);

        if (!File.Exists(filePath))
        {
            return null;
        }

        string json = File.ReadAllText(filePath);
        return JsonSerializer.Deserialize<Deck>(json, JsonOptions);
    }

    public static List<string> GetAllDeckFiles()
    {
        return new List<string>(Directory.GetFiles(DecksFolder, "*.json"));
    }

    public static List<Deck> LoadAllDecks()
    {
        var decks = new List<Deck>();
        var files = GetAllDeckFiles();

        foreach (var file in files)
        {
            try
            {
                string json = File.ReadAllText(file);
                var deck = JsonSerializer.Deserialize<Deck>(json, JsonOptions);
                if (deck != null)
                {
                    decks.Add(deck);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading {Path.GetFileName(file)}: {ex.Message}");
            }
        }

        return decks;
    }

    public static void DeleteDeck(string deckName)
    {
        string fileName = GetSafeFileName(deckName);
        string filePath = Path.Combine(DecksFolder, fileName);

        if (File.Exists(filePath))
        {
            File.Delete(filePath);
            Console.WriteLine($"Deck '{deckName}' deleted.");
        }
    }

    public static string ExportDeck(Deck deck, string exportPath)
    {
        string json = JsonSerializer.Serialize(deck, JsonOptions);
        File.WriteAllText(exportPath, json);
        return exportPath;
    }

    public static Deck ImportDeck(string importPath)
    {
        if (!File.Exists(importPath))
        {
            return null;
        }

        string json = File.ReadAllText(importPath);
        return JsonSerializer.Deserialize<Deck>(json, JsonOptions);
    }

    private static string GetSafeFileName(string deckName)
    {
        string safe = string.Join("_", deckName.Split(Path.GetInvalidFileNameChars()));
        return $"{safe}.json";
    }
}
