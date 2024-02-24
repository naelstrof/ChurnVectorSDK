# Churn Vector SDK

![Churn Vector SDK Image](churn_vector_sdk.png)

This is a work-in-progress SDK for Churn Vector. It's hopefully going to provide everything needed to create and test content.

It's **not functional** right now as we're still working it out!

# Licensing
Churn Vector is built out of many pieces, each piece might be licensed to you differently if at all.
*Please check carefully* if you have rights to use assets contained within.

If something isn't clearly licensed, it is All Rights Reserved.

- Most code available in this SDK is under GPLv3, check the top of each file to be sure.
- Most models are Attribution-4.0-NC.
- Sounds are organized depending on their associated license into folders. Attribution sounds are from freesound.org with their original id_username_filename format.

# Installation

First install the following Unity Packages in order, top to bottom.
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
12. `com.unity.addressables`
13. `https://github.com/naelstrof/ChurnVectorSDK.git`

The SDK is very large ~(500mb), we tried to restrict it to only required game assets. Though most are uncompressed source files. I don't want to maintain a separate low-quality branch so too bad!

During installing packages, Unity will ask to be rebooted several times. Just press OK each time it asks.

After downloading the packages, you will finally need to make sure that the `steam_appid.txt` file that appears in your Unity
project contains the following: `2686900`, otherwise steam workshop integration won't work.