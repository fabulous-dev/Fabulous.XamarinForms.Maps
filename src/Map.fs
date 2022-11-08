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

        /// <summary>The Map control is a cross-platform view for displaying and annotating maps</summary>
        /// <param name ="requestRegion">The region of a map to display when a map is loaded can be set by passing a MapSpan.</param>
        static member inline Map<'msg>(requestRegion: MapSpan) =
            WidgetBuilder<'msg, IMap>(
                Map.WidgetKey,
                AttributesBundle(StackList.one (Map.RequestedRegion.WithValue(requestRegion)), ValueNone, ValueNone)
            )

        /// <summary>The Map control is a cross-platform view for displaying and annotating maps</summary>
        /// <param name ="requestRegion">The region of a map to display when a map is loaded can be set by passing a MapSpan.</param>
        static member inline MapWithPins<'msg>(requestRegion: MapSpan) =
            CollectionBuilder<'msg, IMap, IMapPin>(
                Map.WidgetKey,
                Map.Pins,
                Map.RequestedRegion.WithValue(requestRegion)
            )

[<Extension>]
type MapModifiers =
    /// <summary>Determines whether the map is allowed to zoom.</summary>
    [<Extension>]
    static member inline hasZoomEnabled(this: WidgetBuilder<'msg, #IMap>, value: bool) =
        this.AddScalar(Map.HasZoomEnabled.WithValue(value))

    /// <summary>Determines whether the map is allowed to scroll.</summary>
    [<Extension>]
    static member inline hasScrollEnabled(this: WidgetBuilder<'msg, #IMap>, value: bool) =
        this.AddScalar(Map.HasScrollEnabled.WithValue(value))

    /// <summary>Indicates the display style of the map.</summary>
    [<Extension>]
    static member inline mapType(this: WidgetBuilder<'msg, #IMap>, value: MapType) =
        this.AddScalar(Map.MapType.WithValue(value))

    /// <summary>Indicates whether the map is showing the user's current location.</summary>
    [<Extension>]
    static member inline isShowingUser(this: WidgetBuilder<'msg, #IMap>, value: bool) =
        this.AddScalar(Map.IsShowingUser.WithValue(value))

    /// <summary>Indicates whether traffic data is overlaid on the map.</summary>
    [<Extension>]
    static member inline isTrafficEnabled(this: WidgetBuilder<'msg, #IMap>, value: bool) =
        this.AddScalar(Map.TrafficEnabled.WithValue(value))

    /// <summary>Controls whether the displayed map region will move from its current region to its previously set region when a layout change occurs.</summary>
    [<Extension>]
    static member inline moveToLastRegionOnLayoutChange(this: WidgetBuilder<'msg, #IMap>, value: bool) =
        this.AddScalar(Map.MoveToLastRegionOnLayoutChange.WithValue(value))

    /// <summary>Event that is fired when the user interacts with the map.</summary>
    /// <param name="onMapClicked">Msg to dispatch when the user interacts with the map.</param>
    [<Extension>]
    static member inline onMapClicked(this: WidgetBuilder<'msg, #IMap>, onMapClicked: Position -> 'msg) =
        this.AddScalar(Map.MapClicked.WithValue(fun args -> onMapClicked args.Position |> box))

    /// <summary>Represents the list of elements on the map, such as polygons, circles and polylines.</summary>
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
    static member inline Yield<'msg, 'marker, 'itemType when 'itemType :> IMapPin>
        (
            _: CollectionBuilder<'msg, 'marker, IMapPin>,
            x: WidgetBuilder<'msg, Memo.Memoized<'itemType>>
        ) : Content<'msg> =
        { Widgets = MutStackArray1.One(x.Compile()) }

    [<Extension>]
    static member inline Yield<'msg, 'marker, 'itemType when 'itemType :> IMapPin>
        (
            _: CollectionBuilder<'msg, 'marker, IMapPin>,
            x: WidgetBuilder<'msg, 'itemType>
        ) : Content<'msg> =
        { Widgets = MutStackArray1.One(x.Compile()) }
