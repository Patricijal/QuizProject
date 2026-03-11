using System.Linq;

public class MultipleChoiceStrategy : IStudyStrategy
{
    private readonly Random _random = new();

    public void Run(Deck deck)
    {
        // Delegates or lambda functions (1.5 taško)
        var cardsWithOptions = deck.Cards.Where(c => c.MultipleChoiceOptions.Count >= 4).ToList();
        if (cardsWithOptions.Count == 0)
        {
            Console.WriteLine("\nThis deck has no cards with multiple choice options!\n");
            return;
        }

        Console.WriteLine($"\nMultiple Choice Test: {deck.Name}\n");
        int correct = 0;

        foreach (var card in cardsWithOptions)
            AskQuestion(card, deck, ref correct);

        Console.WriteLine($"Result: {correct}/{cardsWithOptions.Count} ({(double)correct / cardsWithOptions.Count * 100:F1}%)\n");
    }

    private void AskQuestion(Flashcard card, Deck deck, ref int correct)
    {
        Console.WriteLine($"Question: {card.Question}");

        // Delegates or lambda functions (1.5 taško)
        var shuffledOptions = card.MultipleChoiceOptions.OrderBy(x => _random.Next()).ToList();
        for (int i = 0; i < shuffledOptions.Count; i++)
            Console.WriteLine($"{i + 1}. {shuffledOptions[i]}");

        Console.Write("Choose answer (1-4): ");
        if (!int.TryParse(Console.ReadLine(), out int choice) || choice < 1 || choice > 4)
        {
            Console.WriteLine("Invalid choice.\n");
            return;
        }

        string selectedAnswer = shuffledOptions[choice - 1];
        card.TimesReviewed++;

        if (selectedAnswer == card.Answer)
        {
            card.CorrectCount++;
            correct++;
            Console.WriteLine("Correct!\n");
        }
        else
        {
            deck.AddToWeakCards(card);
            Console.WriteLine($"Incorrect. Correct answer: {card.Answer}\n");
        }
    }
}
