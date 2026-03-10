using System;
using System.Collections.Generic;
using System.Text;

public class DeckManager
{
    private readonly List<Deck> _decks;

    public DeckManager(List<Deck> decks)
    {
        _decks = decks;
    }

    public Deck SelectDeck()
    {
        if (_decks.Count == 0)
        {
            Console.WriteLine("\nNo decks available.");
            return null;
        }

        Console.WriteLine("\nSeleck deck:");
        for (int i = 0; i< _decks.Count; i++)
        {
            Console.WriteLine($"{i + 1}. {_decks[i].Name} ({_decks[i].Cards.Count} cards)");
        }
        Console.Write("Enter the number of the deck you want to select: ");

        if (int.TryParse(Console.ReadLine(), out int choice) && choice >= 1 && choice <= _decks.Count)
        {
            return _decks[choice - 1];
        }
        else
        {
            Console.WriteLine("Invalid selection. Please try again.");
            return null;
        }
    }

    public void CreateDeck()
    {
        Console.Write("\nEnter new deck name: ");
        string name = Console.ReadLine();

        if (!string.IsNullOrWhiteSpace(name))
        {
            var deck = new Deck(name);
            _decks.Add(deck);
            FileManager.SaveDeck(deck);
            Console.WriteLine($"Deck '{name}' created successfully.");
        }
        else
        {
            Console.WriteLine("Deck name cannot be empty. Please try again.");
        }
    }

    public void AddCardToDeck()
    {
        var deck = SelectDeck();
        if (deck == null) return;

        Console.Write("Question/Term: ");
        string question = Console.ReadLine();
        Console.Write("Answer: ");
        string answer = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(question) || string.IsNullOrWhiteSpace(answer))
        {
            Console.WriteLine("Question/Term or Answer can't be empty.\n");
            return;
        }

        Console.Write("Add 4 options? (y/n): ");
        List<string> options = new List<string>();

        if (Console.ReadLine()?.ToLower() == "y") // Operators ?., ?[], ??, or ??= (0.5 taško)
        {
            options.Add(answer);
            for (int i = 1; i < 4; i++)
            {
                Console.Write($"Wrong option {i}: ");
                string wrongAnswer = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(wrongAnswer))
                {
                    options.Add(wrongAnswer);
                }
            }
        }

        deck.AddCard(new Flashcard(question, answer, options));
        FileManager.SaveDeck(deck);
        Console.WriteLine("Card added successfully.");
    }

    public void ViewDecks()
    {
        if (_decks.Count == 0)
        {
            Console.WriteLine("\nNo decks available.");
            return;
        }
        Console.WriteLine("\nAvailable Decks:");
        foreach (var deck in _decks)
        {
            Console.WriteLine($"\n");
            Console.WriteLine($"   {deck.Name}");
            Console.WriteLine($"   Cards: {deck.Cards.Count}");
            Console.WriteLine($"   Weak cards: {deck.WeakCards.Count}");
        }
        Console.WriteLine();
    }

    public void DeleteDeck()
    {
        var deck = SelectDeck();
        if (deck == null) return;

        Console.Write($"Are you sure you want to delete the deck '{deck.Name}'? (y/n): ");
        if (Console.ReadLine()?.ToLower() == "y")
        {
            FileManager.DeleteDeck(deck.Name);
            _decks.Remove(deck);
            Console.WriteLine($"Deck '{deck.Name}' deleted successfully.");
        }
        else
        {
            Console.WriteLine("Cancelled. Deck not deleted.");
        }
    }

    public void ExportDeck()
    {
        var deck = SelectDeck();
        if (deck == null) return;

        Console.Write("Enter export path (e.g., C:\\Export\\deck.json): ");
        string path = Console.ReadLine();

        try
        {
            FileManager.ExportDeck(deck, path);
            Console.WriteLine($"Deck exported to {path}\n");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}\n");
        }
    }

    public void ImportDeck()
    {
        Console.Write("Enter file path: ");
        string path = Console.ReadLine();

        try
        {
            var deck = FileManager.ImportDeck(path);
            if (deck != null)
            {
                _decks.Add(deck);
                FileManager.SaveDeck(deck);
                Console.WriteLine($"Deck '{deck.Name}' imported!\n");
            }
            else
            {
                Console.WriteLine("Failed to import deck.\n");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}\n");
        }
    }

    public void InitializeSampleDecks()
    {
        Console.WriteLine("Creating sample decks...\n");

        var geography = new Deck("Geography");
        geography.AddCard(new Flashcard("What is the capital of France?", "Paris",
            new List<string> { "Paris", "London", "Berlin", "Madrid" }));
        geography.AddCard(new Flashcard("What is the capital of Lithuania?", "Vilnius", 
            new List<string> { "Vilnius", "Kaunas", "Riga", "Tallinn" }));
        geography.AddCard(new Flashcard("What is the largest ocean in the world?", "Pacific", 
            new List<string> { "Pacific", "Atlantic", "Indian", "Arctic" }));

        var math = new Deck("Mathematics");
        math.AddCard(new Flashcard("What is 2 + 2?", "4",
            new List<string> { "4", "3", "5", "22" }));
        math.AddCard(new Flashcard("What is 5 × 6?", "30", 
            new List<string> { "30", "25", "35", "56" }));
        math.AddCard(new Flashcard("What is 100 ÷ 4?", "25",
            new List<string> { "25", "20", "24", "50" }));

        _decks.Add(geography);
        _decks.Add(math);

        SaveAllDecks();
    }

    public void SaveAllDecks()
    {
        Console.WriteLine("\nSaving decks...");
        foreach (var deck in _decks)
        {
            FileManager.SaveDeck(deck);
        }
        Console.WriteLine("All decks saved!\n");
    }

    public List<Deck> LoadDecksFromFiles()
    {
        Console.WriteLine("Loading decks...");
        var loadedDecks = FileManager.LoadAllDecks();
        Console.WriteLine($"Loaded {loadedDecks.Count} deck(s).\n");
        return loadedDecks;
    }
}