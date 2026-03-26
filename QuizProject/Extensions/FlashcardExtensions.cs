using System;

public static class FlashcardExtensions
{
    // You created an extension deconstructor (1 point)
    public static void Deconstruct(this Flashcard card, out string question, out string answer, out double successRate)
    {
        if (card == null)
        {
            throw new ArgumentNullException(nameof(card));
        }

        question = card.Question;
        answer = card.Answer;
        successRate = card.GetSuccessRate();
    }
}
