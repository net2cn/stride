// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org/ & https://stride3d.net) and Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using Stride.Core.Mathematics;
using Stride.Engine;
using Stride.Engine.Gizmos;
using Stride.Graphics;
using Stride.Rendering;
using Buffer = Stride.Graphics.Buffer;

namespace Stride.Assets.Presentation.AssetEditors.Gizmos
{
    /// <summary>
    /// The gizmo for the decal component
    /// </summary>
    [GizmoComponent(typeof(DecalComponent), true)]
    public class DecalGizmo : BillboardingGizmo<DecalComponent>
    {
        private BoxMesh box;
        private Entity entity;

        public DecalGizmo(EntityComponent component) : base(component, "Decal gizmo", GizmoResources.DecalGizmo)
        {
        }

        protected override Entity Create()
        {
            var root = base.Create();

            box = new BoxMesh(GraphicsDevice);
            box.Build();

            var material = GizmoEmissiveColorMaterial.Create(GraphicsDevice, Color.White);

            entity = new Entity($"Decal box volume of {Component.Entity.Name}")
            {
                new ModelComponent
                {
                    Model = new Model
                    {
                        material,
                        new Mesh { Draw = box.MeshDraw },
                    },
                    RenderGroup = RenderGroup,
                }
            };

            return root;
        }

        public override void Update()
        {
            // Translation and rotation on bounding boxes
            GizmoRootEntity.Transform.Position = Component.Entity.Transform.Position;
            GizmoRootEntity.Transform.Rotation = Component.Entity.Transform.Rotation;
            GizmoRootEntity.Transform.Scale = Component.Size / 2;   // Scale accrodingly.
            GizmoRootEntity.Transform.UpdateWorldMatrix();
        }

        public override bool IsSelected
        {
            set
            {
                bool hasChanged = IsSelected != value;
                base.IsSelected = value;

                if (hasChanged)
                {
                    if (IsSelected)
                    {
                        GizmoRootEntity.AddChild(entity);
                    }
                    else
                    {
                        GizmoRootEntity.RemoveChild(entity);
                    }
                }
            }
        }

        class BoxMesh
        {
            public MeshDraw MeshDraw;

            private Buffer vertexBuffer;

            private readonly GraphicsDevice graphicsDevice;

            public BoxMesh(GraphicsDevice graphicsDevice)
            {
                this.graphicsDevice = graphicsDevice;
            }

            public void Build()
            {
                var indices = new int[12 * 2];
                var vertices = new VertexPositionNormalTexture[8];

                vertices[0] = new VertexPositionNormalTexture(new Vector3(-1, 1, -1), Vector3.UnitY, Vector2.Zero);
                vertices[1] = new VertexPositionNormalTexture(new Vector3(-1, 1, 1), Vector3.UnitY, Vector2.Zero);
                vertices[2] = new VertexPositionNormalTexture(new Vector3(1, 1, 1), Vector3.UnitY, Vector2.Zero);
                vertices[3] = new VertexPositionNormalTexture(new Vector3(1, 1, -1), Vector3.UnitY, Vector2.Zero);

                int indexOffset = 0;
                // Top sides
                for (int i = 0; i < 4; i++)
                {
                    indices[indexOffset++] = i;
                    indices[indexOffset++] = (i + 1) % 4;
                }

                // Duplicate vertices and indices to bottom part
                for (int i = 0; i < 4; i++)
                {
                    vertices[i + 4] = vertices[i];
                    vertices[i + 4].Position.Y = -vertices[i + 4].Position.Y;

                    indices[indexOffset++] = indices[i * 2] + 4;
                    indices[indexOffset++] = indices[i * 2 + 1] + 4;
                }

                // Sides
                for (int i = 0; i < 4; i++)
                {
                    indices[indexOffset++] = i;
                    indices[indexOffset++] = i + 4;
                }

                vertexBuffer = Buffer.Vertex.New(graphicsDevice, vertices, GraphicsResourceUsage.Dynamic);
                MeshDraw = new MeshDraw
                {
                    PrimitiveType = PrimitiveType.LineList,
                    DrawCount = indices.Length,
                    IndexBuffer = new IndexBufferBinding(Buffer.Index.New(graphicsDevice, indices), true, indices.Length),
                    VertexBuffers = new[] { new VertexBufferBinding(vertexBuffer, VertexPositionNormalTexture.Layout, vertexBuffer.ElementCount) },
                };
            }
        }
    }
}
