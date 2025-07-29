Shader "Sprite/Filled"
{
    Properties
    {
        [PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
        [MaterialToggle] PixelSnap ("Pixel snap", Float) = 0
        [HideInInspector] _RendererColor ("RendererColor", Color) = (1,1,1,1)
        [HideInInspector] _Flip ("Flip", Vector) = (1,1,1,1)
        [PerRendererData] _AlphaTex ("External Alpha", 2D) = "white" {}
        [PerRendererData] _EnableExternalAlpha ("Enable External Alpha", Float) = 0
        _Color ("Tint", Color) = (1,1,1,1)

        [Enum(Horizontal, 0, Vertical, 1)]_FillType ("FillType", Float) = 0
        _FillAmount ("FillAmount", Range(0,1)) = 1
        [Toggle]_FillFlip ("FillFlip", Float) = 0
    }

    SubShader
    {
        Tags
        {
            "Queue"="Transparent"
            "IgnoreProjector"="True"
            "RenderType"="Transparent"
            "PreviewType"="Plane"
            "CanUseSpriteAtlas"="True"
        }
 
        Cull Off
        Lighting Off
        ZWrite Off
        Blend One OneMinusSrcAlpha
 
        Pass
        {
        CGPROGRAM
            #pragma vertex SpriteVert
            // #pragma fragment SpriteFrag
            #pragma fragment Frag
            #pragma target 2.0
            #pragma multi_compile_instancing
            #pragma multi_compile_local _ PIXELSNAP_ON
            #pragma multi_compile _ ETC1_EXTERNAL_ALPHA
            #include "UnitySprites.cginc"
            
            float _FillType;
            fixed _FillAmount;
            fixed _FillFlip;

            fixed4 Frag(v2f IN) : SV_Target
            {
                fixed4 col = SampleSpriteTexture (IN.texcoord) * IN.color;
                //º∆À„fill
                float uvValue = lerp(IN.texcoord.x, IN.texcoord.y, sign(saturate(_FillType)));
                float fValue1 = lerp(uvValue, _FillAmount, sign(saturate(_FillFlip)));
                float fValue2 = lerp(_FillAmount, uvValue, sign(saturate(_FillFlip)));
                col.a *= step(fValue1, fValue2);
                //end
                col.rgb *= col.a;
                return col;
            }
        ENDCG
        }
    }
}
