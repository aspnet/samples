# Chess Trainer Blazor Sample

## Overview

This project presents users with chess tactics puzzles. Users are presented with interesting tactical positions and need to identify the best move. As they attempt puzzles, history is kept of which puzzles are solved correctly and which are not. Users can navigate back to previous puzzles to look at them again and can log in to store their own personal puzzle history.

## Technology used

* ASP.NET Core 3.1
* Server-side Blazor
* Entity Framework Core 3.1
* Material Design Components for Web
* Azure AD B2C

## Project purpose and details

This project is primarily designed as a learning exercise for server-side Blazor. It is branched from a [somewhat larger project](https://github.com/mjrousos/chesstrainer) that also exercised Azure Functions and Azure Logic Apps to queue chess games to have puzzles automatically identified. This version of the sample focuses just on the Blazor app for displaying puzzles to the user and tracking their performance.

This project demonstrates how server-side Blazor can be used to allow users to interact with a non-trivial UI (the chess board) by combining Blazor components and JavaScript.

The app has Blazor components for a chess board and chess pieces. Mouse event handlers on the board component map where the user has clicked to a square on the board, enabling the user to move pieces. `OwningComponentBase` is used to allow each Blazor component to have its own scoped instances of dependency injected-services, such as the EF Core DB Context that retrieves tactics puzzles from a SQL database and stores history of which puzzles users solved correctly or incorrectly.

Azure AD B2C is used (along with Blazor CascadingAuthenticationState) to authenticate users in order to store user-specific puzzle history. Material Design Components for Web style the site, using JSInterop to call the necessary MDC JS functions from Blazor components.

## Pre-requisites

* [.NET Core 3.1 SDK](https://dotnet.microsoft.com/download)
* [NodeJs](https://nodejs.org/) installed on the development machine.
* A SQL database for storing puzzles and puzzle histories. A [SQL Server LocalDB](https://docs.microsoft.com/sql/database-engine/configure-windows/sql-server-express-localdb) can be a simple development-time option.
* An [Azure AD B2C](https://docs.microsoft.com/azure/active-directory-b2c/) instance to manage users. Alternatively, if you don't want to set up an Azure AD B2C instance, authentication is easy to disable.

## Building and running the project

1. Navigate to the src/ChessTrainerApp directory.
1. Modify app settings (either in appsettings.json, using environment variables, or via [user secrets](https://docs.microsoft.com/aspnet/core/security/app-secrets)) to specify your own Azure AD B2C instance, client ID, and domain settings.
    1. If you don't wish to use Azure AD B2C, you can comment out the `services.AddAuthentication` line in Startup.cs and no B2C connection information will be needed. Of course, doing this will cause signing in to fail.
1. Optionally, modify the ConnectionStrings:PuzzleDatabase setting to point at the desired SQL database to use for storing puzzles and puzzle history.
1. Build with `dotnet build` or in Visual Studio. Although the app has both Blazor and JavaScript elements, this single command builds both since a custom MSBuild target in ChessTrainerApp.csproj invokes `npm install` and `npm webpack --mode=development` prior to building the .NET project.
1. Launch the app with `dotnet run` or via Visual Studio. The first time the app is launched, the database will be created and seeded with a few puzzles.
1. Navigate to https://localhost:5001 to use the app.
