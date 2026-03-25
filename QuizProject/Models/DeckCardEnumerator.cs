using System;
using System.Collections;
using System.Collections.Generic;

public class DeckCardEnumerator : IEnumerator<Flashcard>
{
    private readonly List<Flashcard> _cards;
    private int _position = -1;

    public DeckCardEnumerator(List<Flashcard> cards)
    {
        _cards = cards ?? throw new ArgumentNullException(nameof(cards));
    }

    public Flashcard Current
    {
        get
        {
            if (_position < 0 || _position >= _cards.Count)
            {
                throw new InvalidOperationException();
            }

            return _cards[_position];
        }
    }

    object IEnumerator.Current => Current;

    public bool MoveNext()
    {
        if (_position + 1 >= _cards.Count)
        {
            return false;
        }

        _position++;
        return true;
    }

    public void Reset()
    {
        _position = -1;
    }

    public void Dispose()
    {
    }
}
