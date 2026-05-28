# Bank Transfer Console App

A secure, console-based banking system built with **C# (.NET)** and **PostgreSQL (Supabase)**.

This project simulates real-world banking operations such as authentication, secure password handling, and transactional money transfers using a layered architecture.

---

## 🚀 Features

- 🔐 Secure login system using **BCrypt password hashing**
- 💰 View account balance
- 💸 Domestic bank transfers between customers
- ⚡ Transaction-safe operations using PostgreSQL transactions (ACID compliant)
- 🧱 Clean layered architecture (Models / Services / Data)
- ⌨ Console-based user interface
- 🛡 Input validation and retry mechanisms

---

## 🏗 Architecture

The project follows a simple layered architecture:

BankTransferConsoleApp/
│
├── Models/ → Data models (Customer)
├── Data/ → Database access (Repository layer)
├── Services/ → Business logic (Auth, Transfer)
├── App.cs → Application flow
├── Program.cs → Entry point



---

## 🔐 Security

- Passwords are stored using **BCrypt hashing**
- No plain-text passwords are used in authentication
- SQL queries use **parameterized commands** to prevent SQL injection
- Authentication is handled in the service layer

---

## 🧾 Database Schema (PostgreSQL)

```sql
CREATE TABLE public.customers (
    customer_no TEXT PRIMARY KEY,
    full_name TEXT NOT NULL,
    balance NUMERIC NOT NULL,
    password_hash TEXT NOT NULL,
    currency TEXT DEFAULT 'USD'
);


💳 Transaction Logic

Transfers are handled using PostgreSQL transactions:

Sender balance is checked before debit
Amount is deducted only if sufficient funds exist
Recipient account is credited
Transaction is rolled back on failure

This ensures data consistency and reliability (ACID compliance).


🧪 Sample Users

| Customer No | Name            | Balance  |
| ----------- | --------------- | -------- |
| 712201      | Sophia Garcia   | 18500.00 |
| 712202      | John Miller     | 2450.75  |
| 712203      | Ava Harris      | 16200.00 |
| 712204      | Michael Johnson | 820.00   |


🛠 Tech Stack
C# (.NET Console Application)
PostgreSQL (Supabase)
Npgsql (PostgreSQL driver)
BCrypt.Net (Password hashing)

▶ How to Run
Clone the repository
git clone https://github.com/yourusername/bank-transfer-console-app.git
Open project in Visual Studio or VS Code
Restore dependencies
dotnet restore
Run application
dotnet run --project BankTransferConsoleApp

📌 Future Improvements
Multi-currency support (USD / EUR / TRY)
Transaction history logging
Login attempt lockout system
Admin dashboard
REST API version (ASP.NET Core)
Docker support


📖 Purpose

This project was built as a backend learning exercise to demonstrate:

Layered architecture design in .NET
Secure authentication practices
Database transaction handling
Real-world banking logic simulation


👤 Author

Backend-focused learning project built with emphasis on clean architecture and real-world system design.

---

