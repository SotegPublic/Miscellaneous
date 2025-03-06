using UnityEngine;

namespace FoW
{
    [CreateAssetMenu(menuName = "LevelConfig/FOWConfigurator", fileName = nameof(FOWConfigurator))]
    public class FOWConfigurator : ScriptableObject
    {
        [SerializeField] private int _fogOfWarSizeModifier = 1;
        [SerializeField] private float _updateFrequency = 0.3f;
        [SerializeField] private float _textureBlendTime = 0.5f;
        [SerializeField] private int _blurIterations = 2;
        [SerializeField] private float _radiusOffset = 0;
        [SerializeField] private bool _enableSystem = true;
        [SerializeField] private bool _enableRender = true;
        [SerializeField] private bool _enableFog = true;
        [SerializeField] private bool _enableDebug = false;
        [SerializeField] private Color _unexploredColor = new Color(0f, 0f, 0f, 250f / 255f);
        [SerializeField] private Color _exploredColor = new Color(0f, 0f, 0f, 100f / 255f);

        public int FogOfWarModifier => _fogOfWarSizeModifier;
        public float UpdateFrequency => _updateFrequency;
        public float TextureBlendTime => _textureBlendTime;
        public int BlurIterations => _blurIterations;
        public float RadiusOffset => _radiusOffset;
        public bool EnableSystem => _enableSystem;
        public bool EnableRender => _enableRender;
        public bool EnableFog => _enableFog;
        public bool EnableDebug => _enableDebug;
        public Color UnexploredColor => _unexploredColor;
        public Color ExploredColor => _exploredColor;
    }
}