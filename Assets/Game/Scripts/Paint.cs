using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Paint : MonoBehaviour
{
    [Header("Текстура для стирания")]
    [Range(2, 512)]
    [SerializeField] private int _textureSize = 128;
    [SerializeField] private TextureWrapMode _textureWrapMode;
    [SerializeField] private FilterMode _filterMode;
    [SerializeField] private Texture2D _texture;
    [SerializeField] private Material _material;

    [Header("Кисть")]
    [SerializeField] private int _brushSize = 8;
    [SerializeField] private Color _color;

    [Header("Доп компоненты")]
    [SerializeField] private Camera _camera;
    [SerializeField] private Collider _collider;

    private void OnValidate() //Заменить на старт или авейк
    {
        if (_texture == null)
        {
            _texture = new Texture2D(_textureSize, _textureSize);
        }

        if (_texture.width != _textureSize)
        {
            _texture.Reinitialize(_textureSize, _textureSize);
        }

        _texture.wrapMode = _textureWrapMode;
        _texture.filterMode = _filterMode;
        _material.mainTexture = _texture;
        _texture.Apply();
    }

    private void Update()
    {
        if (Input.GetMouseButton(0))
        {
            Ray ray = _camera.ScreenPointToRay(Input.mousePosition);

            RaycastHit hit;
            if (_collider.Raycast(ray, out hit, 100f))
            {
                int rayX = (int)(hit.textureCoord.x * _textureSize);
                int rayY = (int)(hit.textureCoord.y * _textureSize);

                DrawCircle(rayX, rayY);
                _texture.Apply();
            }
        }
    }

    private void DrawCircle(int rayX, int rayY)
    {
        for (int x = 0; x < _brushSize; x++)
        {
            for (int y = 0; y < _brushSize; y++)
            {
                float x2 = Mathf.Pow(x - _brushSize / 2, 2);
                float y2 = Mathf.Pow(y - _brushSize / 2, 2);
                float r2 = Mathf.Pow(_brushSize / 2 - .5f, 2);

                if (x2 + y2 < r2)
                {


                    _texture.SetPixel(rayX + x - _brushSize / 2, rayY + y - _brushSize / 2, _color);
                }

            }
        }
    }
}
