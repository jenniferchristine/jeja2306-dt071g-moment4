using System;
using ReviewClassifier.Services;

namespace ReviewClassifier
{
    class Program
    {
        static void Main(string[] args)
        {
            bool active = true;

            while (active)
            {
                Console.Clear();
                Console.WriteLine("Please write a review:\n");
                string? inputReview = Console.ReadLine();

                if (!string.IsNullOrEmpty(inputReview))
                {
                    string prediction = ModelService.ClassifyReview(inputReview);
                    Console.WriteLine($"\nYour review was deemed {prediction}!");
                }
                else
                {
                    Console.WriteLine("No review was entered");
                }

                Console.WriteLine("\nPress X to exit or any other button to try again\n");

                ConsoleKey key = Console.ReadKey(true).Key;

                if (key == ConsoleKey.X)
                {
                    active = false;
                    Console.WriteLine("Exiting program...");
                }
                else
                {
                    continue;
                }
            }
        }
    }
}

