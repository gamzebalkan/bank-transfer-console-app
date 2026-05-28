# Bank Transfer Console App

A secure, console-based banking system built with **C# (.NET)** and **PostgreSQL (Supabase)**.

This project is a lightweight fintech simulation designed as a **clean architecture demo** to demonstrate real-world backend engineering principles such as authentication, transactional consistency, and layered system design.

Although it is a console application, the architecture is intentionally designed to be **scalable, maintainable, and production-oriented**.

---

## 🚀 Features

- 🔐 Secure authentication using **BCrypt password hashing**
- 💰 Account balance viewing
- 💸 Domestic bank transfers between customers
- ⚡ ACID-compliant transactional operations (PostgreSQL transactions)
- 🧱 Clean layered architecture (Separation of Concerns)
- 🧩 Repository + Service pattern implementation
- ⌨ Console-based interactive UI
- 🛡 Input validation + retry mechanisms for better UX

---

## 🏗 Architecture (Clean & Scalable Design)

This project follows a layered architecture pattern inspired by enterprise backend systems:

```text
BankTransferConsoleApp/
│
├── Models/ → Domain entities (Customer)
├── Data/ → Repository layer (Database access abstraction)
├── Services/ → Business logic layer (Auth, Transfer)
├── App.cs → Application orchestration layer
├── Program.cs → Entry point
```

### Design Principles Used
- Separation of Concerns (SoC)
- Dependency Injection (manual lightweight form)
- Repository Pattern
- Service Layer Pattern
- Single Responsibility Principle (SRP)

---

## 🔐 Security

- Passwords are stored using **BCrypt hashing (non-reversible)**
- No plaintext password storage
- SQL Injection protection via parameterized queries
- Authentication logic separated from data access layer

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

```

## 💳 Transaction Logic

All transfers are handled using database-level transactions (ACID compliance):

Sender balance is validated before debit
Atomic debit + credit operations
Automatic rollback on failure
Ensures data integrity under concurrent operations

## 🧪 Sample Users
| Customer No | Name            | Balance  |
| ----------- | --------------- | -------- |
| 712201      | Sophia Garcia   | 18500.00 |
| 712202      | John Miller     | 2450.75  |
| 712203      | Ava Harris      | 16200.00 |
| 712204      | Michael Johnson | 820.00   |


## 🛠 Tech Stack

- C# (.NET Console Application)
- PostgreSQL (Supabase)
- Npgsql (PostgreSQL driver)
- BCrypt.Net (Password hashing)

## ▶ How to Run

git clone https://github.com/gamzebalkan/bank-transfer-console-app.git
cd bank-transfer-console-app
dotnet restore
dotnet run --project BankTransferConsoleApp


## 📖 Purpose

This project was built as a backend engineering learning project to demonstrate:

Real-world layered architecture design
Secure authentication patterns
Transaction-safe financial operations
Clean code separation (Domain / Data / Service layers)
Scalable system thinking (production-style structure)


## 📄 License

This project is licensed under the MIT License. You are free to use, modify, and distribute this project for personal or commercial purposes with attribution.

## 👤 Author

Backend-focused engineering project built to simulate real-world banking system architecture and secure transaction processing.
