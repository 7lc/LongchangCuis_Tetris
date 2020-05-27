<h1>Longchang Cui's Tetris</h1>
<p>The Tetris project is developed for personal use only.</p>

<div class="row" style="padding-right: 20px;">
    <img src="https://github.com/7lc/LongchangCuis_Tetris/blob/master/screenshot2.png" width="19.6%">
    <img src="https://github.com/7lc/LongchangCuis_Tetris/blob/master/screenshot1.png" width="20%">
    <img src="https://github.com/7lc/LongchangCuis_Tetris/blob/master/screenshot3.png" width="20%">
</div>

## Overview

Longchang Cui's Tetris game is made with Unity and it is production-ready. I implemented the Super Rotation System for the tetromino movement. Players can use advanced techniques like the T-Spin movement to get extra reward points in the game. The current build supports "Hold Piece" and "Show Next Piece" features. Scoring systems and level systems are also included in the game. The game will become more challenging as the player scores more points. Furthermore, I added sound effects, particle effects, UI animations, and royalty-free game music to improve the user experience. The plugins, sample code, and assets that I used in the game development are listed in the reference section.

**Development Environment:**
WARNING! Importing the project with different Unity versions and different OS systems may cause errors. 
Comment: I used Mac OS with Unity version 2019.3.7f1 to open the project, but it failed to import the correct package. I also tried opening the project with Unity version 2019.3.14 on Mac, this time Unity successfully opened the project without any errors.

To minimize the error, please use the following settings to open the project.

Unity Version: 2019.3.7f1

Windows version: Windows 10

Plugins:
 - UniRx
 - DOTween


**Supported Platforms:**

The project is primarily built for Android and iOS mobile devices.
The game works best on mobile devices that have the following aspect ratio.

 - 16:9
 - 18:9
 - 18:10
 - 19.5:9
 - 20:9

The desktop keyboard controls are supported as a default feature. You can easily switch the build version in Unity to play the game in PC or Mac.

**Project Setting:**

The default project build is Android.


**Game Demo Video:**

(1 minute T-Spin and Level Up demo video)

https://youtu.be/Ls7VJbtVPyI


**Game Controls:**

The project supports both desktop keyboard and mobile touch inputs.

|  |Keyboard | Touch Control |
|--|--|--|
| Move Down| Down Arrow | Tap bottom region of the screen |
| Move Right|Right Arrow | Tap right region of the screen|
|Move Left|Left Arrow| Tap left region of the screen
|Hard Drop|Up Arrow|Swipe Down
|90 Degree Left Rotate| X| Swipe Left
|90 Degree Right Rotate|C| Swipe Right
|Hold Tetromino|Z|Swipe Up

<img src = "https://media.githubusercontent.com/media/7lc/LongchangCuis_Tetris/master/Tetris_TouchTutorial.png" width="25%"></img>




# WorkFlow

 <h3>Requirements:</h3>
 
**End user requirements:**
 1. Playfield is 10×40, where rows above 20 are hidden or obstructed by the field frame to trick the player into thinking it's 10×20.
 2. Tetrimino colors are as follows. Cyan I, Yellow O, Purple T, Green S, Red Z, Blue J, Orange L
 3. Tetromino start locations: The I and O spawn in the middle columns. The rest spawn in the left-middle columns.
 4. The tetriminoes spawn horizontally with J, L and T spawning flat-side first. 
 5. Spawn above playfield, row 21 for I, and 21/22 for all other tetriminoes.
 6. Immediately drop one space if no existing Block is in its path.

**Controls Requirements:**

 1. Rotate 90 degrees clockwise
    
 2. Rotate 90 degrees counterclockwise
    
 3. Hard drop
    
 4. Pause the game
    
 5. Move a Tetromino left, right, or down

**Game Rules Requirements:**

 1. The player tops out (loses) when a piece is spawned overlapping at
     least one block (block out), or a piece locks completely above the
     visible portion of the playfield (lock out).
     
 2. A row is cleared and points are scored when the entire row is full
     of blocks.
     
 3. Show the next piece before it drops.
     
 4. T-Spin moves score extra points.

  
**Additional requirements that are fun to implement:**

1.  UI animation
2.  MenuScene
3.  Particle effect for line clears
4.  Dynamic Scoring system
5.  Level System
6.  Touch control for mobile devices
7.  Hold tetromino system
8.  Sound effect and background music
9.  Cartoon Sprites
10.  Super Rotation System for tetromino movements
11.  UI is auto-adjusted based on the mobile screen size
12.  7-bag random generator for spawning

<h3>Game System Design:</h3>

The game has two scenes.

 ***I.  MainScene: The starting scene of the game.***

*MainScene Mechanisms:*

The player can tap the "Challenge" button to start the game.

Clicking "Credit" button will show the developer's name.

    MenuController.cs

> Handles button trigger functions that play UI animation and load different scenes.

***II.  GameScene: The Tetris game play scene.***

**GridSystem:**

    TetrisGrid.cs

>The script contains the basic properties of a single unit gird block.
10x20 grid will be populated when the game starts. Each of the grid blocks in the game will have the same TetrisGrid property.

    GridSystem.cs

>Handles creation of grid, storing tetromino blocks, line cleaning, T-Spin, tetromino blocks position checking. The script also checks if a grid cell is empty or occupied.

**GameSystem:**

    GameSystem.cs

> Controls level up events, sound trigger events, scoring update, and UI animations in the game scene. It also controls game over, pause, quit, time scale mechanism.

    SpawnController.cs
>The script handles the tetrominoes random spawning in two different locations. The 7-bag random generator is also implemented in the script.

    ParticleController.cs
> Handles particle effects after a line clear.

    SoundController.cs
> Handles the sound for tetromino movements, level up, and other events.
  
**TetrominoSystem:**
 
    TetrominoSystem.cs

> Tracks the current tetromino object and tetromino controllers in the game scene. Handles "Next piece" and "Hold piece" operations.

    TetrominoController.cs
> Handles tetromino movement and rotation with keyboard or touch control. UniRx coroutine is implemented in the script for tetromino's auto-falling event.

    TetrominoBlockController.cs
> Handles single tetromino block movement and rotation.
  
    SuperRotationSystemData.cs
> Contains each tetromino types Super Rotation System offset data. The offset data is used for super rotation, such as T-Spin.

**ScoreSystem:**

    ScoreController.cs 
   > Handles the reward of line clears and T-spin. Different levels of line clear have different points rewards. T-Spin has bonus reward in addition to the line clear.

**Touch System:**

    TouchController.cs
> Handles the touch control events, such as swipe and tap.

**UISystem:**

    PanelController.cs
> Controls the scale of the top panel UI. Auto-adjust the panel size based on different screen's aspect ratio.

# References:

**Game Scene Background Music:**

File Name: bensound-jazzyfrenchy

Author: Benjamin Tissot

License: FREE License with Attribution

Source: [https://www.bensound.com/royalty-free-music/track/jazzy-frenchy](https://www.bensound.com/royalty-free-music/track/jazzy-frenchy)

  
**Menu Scene Background Music:**

File Name: Jazz Game Theme Loop.wav

Author: Mrthenoronha

Source: [https://freesound.org/people/Mrthenoronha/sounds/518918/](https://freesound.org/people/Mrthenoronha/sounds/518918/)


  

**Sound Effects:**

  Game Over - Win

[https://freesound.org/people/Mrthenoronha/sounds/518306/](https://freesound.org/people/Mrthenoronha/sounds/518306/)

Game Over – Lose

[https://freesound.org/people/Mrthenoronha/sounds/518307/](https://freesound.org/people/Mrthenoronha/sounds/518307/)

Tetrominoes – Rotation

[https://www.zapsplat.com/music/blunt-metal-object-single-short-scrape-scratch-on-brick-2/](https://www.zapsplat.com/music/blunt-metal-object-single-short-scrape-scratch-on-brick-2/)

Tetrominoes – Fall

[https://www.zapsplat.com/music/breeze-block-drop-on-hard-ground-2/](https://www.zapsplat.com/music/breeze-block-drop-on-hard-ground-2/)

Tetrominoes – Clear Line

[https://www.zapsplat.com/music/science-fiction-beam-up-teleportation-almost-cartoon-like/](https://www.zapsplat.com/music/science-fiction-beam-up-teleportation-almost-cartoon-like/)

 UI Button - Click

[https://www.zapsplat.com/music/rollover-or-ui-tone-short-metal-click/](https://www.zapsplat.com/music/rollover-or-ui-tone-short-metal-click/)

  

  

**Plugins and Assets:**

1.  Icons - simple button set 01:

[https://assetstore.unity.com/packages/2d/gui/icons/simple-button-set-01-153979](https://assetstore.unity.com/packages/2d/gui/icons/simple-button-set-01-153979)

  

2. UniRX:

[https://assetstore.unity.com/packages/tools/integration/unirx-reactive-extensions-for-unity-17276](https://assetstore.unity.com/packages/tools/integration/unirx-reactive-extensions-for-unity-17276)

  
**Tutorials**
How to Properly Rotate Tetris Pieces - Game Development Tutorial
[https://www.youtube.com/watch?v=yIpk5TJ_uaI](https://www.youtube.com/watch?v=yIpk5TJ_uaI)
  

**Background Image**:

Background Image 1

[https://wallpaperaccess.com/kawaii-black#1606655](https://wallpaperaccess.com/kawaii-black#1606655)

  

Background Image 2

[https://wallpaperaccess.com/kawaii-black#305261](https://wallpaperaccess.com/kawaii-black#305261)

  
 Background Image 3

[https://wallpaperaccess.com/kawaii-black#2650630](https://wallpaperaccess.com/kawaii-black#2650630)


