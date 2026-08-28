using CBRE.Localization;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CBRE.Graphics.Shaders
{
    public class ShaderProgram : IDisposable
    {
        public static int CurrentProgram { get; private set; }

        public int ID { get; private set; }
        public List<Variable> Variables { get; private set; }
        public List<Shader> Shaders { get; private set; }

        public ShaderProgram(params Shader[] shaders)
        {
            Variables = new List<Variable>();
            Shaders = shaders.ToList();
            ID = CreateProgram(Shaders);
            int c;
            GL.GetProgram(ID, GetProgramParameterName.ActiveUniforms, out c);
            for (int i = 0; i < c; i++)
            {
                int size;
                ActiveUniformType type;
                string name = GL.GetActiveUniform(ID, i, out size, out type);
                int loc = GL.GetUniformLocation(ID, name);
                Variable v = new Variable(loc, name, type);
                Variables.Add(v);
            }
        }

        public void Dispose()
        {
            Shaders.ForEach(x => GL.DetachShader(ID, x.ID));
            Shaders.ForEach(x => x.Dispose());
            GL.DeleteProgram(ID);
        }

        static int CreateProgram(List<Shader> shaders)
        {
            int program = GL.CreateProgram();
            shaders.ForEach(x => GL.AttachShader(program, x.ID));
            shaders.ForEach(x => x.BindAttribLocations(program));
            GL.LinkProgram(program);

            // Program error logging
            int status;
            GL.GetProgram(program, GetProgramParameterName.LinkStatus, out status);
            if (status == 0)
            {
                string err = GL.GetProgramInfoLog(program);
                throw new Exception(Local.LocalString("exception.error_linking_program", err));
            }

            return program;
        }

        private Variable GetVariable(string name)
        {
            return Variables.FirstOrDefault(x => x.Name == name);
        }

        public void Set(string name, Matrix4 matrix)
        {
            GetVariable(name).Set(matrix);
        }

        public void Set(string name, float f)
        {
            GetVariable(name).Set(f);
        }

        public void Set(string name, int i)
        {
            GetVariable(name).Set(i);
        }

        public void Set(string name, bool b)
        {
            GetVariable(name).Set(b);
        }

        public void Bind()
        {
            GL.UseProgram(ID);
        }

        public void Unbind()
        {
            GL.UseProgram(0);
        }

        public void Set(string name, Vector4 vec)
        {
            GetVariable(name).Set(vec);
        }
    }
}