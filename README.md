# Football Betting System

A web-based football betting simulation built with **ASP.NET Core MVC**, **C#**, **SQL Server**, **HTML**, and **CSS**.
The system allows users to register, log in, and place bets on football matches using **virtual credits** (no real money involved).
Admins (via MatchService) manage matches, odds, and results to ensure fair play.

---

## Project Goal

The goal of this project is to simulate a real-world betting platform in a **safe, educational environment**.
It demonstrates:

* ASP.NET Core MVC concepts
* Object-Oriented Programming (OOP) and SOLID principles
* Database integration with SQL Server

---

## Features

### User

* Register & log in securely
* Manage virtual wallet balance
* View upcoming matches and odds
* Place bets (Win/Draw/Lose, extended types like Over/Under, Exact Score)
* Track betting history and results

### Admin (MatchService)

* Add, edit, and manage matches
* Set and update betting odds
* Settle matches and process payouts
* View system reports

---

## Project Structure

* **Models** → Classes for User, Gambler, Wallet, Bet, Match, Odds, Receipt
* **Controllers** → Handle user requests (authentication, betting, match management)
* **Views** → Razor pages for UI (Register, Login, Matches, Wallet, Betting History, Admin pages)
* **Database** → SQL Server for storing users, bets, matches, odds, receipts

---

## Technology Stack

* **Frontend**: HTML, CSS (basic styling)
* **Backend**: ASP.NET Core MVC (C#)
* **Database**: SQL Server
* **Tools**: Visual Studio, GitHub for version control

---

## Getting Started

### Prerequisites

* Visual Studio (latest version)
* .NET SDK installed
* SQL Server or SQL Express

### Setup Instructions

1. Clone this repository:

   ```bash
   git clone https://github.com/your-username/football-betting-system.git
   ```
2. Open the solution in **Visual Studio**.
3. Update the **appsettings.json** with your SQL Server connection string.
4. Run the database migrations (or use the provided SQL scripts).
5. Build and run the project.

---


* **Wireframes/Prototypes** (designed in Figma)
* **Test Plan** (functional and non-functional tests
