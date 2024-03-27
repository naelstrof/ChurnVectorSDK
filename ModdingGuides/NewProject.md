# Creating a new modding project (Use the example project outlined in the wiki for modding, this file is TBD deleted)

This guide contains information on setting up a new modding project to a state that will allow you to start any of the other modding guides defined here

To start, create a new unity project using the 3D Core template, to the same Editor version that the SDK is currently created in (Currently [2022.3.10f1](https://unity.com/releases/editor/whats-new/2022.3.10))

### Packages

With your project created, open the Unity package manager. This can be found in the uppermost menu of your screen under Window->Package Manager.
In the top left of the package manager, click on the plus icon and choose "Add package from Git URL" and one at a time, copy and paste a URL from the list below and click "Add"

> During installing packages, Unity may ask to be rebooted several times. Just press OK each time it asks and let it do its thing.

1. `https://github.com/mackysoft/Unity-SerializeReferenceExtensions.git?path=Assets/MackySoft/MackySoft.SerializeReferenceExtensions#1.3.0`
2. `https://github.com/naelstrof/UnityScriptableSettings.git#v2.4.5`
3. `https://github.com/naelstrof/com.naelstrof.extensions.git#v0.0.1`
4. `https://github.com/naelstrof/com.naelstrof.projectiles.git#v1.0.0`
5. `https://github.com/naelstrof/com.naelstrof.easing.git#v0.0.1`
6. `https://github.com/naelstrof/com.naelstrof.orbitcam.git#v0.0.1`
7. `https://github.com/naelstrof/UnityJigglePhysics.git#v7.0.3`
8. `https://github.com/naelstrof/com.naelstrof.inflatable.git#v0.0.1`
9. `https://github.com/naelstrof/UnityPenetrationTech.git#v8.0.0`
10. `https://github.com/rlabrecque/Steamworks.NET.git?path=/com.rlabrecque.steamworks.net#20.2.0`
11. `https://github.com/naelstrof/SkinnedMeshDecals.git#v7.0.0`
12. `https://github.com/juniordiscart/com.unity.addressables.git`
13. `https://github.com/naelstrof/ChurnVectorSDK.git`

### Example Scene

> A quick note for anyone new to Unity - the Packages folder in your project window contains all of the packages you just installed, while the Assets folder contains all of your locally created stuff. In the following tutorials you will be asked to bring files and folder into the project, when these appear this is refering to your Assets folder which you can paste directly into like you would a regular windows copy paste. You can also just drag and drop files and folders directly into the Assets folder from windows explorer!

Once the packages are installed copy the Example Scene folder found in `Packages/Churn Vector SDK/ModdingGuides` to your project (you can do this entirely within unity, just copy and paste the folder to Assets)
Open the scene within and hit play, if everything worked you should be in the example scene!

At this point you likely have a HDRP wizard window showing up, this is meant to not happen but if you do see it click "Fix" and then close it.
You also can likely see a very large black square in your scene, this is the UI and you can hide this by clicking on "Layers" in the top right of your screen and clicking the eye icon next to the UI layer

### Console errors

There will most likely be console errors when you hit play, some of these are expected (no game manager, no objectives) but some may indicate issues with your importing of the SDK. 
> If you find that the game is not putting you in control of the player character when these console errors show up its likely that you have "Error Pause" enabled in the console window, you can turn this off by clicking on it so that it is dark grey instead of lit up.

If anything isn't working such as animations or just the game not loading at all try the following:
1) Sometimes the addressables break and need to be reimported, right click on `Packages/Churn Vector SDK/BuiltIn/AddressableAssetsData`. Reimport **(Not reimport all, just reimport)**
2) Likewise sometimes the effects break, you can tell if when you hit play the effects like the pot breaking or funny white liquid come out as squares. Reimport `Packages/Churn Vector Sdk/BuiltIn/VFX`
3) You might also run into an issue where the scripts fail to import, find `Packages/Churn Vector Sdk/Scripts` and reimport the folder.

After doing the above the game should start properly and allow you to do everything you normally would, if you are still having issues, re-import the entire SDK folder and then try the above steps again.
If that still doesn't work the discord has a mod discussion thread that you can ask for help in.

Lastly need to make sure that the `steam_appid.txt` file that appears in your Unity project contains the following: `2686900`, otherwise steam workshop integration won't work.
Note that this may not appear when you first run the game so just remember this for later.

### Mod Profiles
As an extra step that you will need to know for when you finish all of this, you need to create a mod profile object by right clicking in your project window and selecting `Create -> Data -> Mod Profile`
This profile is how you actually build your mod, anything you can do currently is listed in this object.

Right now you can add new levels and replace characters (If you make a new character for a new level rather than replacing an existing character thats fine too!)

Congratulations, with your project set up you may now continue and learn how to [Import a new character](https://github.com/naelstrof/ChurnVectorSDK/blob/main/ModdingGuides/NewCharacter.md)
