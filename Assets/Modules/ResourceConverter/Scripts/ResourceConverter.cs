using System;

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
        private readonly ResourceType _inputResourceType;
        private readonly ResourceType _outputResourceType;
        private float _convertingDuration;
        private int _inputCount;
        private int _outputCount;
        private bool _isConverting;

        public ResourceConverter(ResourceType inputType, ResourceType outputType, 
            int inputCapacity = 1, int outputCapacity = 1, float convertingDuration = 1)
        {
            if (inputType == outputType)
                throw new ArgumentException();
            
            if (inputCapacity <= 0 || outputCapacity <= 0)
                throw new ArgumentOutOfRangeException();
            
            if (convertingDuration <= 0)
                throw new ArgumentOutOfRangeException();

            _inputResourceType = inputType;
            _outputResourceType = outputType;
            _inputCapacity = inputCapacity;
            _outputCapacity = outputCapacity;
            _convertingDuration = convertingDuration;
        }
        
        private int GetFreeInputResourceCount()
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