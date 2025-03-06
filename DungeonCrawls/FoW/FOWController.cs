using System.Collections.Generic;
using Engine;
using LevelGenerator;
using Units;
using UnityEngine.AddressableAssets;
using Utils;

namespace FoW
{
    public class FOWController: IController, IUpdatable
    {
        private FOWSystem _fOWSystem;
        private MapFOWRender _mapFOWRender;
        private AssetReferenceGameObject _renderObjectReference;
        private DungeonConfiguration _dungeonConfiguration;
        private FOWConfigurator _fOWConfigurator;
        private bool _isSystemWasLoaded;


        private List<IFOWRevealer> _revealers = new List<IFOWRevealer>();

        private List<FOWRender> _renders = new List<FOWRender>();
        private List<Unit> _playerUnits;

        public FOWController(List<Unit> playerUnits, AssetReferenceGameObject renderObjectReference, GlobalConfigLoader globalConfig)
        {
            _playerUnits = playerUnits;
            _renderObjectReference = renderObjectReference;
            _dungeonConfiguration = globalConfig.DungeonConfiguration;
            _fOWConfigurator = globalConfig.FOWConfigurator;

            Init();
        }

        public void Init()
        {
            _revealers.Clear();
            _renders.Clear();

            _fOWSystem = new FOWSystem(_dungeonConfiguration, _fOWConfigurator);
            AddUnits(_playerUnits);

            _mapFOWRender = new MapFOWRender(this, _fOWSystem, _renderObjectReference, _fOWConfigurator, _dungeonConfiguration);
            _isSystemWasLoaded = true;
        }

        protected void AddUnits(List<Unit> units)
        {
            for(int i = 0; i < units.Count; i++)
            {
                var revealer = new FOWCharactorRevealer(units[i]);
                _fOWSystem.AddRevealer(revealer);
                _revealers.Add(revealer);
            }
        }

        public void RegistrateRender(FOWRender fOWRender)
        {
            if (fOWRender != null)
            {
                _renders.Add(fOWRender);
                fOWRender.FOWSystemInstance = _fOWSystem;
            }
        }
        
        private void ActivateRender(FOWRender render, bool active)
        {
            if (render != null)
            {
                render.Activate(active);
            }
        }

        public void LocalUpdate(float deltaTime)
        {
            if (!_isSystemWasLoaded)
            {
                return;
            }

            int deltaMS = (int)(deltaTime * 1000f);
            UpdateRenders();
            UpdateRevealers(deltaMS);
            _fOWSystem.Update();
        }

        protected void UpdateRenders()
        {
            for (int i = 0; i < _renders.Count; i++)
            {
                ActivateRender(_renders[i], _fOWSystem.EnableRender);
            }
        }

        protected void UpdateRevealers(int deltaMS)
        {
            for (int i = _revealers.Count - 1; i >= 0 ; i--)
            {
                IFOWRevealer revealer = _revealers[i];
                revealer.Update(deltaMS);
                if (!revealer.IsValid())
                {
                    _revealers.RemoveAt(i);
                    _fOWSystem.RemoveRevealer(revealer);
                    revealer.Release();
                }
            }
        }

        public void Dispose()
        {
            for (int i = 0; i < _revealers.Count; i++)
            {
                IFOWRevealer revealer = _revealers[i];
                if (revealer != null)
                {
                    revealer.Release();
                }
            }
            _revealers.Clear();

            for (int i = 0; i < _renders.Count; i++)
            {
                var render = _renders[i];
                if (render != null)
                {
                    render.enabled = false;
                }
            }
            _renders.Clear();
            _mapFOWRender.Dispose();
            _mapFOWRender = null;
            _fOWSystem.Dispose();
        }
    }
}
