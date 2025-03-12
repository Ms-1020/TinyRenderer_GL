#pragma once

#include <glm/glm.hpp>
#include <glm/matrix.hpp>
#include <string>
#include <vector>

#include "Shader.h"

struct Vertex
{
	glm::vec3 Position;
	glm::vec3 Normal;
	glm::vec2 TexCoords;
};

struct Texture
{
	unsigned int id;
	std::string type;
	aiString path;
};

class Mesh
{
public:
	/*Íø¸ñÊý¾Ý*/
	std::vector<Vertex> mVertices;
	std::vector<unsigned int> mIndices;
	std::vector<Texture> mTextures;
	unsigned int mVAO;

	Mesh(std::vector<Vertex> vertices, std::vector<unsigned int> indices, std::vector<Texture> textures);
	void Draw(const Shader& shader);

private:
	unsigned int mVBO, mEBO;

	void SetUpMesh();
};

