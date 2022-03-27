using NodeEditorFramework;

namespace DefaultNamespace.FSM
{
    [NodeCanvasType("State machine canvas")]
    public class StateMachineCanvas : NodeCanvas
    {
        public override string canvasName => "State machine";
        
        public override bool allowRecursion { get { return true; } }
    }
}