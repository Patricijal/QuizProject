using System;

public class StudySessionManager
{
    private readonly DeckManager _deckManager;

    public StudySessionManager(DeckManager deckManager)
    {
        _deckManager = deckManager;
    }

    // Initialization using out arguments is implemented (1 point)
    private bool TrySelectDeck(out Deck deck)
    {
        deck = _deckManager.SelectDeck();
        return deck != null;
    }

    public void StartSession(IStudyStrategy mode)
    {
        if (!TrySelectDeck(out var deck)) return;

        if (deck.Cards.Count == 0)
        {
            Console.WriteLine("\nDeck is empty!\n");
            return;
        }

        mode.Run(deck);
        FileManager.SaveDeck(deck);
    }

    public void StudyFlashcards() => StartSession(new FlashcardStudyStrategy());
    public void MultipleChoiceTest() => StartSession(new MultipleChoiceStrategy());
    public void TimedTest() => StartSession(new TimedTestStrategy());
    public void StudyWeakCards() => StartSession(new WeakCardsStrategy());
}