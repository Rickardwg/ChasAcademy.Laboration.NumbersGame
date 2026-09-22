// --- CONFIGURATION ---
var easy = new DifficultySettings("enkel", 1, 10, 6);
var medium = new DifficultySettings("mellan", 1, 25, 5);
var hard = new DifficultySettings("svår", 1, 50, 3);

const string GuessPromptMessage = "Skriv in ett nummer: ";
const string InvalidInputMessage = "Ogiltig inmatning, försök igen. ";
const string YesResponse = "j";
const string NoResponse = "n";

var difficultyPromptMessage = $"Välj svårighetsgrad ({easy.DisplayName}/{medium.DisplayName}/{hard.DisplayName}): ";
var replayPromptMessage = $"Vill du spela igen? ({YesResponse}/{NoResponse}): ";

var tooLowMessages = new List<string>
{
    "Tyvärr, du gissade för lågt!",
    "Haha! Det var för lågt!",
    "Bra gissat, men det var för lågt!",
    "FÖR LÅÅÅÅGT!"
};

var tooHighMessages = new List<string>
{
    "Tyvärr, du gissade för högt!",
    "Haha! Det var för högt!",
    "Bra gissat, men det var för högt!",
    "FÖR HÖÖÖÖGT!"
};

// --- PROGRAM FLOW ---
while (true) // REPLAY LOOP
{
    var difficulty = SelectDifficulty(difficultyPromptMessage, easy, medium, hard);
    var answer = Random.Shared.Next(difficulty.MinNumber, difficulty.MaxNumber + 1);

    Console.WriteLine();
    var infoMessage = $"Välkommen! Jag tänker på ett nummer ({difficulty.MinNumber}-{difficulty.MaxNumber}). Kan du gissa vilket? Du får {difficulty.MaxAttempts} försök.";
    Console.WriteLine(infoMessage);

    var attempts = 0;
    while (true) // GAME LOOP
    {
        var guess = GetValidGuessInput(GuessPromptMessage, difficulty);
        attempts++;
        var result = CheckGuess(guess, answer);

        var message = result switch
        {
            GuessResult.Correct => "Wohoo! Du klarade det!",
            GuessResult.TooLow => GetRandomMessage(tooLowMessages),
            GuessResult.TooHigh => GetRandomMessage(tooHighMessages),
            _ => throw new ArgumentOutOfRangeException() // Unhandled
        };

        Console.WriteLine(message);
        Console.WriteLine();

        if (result == GuessResult.Correct) break;

        if (attempts >= difficulty.MaxAttempts)
        {
            Console.WriteLine($"Tyvärr, du lyckades inte gissa talet på {difficulty.MaxAttempts} försök!");
            break;
        }
    }

    if (!GetReplayResponse(ReplayPromptMessage)) break;

    Console.WriteLine();
}

// --- METHODS ---
DifficultySettings SelectDifficulty(string promptMessage, DifficultySettings easy, DifficultySettings medium, DifficultySettings hard)
{
    while (true)
    {
        Console.Write(promptMessage);
        var input = Console.ReadLine();

        if (string.Equals(input, easy.DisplayName, StringComparison.OrdinalIgnoreCase)) return easy;

        if (string.Equals(input, medium.DisplayName, StringComparison.OrdinalIgnoreCase)) return medium;

        if (string.Equals(input, hard.DisplayName, StringComparison.OrdinalIgnoreCase)) return hard;

        Console.Write(InvalidInputMessage);
    }
}

int GetValidGuessInput(string promptMessage, DifficultySettings settings)
{
    while (true)
    {
        Console.Write(promptMessage);
        var input = Console.ReadLine();

        if (int.TryParse(input, out int result) && 
            result >= settings.MinNumber && 
            result <= settings.MaxNumber)
        {
            return result;
        }

        Console.Write(InvalidInputMessage);
    }
}

bool GetReplayResponse(string promptMessage)
{
    while (true)
    {
        Console.Write(promptMessage);
        var input = Console.ReadLine();

        if (string.Equals(input, YesResponse, StringComparison.OrdinalIgnoreCase)) return true;
        if (string.Equals(input, NoResponse, StringComparison.OrdinalIgnoreCase)) return false;

        Console.Write(InvalidInputMessage);
    }
}

GuessResult CheckGuess(int guess, int answer)
{
    if (guess == answer) return GuessResult.Correct;

    if (guess < answer) return GuessResult.TooLow;

    return GuessResult.TooHigh;
}

string GetRandomMessage(List<string> messages) // .Count == 0 is unhandled
{
    return messages[Random.Shared.Next(messages.Count)];
}


// --- DOMAIN MODELS ---
record DifficultySettings
(
    string DisplayName,
    int MinNumber,
    int MaxNumber,
    int MaxAttempts
);

enum GuessResult
{
    Correct,
    TooLow,
    TooHigh
}