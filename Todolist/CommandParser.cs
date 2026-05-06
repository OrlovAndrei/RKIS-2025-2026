using System;
using System.Collections.Generic;
using Todolist.Commands;
using Todolist.Exceptions;

static class CommandParser
{
    private static readonly Dictionary<string, Func<string, ICommand>> _commandHandlers;

    static CommandParser()
    {
        _commandHandlers = new Dictionary<string, Func<string, ICommand>>(StringComparer.OrdinalIgnoreCase)
        {
            ["profile"] = ParseProfile,
            ["add"] = ParseAdd,
            ["view"] = ParseView,
            ["read"] = ParseRead,
            ["status"] = ParseStatus,
            ["delete"] = ParseDelete,
            ["update"] = ParseUpdate,
            ["undo"] = _ => new UndoCommand(),
            ["redo"] = _ => new RedoCommand(),
            ["search"] = ParseSearch,
            ["load"] = ParseLoad,
            ["sync"] = ParseSync
        };
    }

    public static ICommand Parse(string inputString)
    {
        if (string.IsNullOrWhiteSpace(inputString))
        {
            throw new InvalidArgumentException("Р’РІРµРґРёС‚Рµ РєРѕРјР°РЅРґСѓ (РїСѓСЃС‚Р°СЏ СЃС‚СЂРѕРєР° РЅРµ РґРѕРїСѓСЃРєР°РµС‚СЃСЏ).");
        }

        string[] parts = inputString.Trim().Split(' ', 2, StringSplitOptions.RemoveEmptyEntries);
        string command = parts.Length > 0 ? parts[0] : string.Empty;
        string args = parts.Length > 1 ? parts[1] : string.Empty;

        if (!_commandHandlers.TryGetValue(command, out var handler))
        {
            throw new InvalidCommandException($"РќРµРёР·РІРµСЃС‚РЅР°СЏ РєРѕРјР°РЅРґР°: {command}. Р’РІРµРґРёС‚Рµ 'help' РґР»СЏ РїРѕРґСЃРєР°Р·РєРё.");
        }

        return handler(args);
    }

    private static ICommand ParseProfile(string args) => new ProfileCommand(args);

    private static ICommand ParseAdd(string args) => new AddCommand(args);

    private static ICommand ParseView(string args) => new ViewCommand(args);

    private static ICommand ParseSearch(string args) => new SearchCommand(args);

    private static ICommand ParseRead(string args)
    {
        if (string.IsNullOrWhiteSpace(args))
            throw new InvalidArgumentException("РћС€РёР±РєР°: РЅРµ СѓРєР°Р·Р°РЅ РёРЅРґРµРєСЃ. РџСЂРёРјРµСЂ: read 1");
        if (!int.TryParse(args.Trim(), out int readIndex))
            throw new InvalidArgumentException("РћС€РёР±РєР°: РёРЅРґРµРєСЃ РґРѕР»Р¶РµРЅ Р±С‹С‚СЊ С‡РёСЃР»РѕРј.");
        if (readIndex < 1 || readIndex > AppInfo.Todos.Count)
            throw new TaskNotFoundException("Р—Р°РґР°С‡Р° СЃ С‚Р°РєРёРј РёРЅРґРµРєСЃРѕРј РЅРµ СЃСѓС‰РµСЃС‚РІСѓРµС‚.");
        return new ReadCommand(readIndex);
    }

    private static ICommand ParseStatus(string args)
    {
        if (string.IsNullOrWhiteSpace(args))
            throw new InvalidArgumentException("РћС€РёР±РєР°: РЅРµ СѓРєР°Р·Р°РЅС‹ РїР°СЂР°РјРµС‚СЂС‹. РџСЂРёРјРµСЂ: status 2 InProgress");
        string[] statusParts = args.Trim().Split(new[] { ' ' }, 2, StringSplitOptions.RemoveEmptyEntries);
        if (statusParts.Length < 2)
            throw new InvalidArgumentException("РћС€РёР±РєР°: РЅРµРІРµСЂРЅС‹Р№ С„РѕСЂРјР°С‚. РџСЂРёРјРµСЂ: status 2 InProgress");
        if (!int.TryParse(statusParts[0], out int statusIndex))
            throw new InvalidArgumentException("РћС€РёР±РєР°: РёРЅРґРµРєСЃ РґРѕР»Р¶РµРЅ Р±С‹С‚СЊ С‡РёСЃР»РѕРј.");
        if (statusIndex < 1 || statusIndex > AppInfo.Todos.Count)
            throw new TaskNotFoundException("Р—Р°РґР°С‡Р° СЃ С‚Р°РєРёРј РёРЅРґРµРєСЃРѕРј РЅРµ СЃСѓС‰РµСЃС‚РІСѓРµС‚.");

        string statusStr = statusParts[1].Trim();
        if (!TodoStatusHelper.TryParse(statusStr, out TodoStatus status))
            throw new InvalidArgumentException("РћС€РёР±РєР°: РЅРµРёР·РІРµСЃС‚РЅС‹Р№ СЃС‚Р°С‚СѓСЃ. Р’РѕР·РјРѕР¶РЅС‹Рµ: NotStarted, InProgress, Completed, Postponed, Failed");
        return new StatusCommand(statusIndex, status);
    }

    private static ICommand ParseDelete(string args)
    {
        if (string.IsNullOrWhiteSpace(args))
            throw new InvalidArgumentException("РћС€РёР±РєР°: РЅРµ СѓРєР°Р·Р°РЅ РёРЅРґРµРєСЃ. РџСЂРёРјРµСЂ: delete 1");
        if (!int.TryParse(args.Trim(), out int deleteIndex))
            throw new InvalidArgumentException("РћС€РёР±РєР°: РёРЅРґРµРєСЃ РґРѕР»Р¶РµРЅ Р±С‹С‚СЊ С‡РёСЃР»РѕРј.");
        if (deleteIndex < 1 || deleteIndex > AppInfo.Todos.Count)
            throw new TaskNotFoundException("Р—Р°РґР°С‡Р° СЃ С‚Р°РєРёРј РёРЅРґРµРєСЃРѕРј РЅРµ СЃСѓС‰РµСЃС‚РІСѓРµС‚.");
        return new DeleteCommand(deleteIndex);
    }

    private static ICommand ParseUpdate(string args)
    {
        if (string.IsNullOrWhiteSpace(args))
            throw new InvalidArgumentException("РћС€РёР±РєР°: РЅРµ СѓРєР°Р·Р°РЅС‹ РїР°СЂР°РјРµС‚СЂС‹. РџСЂРёРјРµСЂ: update 2 \"РќРѕРІС‹Р№ С‚РµРєСЃС‚\"");
        string[] updateParts = args.Trim().Split(' ', 2, StringSplitOptions.RemoveEmptyEntries);
        if (updateParts.Length < 2)
            throw new InvalidArgumentException("РћС€РёР±РєР°: РЅРµРІРµСЂРЅС‹Р№ С„РѕСЂРјР°С‚. РџСЂРёРјРµСЂ: update 2 \"РќРѕРІС‹Р№ С‚РµРєСЃС‚\"");
        if (!int.TryParse(updateParts[0], out int updateIndex))
            throw new InvalidArgumentException("РћС€РёР±РєР°: РёРЅРґРµРєСЃ РґРѕР»Р¶РµРЅ Р±С‹С‚СЊ С‡РёСЃР»РѕРј.");
        if (updateIndex < 1 || updateIndex > AppInfo.Todos.Count)
            throw new TaskNotFoundException("Р—Р°РґР°С‡Р° СЃ С‚Р°РєРёРј РёРЅРґРµРєСЃРѕРј РЅРµ СЃСѓС‰РµСЃС‚РІСѓРµС‚.");
        string newText = updateParts[1].Trim().Trim('"');
        return new UpdateCommand(updateIndex, newText);
    }

    private static ICommand ParseLoad(string args)
    {
        if (string.IsNullOrWhiteSpace(args))
            throw new InvalidArgumentException("РћС€РёР±РєР°: СѓРєР°Р¶РёС‚Рµ РїР°СЂР°РјРµС‚СЂС‹. РџСЂРёРјРµСЂ: load 3 100");

        string[] loadParts = args.Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries);
        if (loadParts.Length != 2)
            throw new InvalidArgumentException("РћС€РёР±РєР°: РЅСѓР¶РЅРѕ РґРІР° С‡РёСЃР»Р°. РџСЂРёРјРµСЂ: load <РєРѕР»РёС‡РµСЃС‚РІРѕ_СЃРєР°С‡РёРІР°РЅРёР№> <СЂР°Р·РјРµСЂ_СЃРєР°С‡РёРІР°РЅРёР№>");

        if (!int.TryParse(loadParts[0], out int downloadsCount) ||
            !int.TryParse(loadParts[1], out int downloadSize))
            throw new InvalidArgumentException("РћС€РёР±РєР°: РїР°СЂР°РјРµС‚СЂС‹ РєРѕРјР°РЅРґС‹ load РґРѕР»Р¶РЅС‹ Р±С‹С‚СЊ С†РµР»С‹РјРё С‡РёСЃР»Р°РјРё.");

        if (downloadsCount <= 0 || downloadSize <= 0)
            throw new InvalidArgumentException("РћС€РёР±РєР°: РїР°СЂР°РјРµС‚СЂС‹ РєРѕРјР°РЅРґС‹ load РґРѕР»Р¶РЅС‹ Р±С‹С‚СЊ Р±РѕР»СЊС€Рµ 0.");

        return new LoadCommand(downloadsCount, downloadSize);
    }
    private static ICommand ParseSync(string args)
    {
        if (string.IsNullOrWhiteSpace(args))
            throw new InvalidArgumentException("Error: specify --pull or --push.");

        string[] syncParts = args.Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries);
        bool pull = false;
        bool push = false;

        foreach (string part in syncParts)
        {
            if (part.Equals("--pull", StringComparison.OrdinalIgnoreCase))
            {
                pull = true;
                continue;
            }

            if (part.Equals("--push", StringComparison.OrdinalIgnoreCase))
            {
                push = true;
                continue;
            }

            throw new InvalidArgumentException($"Error: unknown sync flag: {part}.");
        }

        if (!pull && !push)
            throw new InvalidArgumentException("Error: specify --pull or --push.");

        return new SyncCommand(pull, push);
    }
}

