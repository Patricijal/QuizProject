using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

public class StudySessionManager
{
    private readonly DeckManager _deckManager;
    private readonly Random _random;

    public StudySessionManager(DeckManager deckManager)
    {
        _deckManager = deckManager;
        _random = new Random();
    }

    private bool TrySelectDeck(out Deck deck)
    {
        deck = _deckManager.SelectDeck();
        return deck != null;
    }

    public void StudyFlashcards()
    {
        if (!TrySelectDeck(out var deck)) return;

        if (deck.Cards.Count == 0)
        {
            Console.WriteLine("\nDeck is empty!\n");
            return;
        }

        Console.WriteLine($"\nStudy Session: {deck.Name}");
        Console.WriteLine("(Press ENTER to reveal answer)\n");

        foreach (var card in deck.Cards)
        {
            // IFormattable "Q" formato naudojimas (1 taškas)
            Console.WriteLine($"Question: {card:Q}");
            Console.Write("(Press ENTER) ");
            Console.ReadLine();

            // Deconstruct (0.5 taško)
            var (_, answer) = card; // discard pattern
            Console.WriteLine($"Answer: {answer}");
            Console.Write("Were you correct? (y/n): ");
            string response = Console.ReadLine()?.ToLower();
            
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
        
        FileManager.SaveDeck(deck);
        Console.WriteLine("Study session complete\n");
    }

    public void MultipleChoiceTest()
    {
        if (!TrySelectDeck(out var deck)) return;

        // Delegates or lambda functions (1.5 taško)
        var cardsWithOptions = deck.Cards.Where(c => c.MultipleChoiceOptions.Count >= 4).ToList();
        if (cardsWithOptions.Count == 0)
        {
            Console.WriteLine("\nThis deck has no cards with multiple choice options!\n");
            return;
        }

        Console.WriteLine($"\nMultiple Choice Test: {deck.Name}\n");
        int correct = 0;
        int total = cardsWithOptions.Count;

        foreach (var card in cardsWithOptions)
        {
            Console.WriteLine($"Question: {card.Question}");
            // Delegates or lambda functions (1.5 taško)
            var shuffledOptions = card.MultipleChoiceOptions.OrderBy(x => _random.Next()).ToList();
            for (int i = 0; i < shuffledOptions.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {shuffledOptions[i]}");
            }
            
            Console.Write("Choose answer (1-4): ");
            if (int.TryParse(Console.ReadLine(), out int choice) && choice >= 1 && choice <= 4)
            {
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
            else
            {
                Console.WriteLine("Invalid choice.\n");
            }
        }

        FileManager.SaveDeck(deck);
        Console.WriteLine($"Result: {correct}/{total} ({(double)correct / total * 100:F1}%)\n");
    }

    public void TimedTest()
    {
        if (!TrySelectDeck(out var deck)) return;

        if (deck.Cards.Count == 0)
        {
            Console.WriteLine("\nDeck is empty!\n");
            return;
        }

        Console.Write("Enter time limit in seconds: ");
        if (!int.TryParse(Console.ReadLine(), out int timeLimit) || timeLimit <= 0)
        {
            Console.WriteLine("Invalid time limit.\n");
            return;
        }

        Console.WriteLine($"\nTimed Test: {timeLimit}s");
        Console.WriteLine("Test will start in 3 seconds...\n");
        System.Threading.Thread.Sleep(3000);

        Stopwatch stopwatch = Stopwatch.StartNew();
        int answered = 0;
        int correct = 0;

        foreach (var card in deck.Cards)
        {
            if (stopwatch.Elapsed.TotalSeconds >= timeLimit)
            {
                Console.WriteLine("\nTime's up!");
                break;
            }

            double timeLeft = timeLimit - stopwatch.Elapsed.TotalSeconds;
            Console.WriteLine($"[Time left: {timeLeft:F1}s] {card.Question}");
            Console.Write("Answer: ");
            string answer = Console.ReadLine();
            
            answered++;
            card.TimesReviewed++;
            
            if (answer?.Trim().Equals(card.Answer, StringComparison.OrdinalIgnoreCase) == true)
            {
                card.CorrectCount++;
                correct++;
                Console.WriteLine("Correct!\n");
            }
            else
            {
                deck.AddToWeakCards(card);
                Console.WriteLine($"Incorrect. Answer: {card.Answer}\n");
            }
        }

        stopwatch.Stop();
        FileManager.SaveDeck(deck);
        Console.WriteLine($"\nTest completed in {stopwatch.Elapsed.TotalSeconds:F1}s ");
        Console.WriteLine($"Answered: {answered}/{deck.Cards.Count}");
        Console.WriteLine($"Correct: {correct}/{answered} ({(answered > 0 ? (double)correct / answered * 100 : 0):F1}%)\n");
    }

    public void StudyWeakCards()
    {
        if (!TrySelectDeck(out var deck)) return;

        if (deck.WeakCards.Count == 0)
        {
            Console.WriteLine("\nNo weak cards! Excellent!\n");
            return;
        }

        Console.WriteLine($"\nWeak Cards: {deck.Name}");
        Console.WriteLine($"Number of cards: {deck.WeakCards.Count}");
        Console.WriteLine("(Need to answer correctly 5 times in a row)\n");

        var weakCardsCopy = deck.WeakCards.ToList();
        
        foreach (var card in weakCardsCopy)
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
        
        FileManager.SaveDeck(deck);
        Console.WriteLine($"Weak cards remaining: {deck.WeakCards.Count}\n");
    }
}