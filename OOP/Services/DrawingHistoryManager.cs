using OOP.Models;

namespace OOP.Services
{
    public class DrawingHistoryManager
    {
        private Stack<ShapeBase> _undoStack = new Stack<ShapeBase>();
        private Stack<ShapeBase> _redoStack = new Stack<ShapeBase>();

        public void AddShape(ShapeBase shape)
        {
            _undoStack.Push(shape);
            _redoStack.Clear();
        }

        public ShapeBase Undo()
        {
            return null;
        }

        public ShapeBase Redo()
        {
            return null;
        }

        public bool CanUndo => _undoStack.Count > 0;
        public bool CanRedo => _redoStack.Count > 0;
        
        public List<ShapeBase> GetShapes()
        {
            return _undoStack.ToList();
        }
        
        public void Clear()
        {
            _undoStack.Clear();
            _redoStack.Clear();
        }
    }
}