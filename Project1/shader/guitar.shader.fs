#version 330 core
out vec4 FragColor;

in vec3 FragPos;
in vec3 Normal;
in vec2 TexCoords;

struct PointLight
{
	vec3 position;

	vec3 ambient;
	vec3 diffuse;
	vec3 specular;

	float constant;
	float linear;
	float quadratic;
};
#define NR_POINT_LIGHTS 2
uniform PointLight pointLights[NR_POINT_LIGHTS];

struct DirLight
{
	vec3 direction;

	vec3 ambient;
	vec3 diffuse;
	vec3 specular;
};
uniform DirLight dirLight;

struct Material
{
	sampler2D texture_diffuse1;
	sampler2D texture_sepcular1;
};

uniform Material material;

vec3 CalcDirLight(DirLight light, vec3 viewDir, vec3 normal)
{
	vec3 lightDir = normalize(-light.direction);
	//漫反射
	float diff = max(dot(lightDir, normal),1.0f);

	//高光
	vec3 reflectDir = reflect(-lightDir, normal);
	float spec = pow(max(dot(viewDir, reflectDir),0.0f),32.0);

	//合并结果
	vec3 ambient = dirLight.ambient * vec3(texture(material.texture_diffuse1, TexCoords));
	vec3 diffuse = diff * dirLight.diffuse * vec3(texture(material.texture_diffuse1, TexCoords));
	vec3 specular = spec * dirLight.specular * vec3(texture(material.texture_sepcular1, TexCoords));

	return ambient + diffuse + specular;
}

vec3 CalcPointLight(PointLight light, vec3 viewDir, vec3 normal, vec3 fragPos)
{
	vec3 lightDir = normalize(light.position - fragPos);

	//漫反射
	float diff = max(dot(lightDir, normal),1.0f);

	//高光
	vec3 reflectDir = reflect(-lightDir, normal);
	float spec = pow(max(dot(viewDir, reflectDir),0.0f),32.0);

	//合并结果
	vec3 ambient = dirLight.ambient * vec3(texture(material.texture_diffuse1, TexCoords));
	vec3 diffuse = diff * dirLight.diffuse * vec3(texture(material.texture_diffuse1, TexCoords));
	vec3 specular = spec * dirLight.specular * vec3(texture(material.texture_sepcular1, TexCoords));

	//衰减
	float distance = length(light.position - fragPos);
	float attenuation = 1.0 / (light.constant + light.linear * distance + light.quadratic * distance * distance);

	ambient *= attenuation;
	diffuse *= attenuation;
	specular *= attenuation;

	return ambient + diffuse + specular;
}

void main()
{
	vec3 viewDir = normalize(-FragPos);

	vec3 res = CalcDirLight(dirLight, viewDir, Normal);

	for(int i = 0; i < NR_POINT_LIGHTS; ++i)
		res += CalcPointLight(pointLights[i], viewDir, Normal, FragPos);

	FragColor = vec4(res, 1.0);
}