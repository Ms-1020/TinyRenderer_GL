#pragma once

#include <glad/glad.h>
#include <glm/glm.hpp>
#include <glm/gtc/matrix_transform.hpp>

enum Camera_Move
{
	FORWARD,
	BACKWARD,
	LEFT,
	RIGHT
};

const float YAW = -90.0f;
const float PITCH = 0.0f;
const float SPEED = 2.5f;
const float SENSITIVITY = 0.1f;
const float FOV = 45.0f;


class Camera
{
public:
	glm::vec3 mPosition;
	glm::vec3 mFront;
	glm::vec3 mUp;
	glm::vec3 mRight;
	glm::vec3 mWorldUp;

	float mPitch;
	float mYaw;

	float mFov;
	float mMoveSpeed;
	float mMoveSensitivity;
	Camera(glm::vec3 position = glm::vec3(0.0f, 0.0f, 0.0f),
		glm::vec3 up = glm::vec3(0.0f, 1.0f, 0.0f),
		float yaw = YAW, float pitch = PITCH) :
		mPosition(position), mWorldUp(up), mYaw(yaw), mPitch(pitch),
		mFront(glm::vec3(0.0f, 0.0f, -1.0f)), mMoveSpeed(SPEED),
		mMoveSensitivity(SENSITIVITY), mFov(FOV)
	{
		UpdateCameraVectors();
	}

	Camera(float xPos, float yPos,float zPos, float xUp, float yUp, float zUp, float pitch, float yaw) :
		mFront(glm::vec3(0.0f,0.0f,-1.0f)), mMoveSpeed(SPEED),mMoveSensitivity(SENSITIVITY), mFov(FOV),
		mPitch(pitch),mYaw(yaw)
	{
		mPosition = glm::vec3(xPos, yPos, zPos);
		mWorldUp = glm::vec3(xUp, yUp, zUp);
		UpdateCameraVectors();
	}

	glm::mat4 GetViewMatrix() const
	{
		return LookAt(mPosition, mPosition + mFront, mUp);
		//return glm::lookAt(mPosition, mPosition + mFront, mUp);
	}

	glm::mat4 LookAt(glm::vec3 eye, glm::vec3 center, glm::vec3 up)const
	{
		glm::mat4 res = glm::mat4(1.0f);

		glm::vec3 zAxis = glm::normalize(eye - center);
		glm::vec3 xAxis = glm::normalize(glm::cross(glm::normalize(up), zAxis));
		glm::vec3 yAxis = glm::cross(zAxis, xAxis);
		res[0][0] = xAxis.x;
		res[1][0] = xAxis.y;
		res[2][0] = xAxis.z;
		res[0][1] = yAxis.x;
		res[1][1] = yAxis.y;
		res[2][1] = yAxis.z;
		res[0][2] = zAxis.x;
		res[1][2] = zAxis.y;
		res[2][2] = zAxis.z;

		glm::mat4 trans = glm::mat4(1.0f);
		trans[3][0] = -eye.x;
		trans[3][1] = -eye.y;
		trans[3][2] = -eye.z;

		return res * trans;
	}

	void ProcessKeyBoard(Camera_Move direction, float deltaTime)
	{
		float velocity = mMoveSpeed * deltaTime;
		if (direction == FORWARD)
		{
			mPosition += velocity * mFront;
		}
		else if (direction == BACKWARD)
		{
			mPosition -= velocity * mFront;
		}
		else if (direction == LEFT)
		{
			mPosition -= velocity * mRight;
		}
		else if (direction == RIGHT)
		{
			mPosition += velocity * mRight;
		}
		mPosition.y = 0.0f;
	}

	void ProcessMouseMovement(float xOffset, float yOffset, GLboolean constrainPitch = true)
	{
		xOffset *= mMoveSensitivity;
		yOffset *= mMoveSensitivity;

		mPitch += yOffset;
		mYaw += xOffset;
		if (constrainPitch)
		{
			if (mPitch > 89.0f)
				mPitch = 89.0f;
			else if (mPitch < -89.0f)
				mPitch = -89.0f;
		}
		UpdateCameraVectors();
	}

	void ProcessScroll(float xOffset, float yOffset)
	{
		mFov -= yOffset;
		if (mFov < 1.0f)
			mFov = 1.0f;
		else if (mFov > 45.0f)
			mFov = 45.0f;
	}
private:
	void UpdateCameraVectors()
	{
		glm::vec3 front;
		front.x = cos(glm::radians(mPitch)) * cos(glm::radians(mYaw));
		front.y = sin(glm::radians(mPitch));
		front.z = cos(glm::radians(mPitch)) * sin(glm::radians(mYaw));
		mFront = glm::normalize(front);

		mRight = glm::normalize(glm::cross(mFront, mWorldUp));
		mUp = glm::normalize(glm::cross(mRight, mFront));
	}
};