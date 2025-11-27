using DodgerGameManager.Models;
using DodgerGameManager.Services;
using DodgerGameManager.Utils;

namespace DodgerGameManager;

/// <summary>
/// Quick demo runner for Sprint 4 without user prompts
/// </summary>
class QuickDemo
{
    static void Main(string[] args)
    {
        var dataService = new GameDataService();
        var generator = new DataGenerator(dataService);
        var analytics = new AnalyticsService(dataService);

        Console.WriteLine("╔══════════════════════════════════════════════════╗");
        Console.WriteLine("║   DODGER GAME - Sprint 4 LINQ Demo (Quick View) ║");
        Console.WriteLine("╚══════════════════════════════════════════════════╝\n");

        // Generate data
        generator.GenerateCompleteDataset();

        // Show all analytics
        analytics.ShowPlayerStatistics();
        analytics.ShowGameSessionStatistics();
        analytics.ShowObstacleStatistics();
        analytics.ShowPowerUpStatistics();
        analytics.ShowAdvancedAnalytics();
        analytics.SearchPlayers("Star");
        analytics.FilterObstaclesBySpeed(3.0, 5.0);

        Console.WriteLine("\n✓ Sprint 4 Complete: All LINQ operations demonstrated!");
    }
}
