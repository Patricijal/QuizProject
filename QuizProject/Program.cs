using System;
using System.Collections.Generic;

class Program
{
    static void Main()
    {
        Console.WriteLine("Anki Learning System\n");
        // Data structures from System.Collections.Generic (1 taškas)
        List<Deck> decks = new List<Deck>();

        var deckManager = new DeckManager(decks);
        deckManager.DeckCreated += (_, deck) => Console.WriteLine($"[Event] Deck created: {deck.Name}");
        deckManager.CardAdded += (_, card) => Console.WriteLine($"[Event] Card added: {card.Question}");
        var studyManager = new StudySessionManager(deckManager);
        var statsManager = new StatisticsManager(decks);
        var menuManager = new MenuManager(studyManager, deckManager, statsManager);

        var loadedDecks = deckManager.LoadDecksFromFiles();
        decks.AddRange(loadedDecks);

        if (decks.Count == 0)
        {
            deckManager.InitializeSampleDecks();
        }

        bool running = true;
        while (running)
        {
            menuManager.ShowMainMenu();
            string? choice = Console.ReadLine();
            running = menuManager.HandleMainMenuChoice(choice);
        }
    }
}