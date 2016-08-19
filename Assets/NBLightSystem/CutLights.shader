Shader "NBLightSystem/CutLights" {
  Properties {
    _GradStartDist ("Gradient Start Distance", Float) = 5
    _GradEndDist ("Gradient End Distance", Float) = 20
    _ScreenX ("Screen X", Float) = 100
    _ScreenY ("Screen Y", Float) = 100
    _ScreenWidth ("Screen Width", Float) = 200
    _ScreenHeight ("Screen Height", Float) = 200
  }

  CGINCLUDE

  #include "UnityCG.cginc"

  struct v2f {
    float4 pos : SV_POSITION;
    float2 uv : TEXCOORD0;
  };

  sampler2D _MainTex;

  float _ScreenX = 100;
  float _ScreenY = 100;
  float _ScreenWidth = 200;
  float _ScreenHeight = 200;
  float _GradStartDist;
  float _GradEndDist;

  v2f vert( appdata_img v ) {
    v2f o;
    o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
    o.uv = v.texcoord.xy;

    return o;
  }

  half4 frag (v2f i) : SV_Target {
    float2 screenPos = float2(_ScreenX, _ScreenY);
    float2 pos = float2(i.uv.x * _ScreenWidth, i.uv.y * _ScreenHeight);
    float len = length(screenPos - pos);

    half res = (len - _GradStartDist) / (_GradEndDist - _GradStartDist);

    return half4(0, 0, 0, res);
  }

  half4 fragSimple (v2f i) : SV_Target {
    return tex2D(_MainTex, i.uv);
  }

  ENDCG

  SubShader {
    // No culling or depth
    Cull Off ZWrite Off ZTest Always

    Pass {
      Blend One One, One One
      BlendOp Add, Min

      CGPROGRAM
      #pragma vertex vert
      #pragma fragment frag
      ENDCG
    }

  }
}
