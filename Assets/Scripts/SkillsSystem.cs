using System;
using Client;
using Leopotam.Ecs;
using UnityEngine;

internal class SkillsSystem : IEcsInitSystem, IEcsRunSystem
{
    public SkillsComponent playerSkills;
    private SceneConfiguration sceneConfiguration;

    private EcsFilter<SkillsComponent, AddToPlayer> filter;
    private EcsWorld ecsWorld;

    private static SkillsSystem instance;
    public static SkillsSystem Instance => instance;

    public static event Action<SkillsComponent, SkillsComponent> onSkillsChanged;

    public void Init()
    {
        instance = this;
        playerSkills = sceneConfiguration.startSkills;
        onSkillsChanged?.Invoke(playerSkills, playerSkills);
    }

    public void Run()
    {
        foreach (int i in filter)
        {
            ref SkillsComponent skillsComponent = ref filter.Get1(i);
            playerSkills.charisma += skillsComponent.charisma;
            playerSkills.fighting += skillsComponent.fighting;
            playerSkills.mechanical += skillsComponent.mechanical;
            playerSkills.survival += skillsComponent.survival;
            playerSkills.science += skillsComponent.science;

            TextPopUpSpawnerManager.Instance.StartTextPopUpTween(skillsComponent.ToString(),
                Color.green);
            onSkillsChanged?.Invoke(skillsComponent, playerSkills);

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