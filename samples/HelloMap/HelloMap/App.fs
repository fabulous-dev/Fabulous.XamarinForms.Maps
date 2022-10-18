namespace HelloMap

open Fabulous
open Xamarin.Forms.Maps
open Fabulous.XamarinForms
open Fabulous.XamarinForms.Maps

open type Fabulous.XamarinForms.View

module App =

    type Model = { Position: Position }

    type Msg =
        | MapClicked of Position
        | MarkerClicked of bool

    let init () =
        { Position = Position(48.8566, 2.3522) }, Cmd.none

    let update msg model =
        match msg with
        | MapClicked _ -> model, Cmd.none
        | MarkerClicked _ -> model, Cmd.none

    let private requestedRegion position =
        MapSpan.FromCenterAndRadius(position, Distance.FromKilometers(25.))

    let view (model: Model) =
        Application(
            ContentPage(
                "HelloMap",
                (Map(requestedRegion model.Position) {
                    Pin(PinType.Place, "I'm a marker", model.Position)
                        .address("My Address")
                        .onMarkerClicked(MarkerClicked)
                        .onInfoWindowClicked (MarkerClicked)
                })
                    .hasZoomEnabled(true)
                    .hasScrollEnabled(true)
                    .mapType(MapType.Street)
                    .isShowingUser(true)
                    .isTrafficEnabled(true)
                    .onMapClicked (MapClicked)
            )
                .ignoreSafeArea ()
        )

    let program = Program.statefulWithCmd init update view
