using System;
using ResScaler;
using UnityEngine;

public class ResolutionUpdater : MonoBehaviour
{
	private void Update()
	{
		Screen.SetResolution(Plugin.Resolution.x, Plugin.Resolution.y, Plugin.FullScreen);
	}
}
