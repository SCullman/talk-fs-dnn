module Hello
open System
open Elmish
open Fable.Helpers.React
open Fable.Helpers.React.Props
open Fable.Core.JsInterop
open Fable.Import

// Model
type Model =
 {
   input : string option 
   greeting : string option
   moduleId : int
 }

type Msgs =
  | ApiMsg of Api.Msgs
  | InputChanged of string
  | Greet

// Init/ update
let init moduleId = 
 {
   input = None
   greeting = None
   moduleId = moduleId
 }, Cmd.none

let update msg model =
  match msg with

  | InputChanged s -> if (String.IsNullOrWhiteSpace s)
                      then {model with input = None}, Cmd.none
                      else {model with input = Some s} , Cmd.none
  | Greet -> 
      match model.input with
      | Some name ->model,Cmd.map ApiMsg ( Api.Client(model.moduleId).GetGreeting name)
      | _ -> model, Cmd.none

  | ApiMsg msg ->
    match msg with 
    | Api.GreetingFetched g -> {model with greeting = Some g}, Cmd.none
    | Api.FetchFailed(_) -> failwith "Not Implemented"

// View
let view (model:Model) dispatch = 
  let inline onValueChange (changed:('s -> unit)) (ev:React.FormEvent)  =  changed !! ev.target?value
  
  div [] [
    div [] [
      str "Name:"
      input [ 
        Type "text"
        DefaultValue (model.input |> Option.defaultValue "") 
        OnChange (onValueChange (InputChanged>>dispatch))
        ]
      a [ 
        ClassName "btn btn-primary"
        Disabled model.input.IsNone 
        OnClick (fun _ -> dispatch Greet) 
        ] 
        [str "Greet"]
    ]
    p [] [
      str (model.greeting |> Option.defaultValue "")
      ]
  ]
  
