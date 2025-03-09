#pragma once

#include <glad/glad.h>
#include <glm/glm.hpp>
#include <glm/gtc/type_ptr.hpp>
#include <string>
#include <iostream>
#include <fstream>
#include <sstream>

class Shader
{
public:
	enum class Type
	{
		PROGRAM,
		VERTEX,
		FRAGMENT,
	};

	unsigned int ID;	//程序ID

	//构造函数，读取着色器
	Shader(const char* vertexPath, const char* fragmentPath);
	~Shader()
	{
		glDeleteProgram(ID);
	}
	//
	void Use();

	void SetBool(const std::string& name, bool value) const;
	void SetInt(const std::string& name, int value) const;
	void SetFloat(const std::string& name, float value) const;
	void SetMat4(const std::string& name, glm::mat4 value) const;
	void SetVec3(const std::string& name, glm::vec3 value) const;
private:
	void CheckCompileError(Type type, unsigned int id);
};

