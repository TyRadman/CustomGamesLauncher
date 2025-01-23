# Custom Game Launcher

This project is a lightweight desktop application designed to simplify and improve the gaming experience. As a gamer, switching between different launchers that need internet, and are filled with ads take a long time to load. This app consolidates all gamers' games from various launchers into one unified interface, allowing you to browse, select, and run games.

## Key Features of the full product:
- **Unified Game Library**(50%): Automatically detects games installed via popular launchers such as Steam, Epic Games, GOG Galaxy, Battle.net, and more.
- **Launcher Integration**(100%): Seamlessly integrates with multiple platforms to retrieve game details like name, installation directory, and icons.
- **Minimalist Design**(0%): A clean, intuitive interface for quick navigation and game launching.
- **Launcher Independence**(100%): Avoid the need to open multiple launchers—launch your games directly from this app.
- **Customizability**(0%): Option to add or remove games manually for full control over your library.


## Why I Built This:
As a gamer, I wanted an easier way to manage my growing collection of games across different platforms. This app streamlines the process, making it convenient to see everything in one place, without the clutter of full launcher applications.

## Tech Stack:
- **C# .NET / WPF**: For building the desktop app and handling both front and back end.
- **MySQL**: To cache the games and all their relevant data.
- **JSON Parsing**: To handle manifest files from various platforms when available.
- **Shell Integration**: To extract the game icons from the executable files.