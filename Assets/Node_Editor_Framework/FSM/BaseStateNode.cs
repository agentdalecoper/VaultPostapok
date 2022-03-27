using System;
using System.Collections.Generic;
using NodeEditorFramework;
using SquadSoldier.StateMachine;
using UnityEditor;
using UnityEngine;
using Action = SquadSoldier.StateMachine.Action;


namespace DefaultNamespace.FSM
{
    //ToDo equals = stateStartNodeId + nodeId
    //ToDo show code
    //ToDo override ToString
    //ToDo previous state yellow
    public abstract class BaseStateNode : Node
    {
        public override Vector2 MinSize => new Vector2(350, 60);
        public override bool AutoLayout => true;
        private const string Id = "stateNode";

        private ValueConnectionKnobAttribute nextConnectionAttribute
            = new ValueConnectionKnobAttribute("Next Node", Direction.Out, "DialogForward", ConnectionCount.Single, NodeSide.Right);

        private ConnectionKnobAttribute previousConnectionAttribute
            = new ConnectionKnobAttribute("Previous Node", Direction.Out, "DialogBack", ConnectionCount.Single, NodeSide.Left);

        [ValueConnectionKnob("From Previous", Direction.In, "DialogForward", NodeSide.Left, 30, MaxConnectionCount = ConnectionCount.Max)]
        public ValueConnectionKnob fromPreviousIN;

        [ConnectionKnob("To Previous", Direction.In, "DialogBack", NodeSide.Right, 30, MaxConnectionCount = ConnectionCount.Max)]
        public ConnectionKnob toPreviousOut;

        public Type GetObjectType => typeof(StateNode);

        public String stateName;
        public List<Action> actions;
        public List<Decision> decisions;

        private StateMachineCanvas stateMachineCanvas;
        public static readonly Dictionary<string, string> statesToOutline = new Dictionary<string, string>();

            protected override void OnCreate()
        {
            base.OnCreate();

            actions = new List<Action>();
            decisions = new List<Decision>();

            stateMachineCanvas = (StateMachineCanvas) canvas;
        }
#if UNITY_EDITOR

        public override void NodeGUI()
        {
            GUILayout.BeginVertical("box");

            if (statesToOutline.ContainsKey(canvas.saveName) && statesToOutline[canvas.saveName] == stateName)
            {
                backgroundColor = Color.green;
            }
            else
            {
                backgroundColor = Color.gray;
            }

            stateName = EditorGUILayout.TextField("State name", stateName);
            DrawActions();
            DrawDecisions();
            GUILayout.EndVertical();
        }

        private void DrawActions()
        {
            GUILayout.ExpandWidth(false);

            GUILayout.BeginHorizontal();
            GUILayout.Label("Actions", GUI.skin.GetStyle("labelBoldCentered"));
            if (GUILayout.Button("+", GUILayout.Width(20)))
            {
                actions.Add(null);
            }

            GUILayout.EndHorizontal();
            GUILayout.Space(5);

            DrawOptions(actions);

            GUILayout.ExpandWidth(false);
        }


        private void DrawDecisions()
        {
            GUILayout.ExpandWidth(false);

            GUILayout.BeginHorizontal();
            GUILayout.Label("Decisions", GUI.skin.GetStyle("labelBoldCentered"));

            if (GUILayout.Button("<-+", GUILayout.Width(30)))
            {
                AddDecisionBackward();
            }

            if (GUILayout.Button("+->", GUILayout.Width(30)))
            {
                AddDecisionForward();
            }

            GUILayout.EndHorizontal();
            GUILayout.Space(5);

            DrawOptions(decisions, true);

            GUILayout.ExpandWidth(false);
        }

        private void AddDecisionForward()
        {
            decisions.Add(null);
            CreateValueConnectionKnob(nextConnectionAttribute);

            NodeEditorCallbacks.IssueOnAddConnectionPort(dynamicConnectionPorts[decisions.Count - 1]);
        }

        private void AddDecisionBackward()
        {
            decisions.Add(null);
            CreateConnectionKnob(previousConnectionAttribute);

            NodeEditorCallbacks.IssueOnAddConnectionPort(dynamicConnectionPorts[decisions.Count - 1]);
        }


        //ToDo animation in editor
        private void DrawOptions<T>(List<T> options, bool dynamicConnections = false) where T : UnityEngine.Object
        {
            EditorGUILayout.BeginVertical();
            for (var i = 0; i < options.Count; i++)
            {
                T option = options[i];
                GUILayout.BeginVertical();
                GUILayout.BeginHorizontal();
                EditorGUILayout.LabelField(i + ".", GUILayout.MaxWidth(15));
                options[i] = (T) EditorGUILayout.ObjectField(option, typeof(T), false);

                if (dynamicConnections)
                {
                    //i=0   i=1  i=2   i=3
                    //0 1  2  3  4 5  6 7
                    ((ConnectionKnob) dynamicConnectionPorts[i]).SetPosition();
                }

                if (GUILayout.Button("‒", GUILayout.Width(20)))
                {
                    options.RemoveAt(i);
                    if (dynamicConnections)
                    {
                        DeleteConnectionPort(i);
                    }

                    i--;
                }

                GUILayout.EndHorizontal();
                GUILayout.EndVertical();
                GUILayout.Space(4);
            }

            GUILayout.EndVertical();
        }

#endif

        public override bool Equals(object other)
        {
            return base.Equals(other);
        }

        protected bool Equals(BaseStateNode other)
        {
            return base.Equals(other) && string.Equals(stateName, other.stateName) && Equals(stateMachineCanvas.saveName, other.stateMachineCanvas.saveName);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hashCode = base.GetHashCode();
                hashCode = (hashCode * 397) ^ (stateName != null ? stateName.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (stateMachineCanvas != null ? stateMachineCanvas.GetHashCode() : 0);
                return hashCode;
            }
        }
    }
}