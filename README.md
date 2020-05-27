<h1>Longchang Cui's Tetris</h1>
<p>The Tetris project is developed for personal use only.</p>

<div class="row" style="padding-right: 20px;">
<img src="https://github.com/7lc/LongchangCuis_Tetris/blob/master/screenshot1.png" width="20%">
<img src="https://github.com/7lc/LongchangCuis_Tetris/blob/master/screenshot2.png" width="20%">
<img src="https://github.com/7lc/LongchangCuis_Tetris/blob/master/screenshot3.png" width="20%">
</div>

**Overview:**

Longchang Cui's Tetris game is made with Unity and it is production-ready. I implemented the Super Rotation System for the tetromino movement. Players can use advanced techniques like the T-Spin movement to get extra reward points in the game. The current build supports "Hold Piece" and "Show Next Piece" features. Scoring systems and level systems are also included in the game. The game will become more challenging as the player scores more points. Furthermore, I added sound effects, particle effects, UI animations, and royalty-free game music to improve the user experience. The plugins, sample code, and assets that I used in the game development are listed in the reference section.



Development Environment:
Unity Version: 2019.3.7f1
Windows version: Windows 10
Plugins: 
	UniRx
	DOTween

Project Platforms:
The project is primarily built for Android and iOS mobile devices.
The game works best on the device that has the following aspect ratio.

Supported Aspect Ratio: 
16:9,  18:9,   18:10,   19.5:9,   20:9

Project Setting:
The default project build is an android.

Game Demo Video in Unity Editor
https://youtu.be/Ls7VJbtVPyI

Game Controls:
The project supports both desktop keyboard and mobile touch inputs.

Keyboard Controls:
Move Down: Press down arrow
Move Right:  Press right arrow
Move Left:    Press left arrow
Hard Drop:  Press up arrow
90 Degree Left Rotate: Press X key
90 Degree Right Rotate: Press C key
Hold Tetromino: Press Z key

Touch Controls:
Move Down: Tap bottom region of the screen
Move Right:  Tap right region of the screen
Move Left:    Tap left region of the screen
Hard Drop:  Swipe down
90 Degree Left Rotate: Swipe left
90 Degree Right Rotate: Swipe right
Hold Tetromino: Swipe up

Comment:
Some of the sound files, music, and images are from the internet. The authors are credited in the reference section.

Work Flow
CIE Interview Requirements:
End User Requirements
Playfield is 10×40, where rows above 20 are hidden or obstructed by the field frame to trick the player into thinking it's 10×20.

Tetrimino colors are as follows.

    Cyan I
    Yellow O
    Purple T
    Green S
    Red Z
    Blue J
    Orange L

Tetromino start locations

    The I and O spawn in the middle columns
    The rest spawn in the left-middle columns
    The tetriminoes spawn horizontally with J, L and T spawning flat-side first.
    Spawn above playfield, row 21 for I, and 21/22 for all other tetriminoes.
    Immediately drop one space if no existing Block is in its path.

Controls
	
	Rotate 90 degrees clockwise
	Rotate 90 degrees counterclockwise
	Hard drop
	Pause the game
	Move a Tetromino left, right, or down

Rules

	The player tops out (loses) when a piece is spawned overlapping at least one block (block out), or a piece locks completely above the visible portion of the playfield (lock out). 
	A row is cleared and points are scored when the entire row is full of blocks.
	Show the next piece before it drops.
	T-Spin moves score extra points.

Additional Features that are not required but fun to implement
UI animation
MenuScene
Particle effect for line clears
Dynamic Scoring system
Level System
Touch control for mobile devices
Hold tetromino system
Sound effect and background music
Cartoon Sprites
Super Rotation System for tetromino movements
UI is auto-adjusted based on the mobile screen size
7-bag random generator for spawning

Game System Design
The game has two scenes.
MainScene: The starting scene of the game.
Mechanisms:

The player can tap the "Challenge" button to start the game.
Clicking "Credit" button will show the developer's name.

Scripts in the scene:
MenuScene.cs
The script has the button trigger functions that play UI animation and load different scenes.



GameScene: The Tetris game play scene.
GridSystem:

TetrisGrid.cs
The script contains the basic properties of a single unit gird block.
10x20 grid will be populated when the game starts. Each of the grid blocks in the game will have the same property.

GridSystem.cs
The script handles storing tetromino blocks, line cleaning, T-Spin, valid tetromino block checking. The script also checks if a grid cell is empty or occupied.


GameSystem:

GameSystem.cs
GameSystem controls the level up, scoring, and UI animations in the game scene. It also controls

SpawnController.cs
The script handles the spawning tetrominoes and randomg


TetrominoSystem
TetrominoSystem.cs
The script tracks the current tetromino object and tetromino controllers in the grid.






References:


Resources:
Game Scene Background Music:
File Name: bensound-jazzyfrenchy
Author: Benjamin Tissot
License: FREE License with Attribution
Source: https://www.bensound.com/royalty-free-music/track/jazzy-frenchy

Menu Scene Background Music:
File Name: Jazz Game Theme Loop.wav
Author: Mrthenoronha
Source: https://freesound.org/people/Mrthenoronha/sounds/518918/



Sound Effects:

Game Over - Win
https://freesound.org/people/Mrthenoronha/sounds/518306/
Game Over – Lose
 
https://freesound.org/people/Mrthenoronha/sounds/518307/
 
Tetrominoes – Rotation
 
https://www.zapsplat.com/music/blunt-metal-object-single-short-scrape-scratch-on-brick-2/
 
Tetrominoes – Fall
 
https://www.zapsplat.com/music/breeze-block-drop-on-hard-ground-2/
 
Tetrominoes – Clear Line
 
https://www.zapsplat.com/music/science-fiction-beam-up-teleportation-almost-cartoon-like/
 
UI Button - Click
https://www.zapsplat.com/music/rollover-or-ui-tone-short-metal-click/


Plugins and Assets:
Icons -  simple button set 01:
       https://assetstore.unity.com/packages/2d/gui/icons/simple-button-set-01-153979

2. UniRX:
       https://assetstore.unity.com/packages/tools/integration/unirx-reactive-extensions-for-unity-17276


Background Image:
Background Image 1
https://wallpaperaccess.com/kawaii-black#1606655

Background Image 2
https://wallpaperaccess.com/kawaii-black#305261

Background Image 3
https://wallpaperaccess.com/kawaii-black#2650630
