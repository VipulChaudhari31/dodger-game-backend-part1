#Please navigate to backend Folder for Backend Project

# High-Score Dodger Game

A space-themed 2D dodging game built with **HTML5 Canvas**, **CSS**, and **JavaScript**.

## How to Play
- Use **Arrow Keys** to move your spaceship (up, down, left, right).
- Avoid falling red meteors.
- Survive as long as possible to increase your score.
- Progress through levels - speed increases every 1000 points!
- Your best score is saved locally.

## Features
- **Space Theme**: Animated starfield background with spaceship and meteor graphics
- **Progressive Difficulty**: Game speed increases with each level
- **Level System**: Advance through levels every 1000 points
- **Score Tracking**: Real-time score display
- **High Score Persistence**: Best score saved in localStorage
- **Canvas-based Rendering**: Smooth animations with custom shapes
- **Collision Detection**: Precise hit detection system
- **Responsive Controls**: Arrow key movement in all directions

## Game Mechanics
- **Starting Speed**: Obstacles fall at speed 2
- **Level Progression**: Every 1000 points = new level
- **Speed Increase**: +0.5 speed per level
- **Score**: Increases every frame (~60 points per second)

## Run the Game
Just open `index.html` in any modern web browser.

## Testing
Open `test/test.html` in a browser to run the test suite, which includes:
- Level calculation tests
- LocalStorage high score tests

All tests should show green checkmarks (✔ PASS) when successful.
