# GitHub Copilot Instructions for ED-ReverseCycleCooler (Continued)

## Mod Overview and Purpose

### Mod Name:
ED-ReverseCycleCooler (Continued)

### Mod Description:
This mod is an update of Jaxxa's original Reverse Cycle Cooler mod, providing enhancements and additional functionality for cooler units in RimWorld. The primary purpose of the mod is to allow the game's existing coolers to function as reverse cycle air conditioners. 

### Version:
2.0.0.0

### Key Features:
- **Reverse Cycle Functionality**: This mod allows coolers to work both as traditional cooling units and as heaters. In heating mode, a cooler will output warm air into the target room while exhausting cold air, functioning like a reverse cycle air conditioner.
- **Automatic Temperature Control**: Coolers can be set to automatically heat or cool a room to maintain a target temperature.
- **Rotatable Coolers**: The mod introduces a button that enables players to rotate already placed coolers to correct any placement mistakes.
- **Compatibility Enhancements**: Added support for compatibility with the 'Replace Stuff' mod, which allows swapping of furniture and building items seamlessly without deconstructing.

## Coding Patterns and Conventions

- **C# Coding Standards**: Follow consistent naming conventions for classes (PascalCase), methods (camelCase), and variables (camelCase).
- **Maintain Readability**: Write descriptive comments especially where complex logic is involved. Utilize region tags to organize code sections logically.
- **Class and Method Design**: Use single responsibility principle to ensure that classes and methods are focused and cohesive.
- **Error Handling**: Implement basic error handling to capture and log unexpected behaviors for better debugging.

## XML Integration

- **XML File Locations**: Ensure all XML files are properly placed within the 'Defs' and 'Patches' directories. For this mod, definitions are located in:
  - `Source\Defs\ThingDefs\`
  - `Patches\`
- **ThingDef Modifications**: Use XML to override and patch ThingDefs to integrate new behaviors or modify existing definitions.

### XML Files Explained:
- `CoolerChange.xml`: Contains patch instructions for altering cooler functions.
- `About.xml`: Provides metadata about the mod and is essential for the mod's identification by the game.
- `Buildings_Temperature.xml`: Defines the properties and capabilities of temperature-related buildings such as coolers.

## Harmony Patching

Harmony is employed to modify existing game functions without altering the original source code directly.

- **Basic Usage**: Use Harmony to patch methods in RimWorld’s codebase, enabling the added functionalities of the mod.
- **Patch Example**: Transition from overwrite to patching mechanisms to enhance compatibility and stability when other mods are active. 

## Suggestions for Using GitHub Copilot

- **Code Completion**: Use Copilot to generate boilerplate for new features, ensuring that it aligns with the existing patterns and conventions.
- **Testing and Error Handling**: Allow Copilot to suggest unit test models which can be invaluable for validating new functionalities.
- **Refactoring Suggestions**: Invite Copilot to propose optimized or more efficient code paths, particularly when making broad alterations or improvements.
- **Documentation Aid**: Utilize Copilot for generating XML summaries and comments where they are lacking or outdated.

This document should guide anyone involved in developing and maintaining the ED-ReverseCycleCooler mod, providing a structured approach to managing the codebase and collaborating effectively using GitHub and Copilot. For any intricate or mod-specific issues, reference the logs and use community resources effectively as outlined in the mod documentation.

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
- When mentioning SonarQube issues, automatically use the SonarQube MCP service to fetch and address issues instead of making inferred fixes without querying SonarQube first.
- When mentioning the rimworld log, automatically use the Rimworld MCP service to fetch the log.

