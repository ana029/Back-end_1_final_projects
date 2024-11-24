using System;
using System.Collections.Generic;
using System.IO;

namespace ATM
{
    class Program
    {
        static void Main(string[] args)
        {
            string directoryPath = "users"; 
            Directory.CreateDirectory(directoryPath); 
            bool running = true;

            while (running)
            {
                Console.WriteLine("\n--- ATM ---");
                Console.WriteLine("1. Register");
                Console.WriteLine("2. Login");
                Console.WriteLine("3. Exit");
                Console.Write("Select an option: ");
                string option = Console.ReadLine();

                switch (option)
                {
                    case "1":
                        RegisterUser(directoryPath); 
                        break;
                    case "2":
                        LoginUser(directoryPath); 
                        break;
                    case "3":
                        running = false;
                        Console.WriteLine("Thank you for using the ATM!");
                        break;
                    default:
                        Console.WriteLine("Invalid option. Try again.");
                        break;
                }
            }
        }

        static void RegisterUser(string directoryPath)
        {
            Console.Write("Enter your username: ");
            string username = Console.ReadLine().Trim();
            string userFilePath = Path.Combine(directoryPath, $"{username}.txt");

            // Check if the username already exists
            if (File.Exists(userFilePath))
            {
                Console.WriteLine("Username already exists. Try another one.");
                return;
            }

            Console.Write("Enter your PIN: ");
            string pinCode = Console.ReadLine().Trim();

            Console.Write("Enter your initial balance: ");
            string balanceInput = Console.ReadLine().Trim();
            decimal initialBalance;
            if (!decimal.TryParse(balanceInput, out initialBalance) || initialBalance < 0)
            {
                Console.WriteLine("Invalid balance. Please try again.");
                return;
            }

            // Write username, PIN, and balance to the file
            File.WriteAllText(userFilePath, $"{username}\n{pinCode}\n{initialBalance}");
            LogAction($"User {username} registered with initial balance of {initialBalance} units.");
            Console.WriteLine("User registered successfully!");
        }

        static void LoginUser(string directoryPath)
        {
            Console.Write("Enter your username: ");
            string username = Console.ReadLine().Trim();
            string userFilePath = Path.Combine(directoryPath, $"{username}.txt");

            if (!File.Exists(userFilePath))
            {
                Console.WriteLine("Username not found. Please register first.");
                return;
            }

            // Read user data from the file
            string[] userData = File.ReadAllLines(userFilePath);
            string storedUsername = userData[0]; // First line is the username
            string storedPinCode = userData[1]; // Second line is the PIN
            decimal balance = decimal.Parse(userData[2]); // Third line is the balance

            
            Console.Write("Enter your PIN: ");
            string enteredPin = Console.ReadLine().Trim();

            
            if (enteredPin != storedPinCode)
            {
                Console.WriteLine("Incorrect PIN. Try again.");
                return;
            }

            // User is logged in successfully
            Console.WriteLine($"Welcome {storedUsername}! Your current balance is {balance} units.");

            bool userRunning = true;
            while (userRunning)
            {
                Console.WriteLine("\n--- ATM Operations ---");
                Console.WriteLine("1. Check Balance");
                Console.WriteLine("2. Deposit Money");
                Console.WriteLine("3. Withdraw Money");
                Console.WriteLine("4. Change PIN");
                Console.WriteLine("5. Logout");
                Console.Write("Select an operation: ");
                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        CheckBalance(balance);
                        break;
                    case "2":
                        balance = Deposit(userFilePath, balance, storedUsername);
                        break;
                    case "3":
                        balance = Withdraw(userFilePath, balance, storedUsername);
                        break;
                    case "4":
                        balance = ChangePin(ref storedPinCode, userFilePath, balance, storedUsername);
                        break;
                    case "5":
                        userRunning = false;
                        Console.WriteLine("Logged out successfully.");
                        break;
                    default:
                        Console.WriteLine("Invalid choice. Please try again.");
                        break;
                }
            }
        }

        static void CheckBalance(decimal balance)
        {
            Console.WriteLine($"Your current balance: {balance} units.");
        }

        static decimal Deposit(string userFilePath, decimal balance, string username)
        {
            Console.Write("Enter the amount to deposit: ");
            if (decimal.TryParse(Console.ReadLine(), out decimal amount) && amount > 0)
            {
                balance += amount;
                File.WriteAllText(userFilePath, $"{username}\n{GetUserData(userFilePath)[1]}\n{balance}");
                LogAction($"User {username} deposited {amount} units. New balance: {balance}");
                Console.WriteLine($"You successfully deposited {amount} units. New balance: {balance}");
            }
            else
            {
                Console.WriteLine("Invalid amount. Please try again.");
            }
            return balance;
        }

        static decimal Withdraw(string userFilePath, decimal balance, string username)
        {
            Console.Write("Enter the amount to withdraw: ");
            if (decimal.TryParse(Console.ReadLine(), out decimal amount) && amount > 0)
            {
                if (amount <= balance)
                {
                    balance -= amount;
                    File.WriteAllText(userFilePath, $"{username}\n{GetUserData(userFilePath)[1]}\n{balance}");
                    LogAction($"User {username} withdrew {amount} units. Remaining balance: {balance}");
                    Console.WriteLine($"You successfully withdrew {amount} units. Remaining balance: {balance}");
                }
                else
                {
                    Console.WriteLine("Insufficient balance for withdrawal.");
                }
            }
            else
            {
                Console.WriteLine("Invalid amount. Please try again.");
            }
            return balance;
        }

        static decimal ChangePin(ref string pinCode, string userFilePath, decimal balance, string username)
        {
            Console.Write("Enter your current PIN: ");
            string currentPin = Console.ReadLine();

            if (currentPin == pinCode)
            {
                Console.Write("Enter your new PIN: ");
                string newPin = Console.ReadLine();
                Console.Write("Confirm your new PIN: ");
                string confirmPin = Console.ReadLine();

                if (newPin == confirmPin)
                {
                    pinCode = newPin;
                    // Update PIN in the file and write the balance
                    File.WriteAllText(userFilePath, $"{username}\n{pinCode}\n{balance}");
                    LogAction($"User {username} changed their PIN.");
                    Console.WriteLine("PIN successfully changed.");
                }
                else
                {
                    Console.WriteLine("PINs do not match. Try again.");
                }
            }
            else
            {
                Console.WriteLine("Incorrect current PIN. PIN change failed.");
            }

            return balance;
        }

        static string[] GetUserData(string userFilePath)
        {
            return File.ReadAllLines(userFilePath); 
        }

        static void LogAction(string action)
        {
            string logDirectory = "logs"; 
            Directory.CreateDirectory(logDirectory); 

            // Get today's date in format yyyy-MM-dd
            string logFileName = DateTime.Now.ToString("yyyy-MM-dd") + ".txt";
            string logFilePath = Path.Combine(logDirectory, logFileName);

            // Log the action with timestamp
            string logEntry = $"{DateTime.Now}: {action}";
            File.AppendAllText(logFilePath, logEntry + Environment.NewLine);
        }
    }
}
