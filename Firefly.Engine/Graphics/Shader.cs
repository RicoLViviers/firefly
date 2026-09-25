using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace Firefly.Engine.Graphics
{
    public class Shader
    {
        private int Handle;

        public Shader(string vertexPath, string fragmentPath)
        {
            string VertexShaderSource = File.ReadAllText(vertexPath);
            string FragmentShaderSource = File.ReadAllText(fragmentPath);

            int VertexShader, FragmentShader;

            VertexShader = GL.CreateShader(ShaderType.VertexShader);
            GL.ShaderSource(VertexShader, VertexShaderSource);


            FragmentShader = GL.CreateShader(ShaderType.FragmentShader);
            GL.ShaderSource(FragmentShader, FragmentShaderSource);

            GL.CompileShader(VertexShader);

            GL.GetShader(VertexShader, ShaderParameter.CompileStatus, out int success);
            if (success == 0)
            {
                string infoLog = GL.GetShaderInfoLog(VertexShader);
                Console.WriteLine(infoLog);
            }

            GL.CompileShader(FragmentShader);

            GL.GetShader(FragmentShader, ShaderParameter.CompileStatus, out success);
            if(success==0)
            {
                string infoLog = GL.GetShaderInfoLog(FragmentShader);
                Console.WriteLine(infoLog);
            }

            Handle = GL.CreateProgram();

            GL.AttachShader(Handle, VertexShader);
            GL.AttachShader(Handle, FragmentShader);

            GL.LinkProgram(Handle);

            GL.GetProgram(Handle, GetProgramParameterName.LinkStatus, out success);
            if (success==0)
            {
                string infoLog = GL.GetProgramInfoLog(Handle);
                Console.WriteLine(infoLog);
            }


            GL.DetachShader(Handle, VertexShader);
            GL.DetachShader(Handle, FragmentShader);
            GL.DeleteShader(FragmentShader);
            GL.DeleteShader(VertexShader);
        }

        public void Use()
        {
            GL.UseProgram(Handle);
        }

        public int GetAttribLocation(string name)
        {
            return GL.GetAttribLocation(Handle, name);
        }

        public int GetUniformLocation(string name)
        {
            return GL.GetUniformLocation(Handle, name);
        }

        public void SetInt(string name, int value)
        {
            int location = GL.GetUniformLocation(Handle, name);

            GL.Uniform1(location, value);
        }

        public void SetMatrix4(string name, Matrix4 matrix)
        {
            int location = GL.GetUniformLocation(Handle, name);
            GL.UniformMatrix4(location, true, ref matrix);
        }

        public void SetFloat(string name, float value)
        {
            int location = GL.GetUniformLocation(Handle, name);

            GL.Uniform1(location, value);
        }

        public void SetVec3(string name, Vector3 value)
        {
            int location = GL.GetUniformLocation(Handle, name);

            GL.Uniform3(location, value);
        }


        private bool disposedValue = false;
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                GL.DeleteProgram(Handle);

                disposedValue = true;
            }
        }

        ~Shader()
        {
            if (disposedValue==false)
            {
                Console.WriteLine("GPU Resource leak! Did you forget to call Dispose()?");
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public void PrintInfo()
        {
            GL.GetProgram(Handle, GetProgramParameterName.LinkStatus, out int linked);
            Console.WriteLine($"Link status: {linked}");

            GL.GetProgramInfoLog(Handle, out string log);
            Console.WriteLine($"Program log: {log}");

            Console.WriteLine($"GL_VERSION:  {GL.GetString(StringName.Version)}");
            Console.WriteLine($"GL_RENDERER: {GL.GetString(StringName.Renderer)}");
            Console.WriteLine($"GL_VENDOR:   {GL.GetString(StringName.Vendor)}");
            Console.WriteLine($"GLSL:        {GL.GetString(StringName.ShadingLanguageVersion)}");
        }
    }
}
