module Counter

open Elmish
open System
open Fable.Helpers
open Fable.Helpers.React
open Fable.Helpers.React.Props

//Model
type Model = int

type Msg =
  | Increment
  | Decrement

//State - update & init 
let init () = 0

let update msg model =
  match msg with
  | Increment -> model + 1
  | Decrement -> model - 1

// View
let view model dispatch =
  div []
    [   
      button [ OnClick (fun e -> e.preventDefault(); dispatch Decrement) ] [ str " Decrease " ]
      div [] [ str (model.ToString()) ]
      button [ OnClick (fun e -> e.preventDefault(); dispatch Increment) ] [ str " Increase " ] 
    ]

