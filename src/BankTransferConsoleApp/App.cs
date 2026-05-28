using System;
using BankTransferConsoleApp.Models;
using BankTransferConsoleApp.Services;
using BankTransferConsoleApp.Data;

namespace BankTransferConsoleApp
{
    public class App
    {
        private readonly AuthService _authService;
        private readonly TransferService _transferService;
        private readonly CustomerRepository _repo;

        public App()
        {
            const string connectionString =
                "Host=aws-1-ap-northeast-1.pooler.supabase.com;Port=5432;Database=postgres;Username=postgres.ucihvmmkfzptmycdaprf;Password=bXZveW3aIKp6C11T;SSL Mode=Require;Trust Server Certificate=true; Timeout=30; Command Timeout=30";

            _repo = new CustomerRepository(connectionString);

            _authService = new AuthService(_repo);
            _transferService = new TransferService(_repo);
        }

        public void Run()
        {
            Customer? user = _authService.Login();

            if (user == null)
            {
                Console.WriteLine("\nLogin failed. Press Enter to exit...");
                Console.ReadLine();
                return;
            }

            Console.WriteLine($"\nLogin successful. Welcome, {user.FullName}!");

            int attempts = 0;
            const int maxAttempts = 3;

            while (true)
            {
                Console.WriteLine("1. View Balance");
                Console.WriteLine("2. Bank Transfer (Domestic)");
                Console.WriteLine("3. International Transfer (SWIFT)");
                Console.WriteLine("4. Exit");
                Console.Write("-->: ");

                string? choice = Console.ReadLine();

                if (choice == "1")
                {
                    decimal balance = _repo.GetBalance(user.CustomerNo);
                    Console.WriteLine($"\nCurrent balance: {balance:F2} USD");
                }
                else if (choice == "2")
                {
                    _transferService.StartTransferFlow(user);
                }
                else if (choice == "3")
                {
                    Console.WriteLine("\nInternational Transfer (SWIFT) is not available yet.");
                }
                else if (choice == "4")
                {
                    Console.WriteLine("\nGoodbye!");
                    break;
                }
                else
                {
                    attempts++;
                    Console.WriteLine($"\nInvalid choice. Attempts left: {maxAttempts - attempts}");

                    if (attempts >= maxAttempts)
                    {
                        Console.WriteLine("\nToo many invalid attempts. Exiting...");
                        break;
                    }
                }
            }

            Console.WriteLine("\nPress Enter to exit...");
            Console.ReadLine();
        }
    }
}