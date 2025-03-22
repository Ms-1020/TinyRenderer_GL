#version 330 core

in vec2 TexCoords;

out vec4 FragColor;

uniform sampler2D screenTexture;

const float offset = 1.0/300.0;


void main()
{
	vec2 offsets[9] = vec2[](
		vec2(-offset, offset),
		vec2(0.0,offset),
		vec2(offset,offset),
		vec2(-offset,0.0),
		vec2(0.0,0.0),
		vec2(offset,0.0),
		vec2(-offset,-offset),
		vec2(0.0,-offset),
		vec2(offset,-offset)
	);

	float kernel[9] = float[](
		1,1,1,
		1, -8,1,
		1,1,1
	);

	vec3 sampleTex[9];
	for(int i = 0; i < 9; ++i)
	{
		sampleTex[i] = vec3(texture(screenTexture, TexCoords.st + offsets[i]));
	}
	vec3 colo = vec3(0.0);
	for(int i = 0; i < 9; ++i)
	{
		colo += sampleTex[i] * kernel[i];
	}
	colo = texture(screenTexture, TexCoords).rgb;
	FragColor = vec4(colo, 1.0);
}