using Unity.Entities;

[UpdateInGroup(typeof(FixedStepSimulationSystemGroup))]
[UpdateAfter(typeof(PhysicsForceSystemGroup))]
public partial class PhysicsForcePassSystemGroup : ComponentSystemGroup
{

}
