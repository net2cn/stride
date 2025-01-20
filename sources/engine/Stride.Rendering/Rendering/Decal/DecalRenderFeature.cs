// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org/ & https://stride3d.net) and Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System;
using Stride.Core.Annotations;
using Stride.Graphics;
using Stride.Rendering.Background;
using Stride.Rendering.Skyboxes;
using Stride.Streaming;

namespace Stride.Rendering.Decal
{
    public class DecalRenderFeature : RootRenderFeature
    {
        public override Type SupportedRenderObjectType => typeof(RenderDecal);

        public DecalRenderFeature()
        {
        }

        public override void Prepare(RenderDrawContext context)
        {
            base.Prepare(context);

            // Register resources usage
            foreach (var renderObject in RenderObjects)
            {
                var renderDecal = (RenderDecal)renderObject;
                Context.StreamingManager?.StreamResources(renderDecal.MaterialPass.Parameters);
            }
        }

        public override void Draw(RenderDrawContext context, RenderView renderView, RenderViewStage renderViewStage, int startIndex, int endIndex)
        {
            for (int index = startIndex; index < endIndex; index++)
            {
                var renderNodeReference = renderViewStage.SortedRenderNodes[index].RenderNode;
                var renderNode = GetRenderNode(renderNodeReference);
                var renderDecal = (RenderDecal)renderNode.RenderObject;

                if (renderDecal.MaterialPass == null)
                    continue;

                Draw(context, renderView, renderDecal);
            }
        }

        protected override void InitializeCore()
        {

        }

        private void Draw([NotNull] RenderDrawContext context, [NotNull] RenderView renderView, [NotNull] RenderDecal renderDecal)
        {

        }
    }
}
