using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

public static class FileManager
{
    private sealed class DeckFileModel
    {
        public string Name { get; set; } = string.Empty;
        public List<Flashcard> Cards { get; set; } = new List<Flashcard>();
    }

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

        var model = new DeckFileModel
        {
            Name = deck.Name,
            Cards = deck.Cards
        };

        string json = JsonSerializer.Serialize(model, JsonOptions);
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
        return DeserializeDeck(json, Path.GetFileNameWithoutExtension(fileName));
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
                var fallbackName = Path.GetFileNameWithoutExtension(file);
                var deck = DeserializeDeck(json, fallbackName);
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
        var model = new DeckFileModel
        {
            Name = deck.Name,
            Cards = deck.Cards
        };

        string json = JsonSerializer.Serialize(model, JsonOptions);
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
        return DeserializeDeck(json, Path.GetFileNameWithoutExtension(importPath));
    }

    private static Deck DeserializeDeck(string json, string fallbackName)
    {
        if (string.IsNullOrWhiteSpace(json))
        {
            return null;
        }

        string trimmed = json.TrimStart();

        if (trimmed.StartsWith("["))
        {
            var cards = JsonSerializer.Deserialize<List<Flashcard>>(json, JsonOptions) ?? new List<Flashcard>();
            return new Deck(fallbackName, cards);
        }

        var model = JsonSerializer.Deserialize<DeckFileModel>(json, JsonOptions);
        if (model == null)
        {
            return null;
        }

        string name = string.IsNullOrWhiteSpace(model.Name) ? fallbackName : model.Name;
        return new Deck(name, model.Cards ?? new List<Flashcard>());
    }

    private static string GetSafeFileName(string deckName)
    {
        string safe = string.Join("_", deckName.Split(Path.GetInvalidFileNameChars()));
        return $"{safe}.json";
    }
}
