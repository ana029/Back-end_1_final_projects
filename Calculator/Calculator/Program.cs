using System;
class Calculator
{
    static void Main()
    {
        Console.WriteLine("Welcome to the Calculator!");

        while (true)
        {
            try
            {
                // Input numbers
                Console.Write("Enter the first number: ");
                double num1 = GetValidNumber();

                Console.Write("Enter the second number: ");
                double num2 = GetValidNumber();

                // Choose operation
                Console.WriteLine("Choose an operation (+, -, *, /): ");
                string operation = Console.ReadLine();

                // Perform operation
                double result = PerformOperation(num1, num2, operation);

                // Display result
                Console.WriteLine($"Result: {result}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }

            // Ask if user wants to continue
            Console.Write("Do you want to perform another calculation? (yes/no): ");
            string repeat = Console.ReadLine()?.Trim().ToLower();
            if (repeat != "yes" && repeat != "y")
            {
                break;
            }
        }

        Console.WriteLine("Goodbye!");
    }

    // Validate input number
    static double GetValidNumber()
    {
        while (true)
        {
            string input = Console.ReadLine();
            if (double.TryParse(input, out double number))
            {
                return number;
            }
            Console.WriteLine("Invalid input. Please try again: ");
        }
    }

    // Perform selected operation
    static double PerformOperation(double num1, double num2, string operation)
    {
        switch (operation)
        {
            case "+":
                return num1 + num2;
            case "-":
                return num1 - num2;
            case "*":
                return num1 * num2;
            case "/":
                if (num2 == 0)
                {
                    throw new DivideByZeroException("Division by zero is not allowed.");
                }
                return num1 / num2;
            default:
                throw new InvalidOperationException("Invalid operation. Please choose +, -, *, or /.");
        }
    }
}
