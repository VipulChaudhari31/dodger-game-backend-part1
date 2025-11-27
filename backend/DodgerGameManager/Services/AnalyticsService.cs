using DodgerGameManager.Models;
using DodgerGameManager.Services;

namespace DodgerGameManager.Services;

/// <summary>
/// Provides LINQ-based data analysis and reporting
/// Demonstrates: Introduction to LINQ, Summarising Data with LINQ
/// Learning Outcomes: Introduction to LINQ, Summarising Data with LINQ
/// </summary>
public class AnalyticsService
{
    private readonly GameDataService dataService;

    public AnalyticsService(GameDataService service)
    {
        dataService = service;
    }

    #region Player Analytics

    /// <summary>
    /// Display comprehensive player statistics using LINQ
    /// </summary>
    public void ShowPlayerStatistics()
    {
        var players = dataService.GetAllPlayers();
        if (!players.Any())
        {
            Console.WriteLine("No players found.");
            return;
        }

        Console.WriteLine("\n╔═══════════════════════════════════════════════╗");
        Console.WriteLine("║         PLAYER STATISTICS (LINQ)              ║");
        Console.WriteLine("╚═══════════════════════════════════════════════╝\n");

        // Total players using Count()
        int totalPlayers = players.Count();
        Console.WriteLine($"Total Players: {totalPlayers}");

        // Average high score using Average()
        double avgHighScore = players.Average(p => p.HighestScore);
        Console.WriteLine($"Average High Score: {avgHighScore:F2}");

        // Highest score using Max()
        int maxScore = players.Max(p => p.HighestScore);
        Console.WriteLine($"Highest Score Ever: {maxScore}");

        // Lowest score using Min()
        int minScore = players.Min(p => p.HighestScore);
        Console.WriteLine($"Lowest High Score: {minScore}");

        // Total games using Sum()
        int totalGames = players.Sum(p => p.TotalGamesPlayed);
        Console.WriteLine($"Total Games Played: {totalGames}");

        // Players by rank using GroupBy()
        Console.WriteLine("\n--- Players by Rank ---");
        var playersByRank = players.GroupBy(p => p.Rank)
                                   .OrderByDescending(g => g.Count());
        
        foreach (var group in playersByRank)
        {
            Console.WriteLine($"  {group.Key}: {group.Count()} players");
        }

        // Most active players using OrderByDescending() and Take()
        Console.WriteLine("\n--- Top 5 Most Active Players ---");
        var topActivePlayers = players.OrderByDescending(p => p.TotalGamesPlayed)
                                     .Take(5);
        
        foreach (var player in topActivePlayers)
        {
            Console.WriteLine($"  {player.PlayerName}: {player.TotalGamesPlayed} games");
        }

        // Top players by high score
        Console.WriteLine("\n--- Top 5 Players by High Score ---");
        var topPlayers = players.OrderByDescending(p => p.HighestScore)
                                .Take(5);
        
        foreach (var player in topPlayers)
        {
            Console.WriteLine($"  {player.PlayerName}: {player.HighestScore} points");
        }
    }

    /// <summary>
    /// Display player leaderboard using LINQ sorting
    /// </summary>
    public void ShowPlayerRankings()
    {
        var players = dataService.GetAllPlayers()
                                 .OrderByDescending(p => p.HighestScore)
                                 .ToList();

        if (!players.Any())
        {
            Console.WriteLine("No players found.");
            return;
        }

        Console.WriteLine("\n╔═══════════════════════════════════════════════════════════════════════════╗");
        Console.WriteLine("║                          PLAYER LEADERBOARD                               ║");
        Console.WriteLine("╚═══════════════════════════════════════════════════════════════════════════╝\n");

        Console.WriteLine("{0,-5} {1,-20} {2,-12} {3,-15} {4,-10}", 
            "Rank", "Player Name", "High Score", "Avg Score", "Games");
        Console.WriteLine(new string('-', 75));

        int rank = 1;
        foreach (var player in players)
        {
            Console.WriteLine("{0,-5} {1,-20} {2,-12} {3,-15:F2} {4,-10}",
                rank++,
                player.PlayerName,
                player.HighestScore,
                player.GetAverageScore(),
                player.TotalGamesPlayed);
        }
    }

    #endregion

    #region Game Session Analytics

    /// <summary>
    /// Display game session statistics using LINQ aggregation
    /// </summary>
    public void ShowGameSessionStatistics()
    {
        var sessions = dataService.GetAllGameSessions();
        if (!sessions.Any())
        {
            Console.WriteLine("No game sessions found.");
            return;
        }

        Console.WriteLine("\n╔═══════════════════════════════════════════════╗");
        Console.WriteLine("║      GAME SESSION STATISTICS (LINQ)           ║");
        Console.WriteLine("╚═══════════════════════════════════════════════╝\n");

        // Total sessions
        Console.WriteLine($"Total Game Sessions: {sessions.Count()}");

        // Average score per session
        double avgScore = sessions.Average(s => s.Score);
        Console.WriteLine($"Average Session Score: {avgScore:F2}");

        // Highest session score
        int maxSessionScore = sessions.Max(s => s.Score);
        Console.WriteLine($"Highest Session Score: {maxSessionScore}");

        // Average level reached
        double avgLevel = sessions.Average(s => s.Level);
        Console.WriteLine($"Average Level Reached: {avgLevel:F2}");

        // Highest level reached
        int maxLevel = sessions.Max(s => s.Level);
        Console.WriteLine($"Highest Level Reached: {maxLevel}");

        // Average duration
        var avgDuration = TimeSpan.FromSeconds(sessions.Average(s => s.Duration.TotalSeconds));
        Console.WriteLine($"Average Game Duration: {avgDuration:mm\\:ss}");

        // Sessions by difficulty using GroupBy()
        Console.WriteLine("\n--- Sessions by Difficulty ---");
        var sessionsByDifficulty = sessions.GroupBy(s => s.Difficulty)
                                          .OrderByDescending(g => g.Count());
        
        foreach (var group in sessionsByDifficulty)
        {
            Console.WriteLine($"  {group.Key}: {group.Count()} sessions");
        }

        // Total obstacles dodged
        int totalObstacles = sessions.Sum(s => s.ObstaclesDodged);
        Console.WriteLine($"\nTotal Obstacles Dodged: {totalObstacles}");

        // Total power-ups collected
        int totalPowerUps = sessions.Sum(s => s.PowerUpsCollected);
        Console.WriteLine($"Total Power-ups Collected: {totalPowerUps}");

        // High score sessions using Count() with predicate
        int highScoreSessions = sessions.Count(s => s.NewHighScore);
        Console.WriteLine($"New High Score Sessions: {highScoreSessions}");
    }

    /// <summary>
    /// Display recent sessions using LINQ ordering
    /// </summary>
    public void ShowRecentSessions(int count = 10)
    {
        var recentSessions = dataService.GetAllGameSessions()
                                       .OrderByDescending(s => s.SessionDate)
                                       .Take(count)
                                       .ToList();

        if (!recentSessions.Any())
        {
            Console.WriteLine("No game sessions found.");
            return;
        }

        Console.WriteLine($"\n╔═══════════════════════════════════════════════════════════════════════════╗");
        Console.WriteLine($"║                     RECENT {count} GAME SESSIONS                               ║");
        Console.WriteLine("╚═══════════════════════════════════════════════════════════════════════════╝\n");

        foreach (var session in recentSessions)
        {
            Console.WriteLine(session.ToString());
        }
    }

    #endregion

    #region Obstacle Analytics

    /// <summary>
    /// Display obstacle statistics using LINQ
    /// </summary>
    public void ShowObstacleStatistics()
    {
        var obstacles = dataService.GetAllObstacles();
        if (!obstacles.Any())
        {
            Console.WriteLine("No obstacles found.");
            return;
        }

        Console.WriteLine("\n╔═══════════════════════════════════════════════╗");
        Console.WriteLine("║       OBSTACLE STATISTICS (LINQ)              ║");
        Console.WriteLine("╚═══════════════════════════════════════════════╝\n");

        // Total obstacles
        Console.WriteLine($"Total Obstacles: {obstacles.Count()}");

        // Average speed
        double avgSpeed = obstacles.Average(o => o.Speed);
        Console.WriteLine($"Average Speed: {avgSpeed:F2}");

        // Fastest obstacle using Max() and First()
        double maxSpeed = obstacles.Max(o => o.Speed);
        var fastestObstacle = obstacles.First(o => o.Speed == maxSpeed);
        Console.WriteLine($"Fastest Obstacle: {fastestObstacle.ObstacleName} ({maxSpeed})");

        // Average damage
        double avgDamage = obstacles.Average(o => o.DamagePoints);
        Console.WriteLine($"Average Damage: {avgDamage:F2}");

        // Most dangerous obstacle
        int maxDamage = obstacles.Max(o => o.DamagePoints);
        var mostDangerous = obstacles.First(o => o.DamagePoints == maxDamage);
        Console.WriteLine($"Most Dangerous: {mostDangerous.ObstacleName} ({maxDamage} damage)");

        // Obstacles by type using GroupBy()
        Console.WriteLine("\n--- Obstacles by Type ---");
        var obstaclesByType = obstacles.GroupBy(o => o.ObstacleType)
                                      .OrderByDescending(g => g.Count());
        
        foreach (var group in obstaclesByType)
        {
            Console.WriteLine($"  {group.Key}: {group.Count()} obstacles");
        }

        // Active vs Inactive using Count() with predicate
        int activeCount = obstacles.Count(o => o.IsActive);
        int inactiveCount = obstacles.Count(o => !o.IsActive);
        Console.WriteLine($"\nActive Obstacles: {activeCount}");
        Console.WriteLine($"Inactive Obstacles: {inactiveCount}");
    }

    #endregion

    #region PowerUp Analytics

    /// <summary>
    /// Display power-up statistics using LINQ
    /// </summary>
    public void ShowPowerUpStatistics()
    {
        var powerUps = dataService.GetAllPowerUps();
        if (!powerUps.Any())
        {
            Console.WriteLine("No power-ups found.");
            return;
        }

        Console.WriteLine("\n╔═══════════════════════════════════════════════╗");
        Console.WriteLine("║       POWER-UP STATISTICS (LINQ)              ║");
        Console.WriteLine("╚═══════════════════════════════════════════════╝\n");

        // Total power-ups
        Console.WriteLine($"Total Power-ups: {powerUps.Count()}");

        // Average points value
        double avgPoints = powerUps.Average(p => p.PointsValue);
        Console.WriteLine($"Average Points Value: {avgPoints:F2}");

        // Most valuable power-up
        int maxPoints = powerUps.Max(p => p.PointsValue);
        var mostValuable = powerUps.First(p => p.PointsValue == maxPoints);
        Console.WriteLine($"Most Valuable: {mostValuable.PowerUpName} ({maxPoints} points)");

        // Average duration
        double avgDuration = powerUps.Average(p => p.DurationSeconds);
        Console.WriteLine($"Average Duration: {avgDuration:F2} seconds");

        // Power-ups by rarity
        Console.WriteLine("\n--- Power-ups by Rarity ---");
        var powerUpsByRarity = powerUps.GroupBy(p => p.Rarity)
                                      .OrderBy(g => g.Key);
        
        foreach (var group in powerUpsByRarity)
        {
            Console.WriteLine($"  {group.Key}: {group.Count()} power-ups");
        }

        // Power-ups by type
        Console.WriteLine("\n--- Power-ups by Type ---");
        var powerUpsByType = powerUps.GroupBy(p => p.PowerUpType)
                                    .OrderByDescending(g => g.Count());
        
        foreach (var group in powerUpsByType)
        {
            Console.WriteLine($"  {group.Key}: {group.Count()} power-ups");
        }

        // Top 5 most valuable power-ups
        Console.WriteLine("\n--- Top 5 Most Valuable Power-ups ---");
        var topPowerUps = powerUps.OrderByDescending(p => p.PointsValue)
                                  .Take(5);
        
        foreach (var powerUp in topPowerUps)
        {
            Console.WriteLine($"  {powerUp.PowerUpName}: {powerUp.PointsValue} points ({powerUp.Rarity})");
        }
    }

    #endregion

    #region Advanced LINQ Queries

    /// <summary>
    /// Display advanced analytics using complex LINQ queries
    /// </summary>
    public void ShowAdvancedAnalytics()
    {
        Console.WriteLine("\n╔═══════════════════════════════════════════════╗");
        Console.WriteLine("║       ADVANCED ANALYTICS (LINQ)               ║");
        Console.WriteLine("╚═══════════════════════════════════════════════╝\n");

        var sessions = dataService.GetAllGameSessions();
        var players = dataService.GetAllPlayers();

        if (!sessions.Any() || !players.Any())
        {
            Console.WriteLine("Insufficient data for advanced analytics.");
            return;
        }

        // Player performance analysis using Join
        Console.WriteLine("--- Player Performance Analysis ---");
        var playerPerformance = from player in players
                               join session in sessions on player.PlayerId equals session.PlayerId into playerSessions
                               select new
                               {
                                   PlayerName = player.PlayerName,
                                   TotalSessions = playerSessions.Count(),
                                   AverageScore = playerSessions.Any() ? playerSessions.Average(s => s.Score) : 0,
                                   BestScore = player.HighestScore,
                                   Rank = player.Rank
                               };

        var topPerformers = playerPerformance.OrderByDescending(p => p.AverageScore)
                                            .Take(5);

        foreach (var performer in topPerformers)
        {
            Console.WriteLine($"  {performer.PlayerName} ({performer.Rank})");
            Console.WriteLine($"    Avg Score: {performer.AverageScore:F2} | Best: {performer.BestScore} | Sessions: {performer.TotalSessions}");
        }

        // Score distribution using GroupBy with range selector
        Console.WriteLine("\n--- Score Distribution ---");
        var scoreRanges = sessions.GroupBy(s => s.Score switch
        {
            < 500 => "0-499",
            < 1000 => "500-999",
            < 2500 => "1000-2499",
            < 5000 => "2500-4999",
            < 10000 => "5000-9999",
            _ => "10000+"
        }).OrderBy(g => g.Key);

        foreach (var range in scoreRanges)
        {
            Console.WriteLine($"  {range.Key}: {range.Count()} sessions");
        }

        // Level progression analysis
        Console.WriteLine("\n--- Level Progression ---");
        var levelStats = sessions.GroupBy(s => s.Level)
                                .OrderBy(g => g.Key)
                                .Select(g => new
                                {
                                    Level = g.Key,
                                    SessionCount = g.Count(),
                                    AvgScore = g.Average(s => s.Score),
                                    AvgDuration = TimeSpan.FromSeconds(g.Average(s => s.Duration.TotalSeconds))
                                })
                                .Take(10);

        foreach (var stat in levelStats)
        {
            Console.WriteLine($"  Level {stat.Level}: {stat.SessionCount} sessions | Avg Score: {stat.AvgScore:F0} | Avg Duration: {stat.AvgDuration:mm\\:ss}");
        }
    }

    /// <summary>
    /// Search players using LINQ Where() with string matching
    /// </summary>
    public void SearchPlayers(string searchTerm)
    {
        var results = dataService.GetAllPlayers()
                                .Where(p => p.PlayerName.Contains(searchTerm, StringComparison.OrdinalIgnoreCase))
                                .ToList();

        if (!results.Any())
        {
            Console.WriteLine($"No players found matching '{searchTerm}'");
            return;
        }

        Console.WriteLine($"\nSearch Results for '{searchTerm}':");
        Console.WriteLine(new string('-', 75));
        
        foreach (var player in results)
        {
            Console.WriteLine(player.ToString());
        }
    }

    /// <summary>
    /// Filter obstacles by speed range using LINQ Where()
    /// </summary>
    public void FilterObstaclesBySpeed(double minSpeed, double maxSpeed)
    {
        var results = dataService.GetAllObstacles()
                                .Where(o => o.Speed >= minSpeed && o.Speed <= maxSpeed)
                                .OrderByDescending(o => o.Speed)
                                .ToList();

        if (!results.Any())
        {
            Console.WriteLine($"No obstacles found with speed between {minSpeed} and {maxSpeed}");
            return;
        }

        Console.WriteLine($"\nObstacles with speed {minSpeed} - {maxSpeed}:");
        Console.WriteLine(new string('-', 75));
        
        foreach (var obstacle in results)
        {
            Console.WriteLine(obstacle.ToString());
        }
    }

    #endregion
}
