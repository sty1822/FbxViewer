#version 430 core

//
in VS_OUT
{
	vec4 color;
} fs_in;

void main(void)
{
	gl_FragColor = fs_in.color;
}