#version 430 core

//[VS In Per Vertex]===================================
layout (location = 0) in vec3 position;
layout (location = 2) in vec2 texcoord;
layout (location = 3) in int bone_id;

//[VS In Uniform]======================================
layout (location = 12) uniform mat4 mvp_matrix;
layout (location = 13) uniform mat4[200] bone_matrices;

//[VS Out]=============================================
out VS_OUT
{
	vec2 texcoord;
} vs_out;

//[Entry]==============================================
void main(void)
{
	vs_out.texcoord = texcoord;

	// skinned animation
	vec4 v;

	if(bone_id < 0)
	{
		v = vec4(position, 1.0);
	}
	else
	{
		v = vec4(position, 1.0) * bone_matrices[bone_id];
	}

	gl_Position = mvp_matrix * v;
}