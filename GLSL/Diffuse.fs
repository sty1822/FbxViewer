#version 430 core

// 변수
uniform sampler2D texture;

// 버텍스 쉐이더로부터의 입력
in VS_OUT
{
	vec2 texcoord;
} fs_in;

// 프래그먼트 쉐이더의 시작점
void main(void)
{
	gl_FragColor = texture2D(texture, fs_in.texcoord);
}