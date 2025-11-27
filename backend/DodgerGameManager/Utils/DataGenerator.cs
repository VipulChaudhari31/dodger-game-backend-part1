using DodgerGameManager.Models;
using DodgerGameManager.Services;

namespace DodgerGameManager.Utils;

/// <summary>
/// Generates random game data for testing and demonstration
/// </summary>
public class DataGenerator
{
    private readonly Random random;
    private readonly GameDataService dataService;

    // Sample data arrays for realistic random generation
    private readonly string[] playerNames = new[]
    {
        "SpaceAce", "MeteorMaster", "DodgeKing", "StarNavigator", "CosmicPilot",
        "NovaHunter", "OrbitRacer", "GalaxyGuardian", "StellarDodger", "AstroNinja",
        "NebulaKnight", "QuasarQueen", "VoidVoyager", "PlanetaryPro", "CometCrusher",
        "LunarLegend", "SolarSurfer", "ZenithZapper", "HorizonHero", "CelestialChamp"
    };

    private readonly string[] obstacleNames = new[]
    {
        "Red Meteor", "Blue Comet", "Asteroid Fragment", "Space Debris", "Ice Crystal",
        "Burning Rock", "Dark Matter", "Plasma Ball", "Cosmic Stone", "Solar Flare",
        "Iron Meteorite", "Crystal Shard", "Frozen Boulder", "Lava Rock", "Neutron Star Chunk"
    };

    private readonly string[] obstacleTypes = new[]
    {
        "Meteor", "Comet", "Asteroid", "Debris", "Crystal"
    };

    private readonly string[] powerUpNames = new[]
    {
        "Shield Boost", "Speed Burst", "Score Multiplier", "Invincibility", "Time Slow",
        "Magnet", "Double Points", "Extra Life", "Turbo Charge", "Star Power",
        "Energy Shield", "Hyper Mode", "Lucky Star", "Power Surge", "Cosmic Blessing"
    };

    private readonly string[] powerUpTypes = new[]
    {
        "Defensive", "Offensive", "Bonus", "Utility", "Special"
    };

    private readonly string[] powerUpEffects = new[]
    {
        "Temporary Shield", "Increased Speed", "2x Score", "Immunity", "Slow Motion",
        "Attract Points", "Double Points", "Extra Life", "Boost Speed", "All Buffs",
        "Damage Protection", "Ultra Fast", "Lucky Bonus", "Power Increase", "Divine Protection"
    };

    private readonly string[] rarities = new[] { "Common", "Uncommon", "Rare", "Epic", "Legendary" };
    private readonly string[] difficulties = new[] { "Easy", "Normal", "Hard", "Expert" };
    private readonly string[] colors = new[] { "Red", "Blue", "Green", "Purple", "Orange", "Yellow", "White", "Black" };

    public DataGenerator(GameDataService service)
    {
        random = new Random();
        dataService = service;
    }

    #region Generate Players

    /// <summary>
    /// Generate a specified number of random players
    /// </summary>
    public void GenerateRandomPlayers(int count)
    {
        Console.WriteLine($"\nGenerating {count} random players...");
        
        var usedNames = new HashSet<string>();
        
        for (int i = 0; i < count; i++)
        {
            // Create unique player names
            string playerName;
            do
            {
                playerName = playerNames[random.Next(playerNames.Length)] + random.Next(100, 999);
            } while (usedNames.Contains(playerName));
            
            usedNames.Add(playerName);
            
            // Generate random stats first
            int totalGamesPlayed = random.Next(5, 100);
            int highestScore = random.Next(100, 15000);
            int totalScore = highestScore + random.Next(1000, 50000);
            DateTime dateRegistered = DateTime.Now.AddDays(-random.Next(1, 365));
            DateTime lastPlayed = DateTime.Now.AddDays(-random.Next(0, 30));
            
            // Determine rank based on high score
            string rank = highestScore switch
            {
                >= 10000 => "Legend",
                >= 5000 => "Master",
                >= 2500 => "Expert",
                >= 1000 => "Advanced",
                >= 500 => "Intermediate",
                _ => "Beginner"
            };
            
            // Create player through the service
            var player = dataService.CreatePlayer(playerName);
            
            // Update with generated statistics
            player.TotalGamesPlayed = totalGamesPlayed;
            player.HighestScore = highestScore;
            player.TotalScore = totalScore;
            player.LastPlayed = lastPlayed;
            player.Rank = rank;
        }
        
        Console.WriteLine($"✓ Successfully generated {count} players");
    }

    #endregion

    #region Generate Game Sessions

    /// <summary>
    /// Generate a specified number of random game sessions
    /// </summary>
    public void GenerateRandomGameSessions(int count)
    {
        var players = dataService.GetAllPlayers();
        if (players.Count == 0)
        {
            Console.WriteLine("⚠ No players found. Generate players first.");
            return;
        }

        Console.WriteLine($"\nGenerating {count} random game sessions...");

        for (int i = 0; i < count; i++)
        {
            // Select random player
            var player = players[random.Next(players.Count)];
            
            // Generate realistic game data
            int score = random.Next(100, 15000);
            int level = (score / 1000) + 1;
            int minutes = random.Next(1, 15);
            int seconds = random.Next(0, 60);
            TimeSpan duration = new TimeSpan(0, minutes, seconds);

            var session = dataService.CreateGameSession(
                player.PlayerId,
                player.PlayerName,
                score,
                level,
                duration
            );

            // Add additional session details
            session.ObstaclesDodged = random.Next(50, 500);
            session.PowerUpsCollected = random.Next(0, 20);
            session.Difficulty = difficulties[random.Next(difficulties.Length)];
            session.SessionDate = DateTime.Now.AddDays(-random.Next(0, 90));
            session.NewHighScore = random.Next(0, 10) < 2; // 20% chance of high score
        }

        Console.WriteLine($"✓ Successfully generated {count} game sessions");
    }

    #endregion

    #region Generate Obstacles

    /// <summary>
    /// Generate a specified number of random obstacles
    /// </summary>
    public void GenerateRandomObstacles(int count)
    {
        Console.WriteLine($"\nGenerating {count} random obstacles...");

        for (int i = 0; i < count; i++)
        {
            string name = obstacleNames[random.Next(obstacleNames.Length)];
            string type = obstacleTypes[random.Next(obstacleTypes.Length)];
            double speed = Math.Round(random.NextDouble() * 5 + 1, 2); // 1.0 to 6.0
            int damage = random.Next(50, 150);
            int size = random.Next(15, 40);

            var obstacle = dataService.CreateObstacle(name, type, speed, damage, size);
            
            obstacle.Color = colors[random.Next(colors.Length)];
            obstacle.PointsOnDodge = (int)(speed * 5) + damage / 10;
            obstacle.IsActive = random.Next(0, 10) < 9; // 90% active
        }

        Console.WriteLine($"✓ Successfully generated {count} obstacles");
    }

    #endregion

    #region Generate PowerUps

    /// <summary>
    /// Generate a specified number of random power-ups
    /// </summary>
    public void GenerateRandomPowerUps(int count)
    {
        Console.WriteLine($"\nGenerating {count} random power-ups...");

        for (int i = 0; i < count; i++)
        {
            string name = powerUpNames[random.Next(powerUpNames.Length)];
            string type = powerUpTypes[random.Next(powerUpTypes.Length)];
            string effect = powerUpEffects[random.Next(powerUpEffects.Length)];
            int duration = random.Next(3, 15);
            int pointsValue = random.Next(50, 500);

            var powerUp = dataService.CreatePowerUp(name, type, effect, duration, pointsValue);
            
            powerUp.Rarity = rarities[random.Next(rarities.Length)];
            powerUp.SpawnRate = Math.Round(random.NextDouble() * 0.3, 3); // 0 to 0.3
            powerUp.IsCollectible = random.Next(0, 10) < 9; // 90% collectible
            
            // Adjust values based on rarity
            switch (powerUp.Rarity)
            {
                case "Legendary":
                    powerUp.PointsValue *= 3;
                    powerUp.SpawnRate *= 0.2;
                    break;
                case "Epic":
                    powerUp.PointsValue *= 2;
                    powerUp.SpawnRate *= 0.5;
                    break;
                case "Rare":
                    powerUp.PointsValue = (int)(powerUp.PointsValue * 1.5);
                    powerUp.SpawnRate *= 0.7;
                    break;
            }
        }

        Console.WriteLine($"✓ Successfully generated {count} power-ups");
    }

    #endregion

    #region Generate Complete Dataset

    /// <summary>
    /// Generate a complete dataset with all entity types
    /// </summary>
    public void GenerateCompleteDataset()
    {
        Console.WriteLine("\n╔════════════════════════════════════════╗");
        Console.WriteLine("║   Generating Complete Sample Dataset   ║");
        Console.WriteLine("╚════════════════════════════════════════╝");

        GenerateRandomPlayers(15);
        GenerateRandomObstacles(20);
        GenerateRandomPowerUps(12);
        GenerateRandomGameSessions(50);

        Console.WriteLine("\n✓ Complete dataset generated successfully!");
        Console.WriteLine($"  - Players: {dataService.GetTotalPlayers()}");
        Console.WriteLine($"  - Obstacles: {dataService.GetTotalObstacles()}");
        Console.WriteLine($"  - Power-ups: {dataService.GetTotalPowerUps()}");
        Console.WriteLine($"  - Game Sessions: {dataService.GetTotalGameSessions()}");
    }

    #endregion
}
