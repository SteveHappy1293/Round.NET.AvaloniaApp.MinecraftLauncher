using Avalonia.OpenGL;
using Avalonia.OpenGL.Controls;
using Avalonia.Threading;
using MinecraftSkinRender;
using MinecraftSkinRender.OpenGL;
using MinecraftSkinRender.OpenGL.Silk;
using Silk.NET.OpenGL;
using SkiaSharp;

public class Renderer : OpenGlControlBase
{

    private GL _gl;
    private SkinRenderOpenGL _skin;
    private bool _initialized = false;

    protected override void OnOpenGlInit(GlInterface gl)
    {
        base.OnOpenGlInit(gl);

        _gl = GL.GetApi(gl.GetProcAddress);

        // 打印OpenGL信息
        Console.WriteLine($"OpenGL版本: {_gl.GetStringS(StringName.Version)}");
        Console.WriteLine($"渲染器: {_gl.GetStringS(StringName.Renderer)}");

        // 初始化OpenGL状态
        _gl.Enable(EnableCap.DepthTest);
        _gl.DepthFunc(DepthFunction.Less);
        _gl.Enable(EnableCap.CullFace);
        _gl.CullFace(TriangleFace.Back);
        _gl.ClearColor(0.2f, 0.3f, 0.3f, 1.0f);

        _skin = new(new SlikOpenglApi(_gl))
        {
            IsGLES = true,
            SkinType = SkinType.NewSlim,
            EnableTop = true,
            RenderType = SkinRenderType.Normal,
            Animation = true,
            EnableCape = true,
            BackColor = new(1, 1, 1, 1),
            Width = Math.Max(1, (int)Bounds.Width),
            Height = Math.Max(1, (int)Bounds.Height)
        };

        try
        {
            var img = SKBitmap.Decode("skin.png");
            if (img == null || img.Width == 0 || img.Height == 0)
                throw new Exception("无效的皮肤纹理");

            _skin.SetSkinTex(img);

            if (File.Exists("cape.png"))
            {
                var capeImg = SKBitmap.Decode("cape.png");
                if (capeImg != null)
                    _skin.SetCapeTex(capeImg);
            }

            _skin.FpsUpdate += (a, b) => Console.WriteLine($"FPS: {b}");
            _skin.OpenGlInit();

            // 移除不存在的SetViewport调用
            // 改为在渲染时设置视口

            _initialized = true;
            RequestNextFrameRendering();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"初始化失败: {ex}");
            _initialized = false;
        }
    }

    protected override void OnOpenGlRender(GlInterface gl, int fb)
    {
        if (!_initialized || Bounds.Width <= 0 || Bounds.Height <= 0)
            return;

        try
        {
            // 1. 重置所有绑定状态
            _gl.BindFramebuffer(FramebufferTarget.Framebuffer, (uint)fb);
            _gl.BindTexture(TextureTarget.Texture2D, 0);

            // 2. 设置视口和清除状态
            _gl.Viewport(0, 0, (uint)Bounds.Width, (uint)Bounds.Height);
            _gl.ClearColor(0.2f, 0.3f, 0.3f, 1.0f);
            _gl.Clear(ClearBufferMask.ColorBufferBit |
                      ClearBufferMask.DepthBufferBit);

            // 3. 验证帧缓冲状态
            var status = _gl.CheckFramebufferStatus(FramebufferTarget.Framebuffer);
            if (status != GLEnum.FramebufferComplete)
            {
                Console.WriteLine($"帧缓冲不完整: {status}");
                return;
            }

            // 4. 更新渲染器状态
            _skin.Width = Math.Max(1, (int)Bounds.Width);
            _skin.Height = Math.Max(1, (int)Bounds.Height);

            // 5. 执行渲染（添加安全检查）
            if (_skin != null)
            {
                _skin.Rot(0, 0.5f);
                _skin.Tick(0.016f);
                _skin.OpenGlRender(0);
            }

            // 6. 强制刷新并检查错误
            _gl.Flush();
            var error = _gl.GetError();
            if (error != GLEnum.NoError)
                Console.WriteLine($"OpenGL错误: {error}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"渲染异常: {ex.Message}");
            _initialized = false;
        }
        finally
        {
            Dispatcher.UIThread.Post(RequestNextFrameRendering, DispatcherPriority.Render);
        }
    }
}