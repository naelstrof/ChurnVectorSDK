# Adding a new character

This guide assumes you have run through the new project guide and have a working example scene, this guide does not include adding the parts necessary to make a predator (player or NPC) character.
You can find how to do that on its own in another guide but this guide will be a required step to reach that point.


# Part 1 - Importing the character

Export your character as a .fbx from your model editor of choice and bring it in to the unity project (drag and drop it into your project window)
Likewise, drag any textures, normal and mask maps into the project (Make sure to set any normal maps as type "Normal Map" in the inspector for that file once in unity!)

> We use Unity standard maps for materials, if you have substance painter select Unity Hd pipeline metalic standard as your export
> Depending on what you have, you may need to alter your textures to work with this but worst case just only import the base colors and fix the rest later)

Once imported, select the model from the project window and in the Inspector (default rightmost panel) open the rig tab.
Click on the Animation Type dropdown and select "Humanoid" then click Apply

Once applied, click on Configure to enter Avatar Configuration and make sure all of the bones that your model has are set up in the correct slots
Scroll down and under Pose select "Enforce T-pose"

> Note that our game does not use the fingers or toes in any animations. If your character has these you may want to pose them in the Avatar Configuration window to be in an idle pose rather than perfectly T-posed

Once done, click apply and then Done to return to the example scene.

Now that the avatar is imported as a humanoid we need to extract the materials to use the ones Churn Vector does
Go back to the imported model and open the Materials tab, click on "Extract Materials" and place them in a new folder named "Materials"

If you brought in the textures prior to this, the materials should automatically have picked them up, if not just drag and drop them from the textures file into the materials as you go.
For now we will only worry about the characters body textures and such, no naughty bits.

Setting materials up is as follows:
1) Click on the material and in the Shader dropdown at the top of the inspector and find the shader that contains the words VoreSlurp (git doesn't like naughty words ツ)
2) Check that the BaseColorMap is defined, if not drag it in (Lock the inspector via the lock icon in the top right to prevent the inspector deselecting the material)
3) Do the same for the other maps you have that the material accepts
4) Don't change any of the other settings, these are mostly controlled by the game and you can experiment with them later

Once all of the materials are set up, Right click in your project window and create a new Prefab.
Open the prefab, drag your imported model in from the project window and then set their x,y,z transforms to zero (0)

Drag your prefab into the scene and check that all of the materials are correct.

# Part 2 - Making the character prefab work with character loaders

In Churn Vector, all characters are spawned into the world via the character loaders. In the example scene you can see these as cardboard cutout characters.
In order to make your new character able to be spawned from one of these loaders they need a bunch of components attached to them which we will now do

> At any time you can refer to the characters in the SKD under BuiltIn -> Characters to see how these are configured

