# DOTS Prototyping
Implementations of Unity DOTS/ECS systems.

&nbsp;
### Systems
- [Destroy entities after X ticks / seconds](./Assets/Systems/Lifecycle/Transient/TransientSystem.cs)
- [Load, instantiate, and initialize prefab entities](./Assets/Systems/Lifecycle/Prefab/)
- [Shader-powered smooth and flipbook animations](./Assets/Systems/Animation/)
- [Instantiate and initialize Low-Level 2D Physics (Box2D v3) worlds and bodies](./Assets/Systems/Physics/Body/)
- [Copy transforms between physics and entity worlds](./Assets/Systems/Physics/Transform/TransformCopySystem.cs)
- [Allow multiple systems to contribute to forces on bodies](./Assets/Systems/Physics/Force/)
- [Compact non-zero forces on all bodies into a single physics API call](./Assets/Systems/Physics/Force/PhysicsBatchForceCreationSystem.cs)

&nbsp;
### Tests
- [Spawn 10,000 flipbook animations of counting even or odd numbers (spritesheet frames authored in unity).](./Assets/Test/MassAnimationTest.cs)
- [Spawn 10,000 smoothly animated circles, changing size and color at random intervals.](./Assets/Test/MassAnimationTest2.cs)
- [Spawn 10,000 dynamic smoothly animated circles, all attracted toward the center of the screen and repulsed by the mouse.](./Assets/Test/MassAnimationTest3.cs)

&nbsp;
### Video
MassAnimationTest3 
- 10,000 dynamic, animated circles, attracted to the center but repulsed by the mouse:

https://github.com/user-attachments/assets/28fe1282-a5c0-4945-9a60-d23f30972081

