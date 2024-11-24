using System;
using System.Collections.Generic;

class Hangman
{
    static void Main()
    {
        Console.WriteLine("Welcome to Hangman!");
        Console.WriteLine("Try to guess the word one letter at a time. You have 6 attempts.");

        // Categories with word lists
        var categories = new Dictionary<string, string[]>
        {
            { "Fruits", new[] { "apple", "banana", "cherry", "grape", "kiwi", "lemon", "mango", "orange", "peach", "pear" } },
            { "Animals", new[] { "tiger", "elephant", "giraffe", "dolphin", "panda", "rabbit", "zebra", "kangaroo", "eagle", "monkey" } },
            { "Countries", new[] { "brazil", "canada", "france", "germany", "india", "japan", "mexico", "norway", "spain", "italy" } },
            { "Capitals", new[] { "paris", "london", "tokyo", "berlin", "madrid", "oslo", "ottawa", "rome", "delhi", "helsinki" } },
            { "Planets", new[] { "mercury", "venus", "earth", "mars", "jupiter", "saturn", "uranus", "neptune", "pluto"} }
        };

        // Choose a category
        string chosenCategory = ChooseCategory(categories);
        string[] wordList = categories[chosenCategory];

        Random random = new Random();
        string wordToGuess = wordList[random.Next(wordList.Length)]; // Randomly select a word
        char[] guessedWord = new string('_', wordToGuess.Length).ToCharArray();
        List<char> guessedLetters = new List<char>();
        int attemptsLeft = 6;

        while (attemptsLeft > 0 && new string(guessedWord) != wordToGuess)
        {
            Console.WriteLine($"\nCategory: {chosenCategory}");
            Console.WriteLine($"Word: {new string(guessedWord)}");
            Console.WriteLine($"Attempts left: {attemptsLeft}");
            Console.WriteLine($"Guessed letters: {string.Join(", ", guessedLetters)}");

            Console.Write("Guess a letter: ");
            string input = Console.ReadLine();

            // Validate input
            if (string.IsNullOrEmpty(input) || input.Length != 1 || !char.IsLetter(input[0]))
            {
                Console.WriteLine("Invalid input. Please enter a single letter.");
                continue;
            }

            char guessedLetter = char.ToLower(input[0]);

            if (guessedLetters.Contains(guessedLetter))
            {
                Console.WriteLine("You've already guessed that letter. Try another one.");
                continue;
            }

            guessedLetters.Add(guessedLetter);

            if (wordToGuess.Contains(guessedLetter))
            {
                Console.WriteLine("Good guess!");

                for (int i = 0; i < wordToGuess.Length; i++)
                {
                    if (wordToGuess[i] == guessedLetter)
                    {
                        guessedWord[i] = guessedLetter;
                    }
                }
            }
            else
            {
                Console.WriteLine("Wrong guess!");
                attemptsLeft--;
                DisplayHangman(6 - attemptsLeft);
            }
        }

        if (new string(guessedWord) == wordToGuess)
        {
            Console.WriteLine($"\nCongratulations! You've guessed the word: {wordToGuess}");
        }
        else
        {
            Console.WriteLine($"\nGame over! The correct word was: {wordToGuess}");
            DisplayHangman(6);
        }
    }

    // Choose category
    static string ChooseCategory(Dictionary<string, string[]> categories)
    {
        Console.WriteLine("\nChoose a category:");
        int index = 1;
        foreach (var category in categories.Keys)
        {
            Console.WriteLine($"{index}. {category}");
            index++;
        }

        while (true)
        {
            Console.Write("Enter the number of your choice: ");
            string input = Console.ReadLine();

            if (int.TryParse(input, out int choice) && choice >= 1 && choice <= categories.Count)
            {
                int categoryIndex = 0;
                foreach (var category in categories.Keys)
                {
                    if (++categoryIndex == choice)
                    {
                        return category;
                    }
                }
            }
            Console.WriteLine("Invalid choice. Please try again.");
        }
    }

    // Display Hangman
    static void DisplayHangman(int phase)
    {
        string[] hangmanStages = {
            @"
             -----
             |   |
                 |
                 |
                 |
                 |
            =========",
            @"
             -----
             |   |
             O   |
                 |
                 |
                 |
            =========",
            @"
             -----
             |   |
             O   |
             |   |
                 |
                 |
            =========",
            @"
             -----
             |   |
             O   |
            /|   |
                 |
                 |
            =========",
            @"
             -----
             |   |
             O   |
            /|\  |
                 |
                 |
            =========",
            @"
             -----
             |   |
             O   |
            /|\  |
            /    |
                 |
            =========",
            @"
             -----
             |   |
             O   |
            /|\  |
            / \  |
                 |
            ========="
        };

        Console.WriteLine(hangmanStages[phase]);
    }
}
