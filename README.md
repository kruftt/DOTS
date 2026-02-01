# DOTS
Generic systems and tests using Unity DOTS

### Systems
This project contains DOTS/ECS systems code for at least the following functionality:

- [Acquire InputSystem input](./Assets/Systems/Input/)
- [Shader-powered flipbook and smooth animations](./Assets/Systems/Animation/)
- [Destroy entity after X ticks / seconds](./Assets/Systems/Lifecycle/Transient/)
- [Load, Instantiate, and Initialize Prefabs](./Assets/Systems/Lifecycle/Prefab/)

- [Instantiate and Initialize Low-Level 2D Physics (Box2D v3) Worlds and Bodies](./Assets/Systems/Physics/Body/)
- [Copy Transforms between physics and entity worlds](./Assets/Systems/Physics/Transform/)
- [Allow multiple systems to contribute to forces on bodies](./Assets/Systems/Physics/Force/)
- [Compact all non-zero forces on bodies into a single physics API call](./Assets/Systems/Physics/Force/PhysicsBatchForceCreationSystem.cs)

### Tests
- [Spawn 10,000 flipbook animations of counting even or odd numbers (spritesheet frames authored in unity).](./Assets/Test/MassAnimationTest.cs)
- [Spawn 10,000 smoothly animated circles, changing size and color at random intervals.](./Assets/Test/MassAnimationTest2.cs)
- [Spawn 10,000 dynamic smoothly animated circles, all attracted toward the center of the screen and repulsed by the mouse.](./Assets/Test/MassAnimationTest3.cs)
  
  
Video of MassAnimationTest3:

<iframe width="560" height="315" src="https://www.youtube.com/embed/a__qk5VGZd4?si=yfWghDieLkdXRE0m" title="YouTube video player" frameborder="0" allow="accelerometer; autoplay; clipboard-write; encrypted-media; gyroscope; picture-in-picture; web-share" referrerpolicy="strict-origin-when-cross-origin" allowfullscreen></iframe>

