//
//          HOW TO USE
//
//   1. Add ResScaler.dll to your BepInEx\Plugins
//   2. Edit config file at BepInEx\config\ResScaler.cfg
//   5. Enjoy your 144p gaming
//

//
//          EXAMPLE OF CONFIG FILE
// [Config]
// Windowed = false
// Width = 1920
// Height = 1080
//

using BepInEx;
using BepInEx.Configuration;
using UnityEngine;

namespace ResScaler;

[BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
public class Plugin : BaseUnityPlugin
{
    public ConfigDefinition WindowedEntry { get; } = new ConfigDefinition("Config", "Windowed");
    public ConfigDefinition WidthEntry { get; } = new ConfigDefinition("Config", "Width");
    public ConfigDefinition HeightEntry { get; } = new ConfigDefinition("Config", "Height");

    private void Awake()
    {
        // Validate config
        ValidateConfig();

        // Parse as resolution as int
        int width = int.Parse(Config[WidthEntry].GetSerializedValue());
        int height = int.Parse(Config[HeightEntry].GetSerializedValue());

        // Parse windowed flag as bool
        bool windowed = bool.Parse(Config[WindowedEntry].GetSerializedValue());

        // Update resolution and fullscreen mode
        Screen.SetResolution(width, height, !windowed);

#if DEBUG
        Logger.LogDebug($"Your resolution: {width}x{height}; Display resolution: {Display.main.systemWidth}x{Display.main.systemHeight}; Windowed = {windowed}");
#endif
    }

    private void ValidateConfig()
    {
        if (!Config.ContainsKey(WindowedEntry))
            Config.Bind(WindowedEntry, false, new ConfigDescription("Set windowed mode for game's window", new AcceptableValueList<bool>(false, true)));

        if (!Config.ContainsKey(WidthEntry))
            Config.Bind(WidthEntry, Display.main.systemWidth, new ConfigDescription("Set game window's width", new AcceptableValueRange<int>(1, Display.main.systemWidth)));

        if (!Config.ContainsKey(HeightEntry))
            Config.Bind(HeightEntry, Display.main.systemHeight, new ConfigDescription("Set game window's height", new AcceptableValueRange<int>(1, Display.main.systemHeight)));
    }
}
