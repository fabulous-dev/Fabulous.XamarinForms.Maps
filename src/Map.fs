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
