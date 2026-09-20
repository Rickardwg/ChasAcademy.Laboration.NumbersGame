// --- CONFIGURATION ---
var easy = new DifficultySettings("enkel", 1, 10, 6);
var medium = new DifficultySettings("mellan", 1, 25, 5);
var hard = new DifficultySettings("svår", 1, 50, 3);

const string GuessPromptMessage = "Skriv in ett nummer: ";
const string InvalidInputMessage = "Ogiltig inmatning, försök igen. ";
var difficultyPromptMessage = $"Välj svårighetsgrad ({easy.DisplayName}/{medium.DisplayName}/{hard.DisplayName}): ";

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
var difficulty = SelectDifficulty(difficultyPromptMessage, easy, medium, hard);
var answer = Random.Shared.Next(difficulty.MinNumber, difficulty.MaxNumber + 1);

Console.WriteLine();
var infoMessage = $"Välkommen! Jag tänker på ett nummer ({difficulty.MinNumber}-{difficulty.MaxNumber}). Kan du gissa vilket? Du får {difficulty.MaxAttempts} försök.";
Console.WriteLine(infoMessage);

var attempts = 0;
while (true)
{
    var guess = GetValidGuessInput(GuessPromptMessage, difficulty);
    attempts++;
    var result = CheckGuess(guess, answer);

    var message = result switch
    {
        GuessResult.Correct => "Wohoo! Du klarade det!",
        GuessResult.TooLow  => GetRandomMessage(tooLowMessages),
        GuessResult.TooHigh => GetRandomMessage(tooHighMessages),
        _                   => throw new ArgumentOutOfRangeException() // Unhandled.
    };

    Console.WriteLine(message);

    if (result == GuessResult.Correct) break;
    
    if (attempts >= difficulty.MaxAttempts)
    {
        Console.WriteLine($"Tyvärr, du lyckades inte gissa talet på {difficulty.MaxAttempts} försök!");
        break;
    }
}

// --- METHODS ---
DifficultySettings SelectDifficulty(string promptMessage, DifficultySettings easy, DifficultySettings medium, DifficultySettings hard)
{
    while (true)
    {
        Console.Write(promptMessage);
        var input = Console.ReadLine();

        if (string.Equals(input, easy.DisplayName, StringComparison.OrdinalIgnoreCase)) return easy;

        else if (string.Equals(input, medium.DisplayName, StringComparison.OrdinalIgnoreCase)) return medium;

        else if (string.Equals(input, hard.DisplayName, StringComparison.OrdinalIgnoreCase)) return hard;

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

GuessResult CheckGuess(int guess, int answer)
{
    if (guess == answer) return GuessResult.Correct;

    else if (guess < answer) return GuessResult.TooLow;

    return GuessResult.TooHigh;
}

string GetRandomMessage(List<string> messages)
{
    return messages[Random.Shared.Next(messages.Count)];
}


// --- DOMAINS ---
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