using DodgerGameManager.Models;
using DodgerGameManager.Services;
using DodgerGameManager.Utils;

namespace DodgerGameManager;

/// <summary>
/// Dodger Game Data Management System
/// Sprint 4: LINQ Queries and Analytics Implementation
/// </summary>
class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("╔══════════════════════════════════════════════════╗");
        Console.WriteLine("║   DODGER GAME DATA MANAGEMENT SYSTEM - Sprint 4  ║");
        Console.WriteLine("╚══════════════════════════════════════════════════╝\n");

        Console.WriteLine("Sprint 4: LINQ Queries and Analytics Implementation\n");
        Console.WriteLine("Learning Outcomes Demonstrated:");
        Console.WriteLine("✓ Introduction to LINQ");
        Console.WriteLine("✓ Summarising Data with LINQ\n");

        // Demonstrate LINQ analytics
        DemonstrateLINQAnalytics();
    }


    static void DemonstrateLINQAnalytics()
    {
        var dataService = new GameDataService();
        var generator = new DataGenerator(dataService);
        var analytics = new AnalyticsService(dataService);

        Console.WriteLine("╔════════════════════════════════════════════════════════╗");
        Console.WriteLine("║          LINQ QUERIES AND ANALYTICS DEMONSTRATION      ║");
        Console.WriteLine("╚════════════════════════════════════════════════════════╝\n");

        // Generate sample data for analysis
        Console.WriteLine("Generating sample dataset for LINQ analysis...\n");
        generator.GenerateCompleteDataset();

        Console.WriteLine("\n" + new string('=', 60));
        Console.WriteLine("DEMONSTRATING LINQ OPERATORS AND QUERIES");
        Console.WriteLine(new string('=', 60));

        // ============ Player Analytics ============
        analytics.ShowPlayerStatistics();
        
        Console.WriteLine("\nPress Enter to continue...");
        Console.ReadLine();
        
        analytics.ShowPlayerRankings();

        Console.WriteLine("\nPress Enter to continue...");
        Console.ReadLine();

        // ============ Game Session Analytics ============
        analytics.ShowGameSessionStatistics();
        
        Console.WriteLine("\nPress Enter to continue...");
        Console.ReadLine();
        
        analytics.ShowRecentSessions(10);

        Console.WriteLine("\nPress Enter to continue...");
        Console.ReadLine();

        // ============ Obstacle Analytics ============
        analytics.ShowObstacleStatistics();

        Console.WriteLine("\nPress Enter to continue...");
        Console.ReadLine();

        // ============ PowerUp Analytics ============
        analytics.ShowPowerUpStatistics();

        Console.WriteLine("\nPress Enter to continue...");
        Console.ReadLine();

        // ============ Advanced LINQ Queries ============
        analytics.ShowAdvancedAnalytics();

        Console.WriteLine("\nPress Enter to continue...");
        Console.ReadLine();

        // ============ Search and Filter Examples ============
        Console.WriteLine("\n╔════════════════════════════════════════════════════════╗");
        Console.WriteLine("║          SEARCH AND FILTER (LINQ Where)                ║");
        Console.WriteLine("╚════════════════════════════════════════════════════════╝");

        // Search players by name
        analytics.SearchPlayers("Star");

        Console.WriteLine();

        // Filter obstacles by speed
        analytics.FilterObstaclesBySpeed(3.0, 5.0);

        // ============ Summary ============
        Console.WriteLine("\n\n╔════════════════════════════════════════════════════════╗");
        Console.WriteLine("║              LINQ OPERATIONS DEMONSTRATED              ║");
        Console.WriteLine("╚════════════════════════════════════════════════════════╝\n");

        Console.WriteLine("✓ Aggregation: Count(), Sum(), Average(), Min(), Max()");
        Console.WriteLine("✓ Ordering: OrderBy(), OrderByDescending()");
        Console.WriteLine("✓ Filtering: Where(), First(), Take()");
        Console.WriteLine("✓ Grouping: GroupBy()");
        Console.WriteLine("✓ Projection: Select()");
        Console.WriteLine("✓ Joining: Join() with group join");
        Console.WriteLine("✓ Quantifiers: Any()");
        Console.WriteLine("✓ Set Operations: Distinct grouping");
        Console.WriteLine("✓ Complex Queries: Multi-level grouping and aggregation");

        Console.WriteLine("\n✓ All LINQ analytics completed successfully!");
        Console.WriteLine("\nReady for Sprint 5: Console Interface and Data Persistence");
    }
}
