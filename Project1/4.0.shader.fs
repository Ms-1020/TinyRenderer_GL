#version 330 core

out vec4 fragColor;

uniform vec3 objectColor;
uniform vec3 lightColor;

void main()
{
	float ambientStrength = 0.1f;
	vec3 ambient = ambientStrength * lightColor;

	fragColor = vec4(objectColor * ambient, 1.0f);
}