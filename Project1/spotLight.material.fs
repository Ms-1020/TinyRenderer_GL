#version 330 core
struct Light{
	vec3 direction;		    //
	vec3 position;			//
	float cutOff;			//内圆锥切光角
	float outterCutOff;		//外圆锥切光角

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

	//聚光灯效果
	float theta = dot(normalize(light.direction), -lightDir);
	float epsilon = light.cutOff - light.outterCutOff;
	float intensity = clamp((theta - light.outterCutOff) / epsilon, 0.0, 1.0);
	diffuse *= intensity;
	specular *= intensity;

	//衰减
	float distance = length(light.position - FragPos);
	float attenuation = 1.0f / (light.constant + light.linear * distance + light.quadratic * distance * distance);
	ambient *= attenuation;
	diffuse *= attenuation;
	specular *= attenuation;

	FragColor = vec4(ambient + diffuse + specular,1.0);
}