# Solasta_UnityMods
Here is the collection of all mods that I wrote to simplify my gaming experience with <Solasta: Crown of the Magister> Unity game.

* #### Hide Sorcerer Marks
  Allows to hide facial tattoos, that are forced by default for Sorcerer subclasses.

* #### Always Alt
  Allows to always show extended information about items, spells, etc., as if "Alt" key was physically pressed, while preserving the ability to view shortened information.

* #### Hide Helmets
  Allows to hide helmets for characters in heavy armor.
  
* #### Allow Metal Armor
  Allows all characters to wear metal armor (to boost your druid or bard, for example).
  
* #### Sync Wild Shape AC
  Allows to preserve druid's AC and Constitution, while in wild shape form.
  
* #### Max Faction Relationship
  Allows to max faction relationship. Turned off by default.

* #### Gain All Achievements
  Allows to gain all achievements on game start. Turned off by default.

## How to use in game
* Download mod as SolastaMods.zip from [releases section](https://github.com/PrincessTina/Solasta_UnityMods/releases/).
* Download UMM ([Unity Mod Manager](https://www.nexusmods.com/site/mods/21?tab=files&file_id=2705)) and extract UnityModManagerInstaller.zip to a folder of your preference [don't use the Solasta game folder]
* Setup UMM:
  - Start the program UnityModManager.exe
  - Select _Solasta_ from the list of available games.
  - Select the game folder in case UMM fails to auto detect it.
  - Select _DoorstopProxy_ installation method, if it's available.
  - Click _Install_ button.
* Install mod: drag and drop downloaded SolastaMods.zip over the UMM's _Mods_ tab. If everything went well, you will see _OK_ status in the window. The mod should appear with green status indicator in UMM's window with the list of all installed mods on game start.

## How to modify mod's code
* You will need installed Unity Mod Manager, Visual Studio Community 2022 with components for .NET and Unity games development. Current target .NET framework is .NET 4.8.1.
* Open SolastaMods.sln and add references to game files, if they aren't set. Check out [project creation section](https://wiki.nexusmods.com/index.php/How_to_create_mod_for_unity_game) from Nexus Mods guide.
* Then rebuild solution for _Release_ configuration.
* Start GenerateArchive.bat that will generate you SolastaMods.zip. To prevent errors make sure that Solasta_UnityMods directory is in the same folder with UnityModManagerInstaller directory.
* Install mod in opened UMM window by drag and drop SolastaMods.zip to _Mods_ tab. If you already have this mod installed, deinstall it first, so you could install its fresh version.
* Run the game.

## Showcase
<img width="1846" height="290" alt="I gained the last 40 achievements using 'Gain All Achievements' mod" src="https://github.com/user-attachments/assets/11a0f010-0cf9-4c26-a47b-fda832447893"/>
