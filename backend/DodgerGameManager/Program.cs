using DodgerGameManager.Models;
using DodgerGameManager.Services;
using DodgerGameManager.Utils;

namespace DodgerGameManager;

/// <summary>
/// Dodger Game Data Management System
/// Sprint 3: Random Data Generation Implementation
/// </summary>
class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("╔══════════════════════════════════════════════════╗");
        Console.WriteLine("║   DODGER GAME DATA MANAGEMENT SYSTEM - Sprint 3  ║");
        Console.WriteLine("╚══════════════════════════════════════════════════╝\n");

        Console.WriteLine("Sprint 3: Random Data Generation Implementation\n");
        Console.WriteLine("Learning Outcomes Demonstrated:");
        Console.WriteLine("✓ Generating Random Data\n");

        // Demonstrate random data generation
        DemonstrateRandomDataGeneration();
    }


    static void DemonstrateRandomDataGeneration()
    {
        var dataService = new GameDataService();
        var generator = new DataGenerator(dataService);

        Console.WriteLine("╔════════════════════════════════════════════════════════╗");
        Console.WriteLine("║        RANDOM DATA GENERATION DEMONSTRATION            ║");
        Console.WriteLine("╚════════════════════════════════════════════════════════╝\n");

        // ============ Individual Generation ============
        Console.WriteLine("--- Individual Entity Generation ---");
        
        generator.GenerateRandomPlayers(5);
        generator.GenerateRandomObstacles(8);
        generator.GenerateRandomPowerUps(6);
        generator.GenerateRandomGameSessions(15);

        Console.WriteLine("\n--- Viewing Sample Generated Data ---\n");

        // Show some sample players
        Console.WriteLine("Sample Players (Top 3):");
        var players = dataService.GetAllPlayers().Take(3);
        foreach (var player in players)
        {
            Console.WriteLine($"  {player}");
        }
        Console.WriteLine();

        // Show some sample obstacles
        Console.WriteLine("Sample Obstacles (Top 3):");
        var obstacles = dataService.GetAllObstacles().Take(3);
        foreach (var obstacle in obstacles)
        {
            Console.WriteLine($"  {obstacle}");
        }
        Console.WriteLine();

        // Show some sample power-ups
        Console.WriteLine("Sample Power-ups (Top 3):");
        var powerUps = dataService.GetAllPowerUps().Take(3);
        foreach (var powerUp in powerUps)
        {
            Console.WriteLine($"  {powerUp}");
        }
        Console.WriteLine();

        // Show some sample game sessions
        Console.WriteLine("Sample Game Sessions (Top 5):");
        var sessions = dataService.GetAllGameSessions().Take(5);
        foreach (var session in sessions)
        {
            Console.WriteLine($"  {session}");
        }
        Console.WriteLine();

        // ============ Complete Dataset Generation ============
        Console.WriteLine("\n--- Testing Complete Dataset Generation ---");
        
        // Clear previous data
        dataService.ClearAllData();
        Console.WriteLine("\nCleared all existing data.");
        
        // Generate complete dataset
        generator.GenerateCompleteDataset();

        // ============ Summary Statistics ============
        Console.WriteLine("\n╔════════════════════════════════════════════════════════╗");
        Console.WriteLine("║                  GENERATION SUMMARY                    ║");
        Console.WriteLine("╚════════════════════════════════════════════════════════╝\n");

        Console.WriteLine($"Total Players:       {dataService.GetTotalPlayers()}");
        Console.WriteLine($"Total Obstacles:     {dataService.GetTotalObstacles()}");
        Console.WriteLine($"Total Power-ups:     {dataService.GetTotalPowerUps()}");
        Console.WriteLine($"Total Game Sessions: {dataService.GetTotalGameSessions()}");

        // Show data distribution
        Console.WriteLine("\n--- Data Distribution ---");
        
        var allPlayers = dataService.GetAllPlayers();
        Console.WriteLine("\nPlayers by Rank:");
        var rankGroups = allPlayers.GroupBy(p => p.Rank);
        foreach (var group in rankGroups.OrderByDescending(g => g.Count()))
        {
            Console.WriteLine($"  {group.Key}: {group.Count()} players");
        }

        var allSessions = dataService.GetAllGameSessions();
        Console.WriteLine("\nSessions by Difficulty:");
        var difficultyGroups = allSessions.GroupBy(s => s.Difficulty);
        foreach (var group in difficultyGroups.OrderByDescending(g => g.Count()))
        {
            Console.WriteLine($"  {group.Key}: {group.Count()} sessions");
        }

        var allPowerUps = dataService.GetAllPowerUps();
        Console.WriteLine("\nPower-ups by Rarity:");
        var rarityGroups = allPowerUps.GroupBy(p => p.Rarity);
        foreach (var group in rarityGroups.OrderBy(g => g.Key))
        {
            Console.WriteLine($"  {group.Key}: {group.Count()} power-ups");
        }

        Console.WriteLine("\n✓ Random data generation completed successfully!");
        Console.WriteLine("\nReady for Sprint 4: LINQ Queries and Analytics");
    }
}
