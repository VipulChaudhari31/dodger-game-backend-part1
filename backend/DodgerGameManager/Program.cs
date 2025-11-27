using DodgerGameManager.Models;
using DodgerGameManager.Services;
using DodgerGameManager.Utils;
using DodgerGameManager.Data;

namespace DodgerGameManager;

/// <summary>
/// Dodger Game Data Management System
/// Sprint 5: Menu-Driven Console Interface and Data Persistence
/// </summary>
class Program
{
    private static GameDataService dataService = new GameDataService();
    private static DataGenerator generator = new DataGenerator(dataService);
    private static AnalyticsService analytics = new AnalyticsService(dataService);
    private static DataPersistenceService persistence = new DataPersistenceService(dataService);

    static async Task Main(string[] args)
    {
        Console.Clear();
        Console.WriteLine("╔══════════════════════════════════════════════════╗");
        Console.WriteLine("║   DODGER GAME DATA MANAGEMENT SYSTEM - Sprint 5  ║");
        Console.WriteLine("╚══════════════════════════════════════════════════╝\n");

        Console.WriteLine("Sprint 5: Menu-Driven Console Interface\n");
        Console.WriteLine("Learning Outcomes Demonstrated:");
        Console.WriteLine("✓ Introduction to Console Applications");
        Console.WriteLine("✓ Understanding Data Management");
        Console.WriteLine("✓ All Previous Learning Outcomes Integrated\n");

        // Check for saved data
        if (persistence.HasSavedData())
        {
            Console.Write("📂 Saved data found. Load it? (y/n): ");
            var response = Console.ReadLine()?.ToLower();
            if (response == "y" || response == "yes")
            {
                await persistence.LoadAllDataAsync();
            }
        }

        await ShowMainMenu();
    }


    static async Task ShowMainMenu()
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("\n╔══════════════════════════════════════════════════════╗");
            Console.WriteLine("║              MAIN MENU                               ║");
            Console.WriteLine("╚══════════════════════════════════════════════════════╝");
            Console.WriteLine($"\n📊 Current Data: {dataService.GetTotalPlayers()} Players | {dataService.GetTotalGameSessions()} Sessions | {dataService.GetTotalObstacles()} Obstacles | {dataService.GetTotalPowerUps()} Power-ups\n");
            
            Console.WriteLine("1. 🎮 Player Management");
            Console.WriteLine("2. 🎯 Game Session Management");
            Console.WriteLine("3. ☄️  Obstacle Management");
            Console.WriteLine("4. ⭐ Power-up Management");
            Console.WriteLine("5. 🎲 Generate Random Data");
            Console.WriteLine("6. 📊 View Analytics & Reports");
            Console.WriteLine("7. 💾 Save Data");
            Console.WriteLine("8. 📂 Load Data");
            Console.WriteLine("9. 📤 Export All Data");
            Console.WriteLine("0. 🚪 Exit");
            
            Console.Write("\nSelect an option: ");
            var choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    await ShowPlayerManagementMenu();
                    break;
                case "2":
                    await ShowGameSessionMenu();
                    break;
                case "3":
                    await ShowObstacleMenu();
                    break;
                case "4":
                    await ShowPowerUpMenu();
                    break;
                case "5":
                    ShowDataGenerationMenu();
                    break;
                case "6":
                    ShowAnalyticsMenu();
                    break;
                case "7":
                    await persistence.SaveAllDataAsync();
                    Console.WriteLine("\nPress Enter to continue...");
                    Console.ReadLine();
                    break;
                case "8":
                    dataService.ClearAllData();
                    await persistence.LoadAllDataAsync();
                    Console.WriteLine("\nPress Enter to continue...");
                    Console.ReadLine();
                    break;
                case "9":
                    await persistence.ExportDataToSingleFileAsync();
                    Console.WriteLine("\nPress Enter to continue...");
                    Console.ReadLine();
                    break;
                case "0":
                    Console.WriteLine("\n👋 Thank you for using Dodger Game Data Management System!");
                    Console.WriteLine("✓ All learning outcomes demonstrated successfully!");
                    return;
                default:
                    Console.WriteLine("\n❌ Invalid option. Please try again.");
                    Console.WriteLine("Press Enter to continue...");
                    Console.ReadLine();
                    break;
            }
        }
    }

    #region Player Management

    static async Task ShowPlayerManagementMenu()
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("\n╔══════════════════════════════════════════════════════╗");
            Console.WriteLine("║           PLAYER MANAGEMENT                          ║");
            Console.WriteLine("╚══════════════════════════════════════════════════════╝\n");
            
            Console.WriteLine("1. View All Players");
            Console.WriteLine("2. Add New Player");
            Console.WriteLine("3. Update Player");
            Console.WriteLine("4. Delete Player");
            Console.WriteLine("5. Search Players");
            Console.WriteLine("0. Back to Main Menu");
            
            Console.Write("\nSelect an option: ");
            var choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    ViewAllPlayers();
                    break;
                case "2":
                    AddNewPlayer();
                    break;
                case "3":
                    UpdatePlayer();
                    break;
                case "4":
                    DeletePlayer();
                    break;
                case "5":
                    SearchPlayers();
                    break;
                case "0":
                    return;
                default:
                    Console.WriteLine("\n❌ Invalid option.");
                    Console.WriteLine("Press Enter to continue...");
                    Console.ReadLine();
                    break;
            }
        }
    }

    static void ViewAllPlayers()
    {
        Console.Clear();
        Console.WriteLine("\n📋 ALL PLAYERS\n");
        
        var players = dataService.GetAllPlayers();
        if (!players.Any())
        {
            Console.WriteLine("No players found.");
        }
        else
        {
            foreach (var player in players)
            {
                Console.WriteLine(player);
            }
        }
        
        Console.WriteLine($"\nTotal: {players.Count} players");
        Console.WriteLine("\nPress Enter to continue...");
        Console.ReadLine();
    }

    static void AddNewPlayer()
    {
        Console.Clear();
        Console.WriteLine("\n➕ ADD NEW PLAYER\n");
        
        Console.Write("Enter player name: ");
        var name = Console.ReadLine();
        
        if (string.IsNullOrWhiteSpace(name))
        {
            Console.WriteLine("❌ Player name cannot be empty.");
        }
        else
        {
            var player = dataService.CreatePlayer(name);
            Console.WriteLine($"\n✓ Player created successfully!");
            Console.WriteLine(player);
        }
        
        Console.WriteLine("\nPress Enter to continue...");
        Console.ReadLine();
    }

    static void UpdatePlayer()
    {
        Console.Clear();
        Console.WriteLine("\n✏️  UPDATE PLAYER\n");
        
        Console.Write("Enter player ID: ");
        if (int.TryParse(Console.ReadLine(), out int id))
        {
            var player = dataService.GetPlayerById(id);
            if (player == null)
            {
                Console.WriteLine("❌ Player not found.");
            }
            else
            {
                Console.WriteLine($"\nCurrent: {player}");
                Console.Write("\nEnter new name (or press Enter to skip): ");
                var name = Console.ReadLine();
                
                Console.Write("Enter high score (or press Enter to skip): ");
                var scoreInput = Console.ReadLine();
                
                dataService.UpdatePlayer(id, p =>
                {
                    if (!string.IsNullOrWhiteSpace(name)) p.PlayerName = name;
                    if (int.TryParse(scoreInput, out int score)) p.HighestScore = score;
                });
                
                Console.WriteLine($"\n✓ Player updated!");
                Console.WriteLine(dataService.GetPlayerById(id));
            }
        }
        else
        {
            Console.WriteLine("❌ Invalid ID.");
        }
        
        Console.WriteLine("\nPress Enter to continue...");
        Console.ReadLine();
    }

    static void DeletePlayer()
    {
        Console.Clear();
        Console.WriteLine("\n🗑️  DELETE PLAYER\n");
        
        Console.Write("Enter player ID to delete: ");
        if (int.TryParse(Console.ReadLine(), out int id))
        {
            var player = dataService.GetPlayerById(id);
            if (player == null)
            {
                Console.WriteLine("❌ Player not found.");
            }
            else
            {
                Console.WriteLine($"\n{player}");
                Console.Write("\nAre you sure? (y/n): ");
                if (Console.ReadLine()?.ToLower() == "y")
                {
                    dataService.DeletePlayer(id);
                    Console.WriteLine("✓ Player deleted.");
                }
                else
                {
                    Console.WriteLine("❌ Deletion cancelled.");
                }
            }
        }
        else
        {
            Console.WriteLine("❌ Invalid ID.");
        }
        
        Console.WriteLine("\nPress Enter to continue...");
        Console.ReadLine();
    }

    static void SearchPlayers()
    {
        Console.Clear();
        Console.WriteLine("\n🔍 SEARCH PLAYERS\n");
        
        Console.Write("Enter search term: ");
        var term = Console.ReadLine();
        
        if (!string.IsNullOrWhiteSpace(term))
        {
            analytics.SearchPlayers(term);
        }
        
        Console.WriteLine("\nPress Enter to continue...");
        Console.ReadLine();
    }

    #endregion

    #region Game Session Management

    static async Task ShowGameSessionMenu()
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("\n╔══════════════════════════════════════════════════════╗");
            Console.WriteLine("║         GAME SESSION MANAGEMENT                      ║");
            Console.WriteLine("╚══════════════════════════════════════════════════════╝\n");
            
            Console.WriteLine("1. View All Sessions");
            Console.WriteLine("2. View Recent Sessions");
            Console.WriteLine("3. Add New Session");
            Console.WriteLine("4. Delete Session");
            Console.WriteLine("0. Back to Main Menu");
            
            Console.Write("\nSelect an option: ");
            var choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    ViewAllSessions();
                    break;
                case "2":
                    ViewRecentSessions();
                    break;
                case "3":
                    AddNewSession();
                    break;
                case "4":
                    DeleteSession();
                    break;
                case "0":
                    return;
                default:
                    Console.WriteLine("\n❌ Invalid option.");
                    Console.WriteLine("Press Enter to continue...");
                    Console.ReadLine();
                    break;
            }
        }
    }

    static void ViewAllSessions()
    {
        Console.Clear();
        Console.WriteLine("\n📋 ALL GAME SESSIONS\n");
        
        var sessions = dataService.GetAllGameSessions();
        if (!sessions.Any())
        {
            Console.WriteLine("No game sessions found.");
        }
        else
        {
            foreach (var session in sessions.Take(20))
            {
                Console.WriteLine(session);
            }
            
            if (sessions.Count > 20)
            {
                Console.WriteLine($"\n... and {sessions.Count - 20} more sessions");
            }
        }
        
        Console.WriteLine($"\nTotal: {sessions.Count} sessions");
        Console.WriteLine("\nPress Enter to continue...");
        Console.ReadLine();
    }

    static void ViewRecentSessions()
    {
        Console.Clear();
        analytics.ShowRecentSessions(15);
        Console.WriteLine("\nPress Enter to continue...");
        Console.ReadLine();
    }

    static void AddNewSession()
    {
        Console.Clear();
        Console.WriteLine("\n➕ ADD NEW GAME SESSION\n");
        
        Console.Write("Enter player ID: ");
        if (!int.TryParse(Console.ReadLine(), out int playerId))
        {
            Console.WriteLine("❌ Invalid player ID.");
            Console.WriteLine("Press Enter to continue...");
            Console.ReadLine();
            return;
        }
        
        var player = dataService.GetPlayerById(playerId);
        if (player == null)
        {
            Console.WriteLine("❌ Player not found.");
            Console.WriteLine("Press Enter to continue...");
            Console.ReadLine();
            return;
        }
        
        Console.Write("Enter score: ");
        if (!int.TryParse(Console.ReadLine(), out int score))
        {
            Console.WriteLine("❌ Invalid score.");
            Console.WriteLine("Press Enter to continue...");
            Console.ReadLine();
            return;
        }
        
        int level = (score / 1000) + 1;
        TimeSpan duration = TimeSpan.FromMinutes(new Random().Next(1, 15));
        
        var session = dataService.CreateGameSession(playerId, player.PlayerName, score, level, duration);
        Console.WriteLine($"\n✓ Game session created!");
        Console.WriteLine(session);
        
        Console.WriteLine("\nPress Enter to continue...");
        Console.ReadLine();
    }

    static void DeleteSession()
    {
        Console.Clear();
        Console.WriteLine("\n🗑️  DELETE GAME SESSION\n");
        
        Console.Write("Enter session ID to delete: ");
        if (int.TryParse(Console.ReadLine(), out int id))
        {
            var session = dataService.GetGameSessionById(id);
            if (session == null)
            {
                Console.WriteLine("❌ Session not found.");
            }
            else
            {
                Console.WriteLine($"\n{session}");
                Console.Write("\nAre you sure? (y/n): ");
                if (Console.ReadLine()?.ToLower() == "y")
                {
                    dataService.DeleteGameSession(id);
                    Console.WriteLine("✓ Session deleted.");
                }
                else
                {
                    Console.WriteLine("❌ Deletion cancelled.");
                }
            }
        }
        else
        {
            Console.WriteLine("❌ Invalid ID.");
        }
        
        Console.WriteLine("\nPress Enter to continue...");
        Console.ReadLine();
    }

    #endregion

    #region Obstacle and PowerUp Management

    static async Task ShowObstacleMenu()
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("\n╔══════════════════════════════════════════════════════╗");
            Console.WriteLine("║           OBSTACLE MANAGEMENT                        ║");
            Console.WriteLine("╚══════════════════════════════════════════════════════╝\n");
            
            Console.WriteLine("1. View All Obstacles");
            Console.WriteLine("2. Add New Obstacle");
            Console.WriteLine("3. Filter by Speed Range");
            Console.WriteLine("0. Back to Main Menu");
            
            Console.Write("\nSelect an option: ");
            var choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    ViewAllObstacles();
                    break;
                case "2":
                    AddNewObstacle();
                    break;
                case "3":
                    FilterObstacles();
                    break;
                case "0":
                    return;
                default:
                    Console.WriteLine("\n❌ Invalid option.");
                    Console.WriteLine("Press Enter to continue...");
                    Console.ReadLine();
                    break;
            }
        }
    }

    static void ViewAllObstacles()
    {
        Console.Clear();
        Console.WriteLine("\n📋 ALL OBSTACLES\n");
        
        var obstacles = dataService.GetAllObstacles();
        if (!obstacles.Any())
        {
            Console.WriteLine("No obstacles found.");
        }
        else
        {
            foreach (var obstacle in obstacles)
            {
                Console.WriteLine(obstacle);
            }
        }
        
        Console.WriteLine($"\nTotal: {obstacles.Count} obstacles");
        Console.WriteLine("\nPress Enter to continue...");
        Console.ReadLine();
    }

    static void AddNewObstacle()
    {
        Console.Clear();
        Console.WriteLine("\n➕ ADD NEW OBSTACLE\n");
        
        Console.Write("Enter obstacle name: ");
        var name = Console.ReadLine() ?? "Unknown Obstacle";
        
        Console.Write("Enter type (Meteor/Comet/Asteroid): ");
        var type = Console.ReadLine() ?? "Meteor";
        
        Console.Write("Enter speed: ");
        double.TryParse(Console.ReadLine(), out double speed);
        if (speed <= 0) speed = 2.0;
        
        Console.Write("Enter damage points: ");
        int.TryParse(Console.ReadLine(), out int damage);
        if (damage <= 0) damage = 100;
        
        Console.Write("Enter size: ");
        int.TryParse(Console.ReadLine(), out int size);
        if (size <= 0) size = 20;
        
        var obstacle = dataService.CreateObstacle(name, type, speed, damage, size);
        Console.WriteLine($"\n✓ Obstacle created!");
        Console.WriteLine(obstacle);
        
        Console.WriteLine("\nPress Enter to continue...");
        Console.ReadLine();
    }

    static void FilterObstacles()
    {
        Console.Clear();
        Console.WriteLine("\n🔍 FILTER OBSTACLES BY SPEED\n");
        
        Console.Write("Enter minimum speed: ");
        double.TryParse(Console.ReadLine(), out double minSpeed);
        
        Console.Write("Enter maximum speed: ");
        double.TryParse(Console.ReadLine(), out double maxSpeed);
        
        if (minSpeed >= 0 && maxSpeed > minSpeed)
        {
            analytics.FilterObstaclesBySpeed(minSpeed, maxSpeed);
        }
        else
        {
            Console.WriteLine("❌ Invalid speed range.");
        }
        
        Console.WriteLine("\nPress Enter to continue...");
        Console.ReadLine();
    }

    static async Task ShowPowerUpMenu()
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("\n╔══════════════════════════════════════════════════════╗");
            Console.WriteLine("║           POWER-UP MANAGEMENT                        ║");
            Console.WriteLine("╚══════════════════════════════════════════════════════╝\n");
            
            Console.WriteLine("1. View All Power-ups");
            Console.WriteLine("2. Add New Power-up");
            Console.WriteLine("0. Back to Main Menu");
            
            Console.Write("\nSelect an option: ");
            var choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    ViewAllPowerUps();
                    break;
                case "2":
                    AddNewPowerUp();
                    break;
                case "0":
                    return;
                default:
                    Console.WriteLine("\n❌ Invalid option.");
                    Console.WriteLine("Press Enter to continue...");
                    Console.ReadLine();
                    break;
            }
        }
    }

    static void ViewAllPowerUps()
    {
        Console.Clear();
        Console.WriteLine("\n📋 ALL POWER-UPS\n");
        
        var powerUps = dataService.GetAllPowerUps();
        if (!powerUps.Any())
        {
            Console.WriteLine("No power-ups found.");
        }
        else
        {
            foreach (var powerUp in powerUps)
            {
                Console.WriteLine(powerUp);
            }
        }
        
        Console.WriteLine($"\nTotal: {powerUps.Count} power-ups");
        Console.WriteLine("\nPress Enter to continue...");
        Console.ReadLine();
    }

    static void AddNewPowerUp()
    {
        Console.Clear();
        Console.WriteLine("\n➕ ADD NEW POWER-UP\n");
        
        Console.Write("Enter power-up name: ");
        var name = Console.ReadLine() ?? "Unknown Power-up";
        
        Console.Write("Enter type (Defensive/Offensive/Bonus): ");
        var type = Console.ReadLine() ?? "Bonus";
        
        Console.Write("Enter effect: ");
        var effect = Console.ReadLine() ?? "Bonus Effect";
        
        Console.Write("Enter duration (seconds): ");
        int.TryParse(Console.ReadLine(), out int duration);
        if (duration <= 0) duration = 5;
        
        Console.Write("Enter points value: ");
        int.TryParse(Console.ReadLine(), out int points);
        if (points <= 0) points = 100;
        
        var powerUp = dataService.CreatePowerUp(name, type, effect, duration, points);
        Console.WriteLine($"\n✓ Power-up created!");
        Console.WriteLine(powerUp);
        
        Console.WriteLine("\nPress Enter to continue...");
        Console.ReadLine();
    }

    #endregion

    #region Data Generation

    static void ShowDataGenerationMenu()
    {
        Console.Clear();
        Console.WriteLine("\n╔══════════════════════════════════════════════════════╗");
        Console.WriteLine("║           RANDOM DATA GENERATION                     ║");
        Console.WriteLine("╚══════════════════════════════════════════════════════╝\n");
        
        Console.WriteLine("1. Generate Complete Dataset (15/20/12/50)");
        Console.WriteLine("2. Generate Custom Number of Players");
        Console.WriteLine("3. Generate Custom Number of Obstacles");
        Console.WriteLine("4. Generate Custom Number of Power-ups");
        Console.WriteLine("5. Generate Custom Number of Sessions");
        Console.WriteLine("0. Back to Main Menu");
        
        Console.Write("\nSelect an option: ");
        var choice = Console.ReadLine();

        switch (choice)
        {
            case "1":
                Console.Clear();
                generator.GenerateCompleteDataset();
                Console.WriteLine("\nPress Enter to continue...");
                Console.ReadLine();
                break;
            case "2":
                Console.Write("\nEnter number of players to generate: ");
                if (int.TryParse(Console.ReadLine(), out int playerCount) && playerCount > 0)
                {
                    generator.GenerateRandomPlayers(playerCount);
                }
                Console.WriteLine("\nPress Enter to continue...");
                Console.ReadLine();
                break;
            case "3":
                Console.Write("\nEnter number of obstacles to generate: ");
                if (int.TryParse(Console.ReadLine(), out int obstacleCount) && obstacleCount > 0)
                {
                    generator.GenerateRandomObstacles(obstacleCount);
                }
                Console.WriteLine("\nPress Enter to continue...");
                Console.ReadLine();
                break;
            case "4":
                Console.Write("\nEnter number of power-ups to generate: ");
                if (int.TryParse(Console.ReadLine(), out int powerUpCount) && powerUpCount > 0)
                {
                    generator.GenerateRandomPowerUps(powerUpCount);
                }
                Console.WriteLine("\nPress Enter to continue...");
                Console.ReadLine();
                break;
            case "5":
                Console.Write("\nEnter number of sessions to generate: ");
                if (int.TryParse(Console.ReadLine(), out int sessionCount) && sessionCount > 0)
                {
                    generator.GenerateRandomGameSessions(sessionCount);
                }
                Console.WriteLine("\nPress Enter to continue...");
                Console.ReadLine();
                break;
            case "0":
                return;
            default:
                Console.WriteLine("\n❌ Invalid option.");
                Console.WriteLine("Press Enter to continue...");
                Console.ReadLine();
                break;
        }
    }

    #endregion

    #region Analytics

    static void ShowAnalyticsMenu()
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("\n╔══════════════════════════════════════════════════════╗");
            Console.WriteLine("║         ANALYTICS & REPORTS (LINQ)                   ║");
            Console.WriteLine("╚══════════════════════════════════════════════════════╝\n");
            
            Console.WriteLine("1. Player Statistics");
            Console.WriteLine("2. Player Leaderboard");
            Console.WriteLine("3. Game Session Statistics");
            Console.WriteLine("4. Obstacle Statistics");
            Console.WriteLine("5. Power-up Statistics");
            Console.WriteLine("6. Advanced Analytics");
            Console.WriteLine("0. Back to Main Menu");
            
            Console.Write("\nSelect an option: ");
            var choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    Console.Clear();
                    analytics.ShowPlayerStatistics();
                    Console.WriteLine("\nPress Enter to continue...");
                    Console.ReadLine();
                    break;
                case "2":
                    Console.Clear();
                    analytics.ShowPlayerRankings();
                    Console.WriteLine("\nPress Enter to continue...");
                    Console.ReadLine();
                    break;
                case "3":
                    Console.Clear();
                    analytics.ShowGameSessionStatistics();
                    Console.WriteLine("\nPress Enter to continue...");
                    Console.ReadLine();
                    break;
                case "4":
                    Console.Clear();
                    analytics.ShowObstacleStatistics();
                    Console.WriteLine("\nPress Enter to continue...");
                    Console.ReadLine();
                    break;
                case "5":
                    Console.Clear();
                    analytics.ShowPowerUpStatistics();
                    Console.WriteLine("\nPress Enter to continue...");
                    Console.ReadLine();
                    break;
                case "6":
                    Console.Clear();
                    analytics.ShowAdvancedAnalytics();
                    Console.WriteLine("\nPress Enter to continue...");
                    Console.ReadLine();
                    break;
                case "0":
                    return;
                default:
                    Console.WriteLine("\n❌ Invalid option.");
                    Console.WriteLine("Press Enter to continue...");
                    Console.ReadLine();
                    break;
            }
        }
    }

    #endregion
}

