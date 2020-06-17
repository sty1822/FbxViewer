#version 430 core

//
layout (location = 0) in vec3 position;
layout (location = 3) in int bone_id;

//
layout (location = 8) uniform vec3 light_dir;
layout (location = 9) uniform float height;
layout (location = 10) uniform mat4 model_matrix;
layout (location = 11) uniform mat4 vp_matrix;
layout (location = 12) uniform mat4[200] bone_matrices;

//
void main(void)
{
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

	vec4 world_pos = model_matrix * v;

	//
	float opposite = world_pos.z - height;
	float cosTheta = -light_dir.z;
	float hypotenuse = opposite / cosTheta;

	vec4 shadow_pos = world_pos + (vec4(light_dir, 0.0) * hypotenuse);

	gl_Position = vp_matrix * shadow_pos;
}