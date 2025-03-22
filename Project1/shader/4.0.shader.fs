#version 330 core

in vec3 FragPos;
in vec3 Normal;
in vec3 LightPos;

out vec4 fragColor;

uniform vec3 objectColor;
uniform vec3 lightColor;

void main()
{
	//环境光
	float ambientStrength = 0.1f;
	vec3 ambient = ambientStrength * lightColor;

	//漫反射光
	vec3 norm = normalize(Normal);
	vec3 lightDir = normalize(LightPos - FragPos);
	float diff = max(dot(norm, lightDir), 0.0f);
	vec3 diffuse = lightColor * diff;

	//高光
	float specularStrength = 0.5f;
	vec3 viewDir = normalize(- FragPos);
	vec3 reflectDir = reflect(-lightDir, Normal);
	float spec = pow(max(dot(viewDir, reflectDir),0.0f),32);
	vec3 specular = specularStrength * spec * lightColor;

	vec3 res = (ambient + diffuse + specular) * objectColor;
	fragColor = vec4(res, 1.0f);
}