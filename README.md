# QuizProject

A `C#` console app for learning with flashcards, inspired by Anki-style study flows.

## Features

- Study cards in classic flashcard mode (press Enter to reveal answer)
- Multiple-choice test mode (4 options)
- Timed test mode
- Weak-cards mode with a `5`-correct-streak rule
- Deck management:
  - Create deck
  - Add/delete cards
  - View decks
  - Delete deck
  - Import/export deck JSON files
- Statistics per deck and overall progress
- Event notifications when:
  - a deck is created
  - a card is added

## Implemented C# Concepts

- `IEnumerable<T>` on `Deck`
- Custom `IEnumerator<T>` via `DeckCardEnumerator`
- Iterator method with `yield return`
- Extension methods for `Deck` and generic enumerable helpers
- Extension deconstructor for `Flashcard`
- Custom exception type: `InvalidDeckOperationException`
- Generic result wrapper: `OperationResult<T> where T : class`
- `ICloneable` on `Flashcard`
- LINQ across deck/statistics/query logic

## Tech Stack

- `.NET 10` (`net10.0`)
- `C# 14`
- JSON persistence via `System.Text.Json`

## Project Structure

- `Program.cs` – app entry point and main loop
- `Managers/` – deck, menu, study session, file, and statistics logic
- `Managers/StudyStrategies/` – pluggable study modes (`IStudyStrategy`)
- `Models/` – `Deck`, `Flashcard`, and `DeckCardEnumerator`
- `Extensions/` – `DeckExtensions`, `EnumerableExtensions`, `FlashcardExtensions`
- `Exceptions/` – custom exception types
- `Common/` – shared generic helpers (`OperationResult<T>`)
- `Decks/` – saved deck files (`*.json`)

## Getting Started

### Prerequisites

- .NET SDK `10.0` or newer

### Run

From the repository root:

```powershell
dotnet run --project QuizProject/QuizProject.csproj
```

## Data Storage

Decks are automatically saved as JSON files in the `Decks` folder. On startup, the app loads existing decks; if none are found, sample decks are created.

The app supports loading both:

- object-based deck JSON (`Name` + `Cards`)
- legacy array-only card JSON files (deck name falls back to filename)

## Main Menu

1. Anki flashcards
2. Multiple choice test
3. Timed test
4. Weak cards
5. Manage decks
6. Statistics
7. Save and exit

## Notes

- Multiple-choice mode requires cards with at least `4` options.
- Weak cards are removed after `5` consecutive correct answers in weak-cards mode.
- Build target: `net10.0`.
