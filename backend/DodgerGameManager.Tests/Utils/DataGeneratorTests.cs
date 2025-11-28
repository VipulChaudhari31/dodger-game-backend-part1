using DodgerGameManager.Services;
using DodgerGameManager.Utils;

namespace DodgerGameManager.Tests.Utils;

public class DataGeneratorTests
{
    private GameDataService _dataService;
    private DataGenerator _generator;

    public DataGeneratorTests()
    {
        _dataService = new GameDataService();
        _generator = new DataGenerator(_dataService);
    }

    [Fact]
    public void GenerateRandomPlayers_ShouldCreateSpecifiedNumberOfPlayers()
    {
        // Act
        _generator.GenerateRandomPlayers(10);

        // Assert
        var players = _dataService.GetAllPlayers();
        Assert.Equal(10, players.Count);
    }

    [Fact]
    public void GenerateRandomPlayers_ShouldCreatePlayersWithUniqueNames()
    {
        // Act
        _generator.GenerateRandomPlayers(15);

        // Assert
        var players = _dataService.GetAllPlayers();
        var uniqueNames = players.Select(p => p.PlayerName).Distinct().Count();
        Assert.Equal(15, uniqueNames);
    }

    [Fact]
    public void GenerateRandomPlayers_ShouldAssignValidRanks()
    {
        // Act
        _generator.GenerateRandomPlayers(10);

        // Assert
        var players = _dataService.GetAllPlayers();
        var validRanks = new[] { "Beginner", "Intermediate", "Advanced", "Expert", "Elite", "Master", "Legend" };
        
        foreach (var player in players)
        {
            Assert.Contains(player.Rank, validRanks);
        }
    }

    [Fact]
    public void GenerateRandomObstacles_ShouldCreateSpecifiedNumberOfObstacles()
    {
        // Act
        _generator.GenerateRandomObstacles(20);

        // Assert
        var obstacles = _dataService.GetAllObstacles();
        Assert.Equal(20, obstacles.Count);
    }

    [Fact]
    public void GenerateRandomObstacles_ShouldHaveValidTypes()
    {
        // Act
        _generator.GenerateRandomObstacles(15);

        // Assert
        var obstacles = _dataService.GetAllObstacles();
        var validTypes = new[] { "Meteor", "Asteroid", "Comet", "Debris", "Crystal" };
        
        foreach (var obstacle in obstacles)
        {
            Assert.Contains(obstacle.ObstacleType, validTypes);
        }
    }

    [Fact]
    public void GenerateRandomObstacles_ShouldHavePositiveSpeed()
    {
        // Act
        _generator.GenerateRandomObstacles(10);

        // Assert
        var obstacles = _dataService.GetAllObstacles();
        foreach (var obstacle in obstacles)
        {
            Assert.True(obstacle.Speed > 0);
            Assert.True(obstacle.Speed <= 10);
        }
    }

    [Fact]
    public void GenerateRandomPowerUps_ShouldCreateSpecifiedNumber()
    {
        // Act
        _generator.GenerateRandomPowerUps(12);

        // Assert
        var powerUps = _dataService.GetAllPowerUps();
        Assert.Equal(12, powerUps.Count);
    }

    [Fact]
    public void GenerateRandomPowerUps_ShouldHaveValidTypes()
    {
        // Act
        _generator.GenerateRandomPowerUps(10);

        // Assert
        var powerUps = _dataService.GetAllPowerUps();
        var validTypes = new[] { "Defensive", "Offensive", "Bonus", "Utility", "Special" };
        
        foreach (var powerUp in powerUps)
        {
            Assert.Contains(powerUp.PowerUpType, validTypes);
        }
    }

    [Fact]
    public void GenerateRandomPowerUps_ShouldHaveValidRarity()
    {
        // Act
        _generator.GenerateRandomPowerUps(15);

        // Assert
        var powerUps = _dataService.GetAllPowerUps();
        var validRarities = new[] { "Common", "Uncommon", "Rare", "Epic", "Legendary" };
        
        foreach (var powerUp in powerUps)
        {
            Assert.Contains(powerUp.Rarity, validRarities);
        }
    }

    [Fact]
    public void GenerateRandomGameSessions_ShouldRequireExistingPlayers()
    {
        // Act
        _generator.GenerateRandomGameSessions(10);

        // Assert - Should create 0 sessions if no players exist
        var sessions = _dataService.GetAllGameSessions();
        Assert.Empty(sessions);
    }

    [Fact]
    public void GenerateRandomGameSessions_ShouldCreateSessionsWithPlayers()
    {
        // Arrange
        _generator.GenerateRandomPlayers(5);

        // Act
        _generator.GenerateRandomGameSessions(20);

        // Assert
        var sessions = _dataService.GetAllGameSessions();
        Assert.Equal(20, sessions.Count);
        
        // All sessions should reference existing players
        var playerIds = _dataService.GetAllPlayers().Select(p => p.PlayerId).ToHashSet();
        foreach (var session in sessions)
        {
            Assert.Contains(session.PlayerId, playerIds);
        }
    }

    [Fact]
    public void GenerateRandomGameSessions_ShouldHaveValidDifficulties()
    {
        // Arrange
        _generator.GenerateRandomPlayers(3);

        // Act
        _generator.GenerateRandomGameSessions(15);

        // Assert
        var sessions = _dataService.GetAllGameSessions();
        var validDifficulties = new[] { "Easy", "Normal", "Hard", "Expert" };
        
        foreach (var session in sessions)
        {
            Assert.Contains(session.Difficulty, validDifficulties);
        }
    }

    [Fact]
    public void GenerateCompleteDataset_ShouldCreateAllEntityTypes()
    {
        // Act
        _generator.GenerateCompleteDataset();

        // Assert
        Assert.Equal(15, _dataService.GetTotalPlayers());
        Assert.Equal(20, _dataService.GetTotalObstacles());
        Assert.Equal(12, _dataService.GetTotalPowerUps());
        Assert.Equal(50, _dataService.GetTotalGameSessions());
    }

    [Fact]
    public void GeneratedPlayers_ShouldHaveReasonableStats()
    {
        // Act
        _generator.GenerateRandomPlayers(10);

        // Assert
        var players = _dataService.GetAllPlayers();
        foreach (var player in players)
        {
            Assert.True(player.TotalGamesPlayed >= 0);
            Assert.True(player.HighestScore >= 0);
            Assert.True(player.TotalScore >= 0);
            Assert.NotNull(player.PlayerName);
            Assert.NotEmpty(player.PlayerName);
        }
    }
}
