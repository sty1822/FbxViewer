#version 430 core

uniform sampler2D texture;

//[FS In ]=============================================
in VS_OUT
{
	vec3 N;
	vec3 L;
	vec3 V;
	vec2 texcoord;
} fs_in;


//[FS In Uniform]======================================
// 재질
layout (std140, binding = 2) uniform phong_mat_block
{
	vec3 specular_albedo;
	float specular_power;
};


//[Material Routine]===================================
// type 선언
subroutine vec4 SubMaterialShading();

// 함수 선언
layout (index = 1)
subroutine(SubMaterialShading)
vec4 funcBasic()
{
	return vec4(0.5, 0.5, 0.5, 1);
}

layout (index = 2)
subroutine(SubMaterialShading)
vec4 funcDiffuse()
{
	return texture2D(texture, fs_in.texcoord);
}

layout (index = 3)
subroutine(SubMaterialShading)
vec4 funcPhong()
{
	// 입력 N, L, V 벡터를 정규화
	vec3 N = normalize(fs_in.N);
	vec3 L = normalize(fs_in.L);
	vec3 V = normalize(fs_in.V);

	// 로컬 좌표 상에서 R을 계산
	vec3 R = reflect(-L, N);

	//
	vec4 diffuse_albedo = texture2D(texture, fs_in.texcoord);

	// 각 프래그먼트별로 diffuse 및 specular 요소 계산 // todo : diffuse min 도 밖으로 빼자!
	vec3 diffuse = max(dot(N, L), 0.8) * diffuse_albedo.xyz;
	vec3 specular = pow(max(dot(R, V), 0.0), specular_power) * specular_albedo;

	// 프레임버퍼에 최종 색상 출력
	return vec4(diffuse + specular, diffuse_albedo.w);
}

// 서브루틴 유니폼 선언
subroutine uniform SubMaterialShading material_Routine;

//[Entry]==============================================
void main(void)
{
	gl_FragColor = material_Routine();
}