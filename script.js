const canvas = document.getElementById("gameCanvas");
const ctx = canvas.getContext("2d");

const scoreDisplay = document.getElementById("score");
const highScoreDisplay = document.getElementById("highScore");
const levelDisplay = document.getElementById("level");

let player, obstacles, score, highScore, gameOver, level;

const PLAYER_SIZE = 20;
const OBSTACLE_WIDTH = 20;
const OBSTACLE_HEIGHT = 20;

let keys = {};
let stars = [];

document.addEventListener("keydown", e => keys[e.key] = true);
document.addEventListener("keyup", e => keys[e.key] = false);

document.getElementById("restartBtn").onclick = startGame;

function startGame() { //  Game Initialization
    player = {
        x: canvas.width / 2 - PLAYER_SIZE / 2,
        y: canvas.height - 50,
        speed: 4,
        size: PLAYER_SIZE
    };

    obstacles = [];
    score = 0;
    level = 1;
    gameOver = false;

    highScore = parseInt(localStorage.getItem("dodgerHighScore")) || 0;
    highScoreDisplay.textContent = highScore;
    levelDisplay.textContent = level;
       
    stars = []; // Create background stars
    for (let i = 0; i < 100; i++) {
        stars.push({
            x: Math.random() * canvas.width,
            y: Math.random() * canvas.height,
            size: Math.random() * 2
        });
    }

    requestAnimationFrame(update);
}

function spawnObstacle() { //  Spawning Obstacles
    const newLevel = Math.floor(score / 1000) + 1; // Calculate level and speed (every 1000 points = new level)
    if (newLevel !== level) {
        level = newLevel;
        levelDisplay.textContent = level;
    }
    
    const baseSpeed = 2;
    const speedIncrease = (level - 1) * 0.5;
    const currentSpeed = baseSpeed + speedIncrease;
    
    obstacles.push({
        x: Math.random() * (canvas.width - OBSTACLE_WIDTH),
        y: -OBSTACLE_HEIGHT,
        speed: currentSpeed
    });
}

function movePlayer() { //  Player Movement
    if (keys["ArrowLeft"] && player.x > 0) {
        player.x -= player.speed;
    }
    if (keys["ArrowRight"] && player.x < canvas.width - player.size) {
        player.x += player.speed;
    }
    if (keys["ArrowUp"] && player.y > 0) {
        player.y -= player.speed;
    }
    if (keys["ArrowDown"] && player.y < canvas.height - player.size) {
        player.y += player.speed;
    }
}

function drawPlayer() { //  Drawing Functions
    ctx.fillStyle = "#00e676"; // Draw a spaceship-like triangle
    ctx.beginPath();
    ctx.moveTo(player.x + player.size / 2, player.y); // Top point
    ctx.lineTo(player.x, player.y + player.size); // Bottom left
    ctx.lineTo(player.x + player.size, player.y + player.size); // Bottom right
    ctx.closePath();
    ctx.fill();
    
    ctx.fillStyle = "#00bcd4"; // Add engine glow
    ctx.beginPath();
    ctx.arc(player.x + player.size / 2, player.y + player.size, 3, 0, Math.PI * 2);
    ctx.fill();
}

function drawObstacle(o) {
    ctx.fillStyle = "#ff1744"; // Draw meteor-like shapes
    ctx.beginPath();
      
    const cx = o.x + OBSTACLE_WIDTH / 2; // Irregular hexagon for meteor look
    const cy = o.y + OBSTACLE_HEIGHT / 2;
    const points = 6;
    
    for (let i = 0; i < points; i++) {
        const angle = (i / points) * Math.PI * 2;
        const radius = (OBSTACLE_WIDTH / 2) * (0.8 + Math.random() * 0.4);
        const x = cx + Math.cos(angle) * radius;
        const y = cy + Math.sin(angle) * radius;
        
        if (i === 0) {
            ctx.moveTo(x, y);
        } else {
            ctx.lineTo(x, y);
        }
    }
    
    ctx.closePath();
    ctx.fill();
    
    ctx.fillStyle = "#c41c3b";
    ctx.beginPath();
    ctx.arc(cx, cy, OBSTACLE_WIDTH / 4, 0, Math.PI * 2);
    ctx.fill();
}

function checkCollision(a, b) { //  Collision Detection
    return (
        a.x < b.x + OBSTACLE_WIDTH &&
        a.x + a.size > b.x &&
        a.y < b.y + OBSTACLE_HEIGHT &&
        a.y + a.size > b.y
    );
}

let spawnTimer = 0; //  Game Loop

function update() {
    if (gameOver) return;

    ctx.clearRect(0, 0, canvas.width, canvas.height);
    
    ctx.fillStyle = "#ffffff"; // Draw background stars
    for (let star of stars) {
        ctx.beginPath();
        ctx.arc(star.x, star.y, star.size, 0, Math.PI * 2);
        ctx.fill();
    }

    movePlayer(); // Player movement

    drawPlayer(); // Draw player

    spawnTimer++; // Obstacle spawning
    if (spawnTimer > 45) { 
        spawnObstacle();
        spawnTimer = 0;
    }

    for (let i = obstacles.length - 1; i >= 0; i--) { // Move and draw obstacles
        let o = obstacles[i];
        o.y += o.speed;

        drawObstacle(o);

        if (o.y > canvas.height) obstacles.splice(i, 1); // Remove off-screen obstacles

        if (checkCollision(player, o)) { // Check collision
            gameOver = true;
            handleGameOver();
        }
    }

    score++; // Update score
    scoreDisplay.textContent = score;

    requestAnimationFrame(update);
}

function handleGameOver() {  //  Game Over Handling
    ctx.fillStyle = "rgba(0,0,0,0.6)";
    ctx.fillRect(0, 0, canvas.width, canvas.height);

    ctx.fillStyle = "white";
    ctx.font = "30px Arial";
    ctx.textAlign = "center";
    ctx.fillText("GAME OVER", canvas.width / 2, canvas.height / 2);

    if (score > highScore) {
        localStorage.setItem("dodgerHighScore", score);
        highScoreDisplay.textContent = score;
    }
}


startGame(); // Start game
