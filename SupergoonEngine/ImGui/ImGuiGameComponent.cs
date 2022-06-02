﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SupergoonDashCrossPlatform.SupergoonEngine.Core;
using SupergoonDashCrossPlatform.SupergoonEngine.Interfaces;
using Num = System.Numerics;

namespace ImGuiNET.SampleProgram.XNA;

public class ImGuiGameComponent : GameComponent, IDraw
{
    //ImGUI
    private RenderTarget2D _imguiRenderTarget;
    private Texture2D _xnaTexture;
    private IntPtr _imGuiTexture;
    private ImGuiRenderer _imGuiRenderer;
    private GraphicsDevice _graphicsDevice;
    private List<Vector2ImguiDisplay> Vector2Watches = new();


    //ImGui Layout stuff from demo
    // Direct port of the example at https://github.com/ocornut/imgui/blob/master/examples/sdl_opengl2_example/main.cpp
    private float f = 0.0f;

    private bool show_test_window = false;
    private bool show_another_window = false;
    private Num.Vector3 clear_color = new Num.Vector3(114f / 255f, 144f / 255f, 154f / 255f);
    private byte[] _textBuffer = new byte[100];
    
    public ImGuiGameComponent(Game game, GraphicsDevice graphicsDevice) : base(game)
    {
        _graphicsDevice = graphicsDevice;
    }

    public override void Initialize()
    {
        //ImGui
        _imguiRenderTarget = new RenderTarget2D(_graphicsDevice, 1920, 1080);
        _imGuiRenderer = new ImGuiRenderer(Game);
        _imGuiRenderer.RebuildFontAtlas();
        // First, load the texture as a Texture2D (can also be done using the XNA/FNA content pipeline)
        _xnaTexture = CreateTexture(_graphicsDevice, 300, 150, pixel =>
        {
            var red = (pixel % 300) / 2;
            return new Color(red, 1, 1);
        });

        // Then, bind it to an ImGui-friendly pointer, that we can use during regular ImGui.** calls (see below)
        _imGuiTexture = _imGuiRenderer.BindTexture(_xnaTexture);
        base.Initialize();
    }

    public override void Update(GameTime gameTime)
    {
        //ImGui
        _graphicsDevice.SetRenderTarget(_imguiRenderTarget);
        _graphicsDevice.Clear(Color.Transparent);
        
        // Call BeforeLayout first to set things up
        _imGuiRenderer.BeforeLayout(gameTime);
        // Draw our UI
        ImGuiLayout();
        // Call AfterLayout now to finish up and draw all the things
        _imGuiRenderer.AfterLayout();
        _graphicsDevice.SetRenderTarget(null);
        //EndImgui
        base.Update(gameTime);
    }

    public void DrawToImGuiRenderTarget(GameTime gameTime)
    {
    }
    
    
    public static Texture2D CreateTexture(GraphicsDevice device, int width, int height, Func<int, Color> paint)
    {
        //initialize a texture
        var texture = new Texture2D(device, width, height);

        //the array holds the color for each pixel in the texture
        Color[] data = new Color[width * height];
        for (var pixel = 0; pixel < data.Length; pixel++)
        {
            //the function applies the color according to the specified pixel
            data[pixel] = paint(pixel);
        }

        //set the color
        texture.SetData(data);

        return texture;
    }
    
    protected virtual void ImGuiLayout()
    {
        // 1. Show a simple window
        // Tip: if we don't call ImGui.Begin()/ImGui.End() the widgets appears in a window automatically called "Debug"
        {
            ImGui.Text("Hello, world!");
            DrawAllWatchedVector2();
            // ImGui.SliderFloat("float", ref f, 0.0f, 1.0f, string.Empty);
            // ImGui.ColorEdit3("clear color", ref clear_color);
            if (ImGui.Button("Test Window")) show_test_window = !show_test_window;
            // if (ImGui.Button("Another Window")) show_another_window = !show_another_window;
            ImGui.Text(string.Format("Application average {0:F3} ms/frame ({1:F1} FPS)",
                1000f / ImGui.GetIO().Framerate, ImGui.GetIO().Framerate));

            // ImGui.InputText("Text input", _textBuffer, 100);
            //
            // ImGui.Text("Texture sample");
            // ImGui.Image(_imGuiTexture, new Num.Vector2(300, 150), Num.Vector2.Zero, Num.Vector2.One, Num.Vector4.One,
            //     Num.Vector4.One); // Here, the previously loaded texture is used
        }

        // 2. Show another simple window, this time using an explicit Begin/End pair
        // if (show_another_window)
        // {
        //     ImGui.SetNextWindowSize(new Num.Vector2(200, 100), ImGuiCond.FirstUseEver);
        //     ImGui.Begin("Another Window", ref show_another_window);
        //     ImGui.Text("Hello");
        //     ImGui.End();
        // }

        // 3. Show the ImGui test window. Most of the sample code is in ImGui.ShowTestWindow()
        if (show_test_window)
        {
            ImGui.SetNextWindowPos(new Num.Vector2(650, 20), ImGuiCond.FirstUseEver);
            ImGui.ShowDemoWindow(ref show_test_window);
        }
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(_imguiRenderTarget, Vector2.Zero, new Rectangle(0, 0, 1920, 1080), Color.White);
    }

    private void DrawAllWatchedVector2()
    {
        Vector2Watches.ForEach(attribute =>
        {
            ImGui.Text(attribute.Name);
            ImGui.Text($"X: {attribute.GetValue.X} Y: {attribute.GetValue.Y}");
            
        });
    }

    public void CheckComponentForDebugAttributes(Component component)
    {
        var type = component.GetType();
        var fields = type.GetFields();
        
        foreach (var field in fields)
        {
            var customAttributes = field.GetCustomAttributes();
            foreach (var customAttribute in customAttributes)
            {
                if (customAttribute.GetType() == typeof(ImGuiReadPropertyAttribute))
                {
                    var fieldType = field.FieldType;
                    if(fieldType == typeof(Vector2))
                    {
                        Console.WriteLine("SUPERHIT");
                        var vector2Boi = new Vector2ImguiDisplay
                            { FieldPtr = field, Name = $"{component.Parent}.{field.Name}", Owner = component };
                        Vector2Watches.Add(vector2Boi);
                    }
                }
            }
        }

    }

    public void AddVector2(ImGuiReadPropertyAttribute attr)
    {
        
    }

    public class Vector2ImguiDisplay
    {
        public string Name;
        public FieldInfo FieldPtr;
        public Component Owner;
        public Vector2 GetValue => (Vector2)FieldPtr.GetValue(Owner);

    }

    public float DrawOrder { get; set; }
    public bool Visible { get; }
    public event EventHandler<EventArgs> DrawOrderChanged;
    public event EventHandler<EventArgs> VisibleChanged;
}