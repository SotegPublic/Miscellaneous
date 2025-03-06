using UnityEngine;


namespace FoW
{
    public class FOWRender : MonoBehaviour
    {
        public Color UnexploredColor = new Color(0f, 0f, 0f, 250f / 255f);
        public Color ExploredColor = new Color(0f, 0f, 0f, 100f / 255f);
        public FOWSystem FOWSystemInstance;
        private Material _mat;

        void Start()
        {
            if (_mat == null)
            {
                MeshRenderer render = GetComponentInChildren<MeshRenderer>();
                if (render != null)
                {
                    _mat = render.sharedMaterial;
                }
            }

            if (_mat == null)
            {
                enabled = false;
                return;
            }
        }

        public void Activate(bool active)
        {
            gameObject.SetActive(active);
        }

        public bool IsActive
        {
            get
            {
                return gameObject.activeSelf;
            }
        }

        void OnWillRenderObject()
        {
            if (FOWSystemInstance is null) return;

            if (_mat != null && FOWSystemInstance.Texture != null)
            {
                _mat.SetTexture("_MainTex", FOWSystemInstance.Texture);
                _mat.SetFloat("_BlendFactor", FOWSystemInstance.BlendFactor);
                if (FOWSystemInstance.EnableFog)
                {
                    _mat.SetColor("_Unexplored", UnexploredColor);
                }
                else
                {
                    _mat.SetColor("_Unexplored", ExploredColor);
                }
                _mat.SetColor("_Explored", ExploredColor);
            }
        }
    }
}