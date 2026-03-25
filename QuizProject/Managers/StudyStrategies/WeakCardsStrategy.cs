using System.Linq;

public class WeakCardsStrategy : IStudyStrategy
{
    public void Run(Deck deck)
    {
        var weakCards = deck.GetWeakCardsIterator().ToList();

        if (weakCards.Count == 0)
        {
            Console.WriteLine("\nNo weak cards! Excellent!\n");
            return;
        }

        Console.WriteLine($"\nWeak Cards: {deck.Name}");
        Console.WriteLine($"Number of cards: {weakCards.Count}");
        Console.WriteLine("(Need to answer correctly 5 times in a row)\n");

        foreach (var card in weakCards)
            ReviewWeakCard(card, deck);

        Console.WriteLine($"Weak cards remaining: {deck.WeakCards.Count}\n");
    }

    private void ReviewWeakCard(Flashcard card, Deck deck)
    {
        Console.WriteLine($"Question: {card.Question}");
        Console.WriteLine($"[Consecutive correct: {card.ConsecutiveCorrect}/5]");
        Console.Write("(Press ENTER to reveal answer) ");
        Console.ReadLine();

        Console.WriteLine($"Answer: {card.Answer}");
        Console.Write("Were you correct? (y/n): ");
        string response = Console.ReadLine()?.ToLower();

        card.TimesReviewed++;

        if (response == "y")
        {
            card.CorrectCount++;
            card.ConsecutiveCorrect++;

            if (card.ConsecutiveCorrect >= 5)
            {
                deck.RemoveFromWeakCards(card);
                Console.WriteLine($"Excellent! Card removed from weak cards ({card.ConsecutiveCorrect}/5)\n");
            }
            else
            {
                Console.WriteLine($"Good! ({card.ConsecutiveCorrect}/5)\n");
            }
        }
        else
        {
            card.ConsecutiveCorrect = 0;
            Console.WriteLine("Start over (0/5)\n");
        }
    }
}
