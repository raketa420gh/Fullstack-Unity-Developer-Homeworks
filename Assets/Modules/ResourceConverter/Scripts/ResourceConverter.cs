namespace Modules.ResourceConverter
{
    public class ResourceConverter
    {
        public int InputCapacity => _inputCapacity;
        public int OutputCapacity => _outputCapacity;
        public int InputCount => _inputCount;
        public int OutputCount => _outputCount;
        public bool IsConverting => _isConverting;

        private readonly int _inputCapacity;
        private readonly int _outputCapacity;
        private int _inputCount;
        private int _outputCount;
        private bool _isConverting;

        public ResourceConverter(int inputCapacity = 1, int outputCapacity = 1)
        {
            _inputCapacity = inputCapacity;
            _outputCapacity = outputCapacity;
        }

        public void AddInputResource(int count, out int changeCount)
        {
            int canAddCount = GetFreeInputResourceCount();
            
            _inputCount += canAddCount;
            
            if (count > canAddCount)
                changeCount = count - canAddCount;
            else
                changeCount = 0;
        }

        public void StartConverting()
        {
            if (_inputCount > 0)
                _isConverting = true;
        }


        public int GetFreeInputResourceCount()
        {
            if (_inputCapacity <= 0 || _inputCount <= 0 || _inputCount == _inputCapacity)
                return 0;

            return _inputCapacity - _inputCount;
        }

        private bool IsInputEmpty()
        {
            return _inputCount <= 0;
        }

        private bool IsOutputEmpty()
        {
            return _outputCount <= 0;
        }
    }
}