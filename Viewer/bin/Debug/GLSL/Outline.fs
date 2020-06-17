#version 430 core
layout (location = 11) uniform vec4 color;

void main(void)
{
	gl_FragColor = color;
}