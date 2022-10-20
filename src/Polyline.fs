namespace Fabulous.XamarinForms.Maps

open System.Runtime.CompilerServices
open Fabulous
open Fabulous.StackAllocatedCollections
open Fabulous.StackAllocatedCollections.StackList
open Fabulous.XamarinForms
open Xamarin.Forms.Maps

type IPolyline =
    inherit IMapElement

module Polyline =
    let WidgetKey = Widgets.register<Polyline> ()

    let GeoPathList =
        Attributes.defineSimpleScalarWithEquality<Position list> "Polyline_GeoPath" (fun _ newValueOpt node ->
            let polyline = node.Target :?> Polyline

            match newValueOpt with
            | ValueNone -> polyline.Geopath.Clear()
            | ValueSome geoPaths -> geoPaths |> List.iter polyline.Geopath.Add)


[<AutoOpen>]
module PolylineBuilders =

    type Fabulous.XamarinForms.View with

        static member inline Polyline<'msg>(geoPaths: Position list) =
            WidgetBuilder<'msg, IPolyline>(
                Polyline.WidgetKey,
                AttributesBundle(StackList.one (Polyline.GeoPathList.WithValue(geoPaths)), ValueNone, ValueNone)
            )

[<Extension>]
type PolylineModifiers =
    /// <summary>Link a ViewRef to access the direct Polyline control instance</summary>
    [<Extension>]
    static member inline reference(this: WidgetBuilder<'msg, IPolyline>, value: ViewRef<Polyline>) =
        this.AddScalar(ViewRefAttributes.ViewRef.WithValue(value.Unbox))
