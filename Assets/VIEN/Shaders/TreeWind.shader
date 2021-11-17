//Originaly developed by: Renan Bomtempo
//https://www.behance.net/renanBomtempo

Shader "Custom/TreeWind"
{
    Properties
    {
		_MainTex("Main Texture", 2D) = "white" {}
		_Tint("Tint", Color) = (1,1,1,1)

		_wind_dir("Wind Direction", Vector) = (0.5,0.05,0.5,0)
		_wind_str("Wind Strength", range(5,50)) = 15

		_leaves_disp("Turbulence", float) = 0.003
		_leaves_speed("Speed", float) = 1

		_influence("Vertex Influence", range(0,1)) = 1
    }
    SubShader{
		CGPROGRAM
		#pragma target 3.0
		#pragma surface surf Lambert vertex:vert addshadow

		//declared variables
		float4 _wind_dir;
		float _wind_str;
		float _leaves_disp;
		float _leaves_speed;
		float _influence;

		sampler2D _MainTex;
		fixed4 _Tint;

		//structs
		struct Input {
			float2 uv_MainTex;
		};

		//vertex movements
		void vert(inout appdata_full i) {

			//get the vertex's world position 
			float3 worldPos = mul(unity_ObjectToWorld, i.vertex).xyz;

			//leaves movement in wind
			i.vertex.x += cos(_Time.w * i.vertex.x * _leaves_speed + (worldPos.x / _wind_str)) * _leaves_disp * _wind_dir.x * i.color.b * _influence;

			i.vertex.z += cos(_Time.w * i.vertex.z * _leaves_speed + (worldPos.x / _wind_str)) * _leaves_disp * _wind_dir.z * i.color.b * _influence;

		}

		//surface Shader
		void surf(Input IN, inout SurfaceOutput o) {
			fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * _Tint;
			o.Albedo = c.rgb;
			o.Alpha = c.a;
		}

		ENDCG
	}
	Fallback "Diffuse"
}

