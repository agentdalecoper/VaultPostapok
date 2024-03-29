﻿using System;
using NodeEditorFramework;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class MyFloatEvent : UnityEvent<float>{}


/// <summary>
/// A node to start a dialog, note that the ID must be entered and be unique
/// </summary>
[Node(false, "Dialog/Dialog Start Node", new Type[] { typeof(DialogNodeCanvas) })]
public class DialogStartNode : BaseDialogNode
{
	public override string Title {get { return "Dialog Start Node"; } }
	public override Vector2 MinSize { get { return new Vector2(350, 60); } }
	public override bool AutoLayout { get { return true; } }

	private const string Id = "dialogStartNode";
	public override string GetID { get { return Id; } }
	public override Type GetObjectType { get { return typeof (DialogStartNode); } }

	[ValueConnectionKnob("To Next", Direction.Out, "DialogForward", NodeSide.Right, 30)]
	public ValueConnectionKnob toNextOUT;
	[ConnectionKnob("From Next",Direction.In, "DialogBack", ConnectionCount.Multi, NodeSide.Right, 50)]
	public ConnectionKnob fromNextIN;

	private Vector2 scroll;
	public string DialogID;
	public Condition[] triggers;

	public bool triggerOnce = true;
	
	public DialogStartType dialogStartType;

#if UNITY_EDITOR
	private SerializedObject serializedObject;
#endif

	protected override void OnCreate ()
	{
		base.OnCreate ();
		CharacterName = "Character name";
		DialogLine = "Insert dialog text here";
		CharacterPotrait = null;
		
#if UNITY_EDITOR
		serializedObject = new SerializedObject(this);
#endif
	}
#if UNITY_EDITOR

	public override void NodeGUI()
	{
		
		if (serializedObject == null)
		{
			serializedObject = new SerializedObject(this);
		}

		EditorGUILayout.BeginVertical("Box");
		GUILayout.BeginHorizontal();
		CharacterPotrait = (Sprite)EditorGUILayout.ObjectField(CharacterPotrait, typeof(Sprite), false, GUILayout.Width(65f), GUILayout.Height(65f));
		CharacterName = EditorGUILayout.TextField("", CharacterName);
		GUILayout.EndHorizontal();

		EditorGUILayout.EndVertical();

		EditorGUIUtility.labelWidth = 90;
		DialogID = EditorGUILayout.TextField("DialogID", DialogID, GUILayout.ExpandWidth(true));
		GUILayout.Space(5);

		EditorStyles.textField.wordWrap = true;

		GUILayout.BeginHorizontal();
		scroll = EditorGUILayout.BeginScrollView(scroll, GUILayout.Height(100));
		DialogLine = EditorGUILayout.TextArea(DialogLine, GUILayout.ExpandHeight(true));
		EditorGUILayout.EndScrollView();
		GUILayout.EndHorizontal();

		GUILayout.BeginHorizontal();
		EditorGUIUtility.labelWidth = 90;
		SoundDialog = EditorGUILayout.ObjectField("Dialog Audio:", SoundDialog, typeof(AudioClip), false) as AudioClip;
		if (GUILayout.Button("►", GUILayout.Width(20)))
		{
			if (SoundDialog)
				AudioUtils.PlayClip(SoundDialog);
		}
		GUILayout.EndHorizontal();
		
		GUILayout.BeginHorizontal();
		SerializedProperty cond = serializedObject.FindProperty("triggers");
		EditorGUILayout.PropertyField(cond, true);

		GUILayout.EndHorizontal();
		EditorGUILayout.PropertyField(serializedObject.FindProperty("triggerOnce"));
		EditorGUILayout.PropertyField(serializedObject.FindProperty("dialogStartType"));
		serializedObject.ApplyModifiedProperties();
	}
#endif

	public override BaseDialogNode Input(int inputValue)
	{
		switch (inputValue)
		{
			case (int)EDialogInputValue.Next:
				if (IsNextAvailable ())
					return getTargetNode (toNextOUT);
			
				break;
		}
		return null;
	}

	public override bool IsBackAvailable()
	{
		return false;
	}

	public override bool IsNextAvailable()
	{
		return IsAvailable (toNextOUT);
	}
}

public enum DialogStartType
{
	Dialog,
	Quest,
	BattleEvent
}