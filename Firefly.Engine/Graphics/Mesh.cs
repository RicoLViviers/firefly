using OpenTK.Graphics.OpenGL4;
using System;
using System.Collections.Generic;
using System.Text;

namespace Firefly.Engine.Graphics
{
    public class Mesh
    {
        public int VBO, VAO, EBO;
        public int VertexCount;

        public Mesh(float[] vertices)
        {
            VertexCount = vertices.Length / 6;

            VBO = GL.GenBuffer();
            VAO = GL.GenVertexArray();

            GL.BindBuffer(BufferTarget.ArrayBuffer, VBO);
            GL.BindVertexArray(VAO);

            GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float), vertices, BufferUsageHint.StaticDraw);

            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 6 * sizeof(float), 0);
            GL.EnableVertexAttribArray(0);

            GL.VertexAttribPointer(1, 3, VertexAttribPointerType.Float, false, 6 * sizeof(float), 3 * sizeof(float));
            GL.EnableVertexAttribArray(1);
        }

        public Mesh(float[] vertices, int[] indices)
        {
            VertexCount = vertices.Length / 6;


            VBO = GL.GenBuffer();
            VAO = GL.GenVertexArray();
            EBO = GL.GenBuffer();

            GL.BindVertexArray(VAO);
            GL.BindBuffer(BufferTarget.ArrayBuffer, VBO);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, EBO);

            GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float), vertices, BufferUsageHint.StaticDraw);
            GL.BufferData(BufferTarget.ElementArrayBuffer, indices.Length * sizeof(int), indices, BufferUsageHint.StaticDraw);

            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 6 * sizeof(float), 0);
            GL.EnableVertexAttribArray(0);

            GL.VertexAttribPointer(1, 3, VertexAttribPointerType.Float, false, 6 * sizeof(float), 3 * sizeof(float));
            GL.EnableVertexAttribArray(1);
        }

        public void Bind()
        {
            GL.BindVertexArray(VAO);
        }
        ~Mesh()
        {

            GL.DeleteBuffer(VBO);
            GL.DeleteVertexArray(VAO);
            GL.DeleteBuffer(EBO);
        }
    }

    
}
