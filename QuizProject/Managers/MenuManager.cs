using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;

// You use a sealed or partial class (0.5 points)
public sealed class MenuManager
{
    private readonly StudySessionManager _studyManager;
    private readonly DeckManager _deckManager;
    private readonly StatisticsManager _statsManager;

    public MenuManager(StudySessionManager studyManager, DeckManager deckManager, StatisticsManager statsManager)
    {
        _studyManager = studyManager;
        _deckManager = deckManager;
        _statsManager = statsManager;
    }

    public void ShowMainMenu()
    {
        Console.WriteLine("\n>>> Main Menu <<<");
        Console.WriteLine("1. Anki flashcards (with reveal)");
        Console.WriteLine("2. Multiple choice test (4 options)");
        Console.WriteLine("3. Timed test");
        Console.WriteLine("4. Weak cards (5 correct streak system)");
        Console.WriteLine("5. Manage decks");
        Console.WriteLine("6. Statistics");
        Console.WriteLine("7. Save and exit");
        Console.Write("Choose (1-7): ");
    }

    // Pattern matching is used (1 point) with switch expressions
    public bool HandleMainMenuChoice(string choice)
    {
        var action = choice switch
        {
            "1" => (Action)_studyManager.StudyFlashcards,
            "2" => _studyManager.MultipleChoiceTest,
            "3" => _studyManager.TimedTest,
            "4" => _studyManager.StudyWeakCards,
            "5" => ShowDeckManagementMenu,
            "6" => _statsManager.ViewStatistics,
            "7" => () => { _deckManager.SaveAllDecks(); Console.WriteLine("Thank you for studying! Goodbye!"); },
            _ => () => Console.WriteLine("Invalid choice.")
        };

        action();
        return choice != "7";
    }

    private void ShowDeckManagementMenu()
    {
        Console.WriteLine("\n>>> Deck Management <<<");
        Console.WriteLine("1. Create new deck");
        Console.WriteLine("2. Add card to deck");
        Console.WriteLine("3. View decks");
        Console.WriteLine("4. Delete deck");
        Console.WriteLine("5. Export deck");
        Console.WriteLine("6. Import deck");
        Console.Write("Choose (1-6): ");

        string choice = Console.ReadLine();
        HandleDeckManagementChoice(choice);
    }

    private void HandleDeckManagementChoice(string choice)
    {
        var action = choice switch
        {
            "1" => (Action)_deckManager.CreateDeck,
            "2" => _deckManager.AddCardToDeck,
            "3" => _deckManager.ViewDecks,
            "4" => _deckManager.DeleteDeck,
            "5" => _deckManager.ExportDeck,
            "6" => _deckManager.ImportDeck,
            _ => () => Console.WriteLine("Invalid choice. Please try again.")
        };

        action();
    }
}

