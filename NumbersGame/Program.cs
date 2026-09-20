// --- CONFIGURATION ---
var easy = new DifficultySettings("enkel", 1, 10, 6);
var medium = new DifficultySettings("mellan", 1, 25, 5);
var hard = new DifficultySettings("svår", 1, 50, 3);



string difficultyPromptMessage = $"Välj svårighetsgrad ({easy.DisplayName}/{medium.DisplayName}/{hard.DisplayName}): ";
const string GuessPromptMessage = "Skriv in ett nummer: ";
const string InvalidInputMessage = "Ogiltig inmatning, försök igen. ";

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

    if (CheckGuess(guess, answer)) break;
    
    if (attempts >= difficulty.MaxAttempts)
    {
        Console.WriteLine($"Tyvärr, du lyckades inte gissa talet på {difficulty.MaxAttempts} försök!");
        break;
    }
}

// --- METHODS ---
DifficultySettings SelectDifficulty(string message, DifficultySettings easy, DifficultySettings medium, DifficultySettings hard)
{
    while (true)
    {
        Console.Write(message);

        var input = Console.ReadLine();

        if (string.Equals(input, easy.DisplayName, StringComparison.OrdinalIgnoreCase)) return easy;

        else if (string.Equals(input, medium.DisplayName, StringComparison.OrdinalIgnoreCase)) return medium;

        else if (string.Equals(input, hard.DisplayName, StringComparison.OrdinalIgnoreCase)) return hard;

        Console.Write(InvalidInputMessage);
    }
}

int GetValidGuessInput(string message, DifficultySettings settings)
{
    while (true)
    {
        Console.Write(message);
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

bool CheckGuess(int guess, int answer)
{
    if (guess == answer)
    {
        Console.WriteLine("Wohoo! Du klarade det!");
        return true;
    }
    else if (guess < answer)
    {
        Console.WriteLine("Tyvärr, du gissade för lågt!");
    }
    else
    {
        Console.WriteLine("Tyvärr, du gissade för högt!");
    }

    Console.WriteLine();
    return false;
}

// --- DOMAINS ---

record DifficultySettings
(
    string DisplayName,
    int MinNumber,
    int MaxNumber,
    int MaxAttempts
);