#version 330 core

in vec3 FragPos;
in vec3 Normal;
in vec2 TexCoords;

out vec4 FragColor;

//Phongģ�Ͳ���
struct Material{
	sampler2D diffuse;
	sampler2D specular;
	float shininess;
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

struct PointLight
{
	vec3 position;			//

	vec3 ambient;
	vec3 diffuse;
	vec3 specular; 

	//˥����
	float constant;
	float linear;
	float quadratic;
};
#define NR_POINT_LIGHTS 4
uniform PointLight pointLights[NR_POINT_LIGHTS];


struct SpotLight
{
	vec3 direction;
	vec3 position;			//
	float cutOff;			//��Բ׶�й��
	float outterCutOff;		//��Բ׶�й��

	vec3 ambient;
	vec3 diffuse;
	vec3 specular; 

	//˥����
	float constant;
	float linear;
	float quadratic;
};
uniform SpotLight spotLight;

vec3 CalcDirLight(DirLight light, vec3 normal, vec3 viewDir)
{
	vec3 lightDir = normalize(-light.direction);

	//������
	float diff = max(dot(lightDir, normal),1.0f);

	//�߹�
	vec3 reflectDir = reflect(-lightDir, normal);
	float spec = pow(max(dot(viewDir, reflectDir),0.0f),material.shininess);

	//�ϲ����
	vec3 ambient = light.ambient * vec3(texture(material.diffuse, TexCoords));
	vec3 diffuse = diff * light.diffuse * vec3(texture(material.diffuse, TexCoords));
	vec3 specular = spec * light.specular * vec3(texture(material.specular, TexCoords));

	return (ambient + diffuse + specular);
}

vec3 CalcPointLight(PointLight light, vec3 normal, vec3 fragPos, vec3 viewDir)
{
	vec3 lightDir = normalize(light.position - fragPos);

	//������
	float diff = max(dot(lightDir, normal),1.0f);

	//�߹�
	vec3 reflectDir = reflect(-lightDir, normal);
	float spec = pow(max(dot(viewDir, reflectDir),0.0f),material.shininess);

	float distance = length(light.position - fragPos);
	float attenuation = 1.0f / (light.constant + light.linear * distance + light.quadratic * distance * distance);
	
	//�ϲ����
	vec3 ambient = light.ambient * vec3(texture(material.diffuse, TexCoords));
	vec3 diffuse = diff * light.diffuse * vec3(texture(material.diffuse, TexCoords));
	vec3 specular = spec * light.specular * vec3(texture(material.specular, TexCoords));
	ambient *= attenuation;
	diffuse *= attenuation;
	specular *= attenuation;

	return (ambient + diffuse + specular);
}

vec3 CalcSpotLight(SpotLight light, vec3 normal, vec3 fragPos, vec3 viewDir)
{
	vec3 lightDir = normalize(light.position - fragPos);

	//������
	float diff = max(dot(lightDir, normal),1.0f);

	//�߹�
	vec3 reflectDir = reflect(-lightDir, normal);
	float spec = pow(max(dot(viewDir, reflectDir),0.0f),material.shininess);

	float distance = length(light.position - fragPos);
	float attenuation = 1.0f / (light.constant + light.linear * distance + light.quadratic * distance * distance);
	
	//�۹��Ч��
	float theta = dot(normalize(light.direction), -lightDir);
	float epsilon = light.cutOff - light.outterCutOff;
	float intensity = clamp((theta - light.outterCutOff) / epsilon, 0.0, 1.0);

	//�ϲ����
	vec3 ambient = light.ambient * vec3(texture(material.diffuse, TexCoords));
	vec3 diffuse = diff * light.diffuse * vec3(texture(material.diffuse, TexCoords));
	vec3 specular = spec * light.specular * vec3(texture(material.specular, TexCoords));
	ambient *= attenuation;
	diffuse = diffuse * attenuation * intensity;
	specular = specular * attenuation * intensity;

	return (ambient + diffuse + specular);
}

void main()
{
	vec3 norm = normalize(Normal);
	vec3 viewDir = normalize(-FragPos);

	vec3 result = CalcDirLight(dirLight, norm, viewDir);

	for(int i = 0; i < NR_POINT_LIGHTS; ++i)
	{
		result += CalcPointLight(pointLights[i], norm, FragPos, viewDir);
	}

	//result += CalcSpotLight(spotLight, norm, FragPos, viewDir);

	FragColor = vec4(result, 1.0);
}