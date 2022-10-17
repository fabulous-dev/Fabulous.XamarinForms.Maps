namespace HelloMap

open Xamarin.Forms
open Fabulous.XamarinForms
open Fabulous.XamarinForms.Maps

open type Fabulous.XamarinForms.View

module App =
    let view () =
        Application(ContentPage("HelloMap", Map()).ignoreSafeArea ())

    let program = Program.stateless view
