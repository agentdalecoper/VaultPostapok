using System;
using DefaultNamespace;
using NodeEditorFramework;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;


/*
 * Что вообще нужно еще кроме unity callbacks?
 *      - проверить что есть кол-во в инвентаре (несколько разных проверок)
 *      - проверить что есть кол-во в квестах (несколько разных проверок)
 *      - проверить что есть ли lablel - типа first arrive
 *      - кастомный текст в зависимости от стейта игры
 *      - добавить/убрать ресурсы к плееру (и показать допустим)
 *      - добавить/убрать квест к плееру
 *      - добавить лейбл?
 *      - добавить картинки к оптионсам
 *      - добавить количество в текст
 *              inventoryId2 inventoryId2Count
 *        тоесть inventoryId2 идет в name
 *
 * Check count {id, 1}
 * Check true  {id, true}
 * Check enum  {id, Finished}
 *
 * Add/remove count {id, -3}
 * Set bool {id, false}
 * Set enum {id, STARTED}
 *
 * Interpolate text
 * for each {} = key in string:
 *      if(key.startsWith quest):
 *          Quest quest = getQuest(key)
 *          if(key.endsWith count) getCount(quest)
 *
 * тоесть короче кодируем {idMethod, value}
 * 
 * Option
 *      Text: "${textId3} Blablabla ${inventoryId2Name} in value of ${inventoryId2Count} blablabla"
 *      ToChecks : {[labelFirstArriveId1, true], [questId1, InProgress],
 *                  [inventoryGrain1, 3]}
 *      Actions: {[questId1, Finished], [inventoryId2, -3]}
 *      Sprites: {spriteId1, spriteId2}
 *
 *
 * Можно знаешь как сделать через запятую одной стрингой
 * id, method, value
 *
 * Так ну а что мне нужно для текста
 * - описание квеста, количкство твоего предмета, название предмета, количество предметов сейчас в инвентаре
 *
 *
 *
 * Так ну пока я вижу тригеры ивентов
 *      playerTime, 180min, chance to occur in a minute?
 *      questLabel
 *      territory/regionLabel
 *      villageCurrent
 *      worldLabel (someone concquered something)
 *
 *
 *
 *
 * Так а что вообще планируется сделать по инвентарю?
 *       ну вообще в идеааале это система как в sunless sea
 *       - у тебя добавляются убираются айтемы (количество рационов), куриос (дарк сикрет), деньги,
 *         статусы для города
 *       - крафт явно не собираюсь добавлять
 *       - 
 *
 *
 *
 *
 *
 * 
 * 
 */

[Node(false, "Dialog/DialogActionNode", new Type[] {typeof(DialogNodeCanvas)})]
public class DialogActionNode : BaseDialogNode
{
    public UnityEvent onChangeEvent;
    public Condition[] condition;

    //Previous Node Connections
    [ValueConnectionKnob("From Previous", Direction.In, "DialogForward", NodeSide.Left, 30)]
    public ValueConnectionKnob fromPreviousIN;
    [ConnectionKnob("To Previous", Direction.Out, "DialogBack", NodeSide.Left, 50)]
    public ConnectionKnob toPreviousOut;

    //Next Node to go to
    [ValueConnectionKnob("To Next", Direction.Out, "DialogForward", NodeSide.Right, 30)]
    public ValueConnectionKnob toNextOUT;

    public override Type GetObjectType { get; }

#if UNITY_EDITOR

    private SerializedObject serializedObject;
    
    protected override void OnCreate ()
    {
        serializedObject = new SerializedObject(this);
    }
#if UNITY_EDITOR

    public override void NodeGUI()
    {
        if (serializedObject == null)
        {
            serializedObject = new SerializedObject(this);
        }
#endif

        GUILayout.BeginHorizontal();
        SerializedProperty cond = serializedObject.FindProperty("Condition");
        EditorGUILayout.PropertyField(cond, true);
        serializedObject.ApplyModifiedProperties();

        GUILayout.EndHorizontal();
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
            case (int)EDialogInputValue.Back:
                if (IsBackAvailable ())
                    return getTargetNode (fromPreviousIN);
                break;
        }
        return null;
    }

    public override bool IsBackAvailable()
    {
        return IsAvailable (toPreviousOut);
    }

    public override bool IsNextAvailable()
    {
        return IsAvailable (toNextOUT);
    }
    

    private const string Id = "dialogStartNode123";
    public override string GetID { get { return Id; } }
}