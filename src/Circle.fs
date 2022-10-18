namespace Fabulous.XamarinForms.Maps

open System.Collections.Generic
open System.Runtime.CompilerServices
open Fabulous
open Fabulous.StackAllocatedCollections
open Fabulous.StackAllocatedCollections.StackList
open Fabulous.XamarinForms
open Xamarin.Forms.Maps

type ICircle =
    inherit IMapElement

module Circle =
    let WidgetKey = Widgets.register<Circle> ()

    let Center = Attributes.defineBindableWithEquality<Position> Circle.CenterProperty

    let Radius = Attributes.defineBindableWithEquality<Distance> Circle.RadiusProperty

    let FillColor = Attributes.defineBindableAppThemeColor Circle.FillColorProperty

[<AutoOpen>]
module CircleBuilders =

    type Fabulous.XamarinForms.View with

        static member inline Circle<'msg>(light: FabColor, ?dark: FabColor) =
            WidgetBuilder<'msg, ICircle>(
                Circle.WidgetKey,
                AttributesBundle(
                    StackList.one (Circle.FillColor.WithValue(AppTheme.create light dark)),
                    ValueNone,
                    ValueNone
                )
            )

[<Extension>]
type CircleModifiers =
    [<Extension>]
    static member inline center(this: WidgetBuilder<'msg, #ICircle>, value: Position) =
        this.AddScalar(Circle.Center.WithValue(value))

    [<Extension>]
    static member inline radius(this: WidgetBuilder<'msg, #ICircle>, value: Distance) =
        this.AddScalar(Circle.Radius.WithValue(value))

    /// <summary>Link a ViewRef to access the direct Circle control instance</summary>
    [<Extension>]
    static member inline reference(this: WidgetBuilder<'msg, ICircle>, value: ViewRef<Circle>) =
        this.AddScalar(ViewRefAttributes.ViewRef.WithValue(value.Unbox))
