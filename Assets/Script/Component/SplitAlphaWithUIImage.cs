using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace tiger.Component
{

[AddComponentMenu("Split Alpha/UI Image")]
[RequireComponent(typeof(CanvasRenderer))]
public class SplitAlphaWithUIImage : Image
{
    private static string MAIN_TEX = "_MainTex";
    private static string ALPHA_TEX = "_AlphaTex";
    // private static string COLOR_MASK = "_ColorMask";

    [SerializeField]
    private Texture2D m_RGBSource;
    public Texture2D RGB_SOURCE{
        set
        {
            m_RGBSource = value;
        }
    }

    [SerializeField]
    private Texture2D m_AlphaSource;
    public Texture2D ALPHA_SOURCE{
        set
        {
            m_AlphaSource = value;
        }
    }

    public void UpdateTexture2D()
    {
        this.material = CreateMaterial(m_RGBSource, m_AlphaSource);
        transform.GetComponent<RectTransform>().sizeDelta = new Vector2(m_RGBSource.width, m_RGBSource.height);

        Resources.UnloadUnusedAssets();
    }

    private Material CreateMaterial(Texture2D rgb, Texture2D alpha, Color? color=null)
    {
        Material m = new Material(Shader.Find("Custom/SplitAlphaUIImageShader") as Shader);
        m.SetTexture(MAIN_TEX, m_RGBSource);
        m.SetTexture(ALPHA_TEX, m_AlphaSource);
        m.color = !color.HasValue ? Color.white:color.Value;

        return m;
    }

    private void CleanMaterial()
    {
        this.material = null;
        Resources.UnloadUnusedAssets();
    }

#if UNITY_EDITOR

    #pragma warning disable 0114
    public void OnValidate()
    {
        if(m_RGBSource && m_AlphaSource)
        {
            UpdateTexture2D();
        }
        else
        {
            CleanMaterial();
        }
    }

#endif

}
}