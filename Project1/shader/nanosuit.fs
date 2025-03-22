#version 330 core

in vec3 FragPos;
in vec3 Normal;
in vec2 TexCoords;

out vec4 FragColor;

struct Material
{
	sampler2D texture_reflect1;
	sampler2D texture_diffuse1;
	sampler2D texture_sepcular1;
};
uniform Material material;

struct DirLight
{
	vec3 direction;

	vec3 ambient;
	vec3 diffuse;
	vec3 specular;
};
uniform DirLight dirLight;

uniform samplerCube skyboxNano;

vec3 CalcDirLight(DirLight light, vec3 viewDir, vec3 normal)
{
	vec3 lightDir = normalize(-light.direction);
	//漫反射
	float diff = max(dot(lightDir, normal),0.0f);

	//高光
	vec3 reflectDir = reflect(-lightDir, normal);
	float spec = pow(max(dot(viewDir, reflectDir),0.0f),32.0);

	//合并结果
	vec3 ambient = light.ambient * vec3(texture(material.texture_diffuse1, TexCoords));
	vec3 diffuse = diff * light.diffuse * vec3(texture(material.texture_diffuse1, TexCoords));
	vec3 specular = spec * light.specular * vec3(texture(material.texture_sepcular1, TexCoords));

	return ambient + diffuse + specular;
}

void main()
{
	vec3 N = normalize(Normal);
	vec3 I = normalize(FragPos);
	vec3 R = reflect(I, N);
	vec3 reflectColor = texture(material.texture_reflect1, TexCoords).rgb;
	vec3 skyboxColor = texture(skyboxNano, R).rgb;
	FragColor = vec4(reflectColor * skyboxColor + CalcDirLight(dirLight,-I ,N),1.0);
}