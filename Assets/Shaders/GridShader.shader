Shader "Custom/GridShader" {
  Properties {
    // _MainTex("Texture", 2D) = "white" {}
  }

  SubShader {
    Pass {
      CGPROGRAM
      #pragma vertex vert_img
      #pragma fragment frag
      #include "UnityCG.cginc" // required for v2f_img

      // Properties
      // sampler2D _MainTex;

      int rows;
      int cols;

      StructuredBuffer<int> currentState;

      int get_index(int x, int y) {
        return cols * y + x;
      }

      float4 frag(v2f_img input) : COLOR {
        // sample texture for color
        // float4 base = tex2D(_MainTex, input.uv);

        int r = floor(input.uv[0] * rows);
        int c = floor(input.uv[1] * cols);

        return float4(0.0, currentState[get_index(c, r)], 0.0, 1.0);
      }
      ENDCG
}}}