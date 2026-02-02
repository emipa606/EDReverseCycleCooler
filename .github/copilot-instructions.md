# GitHub Copilot Instructions for ED-ReverseCycleCooler (Continued)

## Mod Overview and Purpose

**ED-ReverseCycleCooler (Continued)** is a RimWorld mod that enhances the game's standard cooler by adding reverse cycle air conditioning functionality. This modification allows coolers to operate not only as cooling devices but also as heating units or to automatically adjust a room's temperature according to the player's preferences. The mod also introduces a feature to rotate already placed coolers easily.

## Key Features and Systems

- **Reverse Cycle Mode**: The cooler can now function as a reverse cycle air conditioner, providing the ability to heat rooms while cooling the exhaust.
- **Automatic Temperature Control**: The cooler can be set to automatically switch between heating and cooling modes to maintain a target temperature.
- **Cooler Rotation**: A user-friendly button has been added to allow players to rotate coolers after placement, ensuring optimal orientation.
- **Compatibility Enhancements**: Added support for cooler integration with the "Replace Stuff" mod.

## Coding Patterns and Conventions

- **Code Structure**: Follow clear and consistent class and method naming conventions.
  - Example: `Building_ReverseCycleCooler` class encapsulates the core functionality of the enhanced cooler.
- **Method Visibility**: Use appropriate access modifiers (`public`, `private`, `internal`) to control the visibility and modification scope of classes and methods.
- **Enum Usage**: Enum `CoolerMode` provides a clear and manageable way to handle different modes the cooler can operate in.

## XML Integration

- Define necessary XML files to integrate with RimWorld's data-driven modding framework. Ensure compatibility with vanilla RimWorld XML tags and introduce new XML elements/categories responsibly.

## Harmony Patching

- Use Harmony library techniques to apply patches effectively without overwriting vanilla game code. This ensures mod compatibility and stability.
- Example: Implement patches in a structured manner to modify specific methods or RimWorld behavior as needed, ensuring minimal disruption to the base game logic.

## Suggestions for Copilot

- When suggesting code snippets or auto-completions, prioritize personalized class and method naming typical to the context, such as `ReverseCycleCoolerMode`.
- Ensure suggestions align with C# and RimWorld modding best practices by maintaining proper access levels and leveraging RimWorld-specific libraries.
- When suggesting XML tags for integration, reference existing RimWorld XML conventions and provide logical extensions to those structures.
- Suggest detailed Harmony patch implementations to extend or modify RimWorld functionalities while preserving the game's core mechanics.

---

For further questions and discussion, please use the Discord channel dedicated to this mod. Bug reports should include logs and, if possible, reproduction steps to expedite troubleshooting. Happy modding!
