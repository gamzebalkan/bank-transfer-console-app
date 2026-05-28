using System;
using BankTransferConsoleApp.Data;
using BankTransferConsoleApp.Models;

namespace BankTransferConsoleApp.Services
{
    public class TransferService
    {
        private readonly CustomerRepository _repo;

        public TransferService(CustomerRepository repo)
        {
            _repo = repo;
        }

        public void StartTransferFlow(Customer sender)
        {
            Console.WriteLine("\nEnter transfer details.");

            Console.Write("Amount (USD): ");
            string amountText = Console.ReadLine() ?? "";

            if (!decimal.TryParse(amountText, out decimal amount))
            {
                Console.WriteLine("Invalid amount.");
                return;
            }

            if (amount <= 0)
            {
                Console.WriteLine("Amount must be greater than 0.");
                return;
            }

            Console.Write("Recipient Account No: ");
            string receiver = (Console.ReadLine() ?? "").Trim();

            Console.WriteLine("\nChecking recipient account...");

            if (string.IsNullOrWhiteSpace(receiver))
            {
                Console.WriteLine("Recipient cannot be empty.");
                return;
            }

            if (receiver == sender.CustomerNo)
            {
                Console.WriteLine("You cannot transfer money to yourself.");
                return;
            }

            Console.WriteLine("Validating recipient...");

            if (!_repo.CustomerExists(receiver))
            {
                Console.WriteLine("Recipient not found.");
                return;
            }
            
            Console.WriteLine("Processing transfer. Please wait...");

            bool success = _repo.TransferMoney(sender.CustomerNo, receiver, amount);

            if (!success)
            {
                Console.WriteLine("Transfer failed: insufficient balance.");
                return;
            }
            
            Console.WriteLine("Finalizing transaction...");

            var senderBalance = _repo.GetBalance(sender.CustomerNo);

            Console.WriteLine($"\nTransfer successful. {amount:F2} USD has been sent to recipient account {receiver}.");
            Console.WriteLine($"Your new balance: {senderBalance:F2} USD\n");
        }
    }
}