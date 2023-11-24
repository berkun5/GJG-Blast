# Berk Unal / GJG-Blast

<div align="left">

  <h1>A demo of a Blast-style matching game.</h1>

  
<!-- Badges -->
<p>

<b>Development Hours:</b> ~22h.<br>
<b>Unity Version:</b> 2021.3.24f
</p>
    <h4>
        <a href="https://youtu.be/3LOEwRJ8nWw">YouTube</a> Demo Video
    <br> <br>
        <a href="https://drive.google.com/file/d/1bQYO9V68AoPxy0CaNT1l-qh_pjhSwonl/view?usp=sharing">GoogleDrive</a> link for Android APK
    </h4>
</div>

<div align="left"> 
   <blockquote style="border-left: 2px solid #4CAF50; padding: 10px; background-color: #FFFFFF; border-radius: 5px; margin-bottom: 20px;">
<!-- Features -->
  🎯<b>Goal</b>:
     
  Tap and blast a minimum of two same types(colors) of adjacent Blocks.
  
![ezgif com-video-to-gif](https://github.com/berkun5/GJG-Blast/assets/80388989/1707d637-5476-480e-b64b-642e174b0c2e)


  📜<b>Gameplay Rules</b>:

  The game will never be in a <b>deadlock</b>, where there are no more moves left to play.
  - The first row will never get obstacles (Box-Blocks).
  - Every time new blocks appear, if it is a deadlock situation, at least one of them will be converted to their upcoming neighbors before appearing in the scene.

  <b>Obstacles</b> (Boxes) can not be tapped and blasted by merge.
  - They are fixed blocks and, can not fall.
  - They block other Blocks above from falling.
  - They can be eliminated by blasting the neighboring Blocks once they run out of HP.

  Standard block <b>Visuals</b> will change based on their matching neighbors and they will have 4 different sprite states. Obstacles will change visuals based on their HP.
</blockquote>

🛠️ Editor:

  All of the ScriptableObjects below can be created through; "<b>Create>Data</b>" or "<b>Create>SerializedData</b>".


📋<b>Grid Manager</b>:
  - <b>BlockSpawner</b> is a prefab that handles object pooling and block entities that is actively resides in the scene.
  - <b>Enabling auto matching</b> will randomly play the game on its own for testing. It will randomly try to match tiles.
  - <b>Reinitialize Grid</b> will reset the grid using Config properties to test different configs and/or restart the game.
  - Create and Assign <b>Config</b> ScriptableObject(SO). Details of the SO can be edited on GridManager inspector window, or it can be edited on its own inspector window.
    - Rows and column are limited between 2 to 10.
    - Width and Height offsets represents the percentage of the full screen. All grid Blocks will fit to the screen with any offset ratio.
    - Config Block Pool is the limitation of newly spawned Blocks during the game.

![image](https://github.com/berkun5/GJG-Blast/assets/80388989/b04c38ab-f361-4913-8f44-c0bd952b89a4)


👜<b>GameBlock Cache</b>:
  - <b>GameBlockData</b> is a list of ScriptableObjects. Each object can be edited on GameBlockData inspector window, or on their own inspector window.
  - This SO resides in Resources folder and can be access from static reference.

		GameBlockType blockType = GameBlockType.Blue;
		GameBlockData blockData = AllGameBlockData.GetBlockData(blockType);
		var sprite = blockData.defaultIcon;
![image](https://github.com/berkun5/GJG-Blast/assets/80388989/d34f54c6-1193-4db9-ae03-fb3c2ffb62b0)

🗄️<b>GameBlock Config</b>:
  - <b>GameBlockConfig</b> is a reference for the spawner that the GridManager uses.
  - At instantiation, a system component will be created for each SO Config.
  - Each system is responsible for their own jobs.
    - Events Component, holds all the events of the entire GameBlock entity.
    - Graphics Component, responsible for the visuals such as swapping sprites.
    - Coordinates Component, responsible for holding coordinates, finding neighbors and matching Blocks.
    - Animations Component, responsible for the Block Tween animations.
    - Physics Component, responsible for the Button interaction such as blasting / merging, keeping the HP.

![image](https://github.com/berkun5/GJG-Blast/assets/80388989/f50747b6-455e-4412-8e29-54ad3d23be73)


🧰Used Assets:
- Dotween
- BCS
</div>
