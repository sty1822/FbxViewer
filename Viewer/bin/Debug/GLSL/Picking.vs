#version 430 core

//
layout (location = 0) in vec3 position;
layout (location = 3) in int bone_id;

//
layout (location = 11) uniform vec4 key_color;
layout (location = 12) uniform mat4 mvp_matrix;
layout (location = 13) uniform mat4[200] bone_matrices;

out VS_OUT
{
	vec4 color;
} vs_out;

void main(void)
{
	vs_out.color = key_color;

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

	gl_Position = mvp * v;
}