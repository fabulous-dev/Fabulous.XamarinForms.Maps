namespace Fabulous.XamarinForms.Maps

open System.Runtime.CompilerServices
open Fabulous
open Fabulous.StackAllocatedCollections
open Fabulous.StackAllocatedCollections.StackList
open Fabulous.XamarinForms
open Xamarin.Forms.Maps

type IMap =
    inherit Fabulous.XamarinForms.IView

module Map =
    let WidgetKey = Widgets.register<Map> ()

    let MapType =
        Attributes.defineEnum<MapType> "Map_MapType" (fun _ newValueOpt node ->
            let map = node.Target :?> Map

            let value =
                match newValueOpt with
                | ValueNone -> MapType.Street
                | ValueSome v -> v

            map.MapType <- value)

    let IsShowingUser = Attributes.defineBindableBool Map.IsShowingUserProperty

    let TrafficEnabled = Attributes.defineBindableBool Map.TrafficEnabledProperty

    let HasScrollEnabled = Attributes.defineBindableBool Map.HasScrollEnabledProperty

    let HasZoomEnabled = Attributes.defineBindableBool Map.HasZoomEnabledProperty

    let MoveToLastRegionOnLayoutChange =
        Attributes.defineBindableBool Map.MoveToLastRegionOnLayoutChangeProperty

    let RequestedRegion =
        Attributes.defineSimpleScalarWithEquality<MapSpan> "Map_RequestedRegion" (fun _ newValueOpt node ->
            let map = node.Target :?> Map

            match newValueOpt with
            | ValueNone -> ()
            | ValueSome mapSpan -> map.MoveToRegion(mapSpan))

    let Pins =
        Attributes.defineListWidgetCollection "Map_Pins" (fun target -> (target :?> Map).Pins)

    let MapElements =
        Attributes.defineListWidgetCollection "Map_MapElements" (fun target -> (target :?> Map).MapElements)

    let MapClicked =
        Attributes.defineEvent<MapClickedEventArgs> "Map_MapClicked" (fun target -> (target :?> Map).MapClicked)

[<AutoOpen>]
module MapBuilders =
    type Fabulous.XamarinForms.View with

        /// Defines a Map widget
        static member inline Map<'msg>(?requestRegion: MapSpan) =
            match requestRegion with
            | Some mapSpan ->
                WidgetBuilder<'msg, IMap>(Map.WidgetKey, AttributesBundle(StackList.one(Map.RequestedRegion.WithValue(mapSpan)), ValueNone, ValueNone))
            | None ->
                WidgetBuilder<'msg, IMap>(Map.WidgetKey, AttributesBundle(StackList.empty (), ValueNone, ValueNone))

        static member inline MapWithPins<'msg>(requestRegion: MapSpan) =
            CollectionBuilder<'msg, IMap, IPin>(Map.WidgetKey, Map.Pins, Map.RequestedRegion.WithValue(requestRegion))

[<Extension>]
type MapModifiers =
    [<Extension>]
    static member inline hasZoomEnabled(this: WidgetBuilder<'msg, #IMap>, value: bool) =
        this.AddScalar(Map.HasZoomEnabled.WithValue(value))

    [<Extension>]
    static member inline hasScrollEnabled(this: WidgetBuilder<'msg, #IMap>, value: bool) =
        this.AddScalar(Map.HasScrollEnabled.WithValue(value))

    [<Extension>]
    static member inline mapType(this: WidgetBuilder<'msg, #IMap>, value: MapType) =
        this.AddScalar(Map.MapType.WithValue(value))

    [<Extension>]
    static member inline isShowingUser(this: WidgetBuilder<'msg, #IMap>, value: bool) =
        this.AddScalar(Map.IsShowingUser.WithValue(value))

    [<Extension>]
    static member inline isTrafficEnabled(this: WidgetBuilder<'msg, #IMap>, value: bool) =
        this.AddScalar(Map.TrafficEnabled.WithValue(value))

    [<Extension>]
    static member inline moveToLastRegionOnLayoutChange(this: WidgetBuilder<'msg, #IMap>, value: bool) =
        this.AddScalar(Map.MoveToLastRegionOnLayoutChange.WithValue(value))

    [<Extension>]
    static member inline onMapClicked(this: WidgetBuilder<'msg, #IMap>, onMapClicked: Position -> 'msg) =
        this.AddScalar(Map.MapClicked.WithValue(fun args -> onMapClicked args.Position |> box))

    [<Extension>]
    static member inline mapElements<'msg, 'marker when 'marker :> IMap>(this: WidgetBuilder<'msg, 'marker>) =
        WidgetHelpers.buildAttributeCollection<'msg, 'marker, IMapElement> Map.MapElements this

    /// <summary>Link a ViewRef to access the direct Map control instance</summary>
    [<Extension>]
    static member inline reference(this: WidgetBuilder<'msg, IMap>, value: ViewRef<Map>) =
        this.AddScalar(ViewRefAttributes.ViewRef.WithValue(value.Unbox))

[<Extension>]
type CollectionBuilderExtensions =
    [<Extension>]
    static member inline Yield<'msg, 'marker, 'itemType when 'itemType :> IPin>
        (
            _: AttributeCollectionBuilder<'msg, 'marker, IPin>,
            x: WidgetBuilder<'msg, 'itemType>
        ) : Content<'msg> =
        { Widgets = MutStackArray1.One(x.Compile()) }

    [<Extension>]
    static member inline Yield<'msg, 'marker, 'itemType when 'itemType :> IPin>
        (
            _: AttributeCollectionBuilder<'msg, 'marker, IPin>,
            x: WidgetBuilder<'msg, Memo.Memoized<'itemType>>
        ) : Content<'msg> =
        { Widgets = MutStackArray1.One(x.Compile()) }

    [<Extension>]
    static member inline Yield<'msg, 'marker, 'itemType when 'itemType :> IMapElement>
        (
            _: AttributeCollectionBuilder<'msg, 'marker, IMapElement>,
            x: WidgetBuilder<'msg, 'itemType>
        ) : Content<'msg> =
        { Widgets = MutStackArray1.One(x.Compile()) }

    [<Extension>]
    static member inline Yield<'msg, 'marker, 'itemType when 'itemType :> IMapElement>
        (
            _: AttributeCollectionBuilder<'msg, 'marker, IMapElement>,
            x: WidgetBuilder<'msg, Memo.Memoized<'itemType>>
        ) : Content<'msg> =
        { Widgets = MutStackArray1.One(x.Compile()) }


    [<Extension>]
    static member inline Yield<'msg, 'marker, 'itemType when 'itemType :> IPin>
        (
            _: CollectionBuilder<'msg, 'marker, IPin>,
            x: WidgetBuilder<'msg, Memo.Memoized<'itemType>>
        ) : Content<'msg> =
        { Widgets = MutStackArray1.One(x.Compile()) }

    [<Extension>]
    static member inline Yield<'msg, 'marker, 'itemType when 'itemType :> IPin>
        (
            _: CollectionBuilder<'msg, 'marker, IPin>,
            x: WidgetBuilder<'msg, 'itemType>
        ) : Content<'msg> =
        { Widgets = MutStackArray1.One(x.Compile()) }
