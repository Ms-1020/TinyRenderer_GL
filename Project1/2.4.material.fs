#version 330 core
struct Light{
	vec3 position;
	vec3 ambient;
	vec3 diffuse;
	vec3 specular; 
};

//Phong模型材质
struct Material{
	sampler2D diffuse;
	sampler2D specular;
	sampler2D emission;
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

	vec3 emission = texture(material.emission, TexCoords).rgb;

	FragColor = vec4(ambient + diffuse + specular + emission,1.0);
}