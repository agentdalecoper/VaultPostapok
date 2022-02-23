using System;
using Client;
using Leopotam.Ecs;
using UnityEngine;

internal class SkillsSystem : IEcsInitSystem, IEcsRunSystem
{
    private SceneConfiguration sceneConfiguration;
    private GameContext gameContext;

    private EcsFilter<SkillsComponent, AddToPlayer> filter;
    private EcsWorld ecsWorld;

    private static SkillsSystem instance;
    public static SkillsSystem Instance => instance;

    public static event Action<SkillsComponent, SkillsComponent> onSkillsChanged;

    public void Init()
    {
        instance = this;
        gameContext.playerSkills = sceneConfiguration.startSkills;
        onSkillsChanged?.Invoke(gameContext.playerSkills, gameContext.playerSkills);
    }

    public void Run()
    {
        foreach (int i in filter)
        {
            ref SkillsComponent skillsComponent = ref filter.Get1(i);
            gameContext.playerSkills.charisma += skillsComponent.charisma;
            gameContext.playerSkills.fighting += skillsComponent.fighting;
            gameContext.playerSkills.mechanical += skillsComponent.mechanical;
            gameContext.playerSkills.survival += skillsComponent.survival;
            gameContext.playerSkills.science += skillsComponent.science;

            TextPopUpSpawnerManager.Instance.StartTextPopUpTween(skillsComponent.ToString(),
                Color.green);
            onSkillsChanged?.Invoke(skillsComponent, gameContext.playerSkills);

            filter.GetEntity(i).Destroy();
        }
    }

    public EcsEntity CreateSkillsUpdate(SkillsComponent skillsComponent)
    {
        EcsEntity entity = ecsWorld.NewEntity();
        entity.Get<AddToPlayer>();
        entity.Replace(skillsComponent);
        return entity;
    }
}