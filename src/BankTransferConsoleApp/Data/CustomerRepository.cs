using System;
using Npgsql;
using BankTransferConsoleApp.Models;

namespace BankTransferConsoleApp.Data
{
    public class CustomerRepository
    {
        private readonly string _connectionString;

        public CustomerRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        private NpgsqlConnection CreateConnection()
        {
            return new NpgsqlConnection(_connectionString);
        }

        // Login query

        public Customer? GetCustomerByLogin(string customerNo, string password)
        {
            using var conn = CreateConnection();
            conn.Open();

            using var cmd = new NpgsqlCommand(
                @"SELECT customer_no, full_name, balance
                  FROM public.customers
                  WHERE customer_no = @customerNo AND password = @password;",
                conn);

            cmd.CommandTimeout = 30;

            cmd.Parameters.AddWithValue("customerNo", customerNo);
            cmd.Parameters.AddWithValue("password", password);

            using var reader = cmd.ExecuteReader();

            if (reader.Read())
            {
                return new Customer
                {
                    CustomerNo = reader.GetString(0),
                    FullName = reader.GetString(1),
                    Balance = reader.GetDecimal(2)
                };
            }

            return null;
        }

        // Check customer exists

        public bool CustomerExists(string customerNo)
        {
            using var conn = CreateConnection();
            conn.Open();

            using var cmd = new NpgsqlCommand(
                @"SELECT 1
                  FROM public.customers
                  WHERE customer_no = @customerNo;",
                conn);

            cmd.CommandTimeout = 30;

            cmd.Parameters.AddWithValue("customerNo", customerNo);

            var result = cmd.ExecuteScalar();
            return result != null;
        }


        // Get customer
        public Customer? GetCustomerByCustomerNo(string customerNo)
        {
            using var conn = CreateConnection();
            conn.Open();

            using var cmd = new Npgsql.NpgsqlCommand(
                @"SELECT customer_no, full_name, balance, password_hash
                FROM public.customers
                WHERE customer_no = @customerNo;",
                conn);

            cmd.Parameters.AddWithValue("customerNo", customerNo);
            cmd.CommandTimeout = 30;

            using var reader = cmd.ExecuteReader();

            if (!reader.Read())
                return null;

            return new Customer
            {
                CustomerNo = reader.GetString(0),
                FullName = reader.GetString(1),
                Balance = reader.GetDecimal(2),
                PasswordHash = reader.GetString(3)
            };
        }

       // Get balance

        public decimal GetBalance(string customerNo)
        {
            using var conn = CreateConnection();
            conn.Open();

            using var cmd = new NpgsqlCommand(
                @"SELECT balance
                  FROM public.customers
                  WHERE customer_no = @customerNo;",
                conn);

            cmd.CommandTimeout = 30;

            cmd.Parameters.AddWithValue("customerNo", customerNo);

            var result = cmd.ExecuteScalar();
            return result is decimal d ? d : 0m;
        }


        // Transfer money

        public bool TransferMoney(string senderNo, string receiverNo, decimal amount)
        {
            using var conn = CreateConnection();
            conn.Open();

            using var tx = conn.BeginTransaction();

            try
            {
                // Console.WriteLine("DEBUG: Starting transfer transaction...");


                // Debit sender
                using (var debitCmd = new NpgsqlCommand(
                    @"UPDATE public.customers
                      SET balance = balance - @amount
                      WHERE customer_no = @senderNo AND balance >= @amount;",
                    conn, tx))
                {
                    debitCmd.CommandTimeout = 30;

                    debitCmd.Parameters.AddWithValue("amount", amount);
                    debitCmd.Parameters.AddWithValue("senderNo", senderNo);

                    int affected = debitCmd.ExecuteNonQuery();

                    // Console.WriteLine($"DEBUG: Debit affected rows = {affected}");

                    if (affected == 0)
                    {
                        tx.Rollback();
                        // Console.WriteLine("DEBUG: Debit failed → rollback");
                        return false;
                    }
                }

                // Credit receiver
                using (var creditCmd = new NpgsqlCommand(
                    @"UPDATE public.customers
                      SET balance = balance + @amount
                      WHERE customer_no = @receiverNo;",
                    conn, tx))
                {
                    creditCmd.CommandTimeout = 30;

                    creditCmd.Parameters.AddWithValue("amount", amount);
                    creditCmd.Parameters.AddWithValue("receiverNo", receiverNo);

                    int affected = creditCmd.ExecuteNonQuery();

                    // Console.WriteLine($"DEBUG: Credit affected rows = {affected}");

                    if (affected == 0)
                    {
                        tx.Rollback();
                        // Console.WriteLine("DEBUG: Credit failed → rollback");
                        return false;
                    }
                }

                tx.Commit();

                // Console.WriteLine("DEBUG: Transaction committed successfully");

                return true;
            }
            catch (Exception ex)
            {
                //Console.WriteLine("DEBUG: Exception occurred → rollback");
                Console.WriteLine(ex.Message);

                tx.Rollback();
                throw;
            }
        }
    }
}