#version 330 core

in vec3 Normal;

out vec4 FragColor;

uniform vec3 materialColor;

void main()
{
    vec3 normal = normalize(Normal);
    vec3 lightDir = normalize(vec3(0.5, 1.0, 0.3));

    float diffuse = max(dot(normal, lightDir), 0.0);

    vec3 ambient = materialColor * 0.2;
    vec3 lighting = ambient + materialColor * diffuse;

    FragColor = vec4(lighting, 1.0);
}