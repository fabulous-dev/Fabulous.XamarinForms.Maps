namespace HelloMap

open Fabulous
open Xamarin.Forms
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
        { Position = Position(36.9628066, -122.0194722) }, Cmd.none

    let update msg model =
        match msg with
        | MapClicked _ -> model, Cmd.none
        | MarkerClicked _ -> model, Cmd.none

    let view (model: Model) =
        Application(
            ContentPage(
                "HelloMap",
                (MapWithPins() {
                    Pin(PinType.Place, "I'm a marker1", Position(37.79752, -122.40183))
                        .address("My Address1")
                        .onMarkerClicked(MarkerClicked)
                        .onInfoWindowClicked (MarkerClicked)

                // Pin(PinType.Place, "I'm a marker2", Position(36.9571571, -122.0173544))
                //     .address("My Address2")
                //     .onMarkerClicked(MarkerClicked)
                //     .onInfoWindowClicked(MarkerClicked)
                })

                    //Map()
                    .moveToRegion(
                        MapSpan.FromCenterAndRadius(Position(47.640663, -122.1376177), Distance.FromMiles(1.))
                    )
                    //.moveToRegion(MapSpan(Position(37.79752, -122.40183), 0.01, 0.01))
                    .hasZoomEnabled(
                        true
                    )
                    .hasScrollEnabled(true)
                    .mapType(MapType.Street)
                    .isShowingUser(true)
                    .isTrafficEnabled(true)
                    .onMapClicked(MapClicked)
                    .mapElements () {
                    Polyline(
                        [ Position(47.6381401, -122.1317367)
                          Position(47.6381473, -122.1350841)
                          Position(47.6382847, -122.1353094)
                          Position(47.6384582, -122.1354703)
                          Position(47.6401136, -122.1360819)
                          Position(47.6403883, -122.1364681)
                          Position(47.6407426, -122.1377019)
                          Position(47.6412558, -122.1404056)
                          Position(47.6414148, -122.1418647)
                          Position(47.6414654, -122.1432702) ]
                    )
                        .strokeColor(Color.Blue.ToFabColor())
                        .strokeWidth (12.)
                }
            // .mapElements() {
            //     Circle(Color.FromHex("#88FFC0CB").ToFabColor())
            //         .center(Position(37.79752, -122.40183))
            //         .radius(Distance(250.))
            //         .strokeColor(Color.FromHex("#88FF0000").ToFabColor())
            //         .strokeWidth(8.)
            // }
            //     .mapElements () {
            //     Polygon(
            //         [ Position(47.6458676, -122.1356007)
            //           Position(47.6458097, -122.142789)
            //           Position(47.6367593, -122.1428104)
            //           Position(47.6368027, -122.1398707)
            //           Position(47.6380172, -122.1376177)
            //           Position(47.640663, -122.1352359)
            //           Position(47.6426148, -122.1347209)
            //           Position(47.6458676, -122.1356007) ]
            //     )
            //         .strokeWidth(8.)
            //         .fillColor(Color.Red.ToFabColor())
            //         .strokeColor (Color.Blue.ToFabColor())
            //
            //     Polygon(
            //         [ Position(47.6458676, -122.1356007)
            //           Position(47.6458097, -122.142789)
            //           Position(47.6367593, -122.1428104)
            //           Position(47.6368027, -122.1398707)
            //           Position(47.6380172, -122.1376177)
            //           Position(47.640663, -122.1352359)
            //           Position(47.6426148, -122.1347209)
            //           Position(47.6458676, -122.1356007) ]
            //     )
            //         .strokeWidth(8.)
            //         .fillColor(Color.Yellow.ToFabColor())
            //         .strokeColor (Color.Black.ToFabColor())
            //
            //     Polygon(
            //         [
            //           Position(47.6381401, -122.1317367)
            //           Position(47.6381473, -122.1350841)
            //           Position(47.6382847, -122.1353094)
            //           Position(47.6384582, -122.1354703)
            //           Position(47.6401136, -122.1360819)
            //           Position(47.6403883, -122.1364681)
            //           Position(47.6407426, -122.1377019)
            //           Position(47.6412558, -122.1404056)
            //           Position(47.6414148, -122.1418647)
            //           Position(47.6414654, -122.1432702)
            //         ]
            //     )
            //         .strokeWidth(12.)
            //         .strokeColor (Color.Black.ToFabColor())
            // }
            )
                .ignoreSafeArea ()
        )

    let program = Program.statefulWithCmd init update view
