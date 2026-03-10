using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

public class Flashcard : IComparable<Flashcard>, IEquatable<Flashcard>, IFormattable
{
    public string Question { get; set; }
    public string Answer { get; set; }
    public List<string> MultipleChoiceOptions { get; set; }
    public int TimesReviewed { get; set; }
    public int CorrectCount { get; set; }
    public int ConsecutiveCorrect { get; set; }

    public Flashcard()
    {
        Question = string.Empty;
        Answer = string.Empty;
        MultipleChoiceOptions = new List<string>();
        TimesReviewed = 0;
        CorrectCount = 0;
        ConsecutiveCorrect = 0;
    }

    public Flashcard(string question, string answer, List<string> options = null)
    {
        Question = question;
        Answer = answer;
        MultipleChoiceOptions = options ?? new List<string>();
        TimesReviewed = 0;
        CorrectCount = 0;
        ConsecutiveCorrect = 0;
    }

    [JsonConstructor]
    public Flashcard(string question, string answer, List<string> multipleChoiceOptions, int timesReviewed, int correctCount, int consecutiveCorrect)
    {
        Question = question;
        Answer = answer;
        MultipleChoiceOptions = multipleChoiceOptions ?? new List<string>();
        TimesReviewed = timesReviewed;
        CorrectCount = correctCount;
        ConsecutiveCorrect = consecutiveCorrect;
    }

    public double GetSuccessRate()
    {
        return TimesReviewed == 0 ? 0 : (double)CorrectCount / TimesReviewed * 100;
    }

    // You correctly implemented IComparable<T> (0.5 points)
    public int CompareTo(Flashcard other)
    {
        if (other == null) return 1;
        return GetSuccessRate().CompareTo(other.GetSuccessRate());
    }

    // You correctly implemented IEquatable<T> (0.5 points)
    public bool Equals(Flashcard other)
    {
        if (other == null) return false;
        return Question == other.Question && Answer == other.Answer;
    }

    public override bool Equals(object obj) => Equals(obj as Flashcard);
    public override int GetHashCode() => HashCode.Combine(Question, Answer);

    // You correctly implemented IFormattable (1 point)
    public string ToString(string format, IFormatProvider formatProvider) => format switch
    {
        "Q" => Question,
        "A" => Answer,
        "S" => $"{Question} ({GetSuccessRate():F1}%)",
        _ => $"Q: {Question} | A: {Answer}"
    };

    public void Deconstruct(out string question, out string answer)
    {
        question = Question;
        answer = Answer;
    }
}