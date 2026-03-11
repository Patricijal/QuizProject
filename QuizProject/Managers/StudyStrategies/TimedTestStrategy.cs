using System;
using System.Diagnostics;

public class TimedTestStrategy : IStudyStrategy
{
    public void Run(Deck deck)
    {
        if (!TryGetTimeLimit(out int timeLimit)) return;

        Console.WriteLine($"\nTimed Test: {timeLimit}s");
        Console.WriteLine("Test will start in 3 seconds...\n");
        System.Threading.Thread.Sleep(3000);

        var stopwatch = Stopwatch.StartNew();
        var (answered, correct) = RunTimedLoop(deck, stopwatch, timeLimit);
        stopwatch.Stop();

        PrintResults(stopwatch, answered, correct, deck.Cards.Count);
    }

    private bool TryGetTimeLimit(out int timeLimit)
    {
        Console.Write("Enter time limit in seconds: ");
        if (!int.TryParse(Console.ReadLine(), out timeLimit) || timeLimit <= 0)
        {
            Console.WriteLine("Invalid time limit.\n");
            return false;
        }
        return true;
    }

    private (int answered, int correct) RunTimedLoop(Deck deck, Stopwatch stopwatch, int timeLimit)
    {
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

        return (answered, correct);
    }

    private void PrintResults(Stopwatch stopwatch, int answered, int correct, int total)
    {
        Console.WriteLine($"\nTest completed in {stopwatch.Elapsed.TotalSeconds:F1}s ");
        Console.WriteLine($"Answered: {answered}/{total}");
        Console.WriteLine($"Correct: {correct}/{answered} ({(answered > 0 ? (double)correct / answered * 100 : 0):F1}%)\n");
    }
}
