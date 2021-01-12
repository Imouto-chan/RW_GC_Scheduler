# GC Scheduler
Rimworld Mod allowing the user to disable/enable the Garbage Cleaner of the game. Also, gives the option to schedule when to run the GC based on memory used.

## Requirements
[Harmony](https://steamcommunity.com/workshop/filedetails/?id=2009463077)

## Usage
Once enabled, set the amount of memory the heap needs to have before enabling the GC to run and release memory.
#### Other Options:
* Set the amount of ticks that need to pass before memory gets updated in the mod (60 ticks are about 1~ second)
* Force Pause - Whether or not the game should pause when the GUI is opened in-game
* Enable GC When You...
  * Open Main Tab - Enables the GC so that it can run when you open a main tab in-game
  * Open Notification Window - Enables the GC so that it can run when you open a notification message in-game
* Button to enable the mod and disable the auto GC or disable the mod and enable the auto GC

## In-Game Preview
Enabled | Disabled | Tab
------------ | ------------- | -------------
![Enabled](https://github.com/Imouto-chan/RW_GC_Scheduler/blob/main/Source/preview/preview_enabled.jpg) | ![Disabled](https://github.com/Imouto-chan/RW_GC_Scheduler/blob/main/Source/preview/preview_disable.jpg) | ![Tab](https://github.com/Imouto-chan/RW_GC_Scheduler/blob/main/Source/preview/preview_icon.jpg)

## Misc
I made this mod because I was tired of how little RAM the game would use (4~ GB) when I have so much (32 GB). This caused the game to stutter every few seconds because it would be running the GC and clearing the heap every few seconds as it filled, never allocating more available memory to the heap so it would not need to run as often. Now, using this mod, I can set the memory limit to whatever I desire and still use the GC to release memory when needed, causing less stutters overall and a better game experience.
