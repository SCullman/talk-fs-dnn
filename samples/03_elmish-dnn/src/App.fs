module App

open Elmish
open Elmish.React
open Elmish.HMR
open Elmish.Debug
open Fable.Import.Browser

open Counter

let runApp elementId =
    Program.mkSimple init update view
    #if DEBUG
    |> Program.withConsoleTrace
    |> Program.withHMR
    |> Program.withDebugger
    #endif
    |> Program.withReact elementId
    |> Program.run

let runApps className =
    let elements = document.getElementsByClassName className
    let runOnElement index  =
         let elementId =  sprintf "%s-%i" className index
         elements.[index].id <- elementId
         runApp elementId
    Seq.iter (runOnElement) [0 .. int elements.length - 1]

runApps "counters"