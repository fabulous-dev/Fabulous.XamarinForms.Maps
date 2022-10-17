namespace Fabulous.XamarinForms.Maps

open System.Runtime.CompilerServices
open Fabulous
open Fabulous.StackAllocatedCollections.StackList
open Fabulous.XamarinForms
open Xamarin.Forms.Maps

type IMap =
    inherit Fabulous.XamarinForms.IView

module Map =
    let WidgetKey = Widgets.register<Map> ()
    
    let HasScrollEnabled = Attributes.defineBindableBool Map.HasScrollEnabledProperty
    let HasZoomEnabled = Attributes.defineBindableBool Map.HasZoomEnabledProperty
    let IsShowingUse = Attributes.defineBindableBool Map.IsShowingUserProperty
    
    let TrafficEnabledProperty = Attributes.defineBindableBool Map.TrafficEnabledProperty
    
    let MoveToLastRegionOnLayoutChange = Attributes.defineBindableBool Map.MoveToLastRegionOnLayoutChangeProperty
    
    let MapType =
        Attributes.defineEnum<MapType>
            "Map_MapType"
            (fun _ newValueOpt node ->
                let map = node.Target :?> Map

                let value =
                    match newValueOpt with
                    | ValueNone -> MapType.Street
                    | ValueSome v -> v

                map.MapType <- value)
            
    let ItemsSource<'T> =
        Attributes.defineBindable<WidgetItems<'T>, System.Collections.Generic.IEnumerable<Widget>>
            Map.ItemsSourceProperty
            (fun modelValue ->
                seq {
                    for x in modelValue.OriginalItems do
                        modelValue.Template x
                })
            (fun a b -> ScalarAttributeComparers.equalityCompare a.OriginalItems b.OriginalItems)


[<AutoOpen>]
module MapBuilders =
    type Fabulous.XamarinForms.View with

        /// Defines a Map widget
        static member inline Map<'msg>() =
            WidgetBuilder<'msg, IMap>(Map.WidgetKey, AttributesBundle(StackList.empty (), ValueNone, ValueNone))

[<Extension>]
type MapModifiers =
    /// <summary>Link a ViewRef to access the direct Map control instance</summary>
    [<Extension>]
    static member inline reference(this: WidgetBuilder<'msg, IMap>, value: ViewRef<Map>) =
        this.AddScalar(ViewRefAttributes.ViewRef.WithValue(value.Unbox))
