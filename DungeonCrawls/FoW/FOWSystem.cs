using UnityEngine;
using System.Threading;
using LevelGenerator;

namespace FoW
{
    public class FOWSystem
    {
        private FOWSystemModel _model;
        private Thread _thread;
        private volatile bool _threadWork;

        public Texture2D Texture => _model.Texture;
        public float BlendFactor => _model.BlendFactor;
        public float WorldSize => _model.WorldSize;
        public bool EnableFog => _model.EnableFog;
        public bool EnableRender => _model.EnableRender;

        public FOWSystem(DungeonConfiguration dungeonConfiguration, FOWConfigurator fOWConfigurator)
        {
            _model = new FOWSystemModel(dungeonConfiguration, fOWConfigurator);

            _thread = new Thread(ThreadUpdate);
            _threadWork = true;
            _thread.Start();
        }

        public void AddRevealer(IFOWRevealer rev)
        {
            if (rev != null)
            {
                lock (_model.Added) _model.Added.Add(rev);
            }
        }

        public void RemoveRevealer(IFOWRevealer rev)
        {
            if (rev != null)
            {
                lock (_model.Removed) _model.Removed.Add(rev);
            }
        } 

        public void Update()
        {
            if (!_model.EnableSystem)
            {
                return;
            }

            if (_model.TextureBlendTime > 0f)
            {
                _model.BlendFactor = Mathf.Clamp01(_model.BlendFactor + Time.deltaTime / _model.TextureBlendTime);
            }
            else _model.BlendFactor = 1f;

            if (_model.State == FoWStates.Blending)
            {
                float time = Time.time;

                if (_model.NextUpdate < time)
                {
                    _model.NextUpdate = time + _model.UpdateFrequency;
                    _model.State = FoWStates.NeedUpdate;
                }
            }
            else if (_model.State != FoWStates.NeedUpdate)
            {
                UpdateTexture();
            }
        }

        void ThreadUpdate()
        {
            System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();

            while (_threadWork)
            {
                if (_model.State == FoWStates.NeedUpdate)
                {
                    sw.Reset();
                    sw.Start();
                    UpdateBuffer();
                    sw.Stop();
                    _model.Elapsed = 0.001f * (float)sw.ElapsedMilliseconds;
                    _model.State = FoWStates.UpdateTexture;
                }
                Thread.Sleep(1);
            }
#if UNITY_EDITOR
            Debug.Log("FOW thread exit!");
#endif
        }

        void UpdateBuffer()
        {
            if (_model.Added.size > 0)
            {
                lock (_model.Added)
                {
                    while (_model.Added.size > 0)
                    {
                        int index = _model.Added.size - 1;
                        _model.Revealers.Add(_model.Added.buffer[index]);
                        _model.Added.RemoveAt(index);
                    }
                }
            }

            if (_model.Removed.size > 0)
            {
                lock (_model.Removed)
                {
                    while (_model.Removed.size > 0)
                    {
                        int index = _model.Removed.size - 1;
                        _model.Revealers.Remove(_model.Removed.buffer[index]);
                        _model.Removed.RemoveAt(index);
                    }
                }
            }

            float factor = (_model.TextureBlendTime > 0f) ? Mathf.Clamp01(_model.BlendFactor + _model.Elapsed / _model.TextureBlendTime) : 1f;

            for (int i = 0, imax = _model.Buffer0.Length; i < imax; ++i)
            {
                _model.Buffer0[i] = Color32.Lerp(_model.Buffer0[i], _model.Buffer1[i], factor);
                _model.Buffer1[i].r = 0;
            }

            float worldToTex = (float)_model.TextureSize / _model.WorldSize;

            for (int i = 0; i < _model.Revealers.size; ++i)
            {
                IFOWRevealer rev = _model.Revealers[i];
                if (rev.IsValid())
                {
                    RevealUsingRadius(rev, worldToTex);
                }
            }

            for (int i = 0; i < _model.BlurIterations; ++i) BlurVisibility();

            RevealMap();

            MergeBuffer();
        }

        void RevealUsingRadius(IFOWRevealer r, float worldToTex)
        {
            Vector3 pos = (r.GetPosition() - _model.RevealersOffset) * worldToTex;
            float radius = r.GetRadius() * worldToTex + _model.RadiusOffset;

            int xmin = Mathf.RoundToInt(pos.x - radius);
            int ymin = Mathf.RoundToInt(pos.z - radius);
            int xmax = Mathf.RoundToInt(pos.x + radius);
            int ymax = Mathf.RoundToInt(pos.z + radius);

            int cx = Mathf.RoundToInt(pos.x);
            int cy = Mathf.RoundToInt(pos.z);

            cx = Mathf.Clamp(cx, 0, _model.TextureSize - 1);
            cy = Mathf.Clamp(cy, 0, _model.TextureSize - 1);

            int radiusSqr = Mathf.RoundToInt(radius * radius);

            for (int y = ymin; y < ymax; ++y)
            {
                if (y > -1 && y < _model.TextureSize)
                {
                    int yw = y * _model.TextureSize;

                    for (int x = xmin; x < xmax; ++x)
                    {
                        if (x > -1 && x < _model.TextureSize)
                        {
                            int xd = x - cx;
                            int yd = y - cy;
                            int dist = xd * xd + yd * yd;

                            // Reveal this pixel
                            if (dist < radiusSqr) _model.Buffer1[x + yw].r = 255;
                        }
                    }
                }
            }
        }

        void BlurVisibility()
        {
            Color32 c;

            for (int y = 0; y < _model.TextureSize; ++y)
            {
                int yw = y * _model.TextureSize;
                int yw0 = (y - 1);
                if (yw0 < 0) yw0 = 0;
                int yw1 = (y + 1);
                if (yw1 == _model.TextureSize) yw1 = y;

                yw0 *= _model.TextureSize;
                yw1 *= _model.TextureSize;

                for (int x = 0; x < _model.TextureSize; ++x)
                {
                    int x0 = (x - 1);
                    if (x0 < 0) x0 = 0;
                    int x1 = (x + 1);
                    if (x1 == _model.TextureSize) x1 = x;

                    int index = x + yw;
                    int val = _model.Buffer1[index].r;

                    val += _model.Buffer1[x0 + yw].r;
                    val += _model.Buffer1[x1 + yw].r;
                    val += _model.Buffer1[x + yw0].r;
                    val += _model.Buffer1[x + yw1].r;

                    val += _model.Buffer1[x0 + yw0].r;
                    val += _model.Buffer1[x1 + yw0].r;
                    val += _model.Buffer1[x0 + yw1].r;
                    val += _model.Buffer1[x1 + yw1].r;

                    c = _model.Buffer2[index];
                    c.r = (byte)(val / 9);
                    _model.Buffer2[index] = c;
                }
            }

            Color32[] temp = _model.Buffer1;
            _model.Buffer1 = _model.Buffer2;
            _model.Buffer2 = temp;
        }

        void RevealMap()
        {
            for (int index = 0; index < _model.TextureSizeSqr; ++index)
            {
                if (_model.Buffer1[index].g < _model.Buffer1[index].r)
                {
                    _model.Buffer1[index].g = _model.Buffer1[index].r;
                }
            }
        }

        void MergeBuffer()
        {
            for (int index = 0; index < _model.TextureSizeSqr; ++index)
            {
                _model.Buffer0[index].b = _model.Buffer1[index].r;
                _model.Buffer0[index].a = _model.Buffer1[index].g;
            }
        }

        void UpdateTexture()
        {
            if (!_model.EnableRender)
            {
                return;
            }

            if (_model.Texture == null)
            {
                _model.Texture = new Texture2D(_model.TextureSize, _model.TextureSize, TextureFormat.ARGB32, false);

                _model.Texture.wrapMode = TextureWrapMode.Clamp;

                _model.Texture.SetPixels32(_model.Buffer0);
                _model.Texture.Apply();
                _model.State = FoWStates.Blending;
            }
            else if (_model.State == FoWStates.UpdateTexture)
            {
                _model.Texture.SetPixels32(_model.Buffer0);
                _model.Texture.Apply();
                _model.BlendFactor = 0f;
                _model.State = FoWStates.Blending;
            }
        }

        public bool IsVisible(Vector3 pos)
        {
            if (_model.Buffer0 == null || _model.Buffer1 == null)
            {
                return false;
            }

            pos -= _model.RevealersOffset;
            float worldToTex = (float)_model.TextureSize / _model.WorldSize;
            int cx = Mathf.RoundToInt(pos.x * worldToTex);
            int cy = Mathf.RoundToInt(pos.z * worldToTex);

            cx = Mathf.Clamp(cx, 0, _model.TextureSize - 1);
            cy = Mathf.Clamp(cy, 0, _model.TextureSize - 1);
            int index = cx + cy * _model.TextureSize;
            return _model.Buffer0[index].r > 64 || _model.Buffer1[index].r > 0;
        }

        public bool IsExplored(Vector3 pos)
        {
            if (_model.Buffer0 == null)
            {
                return false;
            }
            pos -= _model.RevealersOffset;

            float worldToTex = (float)_model.TextureSize / _model.WorldSize;
            int cx = Mathf.RoundToInt(pos.x * worldToTex);
            int cy = Mathf.RoundToInt(pos.z * worldToTex);

            cx = Mathf.Clamp(cx, 0, _model.TextureSize - 1);
            cy = Mathf.Clamp(cy, 0, _model.TextureSize - 1);
            return _model.Buffer0[cx + cy * _model.TextureSize].g > 0;
        }

        public void Dispose()
        {
            if (_thread != null)
            {
                _threadWork = false;
                _thread.Join();
                _thread = null;
            }

            _model.Buffer0 = null;
            _model.Buffer1 = null;
            _model.Buffer2 = null;

            if (_model.Texture != null)
            {
                Object.Destroy(_model.Texture);
                _model.Texture = null;
            }
        }
    }
}