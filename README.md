# Level Manager
A manager system to control levels and states with ease. Supports Scene based and prefab based level system.

# Usage
 * Create a LevelContainer asset `Create/MD/LevelContainer`.  
 * Call Level Complete, Fail, or NextLevel methods from `GameManager`.  
 * Listen to level status events where you need them.  

|Prefab Mode|Scene Mode|
|----|---|
|<img src="/.github/screenshots/prefabmode.png">|<img src="/.github/screenshots/scenemode.png">|

## Documentation

## Level Container

|Events | Description|
|----|---|
| Level Mode | Scene Mode or Prefab Mode |
| LoopMode | continuous or random |
| Levels | Container for game levels |
| MainScene | Main Scene reference (Scene Mode only) |

### Level Mode
 * `Prefab mode` : The level system spawns levels prefab. It works in the same scene.  
 * `Scene mode` : The Level system loads scenes. Need Init scene. Automatically adds scenes to buildsettings.  
 
 ### Loop Mode
  * `EndlessRandom` : After completing entire levels, Level Manager loads random level.
  * `EndlessContinuous` : After completing entire levels, Level Manager loads levels in container order.

## Events

| Events | Description|
|----|---|
| OnLevelStart | Invokes with `LevelData` and Level index when level started |
| OnLevelComplete | Invokes with Score and Level index when level completed|
| OnLevelFail | Invokes with Level index when level failed |
| OnReturnMainMenu | Invokes when level scene loaded (Scene mode only |


## Level Info

| Variable | Description|
|----|---|
| GetLevelCount | Returns levels count |
| CurrentLevelIndex | returns last saved level index |
| GetLastLevelIndex | returns last level index in the level container |
| IsLevelsCompleted | returns true if all levels completed |

## Methods

| Methods | Description|
|----|---|
| StartGame() | Starts game, needs for first play. Initializer for save data |
| LoadNextLevel() | Loads next level |
| LoadLevel(int index) | Loads specific level with given index |
| RestartLevel()| Loads current level |
| StartPreviousLevel()| Loads previous level|
| StartRandomLevel()| Loads random level|

## Manager methods

| Methods | Description|
|----|---|
| StartLevel(Leveldata,levelIndex) | Called after level loading complete |
| OnLevelCompleted(int score) | Called when level completed |
| OnLevelFail(levelIndex) | Calleded when level failed |
| OnReturnMenu | called when exit the game and returning to the main menu. |

## LevelData
Level data contains information about levels, such as display level index, difficulty, win or lose conditions. You can set the data while loading the level or use the preset data. It can vary from game to game. So it's up to you. Declare the data and set it as you want.
