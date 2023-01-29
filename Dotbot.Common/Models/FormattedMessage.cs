﻿using System.Drawing;
using FluentResults;

namespace Dotbot.Common.Models;

public class FormattedMessage
{
    public string Title { get; set; }

    public DateTimeOffset? Timestamp { get; set; }
    public string ImageUrl { get; init; }
    public string Description { get; set; }
    public Color? Color { get; set; }
    public List<Field> Fields { get; set; } = new();

    //TODO: Footer, Author, Header

    public class Field
    {
        public string Name { get; set; }
        public string? Value { get; set; }

        public bool Inline { get; set; } = false;
    }

    public static FormattedMessage Info(string message)
    {
        return new FormattedMessage
        {
            Color = System.Drawing.Color.Blue,
            Title = "Info",
            Description = message
        };
    }
    
    public static FormattedMessage Success(string message)
    {
        return new FormattedMessage
        {
            Color = System.Drawing.Color.LimeGreen,
            Title = "Success",
            Description = message
        };
    }
    
    public static FormattedMessage ErrorMessage(string message)
    {
        return new FormattedMessage
        {
            Color = System.Drawing.Color.DarkRed,
            Title = "Error",
            Description = message
        };
    }
    
    public static FormattedMessage ErrorMessage(IEnumerable<IError> errors)
    {
        var errorNum = 1;
        return new FormattedMessage
        {
            Color = System.Drawing.Color.DarkRed,
            Title = "Error",
            Fields = errors.Select(x => new Field{Name = $"Error {errorNum++}", Value = x.Message}).ToList()
        };
    }

}