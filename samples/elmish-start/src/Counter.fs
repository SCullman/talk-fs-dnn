module Counter

open Elmish
open Fable.Helpers.React
open Fable.Helpers.React.Props

//Model
type Model = int

type Msg =
  | Increment
  | Decrement

//State - update & init 
let init () : Model = 0

let update msg model : Model =
  match msg with
  | Increment -> model + 1
  | Decrement -> model - 1

// View
let view model dispatch =
  div []
    [ 
      button [ OnClick (fun _ -> dispatch Decrement) ] [ str "-" ]
      div [] [ str (model.ToString()) ]
      button [ OnClick (fun _ -> dispatch Increment) ] [ str "+" ] 
    ]

