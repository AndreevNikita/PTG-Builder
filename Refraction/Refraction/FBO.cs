using OpenTK;
using OpenTK.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Refraction
{
    class FBO {
        public int REFLECTION_WIDTH = 320;
        public int REFLECTION_HEIGHT = 180;

        public int REFRACTION_WIDTH = 1280;
        public int REFRACTION_HEIGHT = 720;

        public int reflectionFrameBuffer;
        public int reflectionTexture;
        public int reflectionDepthBuffer;

        public int refractionFrameBuffer;
        public int refractionTexture;
        public int refractionDepthTexture;

        public FBO(GLControl control) {//call when loading the game
            REFRACTION_WIDTH = control.Width;
            REFRACTION_HEIGHT = control.Height;
            REFLECTION_WIDTH = control.Width;
            REFLECTION_HEIGHT = control.Height;
            initialiseReflectionFrameBuffer(control);
            initialiseRefractionFrameBuffer(control);
        } 

        public void cleanUp() {//call when closing the game
            GL.DeleteFramebuffers(1, ref reflectionFrameBuffer);
            GL.DeleteTextures(1, ref reflectionTexture);
            GL.DeleteRenderbuffers(1, ref reflectionDepthBuffer);
            GL.DeleteFramebuffers(1, ref refractionFrameBuffer);
            GL.DeleteTextures(1, ref refractionTexture);
            GL.DeleteTextures(1, ref refractionDepthTexture);
        }

        public void bindReflectionFrameBuffer()
        {//call before rendering to this FBO
            bindFrameBuffer(reflectionFrameBuffer, REFLECTION_WIDTH, REFLECTION_HEIGHT);
        }

        public void bindRefractionFrameBuffer()
        {//call before rendering to this FBO
            bindFrameBuffer(refractionFrameBuffer, REFRACTION_WIDTH, REFRACTION_HEIGHT);
        }

        public void unbindCurrentFrameBuffer(GLControl glControl) {//call to switch to default frame buffer
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, 0);

            GL.Viewport(0, 0, glControl.Width, glControl.Height); //Устанавливаем новые координаты, где нужно отрисовывать картинку
            //Матрица проекции тоже меняется
            Matrix4 p = Matrix4.CreatePerspectiveFieldOfView((float)(80 * Math.PI / 180), (float)glControl.Width / (float)glControl.Height, 0.1f, 6000);
            //Устанавливаем матрицу проекции, полученную выше
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadMatrix(ref p);
            GL.MatrixMode(MatrixMode.Modelview);
        }

        public int getReflectionTexture() {//get the resulting texture
            return reflectionTexture;
        }

        public int getRefractionTexture() {//get the resulting texture
            return refractionTexture;
        }

        public int getRefractionDepthTexture() {//get the resulting depth texture
            return refractionDepthTexture;
        }

        private void initialiseReflectionFrameBuffer(GLControl control) {
            reflectionFrameBuffer = createFrameBuffer();
            reflectionTexture = createTextureAttachment(REFLECTION_WIDTH, REFLECTION_HEIGHT);
            reflectionDepthBuffer = createDepthBufferAttachment(REFLECTION_WIDTH, REFLECTION_HEIGHT);
            unbindCurrentFrameBuffer(control);
        }

        private void initialiseRefractionFrameBuffer(GLControl control) {
            refractionFrameBuffer = createFrameBuffer();
            refractionTexture = createTextureAttachment(REFRACTION_WIDTH, REFRACTION_HEIGHT);
            refractionDepthTexture = createDepthTextureAttachment(REFRACTION_WIDTH, REFRACTION_HEIGHT);
            unbindCurrentFrameBuffer(control);
        }

        private void bindFrameBuffer(int frameBuffer, int width, int height) {
            GL.BindTexture(TextureTarget.Texture2D, 0);//To make sure the texture isn't bound
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, frameBuffer);
            GL.Viewport(0, 0, width, height);
            Matrix4 p = Matrix4.CreatePerspectiveFieldOfView((float)(80 * Math.PI / 180), (float)width / (float)height, 0.1f, 6000);
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadMatrix(ref p);
            GL.MatrixMode(MatrixMode.Modelview);
        }

        private int createFrameBuffer()
        {
            int frameBuffer;
            GL.GenFramebuffers(1, out frameBuffer);
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, frameBuffer);
            GL.DrawBuffer(DrawBufferMode.ColorAttachment0);
            return frameBuffer;
        }

        private int createTextureAttachment(int width, int height)
        {
            int texture = GL.GenTexture();
            GL.BindTexture(TextureTarget.Texture2D, texture);
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgb, width, height,
                0, PixelFormat.Rgb, PixelType.UnsignedByte, IntPtr.Zero);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.ClampToEdge);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.ClampToEdge);
            GL.FramebufferTexture2D(FramebufferTarget.Framebuffer, FramebufferAttachment.ColorAttachment0, TextureTarget.Texture2D, texture, 0);
            return texture;
        }

        private int createDepthTextureAttachment(int width, int height)
        {

            int texture = GL.GenTexture();
            GL.BindTexture(TextureTarget.Texture2D, texture); 
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.DepthComponent32, width, height,
                0, PixelFormat.DepthComponent, PixelType.Float, IntPtr.Zero);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.ClampToEdge);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.ClampToEdge);
            GL.FramebufferTexture2D(FramebufferTarget.Framebuffer, FramebufferAttachment.DepthAttachment, TextureTarget.Texture2D, texture, 0);
            return texture;
        }

        private int createDepthBufferAttachment(int width, int height)
        {
            int depthBuffer;
            GL.GenRenderbuffers(1, out depthBuffer);
            GL.BindRenderbuffer(RenderbufferTarget.Renderbuffer, depthBuffer);
            GL.RenderbufferStorage(RenderbufferTarget.Renderbuffer, RenderbufferStorage.DepthComponent32, width,
                    height);
            GL.FramebufferRenderbuffer(FramebufferTarget.Framebuffer, FramebufferAttachment.DepthAttachment,
                    RenderbufferTarget.Renderbuffer, depthBuffer);
            return depthBuffer;
        }
    }
}
