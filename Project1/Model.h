#pragma once
#include <glad/glad.h> 

#include <glm/glm.hpp>
#include <glm/gtc/matrix_transform.hpp>
#include "stb_image.h"
#include <assimp/Importer.hpp>
#include <assimp/scene.h>
#include <assimp/postprocess.h>

#include "Mesh.h"


class Model
{
public:
	Model(std::string path)
	{
		LoadModel(path);
	}
	void Draw(const Shader& shader);
private:
	std::vector<Texture> mTexturesLoaded;
	std::vector<Mesh> mMeshes;
	std::string mDirectory;

	void LoadModel(std::string path);
	void ProcessNode(aiNode* node, const aiScene* scene);
	Mesh ProcessMesh(aiMesh* mesh, const aiScene* scene);
	std::vector<Texture> LoadMaterialTexture(aiMaterial* mat, aiTextureType type, std::string typeName);
	unsigned int LoadTexture(const char* path, const std::string& directory, bool gamma = false);
};

