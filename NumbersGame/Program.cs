// --- CONFIGURATION ---
const int MaxAttempts = 5;
const int MinRandomNumber = 1;
const int MaxRandomNumber = 20;

int answer = Random.Shared.Next(MinRandomNumber, MaxRandomNumber + 1);

string infoMessage = $"Välkommen! Jag tänker på ett nummer. Kan du gissa vilket? Du får {MaxAttempts} försök.";
const string GuessPromptMessage = "Skriv in ett nummer: ";
const string InvalidInputMessage = "Ogiltig inmatning, försök igen. ";

// --- PROGRAM FLOW ---
Console.WriteLine(infoMessage);


var attempts = 0;
while (true)
{
    var guess = GetValidGuessInput(GuessPromptMessage);
    attempts++;

    if (CheckGuess(guess, answer)) break;
    
    if (attempts >= MaxAttempts)
    {
        Console.WriteLine($"Tyvärr, du lyckades inte gissa talet på {MaxAttempts} försök!");
        break;
    }
}

// --- METHODS ---
int GetValidGuessInput(string message)
{
    while (true)
    {
        Console.Write(message);
        var input = Console.ReadLine();

        if (int.TryParse(input, out int result) && (result >= MinRandomNumber || result <= MaxRandomNumber))
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