using System;
using System.Collections.Generic;
using DefaultNamespace.FSM;
using NodeEditorFramework;
using SquadSoldier.StateMachine;
using UnityEditor;
using UnityEngine;
using Action = SquadSoldier.StateMachine.Action;
using Object = UnityEngine.Object;

[Node(false, "FSM/StateNode", new Type[] {typeof(StateMachineCanvas)})]
public class StateNode : BaseStateNode
{
    private const string Id = "stateNode";
    public override string GetID => Id;
}