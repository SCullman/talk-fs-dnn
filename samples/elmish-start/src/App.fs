module App


open Elmish
open Elmish.React
open Elmish.HMR
open Elmish.Debug

open Counter

Program.mkSimple init update view
|> Program.withConsoleTrace
|> Program.withDebugger
|> Program.withHMR
|> Program.withReact "elmish-app"
|> Program.run