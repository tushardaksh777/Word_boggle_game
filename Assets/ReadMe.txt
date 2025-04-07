1. GameManager.cs (Singleton) -

Acts as the central controller, managing:
Game state transitions (SelectGameModes, Play)
Game modes (Endless, Levels)
Word selection, validation, and scoring
Maintains references to key components like GridManager.cs, UIVisualsController.cs, and DataPatcher.cs
Implements an observer pattern with Action and Delegate events for word updates, score changes, block selections, and UI control.

2. DataPatcher.cs -

The predefined level data from levelData.json is parsed using a separate class called MylevelData.txt, which helps maintain a clean separation between data structure and logic.
Separates logic for "Endless" and "Levels" game modes.
Ensures preloaded solvable levels in Levels mode.

3. GridManager.cs & ColumnManager.cs -

GridManager.cs: Handles creation and logic of the 2D block grid.
ColumnManager.cs: Manages individual columns of tiles and their dynamic updates (like falling and tile replacement).
Uses interfaces (IBlockBehaviour) to maintain flexibility and reusability.

4. BlockHandler.cs -

Implements IBlockBehaviour, encapsulating tile-specific behavior:
Letter assignment
Block type visuals (Normal, Bonus, Locked)
Selection and deselection handling

5. WordValidate.cs -

Loads the dictionary from a text asset.
Uses a HashSet<string> for efficient word validation (O(1) lookup).
Exposed to the GameManager.cs via a delegate (Func<string, bool>).

6. Controls.cs -

Manages player touch input using Unity's GraphicRaycaster and EventSystem.
Detects valid block selections (ensures adjacency and non-duplicates).
Invokes selection/deselection events in the GameManager.cs

7. UI System -

PlayScreenUIController.cs: Handles score, word, and objective display.
UIVisualsController.cs: Coordinates the visibility of screens (play mode, selection menu).
GameSelectionController.cs: Manages mode selection and initiates the game accordingly.

