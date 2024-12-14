using System.Collections.Generic;

public sealed class ResourceConverter
{
    public bool IsLaunched => _isLaunched;
    public ResourceConfig[] ResourcesIn => _resourcesIn.ToArray();
    public ResourceConfig[] ResourcesOut => _resourceOut.ToArray();
    
    private ResourceType _resourceTypeIn;
    private ResourceType _resourceTypeOut;
    private readonly List<ResourceConfig> _resourcesIn = new();
    private readonly List<ResourceConfig> _resourceOut = new();
    private bool _isLaunched; 
    
    public ResourceConverter(ResourceType resourceTypeIn, ResourceType resourceTypeOut)
    {
        _resourceTypeIn = resourceTypeIn;
        _resourceTypeOut = resourceTypeOut;
    }

    public void Launch()
    {
        if (_resourcesIn.Count <= 0)
            return;
        
        _isLaunched = true;
    }
}