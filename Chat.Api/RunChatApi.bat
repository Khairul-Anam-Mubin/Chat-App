@echo off
Title ChatApi
cd C:\workstation\Training\Chat-WebApp\Chat.Api
dotnet clean
dotnet restore
dotnet build
dotnet run