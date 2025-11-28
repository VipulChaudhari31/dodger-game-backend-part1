using DodgerGameManager.Services;
using DodgerGameManager.Models;

namespace DodgerGameManager.Tests.Services;

public class GameDataServiceTests
{
    private GameDataService _service;

    public GameDataServiceTests()
    {
        _service = new GameDataService();
    }

    #region Player Tests

    [Fact]
    public void CreatePlayer_ShouldAddPlayerToList()
    {
        // Arrange
        var playerName = "TestPlayer";

        // Act
        var player = _service.CreatePlayer(playerName);

        // Assert
        Assert.NotNull(player);
        Assert.Equal(playerName, player.PlayerName);
        Assert.Equal(1, player.PlayerId);
        Assert.Equal("Beginner", player.Rank);
        Assert.Single(_service.GetAllPlayers());
    }

    [Fact]
    public void CreatePlayer_ShouldAutoIncrementPlayerId()
    {
        // Act
        var player1 = _service.CreatePlayer("Player1");
        var player2 = _service.CreatePlayer("Player2");
        var player3 = _service.CreatePlayer("Player3");

        // Assert
        Assert.Equal(1, player1.PlayerId);
        Assert.Equal(2, player2.PlayerId);
        Assert.Equal(3, player3.PlayerId);
    }

    [Fact]
    public void GetAllPlayers_ShouldReturnEmptyListInitially()
    {
        // Act
        var players = _service.GetAllPlayers();

        // Assert
        Assert.NotNull(players);
        Assert.Empty(players);
    }

    [Fact]
    public void GetPlayerById_ShouldReturnCorrectPlayer()
    {
        // Arrange
        var player = _service.CreatePlayer("TestPlayer");

        // Act
        var found = _service.GetPlayerById(player.PlayerId);

        // Assert
        Assert.NotNull(found);
        Assert.Equal(player.PlayerId, found.PlayerId);
        Assert.Equal(player.PlayerName, found.PlayerName);
    }

    [Fact]
    public void GetPlayerById_ShouldReturnNullForNonExistentId()
    {
        // Act
        var found = _service.GetPlayerById(999);

        // Assert
        Assert.Null(found);
    }

    [Fact]
    public void UpdatePlayer_ShouldModifyPlayerProperties()
    {
        // Arrange
        var player = _service.CreatePlayer("OriginalName");
        var playerId = player.PlayerId;

        // Act
        _service.UpdatePlayer(playerId, p =>
        {
            p.PlayerName = "UpdatedName";
            p.HighestScore = 5000;
            p.TotalGamesPlayed = 10;
        });

        // Assert
        var updated = _service.GetPlayerById(playerId);
        Assert.Equal("UpdatedName", updated.PlayerName);
        Assert.Equal(5000, updated.HighestScore);
        Assert.Equal(10, updated.TotalGamesPlayed);
    }

    [Fact]
    public void UpdatePlayer_ShouldUpdateRankBasedOnScore()
    {
        // Arrange
        var player = _service.CreatePlayer("TestPlayer");
        var playerId = player.PlayerId;

        // Act - Create a game session which will update rank automatically
        _service.CreateGameSession(playerId, player.PlayerName, 12000, 12, TimeSpan.FromMinutes(10));

        // Assert
        var updated = _service.GetPlayerById(playerId);
        Assert.Equal("Legend", updated!.Rank);
    }

    [Fact]
    public void DeletePlayer_ShouldRemovePlayerFromList()
    {
        // Arrange
        var player = _service.CreatePlayer("TestPlayer");
        var playerId = player.PlayerId;

        // Act
        var result = _service.DeletePlayer(playerId);

        // Assert
        Assert.True(result);
        Assert.Empty(_service.GetAllPlayers());
        Assert.Null(_service.GetPlayerById(playerId));
    }

    [Fact]
    public void DeletePlayer_ShouldReturnFalseForNonExistentId()
    {
        // Act
        var result = _service.DeletePlayer(999);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void GetTotalPlayers_ShouldReturnCorrectCount()
    {
        // Arrange
        _service.CreatePlayer("Player1");
        _service.CreatePlayer("Player2");
        _service.CreatePlayer("Player3");

        // Act
        var count = _service.GetTotalPlayers();

        // Assert
        Assert.Equal(3, count);
    }

    #endregion

    #region GameSession Tests

    [Fact]
    public void CreateGameSession_ShouldAddSessionToList()
    {
        // Arrange
        var player = _service.CreatePlayer("TestPlayer");

        // Act
        var session = _service.CreateGameSession(
            player.PlayerId, 
            player.PlayerName, 
            1000, 
            5, 
            TimeSpan.FromMinutes(10)
        );

        // Assert
        Assert.NotNull(session);
        Assert.Equal(1, session.SessionId);
        Assert.Equal(player.PlayerId, session.PlayerId);
        Assert.Equal(1000, session.Score);
        Assert.Equal(5, session.Level);
        Assert.Single(_service.GetAllGameSessions());
    }

    [Fact]
    public void CreateGameSession_ShouldUpdatePlayerStats()
    {
        // Arrange
        var player = _service.CreatePlayer("TestPlayer");
        var initialGamesPlayed = player.TotalGamesPlayed;

        // Act
        _service.CreateGameSession(
            player.PlayerId,
            player.PlayerName,
            3000,
            3,
            TimeSpan.FromMinutes(5)
        );

        // Assert
        var updatedPlayer = _service.GetPlayerById(player.PlayerId);
        Assert.Equal(3000, updatedPlayer.HighestScore);
        Assert.Equal(3000, updatedPlayer.TotalScore);
        Assert.Equal(initialGamesPlayed + 1, updatedPlayer.TotalGamesPlayed);
    }

    [Fact]
    public void GetGameSessionById_ShouldReturnCorrectSession()
    {
        // Arrange
        var player = _service.CreatePlayer("TestPlayer");
        var session = _service.CreateGameSession(
            player.PlayerId,
            player.PlayerName,
            1500,
            2,
            TimeSpan.FromMinutes(7)
        );

        // Act
        var found = _service.GetGameSessionById(session.SessionId);

        // Assert
        Assert.NotNull(found);
        Assert.Equal(session.SessionId, found.SessionId);
        Assert.Equal(1500, found.Score);
    }

    [Fact]
    public void DeleteGameSession_ShouldRemoveSessionFromList()
    {
        // Arrange
        var player = _service.CreatePlayer("TestPlayer");
        var session = _service.CreateGameSession(
            player.PlayerId,
            player.PlayerName,
            1000,
            1,
            TimeSpan.FromMinutes(5)
        );

        // Act
        var result = _service.DeleteGameSession(session.SessionId);

        // Assert
        Assert.True(result);
        Assert.Empty(_service.GetAllGameSessions());
    }

    #endregion

    #region Obstacle Tests

    [Fact]
    public void CreateObstacle_ShouldAddObstacleToList()
    {
        // Act
        var obstacle = _service.CreateObstacle(
            "Test Meteor",
            "Meteor",
            3.5,
            150,
            25
        );

        // Assert
        Assert.NotNull(obstacle);
        Assert.Equal(1, obstacle.ObstacleId);
        Assert.Equal("Test Meteor", obstacle.ObstacleName);
        Assert.Equal(3.5, obstacle.Speed);
        Assert.True(obstacle.IsActive);
        Assert.Single(_service.GetAllObstacles());
    }

    [Fact]
    public void GetObstacleById_ShouldReturnCorrectObstacle()
    {
        // Arrange
        var obstacle = _service.CreateObstacle("Fast Meteor", "Meteor", 5.0, 200, 30);

        // Act
        var found = _service.GetObstacleById(obstacle.ObstacleId);

        // Assert
        Assert.NotNull(found);
        Assert.Equal("Fast Meteor", found.ObstacleName);
        Assert.Equal(5.0, found.Speed);
    }

    [Fact]
    public void DeleteObstacle_ShouldRemoveObstacleFromList()
    {
        // Arrange
        var obstacle = _service.CreateObstacle("Temp Meteor", "Meteor", 2.0, 100, 20);

        // Act
        var result = _service.DeleteObstacle(obstacle.ObstacleId);

        // Assert
        Assert.True(result);
        Assert.Empty(_service.GetAllObstacles());
    }

    #endregion

    #region PowerUp Tests

    [Fact]
    public void CreatePowerUp_ShouldAddPowerUpToList()
    {
        // Act
        var powerUp = _service.CreatePowerUp(
            "Test Shield",
            "Defensive",
            "Temporary Invincibility",
            10,
            500
        );

        // Assert
        Assert.NotNull(powerUp);
        Assert.Equal(1, powerUp.PowerUpId);
        Assert.Equal("Test Shield", powerUp.PowerUpName);
        Assert.Equal("Defensive", powerUp.PowerUpType);
        Assert.True(powerUp.IsCollectible);
        Assert.Single(_service.GetAllPowerUps());
    }

    [Fact]
    public void GetPowerUpById_ShouldReturnCorrectPowerUp()
    {
        // Arrange
        var powerUp = _service.CreatePowerUp(
            "Speed Boost",
            "Offensive",
            "Increase Speed",
            5,
            300
        );

        // Act
        var found = _service.GetPowerUpById(powerUp.PowerUpId);

        // Assert
        Assert.NotNull(found);
        Assert.Equal("Speed Boost", found.PowerUpName);
        Assert.Equal(300, found.PointsValue);
    }

    [Fact]
    public void DeletePowerUp_ShouldRemovePowerUpFromList()
    {
        // Arrange
        var powerUp = _service.CreatePowerUp(
            "Temp Boost",
            "Bonus",
            "Extra Points",
            3,
            100
        );

        // Act
        var result = _service.DeletePowerUp(powerUp.PowerUpId);

        // Assert
        Assert.True(result);
        Assert.Empty(_service.GetAllPowerUps());
    }

    #endregion

    #region Clear Data Tests

    [Fact]
    public void ClearAllData_ShouldRemoveAllEntities()
    {
        // Arrange
        var player = _service.CreatePlayer("TestPlayer");
        _service.CreateGameSession(player.PlayerId, player.PlayerName, 1000, 1, TimeSpan.FromMinutes(5));
        _service.CreateObstacle("Meteor", "Meteor", 3.0, 100, 20);
        _service.CreatePowerUp("Shield", "Defensive", "Protection", 5, 200);

        // Act
        _service.ClearAllData();

        // Assert
        Assert.Empty(_service.GetAllPlayers());
        Assert.Empty(_service.GetAllGameSessions());
        Assert.Empty(_service.GetAllObstacles());
        Assert.Empty(_service.GetAllPowerUps());
    }

    #endregion
}
