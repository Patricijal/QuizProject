public class FlashcardStudyStrategy : IStudyStrategy
{
    public void Run(Deck deck)
    {
        Console.WriteLine($"\nStudy Session: {deck.Name}");
        Console.WriteLine("(Press ENTER to reveal answer)\n");

        foreach (var card in deck)
            StudyCard(card, deck);

        Console.WriteLine("Study session complete\n");
    }

    private void StudyCard(Flashcard card, Deck deck)
    {
        // IFormattable "Q" formato naudojimas (1 taškas)
        Console.WriteLine($"Question: {card:Q}");
        Console.Write("(Press ENTER) ");
        Console.ReadLine();

        // Deconstruct (0.5 taško)
        var (_, answer) = card; // discard pattern
        Console.WriteLine($"Answer: {answer}");
        Console.Write("Were you correct? (y/n): ");

        string? response = Console.ReadLine()?.ToLower();
        card.TimesReviewed++;

        if (response == "y")
        {
            card.CorrectCount++;
            Console.WriteLine("Great!\n");
        }
        else
        {
            deck.AddToWeakCards(card);
            Console.WriteLine("Added to weak cards.\n");
        }
    }
}
