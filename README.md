# Musa3da ( help in Arabic )



## Description
The core engine of the Musa3da platform. This is a robust, scalable RESTful API built with .NET 8 that manages authentication, provider profiles, and the bridge between the web and the Telegram Bot.

## Key Features
Unified Auth System: Secure registration and login using Mobile Number and Password with JWT Authentication.

Dual-Role Management: Built-in logic for Providers (profile management) and Admins (activation and oversight).

Telegram Bot Integration: A fully integrated background service using the Telegram.Bot library, allowing seekers to search for providers via chat.

Clean Architecture: Organised into Domain, Application, and Infrastructure layers for high maintainability.

## Tech Stack
Framework: ASP.NET Core 9
Database: Postgresql via Entity Framework Core

Security: BCrypt.Net for password hashing and JWT for session management

Integration: Telegram Bot API



## Installation

```console
dotnet resotre

```

## Build the Tool from source

You can build and package the tool using the following commands. The instructions assume that you are in the root of the repository.

```console
dotnet build
```