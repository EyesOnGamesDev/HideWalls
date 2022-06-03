# HideWalls
In Some cases and especially in top down games in inner spaces, vision of camera is blocked by walls.
This project aims to provide solution in such cases.

Problem
![image](https://user-images.githubusercontent.com/71728654/171873333-1f168c7b-9ad8-42b1-9b9a-4f99ce6722c4.png)

Script is attached on an empty gameObject under camera. This script raycast from camera to player in each frame.
In case collision with wall is made it trigger an Sphere cast on player pos. Sphere cast returns the total number of walls that are detected.
If walls are under player on Z axis, meaning that propably will block view, it hides them to unblock view.

Script can either Hide Wall completely by disactivating mesh render and leave a second lower wall visible (Activate Second Wall - Variable)
or
Change material of each wall (Change Shader Method - Variable). Assigned material has opacity alpha channel set to zero.

![image](https://user-images.githubusercontent.com/71728654/171875824-ca217e1c-3593-41dd-b424-97acfb11caba.png)

On project you will find two scenes. 
First scene called "WallsHide". 
>>In this scene each wall is one gameObject with one mesh renderer.
![image](https://user-images.githubusercontent.com/71728654/171874936-9c9b4db5-364a-4cec-8507-8b14f47c6505.png)

Second scene called "SubWallsHide"
>>In this scene each wall is part of smaller walls and are saved under the same parent
![image](https://user-images.githubusercontent.com/71728654/171874992-787d547a-f029-4f8d-a800-63c9bc7d34e6.png)

If you like this project consider following me on twitter @EyesOngamesDev

