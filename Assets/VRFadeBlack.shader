Shader "Custom/VRFadeBlack"
{
    Properties
    {
        _Color ("Fade Color", Color) = (0,0,0,1)
    }

    SubShader
    {
        Tags { "Queue"="Overlay" "RenderType"="Transparent" }
        ZWrite On
        Lighting Off
        Cull Off
        Fog { Mode Off }
        Blend SrcAlpha OneMinusSrcAlpha

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            fixed4 _Color;

            struct appdata
            {
                float4 vertex : POSITION;
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
            };

            v2f vert (appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                return _Color;
            }
            ENDCG
        }
    }
}
