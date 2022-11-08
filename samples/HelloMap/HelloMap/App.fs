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

    let mapWithRegion () =
        Map(MapSpan.FromCenterAndRadius(Position(47.640663, -122.1376177), Distance.FromMiles(250.)))

    let mapWithPins () =
        let position = Position(36.9628066, -122.0194722)
        let mapSpan = MapSpan(position, 0.01, 0.01)

        (MapWithPins(mapSpan) {
            MapPin(position)
                .address("My Address1")
                .label("I'm a marker1")
                .pinType(PinType.Place)
                .onMarkerClicked(MarkerClicked)
                .onInfoWindowClicked (MarkerClicked)

            MapPin(Position(36.9641949, -122.0177232))
                .address("My Address2")
                .label("I'm a marker1")
                .pinType(PinType.Place)
                .onMarkerClicked(MarkerClicked)
                .onInfoWindowClicked (MarkerClicked)
        })

    let mapWithPolylineElement () =
        Map(MapSpan.FromCenterAndRadius(Position(47.640663, -122.1376177), Distance.FromMiles(1.)))
            .mapElements () {
            MapPolyline(
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

    let mapWihCircleElement () =
        Map(MapSpan(Position(37.79752, -122.40183), 0.01, 0.01)).mapElements () {
            MapCircle(Position(37.79752, -122.40183), Distance(250.))
                .fillColor(Color.FromHex("#88FFC0CB").ToFabColor())
                .strokeColor(Color.FromHex("#88FF0000").ToFabColor())
                .strokeWidth (8.)
        }

    let mapWithPolygonElement () =
        Map(MapSpan.FromCenterAndRadius(Position(47.640663, -122.1376177), Distance.FromMiles(1.)))
            .mapElements () {
            MapPolygon(
                [ Position(47.6458676, -122.1356007)
                  Position(47.6458097, -122.142789)
                  Position(47.6367593, -122.1428104)
                  Position(47.6368027, -122.1398707)
                  Position(47.6380172, -122.1376177)
                  Position(47.640663, -122.1352359)
                  Position(47.6426148, -122.1347209)
                  Position(47.6458676, -122.1356007) ]
            )
                .strokeWidth(8.)
                .fillColor(Color.Red.ToFabColor())
                .strokeColor (Color.Blue.ToFabColor())

            MapPolygon(
                [ Position(47.6458676, -122.1356007)
                  Position(47.6458097, -122.142789)
                  Position(47.6367593, -122.1428104)
                  Position(47.6368027, -122.1398707)
                  Position(47.6380172, -122.1376177)
                  Position(47.640663, -122.1352359)
                  Position(47.6426148, -122.1347209)
                  Position(47.6458676, -122.1356007) ]
            )
                .strokeWidth(8.)
                .fillColor(Color.Yellow.ToFabColor())
                .strokeColor (Color.Black.ToFabColor())

            MapPolygon(
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
                .strokeWidth(12.)
                .strokeColor (Color.Black.ToFabColor())
        }

    let view (_: Model) =
        Application(
            (TabbedPage("HelloMap") {
                ContentPage("Region", mapWithRegion ())
                ContentPage("Pins", mapWithPins ())
                ContentPage("Circle", mapWihCircleElement ())
                ContentPage("Polyline", mapWithPolylineElement ())

                ContentPage(
                    "Polygons",
                    mapWithPolygonElement()
                        .hasZoomEnabled(true)
                        .hasScrollEnabled(true)
                        .mapType(MapType.Street)
                        .isShowingUser(true)
                        .isTrafficEnabled(true)
                        .onMapClicked (MapClicked)
                )
            })
                .ignoreSafeArea ()
        )

    let program = Program.statefulWithCmd init update view
