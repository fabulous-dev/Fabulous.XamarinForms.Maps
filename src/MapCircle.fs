﻿namespace Fabulous.XamarinForms.Maps

open System.Runtime.CompilerServices
open Fabulous
open Fabulous.XamarinForms
open Xamarin.Forms.Maps

type IMapCircle =
    inherit IMapElement

module MapCircle =
    let WidgetKey = Widgets.register<Circle> ()

    let Center = Attributes.defineBindableWithEquality<Position> Circle.CenterProperty

    let Radius = Attributes.defineBindableWithEquality<Distance> Circle.RadiusProperty

    let FillColor = Attributes.defineBindableAppThemeColor Circle.FillColorProperty

[<AutoOpen>]
module MapCircleBuilders =

    type Fabulous.XamarinForms.View with

        /// <summary>A Circle object can be added to a map by instantiating it and adding it to the map's MapElements collection</summary>
        /// <param name ="center">Position object that defines the center of the circle, in latitude and longitude.</param>
        /// <param name ="radius">Distance object that defines the radius of the circle in meters, kilometers, or miles.</param>
        static member inline MapCircle<'msg>(center: Position, radius: Distance) =
            WidgetBuilder<'msg, IMapCircle>(
                MapCircle.WidgetKey,
                MapCircle.Center.WithValue(center),
                MapCircle.Radius.WithValue(radius)
            )

[<Extension>]
type MapCircleModifiers =
    /// <summary>Set the color within the circle perimeter. If is not specified the stroke will default to transparent.</summary>
    /// <param name="light">The color within the circle perimeter in the light theme.</param>
    /// <param name="dark">The color within the circle perimeter in the dark theme.</param>
    [<Extension>]
    static member inline fillColor(this: WidgetBuilder<'msg, #IMapCircle>, light: FabColor, ?dark: FabColor) =
        this.AddScalar(MapCircle.FillColor.WithValue(AppTheme.create light dark))

    /// <summary>Link a ViewRef to access the direct Circle control instance</summary>
    [<Extension>]
    static member inline reference(this: WidgetBuilder<'msg, IMapCircle>, value: ViewRef<Circle>) =
        this.AddScalar(ViewRefAttributes.ViewRef.WithValue(value.Unbox))
