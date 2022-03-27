using System;
using System.Collections.Generic;
using System.Linq;
using DefaultNamespace;
using NodeEditorFramework;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;


/// <summary>
/// One entry and multiple exits, one for each possible answer
/// </summary>
[Node(false, "Dialog/Card node", new Type[]{typeof(DialogNodeCanvas)})]
public class CardNode : BaseDialogNode
{
	public override string Title { get { return "Card node"; } }
	public override Vector2 MinSize { get { return new Vector2(400, 60); } }
	public override bool AutoLayout { get { return true; } }

	private const string Id = "multiOptionDialogNode";
	public override string GetID { get { return Id; } }
	public override Type GetObjectType { get { return typeof(CardNode); } }

	//previous node connections
	[ValueConnectionKnob("From Previous", Direction.In, "DialogForward", NodeSide.Left, 30)]
	public ValueConnectionKnob frinPreviousIN;
	[ConnectionKnob("To Previous", Direction.Out, "DialogBack", NodeSide.Left, 50)]
	public ConnectionKnob toPreviousOUT;

	///Next node 
	[ConnectionKnob("From Next",Direction.In, "DialogBack", ConnectionCount.Multi, NodeSide.Right, 50)]
	public ConnectionKnob fromNextIN;

	private const int StartValue = 276;
	private const int SizeValue = 24;

	[SerializeField]
	public List<DialogOption> _options;
	
	public DialogAction[] actionsOnEnter;

	private Vector2 scroll;

	private ValueConnectionKnobAttribute dynaCreationAttribute 
	    = new ValueConnectionKnobAttribute(
		   "Next Node", Direction.Out, "DialogForward", NodeSide.Right);

	public CardTest cardObject;

#if UNITY_EDITOR
	private SerializedObject serializedObject;
#endif


	protected override void OnCreate ()
	{
		CharacterName = "CharacterId";
		DialogLine = "";
		CharacterPotrait = null;

		_options = new List<DialogOption>();
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
		GUILayout.EndVertical();

		GUILayout.Space(5);

		EditorStyles.textField.wordWrap = true;

		GUILayout.BeginHorizontal();

		scroll = EditorGUILayout.BeginScrollView(scroll, GUILayout.Height(100));
		DialogLine = EditorGUILayout.TextArea(DialogLine, GUILayout.ExpandHeight(true));
		EditorGUILayout.EndScrollView();
		GUILayout.EndHorizontal();

		GUILayout.Space(5);

		GUILayout.BeginHorizontal();
		EditorGUIUtility.labelWidth = 90;
		SoundDialog = EditorGUILayout.ObjectField("Dialog Audio:", SoundDialog, typeof(AudioClip), false) as AudioClip;

		if (GUILayout.Button("►", GUILayout.Width(20)))
		{
			if (SoundDialog)
				AudioUtils.PlayClip(SoundDialog);
		}
		GUILayout.EndHorizontal();

		GUILayout.Space(5);

		#region Options
		GUILayout.BeginVertical("box");
		GUILayout.ExpandWidth(false);

		GUILayout.BeginHorizontal();
		GUILayout.Label("Options", GUI.skin.GetStyle("labelBoldCentered"));
		if (GUILayout.Button("+", GUILayout.Width(20)))
		{
			AddNewOption(_options);
			IssueEditorCallBacks();
		}

		GUILayout.EndHorizontal();
		GUILayout.Space(5);

		DrawOptions();
		

		GUILayout.ExpandWidth(false);
		
		GUILayout.BeginHorizontal();
		// GUILayout.Label("Items to add", GUI.skin.GetStyle("labelBoldCentered"));
		// ItemsToAdd = EditorGUILayout.TextArea(ItemsToAdd);
		
		GUILayout.BeginHorizontal();
		SerializedProperty cond = serializedObject.FindProperty("actionsOnEnter");
		EditorGUILayout.PropertyField(cond, true);
		serializedObject.ApplyModifiedProperties();
		GUILayout.EndHorizontal();
		
	

		
		GUILayout.EndHorizontal();
		GUILayout.BeginHorizontal();
		cond = serializedObject.FindProperty("cardObject");
		EditorGUILayout.PropertyField(cond, true);
		serializedObject.ApplyModifiedProperties();
		GUILayout.EndHorizontal();
		GUILayout.Space(5);

		// GUILayout.Label("Change points options", GUI.skin.GetStyle("labelBoldCentered"));
		// if (GUILayout.Button("+", GUILayout.Width(20)))
		// {
		// 	AddNewOption(changePointsOptions);
		// 	IssueEditorCallBacks();
		// }
		//
		// GUILayout.EndHorizontal();
		// GUILayout.Space(5);
		// EditorGUILayout.BeginVertical();
		//
		// DrawOptions(changePointsOptions);
		// GUILayout.EndVertical();
		//
		// GUILayout.ExpandWidth(false);

		GUILayout.EndVertical();
	#endregion

	}

	private void RemoveLastOption()
	{
		if(_options.Count > 1)
		{
			DialogOption option = _options.Last();
			_options.Remove(option);
			DeleteConnectionPort(dynamicConnectionPorts.Count-1);
		}
	}

	private void DrawOptions()
	{
		EditorGUILayout.BeginVertical();
		DrawOptions(_options);
		// EditorGUILayout.PropertyField(cond, true);
		serializedObject.Update();
		// serializedObject.ApplyModifiedProperties();
		GUILayout.EndVertical();
	}

	private void DrawOptions(List<DialogOption> dataHolderForOptions)
	{
		for (var i = 0; i < dataHolderForOptions.Count; i++)
		{
			DialogOption option = dataHolderForOptions[i];
			GUILayout.BeginVertical();
			GUILayout.BeginHorizontal();
			EditorGUILayout.LabelField(i + ".", GUILayout.MaxWidth(15));
			option.text = EditorGUILayout.TextArea(option.text, GUILayout.MinWidth(80));
			((ValueConnectionKnob) dynamicConnectionPorts[i]).SetPosition();
			GUILayout.EndHorizontal();
			GUILayout.EndVertical();
			SerializedProperty cond = serializedObject.FindProperty($"_options.Array.data[{i}].conditions");
			// serializedObject.FindProperty($"_options.Array.data[{i}.OptionDisplay]");
			// cond.isExpanded = true;
			EditorGUILayout.PropertyField(cond, true);
			// EditorGUILayout.TextArea(option.OptionDisplay, GUILayout.MinWidth(80));
			if (GUILayout.Button("‒", GUILayout.Width(20)))
			{
				dataHolderForOptions.RemoveAt(i);
				DeleteConnectionPort(i);
				i--;
			}

			GUILayout.Space(4);
		}
	}

	private void AddNewOption(List<DialogOption> dataHolderForOptions)
	{
		DialogOption option = new DialogOption {text = "Write Here"};
		CreateValueConnectionKnob(dynaCreationAttribute);
		_options.Add(option);
		serializedObject.Update();

	}
#endif

	//For Resolving the Type Mismatch Issue
	private void IssueEditorCallBacks()
	{
		NodeEditorCallbacks.IssueOnAddConnectionPort (dynamicConnectionPorts[_options.Count - 1]);
	}

	public override BaseDialogNode Input(int inputValue)
	{
		switch (inputValue)
		{
		case (int)EDialogInputValue.Next:
			break;

		case (int)EDialogInputValue.Back:
			if (IsAvailable(toPreviousOUT))
				return getTargetNode(toPreviousOUT);
			break;
		
		
		default:
				//if(Outputs[_options[inputValue].dynamicConnectionPortsIndex].GetNodeAcrossConnection() != default(Node))
				//	return Outputs[_options[inputValue].dynamicConnectionPortsIndex].GetNodeAcrossConnection() as BaseDialogNode;
			//I think we -2 for next and back, but not really sure yet
			//TODO is this right?
			Debug.Log("checking dynamic connection port for dialog node " + inputValue);
			if (IsAvailable (dynamicConnectionPorts [inputValue]))
				return getTargetNode (dynamicConnectionPorts [inputValue]);
			break;
		}
		return null;
	}

	public override bool IsBackAvailable()
	{
		return IsAvailable (toPreviousOUT);
	}

	public override bool IsNextAvailable()
	{
		return false;
	}

	

	public List<string> GetAllOptions()
	{
		return _options.Select(option => option.text).ToList();
	}
}

[Serializable]
public class Condition : SerializableCallback<float, bool> { }

[Serializable]
public class DialogAction : SerializableCallback<string, int, bool>
{
}

[Serializable]
public class DialogOption
{
	public string text;
	public Condition[] conditions;
	//public int dynamicConnectionPortsIndex;				
}