namespace Fabulous.XamarinForms.Maps

open System.Runtime.CompilerServices
open Fabulous
open Fabulous.XamarinForms
open Xamarin.Forms.Maps


type IMapPin =
    inherit Fabulous.XamarinForms.IElement

module MapPin =
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
module MapPinBuilders =
    type Fabulous.XamarinForms.View with

        /// Defines a Pin widget
        static member inline MapPin<'msg>(position: Position) =
            WidgetBuilder<'msg, IMapPin>(MapPin.WidgetKey, MapPin.Position.WithValue(position))

[<Extension>]
type MapPinModifiers =
    [<Extension>]
    static member inline address(this: WidgetBuilder<'msg, #IMapPin>, value: string) =
        this.AddScalar(MapPin.Address.WithValue(value))

    [<Extension>]
    static member inline label(this: WidgetBuilder<'msg, #IMapPin>, value: string) =
        this.AddScalar(MapPin.Label.WithValue(value))

    [<Extension>]
    static member inline pinType(this: WidgetBuilder<'msg, #IMapPin>, value: PinType) =
        this.AddScalar(MapPin.PinType.WithValue(value))

    [<Extension>]
    static member inline onMarkerClicked(this: WidgetBuilder<'msg, #IMapPin>, onMarkerClicked: bool -> 'msg) =
        this.AddScalar(MapPin.MarkerClicked.WithValue(fun args -> onMarkerClicked args.HideInfoWindow |> box))

    [<Extension>]
    static member inline onInfoWindowClicked(this: WidgetBuilder<'msg, #IMapPin>, onInfoWindowClicked: bool -> 'msg) =
        this.AddScalar(MapPin.InfoWindowClicked.WithValue(fun args -> onInfoWindowClicked args.HideInfoWindow |> box))

    /// <summary>Link a ViewRef to access the direct Pin control instance</summary>
    [<Extension>]
    static member inline reference(this: WidgetBuilder<'msg, IMapPin>, value: ViewRef<Pin>) =
        this.AddScalar(ViewRefAttributes.ViewRef.WithValue(value.Unbox))
