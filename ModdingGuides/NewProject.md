# Creating a new modding project

This guide contains information on setting up a new modding project to a state that will allow you to start any of the other modding guides defined here

To start, create a new unity project using the 3D Core template, to the same Editor version that the SDK is currently created in (Currently 2022.3.10f1)

With your project created, open the Unity package manager (Window -> Package Manager) and add the following packages via their Git URL:
During installing packages, Unity will ask to be rebooted several times. Just press OK each time it asks.

1. `https://github.com/mackysoft/Unity-SerializeReferenceExtensions.git?path=Assets/MackySoft/MackySoft.SerializeReferenceExtensions#1.3.0`
2. `https://github.com/naelstrof/UnityScriptableSettings.git#v2.4.5`
3. `https://github.com/naelstrof/com.naelstrof.extensions.git#v0.0.1`
4. `https://github.com/naelstrof/com.naelstrof.projectiles.git#v0.0.1`
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
If it doesn't, check your console for errors, sometimes the addressables break and need to be reimported, you can find these in `Packages/Churn Vector SDK/BuildIn`. Right click on the AddressableAssetsData folder and hit Reimport (Not reimport all, just reimport)

Lastly need to make sure that the `steam_appid.txt` file that appears in your Unity
project contains the following: `2686900`, otherwise steam workshop integration won't work.
