// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org/ & https://stride3d.net) and Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System;
using Stride.Core.Mathematics;
using Stride.Rendering.Lights;

namespace Stride.Rendering.Decal
{
    public class RenderDecal : RenderObject
    {
        public Matrix WorldMatrix;
        public Vector3 Size;
        public MaterialPass MaterialPass;
        public float DrawDistance;
        public float FadeScale;

        /// <summary>
        /// Gets the decal position in World-Space (computed by the `DecalProcessor`) (readonly field). See remarks.
        /// </summary>
        /// <value>The position.</value>
        /// <remarks>This property should only be used inside a renderer and not from a script as it is updated after scripts</remarks>
        public Vector3 Position;

        /// <summary>
        /// The determines whether this instance has a valid bounding box (readonly field).
        /// </summary>
        public bool HasBoundingBox;

        /// <summary>
        /// Updates this instance( <see cref="HasBoundingBox"/>, <see cref="this.BoundingBox"/> )
        /// </summary>
        internal void UpdateBoundingBox()
        {
            // Computes the bounding boxes
            HasBoundingBox = true;
            BoundingBox = new BoundingBoxExt(new BoundingBox(Position - Size, Position + Size));
        }
    }
}
