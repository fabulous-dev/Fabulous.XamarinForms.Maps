namespace Fabulous.XamarinForms.Maps

open System.Runtime.CompilerServices
open Fabulous
open Fabulous.StackAllocatedCollections.StackList
open Fabulous.XamarinForms
open Xamarin.Forms.Maps

type IPin =
    inherit Fabulous.XamarinForms.IElement

module Pin =
    let WidgetKey = Widgets.register<Pin> ()
    
[<AutoOpen>]
module PinBuilders =
    type Fabulous.XamarinForms.View with

        /// Defines a Pin widget
        static member inline Pin<'msg>() =
            WidgetBuilder<'msg, IPin>(Pin.WidgetKey, AttributesBundle(StackList.empty (), ValueNone, ValueNone))

[<Extension>]
type PinModifiers =
    /// <summary>Link a ViewRef to access the direct Pin control instance</summary>
    [<Extension>]
    static member inline reference(this: WidgetBuilder<'msg, IPin>, value: ViewRef<Pin>) =
        this.AddScalar(ViewRefAttributes.ViewRef.WithValue(value.Unbox))
