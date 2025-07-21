using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

public static class MapValidator {
    static readonly Regex IdentifierRegex = new(@"^[A-Za-z_][A-Za-z0-9_]*$");

    public static bool ValidateMapInputs(string mapName, string[] keys, out string error) {
        if (string.IsNullOrWhiteSpace(mapName)) {
            error = "Map name can not be null or empty.";
            return false;
        }

        if (!IdentifierRegex.IsMatch(mapName)) {
            error = $"Invalid map name: '{mapName}'. It must be a valid C# class name.";
            return false;
        }

        if (keys == null || keys.Length == 0) {
            error = "Key array cannot be null or empty.";
            return false;
        }

        for (var i = 0; i < keys.Length; i++) {
            var key = keys[i];

            if (string.IsNullOrWhiteSpace(key)) {
                error = $"Key[{i}] cannot be null or empty.";
                return false;
            }

            if (IdentifierRegex.IsMatch(key))
                continue;

            error = $"Key[{i}] = '{key}' is not a valid C# identifier.";
            return false;
        }

        HashSet<string> uniqueKeys = new(keys);
        if (uniqueKeys.Count != keys.Length) {
            error = "Duplicate keys found in the key array.";
            return false;
        }

        Type existingType = AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(a => a.GetTypes())
            .FirstOrDefault(t => t.Name == mapName + "Mapper");

        if (existingType != null) {
            error = $"A type with the name '{mapName}Mapper' already exists. Please choose a different name.";
            return false;
        }

        error = null;
        return true;
    }
}