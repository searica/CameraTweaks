# CameraTweaks
Client-side mod that lets you change the field of view and set the maximum distance you can zoom out to when on foot or in a boat.

## Instructions
If you are using a mod manager for Thunderstore simply install the mod from there. If you are not using a mod manager then, you need a modded instance of Valheim with BepInEx installed and to put the dll file for this mod into the plugins folder of BepInEx.

## Configuration
Changes made to the configuration settings will be reflected in-game immediately (no restart required). The mod also has a built in file watcher so you can edit settings via an in-game configuration manager (changes applied upon closing the in-game configuration manager) or by changing values in the file via a text editor or mod manager.


<div class="header">
	<h3>Global Section</h3>
    These settings control how verbose the output to the log is.
</div>
<table>
	<tbody>
		<tr>
            <th align="center">Setting</th>
            <th align="center">Server Sync</th>
			<th align="center">Description</th>
		</tr>
            <td align="center"><b>Verbosity</b></td>
            <td align="center">No</td>
			<td align="left">
                Low will log basic information about the mod. Medium will log information that is useful for troubleshooting. High will log a lot of information, do not set it to this without good reason as it will slow down your game.
				<ul>
					<li>Acceptable values: Low, Medium, High</li>
					<li>Default value: Low</li>
				</ul>
			</td>
		</tr>
  </tbody>
</table>

AlwaysFaceCamera = ConfigManager.BindConfig(
                CameraSection,
                "",
                false,
                ""
            );
<div class="header">
	<h3>Camera Section</h3>
    These settings control the main features of the mod that allow you to tweak the camera.
</div>
<table>
	<tbody>
		<tr>
            <th align="center">Setting</th>
            <th align="center">Server Sync</th>
			<th align="center">Description</th>
		</tr>
		<tr>
            <td align="center"><b>Always Face Camera</b></td>
            <td align="center">Yes</td>
			<td align="left">
                Controls whether the player character will always face in the direction of the crosshairs. If left as false, then Vanilla camera behaviour will be used.
				<ul>
					<li>Acceptable values: true, false</li>
					<li>Default value: false</li>
				</ul>
			</td>
		</tr>
		<tr>
            <td align="center"><b>Field of View</b></td>
            <td align="center">Yes</td>
			<td align="left">
                Camera field of view in degrees. Vanilla default is 65.
				<ul>
					<li>Acceptable values: (65, 120)</li>
					<li>Default value: 65</li>
				</ul>
			</td>
		</tr>
		<tr>
            <td align="center"><b>Max Distance</b></td>
            <td align="center">Yes</td>
			<td align="left">
                Maximum distance you can zoom out to while on foot. Vanilla default is 6.
				<ul>
					<li>Acceptable values: (6, 20)</li>
					<li>Default value: 6</li>
				</ul>
			</td>
		</tr>
        <tr>
            <td align="center"><b>Max Distance (Boat)</b></td>
            <td align="center">Yes</td>
			<td align="left">
                Maximum distance you can zoom out to while in a boat. Vanilla default is 6.
				<ul>
					<li>Acceptable values: (6, 20)</li>
					<li>Default value: 12</li>
				</ul>
			</td>
		</tr>
  </tbody>
</table>


## Known Issues
None so far, tell me if you find any.

## Donations/Tips
My mods will always be free to use but if you feel like saying thanks you can tip/donate.

| My Ko-fi: | [![ko-fi](https://ko-fi.com/img/githubbutton_sm.svg)](https://ko-fi.com/searica) |
|-----------|---------------|

## Source Code
Source code is available on Github.

| Github Repository: | <button style="font-size:20px"><img height="18" src="https://github.githubassets.com/favicons/favicon-dark.svg"></img><a href="https://https://github.com/searica/CameraTweaks"> CameraTweaks</button> |
|-----------|---------------|

### Contributions
If you would like to provide suggestions, make feature requests, or reports bugs and compatibility issues you can either open an issue on the Github repository or tag me (@searica) with a message on my discord [Searica's Mods](https://discord.gg/sFmGTBYN6n).

I'm a grad student and have a lot of personal responsibilities on top of that so I can't promise I will respond quickly, but I do intend to maintain and improve the mod in my free time.

## Shameless Self Plug (Other Mods By Me)
If you like this mod you might like some of my other ones.

#### Building Mods
- [More Vanilla Build Prefabs](https://thunderstore.io/c/valheim/p/Searica/More_Vanilla_Build_Prefabs/)
- [Extra Snap Points Made Easy](https://thunderstore.io/c/valheim/p/Searica/Extra_Snap_Points_Made_Easy/)
- [AdvancedTerrainModifiers](https://thunderstore.io/c/valheim/p/Searica/AdvancedTerrainModifiers/)
- [BuildRestrictionTweaksSync](https://thunderstore.io/c/valheim/p/Searica/BuildRestrictionTweaksSync/)
- [ToolTweaks](https://thunderstore.io/c/valheim/p/Searica/ToolTweaks/)

#### Gameplay Mods
- [DodgeShortcut](https://thunderstore.io/c/valheim/p/Searica/DodgeShortcut/)
- [FortifySkillsRedux](https://thunderstore.io/c/valheim/p/Searica/FortifySkillsRedux/)
- [ProjectileTweaks](https://thunderstore.io/c/valheim/p/Searica/ProjectileTweaks/)
- [SkilledCarryWeight](https://thunderstore.io/c/valheim/p/Searica/SkilledCarryWeight/)
- [SafetyStatus](https://thunderstore.io/c/valheim/p/Searica/SafetyStatus/)
