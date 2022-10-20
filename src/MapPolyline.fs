namespace Fabulous.XamarinForms.Maps

open System.Runtime.CompilerServices
open Fabulous
open Fabulous.StackAllocatedCollections
open Fabulous.StackAllocatedCollections.StackList
open Fabulous.XamarinForms
open Xamarin.Forms.Maps

type IMapPolyline =
    inherit IMapElement

module MapPolyline =
    let WidgetKey = Widgets.register<Polyline> ()

    let GeoPathList =
        Attributes.defineSimpleScalarWithEquality<Position list> "Polyline_GeoPath" (fun _ newValueOpt node ->
            let polyline = node.Target :?> Polyline

            match newValueOpt with
            | ValueNone -> polyline.Geopath.Clear()
            | ValueSome geoPaths -> geoPaths |> List.iter polyline.Geopath.Add)

[<AutoOpen>]
module MapPolylineBuilders =

    type Fabulous.XamarinForms.View with

        static member inline MapPolyline<'msg>(geoPaths: Position list) =
            WidgetBuilder<'msg, IMapPolyline>(
                MapPolyline.WidgetKey,
                AttributesBundle(StackList.one (MapPolyline.GeoPathList.WithValue(geoPaths)), ValueNone, ValueNone)
            )

[<Extension>]
type PolylineModifiers =
    /// <summary>Link a ViewRef to access the direct Polyline control instance</summary>
    [<Extension>]
    static member inline reference(this: WidgetBuilder<'msg, IMapPolyline>, value: ViewRef<Polyline>) =
        this.AddScalar(ViewRefAttributes.ViewRef.WithValue(value.Unbox))
