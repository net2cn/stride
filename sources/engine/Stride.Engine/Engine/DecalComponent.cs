// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org/ & https://stride3d.net) and Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System;
using System.ComponentModel;

using Stride.Core;
using Stride.Core.Mathematics;
using Stride.Engine.Design;
using Stride.Rendering;
using Stride.Rendering.Decal;

namespace Stride.Engine
{
    [DataContract("DecalComponent")]
    [Display("Decal")]
    [DefaultEntityComponentRenderer(typeof(DecalProcessor))]
    [ComponentOrder(11100)]
    [ComponentCategory("Rendering")]
    public class DecalComponent : ActivableEntityComponent
    {
        [DataMember(10)]
        public Material Material { get; set; }

        [DataMember(20)]
        public Vector3 Size { get; set; }

        [DataMember(30)]
        [DefaultValue(1000f)]
        public float DrawDistance { get; set; }

        [DataMember(40)]
        [DefaultValue(0.9f)]
        public float FadeScale { get; set; }

        public DecalComponent()
        {
            Size = Vector3.One;
            DrawDistance = 1000f;
            FadeScale = 0.9f;
        }
    }
}
