using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using ACE.Adapter.GDLE.Models;
using ACE.Database.Models.World;

namespace ACE.Adapter.Lifestoned;

public static class LifestonedLoader
{
    public static bool TryLoadWeenie(string file, out LSDWeenie result)
    {
        try
        {
            var fileText = File.ReadAllText(file);

            result = JsonSerializer.Deserialize<LSDWeenie>(fileText);

            return true;
        }
        catch
        {
            result = null;
            return false;
        }
    }

    public static bool TryLoadWeenies(string folder, out List<LSDWeenie> results)
    {
        try
        {
            results = new List<LSDWeenie>();

            var files = Directory
                .GetFiles(folder, "*.json", SearchOption.AllDirectories)
                .OrderByDescending(f => new FileInfo(f).CreationTime)
                .ToList();

            foreach (var file in files)
            {
                if (TryLoadWeenie(file, out var result))
                {
                    results.Add(result);
                }
            }

            return true;
        }
        catch
        {
            results = null;
            return false;
        }
    }

    public static bool TryLoadWeeniesInParallel(string folder, out List<LSDWeenie> results)
    {
        try
        {
            var files = Directory.GetFiles(folder, "*.json", SearchOption.AllDirectories);

            var weenies = new ConcurrentBag<LSDWeenie>();

            Parallel.ForEach(
                files,
                file =>
                {
                    if (TryLoadWeenie(file, out var result))
                    {
                        weenies.Add(result);
                    }
                }
            );

            results = new List<LSDWeenie>(weenies);

            return true;
        }
        catch
        {
            results = null;
            return false;
        }
    }

    public static bool TryLoadWeenieConverted(string file, out Weenie result, bool correctForEnumShift = false)
    {
        try
        {
            var fileText = File.ReadAllText(file);

            var lifestonedModel = JsonSerializer.Deserialize<LSDWeenie>(fileText);

            return LifestonedConverter.TryConvert(lifestonedModel, out result, correctForEnumShift);
        }
        catch
        {
            result = null;
            return false;
        }
    }

    public static bool TryLoadWeeniesConverted(
        string folder,
        out List<Weenie> results,
        bool correctForEnumShift = false
    )
    {
        try
        {
            results = new List<Weenie>();

            var files = Directory
                .GetFiles(folder, "*.json", SearchOption.AllDirectories)
                .OrderByDescending(f => new FileInfo(f).CreationTime)
                .ToList();

            foreach (var file in files)
            {
                if (TryLoadWeenieConverted(file, out var result, correctForEnumShift))
                {
                    results.Add(result);
                }
            }

            return true;
        }
        catch
        {
            results = null;
            return false;
        }
    }

    public static bool TryLoadWeeniesConvertedInParallel(
        string folder,
        out List<Weenie> results,
        bool correctForEnumShift = false
    )
    {
        try
        {
            var files = Directory.GetFiles(folder, "*.json", SearchOption.AllDirectories);

            var weenies = new ConcurrentBag<Weenie>();

            Parallel.ForEach(
                files,
                file =>
                {
                    if (TryLoadWeenieConverted(file, out var result, correctForEnumShift))
                    {
                        weenies.Add(result);
                    }
                }
            );

            results = new List<Weenie>(weenies);

            return true;
        }
        catch
        {
            results = null;
            return false;
        }
    }

    public static bool AppendMetadata(string json_filename, LSDWeenie weenie)
    {
        // read existing json weenie
        var success = TryLoadWeenie(json_filename, out var json_weenie);

        if (!success)
        {
            return false;
        }

        var metadata = new Metadata(json_weenie);

        if (!metadata.HasInfo)
        {
            return false;
        }

        weenie.LastModified = DateTime.UtcNow;
        weenie.ModifiedBy = "ACE.Adapter";

        if (metadata.Changelog != null && metadata.Changelog.Count > 0)
        {
            metadata.Changelog.AddRange(weenie.Changelog);
            weenie.Changelog = metadata.Changelog;
        }

        weenie.UserChangeSummary = "Weenie exported from ACEmulator world database using ACE.Adapter";
        weenie.IsDone = metadata.IsDone;

        return true;
    }
}
