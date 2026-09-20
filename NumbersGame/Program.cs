// --- CONFIGURATION ---
const int MaxGuesses = 5;
const int MinAnswer = 1;
const int MaxAnswer = 20;

var answer = Random.Shared.Next(MinAnswer, MaxAnswer + 1);

var infoMessage = $"Välkommen! Jag tänker på ett nummer. Kan du gissa vilket? Du får {MaxGuesses} försök.";
const string GuessPromptMessage = "Skriv in ett nummer: ";
const string InvalidInputMessage = "Ogiltig inmatning, försök igen. ";

// --- PROGRAM FLOW ---
Console.WriteLine(infoMessage);


var totalGuesses = 0;
while (true)
{
    var guess = GetIntegerInput(GuessPromptMessage);
    totalGuesses++;

    if (CheckGuess(guess, answer)) break;
    
    if (totalGuesses >= MaxGuesses)
    {
        Console.WriteLine($"Tyvärr, du lyckades inte gissa talet på {MaxGuesses} försök!");
        break;
    }
}

// --- METHODS ---
int GetIntegerInput(string message)
{
    while (true)
    {
        Console.Write(message);
        var input = Console.ReadLine();

        if (int.TryParse(input, out int result))
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
    else if (guess > answer)
    {
        Console.WriteLine("Tyvärr, du gissade för högt!");
    }

    Console.WriteLine();
    return false;
}