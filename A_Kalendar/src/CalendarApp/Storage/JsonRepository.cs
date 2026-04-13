using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace CalendarApp.Storage;

public sealed class JsonRepository<T> where T : class, IHasGuidId
{
    private readonly string _filePath;
    private readonly JsonSerializerOptions _jsonOptions;

    public JsonRepository(string filePath)
    {
        _filePath = filePath;
        _jsonOptions = new JsonSerializerOptions
        {
            WriteIndented = true
        };
    }

    public List<T> LoadAll()
    {
        if (!File.Exists(_filePath))
            return new List<T>();

        var json = File.ReadAllText(_filePath);
        if (string.IsNullOrWhiteSpace(json))
            return new List<T>();

        var result = JsonSerializer.Deserialize<List<T>>(json, _jsonOptions);
        return result ?? new List<T>();
    }

    public void SaveAll(IEnumerable<T> items)
    {
        var dir = Path.GetDirectoryName(_filePath);
        if (!string.IsNullOrWhiteSpace(dir) && !Directory.Exists(dir))
            Directory.CreateDirectory(dir);

        var json = JsonSerializer.Serialize(items, _jsonOptions);
        File.WriteAllText(_filePath, json);
    }

    public void Upsert(List<T> items, T updated)
    {
        var idx = items.FindIndex(x => x.Id == updated.Id);
        if (idx >= 0)
            items[idx] = updated;
        else
            items.Add(updated);
    }

    public void Delete(List<T> items, Guid id)
    {
        items.RemoveAll(x => x.Id == id);
    }
}

