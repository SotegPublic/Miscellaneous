using UnityEngine;
using FoW;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using LevelGenerator;

/// <summary>
/// Описание: Сцена туман войны
/// 
/// @by wsh 2017-05-20
/// </summary>

public class MapFOWRender
{
    private FOWRender _fOWRender;
    private AsyncOperationHandle<GameObject> _fOWRenderHandle;
    FOWController _fOWController;
    FOWSystem _fOWSystem;
    FOWConfigurator _fOWConfigurator;
    DungeonConfiguration _dungeonConfiguration;

    public MapFOWRender(FOWController fOWController, FOWSystem fOWSystem, AssetReferenceGameObject renderObjectReference,
        FOWConfigurator fOWConfigurator, DungeonConfiguration dungeonConfiguration)
    {
        _fOWController = fOWController;
        _fOWSystem = fOWSystem;
        _fOWConfigurator = fOWConfigurator;
        _dungeonConfiguration = dungeonConfiguration;

        Addressables.InstantiateAsync(renderObjectReference).Completed += Init;
    }

    private void Init(AsyncOperationHandle<GameObject> renderObjectHandle)
    {
        _fOWRenderHandle = renderObjectHandle;
        var rendererObject = renderObjectHandle.Result;
        _fOWRender = rendererObject.GetComponent<FOWRender>();

        if (_fOWRender != null)
        {
            float fCenterX = _dungeonConfiguration.GridSize.x * 0.5f;
            float fCenterZ = _dungeonConfiguration.GridSize.y * 0.5f;
            float scale = _fOWSystem.WorldSize / 128f * 2.56f;

            _fOWRender.transform.position = new Vector3(fCenterX, 2f, fCenterZ);
            _fOWRender.transform.eulerAngles = new Vector3(-90f, 180f, 0f);
            _fOWRender.transform.localScale = new Vector3(scale, scale, 1f);
        }

        _fOWRender.ExploredColor = _fOWConfigurator.ExploredColor;
        _fOWRender.UnexploredColor = _fOWConfigurator.UnexploredColor;

        _fOWController.RegistrateRender(_fOWRender);

        renderObjectHandle.Completed -= Init;
    }

    public void Dispose()
    {
        Addressables.ReleaseInstance(_fOWRenderHandle);
    }
}
