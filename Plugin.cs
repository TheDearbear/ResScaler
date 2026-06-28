//
//          HOW TO USE
//
//   1. Add ResScaler.dll to your BepInEx\Plugins
//   2. Edit config file at BepInEx\config\ResScaler.cfg
//   3. Enjoy your 144p gaming
//

//
//          EXAMPLE OF CONFIG FILE
// [Config]
// Windowed = false
// Width = 1920
// Height = 1080
// UpdateMode = 0
//

using BepInEx;
using BepInEx.Configuration;

using UnityEngine;
using UnityEngine.SceneManagement;

namespace ResScaler;

[BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
public class Plugin : BaseUnityPlugin
{
    public ConfigDefinition WindowedEntry { get; } = new ConfigDefinition("Config", "Windowed");
    public ConfigDefinition WidthEntry { get; } = new ConfigDefinition("Config", "Width");
    public ConfigDefinition HeightEntry { get; } = new ConfigDefinition("Config", "Height");
    public ConfigDefinition UpdateModeEntry { get; } = new ConfigDefinition("Config", "UpdateMode");

    public static Vector2Int Resolution;
    public static bool FullScreen;
    
    private void Awake()
    {
        // Validate config
        ValidateConfig();

        // Parse resolution as int
        int width = int.Parse(Config[WidthEntry].GetSerializedValue());
        int height = int.Parse(Config[HeightEntry].GetSerializedValue());

        // Parse windowed flag as bool
        bool windowed = bool.Parse(Config[WindowedEntry].GetSerializedValue());

        int updateMode = int.Parse(Config[UpdateModeEntry].GetSerializedValue());

        // Update resolution and fullscreen mode
        Resolution.x = width;
	    Resolution.y = height;
		FullScreen = !windowed;

		if (updateMode == 0 || updateMode == 3 || updateMode == 4 || updateMode == 5)
		{
			Screen.SetResolution(width, height, FullScreen);
		}
		if (updateMode == 1 || updateMode == 3 || updateMode == 5)
		{
			SceneManager.sceneLoaded += delegate(Scene s, LoadSceneMode m)
			{
				Screen.SetResolution(width, height, FullScreen);
			};
		}
		if (updateMode == 2 || updateMode == 4 || updateMode == 5)
		{
			SceneManager.sceneLoaded += delegate(Scene s, LoadSceneMode m)
			{
				CreateUpdater();
			};
		}

#if DEBUG
        Logger.LogDebug($"Your resolution: {width}x{height}; Display resolution: {Display.main.systemWidth}x{Display.main.systemHeight}; Windowed = {windowed}");
#endif
    }

    private void CreateUpdater()
	{
		new GameObject("updater").AddComponent<ResolutionUpdater>();
	}

    private void ValidateConfig()
    {
        if (!Config.ContainsKey(WindowedEntry))
        {
            Config.Bind(WindowedEntry, false,
                new ConfigDescription("Set windowed mode for game window",
                    new AcceptableValueList<bool>(false, true)
                )
            );
        }

        if (!Config.ContainsKey(WidthEntry))
        {
            Config.Bind(WidthEntry, Display.main.systemWidth,
                new ConfigDescription("Set game window width",
                    new AcceptableValueRange<int>(1, Display.main.systemWidth)
                )
            );
        }

        if (!Config.ContainsKey(HeightEntry))
        {
            Config.Bind(HeightEntry, Display.main.systemHeight,
                new ConfigDescription("Set game window height",
                    new AcceptableValueRange<int>(1, Display.main.systemHeight)
                )
            );
        }

        if (!Config.ContainsKey(UpdateModeEntry))
		{
			Config.Bind(UpdateModeEntry, 0, 
                new ConfigDescription("When change the resolution\n0 - only when the plugin is loaded\n1 - only when a scene is loaded\n2 - every frame\n3 - options 0 and 1\n4 - options 0 and 2\n5 - all options",
                    new AcceptableValueRange<int>(0, 5)
                )
            );
		}
    }
}
