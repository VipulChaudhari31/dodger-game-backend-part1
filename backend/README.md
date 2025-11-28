# Dodger Game Manager - Backend Console Application

A comprehensive C# console application for managing and analyzing video game data with CRUD operations, random data generation, LINQ queries, and JSON-based data persistence.

## 🎮 Project Overview

This application demonstrates a complete data management system for a space-themed dodger game, featuring players, game sessions, obstacles (meteors), and power-ups.

## 🛠️ Technologies & Tools Used

### Core Technologies
- **.NET 8.0** - Latest LTS framework
- **C# 12** - Modern language features including records, init-only properties
- **System.Text.Json** - High-performance JSON serialization/deserialization

### Language Features Demonstrated
- **Records** - Immutable data types (Player, Obstacle)
- **Classes** - Mutable entities (GameSession, PowerUp)
- **LINQ** - Comprehensive query operations
- **Async/Await** - Asynchronous I/O operations
- **Generic Collections** - List<T> for type-safe storage
- **Lambda Expressions** - Functional programming patterns
- **Extension Methods** - Clean, readable code

## 📁 Project Structure

```
DodgerGameManager/
├── Models/
│   ├── Player.cs           # Player record with stats
│   ├── GameSession.cs      # Game session data
│   ├── Obstacle.cs         # Obstacle record (meteors)
│   └── PowerUp.cs          # Power-up collectibles
├── Services/
│   ├── GameDataService.cs  # CRUD operations
│   └── AnalyticsService.cs # LINQ queries & analytics
├── Utils/
│   └── DataGenerator.cs    # Random data generation
├── Data/
│   └── DataPersistenceService.cs # JSON persistence
└── Program.cs              # Main menu interface
```

## ✨ Features

### 1. **Complete CRUD Operations**
- Create, Read, Update, Delete for all entities
- In-memory data storage with List<T>
- Auto-incrementing primary keys
- Referential integrity

### 2. **Random Data Generation**
- 15 unique players with realistic names
- 50 game sessions with varied difficulty
- 20 obstacles with different attributes
- 12 power-ups with rarity tiers
- and then save it from main menu to see it in GameData Folder

### 3. **LINQ Query Operations**
- **Aggregations**: Count(), Sum(), Average(), Min(), Max()
- **Filtering**: Where(), First(), Take()
- **Ordering**: OrderBy(), OrderByDescending()
- **Grouping**: GroupBy() with nested aggregations
- **Projection**: Select() for data transformation
- **Joins**: Join() with complex relationships
- **Quantifiers**: Any() for existence checks

### 4. **Analytics & Reports**
- Player statistics and leaderboards
- Game session performance analysis
- Obstacle effectiveness metrics
- Power-up collection statistics
- Advanced multi-level analytics

### 5. **Data Persistence**
- JSON file-based storage
- Async save/load operations
- Export to single consolidated file
- Auto-load on startup

### 6. **Interactive Menu System**
- User-friendly console interface
- Navigation between modules
- Input validation
- Formatted output with emojis

## 🚀 Getting Started

### Prerequisites
- .NET 8.0 SDK or later
- Terminal/Command Prompt
- Text editor or IDE (VS Code, Visual Studio, Rider)

### Installation

1. **Navigate to the backend directory:**
```bash
cd /dodger-game/backend/DodgerGameManager
```

2. **Restore dependencies:**
```bash
dotnet restore
```

3. **Build the project:**
```bash
dotnet build
```

4. **Run the application:**
```bash
dotnet run
```

## 📖 Usage Guide

### First Run

1. **Start the application:**
   ```bash
   dotnet run
   ```

2. **Generate sample data:**
   - If no saved data exists, press `N` to start fresh
   - Select option `5` (Generate Random Data)
   - Choose option `1` (Generate Complete Dataset)
   - This creates 15 players, 50 sessions, 20 obstacles, 12 power-ups

### Main Menu Options

```
1. 🎮 Player Management      - View, add, update, delete, search players
2. 🎯 Game Session Management - Manage game sessions
3. ☄️  Obstacle Management    - View and filter obstacles
4. ⭐ Power-up Management     - Manage power-ups
5. 🎲 Generate Random Data    - Create test data
6. 📊 View Analytics & Reports - LINQ-based analytics
7. 💾 Save Data               - Persist to JSON files
8. 📂 Load Data               - Restore from JSON files
9. 📤 Export All Data         - Export to single file
0. 🚪 Exit                    - Close application
```

### Example Workflows

#### **Add a New Player:**
1. Select `1` (Player Management)
2. Select `2` (Add New Player)
3. Enter player name
4. Player is created with default stats

#### **View Analytics:**
1. Select `6` (Analytics & Reports)
2. Choose from 6 different report types
3. View LINQ query results

#### **Save Your Data:**
1. Select `7` (Save Data)
2. Data is persisted to `Data/` folder
3. Creates 4 JSON files (players, sessions, obstacles, powerups)

## 📊 LINQ Operations Demonstrated

### Basic Aggregations
```csharp
// Count total players
var totalPlayers = players.Count();

// Calculate average score
var avgScore = players.Average(p => p.HighestScore);

// Find highest score
var maxScore = players.Max(p => p.HighestScore);
```

### Filtering & Ordering
```csharp
// Filter active obstacles
var activeObstacles = obstacles.Where(o => o.IsActive);

// Order players by score
var leaderboard = players.OrderByDescending(p => p.HighestScore);
```

### Grouping & Advanced Queries
```csharp
// Group sessions by difficulty
var sessionsByDifficulty = sessions
    .GroupBy(s => s.Difficulty)
    .Select(g => new {
        Difficulty = g.Key,
        Count = g.Count(),
        AvgScore = g.Average(s => s.Score)
    });

// Join players with their sessions
var playerSessions = players
    .Join(sessions, 
        p => p.PlayerId, 
        s => s.PlayerId,
        (p, s) => new { p.PlayerName, s.Score });
```

## 💾 Data Storage

### File Locations
All data is stored in the `Data/` directory:
- `players.json` - Player records
- `gamesessions.json` - Game session data
- `obstacles.json` - Obstacle definitions
- `powerups.json` - Power-up configurations
- `all_data.json` - Consolidated export

### JSON Format Example
```json
{
  "PlayerId": 1,
  "PlayerName": "CosmicHunter",
  "TotalGamesPlayed": 12,
  "HighestScore": 8750,
  "TotalScore": 45600,
  "DateRegistered": "2025-09-15T00:00:00",
  "LastPlayed": "2025-11-28T10:30:00",
  "Rank": "Elite"
}
```

## 🎯 Learning Outcomes

This project demonstrates:
- ✅ **CRUD Operations** - Complete data management lifecycle
- ✅ **Random Data Generation** - Realistic test data creation
- ✅ **LINQ Queries** - 10+ different LINQ operations
- ✅ **Menu-Driven Interface** - User interaction patterns
- ✅ **Data Persistence** - File I/O with JSON serialization
- ✅ **OOP Principles** - Encapsulation, abstraction, modularity
- ✅ **Async Programming** - Non-blocking I/O operations
- ✅ **Error Handling** - Input validation and error management

## 🧪 Testing the Application

### Quick Demo Flow
```bash
# 1. Run application
dotnet run

# 2. Generate data (Option 5 → 1)
# 3. View player statistics (Option 6 → 1)
# 4. Add new player (Option 1 → 2)
# 5. Search players (Option 1 → 5)
# 6. View analytics (Option 6 → 6)
# 7. Save data (Option 7)
# 8. Exit (Option 0)

# 9. View saved files
ls -lh Data/*.json
cat Data/all_data.json | head -50
```

## 📝 Development Notes

### Sprint-Based Development
This project was built using Agile methodology:
- **Sprint 1**: Data models and project structure
- **Sprint 2**: CRUD operations implementation
- **Sprint 3**: Random data generation
- **Sprint 4**: LINQ queries and analytics
- **Sprint 5**: Console interface and persistence
