Shader "Vertex Color Lit"
{
	Properties
	{
		_MainTex("Texture", 2D) = "white" {}
	}
	SubShader
	{
		Tags{ "Queue" = "Geometry" }
		Pass
		{
			BindChannels
			{
				Bind "Color", color
				Bind "Vertex", vertex
				Bind "TexCoord", texcoord
			}

			SetTexture[_MainTex]
			{
				Combine texture * primary
			}
		}
	}
}