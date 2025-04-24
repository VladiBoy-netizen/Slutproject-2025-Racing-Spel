// simple_shader.fs
#version 330
in vec2 fragTexCoord;
in vec3 fragNormal;

uniform sampler2D texture0;

out vec4 finalColor;

void main()
{
    finalColor = texture(texture0, fragTexCoord);
}
