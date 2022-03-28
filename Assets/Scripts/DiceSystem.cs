using Leopotam.Ecs;
using UnityEngine;

internal class DiceSystem : IEcsInitSystem, IEcsRunSystem
{
    private EcsWorld ecsWorld;

    private EcsFilter<DiceClick> filter;

    public void Init()
    {
        // DiceView.onDiceClicked += () =>
        // {
        //     EcsEntity entity = ecsWorld.NewEntity();
        //     entity.Get<DiceClick>();
        // };
    }

    public void Run()
    {
        if (!filter.IsEmpty())
        {
            var entity = filter.GetEntity(0);
        }
    }
}