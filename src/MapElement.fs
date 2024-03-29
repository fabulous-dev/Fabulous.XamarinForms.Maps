﻿namespace Fabulous.XamarinForms.Maps

open System.Runtime.CompilerServices
open Fabulous
open Fabulous.XamarinForms
open Xamarin.Forms.Maps

type IMapElement =
    inherit Fabulous.XamarinForms.IElement

module MapElement =
    let WidgetKey = Widgets.register<MapElement> ()

    let StrokeColor =
        Attributes.defineBindableAppThemeColor MapElement.StrokeColorProperty

    let StrokeWidth = Attributes.defineBindableFloat MapElement.StrokeWidthProperty

[<Extension>]
type MapElementModifiers =
    /// <summary>Set the line color. If is not specified the stroke will default to black.</summary>
    /// <param name="light">The color of the line in the light theme.</param>
    /// <param name="dark">The color of the line in the dark theme.</param>
    [<Extension>]
    static member inline strokeColor(this: WidgetBuilder<'msg, #IMapElement>, light: FabColor, ?dark: FabColor) =
        this.AddScalar(MapElement.StrokeColor.WithValue(AppTheme.create light dark))

    /// <summary>Sets the line width.</summary>
    [<Extension>]
    static member inline strokeWidth(this: WidgetBuilder<'msg, #IMapElement>, value: float) =
        this.AddScalar(MapElement.StrokeWidth.WithValue(value))

    /// <summary>Link a ViewRef to access the direct MapElement control instance</summary>
    [<Extension>]
    static member inline reference(this: WidgetBuilder<'msg, IMapElement>, value: ViewRef<MapElement>) =
        this.AddScalar(ViewRefAttributes.ViewRef.WithValue(value.Unbox))
