namespace Fabulous.XamarinForms.Maps

open System.Runtime.CompilerServices
open Fabulous
open Fabulous.XamarinForms
open Xamarin.Forms.Maps

type IPin =
    inherit Fabulous.XamarinForms.IElement

module Pin =
    let WidgetKey = Widgets.register<Pin> ()

    let PinType = Attributes.defineBindableWithEquality<PinType> Pin.TypeProperty

    let Position = Attributes.defineBindableWithEquality<Position> Pin.PositionProperty

    let Address = Attributes.defineBindableWithEquality<string> Pin.AddressProperty

    let Label = Attributes.defineBindableWithEquality<string> Pin.LabelProperty

    let MarkerClicked =
        Attributes.defineEvent<PinClickedEventArgs> "Pin_MarkerClicked" (fun target -> (target :?> Pin).MarkerClicked)

    let InfoWindowClicked =
        Attributes.defineEvent<PinClickedEventArgs> "Pin_InfoWindowClicked" (fun target ->
            (target :?> Pin).InfoWindowClicked)

[<AutoOpen>]
module PinBuilders =
    type Fabulous.XamarinForms.View with

        /// Defines a Pin widget
        static member inline Pin<'msg>(pinType: PinType, label: string, position: Position) =
            WidgetBuilder<'msg, IPin>(
                Pin.WidgetKey,
                Pin.PinType.WithValue(pinType),
                Pin.Label.WithValue(label),
                Pin.Position.WithValue(position)
            )

[<Extension>]
type PinModifiers =
    [<Extension>]
    static member inline address(this: WidgetBuilder<'msg, #IPin>, value: string) =
        this.AddScalar(Pin.Address.WithValue(value))

    [<Extension>]
    static member inline onMarkerClicked(this: WidgetBuilder<'msg, #IPin>, onMarkerClicked: bool -> 'msg) =
        this.AddScalar(Pin.MarkerClicked.WithValue(fun args -> onMarkerClicked args.HideInfoWindow |> box))

    [<Extension>]
    static member inline onInfoWindowClicked(this: WidgetBuilder<'msg, #IPin>, onInfoWindowClicked: bool -> 'msg) =
        this.AddScalar(Pin.InfoWindowClicked.WithValue(fun args -> onInfoWindowClicked args.HideInfoWindow |> box))

    /// <summary>Link a ViewRef to access the direct Pin control instance</summary>
    [<Extension>]
    static member inline reference(this: WidgetBuilder<'msg, IPin>, value: ViewRef<Pin>) =
        this.AddScalar(ViewRefAttributes.ViewRef.WithValue(value.Unbox))
