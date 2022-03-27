using System;
using NodeEditorFramework;
using UnityEditor;
using UnityEngine;

namespace DefaultNamespace.FSM
{
    //Maybe just do it in realtime
    [Node(false, "FSM/StartStateNode", new Type[] {typeof(StateMachineCanvas)})]
    public class StateStartNode : BaseStateNode
    {
        private const string Id = "stateStartNode";
        public override string GetID => Id;

        public string stateGraphId;
#if UNITY_EDITOR

        public override void NodeGUI()
        {
            GUILayout.BeginVertical("box");

            stateGraphId = EditorGUILayout.TextField("State graph ID (*)", stateGraphId);
            GUILayout.EndVertical();
            
            base.NodeGUI();
        }
#endif

    }

}