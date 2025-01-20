// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org/ & https://stride3d.net) and Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System.Collections.Generic;
using Stride.Core;
using Stride.Core.Mathematics;
using Stride.Engine;
using Stride.Rendering.Lights;

namespace Stride.Rendering.Decal
{
    /// <summary>
    /// Process <see cref="DecalComponent"/> stored in an <see cref="EntityManager"/>.
    /// </summary>
    public class DecalProcessor : EntityProcessor<DecalComponent, RenderDecal>, IEntityComponentRenderProcessor
    {
        private const int DefaultDecalCapacityCount = 512;

        /// <summary>
        /// Initializes a new instance of the <see cref="DecalProcessor"/> class.
        /// </summary>
        public DecalProcessor()
        {
        }

        /// <inheritdoc/>
        public VisibilityGroup VisibilityGroup { get; set; }

        /// <summary>
        /// Gets the active decals.
        /// </summary>
        /// <value>The decals.</value>
        public List<RenderDecal> Decals { get; } = new List<RenderDecal>(DefaultDecalCapacityCount);

        protected override RenderDecal GenerateComponentData(Entity entity, DecalComponent component) => new RenderDecal();

        protected override bool IsAssociatedDataValid(Entity entity, DecalComponent component, RenderDecal associatedData) => true;

        protected internal override void OnSystemAdd()
        {
            base.OnSystemAdd();
        }

        protected internal override void OnSystemRemove()
        {
            base.OnSystemRemove();
        }

        public override void Draw(RenderContext context)
        {
            // 1) Clear the cache of current decals (without destroying collections but keeping previously allocated ones)
            Decals.Clear();


            // 2) Prepare decals
            foreach (var decalPair in ComponentDatas)
            {
                var decalComponent = decalPair.Key;
                var renderDecal = decalPair.Value;

                // Disabled decal
                if (!decalComponent.Enabled)
                {
                    continue;
                }

                // Decal with no material
                if (decalComponent.Material == null || decalComponent.Material.Passes.Count <= 0)
                {
                    continue;
                }

                renderDecal.Size = decalComponent.Size;
                renderDecal.MaterialPass = decalComponent.Material.Passes[0];   // Only one pass
                renderDecal.WorldMatrix = decalComponent.Entity.Transform.WorldMatrix;

                // Compute decal position
                renderDecal.Position = renderDecal.WorldMatrix.TranslationVector;

                // Compute bounding boxes
                renderDecal.UpdateBoundingBox();

                Decals.Add(renderDecal);
            }
        }
    }
}
