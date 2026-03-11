using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;

public class Deck
{
    public string Name { get; set; }
    public List<Flashcard> Cards { get; set; }

    [JsonIgnore]
    public List<Flashcard> WeakCards { get; set; }

    public Deck(string name)
    {
        Name = name;
        Cards = new List<Flashcard>();
        WeakCards = new List<Flashcard>();
    }

    [JsonConstructor]
    public Deck(string name, List<Flashcard> cards)
    {
        Name = name;
        Cards = cards ?? new List<Flashcard>(); // Operators ?., ?[], ??, or ??= (0.5 taško)
        WeakCards = new List<Flashcard>();
        RebuildWeakCards();
    }

    public void AddCard(Flashcard card)
    {
        Cards.Add(card);
    }

    public void RemoveCard(Flashcard card)
    {
        Cards.Remove(card);
        WeakCards.Remove(card);
    }

    public void AddToWeakCards(Flashcard card)
    {
        if (!WeakCards.Contains(card))
        {
            card.ConsecutiveCorrect = 0;
            WeakCards.Add(card);
        }
    }

    public void RemoveFromWeakCards(Flashcard card)
    {
        WeakCards.Remove(card);
    }

    public double GetSuccessRate()
    {
        int totalReviews = Cards.Sum(c => c.TimesReviewed);
        int totalCorrect = Cards.Sum(c => c.CorrectCount);
        return totalReviews == 0 ? 0 : (double)totalCorrect / totalReviews * 100;
    }

    public void PrintSummary()
    {
        Console.WriteLine($"   Cards: {Cards.Count}");
        Console.WriteLine($"   Weak cards: {WeakCards.Count}");
    }

    private void RebuildWeakCards()
    {
        WeakCards.Clear();
        foreach (var card in Cards)
        {
            if (card.ConsecutiveCorrect < 5 && card.TimesReviewed > card.CorrectCount)
            {
                WeakCards.Add(card);
            }
        }
    }
}

