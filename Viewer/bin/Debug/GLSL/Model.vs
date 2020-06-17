#version 430 core

//[VS In Per Vertex]===================================
layout (location = 0) in vec3 position;
layout (location = 1) in vec3 normal;
layout (location = 2) in vec2 texcoord;
layout (location = 3) in int bone_id;


//[VS Out]=============================================
out VS_OUT
{
	vec3 N;
	vec3 L;
	vec3 V;
	vec2 texcoord;
} vs_out;


//[VS In Uniform]======================================
// 전역 uniform buffer object
layout (std140, binding = 0) uniform cam_trans_block
{
	mat4 view_matrix;
	mat4 proj_matrix;
};

// 모델
layout (std140, binding = 1) uniform model_block
{
	mat4 model_matrix;
};

layout (location = 13) uniform mat4[200] bone_matrices;

// 조명
layout (location = 7) uniform vec3 light_dir;



//[Animation Routine]==================================
struct AnimatedVertex
{
	vec4 position;
	vec4 normal;
};

// type 선언
subroutine AnimatedVertex SubAnimation();

// 함수 선언
layout (index = 1)
subroutine(SubAnimation)
AnimatedVertex funcAnimated()
{
	AnimatedVertex v;
	v.position = vec4(position, 1.0) * bone_matrices[bone_id];
	v.normal = vec4(normal, 0.0) * bone_matrices[bone_id];

	return v;
}

layout (index = 2)
subroutine(SubAnimation)
AnimatedVertex funcDontAnimated()
{
	AnimatedVertex v;
	v.position = vec4(position, 1.0);
	v.normal = vec4(normal, 0.0);
	return v;
}

// 서브루틴 유니폼 선언
subroutine uniform SubAnimation animated_Routine;


//[Entry]==============================================
void main(void)
{
	// skinned animation
	AnimatedVertex av = animated_Routine();

	mat4 mv_matrix = view_matrix * model_matrix;

	// 뷰 공간 좌표 계산
	vec4 P = mv_matrix * av.position;

	// 뷰 공간 노말 계산
	vs_out.N = mat3(mv_matrix) * av.normal.xyz;

	// 라이트 벡터 계산(전역 라이트) // vs_out.L = light_pos - P.xyz; // 지역 라이트
	vs_out.L = normalize(light_dir);

	// 뷰 벡터 계산
	vs_out.V = -P.xyz;

	// UV 좌표
	vs_out.texcoord = texcoord;

	// 각 버텍스의 절단 공간 위치 계산
	gl_Position = proj_matrix * P;
}