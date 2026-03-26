using System;
using System.Collections.Generic;
using System.Linq;

// You extended C# types (0.5 points)
public static class DeckExtensions
{
    public static int GetReviewedCardsCount(this Deck deck)
    {
        if (deck == null)
        {
            throw new ArgumentNullException(nameof(deck));
        }

        return deck.Cards.Count(c => c.TimesReviewed > 0);
    }

    public static IEnumerable<Flashcard> GetTopWeakCards(this Deck deck, int count)
    {
        if (deck == null)
        {
            throw new ArgumentNullException(nameof(deck));
        }

        return deck.Cards
            .Where(c => c.TimesReviewed > 0)
            .OrderBy(c => c)
            .TakeSafe(count);
    }
}
