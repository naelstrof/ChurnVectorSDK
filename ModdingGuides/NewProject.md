# Creating a new modding project

This guide contains information on setting up a new modding project to a state that will allow you to start any of the other modding guides defined here

To start, create a new unity project using the 3D Core template, to the same Editor version that the SDK is currently created in (Currently [2022.3.10f1](https://unity.com/releases/editor/whats-new/2022.3.10))

With your project created, open the Unity package manager (Window -> Package Manager) and add the following packages via their Git URL:
During installing packages, Unity will ask to be rebooted several times. Just press OK each time it asks.

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

Once this is done copy the Example Scene folder found in `Packages/Churn Vector SDK/ModdingGuides` to your project, Open the scene within and hit play, if everything worked you should be in the example scene!
If it doesn't, check your console for errors and try the following:
1) Sometimes the addressables break and need to be reimported, right click on `Packages/Churn Vector SDK/BuiltIn/AddressableAssetsData`. Reimport **(Not reimport all, just reimport)**
2) Likewise sometimes the effects break, you can tell if when you hit play the effects like the pot breaking or funny white liquid come out as squares. Reimport `Packages/Churn Vector Sdk/BuiltIn/VFX`
3) You might also run into an issue where the scripts fail to import, find `Packages/Churn Vector Sdk/Scripts` and reimport the folder.

Lastly need to make sure that the `steam_appid.txt` file that appears in your Unity project contains the following: `2686900`, otherwise steam workshop integration won't work.
Note that this may not appear when you first run the game so just remember this for later.

### Mod Profiles
As an extra step that you will need to know for when you finish all of this, you need to create a mod profile object by right clicking in your project window and selecting `Create -> Data -> Mod Profile`
This profile is how you actually build your mod, anything you can do currently is listed in this object.

Right now you can add new levels and replace characters (If you make a new character for a new level rather than replacing an existing character thats fine too!)

Congratulations, with your project set up you may now continue and learn how to [Import a new character](https://github.com/naelstrof/ChurnVectorSDK/blob/main/ModdingGuides/NewCharacter.md)
