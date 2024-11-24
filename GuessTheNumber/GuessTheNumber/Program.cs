using System;

class GuessTheNumberWithLives
{
    static void Main()
    {
        Console.WriteLine("Welcome to the 'Guess the Number' game with limited lives!");
        Console.WriteLine("I am thinking of a number between 1 and 100. Can you guess it?");
        Console.WriteLine("You have 5 lives. Let's begin!");

        Random random = new Random();
        int targetNumber = random.Next(1, 101); 
        int lives = 5; 
        int attempts = 0; 

        while (lives > 0)
        {
            try
            {
                Console.Write("Enter your guess: ");
                int userGuess = int.Parse(Console.ReadLine());
                attempts++;

                if (userGuess < 1 || userGuess > 100)
                {
                    Console.WriteLine("Your guess is out of range! Please enter a number between 1 and 100.");
                    continue;
                }

                if (userGuess > targetNumber)
                {
                    lives--;
                    Console.WriteLine($"Too high! Try a lower number. Lives remaining: {lives}");
                }
                else if (userGuess < targetNumber)
                {
                    lives--;
                    Console.WriteLine($"Too low! Try a higher number. Lives remaining: {lives}");
                }
                else
                {
                    Console.WriteLine($"Congratulations! You guessed the number {targetNumber} in {attempts} attempts.");
                    break;
                }
            }
            catch (FormatException)
            {
                Console.WriteLine("Invalid input! Please enter a valid number.");
            }

            if (lives == 0)
            {
                Console.WriteLine($"Game over! You've run out of lives. The correct number was {targetNumber}.");
            }
        }

        Console.WriteLine("Thanks for playing! Goodbye!");
    }
}
