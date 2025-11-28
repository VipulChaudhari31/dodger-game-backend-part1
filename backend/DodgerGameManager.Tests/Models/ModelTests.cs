using DodgerGameManager.Models;

namespace DodgerGameManager.Tests.Models;

public class PlayerTests
{
    [Fact]
    public void Player_ShouldCalculateAverageScore()
    {
        // Arrange
        var player = new Player
        {
            PlayerId = 1,
            PlayerName = "TestPlayer",
            TotalGamesPlayed = 10,
            TotalScore = 5000,
            HighestScore = 1000,
            DateRegistered = DateTime.Now,
            LastPlayed = DateTime.Now,
            Rank = "Advanced"
        };

        // Act
        var avgScore = player.GetAverageScore();

        // Assert
        Assert.Equal(500, avgScore);
    }

    [Fact]
    public void Player_GetAverageScore_ShouldReturnZeroWhenNoGamesPlayed()
    {
        // Arrange
        var player = new Player
        {
            PlayerId = 1,
            PlayerName = "NewPlayer",
            TotalGamesPlayed = 0,
            TotalScore = 0,
            HighestScore = 0,
            DateRegistered = DateTime.Now,
            LastPlayed = DateTime.Now,
            Rank = "Beginner"
        };

        // Act
        var avgScore = player.GetAverageScore();

        // Assert
        Assert.Equal(0, avgScore);
    }

    [Fact]
    public void Player_ToString_ShouldContainPlayerDetails()
    {
        // Arrange
        var player = new Player
        {
            PlayerId = 1,
            PlayerName = "TestPlayer",
            TotalGamesPlayed = 5,
            TotalScore = 2500,
            HighestScore = 800,
            DateRegistered = DateTime.Now,
            LastPlayed = DateTime.Now,
            Rank = "Intermediate"
        };

        // Act
        var result = player.ToString();

        // Assert
        Assert.Contains("TestPlayer", result);
        Assert.Contains("Intermediate", result);
        Assert.Contains("800", result);
    }
}

public class GameSessionTests
{
    [Fact]
    public void GameSession_ShouldCalculateScorePerMinute()
    {
        // Arrange
        var session = new GameSession
        {
            SessionId = 1,
            PlayerId = 1,
            Score = 3000,
            Level = 5,
            Duration = TimeSpan.FromMinutes(10),
            ObstaclesDodged = 50,
            PowerUpsCollected = 5,
            NewHighScore = false,
            Difficulty = "Normal"
        };

        // Act
        var scorePerMinute = session.GetScorePerMinute();

        // Assert
        Assert.Equal(300, scorePerMinute);
    }

    [Fact]
    public void GameSession_GetScorePerMinute_ShouldReturnZeroForZeroDuration()
    {
        // Arrange
        var session = new GameSession
        {
            SessionId = 1,
            PlayerId = 1,
            Score = 1000,
            Level = 1,
            Duration = TimeSpan.Zero,
            ObstaclesDodged = 10,
            PowerUpsCollected = 2,
            NewHighScore = false,
            Difficulty = "Easy"
        };

        // Act
        var scorePerMinute = session.GetScorePerMinute();

        // Assert
        Assert.Equal(0, scorePerMinute);
    }

    [Fact]
    public void GameSession_ToString_ShouldContainSessionDetails()
    {
        // Arrange
        var session = new GameSession
        {
            SessionId = 1,
            PlayerId = 1,
            Score = 5000,
            Level = 7,
            Duration = TimeSpan.FromMinutes(15),
            ObstaclesDodged = 100,
            PowerUpsCollected = 10,
            NewHighScore = true,
            Difficulty = "Hard"
        };

        // Act
        var result = session.ToString();

        // Assert
        Assert.Contains("5000", result);
        Assert.Contains("7", result);
        // ToString doesn't include Difficulty in output
    }
}

public class ObstacleTests
{
    [Fact]
    public void Obstacle_ShouldHaveCorrectProperties()
    {
        // Arrange & Act
        var obstacle = new Obstacle
        {
            ObstacleId = 1,
            ObstacleName = "Fast Meteor",
            ObstacleType = "Meteor",
            Speed = 5.5,
            DamagePoints = 200,
            Size = 30,
            Color = "Red",
            PointsOnDodge = 150,
            IsActive = true
        };

        // Assert
        Assert.Equal(1, obstacle.ObstacleId);
        Assert.Equal("Fast Meteor", obstacle.ObstacleName);
        Assert.Equal(5.5, obstacle.Speed);
        Assert.True(obstacle.IsActive);
    }

    [Fact]
    public void Obstacle_ToString_ShouldContainObstacleDetails()
    {
        // Arrange
        var obstacle = new Obstacle
        {
            ObstacleId = 1,
            ObstacleName = "Giant Asteroid",
            ObstacleType = "Asteroid",
            Speed = 3.0,
            DamagePoints = 300,
            Size = 50,
            Color = "Gray",
            PointsOnDodge = 200,
            IsActive = true
        };

        // Act
        var result = obstacle.ToString();

        // Assert
        Assert.Contains("Giant Asteroid", result);
        Assert.Contains("Asteroid", result);
        Assert.Contains("3", result);
    }
}

public class PowerUpTests
{
    [Fact]
    public void PowerUp_ShouldHaveCorrectProperties()
    {
        // Arrange & Act
        var powerUp = new PowerUp
        {
            PowerUpId = 1,
            PowerUpName = "Shield",
            PowerUpType = "Defensive",
            DurationSeconds = 10,
            PointsValue = 500,
            Effect = "Invincibility",
            SpawnRate = 0.15,
            Rarity = "Rare",
            IsCollectible = true
        };

        // Assert
        Assert.Equal(1, powerUp.PowerUpId);
        Assert.Equal("Shield", powerUp.PowerUpName);
        Assert.Equal(500, powerUp.PointsValue);
        Assert.Equal("Rare", powerUp.Rarity);
        Assert.True(powerUp.IsCollectible);
    }

    [Fact]
    public void PowerUp_ToString_ShouldContainPowerUpDetails()
    {
        // Arrange
        var powerUp = new PowerUp
        {
            PowerUpId = 1,
            PowerUpName = "Speed Boost",
            PowerUpType = "Offensive",
            DurationSeconds = 5,
            PointsValue = 300,
            Effect = "Double Speed",
            SpawnRate = 0.2,
            Rarity = "Common",
            IsCollectible = true
        };

        // Act
        var result = powerUp.ToString();

        // Assert
        Assert.Contains("Speed Boost", result);
        Assert.Contains("Offensive", result);
        Assert.Contains("300", result);
    }
}
