﻿namespace Chat.Framework.Results;

public static class Error
{
    public static IResult Message(string message) => Result.Error(message);
    public static IResult Empty(string title) => Message($"{title} is empty.");
    public static IResult NotExist(string entity) => Message($"{entity} not exist.");
    public static IResult NotFound(string entity) => Message($"{entity} not found.");
    public static IResult NotSet(string entity) => Message($"{entity} not set.");
    public static IResult SaveProblem(string entity) => Message($"{entity} Save Problem.");
}
