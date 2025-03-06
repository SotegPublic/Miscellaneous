using UnityEngine;

public static class GameObjectExtentions
{
    public static Transform FindChildrenTransformByName(this GameObject gameObject, string name)
    {
        var transformsList = gameObject.GetComponentsInChildren<Transform>();
        Transform resultObjectTransform = null;

        for (int i = 0; i < transformsList.Length; i++)
        {
            if (transformsList[i].name.Equals(name))
            {
                resultObjectTransform = transformsList[i];
            }
        }

        return resultObjectTransform;
    }
}