using System;
using System.Collections.Generic;
using System.Linq;

public class StatisticsManager
{
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
        Console.WriteLine($"\n>> {deck.Name}");
        deck.PrintSummary();

        int totalReviews = deck.Cards.Sum(c => c.TimesReviewed);
        int totalCorrect = deck.Cards.Sum(c => c.CorrectCount);

        Console.WriteLine($"   Reviews: {totalReviews}");
        Console.WriteLine($"   Success rate: {deck.GetSuccessRate():F1}%");
        
        if (deck.Cards.Count > 0)
        {
            // IComparable<T> - rūšiuoja per CompareTo (0.5 taško)
            var sorted = deck.Cards
                .Where(c => c.TimesReviewed > 0)
                .OrderBy(c => c)  // naudoja CompareTo
                .ToList();

            // Range type (0.5 taško)
            var top3Weakest = sorted[..Math.Min(3, sorted.Count)];

            if (top3Weakest.Count > 0)
            {
                Console.WriteLine("   Top weakest cards:");
                foreach (var card in top3Weakest)
                {
                    var (question, answer) = card;
                    Console.WriteLine($"     - {question}: {answer} ({card:S})");
                }
            }
        }
    }

    private void DisplayOverallStatistics()
    {
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