using LevelGenerator;
using UnityEngine;

namespace FoW
{
    public class FOWSystemModel
    {
        public BetterList<IFOWRevealer> Revealers = new BetterList<IFOWRevealer>();
        public BetterList<IFOWRevealer> Added = new BetterList<IFOWRevealer>();
        public BetterList<IFOWRevealer> Removed = new BetterList<IFOWRevealer>();

        public Color32[] Buffer0;
        public Color32[] Buffer1;
        public Color32[] Buffer2;

        
        public float Elapsed;
        public float WorldSize;
        public int TextureSize;
        public Texture2D Texture;
        public float BlendFactor;
        public float NextUpdate;
        public FoWStates State = FoWStates.Blending;
        public int TextureSizeSqr;

        private Vector3 _revealersOffset;
        private float _updateFrequency;
        private float _textureBlendTime;
        private int _blurIterations;
        private float _radiusOffset;
        private bool _enableSystem;
        private bool _enableRender;
        private bool _enableFog;
        private bool _enableDebug;

        public float UpdateFrequency => _updateFrequency;
        public float TextureBlendTime => _textureBlendTime;
        public int BlurIterations => _blurIterations;
        public float RadiusOffset => _radiusOffset;
        public bool EnableSystem => _enableSystem;
        public bool EnableRender => _enableRender;
        public bool EnableFog => _enableFog;
        public bool EnableDebug => _enableDebug;
        public Vector3 RevealersOffset => _revealersOffset;

        public FOWSystemModel(DungeonConfiguration dungeonConfiguration, FOWConfigurator fOWConfigurator)
        {
            _revealersOffset = new Vector3(0f, 0f, 0f);
            _revealersOffset.x -= (dungeonConfiguration.GridSize.x * fOWConfigurator.FogOfWarModifier - dungeonConfiguration.GridSize.x) * 0.5f;
            _revealersOffset.z -= (dungeonConfiguration.GridSize.y * fOWConfigurator.FogOfWarModifier - dungeonConfiguration.GridSize.y) * 0.5f;
            WorldSize = dungeonConfiguration.GridSize.x * fOWConfigurator.FogOfWarModifier;
            TextureSize = dungeonConfiguration.GridSize.x * fOWConfigurator.FogOfWarModifier;

            _updateFrequency = fOWConfigurator.UpdateFrequency;
            _textureBlendTime = fOWConfigurator.TextureBlendTime;
            _blurIterations = fOWConfigurator.BlurIterations;
            _radiusOffset = fOWConfigurator.BlurIterations;
            _enableSystem = fOWConfigurator.EnableSystem;
            _enableRender = fOWConfigurator.EnableRender;
            _enableFog = fOWConfigurator.EnableFog;
            _enableDebug = fOWConfigurator.EnableDebug;

            TextureSizeSqr = TextureSize * TextureSize;
            Buffer0 = new Color32[TextureSizeSqr];
            Buffer1 = new Color32[TextureSizeSqr];
            Buffer2 = new Color32[TextureSizeSqr];
            Revealers.Clear();
            Removed.Clear();
            Added.Clear();
        }
    }
}