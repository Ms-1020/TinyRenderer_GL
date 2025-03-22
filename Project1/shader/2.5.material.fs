#version 330 core
struct Light{
	//vec3 direction;		//平行光
	vec3 position;			//点光源

	vec3 ambient;
	vec3 diffuse;
	vec3 specular; 

	//衰减项
	float constant;
	float linear;
	float quadratic;
};

//Phong模型材质
struct Material{
	sampler2D diffuse;
	sampler2D specular;
	float shininess;
};

in vec3 FragPos;
in vec3 Normal;
in vec2 TexCoords;

out vec4 FragColor;
uniform Material material;
uniform Light light;

void main()
{
	float distance = length(light.position - FragPos);
	float attenuation = 1.0f / (light.constant + light.linear * distance + light.quadratic * distance * distance);

	//环境光
	vec3 ambient = light.ambient * vec3(texture(material.diffuse,TexCoords));

	//漫反射
	vec3 norm = normalize(Normal);
	vec3 lightDir = normalize(light.position - FragPos);
	float diff = max(dot(lightDir, norm), 0.0f);
	vec3 diffuse = light.diffuse * diff * vec3(texture(material.diffuse,TexCoords));

	//高光
	vec3 viewDir = normalize(- FragPos);
	vec3 reflectDir = reflect(-lightDir, norm);
	float spec = pow(max(dot(viewDir, reflectDir),0.0f),material.shininess);
	vec3 specular = light.specular * spec * vec3(texture(material.specular, TexCoords));

	ambient *= attenuation;
	diffuse *= attenuation;
	specular *= attenuation;

	FragColor = vec4(ambient + diffuse + specular,1.0);
}