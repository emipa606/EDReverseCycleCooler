# GitHub Copilot Instructions for ED-ReverseCycleCooler (Continued)

## Mod Overview and Purpose
The ED-ReverseCycleCooler mod transforms the standard cooler in RimWorld into a versatile Reverse Cycle Air Conditioner. Originally created by Jaxxa, this updated version by Mlie enhances the cooler's functionality by allowing it to both heat and cool rooms. The mod seamlessly integrates into RimWorld, offering players the flexibility to manage room temperatures with greater precision.

## Key Features and Systems
- **Reverse Cycle Functionality:** The cooler can be set to heating mode to warm rooms or cooling mode to cool them, similar to a real-world reverse cycle air conditioner.
- **Automatic Mode:** Automatically heats or cools the room to reach a desired temperature.
- **Rotatable Coolers:** A new button feature that allows players to rotate an already placed cooler, correcting placement errors without requiring deconstruction.
- **Support for Replace Stuff Mod:** Ensures compatibility and functionality with coolers and 2-wide coolers when using the Replace Stuff mod.

## Coding Patterns and Conventions
The mod's source code follows several key programming patterns:
- **Object-Oriented Design:** Using classes to encapsulate cooler functionality (`Building_ReverseCycleCooler`) and enum types for mode selection (`EnumCoolerMode`).
- **Encapsulation:** Utilizes methods such as `changeMode` and `changeRotation` to change the cooler's state.
- **Modularity:** Efficient function separation with specific methods handling data exposure (`ExposeData`) and game-specific functionalities (`GetGizmos`, `GetInspectString`, `TickRare`).

## XML Integration
The mod includes several XML files that support its integration:
- **Defs:** `Buildings_Temperature.xml` defines the cooler's properties and behaviors within the game world.
- **Patches:** `CoolerChange.xml` contains Harmony patches that modify the existing coolers' functionality without overwriting the base game files.
- **About Information:** `About.xml` provides metadata about the mod, such as the author and package ID.

## Harmony Patching
The mod uses Harmony for code patching to safely and effectively extend RimWorld's functionality:
- **Non-invasive Modifications:** The transition from overwrite to patching ensures that the mod's changes are layered on top of the base game functionalities, minimizing conflicts.
- **Compatibility Updates:** Additional patches ensure the cooler remains compatible with other mods, such as Replace Stuff, by adjusting specific functionalities (e.g., `ReplaceStuffFix`).

## Suggestions for Copilot
When using GitHub Copilot, consider the following suggestions to maintain and enhance this mod:
- **Automate Routine Tasks:** Use Copilot to generate repetitive C# and XML code snippets, such as method signatures, XML def structures, and property setters/getters.
- **AI-Assisted Refactoring:** Seek Copilot's assistance in refactoring code for improved readability and maintainability, especially complex switch cases or conditional logic in `Building_ReverseCycleCooler`.
- **Documentation Enhancement:** Leverage Copilot to enhance inline code comments and XML documentation to better explain mod logic and configurations.
- **Bug Identification and Fixing:** Use Copilot for introducing debug loggers or exception handling constructs, aiding in troubleshooting and resolving mod errors effectively.
- **Feature Expansion Ideas:** Consider suggestions by Copilot during code editing to explore new functionalities or enhance existing features.

By following these instructions and utilizing GitHub Copilot effectively, you can ensure the continued development and improvement of the ED-ReverseCycleCooler mod.

## Project Solution Guidelines
- Relevant mod XML files are included as Solution Items under the solution folder named XML, these can be read and modified from within the solution.
- Use these in-solution XML files as the primary files for reference and modification.
- The `.github/copilot-instructions.md` file is included in the solution under the `.github` solution folder, so it should be read/modified from within the solution instead of using paths outside the solution. Update this file once only, as it and the parent-path solution reference point to the same file in this workspace.
- When making functional changes in this mod, ensure the documented features stay in sync with implementation; use the in-solution `.github` copy as the primary file.
- In the solution is also a project called Assembly-CSharp, containing a read-only version of the decompiled game source, for reference and debugging purposes.
- For any new documentation, update this copilot-instructions.md file rather than creating separate documentation files.


## Hard rules (must follow)
- Do NOT run commands that modify the repo (no git commit, git apply, dotnet format) unless explicitly asked.
- Prefer minimal reads: read only the smallest code region needed (around the suspicious lines).

