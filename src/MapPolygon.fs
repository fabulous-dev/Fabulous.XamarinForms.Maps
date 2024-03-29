﻿namespace Fabulous.XamarinForms.Maps

open System.Runtime.CompilerServices
open Fabulous
open Fabulous.StackAllocatedCollections
open Fabulous.StackAllocatedCollections.StackList
open Fabulous.XamarinForms
open Xamarin.Forms.Maps

type IMapPolygon =
    inherit IMapElement

module MapPolygon =
    let WidgetKey = Widgets.register<Polygon> ()

    let FillColor = Attributes.defineBindableAppThemeColor Polygon.FillColorProperty

    let GeoPathList =
        Attributes.defineSimpleScalarWithEquality<Position list> "Polygon_GeoPath" (fun _ newValueOpt node ->
            let map = node.Target :?> Polygon

            match newValueOpt with
            | ValueNone -> map.Geopath.Clear()
            | ValueSome geoPaths -> geoPaths |> List.iter map.Geopath.Add)

[<AutoOpen>]
module MapPolygonBuilders =

    type Fabulous.XamarinForms.View with

        /// <summary>A Polygon object can be added to a map by instantiating it and adding it to the map's MapElements collection. A Polygon is a fully enclosed shape. The first and last points will automatically be connected if they do not match.</summary>
        /// <param name ="geoPaths">Contains a list of Position objects defining the geographic coordinates of the polygon points. A Polygon object is rendered on the map once it has been added to the MapElements collection of the Map.</param>
        static member inline MapPolygon<'msg>(geoPaths: Position list) =
            WidgetBuilder<'msg, IMapPolygon>(
                MapPolygon.WidgetKey,
                AttributesBundle(StackList.one (MapPolygon.GeoPathList.WithValue(geoPaths)), ValueNone, ValueNone)
            )

[<Extension>]
type MapPolygonModifiers =
    /// <summary>Set the polygon's background color. If is not specified the stroke will default to transparent.</summary>
    /// <param name="light">The polygon's background color in the light theme.</param>
    /// <param name="dark">The polygon's background color in the dark theme.</param>
    [<Extension>]
    static member inline fillColor(this: WidgetBuilder<'msg, #IMapPolygon>, light: FabColor, ?dark: FabColor) =
        this.AddScalar(MapPolygon.FillColor.WithValue(AppTheme.create light dark))

    /// <summary>Link a ViewRef to access the direct Polygon control instance</summary>
    [<Extension>]
    static member inline reference(this: WidgetBuilder<'msg, IMapPolygon>, value: ViewRef<Polygon>) =
        this.AddScalar(ViewRefAttributes.ViewRef.WithValue(value.Unbox))
