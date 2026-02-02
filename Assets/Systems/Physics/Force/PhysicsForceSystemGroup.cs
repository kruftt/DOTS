using Unity.Entities;

[UpdateInGroup(typeof(FixedStepSimulationSystemGroup))]
[UpdateBefore(typeof(PhysicsForcePassSystemGroup))]
public partial class PhysicsForceSystemGroup : ComponentSystemGroup
{

}
