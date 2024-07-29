uniform sampler2DRect tex; //0

uniform int radius;

void main(void)
{
    vec4 sum = vec4(0.0, 0.0, 0.0, 0.0);

    for (int i = -radius; i <= radius; i++)
        sum += pow(texture2DRect(tex, gl_TexCoord[0].xy + vec2(0, i)), vec4(2.0));
    sum /= (radius * 2 + 1);
    sum = sqrt(sum);

    gl_FragColor = sum;
}