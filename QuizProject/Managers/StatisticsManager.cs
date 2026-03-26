using System;
using System.Collections.Generic;
using System.Linq;

public class StatisticsManager
{
    // Data structures from System.Collections or System.Collections.Generic are used (1 point)
    private readonly List<Deck> _decks;

    public StatisticsManager(List<Deck> decks)
    {
        _decks = decks;
    }

    public void ViewStatistics()
    {
        if (_decks.Count == 0)
        {
            Console.WriteLine("\nNo data for statistics.\n");
            return;
        }

        Console.WriteLine("\n--- STATISTICS ---");
        
        foreach (var deck in _decks)
        {
            DisplayDeckStatistics(deck);
        }

        DisplayOverallStatistics();
    }

    private void DisplayDeckStatistics(Deck deck)
    {
        DisplayDeckHeader(deck);
        DisplayDeckReviewStats(deck);
        DisplayWeakestCards(deck);
    }

    private void DisplayDeckHeader(Deck deck)
    {
        Console.WriteLine($"\n>> {deck.Name}");
        deck.PrintSummary();
    }

    private void DisplayDeckReviewStats(Deck deck)
    {
        int totalReviews = deck.Cards.Sum(c => c.TimesReviewed);
        int reviewedCards = deck.GetReviewedCardsCount();

        Console.WriteLine($"   Reviews: {totalReviews}");
        Console.WriteLine($"   Reviewed cards: {reviewedCards}");
        Console.WriteLine($"   Success rate: {deck.GetSuccessRate():F1}%");
    }

    private void DisplayWeakestCards(Deck deck)
    {
        if (deck.Cards.Count == 0) return;

        var top3Weakest = deck.GetTopWeakCards(3)
            .Select(c => (Flashcard)c.Clone())
            .ToList();

        if (top3Weakest.Count == 0) return;

        Console.WriteLine("   Top weakest cards:");
        foreach (var card in top3Weakest)
        {
            var (question, answer, successRate) = card;
            Console.WriteLine($"     - {question}: {answer} ({successRate:F1}%)");
        }
    }

    private void DisplayOverallStatistics()
    {
        // Delegates or lambda functions are used (1.5 points)
        int totalCards = _decks.Sum(d => d.Cards.Count);
        int totalReviews = _decks.Sum(d => d.Cards.Sum(c => c.TimesReviewed));
        int totalCorrect = _decks.Sum(d => d.Cards.Sum(c => c.CorrectCount));
        double overallSuccessRate = totalReviews > 0 ? (double)totalCorrect / totalReviews * 100 : 0;

        Console.WriteLine("\n--- Overall Statistics ---");
        Console.WriteLine($"Total decks: {_decks.Count}");
        Console.WriteLine($"Total cards: {totalCards}");
        Console.WriteLine($"Total reviews: {totalReviews}");
        Console.WriteLine($"Overall success rate: {overallSuccessRate:F1}%");
        Console.WriteLine();
    }
}