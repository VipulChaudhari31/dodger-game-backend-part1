using System.Text.Json;
using DodgerGameManager.Models;
using DodgerGameManager.Services;

namespace DodgerGameManager.Data;

/// <summary>
/// Handles data persistence using JSON file storage
/// Demonstrates: Data Persistence, File I/O, Understanding Data Management
/// Learning Outcome: Understanding Data Management
/// </summary>
public class DataPersistenceService
{
    private readonly GameDataService dataService;
    private readonly string dataDirectory;
    private readonly string playersFile;
    private readonly string sessionsFile;
    private readonly string obstaclesFile;
    private readonly string powerUpsFile;

    private readonly JsonSerializerOptions jsonOptions;

    public DataPersistenceService(GameDataService service)
    {
        dataService = service;
        dataDirectory = Path.Combine(Directory.GetCurrentDirectory(), "GameData");
        playersFile = Path.Combine(dataDirectory, "players.json");
        sessionsFile = Path.Combine(dataDirectory, "sessions.json");
        obstaclesFile = Path.Combine(dataDirectory, "obstacles.json");
        powerUpsFile = Path.Combine(dataDirectory, "powerups.json");

        jsonOptions = new JsonSerializerOptions
        {
            WriteIndented = true,
            PropertyNameCaseInsensitive = true
        };

        EnsureDataDirectoryExists();
    }

    private void EnsureDataDirectoryExists()
    {
        if (!Directory.Exists(dataDirectory))
        {
            Directory.CreateDirectory(dataDirectory);
        }
    }

    #region Save Data

    public async Task<bool> SaveAllDataAsync()
    {
        try
        {
            Console.WriteLine("\n💾 Saving all data to files...");

            await SavePlayersAsync();
            await SaveGameSessionsAsync();
            await SaveObstaclesAsync();
            await SavePowerUpsAsync();

            Console.WriteLine("✓ All data saved successfully!");
            Console.WriteLine($"📁 Data location: {dataDirectory}");
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"✗ Error saving data: {ex.Message}");
            return false;
        }
    }

    private async Task SavePlayersAsync()
    {
        var players = dataService.GetAllPlayers();
        var json = JsonSerializer.Serialize(players, jsonOptions);
        await File.WriteAllTextAsync(playersFile, json);
        Console.WriteLine($"  ✓ Saved {players.Count} players");
    }

    private async Task SaveGameSessionsAsync()
    {
        var sessions = dataService.GetAllGameSessions();
        var json = JsonSerializer.Serialize(sessions, jsonOptions);
        await File.WriteAllTextAsync(sessionsFile, json);
        Console.WriteLine($"  ✓ Saved {sessions.Count} game sessions");
    }

    private async Task SaveObstaclesAsync()
    {
        var obstacles = dataService.GetAllObstacles();
        var json = JsonSerializer.Serialize(obstacles, jsonOptions);
        await File.WriteAllTextAsync(obstaclesFile, json);
        Console.WriteLine($"  ✓ Saved {obstacles.Count} obstacles");
    }

    private async Task SavePowerUpsAsync()
    {
        var powerUps = dataService.GetAllPowerUps();
        var json = JsonSerializer.Serialize(powerUps, jsonOptions);
        await File.WriteAllTextAsync(powerUpsFile, json);
        Console.WriteLine($"  ✓ Saved {powerUps.Count} power-ups");
    }

    #endregion

    #region Load Data

    public async Task<bool> LoadAllDataAsync()
    {
        try
        {
            Console.WriteLine("\n📂 Loading data from files...");

            bool hasData = false;

            if (File.Exists(playersFile))
            {
                await LoadPlayersAsync();
                hasData = true;
            }

            if (File.Exists(obstaclesFile))
            {
                await LoadObstaclesAsync();
                hasData = true;
            }

            if (File.Exists(powerUpsFile))
            {
                await LoadPowerUpsAsync();
                hasData = true;
            }

            if (File.Exists(sessionsFile))
            {
                await LoadGameSessionsAsync();
                hasData = true;
            }

            if (hasData)
            {
                Console.WriteLine("✓ Data loaded successfully!");
                Console.WriteLine($"  Players: {dataService.GetTotalPlayers()}");
                Console.WriteLine($"  Obstacles: {dataService.GetTotalObstacles()}");
                Console.WriteLine($"  Power-ups: {dataService.GetTotalPowerUps()}");
                Console.WriteLine($"  Game Sessions: {dataService.GetTotalGameSessions()}");
            }
            else
            {
                Console.WriteLine("⚠ No saved data found.");
            }

            return hasData;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"✗ Error loading data: {ex.Message}");
            return false;
        }
    }

    private async Task LoadPlayersAsync()
    {
        var json = await File.ReadAllTextAsync(playersFile);
        var players = JsonSerializer.Deserialize<List<Player>>(json, jsonOptions);
        
        if (players != null)
        {
            foreach (var player in players)
            {
                var newPlayer = dataService.CreatePlayer(player.PlayerName);
                dataService.UpdatePlayer(newPlayer.PlayerId, p =>
                {
                    p.TotalGamesPlayed = player.TotalGamesPlayed;
                    p.HighestScore = player.HighestScore;
                    p.TotalScore = player.TotalScore;
                    p.Rank = player.Rank;
                    p.LastPlayed = player.LastPlayed;
                });
            }
        }
    }

    private async Task LoadGameSessionsAsync()
    {
        var json = await File.ReadAllTextAsync(sessionsFile);
        var sessions = JsonSerializer.Deserialize<List<GameSession>>(json, jsonOptions);
        
        if (sessions != null)
        {
            foreach (var session in sessions)
            {
                // Skip if player doesn't exist
                if (dataService.GetPlayerById(session.PlayerId) == null) continue;

                var newSession = dataService.CreateGameSession(
                    session.PlayerId,
                    session.PlayerName,
                    session.Score,
                    session.Level,
                    session.Duration
                );
                
                dataService.UpdateGameSession(newSession.SessionId, s =>
                {
                    s.ObstaclesDodged = session.ObstaclesDodged;
                    s.PowerUpsCollected = session.PowerUpsCollected;
                    s.Difficulty = session.Difficulty;
                    s.SessionDate = session.SessionDate;
                    s.NewHighScore = session.NewHighScore;
                });
            }
        }
    }

    private async Task LoadObstaclesAsync()
    {
        var json = await File.ReadAllTextAsync(obstaclesFile);
        var obstacles = JsonSerializer.Deserialize<List<Obstacle>>(json, jsonOptions);
        
        if (obstacles != null)
        {
            foreach (var obstacle in obstacles)
            {
                var newObstacle = dataService.CreateObstacle(
                    obstacle.ObstacleName,
                    obstacle.ObstacleType,
                    obstacle.Speed,
                    obstacle.DamagePoints,
                    obstacle.Size
                );
                
                dataService.UpdateObstacle(newObstacle.ObstacleId, o =>
                {
                    o.Color = obstacle.Color;
                    o.PointsOnDodge = obstacle.PointsOnDodge;
                    o.IsActive = obstacle.IsActive;
                });
            }
        }
    }

    private async Task LoadPowerUpsAsync()
    {
        var json = await File.ReadAllTextAsync(powerUpsFile);
        var powerUps = JsonSerializer.Deserialize<List<PowerUp>>(json, jsonOptions);
        
        if (powerUps != null)
        {
            foreach (var powerUp in powerUps)
            {
                var newPowerUp = dataService.CreatePowerUp(
                    powerUp.PowerUpName,
                    powerUp.PowerUpType,
                    powerUp.Effect,
                    powerUp.DurationSeconds,
                    powerUp.PointsValue
                );
                
                dataService.UpdatePowerUp(newPowerUp.PowerUpId, p =>
                {
                    p.Rarity = powerUp.Rarity;
                    p.SpawnRate = powerUp.SpawnRate;
                    p.IsCollectible = powerUp.IsCollectible;
                });
            }
        }
    }

    #endregion

    #region Export Data

    public async Task<bool> ExportDataToSingleFileAsync(string fileName = "game_data_export.json")
    {
        try
        {
            var exportPath = Path.Combine(dataDirectory, fileName);
            
            var exportData = new
            {
                ExportDate = DateTime.Now,
                Players = dataService.GetAllPlayers(),
                GameSessions = dataService.GetAllGameSessions(),
                Obstacles = dataService.GetAllObstacles(),
                PowerUps = dataService.GetAllPowerUps()
            };

            var json = JsonSerializer.Serialize(exportData, jsonOptions);
            await File.WriteAllTextAsync(exportPath, json);

            Console.WriteLine($"\n✓ Data exported to: {exportPath}");
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"✗ Error exporting data: {ex.Message}");
            return false;
        }
    }

    #endregion

    public bool HasSavedData()
    {
        return File.Exists(playersFile) || 
               File.Exists(sessionsFile) || 
               File.Exists(obstaclesFile) || 
               File.Exists(powerUpsFile);
    }
}
