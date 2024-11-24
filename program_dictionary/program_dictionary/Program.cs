using System;
using System.Collections.Generic;
using System.IO;

class TranslatorApp
{
    static void Main()
    {
        // Language pair file names
        string englishFrenchFile = "english_french.txt";
        string frenchEnglishFile = "french_english.txt";
        string englishSpanishFile = "english_spanish.txt";
        string spanishEnglishFile = "spanish_english.txt";
        string englishGeorgianFile = "english_georgian.txt";

        CreateFileIfNotExists(englishFrenchFile);
        CreateFileIfNotExists(frenchEnglishFile);
        CreateFileIfNotExists(englishSpanishFile);
        CreateFileIfNotExists(spanishEnglishFile);
        CreateFileIfNotExists(englishGeorgianFile);

        Console.WriteLine("Welcome to the Translator!");
        Console.WriteLine("Choose the language pair for translation:");
        Console.WriteLine("1. English-French");
        Console.WriteLine("2. French-English");
        Console.WriteLine("3. English-Spanish");
        Console.WriteLine("4. Spanish-English");
        Console.WriteLine("5. English-Georgian");

        string choice = Console.ReadLine();
        string languagePair = GetLanguagePair(choice);

        if (string.IsNullOrEmpty(languagePair))
        {
            Console.WriteLine("Invalid choice. Exiting program.");
            return;
        }

        string selectedFile = GetFileForLanguagePair(languagePair);

        while (true)
        {
            Console.Write("\nEnter a word or phrase to translate (or type 'exit' to quit): ");
            string input = Console.ReadLine().Trim();

            if (input.ToLower() == "exit")
            {
                break;
            }

            string translation = FindTranslation(selectedFile, languagePair, input);

            if (translation != null)
            {
                Console.WriteLine($"Translation: {translation}");
            }
            else
            {
                Console.WriteLine("Word not found in the dictionary.");
                Console.Write("Would you like to add a translation for this word? (yes/no): ");
                string response = Console.ReadLine().Trim().ToLower();

                if (response == "yes" || response == "y")
                {
                    Console.Write("Enter the translation: ");
                    string newTranslation = Console.ReadLine().Trim();
                    AddTranslation(selectedFile, languagePair, input, newTranslation);
                    Console.WriteLine("Translation added to the dictionary.");
                }
            }
        }

        Console.WriteLine("Goodbye!");
    }

    static string GetLanguagePair(string choice)
    {
        switch (choice)
        {
            case "1":
                return "English-French";
            case "2":
                return "French-English";
            case "3":
                return "English-Spanish";
            case "4":
                return "Spanish-English";
            case "5":
                return "English-Georgian";
            default:
                return null;
        }
    }

    static string GetFileForLanguagePair(string languagePair)
    {
        switch (languagePair)
        {
            case "English-French":
                return "english_french.txt";
            case "French-English":
                return "french_english.txt";
            case "English-Spanish":
                return "english_spanish.txt";
            case "Spanish-English":
                return "spanish_english.txt";
            case "English-Georgian":
                return "english_georgian.txt";
            default:
                return null;
        }
    }

    static void CreateFileIfNotExists(string filePath)
    {
        if (!File.Exists(filePath))
        {
            File.Create(filePath).Close();
        }
    }

    static string FindTranslation(string filePath, string languagePair, string word)
    {
        foreach (string line in File.ReadLines(filePath))
        {
            string[] parts = line.Split('|');
            if (parts.Length == 3 && parts[0] == languagePair && parts[1] == word)
            {
                return parts[2];
            }
        }
        return null;
    }

    static void AddTranslation(string filePath, string languagePair, string word, string translation)
    {
        using (StreamWriter writer = File.AppendText(filePath))
        {
            writer.WriteLine($"{languagePair}|{word}|{translation}");
        }
    }
}
